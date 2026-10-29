using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    /// <summary>
    /// Booking and rescheduling dialog for one appointment.
    /// Every rule is enforced by AppointmentRepository.ValidateBooking; this form only collects
    /// the choices, shows what the booking costs and shows the problems in a readable way.
    ///
    /// Reschedule mode (RescheduleId > 0): the appointment is loaded, only the date and the time
    /// stay usable, and saving moves the appointment (with a reason when it is inside the
    /// 3-day window).
    /// </summary>
    public partial class AppointmentForm : Form
    {
        // The legend rows of the two line dropdowns.
        private const string AnyTechnician = "Any available";
        private const string NoNurse = "None";

        // Shown under the client picker until one is chosen.
        private const string NoClientMessage = "Pick a client to see the medical alerts.";

        /// <summary>The appointment being rescheduled, or 0 to book a new one.</summary>
        public int RescheduleId { get; set; }

        /// <summary>The day the dialog opens on (the day shown on the Appointments page).</summary>
        public DateTime StartDate { get; set; }

        /// <summary>The appointment that was saved, so the caller can select it again.</summary>
        public int SavedAppointmentId { get; private set; }

        /// <summary>The day it was saved on, so the caller can switch to that day.</summary>
        public DateTime SavedAppointmentDate { get; private set; }

        // True while the dialog fills its own controls, so the change events do not recalculate
        // (or reload) five times for one edit.
        private bool _loading;

        // The start times, kept in the same order as the items of cboLineTime, so an index always
        // maps back to a real time without parsing the text.
        private readonly List<TimeSpan> _slots = new List<TimeSpan>();

        // The promotion ids behind the promo entries of cboCategoryFilter, aligned with its items
        // (0 = a normal category or "All categories").
        private readonly List<int> _promoIds = new List<int>();

        // The line grpLine is showing, so its two dropdowns know which row to write into.
        private DataRowView _selectedLine;

        // The services of the appointment being built (bound to dgvSelected).
        private DataTable _lines;

        // The Active services the desk may add, kept in a field so the category filter can reuse
        // them without asking the database again.
        private DataTable _catalog;

        // What the last recalculation worked out, so the availability check and the save agree.
        private decimal _subtotal;
        private decimal _discount;
        private decimal _required;

        public AppointmentForm()
        {
            InitializeComponent();
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            // The designer only says "N2"; Money is the one formatter of the app.
            Money.ApplyTo(colCatPrice);
            Money.ApplyTo(colSelPrice);

            // A letter typed into Qty would otherwise pop up the raw .NET error dialog of the grid.
            // It is wired here because it keeps a bad entry quiet instead of showing two dialogs.
            dgvSelected.DataError += dgvSelected_DataError;

            // These three are wired in code for the same reason the DataError above is: the grid
            // and the two line dropdowns only exist to feed the "Selected service" block.
            dgvSelected.SelectionChanged += dgvSelected_SelectionChanged;
            cboLineTime.SelectedIndexChanged += cboLineTime_SelectedIndexChanged;
            cboLineTech.SelectedIndexChanged += cboLineTech_SelectedIndexChanged;
            cboLineNurse.SelectedIndexChanged += cboLineNurse_SelectedIndexChanged;

            _loading = true;
            try
            {
                // The date may not be in the past. Today only works while the time is still ahead,
                // which the availability check reports line by line.
                dtpDate.MinDate = DateTime.Today;
                dtpDate.Value = DateTime.Today;

                BuildLinesTable();
                FillSources();
                FillCategories();
                FillCatalog();
                FillClients();
            }
            finally
            {
                _loading = false;
            }

            if (StartDate.Date > DateTime.MinValue.Date && StartDate.Date >= DateTime.Today)
            {
                dtpDate.Value = StartDate.Date;
            }

            if (RescheduleId > 0)
            {
                SetupRescheduleMode();
            }
            else
            {
                lblClientInfo.Text = NoClientMessage;
            }

            UpdateLinePanel();
            Recalculate();
        }

        // ---------------------------------------------------------------- filling the screen

        private void BuildLinesTable()
        {
            _lines = new DataTable();
            _lines.Columns.Add(AppointmentRepository.ColServiceId, typeof(int));
            _lines.Columns.Add(AppointmentRepository.ColServiceName, typeof(string));
            _lines.Columns.Add(AppointmentRepository.ColQuantity, typeof(int));
            _lines.Columns.Add(AppointmentRepository.ColPrice, typeof(decimal));
            _lines.Columns.Add(AppointmentRepository.ColDuration, typeof(int));
            _lines.Columns.Add("package_label", typeof(string));

            // Extra columns the booking needs but the grid does not show.
            _lines.Columns.Add("default_total_sessions", typeof(int));
            _lines.Columns.Add(AppointmentRepository.ColPackageId, typeof(int));
            _lines.Columns.Add(AppointmentRepository.ColSessionNumber, typeof(int));
            _lines.Columns.Add(AppointmentRepository.ColTotalSessions, typeof(int));

            // The technician may be changed by the user, so it is the last column (the grid shows
            // technician_name as plain text; technician_id is written by cboLineTech).
            _lines.Columns.Add(AppointmentRepository.ColTechnicianId, typeof(int));
            _lines.Columns.Add("technician_name", typeof(string));

            // The columns the booking keeps per line but the grid does not show yet.
            _lines.Columns.Add(AppointmentRepository.ColStartTime, typeof(TimeSpan));
            _lines.Columns.Add("start_text", typeof(string));
            _lines.Columns.Add("end_text", typeof(string));
            _lines.Columns.Add("nurse_name", typeof(string));
            _lines.Columns.Add(AppointmentRepository.ColAssessedBy, typeof(int));
            _lines.Columns.Add("requires_assessment", typeof(bool));
            _lines.Columns.Add("promo_id", typeof(int));
            _lines.Columns.Add("category_name", typeof(string));
            _lines.Columns.Add("staff_busy_minutes", typeof(int));

            _lines.DefaultView.Sort = AppointmentRepository.ColServiceName + " ASC";
            _lines.Columns[AppointmentRepository.ColTechnicianId].DefaultValue = 0;
            _lines.Columns[AppointmentRepository.ColStartTime].DefaultValue = TimeSpan.Zero;
            _lines.Columns[AppointmentRepository.ColAssessedBy].DefaultValue = 0;
            _lines.Columns["promo_id"].DefaultValue = 0;
            _lines.Columns["staff_busy_minutes"].DefaultValue = 0;

            dgvSelected.DataSource = _lines;
        }

        private void FillSources()
        {
            if (cboSource.Items.Count > 0) cboSource.SelectedIndex = 0;   // "Walk-in"
        }

        private void FillCategories()
        {
            _promoIds.Clear();
            cboCategoryFilter.Items.Clear();

            cboCategoryFilter.Items.Add("All categories");
            _promoIds.Add(0);

            try
            {
                foreach (DataRow row in ServiceRepository.GetCategories().Rows)
                {
                    cboCategoryFilter.Items.Add(Db.Get<string>(row["category_name"]));
                    _promoIds.Add(0);
                }

                // A promotion is one more entry of the same filter, so the desk can book a promo
                // the same way it picks a category ("[PROMO] <name>").
                foreach (DataRow row in PromotionRepository.GetActiveValid().Rows)
                {
                    cboCategoryFilter.Items.Add("[PROMO] " + Db.Get<string>(row["promo_name"]));
                    _promoIds.Add(Db.Get<int>(row["promo_id"]));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            cboCategoryFilter.SelectedIndex = 0;
        }

        /// <summary>The Active services the desk may add, kept in a field so the filter can reuse it.</summary>
        private void FillCatalog()
        {
            try
            {
                _catalog = ServiceRepository.GetActiveForBooking();
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                _catalog = null;
                return;
            }

            FilterCatalog();
        }

        /// <summary>Rebinds the catalog with the chosen category, without asking the database again.</summary>
        private void FilterCatalog()
        {
            if (_catalog == null)
            {
                dgvCatalog.DataSource = null;
                return;
            }

            DataView view = new DataView(_catalog);
            int promoId = SelectedPromoCategoryId();

            // A promo entry shows the services the promotion is made of, so the desk sees what it
            // is about to add.
            if (promoId > 0)
            {
                dgvCatalog.DataSource = PromoServicesView(promoId);
                return;
            }

            string category = cboCategoryFilter.SelectedItem == null
                ? "All categories"
                : cboCategoryFilter.SelectedItem.ToString();

            if (!string.Equals(category, "All categories", StringComparison.OrdinalIgnoreCase))
            {
                // Category names come from the database, so a name with an apostrophe must not
                // break the filter.
                view.RowFilter = "category_name = '" + category.Replace("'", "''") + "'";
            }

            dgvCatalog.DataSource = view;
        }

        /// <summary>
        /// The ids of the services of the promotion the filter points at, or an empty list when the
        /// selection is not a promo entry (or the promo is not linked to a single service).
        /// </summary>
        private List<int> SelectedPromoServices()
        {
            int promoId = SelectedPromoCategoryId();
            if (promoId <= 0) return new List<int>();

            try
            {
                return PromotionRepository.GetServiceIds(promoId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return new List<int>();
            }
        }

        /// <summary>The catalog rows of a promotion: only the services the promo is made of.</summary>
        private DataView PromoServicesView(int promoId)
        {
            List<int> ids = SelectedPromoServices();
            DataView view = new DataView(_catalog);

            // A promotion with no linked service has nothing to add yet, so the catalog is empty
            // instead of offering services that are not part of it.
            if (ids.Count == 0)
            {
                view.RowFilter = "service_id = 0";
                return view;
            }

            List<string> parts = new List<string>();
            foreach (int id in ids) parts.Add("service_id = " + id.ToString());

            view.RowFilter = string.Join(" OR ", parts.ToArray());
            return view;
        }

        private void FillClients()
        {
            try
            {
                DataTable clients = ClientRepository.GetForCombo();

                cboClient.DisplayMember = "display_name";
                cboClient.ValueMember = "client_id";
                cboClient.DataSource = clients;
                cboClient.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>
        /// Reschedule mode: the appointment is loaded and everything except the date and the time
        /// is locked, because a reschedule never changes the services or the money.
        /// </summary>
        private void SetupRescheduleMode()
        {
            DataRow row;
            DataTable lines;

            try
            {
                row = AppointmentRepository.GetById(RescheduleId);
                lines = AppointmentRepository.GetLinesForBooking(RescheduleId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                Messages.Warn("This appointment no longer exists.");
                return;
            }

            _loading = true;
            try
            {
                Text = "Reschedule Appointment";
                lblTitle.Text = "Reschedule Appointment";
                btnSave.Text = "Save Changes";

                int clientId = Db.Get<int>(row["client_id"]);
                SelectComboValue(cboClient, clientId);
                ShowClientInfo(clientId);

                // The dialog opens on the day the appointment is currently on, so the user sees
                // what is being moved and only has to change it.
                DateTime appointmentDate = Db.Get<DateTime>(row["appointment_date"]).Date;
                if (appointmentDate >= dtpDate.MinDate && appointmentDate <= dtpDate.MaxDate)
                {
                    dtpDate.Value = appointmentDate;
                }

                string source = Db.Get<string>(row["booking_source"]);
                int sourceIndex = cboSource.Items.IndexOf(source);
                if (sourceIndex >= 0) cboSource.SelectedIndex = sourceIndex;

                TimeSpan start = ReadTime(row["start_time"]);
                int slotIndex = _slots.IndexOf(start);
                if (slotIndex >= 0) cboLineTime.SelectedIndex = slotIndex;

                // The promotion the bill was made with is passed on, so the totals below match the
                // original bill instead of pretending the booking has no promotion.
                int savedPromoId = SavedPromoId(RescheduleId);
                List<int> savedPromoServices = savedPromoId > 0
                    ? PromotionRepository.GetServiceIds(savedPromoId)
                    : new List<int>();

                // The existing services are shown as they are (the user cannot change them).
                foreach (DataRow line in lines.Rows)
                {
                    int technicianId = Db.Get<int>(line[AppointmentRepository.ColTechnicianId]);
                    int serviceId = Db.Get<int>(line[AppointmentRepository.ColServiceId]);
                    int sessionNumber = Db.Get<int>(line[AppointmentRepository.ColSessionNumber]);
                    int totalSessions = Db.Get<int>(line[AppointmentRepository.ColTotalSessions]);
                    int assessedBy = Db.Get<int>(line[AppointmentRepository.ColAssessedBy]);

                    // requires_assessment and the category name are not part of a stored line, so
                    // they are read from the catalog the dialog already loaded.
                    DataRow catalogRow = CatalogRow(serviceId);
                    bool requiresAssessment = catalogRow != null
                        && Db.Get<bool>(catalogRow["requires_assessment"]);
                    string categoryName = catalogRow == null
                        ? string.Empty
                        : Db.Get<string>(catalogRow["category_name"]);

                    int promoId = savedPromoId > 0 && savedPromoServices.Contains(serviceId)
                        ? savedPromoId
                        : 0;

                    _lines.Rows.Add(
                        serviceId,
                        Db.Get<string>(line[AppointmentRepository.ColServiceName]),
                        Db.Get<int>(line[AppointmentRepository.ColQuantity], 1),
                        Db.Get<decimal>(line[AppointmentRepository.ColPrice]),
                        Db.Get<int>(line[AppointmentRepository.ColDuration]),
                        AppointmentRepository.SessionText(sessionNumber, totalSessions),
                        Db.Get<int>(line["default_total_sessions"], 1),
                        Db.Get<int>(line[AppointmentRepository.ColPackageId]),
                        sessionNumber,
                        totalSessions,
                        technicianId,
                        StaffName(technicianId),
                        Db.Get<TimeSpan>(line[AppointmentRepository.ColStartTime], TimeSpan.Zero),
                        string.Empty,
                        string.Empty,
                        StaffName(assessedBy),
                        assessedBy,
                        requiresAssessment,
                        promoId,
                        categoryName,
                        Db.Get<int>(line["staff_busy_minutes"], 0));
                }

                txtNotes.Text = Db.Get<string>(row["notes"]);

                // Only the date and the time may change.
                cboClient.Enabled = false;
                btnNewClient.Enabled = false;
                cboSource.Enabled = false;
                cboCategoryFilter.Enabled = false;
                dgvCatalog.Enabled = false;
                btnAddService.Enabled = false;
                btnRemoveService.Enabled = false;
                dgvSelected.ReadOnly = true;
                txtNotes.ReadOnly = true;
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>The promotion the appointment being moved was billed with, or 0.</summary>
        private static int SavedPromoId(int appointmentId)
        {
            DataRow billing;
            try
            {
                billing = BillingRepository.GetByAppointment(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return 0;
            }

            return billing == null ? 0 : Db.Get<int>(billing["promo_id"]);
        }

        /// <summary>The catalog row of one service, or null when the service is not Active any more.</summary>
        private DataRow CatalogRow(int serviceId)
        {
            if (_catalog == null) return null;

            foreach (DataRow row in _catalog.Rows)
            {
                if (Db.Get<int>(row["service_id"]) == serviceId) return row;
            }

            return null;
        }

        /// <summary>The full name of an account id, or an empty string when nobody is set.</summary>
        private static string StaffName(int userId)
        {
            if (userId <= 0) return string.Empty;

            DataRow user;
            try
            {
                user = UserRepository.GetById(userId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return string.Empty;
            }

            return user == null ? string.Empty : Db.Get<string>(user["full_name"]);
        }

        /// <summary>Selects the row of a bound combo box whose value column equals value.</summary>
        private static void SelectComboValue(ComboBox combo, int value)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                DataRowView view = combo.Items[i] as DataRowView;
                if (view == null) continue;

                if (Db.Get<int>(view["client_id"]) == value)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        // ---------------------------------------------------------------- events

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            using (ClientEditForm form = new ClientEditForm())
            {
                form.EditId = 0;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The new client is the one being booked, so the combo is refilled and selects it.
                FillClients();
                SelectComboValue(cboClient, form.SavedClientId);
                ShowClientInfo(form.SavedClientId);
            }
        }

        private void cboClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            int clientId = SelectedClientId();
            ShowClientInfo(clientId);

            // The package label and the price of a package service depend on the client,
            // so the lines are rebuilt from the catalog for the new client.
            RefreshLinePackages();
            RevalidateLines();
            Recalculate();
        }

        private void ShowClientInfo(int clientId)
        {
            if (clientId <= 0)
            {
                lblClientInfo.Text = NoClientMessage;
                lblClientInfo.ForeColor = Theme.TextDark;
                return;
            }

            DataRow row;
            try
            {
                row = ClientRepository.GetById(clientId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                lblClientInfo.Text = "This client no longer exists.";
                lblClientInfo.ForeColor = Theme.TextDark;
                return;
            }

            string allergies = Db.Get<string>(row["allergies"]);
            string medical = Db.Get<string>(row["medical_history"]);

            List<string> alerts = new List<string>();
            if (!string.IsNullOrWhiteSpace(allergies)) alerts.Add("allergies: " + allergies.Trim());
            if (!string.IsNullOrWhiteSpace(medical)) alerts.Add("medical history: " + medical.Trim());

            // The consent form is no longer part of the booking rules, so only the medical alerts
            // are shown: in the normal text color, because this dialog has no red text at all.
            if (alerts.Count == 0)
            {
                lblClientInfo.Text = "No medical alerts.";
                lblClientInfo.ForeColor = Theme.TextMuted;
                return;
            }

            lblClientInfo.Text = "Alerts: " + string.Join("; ", alerts.ToArray());
            lblClientInfo.ForeColor = Theme.TextDark;
        }

        private void cboCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            FilterCatalog();
        }

        private void btnAddService_Click(object sender, EventArgs e)
        {
            AddSelectedCatalogService();
        }

        private void dgvCatalog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            AddSelectedCatalogService();
        }

        /// <summary>
        /// Adds the service selected in the catalog. A service that is already in the list only
        /// gets one more session when it is not a package, because a package has ONE price and a
        /// fixed number of sessions.
        /// </summary>
        private void AddSelectedCatalogService()
        {
            // A promo entry adds the whole promotion, not the row under the pointer.
            int promoId = SelectedPromoCategoryId();
            if (promoId > 0)
            {
                AddPromoLines(promoId);
                return;
            }

            if (dgvCatalog.CurrentRow == null) return;

            int serviceId = Db.Get<int>(dgvCatalog.CurrentRow.Cells["colCatServiceId"].Value);
            if (serviceId <= 0) return;

            AddServiceLine(serviceId, 0);
        }

        /// <summary>
        /// Adds every service of a promotion as its own line (quantity 1, regular price, the promo
        /// id set). Only ONE promotion per booking: a second one is refused until the lines of the
        /// current promotion are removed.
        /// </summary>
        private void AddPromoLines(int promoId)
        {
            if (PromoIdInLines() > 0)
            {
                Messages.Info("Only one promotion per appointment. Remove the current promo lines first.");
                return;
            }

            List<int> serviceIds = SelectedPromoServices();
            if (serviceIds.Count == 0)
            {
                Messages.Info("This promotion is not linked to a service, so nothing can be added.");
                return;
            }

            // The lines are added quietly, so the totals are worked out once at the end.
            _loading = true;
            try
            {
                foreach (int serviceId in serviceIds)
                {
                    AddServiceLine(serviceId, promoId);
                }
            }
            finally
            {
                _loading = false;
            }

            Recalculate();
        }

        /// <summary>
        /// Adds one service line, or one more session of a line that is already there. promoId > 0
        /// marks the line as part of a promotion (its name reads "[PROMO] <service name>").
        /// </summary>
        private void AddServiceLine(int serviceId, int promoId)
        {
            int clientId = SelectedClientId();

            DataRow service;
            try
            {
                service = ServiceRepository.GetById(serviceId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (service == null) return;

            string serviceName = Db.Get<string>(service["service_name"]);
            int duration = Db.Get<int>(service["duration_minutes"]);
            decimal price = Db.Get<decimal>(service["price"]);
            int defaultSessions = Db.Get<int>(service["default_total_sessions"], 1);

            DataRowView existing = FindLine(serviceId);
            if (existing != null)
            {
                // A line that is already there simply joins the promotion (its quantity stays 1).
                if (promoId > 0)
                {
                    existing["promo_id"] = promoId;
                    existing["quantity"] = 1;
                    existing["service_name"] = "[PROMO] " + serviceName;
                    return;
                }

                if (defaultSessions > 1)
                {
                    Messages.Info("'" + serviceName + "' is a package and is already in this appointment. " +
                        "Remove it first if you want to change it.");
                    return;
                }

                // A normal service simply gets one more session in the same line.
                existing["quantity"] = Db.Get<int>(existing["quantity"], 1) + 1;
                Recalculate();
                return;
            }

            int packageId = 0;
            int sessionNumber = 0;
            int totalSessions = 0;
            bool isNewPackage = false;

            // A promo line keeps the regular price of its service and is never split into
            // sessions, because the promotion is sold as one package for one promotional price.
            if (promoId <= 0 && clientId > 0 && defaultSessions > 1)
            {
                try
                {
                    if (AppointmentRepository.TryGetPackagePlan(clientId, serviceId,
                        out packageId, out sessionNumber, out totalSessions, out isNewPackage))
                    {
                        // The whole package price sits on the first session; later sessions are free.
                        if (!isNewPackage || sessionNumber > 1) price = 0m;
                    }
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }
            }

            bool requiresAssessment = Db.Get<bool>(service["requires_assessment"]);
            string categoryName = CategoryName(serviceId);
            int staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);

            // A promo line is quantity 1 and marked, so the whole booking can tell which services
            // belong to the one promotion it may hold.
            string shownName = promoId > 0 ? "[PROMO] " + serviceName : serviceName;

            _lines.Rows.Add(serviceId, shownName, 1, price, duration,
                AppointmentRepository.SessionText(sessionNumber, totalSessions),
                defaultSessions, packageId, sessionNumber, totalSessions, 0,
                string.Empty, TimeSpan.Zero, string.Empty, string.Empty, string.Empty, 0,
                requiresAssessment, promoId, categoryName, staffBusyMinutes);

            // Every line is placed on the day as soon as it is added, so the booking always reads
            // as a real plan (the first free time after the lines that are already placed).
            PlaceLine(FindLine(serviceId));

            Recalculate();

            // The new line is the one being worked on, so it is selected (Qty and Technician).
            GridHelper.SelectRowById(dgvSelected, "colSelServiceId", serviceId);
        }

        private void btnRemoveService_Click(object sender, EventArgs e)
        {
            int serviceId = GridHelper.GetSelectedId(dgvSelected, "colSelServiceId");
            if (serviceId <= 0) return;

            // The grid shows a SORTED view of the lines, so the row is looked up by service id
            // instead of trusting the index the grid happens to show.
            DataRowView line = FindLine(serviceId);
            if (line == null) return;

            int promoId = Db.Get<int>(line["promo_id"]);

            // A promo line belongs to its promotion, so removing it removes the whole promo.
            if (promoId > 0)
            {
                RemovePromoLines(promoId);
                return;
            }

            // The block must not keep pointing at a row that is about to disappear.
            _selectedLine = null;
            _lines.Rows.Remove(line.Row);

            RevalidateLines();
            UpdateLinePanel();
            Recalculate();
        }

        /// <summary>Removes every line that belongs to one promotion.</summary>
        private void RemovePromoLines(int promoId)
        {
            // The rows are collected first, because a DataTable cannot be changed while it is
            // being walked over.
            List<DataRow> doomed = new List<DataRow>();
            foreach (DataRow row in _lines.Rows)
            {
                if (Db.Get<int>(row["promo_id"]) == promoId) doomed.Add(row);
            }

            _selectedLine = null;
            foreach (DataRow row in doomed) _lines.Rows.Remove(row);

            RevalidateLines();
            UpdateLinePanel();
            Recalculate();
        }

        // ---------------------------------------------------------------- the selected line

        /// <summary>
        /// Fills cboLineTime with the start times at which the SELECTED line really fits: the past
        /// times and the busy times are simply not in the list. _slots keeps the real values in the
        /// same order as the items, so an index always maps back to a time. The line's own start is
        /// selected again when it is still free; otherwise nothing is selected.
        /// </summary>
        private void RefreshLineTimes()
        {
            bool wasLoading = _loading;
            _loading = true;
            try
            {
                _slots.Clear();
                cboLineTime.Items.Clear();

                if (_selectedLine == null)
                {
                    cboLineTime.Enabled = false;
                    return;
                }

                int serviceId = Db.Get<int>(_selectedLine[AppointmentRepository.ColServiceId]);
                int quantity = Db.Get<int>(_selectedLine[AppointmentRepository.ColQuantity], 1);
                int technicianId = Db.Get<int>(_selectedLine[AppointmentRepository.ColTechnicianId]);
                TimeSpan current = AppointmentRepository.ReadTime(
                    _selectedLine[AppointmentRepository.ColStartTime]);

                List<TimeSpan> free;
                try
                {
                    free = AvailabilityRepository.GetFreeStarts(null, null, dtpDate.Value.Date,
                        serviceId, quantity, technicianId, SelectedClientId(), RescheduleId,
                        OtherLines(_selectedLine));
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    cboLineTime.Enabled = false;
                    return;
                }

                int selectedIndex = -1;
                for (int i = 0; i < free.Count; i++)
                {
                    _slots.Add(free[i]);
                    cboLineTime.Items.Add(Clock(free[i]));

                    if (free[i] == current) selectedIndex = i;
                }

                if (selectedIndex >= 0) cboLineTime.SelectedIndex = selectedIndex;

                if (free.Count == 0)
                {
                    cboLineTime.Enabled = false;
                    lblLineNote.Text = "No free time for this service on this date. Choose another date.";
                    return;
                }

                // A reschedule only moves the booking, so its time list stays usable even though the
                // services themselves are locked.
                cboLineTime.Enabled = RescheduleId > 0 || !dgvSelected.ReadOnly;
            }
            finally
            {
                _loading = wasLoading;
            }
        }

        /// <summary>
        /// Gives a line that has no start yet its default time: the first free start that is on or
        /// after the end of the latest PLACED line, otherwise the first free start of the day.
        /// </summary>
        private void PlaceLine(DataRowView line)
        {
            if (line == null) return;
            if (AppointmentRepository.ReadTime(line[AppointmentRepository.ColStartTime]) != TimeSpan.Zero) return;

            int serviceId = Db.Get<int>(line[AppointmentRepository.ColServiceId]);
            int quantity = Db.Get<int>(line[AppointmentRepository.ColQuantity], 1);
            int technicianId = Db.Get<int>(line[AppointmentRepository.ColTechnicianId]);

            List<TimeSpan> free;
            try
            {
                free = AvailabilityRepository.GetFreeStarts(null, null, dtpDate.Value.Date, serviceId,
                    quantity, technicianId, SelectedClientId(), RescheduleId, OtherLines(line));
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (free.Count == 0) return;

            // The line is put after the lines that are already on the day, so the booking reads
            // like a plan instead of stacking everything on one time.
            TimeSpan after = LatestPlacedEnd(line);
            TimeSpan chosen = TimeSpan.Zero;

            foreach (TimeSpan slot in free)
            {
                if (slot >= after)
                {
                    chosen = slot;
                    break;
                }
            }

            if (chosen == TimeSpan.Zero) chosen = free[0];

            WriteLineStart(line, chosen);
        }

        /// <summary>
        /// Re-checks every placed line against its own date and time. A line that no longer fits
        /// (the date or the client changed, or another line took its slot) loses its time and is
        /// placed again. A reschedule does nothing here: it moves the whole booking at once.
        /// </summary>
        private void RevalidateLines()
        {
            if (RescheduleId > 0) return;

            // The rows are collected first, because placing a line writes into the table while the
            // collection is being walked over.
            List<DataRowView> views = new List<DataRowView>();
            foreach (DataRowView view in _lines.DefaultView) views.Add(view);

            bool wasLoading = _loading;
            _loading = true;
            try
            {
                foreach (DataRowView view in views)
                {
                    TimeSpan start = AppointmentRepository.ReadTime(view[AppointmentRepository.ColStartTime]);
                    if (start == TimeSpan.Zero) continue;

                    string answer;
                    try
                    {
                        answer = AvailabilityRepository.WhyNotPossible(null, null, dtpDate.Value.Date,
                            Db.Get<int>(view[AppointmentRepository.ColServiceId]),
                            Db.Get<int>(view[AppointmentRepository.ColQuantity], 1), start,
                            Db.Get<int>(view[AppointmentRepository.ColTechnicianId]),
                            SelectedClientId(), RescheduleId, OtherLines(view));
                    }
                    catch (Exception ex)
                    {
                        Messages.DbError(ex);
                        return;
                    }

                    if (string.IsNullOrEmpty(answer)) continue;

                    view[AppointmentRepository.ColStartTime] = TimeSpan.Zero;
                    view["start_text"] = string.Empty;
                    view["end_text"] = string.Empty;

                    PlaceLine(view);
                }
            }
            finally
            {
                _loading = wasLoading;
            }
        }

        /// <summary>Writes the time of one line: its start, and the texts the grid shows.</summary>
        private static void WriteLineStart(DataRowView line, TimeSpan start)
        {
            int duration = Db.Get<int>(line[AppointmentRepository.ColDuration]);
            int quantity = Db.Get<int>(line[AppointmentRepository.ColQuantity], 1);

            line[AppointmentRepository.ColStartTime] = start;
            line["start_text"] = Clock(start);
            line["end_text"] = Clock(AvailabilityRepository.LineEnd(start, duration, quantity));
        }

        /// <summary>The end of the latest line that already has a time, or 0 when none has one.</summary>
        private TimeSpan LatestPlacedEnd(DataRowView except)
        {
            TimeSpan latest = TimeSpan.Zero;

            foreach (DataRow row in _lines.Rows)
            {
                if (except != null && ReferenceEquals(row, except.Row)) continue;

                TimeSpan start = AppointmentRepository.ReadTime(row[AppointmentRepository.ColStartTime]);
                if (start == TimeSpan.Zero) continue;

                TimeSpan end = AvailabilityRepository.LineEnd(start,
                    Db.Get<int>(row[AppointmentRepository.ColDuration]),
                    Db.Get<int>(row[AppointmentRepository.ColQuantity], 1));

                if (end > latest) latest = end;
            }

            return latest;
        }

        /// <summary>
        /// The OTHER lines of the booking in the shape AvailabilityRepository wants them, so a line
        /// never counts itself as busy.
        /// </summary>
        private DataTable OtherLines(DataRowView line)
        {
            DataTable table = new DataTable();
            table.Columns.Add(AppointmentRepository.ColServiceId, typeof(int));
            table.Columns.Add(AppointmentRepository.ColQuantity, typeof(int));
            table.Columns.Add(AppointmentRepository.ColDuration, typeof(int));
            table.Columns.Add(AppointmentRepository.ColStartTime, typeof(TimeSpan));

            foreach (DataRowView view in _lines.DefaultView)
            {
                if (line != null && ReferenceEquals(view.Row, line.Row)) continue;

                table.Rows.Add(Db.Get<int>(view[AppointmentRepository.ColServiceId]),
                    Db.Get<int>(view[AppointmentRepository.ColQuantity], 1),
                    Db.Get<int>(view[AppointmentRepository.ColDuration]),
                    AppointmentRepository.ReadTime(view[AppointmentRepository.ColStartTime]));
            }

            return table;
        }

        /// <summary>
        /// Fills the "Selected service" block from the row selected in dgvSelected: its name, the
        /// technicians of its own category and (only for a service that needs one) the nurses.
        /// Called whenever the selection changes or the lines are rebuilt.
        /// </summary>
        private void UpdateLinePanel()
        {
            _selectedLine = SelectedLine();

            if (_selectedLine == null)
            {
                lblLineService.Text = "No service selected";
                lblLineNote.Text = "Select a service above to set its technician and, when the " +
                    "service needs an assessment, the nurse who assessed the client.";
                lblSessionPlan.Text = string.Empty;
                cboLineTech.DataSource = null;
                cboLineTech.Enabled = false;
                cboLineNurse.DataSource = null;
                cboLineNurse.Enabled = false;
                RefreshLineTimes();
                return;
            }

            bool wasLoading = _loading;
            _loading = true;
            try
            {
                string categoryName = Db.Get<string>(_selectedLine["category_name"]);
                bool requiresAssessment = Db.Get<bool>(_selectedLine["requires_assessment"]);
                int staffBusyMinutes = Db.Get<int>(_selectedLine["staff_busy_minutes"]);
                int technicianId = Db.Get<int>(_selectedLine[AppointmentRepository.ColTechnicianId]);
                int nurseId = Db.Get<int>(_selectedLine[AppointmentRepository.ColAssessedBy]);

                lblLineService.Text = Db.Get<string>(_selectedLine[AppointmentRepository.ColServiceName]);
                lblSessionPlan.Text = PackagePlanText(_selectedLine);

                FillLineTechnicians(categoryName, technicianId);
                FillLineNurses(requiresAssessment, nurseId);

                lblLineNote.Text = LineNote(requiresAssessment, staffBusyMinutes,
                    Db.Get<int>(_selectedLine[AppointmentRepository.ColDuration]));
            }
            finally
            {
                _loading = wasLoading;
            }

            // The time list depends on the line (its service, its technician and the other lines),
            // so it is always refilled after the two dropdowns above.
            RefreshLineTimes();
        }

        /// <summary>The technicians of one line: "Any available" plus the staff of its category.</summary>
        private void FillLineTechnicians(string categoryName, int technicianId)
        {
            DataTable staff = new DataTable();
            staff.Columns.Add("user_id", typeof(int));
            staff.Columns.Add("full_name", typeof(string));
            staff.Rows.Add(0, AnyTechnician);

            try
            {
                foreach (DataRow row in UserRepository.GetActiveStaffFor(categoryName).Rows)
                {
                    staff.Rows.Add(Db.Get<int>(row["user_id"]), Db.Get<string>(row["full_name"]));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            // A technician who was deactivated after the appointment was booked is not in the list
            // any more, but the line still has to show who did the service.
            if (technicianId > 0 && !HasUserId(staff, technicianId))
            {
                staff.Rows.Add(technicianId, StaffName(technicianId) + " (inactive)");
            }

            cboLineTech.DataSource = staff;
            cboLineTech.DisplayMember = "full_name";
            cboLineTech.ValueMember = "user_id";
            cboLineTech.Enabled = !dgvSelected.ReadOnly;
            SelectUserId(cboLineTech, technicianId);
        }

        /// <summary>
        /// The nurses of one line. The dropdown is only usable for a service that requires an
        /// assessment; otherwise it is disabled and reads "None".
        /// </summary>
        private void FillLineNurses(bool requiresAssessment, int nurseId)
        {
            DataTable nurses = new DataTable();
            nurses.Columns.Add("user_id", typeof(int));
            nurses.Columns.Add("full_name", typeof(string));
            nurses.Rows.Add(0, NoNurse);

            try
            {
                foreach (DataRow row in UserRepository.GetActiveNurses().Rows)
                {
                    nurses.Rows.Add(Db.Get<int>(row["user_id"]), Db.Get<string>(row["full_name"]));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            if (nurseId > 0 && !HasUserId(nurses, nurseId))
            {
                nurses.Rows.Add(nurseId, StaffName(nurseId) + " (inactive)");
            }

            cboLineNurse.DataSource = nurses;
            cboLineNurse.DisplayMember = "full_name";
            cboLineNurse.ValueMember = "user_id";
            cboLineNurse.Enabled = requiresAssessment && !dgvSelected.ReadOnly;
            SelectUserId(cboLineNurse, requiresAssessment ? nurseId : 0);
        }

        /// <summary>
        /// The note under the two dropdowns: the assessment warning of the service and, when the
        /// service has staff_busy_minutes, how long the chair is kept against how long the staff
        /// member is needed.
        /// </summary>
        private static string LineNote(bool requiresAssessment, int staffBusyMinutes, int durationMinutes)
        {
            List<string> notes = new List<string>();

            if (requiresAssessment) notes.Add("Needs a nurse assessment first.");

            if (staffBusyMinutes > 0)
            {
                notes.Add("The chair is busy " + durationMinutes + " min, the staff only " +
                    staffBusyMinutes + " min.");
            }

            return string.Join(" ", notes.ToArray());
        }

        /// <summary>The "Session 2 of 4" line of the selected service.</summary>
        private static string PackagePlanText(DataRowView line)
        {
            int sessionNumber = Db.Get<int>(line[AppointmentRepository.ColSessionNumber]);
            int totalSessions = Db.Get<int>(line[AppointmentRepository.ColTotalSessions]);

            if (totalSessions <= 1) return string.Empty;
            return AppointmentRepository.SessionText(sessionNumber, totalSessions);
        }

        /// <summary>The line of the row selected in dgvSelected, or null when nothing is selected.</summary>
        private DataRowView SelectedLine()
        {
            int serviceId = GridHelper.GetSelectedId(dgvSelected, "colSelServiceId");
            return serviceId <= 0 ? null : FindLine(serviceId);
        }

        private static bool HasUserId(DataTable table, int userId)
        {
            foreach (DataRow row in table.Rows)
            {
                if (Db.Get<int>(row["user_id"]) == userId) return true;
            }

            return false;
        }

        /// <summary>Selects the row of a bound combo whose user_id column equals userId.</summary>
        private static void SelectUserId(ComboBox combo, int userId)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                DataRowView view = combo.Items[i] as DataRowView;
                if (view == null) continue;

                if (Db.Get<int>(view["user_id"]) == userId)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        /// <summary>The line of a service in dgvSelected, or null when it is not there yet.</summary>
        private DataRowView FindLine(int serviceId)
        {
            foreach (DataRowView view in _lines.DefaultView)
            {
                if (Db.Get<int>(view[AppointmentRepository.ColServiceId]) == serviceId) return view;
            }

            return null;
        }

        /// <summary>
        /// Re-reads the package label and the package price of every line for the selected client.
        /// WHY: the same service costs the whole package price for a new client and 0.00 for the
        /// session of a package the client already has.
        /// </summary>
        private void RefreshLinePackages()
        {
            int clientId = SelectedClientId();

            // Changing a bound DataTable raises CellValueChanged on every row, and each of those
            // would recalculate the whole booking, so the recalculation is switched off around the
            // edit and run once at the end by the caller.
            _loading = true;
            try
            {
                UpdatePackageLabels(clientId);
            }
            finally
            {
                _loading = false;
            }
        }

        private void UpdatePackageLabels(int clientId)
        {
            foreach (DataRow line in _lines.Rows)
            {
                int serviceId = Db.Get<int>(line[AppointmentRepository.ColServiceId]);
                int defaultSessions = Db.Get<int>(line["default_total_sessions"], 1);

                if (clientId <= 0)
                {
                    line["package_label"] = AppointmentRepository.SessionText(0, 0);
                    continue;
                }

                if (defaultSessions <= 1)
                {
                    line["package_label"] = AppointmentRepository.SessionText(0, 0);
                    continue;
                }

                try
                {
                    int packageId;
                    int sessionNumber;
                    int totalSessions;
                    bool isNewPackage;

                    if (!AppointmentRepository.TryGetPackagePlan(clientId, serviceId,
                        out packageId, out sessionNumber, out totalSessions, out isNewPackage)) continue;

                    line["package_label"] = AppointmentRepository.SessionText(sessionNumber, totalSessions);

                    line[AppointmentRepository.ColPackageId] = packageId;
                    line[AppointmentRepository.ColSessionNumber] = sessionNumber;
                    line[AppointmentRepository.ColTotalSessions] = totalSessions;
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            RevalidateLines();
            Recalculate();
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            Recalculate();
        }

        /// <summary>
        /// Writes the chosen technician of the selected line into that row. The technician is a
        /// text column now, so the name is written next to the id.
        /// </summary>
        private void cboLineTech_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading || _selectedLine == null) return;

            DataRowView picked = cboLineTech.SelectedItem as DataRowView;
            int technicianId = picked == null ? 0 : Db.Get<int>(picked["user_id"]);
            string technicianName = picked == null ? string.Empty : Db.Get<string>(picked["full_name"]);

            _loading = true;
            try
            {
                _selectedLine[AppointmentRepository.ColTechnicianId] = technicianId;
                _selectedLine["technician_name"] = technicianId > 0 ? technicianName : string.Empty;
            }
            finally
            {
                _loading = false;
            }

            // Another technician may free or block the other lines, and this line may now fit at a
            // different time, so the whole plan is re-checked and the time list is refilled.
            RevalidateLines();
            UpdateLinePanel();
            Recalculate();
        }

        /// <summary>Writes the assessing nurse of the selected line into that row.</summary>
        private void cboLineNurse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading || _selectedLine == null) return;

            DataRowView picked = cboLineNurse.SelectedItem as DataRowView;
            int nurseId = picked == null ? 0 : Db.Get<int>(picked["user_id"]);
            string nurseName = picked == null ? string.Empty : Db.Get<string>(picked["full_name"]);

            _loading = true;
            try
            {
                _selectedLine[AppointmentRepository.ColAssessedBy] = nurseId;
                _selectedLine["nurse_name"] = nurseId > 0 ? nurseName : string.Empty;
            }
            finally
            {
                _loading = false;
            }

            Recalculate();
        }

        private void dgvSelected_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLinePanel();
        }

        /// <summary>
        /// Writes the start the user picked into the selected line, with the texts the grid shows
        /// (start and end). The pick is already a time that fits, so nothing has to be re-checked.
        /// </summary>
        private void cboLineTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading || _selectedLine == null) return;

            int index = cboLineTime.SelectedIndex;
            if (index < 0 || index >= _slots.Count) return;

            _loading = true;
            try
            {
                WriteLineStart(_selectedLine, _slots[index]);
            }
            finally
            {
                _loading = false;
            }

            Recalculate();
        }

        private void dgvSelected_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // A combo box or a check box only raises CellValueChanged after the edit is committed,
            // so the change is committed as soon as the cell is dirty. Without this the totals
            // would still show the old value while the user is still inside the cell.
            if (dgvSelected.IsCurrentCellDirty)
            {
                dgvSelected.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Keeps a typing mistake in the Qty column quiet: the grid is told the value is fine and
        /// the Qty cell is put back to 1 below, instead of the raw .NET error dialog.
        /// </summary>
        private void dgvSelected_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;

            // Only a bad Qty is guessed back; anything else is left as the grid handles it.
            if (dgvSelected.Columns[e.ColumnIndex].Name != "colSelQty") return;

            _loading = true;
            try { dgvSelected.Rows[e.RowIndex].Cells["colSelQty"].Value = 1; }
            finally { _loading = false; }
        }

        private void dgvSelected_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0) return;

            string column = dgvSelected.Columns[e.ColumnIndex].Name;

            if (column == "colSelQty")
            {
                // A quantity of 0 or a letter would make the totals nonsense, so it is put back.
                DataGridViewCell cell = dgvSelected.Rows[e.RowIndex].Cells["colSelQty"];
                int quantity = Db.Get<int>(cell.Value, 0);

                if (quantity < 1)
                {
                    _loading = true;
                    try { cell.Value = 1; }
                    finally { _loading = false; }
                }
            }

            Recalculate();
        }

        // ---------------------------------------------------------------- the totals

        private int SelectedClientId()
        {
            if (cboClient.SelectedValue == null) return 0;
            return Db.Get<int>(cboClient.SelectedValue);
        }

        /// <summary>
        /// The start of the booking: the earliest placed line. Every line has its own start, so the
        /// dialog only hands the repository the earliest one (it is the appointment's start_time).
        /// </summary>
        private TimeSpan EarliestStart()
        {
            TimeSpan earliest = TimeSpan.Zero;

            foreach (DataRow row in _lines.Rows)
            {
                TimeSpan start = AppointmentRepository.ReadTime(row[AppointmentRepository.ColStartTime]);
                if (start == TimeSpan.Zero) continue;

                if (earliest == TimeSpan.Zero || start < earliest) earliest = start;
            }

            return earliest == TimeSpan.Zero ? AppointmentRepository.AestheticOpening : earliest;
        }

        private string SelectedSource()
        {
            return cboSource.SelectedItem == null ? "Walk-in" : cboSource.SelectedItem.ToString();
        }

        /// <summary>
        /// Recomputes everything the user can see: the end time, the subtotal, the discount, the
        /// total, the amount required now and the availability message. Every change of a choice
        /// on the dialog ends up here, so the numbers can never disagree with each other.
        /// </summary>
        private void Recalculate()
        {
            if (_loading) return;

            _subtotal = AppointmentRepository.Subtotal(_lines);
            _discount = 0m;

            // The discount comes from the promotion the LINES carry (0 = no promo). Billing reads
            // the promo id from the bill; here the promo lives in the lines the user just picked.
            int promoId = PromoIdInLines();

            if (promoId > 0)
            {
                string message;
                _discount = PromotionRepository.CalculateDiscount(promoId, PromoLines(), out message);

                lblPromoNote.Text = PromoNames(promoId) + ": " +
                    (string.IsNullOrEmpty(message) ? "applied" : message);
                lblPromoNote.Visible = true;
            }
            else
            {
                // The note is only there while it has something to say.
                lblPromoNote.Text = string.Empty;
                lblPromoNote.Visible = false;
            }

            decimal total = _subtotal - _discount;
            if (total < 0m) total = 0m;

            _required = AppointmentRepository.DownpaymentDue(_subtotal, _discount);

            lblSubtotal.Text = "Subtotal: " + Money.Text(_subtotal);
            lblDiscount.Text = "Discount: -" + Money.Text(_discount);
            lblTotal.Text = "Total: " + Money.Text(total);
            lblRequired.Text = "Down payment due after saving: " + Money.Text(_required) +
                (promoId > 0 ? " (promotion: paid in full)" : string.Empty);

            LockSessionQtyCells();
            CheckAvailability();
        }

        /// <summary>
        /// A package and a promo line are ONE line for ONE price, so their Qty cell is read only;
        /// only a plain service may be repeated.
        /// </summary>
        private void LockSessionQtyCells()
        {
            foreach (DataGridViewRow row in dgvSelected.Rows)
            {
                int serviceId = Db.Get<int>(row.Cells["colSelServiceId"].Value);
                DataRowView line = serviceId <= 0 ? null : FindLine(serviceId);
                if (line == null) continue;

                int totalSessions = Db.Get<int>(line[AppointmentRepository.ColTotalSessions]);
                int defaultSessions = Db.Get<int>(line["default_total_sessions"], 1);
                int promoId = Db.Get<int>(line["promo_id"]);

                // A package is a service with more than one session; its label may still read
                // "Session 1 of 1" until a client turns it into a real plan.
                row.Cells["colSelQty"].ReadOnly = defaultSessions > 1 || totalSessions > 1 || promoId > 0;
            }
        }

        /// <summary>The lines of the booking in the shape the discount formula wants them.</summary>
        private DataTable PromoLines()
        {
            DataTable table = new DataTable();
            table.Columns.Add("service_id", typeof(int));
            table.Columns.Add("amount", typeof(decimal));

            foreach (DataRow row in _lines.Rows)
            {
                int quantity = Db.Get<int>(row[AppointmentRepository.ColQuantity], 1);
                if (quantity < 1) quantity = 1;

                table.Rows.Add(Db.Get<int>(row[AppointmentRepository.ColServiceId]),
                    Db.Get<decimal>(row[AppointmentRepository.ColPrice]) * quantity);
            }

            return table;
        }

        private string PromoNames(int promoId)
        {
            DataRow promo;
            try
            {
                promo = PromotionRepository.GetById(promoId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return "The promotion";
            }

            return promo == null ? "The promotion" : Db.Get<string>(promo["promo_name"]);
        }

        /// <summary>The promotion id the lines of the booking carry (0 = no promotion).</summary>
        private int PromoIdInLines()
        {
            foreach (DataRow row in _lines.Rows)
            {
                int promoId = Db.Get<int>(row["promo_id"]);
                if (promoId > 0) return promoId;
            }

            return 0;
        }

        /// <summary>The promo id behind the selected entry of cboCategoryFilter (0 = a category).</summary>
        private int SelectedPromoCategoryId()
        {
            int index = cboCategoryFilter.SelectedIndex;
            if (index < 0 || index >= _promoIds.Count) return 0;
            return _promoIds[index];
        }

        /// <summary>The category name of one service, from the catalog the dialog already loaded.</summary>
        private string CategoryName(int serviceId)
        {
            DataRow row = CatalogRow(serviceId);
            return row == null ? string.Empty : Db.Get<string>(row["category_name"]);
        }

        /// <summary>
        /// The live availability check: the first problems are written under the schedule line,
        /// in amber when something has to change and in green when the booking is possible.
        /// </summary>
        private void CheckAvailability()
        {
            if (_lines.Rows.Count == 0)
            {
                lblSlotInfo.Text = "Add at least one service. The opening hours and the free rooms are checked here.";
                lblSlotInfo.ForeColor = Theme.TextMuted;
                return;
            }

            List<string> problems;
            try
            {
                problems = AppointmentRepository.ValidateBooking(RescheduleId, SelectedClientId(),
                    dtpDate.Value.Date, EarliestStart(), SelectedSource(), _lines, PromoIdInLines());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (problems.Count == 0)
            {
                lblSlotInfo.Text = "All services are available.";
                lblSlotInfo.ForeColor = Theme.Success;
                return;
            }

            // A missing client and a missing service are the user's next step, not an error in the
            // slot, so those two are shown in the muted color instead of the warning color.
            string first = problems[0];
            lblSlotInfo.Text = problems.Count == 1 ? first : first + "   (+" + (problems.Count - 1) + " more)";
            lblSlotInfo.ForeColor = first.StartsWith("Choose the client") || first.StartsWith("Add at least")
                ? Theme.TextMuted
                : Theme.Warning;
        }

        // ---------------------------------------------------------------- saving

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            if (RescheduleId > 0)
            {
                SaveReschedule();
                return;
            }

            SaveNewBooking();
        }

        /// <summary>The client and the service are checked before anything is saved.</summary>
        private bool ValidateInput()
        {
            errorProvider1.Clear();

            if (SelectedClientId() <= 0)
            {
                Validators.SetError(errorProvider1, cboClient, "Choose the client.");
                Messages.Warn("Choose the client first.");
                cboClient.Focus();
                return false;
            }

            if (_lines.Rows.Count == 0)
            {
                Messages.Warn("Add at least one service to the appointment.");
                return false;
            }

            return true;
        }

        private void SaveNewBooking()
        {
            List<string> problems;

            try
            {
                problems = AppointmentRepository.ValidateBooking(0, SelectedClientId(), dtpDate.Value.Date,
                    EarliestStart(), SelectedSource(), _lines, PromoIdInLines());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (problems.Count > 0)
            {
                // All the problems are shown at once, so the user can fix them in one go.
                Messages.Warn(string.Join(Environment.NewLine + Environment.NewLine, problems.ToArray()));
                return;
            }

            int appointmentId;
            try
            {
                appointmentId = AppointmentRepository.Create(SelectedClientId(), Session.UserId,
                    dtpDate.Value.Date, EarliestStart(), SelectedSource(), txtNotes.Text, _lines,
                    PromoIdInLines());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            SavedAppointmentId = appointmentId;
            SavedAppointmentDate = dtpDate.Value.Date;

            // The booking takes no money, so the desk is told where the down payment goes next.
            Messages.Info("Appointment #" + appointmentId + " was booked. Next: take the down payment of " +
                Money.Text(_required) + " in Billing.");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveReschedule()
        {
            DataRow appointment;
            try
            {
                appointment = AppointmentRepository.GetById(RescheduleId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (appointment == null)
            {
                Messages.Warn("This appointment no longer exists.");
                return;
            }

            DateTime appointmentDate = Db.Get<DateTime>(appointment["appointment_date"]);
            string reason = string.Empty;

            // Inside the 3-day window an Admin must give a reason. The user is warned first, so the
            // prompt does not appear out of nowhere.
            int daysAway;
            if (AppointmentRepository.IsInsideNoticeWindow(appointmentDate, out daysAway) && Session.IsAdmin)
            {
                Messages.Warn("This appointment is less than " + AppointmentRepository.NoticeDays +
                    " days away, so a reason is required for the reschedule.");

                if (!AskReason(this, "Reason for rescheduling (required)", out reason)) return;
            }

            try
            {
                AppointmentRepository.Reschedule(RescheduleId, dtpDate.Value.Date,
                    EarliestStart(), reason);
            }
            catch (Exception ex)
            {
                // Reschedule spells out the rule it could not apply, so the user gets that sentence
                // instead of a database error.
                Messages.Warn(ex.Message);
                return;
            }

            SavedAppointmentId = RescheduleId;
            SavedAppointmentDate = dtpDate.Value.Date;

            Messages.Info("Appointment #" + RescheduleId + " moved to " +
                dtpDate.Value.ToString("MMM dd, yyyy") + " at " + Clock(EarliestStart()) + ".");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // The Designer sets DialogResult.Cancel, so nothing else has to happen here.
        }

        // ---------------------------------------------------------------- the reason prompt

        /// <summary>
        /// A small modal that asks for a reason (used by the cancel and the reschedule inside the
        /// 3-day window). It is built in code because it is a throwaway dialog with one box.
        /// Returns false when the user closes it without confirming.
        /// </summary>
        public static bool AskReason(IWin32Window owner, string heading, out string reason)
        {
            reason = string.Empty;

            using (Form dialog = new Form())
            {
                dialog.Text = "Little Erese's";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(460, 220);
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.BackColor = Theme.AppBack;

                Label label = new Label();
                label.Text = heading;
                label.Font = Theme.BoldFont;
                label.ForeColor = Theme.TextDark;
                label.Location = new Point(16, 16);
                label.Size = new Size(428, 40);
                dialog.Controls.Add(label);

                TextBox box = new TextBox();
                box.Multiline = true;
                box.ScrollBars = ScrollBars.Vertical;
                box.Font = Theme.BodyFont;
                box.Location = new Point(16, 62);
                box.Size = new Size(428, 96);
                dialog.Controls.Add(box);

                Button ok = new Button();
                ok.Text = "OK";
                ok.Size = new Size(96, 32);
                ok.Location = new Point(248, 172);
                Theme.StylePrimaryButton(ok);
                ok.DialogResult = DialogResult.OK;
                dialog.Controls.Add(ok);

                Button cancel = new Button();
                cancel.Text = "Cancel";
                cancel.Size = new Size(96, 32);
                cancel.Location = new Point(348, 172);
                Theme.StyleSecondaryButton(cancel);
                cancel.DialogResult = DialogResult.Cancel;
                dialog.Controls.Add(cancel);

                dialog.AcceptButton = ok;
                dialog.CancelButton = cancel;
                box.Focus();

                if (dialog.ShowDialog(owner) != DialogResult.OK) return false;

                reason = box.Text;
                return true;
            }
        }

        // ---------------------------------------------------------------- small helpers

        /// <summary>A time column read back safely whatever the provider hands over.</summary>
        private static TimeSpan ReadTime(object value)
        {
            if (value == null || value == DBNull.Value) return TimeSpan.Zero;
            if (value is TimeSpan) return (TimeSpan)value;
            if (value is DateTime) return ((DateTime)value).TimeOfDay;

            TimeSpan parsed;
            if (TimeSpan.TryParse(value.ToString(), out parsed)) return parsed;
            return TimeSpan.Zero;
        }

        /// <summary>"10:00 AM" for a time value, so every line reads the same way.</summary>
        private static string Clock(TimeSpan value)
        {
            return DateTime.Today.Add(value).ToString("h:mm tt");
        }

    }
}
