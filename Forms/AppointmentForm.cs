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
        private const string NoPromo = "No promo";
        private const string AnyTechnician = "Any available";

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

        // The start times, kept in the same order as the items of cboTime, so an index always
        // maps back to a real time without parsing the text.
        private readonly List<TimeSpan> _slots = new List<TimeSpan>();

        // The promotion ids behind the items of cboPromo (0 = no promotion).
        private readonly List<int> _promoIds = new List<int>();

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

            _loading = true;
            try
            {
                // The date may not be in the past. Today only works while the time is still ahead,
                // which the availability check reports line by line.
                dtpDate.MinDate = DateTime.Today;
                dtpDate.Value = DateTime.Today;

                BuildLinesTable();
                FillTimeSlots();
                FillSources();
                FillCategories();
                FillCatalog();
                FillClients();
                FillTechnicians();
                FillPromotions();
                FillPaymentMethods();
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
                lblClientInfo.Text = "Pick a client to see the consent status and the medical alerts.";
            }

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

            // The technician may be changed by the user, so it is the last column (see the
            // technician combo column of dgvSelected).
            _lines.Columns.Add(AppointmentRepository.ColTechnicianId, typeof(int));
            _lines.DefaultView.Sort = AppointmentRepository.ColServiceName + " ASC";
            _lines.Columns[AppointmentRepository.ColTechnicianId].DefaultValue = 0;

            dgvSelected.DataSource = _lines;
        }

        private void FillTimeSlots()
        {
            _slots.Clear();
            cboTime.Items.Clear();

            foreach (TimeSpan slot in AppointmentRepository.GetTimeSlots())
            {
                _slots.Add(slot);
                cboTime.Items.Add(Clock(slot));
            }

            if (cboTime.Items.Count > 0) cboTime.SelectedIndex = 0;
        }

        private void FillSources()
        {
            if (cboSource.Items.Count > 0) cboSource.SelectedIndex = 0;   // "Walk-in"
        }

        private void FillCategories()
        {
            cboCategoryFilter.Items.Clear();
            cboCategoryFilter.Items.Add("All categories");

            try
            {
                foreach (DataRow row in ServiceRepository.GetCategories().Rows)
                {
                    cboCategoryFilter.Items.Add(Db.Get<string>(row["category_name"]));
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
        /// The technician drop-down of the selected-services grid: every Active Staff member plus a
        /// blank "Any available" line, so a service may be left unassigned.
        /// </summary>
        private void FillTechnicians()
        {
            DataTable staff = new DataTable();
            staff.Columns.Add("user_id", typeof(int));
            staff.Columns.Add("full_name", typeof(string));
            staff.Rows.Add(0, AnyTechnician);

            try
            {
                foreach (DataRow row in UserRepository.GetActiveStaff().Rows)
                {
                    staff.Rows.Add(Db.Get<int>(row["user_id"]), Db.Get<string>(row["full_name"]));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            colSelTechnician.DataSource = staff;
            colSelTechnician.DisplayMember = "full_name";
            colSelTechnician.ValueMember = "user_id";
        }

        /// <summary>
        /// Makes sure the technician list of dgvSelected knows this account before a line uses it.
        /// WHY: a technician who was deactivated after the appointment was booked is no longer in
        /// GetActiveStaff, and a DataGridViewComboBoxColumn throws for a value it does not know.
        /// </summary>
        private void EnsureTechnicianInList(int technicianId)
        {
            if (technicianId <= 0) return;

            DataTable staff = colSelTechnician.DataSource as DataTable;
            if (staff == null) return;

            foreach (DataRow row in staff.Rows)
            {
                if (Db.Get<int>(row["user_id"]) == technicianId) return;
            }

            DataRow user;
            try
            {
                user = UserRepository.GetById(technicianId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            string name = user == null
                ? "Technician #" + technicianId
                : Db.Get<string>(user["full_name"]) + " (inactive)";

            staff.Rows.Add(technicianId, name);
        }

        private void FillPromotions()
        {
            _promoIds.Clear();
            cboPromo.Items.Clear();
            cboPromo.Items.Add(NoPromo);
            _promoIds.Add(0);

            try
            {
                // GetActiveValid already means "Active and valid today".
                foreach (DataRow row in PromotionRepository.GetActiveValid().Rows)
                {
                    _promoIds.Add(Db.Get<int>(row["promo_id"]));

                    string type = Db.Get<string>(row["discount_type"]);
                    decimal value = Db.Get<decimal>(row["discount_value"]);
                    string amount = string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase)
                        ? value.ToString("0.##") + " % off"
                        : Money.Text(value) + " off";

                    cboPromo.Items.Add(Db.Get<string>(row["promo_name"]) + " - " + amount);
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            cboPromo.SelectedIndex = 0;
        }

        private void FillPaymentMethods()
        {
            if (cboPayMethod.Items.Count > 0) cboPayMethod.SelectedIndex = 0;   // "Cash"
            UpdatePaymentFields();
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
                if (slotIndex >= 0) cboTime.SelectedIndex = slotIndex;

                // The existing services are shown as they are (the user cannot change them).
                foreach (DataRow line in lines.Rows)
                {
                    int technicianId = Db.Get<int>(line[AppointmentRepository.ColTechnicianId]);

                    // A technician who has since been deactivated is not in the list of the combo
                    // column, and a value the combo does not know makes the grid throw. The name is
                    // added here so an old appointment still opens.
                    EnsureTechnicianInList(technicianId);

                    _lines.Rows.Add(
                        Db.Get<int>(line[AppointmentRepository.ColServiceId]),
                        Db.Get<string>(line[AppointmentRepository.ColServiceName]),
                        Db.Get<int>(line[AppointmentRepository.ColQuantity], 1),
                        Db.Get<decimal>(line[AppointmentRepository.ColPrice]),
                        Db.Get<int>(line[AppointmentRepository.ColDuration]),
                        PackageLabel(line),
                        Db.Get<int>(line["default_total_sessions"], 1),
                        Db.Get<int>(line[AppointmentRepository.ColPackageId]),
                        Db.Get<int>(line[AppointmentRepository.ColSessionNumber]),
                        Db.Get<int>(line[AppointmentRepository.ColTotalSessions]),
                        technicianId);
                }

                txtNotes.Text = Db.Get<string>(row["notes"]);

                // The promotion the bill was made with is shown too, so the totals below match
                // the original bill instead of pretending the booking has no promotion.
                ShowSavedPromo();

                // Only the date and the time may change.
                cboClient.Enabled = false;
                btnNewClient.Enabled = false;
                cboSource.Enabled = false;
                cboCategoryFilter.Enabled = false;
                dgvCatalog.Enabled = false;
                btnAddService.Enabled = false;
                btnRemoveService.Enabled = false;
                dgvSelected.ReadOnly = true;
                cboPromo.Enabled = false;
                grpPayment.Enabled = false;
                txtNotes.ReadOnly = true;
            }
            finally
            {
                _loading = false;
            }
        }

        /// <summary>
        /// Selects the promotion of the appointment being rescheduled (read only, the user cannot
        /// change the money of a booking that is only being moved).
        /// </summary>
        private void ShowSavedPromo()
        {
            DataRow billing;
            try
            {
                billing = BillingRepository.GetByAppointment(RescheduleId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (billing == null) return;

            int promoId = Db.Get<int>(billing["promo_id"]);
            int index = _promoIds.IndexOf(promoId);
            if (index > 0) cboPromo.SelectedIndex = index;
        }

        /// <summary>The "Session 3 of 8" / empty label of a stored appointment line.</summary>
        private static string PackageLabel(DataRow line)
        {
            int sessionNumber = Db.Get<int>(line[AppointmentRepository.ColSessionNumber]);
            int totalSessions = Db.Get<int>(line[AppointmentRepository.ColTotalSessions]);

            if (sessionNumber <= 0 || totalSessions <= 0) return string.Empty;
            return "Session " + sessionNumber + " of " + totalSessions;
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
            Recalculate();
        }

        private void ShowClientInfo(int clientId)
        {
            if (clientId <= 0)
            {
                lblClientInfo.Text = "Pick a client to see the consent status and the medical alerts.";
                lblClientInfo.ForeColor = Color.FromArgb(0x7A, 0x72, 0x66);
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
                lblClientInfo.ForeColor = Color.FromArgb(0xC0, 0x39, 0x2B);
                return;
            }

            bool signed = Db.Get<bool>(row["consent_status"]);
            string allergies = Db.Get<string>(row["allergies"]);
            string medical = Db.Get<string>(row["medical_history"]);

            List<string> alerts = new List<string>();
            if (!string.IsNullOrWhiteSpace(allergies)) alerts.Add("allergies: " + allergies.Trim());
            if (!string.IsNullOrWhiteSpace(medical)) alerts.Add("medical history: " + medical.Trim());

            string text = signed ? "Consent form signed." : "Consent form NOT signed yet.";
            text += alerts.Count == 0 ? "  No medical alerts." : "  Alert - " + string.Join("; ", alerts.ToArray());

            lblClientInfo.Text = text;
            // Red while a warning is on the line, so nobody can miss a medical alert.
            lblClientInfo.ForeColor = alerts.Count > 0 || !signed
                ? Color.FromArgb(0xC0, 0x39, 0x2B)
                : Color.FromArgb(0x7A, 0x72, 0x66);
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
            if (dgvCatalog.CurrentRow == null) return;

            int serviceId = Db.Get<int>(dgvCatalog.CurrentRow.Cells["colCatServiceId"].Value);
            if (serviceId <= 0) return;

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
            string label = string.Empty;

            if (clientId > 0 && defaultSessions > 1)
            {
                try
                {
                    if (AppointmentRepository.TryGetPackagePlan(clientId, serviceId,
                        out packageId, out sessionNumber, out totalSessions, out isNewPackage))
                    {
                        label = isNewPackage
                            ? "New package (" + sessionNumber + " of " + totalSessions + ")"
                            : "Session " + sessionNumber + " of " + totalSessions;

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

            _lines.Rows.Add(serviceId, serviceName, 1, price, duration, label, defaultSessions,
                packageId, sessionNumber, totalSessions, 0);

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

            _lines.Rows.Remove(line.Row);
            Recalculate();
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

                if (defaultSessions <= 1) continue;

                if (clientId <= 0)
                {
                    line["package_label"] = string.Empty;
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

                    line["package_label"] = isNewPackage
                        ? "New package (" + sessionNumber + " of " + totalSessions + ")"
                        : "Session " + sessionNumber + " of " + totalSessions;

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

        private void cboTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            Recalculate();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            Recalculate();
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            Recalculate();
        }

        private void cboPromo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            Recalculate();
        }

        private void cboPayMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            UpdatePaymentFields();
        }

        /// <summary>
        /// A GCash or bank payment needs a reference number and a verified tick; cash does not,
        /// so those two controls follow the method.
        /// </summary>
        private void UpdatePaymentFields()
        {
            string method = SelectedPaymentMethod();
            bool needsReference = string.Equals(method, "GCash", StringComparison.OrdinalIgnoreCase)
                || string.Equals(method, "Bank Transfer", StringComparison.OrdinalIgnoreCase);

            txtPayReference.Enabled = needsReference;
            chkPayVerified.Enabled = needsReference;

            if (!needsReference)
            {
                txtPayReference.Clear();
                chkPayVerified.Checked = false;
            }
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

        private int SelectedPromoId()
        {
            int index = cboPromo.SelectedIndex;
            if (index < 0 || index >= _promoIds.Count) return 0;
            return _promoIds[index];
        }

        private TimeSpan SelectedStartTime()
        {
            int index = cboTime.SelectedIndex;
            if (index < 0 || index >= _slots.Count) return AppointmentRepository.AestheticOpening;
            return _slots[index];
        }

        private string SelectedSource()
        {
            return cboSource.SelectedItem == null ? "Walk-in" : cboSource.SelectedItem.ToString();
        }

        private string SelectedPaymentMethod()
        {
            return cboPayMethod.SelectedItem == null ? "Cash" : cboPayMethod.SelectedItem.ToString();
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

            int promoId = SelectedPromoId();

            if (promoId > 0)
            {
                string message;
                _discount = PromotionRepository.CalculateDiscount(promoId, PromoLines(), out message);
                lblPromoNote.Text = string.IsNullOrEmpty(message)
                    ? PromoNames(promoId) + " applies to: " + EligibleNames(promoId)
                    : message;
            }
            else
            {
                lblPromoNote.Text = "No promotion chosen, so the bill is the plain price of the services.";
            }

            decimal total = _subtotal - _discount;
            if (total < 0m) total = 0m;

            _required = AppointmentRepository.RequiredNow(SelectedSource(), _subtotal, _discount);

            TimeSpan duration = AppointmentRepository.TotalDuration(_lines);
            TimeSpan start = SelectedStartTime();

            lblEndTime.Text = duration <= TimeSpan.Zero
                ? "Ends -"
                : "Ends " + Clock(start + duration) + " (" + duration.TotalMinutes.ToString("0") + " min)";

            lblSubtotal.Text = "Subtotal: " + Money.Text(_subtotal);
            lblDiscount.Text = "Discount: -" + Money.Text(_discount);
            lblTotal.Text = "Total: " + Money.Text(total);
            lblRequired.Text = _required > 0m
                ? "Required now: " + Money.Text(_required)
                : "Required now: " + Money.Text(0m) + " (pay at the counter)";

            // A reschedule never takes money, so the payment group stays closed in that mode.
            grpPayment.Enabled = _required > 0m && RescheduleId <= 0;

            if (_required > 0m)
            {
                txtPayAmount.Text = _required.ToString("0.00");
            }
            else
            {
                txtPayAmount.Clear();
            }

            CheckAvailability();
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

        /// <summary>
        /// The names of the selected services the promotion really discounts. When the promotion is
        /// linked to no service at all, it applies to the whole bill.
        /// </summary>
        private string EligibleNames(int promoId)
        {
            List<int> linked;
            try
            {
                linked = PromotionRepository.GetServiceIds(promoId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return "the whole bill";
            }

            if (linked.Count == 0) return "every service on the bill";

            List<string> names = new List<string>();
            foreach (DataRow row in _lines.Rows)
            {
                int serviceId = Db.Get<int>(row[AppointmentRepository.ColServiceId]);
                if (!linked.Contains(serviceId)) continue;

                string name = Db.Get<string>(row[AppointmentRepository.ColServiceName]);
                if (!names.Contains(name)) names.Add(name);
            }

            return names.Count == 0 ? "no service of this appointment" : string.Join(", ", names.ToArray());
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
                    dtpDate.Value.Date, SelectedStartTime(), SelectedSource(), _lines, SelectedPromoId(),
                    PaymentAmount(), SelectedPaymentMethod(), txtPayReference.Text, chkPayVerified.Checked);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (problems.Count == 0)
            {
                lblSlotInfo.Text = "This slot is free: " + dtpDate.Value.ToString("MMM dd, yyyy") + " " +
                    Clock(SelectedStartTime()) + " - " + Clock(SelectedStartTime() +
                    AppointmentRepository.TotalDuration(_lines)) + ".";
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

        private decimal PaymentAmount()
        {
            decimal amount;
            if (!Validators.TryMoney(txtPayAmount.Text, out amount)) return 0m;
            return amount < 0m ? 0m : amount;
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

        /// <summary>The client, the service and the payment amount are checked before anything is saved.</summary>
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

            // The amount box may be left empty (a walk-in pays at the counter), but a value that
            // is not a number is a typing mistake and would silently become "nothing".
            if (!string.IsNullOrWhiteSpace(txtPayAmount.Text) && !Validators.TryMoney(txtPayAmount.Text, out _))
            {
                Validators.SetError(errorProvider1, txtPayAmount, "Enter a number, for example 500.00.");
                Messages.Warn("The amount paid must be a number, for example 500.00.");
                txtPayAmount.Focus();
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
                    SelectedStartTime(), SelectedSource(), _lines, SelectedPromoId(), PaymentAmount(),
                    SelectedPaymentMethod(), txtPayReference.Text, chkPayVerified.Checked);
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
                    dtpDate.Value.Date, SelectedStartTime(), SelectedSource(), txtNotes.Text, _lines,
                    SelectedPromoId(), PaymentAmount(), SelectedPaymentMethod(), txtPayReference.Text,
                    chkPayVerified.Checked);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            SavedAppointmentId = appointmentId;
            SavedAppointmentDate = dtpDate.Value.Date;

            Messages.Info("Appointment #" + appointmentId + " was booked for " +
                dtpDate.Value.ToString("MMM dd, yyyy") + " at " + Clock(SelectedStartTime()) + ".");

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
                    SelectedStartTime(), reason);
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
                dtpDate.Value.ToString("MMM dd, yyyy") + " at " + Clock(SelectedStartTime()) + ".");

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
