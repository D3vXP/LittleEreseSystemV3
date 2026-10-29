using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// Appointments page: the appointments of one day on top and the details of the selected
    /// one below. Every database rule lives in AppointmentRepository; this screen only decides
    /// what the user sees and which buttons are allowed for the selected row.
    /// </summary>
    public partial class ucAppointments : UserControl
    {
        private const string StatusUpcoming = "Upcoming";

        // True while the screen fills its own controls, so the filter and selection events do
        // not reload the grid or the details card twice.
        private bool _loading;

        /// <summary>
        /// The day the page opens on. The caller (the Dashboard later) may set this before the
        /// control is shown; when it is not set the page opens on today.
        /// </summary>
        public DateTime StartDate { get; set; }

        public ucAppointments()
        {
            InitializeComponent();
        }

        private void ucAppointments_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvAppointments);
            Theme.StyleGrid(dgvDetailServices);

            Theme.StylePrimaryButton(btnNew);
            Theme.StyleSecondaryButton(btnTakePayment);
            Theme.StyleSecondaryButton(btnStart);
            Theme.StyleSecondaryButton(btnReschedule);
            Theme.StyleDangerButton(btnCancelAppointment);
            Theme.StyleSecondaryButton(btnRefresh);

            // The two glyph buttons keep the icon font the Designer gave them, which is why the
            // secondary style (it sets the body font) is NOT applied to them.

            // The grid column only says "N2" in the Designer; Money is the one formatter of the app.
            Money.ApplyTo(colDetailPrice);

            _loading = true;
            try
            {
                // "Active" is first on purpose: the day list is the working list, and a cancelled
                // or rescheduled appointment is not something the shop still has to do.
                cboStatus.SelectedIndex = 0;   // "Active"

                // StartDate is DateTime.MinValue when nobody set it, and that is not a real day.
                dtpDate.Value = StartDate.Date > DateTime.MinValue.Date ? StartDate.Date : DateTime.Today;
            }
            finally
            {
                _loading = false;
            }

            LoadAppointments(0);
        }

        // ---------------------------------------------------------------- loading

        /// <summary>
        /// Reloads the day list. keepAppointmentId is selected again afterwards (0 = nothing),
        /// so the row the user just booked, moved or cancelled stays highlighted.
        /// </summary>
        private void LoadAppointments(int keepAppointmentId)
        {
            string status = cboStatus.SelectedItem == null ? "Active" : cboStatus.SelectedItem.ToString();

            // _loading stops SelectionChanged from filling the card while the grid rebinds.
            _loading = true;
            try
            {
                try
                {
                    dgvAppointments.DataSource = AppointmentRepository.GetByDate(dtpDate.Value.Date, status, txtSearch.Text);
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                dgvAppointments.ClearSelection();
                GridHelper.SelectRowById(dgvAppointments, "colAppointmentId", keepAppointmentId);
            }
            finally
            {
                _loading = false;
            }

            UpdateSummary();

            int appointmentId = CurrentAppointmentId();
            if (appointmentId > 0) ShowDetails(appointmentId);
            else ClearDetails();
        }

        /// <summary>The strip on the toolbar: how busy the day is (the example "3 appointments - 1 massage").</summary>
        private void UpdateSummary()
        {
            int total;
            int massage;

            try
            {
                AppointmentRepository.GetDayCounts(dtpDate.Value.Date, out total, out massage);
            }
            catch (Exception ex)
            {
                // The count is only information, so a problem here must not break the screen.
                Messages.DbError(ex);
                total = dgvAppointments.Rows.Count;
                massage = 0;
            }

            lblSummary.Text = total + (total == 1 ? " appointment" : " appointments") +
                " - " + massage + " massage";
        }

        private int CurrentAppointmentId()
        {
            return GridHelper.GetSelectedId(dgvAppointments, "colAppointmentId");
        }

        // ---------------------------------------------------------------- details card

        /// <summary>Fills the card below the grid with the selected appointment. Id 0 clears it.</summary>
        private void ShowDetails(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                ClearDetails();
                return;
            }

            DataRow row;
            try
            {
                row = AppointmentRepository.GetById(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                // The row disappeared (the day changed under us), so nothing is shown for it.
                ClearDetails();
                return;
            }

            string status = Db.Get<string>(row["status"]);

            lblDetailClient.Text = Db.Get<string>(row["client_name"]) + "   (" + status + ")";

            TimeSpan start = ReadTime(row["start_time"]);
            TimeSpan end = ReadTime(row["end_time"]);

            lblDetailSchedule.Text = "Schedule: " + Db.Get<DateTime>(row["appointment_date"]).ToString("MMM dd, yyyy") +
                "   " + Clock(start) + " - " + Clock(end) + "   (" + Db.Get<string>(row["booking_source"]) + ")";

            lblDetailPayment.Text = PaymentSummary(appointmentId);

            txtDetailNotes.Text = Db.Get<string>(row["notes"]);

            try
            {
                dgvDetailServices.DataSource = AppointmentRepository.GetServiceLines(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            // Start Service only makes sense while the appointment is upcoming AND today.
            bool isToday = Db.Get<DateTime>(row["appointment_date"]).Date == DateTime.Today;
            bool upcoming = string.Equals(status, StatusUpcoming, StringComparison.OrdinalIgnoreCase);
            bool ongoing = string.Equals(status, "Ongoing", StringComparison.OrdinalIgnoreCase);

            btnStart.Enabled = upcoming && isToday;
            btnReschedule.Enabled = upcoming;
            btnCancelAppointment.Enabled = upcoming;

            // A payment may be taken at any time while the appointment is still work, as long as
            // the bill still has something left to pay (a bill of 0.00 has nothing to take).
            btnTakePayment.Enabled = (upcoming || ongoing) && HasBalance(appointmentId);
        }

        /// <summary>Empties the card and switches off every action that needs an appointment.</summary>
        private void ClearDetails()
        {
            lblDetailClient.Text = "Select an appointment";
            lblDetailSchedule.Text = "Schedule: -";
            lblDetailPayment.Text = "Payment: -";
            txtDetailNotes.Text = string.Empty;
            dgvDetailServices.DataSource = null;

            btnStart.Enabled = false;
            btnReschedule.Enabled = false;
            btnCancelAppointment.Enabled = false;
            btnTakePayment.Enabled = false;
        }

        /// <summary>True when the appointment has a bill with something left to pay.</summary>
        private static bool HasBalance(int appointmentId)
        {
            try
            {
                DataRow billing = BillingRepository.GetByAppointment(appointmentId);
                return billing != null && Db.Get<decimal>(billing["balance_due"]) > 0m;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return false;
            }
        }

        /// <summary>
        /// "Paid PHP 500.00 of PHP 1,000.00 - Balance PHP 500.00" for the selected appointment.
        /// This is information only, so a missing bill simply says so.
        /// </summary>
        private static string PaymentSummary(int appointmentId)
        {
            DataRow billing;
            try
            {
                billing = BillingRepository.GetByAppointment(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return "Payment: -";
            }

            if (billing == null) return "Payment: no bill yet";

            decimal paid = Db.Get<decimal>(billing["paid_amount"]);
            decimal total = Db.Get<decimal>(billing["total_amount"]);
            decimal balance = Db.Get<decimal>(billing["balance_due"]);

            // A later session of a package carries no price of its own, so there is nothing to pay.
            if (total == 0m) return "Payment: Included in the package - nothing to pay.";

            string summary = "Payment: Paid " + Money.Text(paid) + " of " + Money.Text(total) +
                " - Balance " + Money.Text(balance);

            decimal discount = Db.Get<decimal>(billing["discount_amount"]);
            if (discount > 0m)
            {
                summary += "   (promo " + Db.Get<string>(billing["promo_name"], "-") + ", -" + Money.Text(discount) + ")";
            }

            return summary;
        }

        // ---------------------------------------------------------------- toolbar actions

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            MoveDay(-1);
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            MoveDay(1);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Today;
        }

        private void MoveDay(int days)
        {
            DateTime target = dtpDate.Value.Date.AddDays(days);

            // The picker refuses dates before its minimum, so going back too far is simply ignored.
            if (target < dtpDate.MinDate) return;

            dtpDate.Value = target;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadAppointments(0);
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadAppointments(0);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadAppointments(0);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments(CurrentAppointmentId());
        }

        private void btnTakePayment_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0) return;

            PaymentForm.TakePayment(this, appointmentId, "Payment - appointment #" + appointmentId);

            // The balance and the payment column have changed, so the page is filled again with
            // the same row still selected.
            LoadAppointments(appointmentId);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (AppointmentForm form = new AppointmentForm())
            {
                form.RescheduleId = 0;
                form.StartDate = dtpDate.Value.Date;

                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The new appointment may be on another day, so the page moves to its date.
                if (form.SavedAppointmentDate > DateTime.MinValue.Date)
                {
                    _loading = true;
                    try { dtpDate.Value = form.SavedAppointmentDate.Date; }
                    finally { _loading = false; }
                }

                LoadAppointments(form.SavedAppointmentId);
            }
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0) return;

            using (AppointmentForm form = new AppointmentForm())
            {
                form.RescheduleId = appointmentId;

                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The appointment moved to another day, so the page follows it.
                if (form.SavedAppointmentDate > DateTime.MinValue.Date)
                {
                    _loading = true;
                    try { dtpDate.Value = form.SavedAppointmentDate.Date; }
                    finally { _loading = false; }
                }

                LoadAppointments(appointmentId);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0) return;

            if (!Messages.Confirm("Start the service for this appointment now?")) return;

            try
            {
                AppointmentRepository.SetStatus(appointmentId, "Ongoing");
            }
            catch (Exception ex)
            {
                // SetStatus spells out the rule it could not apply ("only on its own day", ...),
                // so that message is shown as a warning instead of a database error.
                Messages.Warn(ex.Message);
                LoadAppointments(appointmentId);
                return;
            }

            Messages.Info("The appointment is now ongoing.");
            LoadAppointments(appointmentId);
        }

        private void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0) return;

            DataRow row;
            try
            {
                row = AppointmentRepository.GetById(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                LoadAppointments(0);
                return;
            }

            string clientName = Db.Get<string>(row["client_name"]);
            DateTime appointmentDate = Db.Get<DateTime>(row["appointment_date"]);

            // The 3-day rule: inside the notice window only an Admin may cancel, with a reason.
            int daysAway;
            bool insideWindow = AppointmentRepository.IsInsideNoticeWindow(appointmentDate, out daysAway);

            if (insideWindow && !Session.IsAdmin)
            {
                Messages.Warn("An appointment can only be cancelled at least " +
                    AppointmentRepository.NoticeDays + " days before its date. This one is in " +
                    Math.Max(daysAway, 0) + " day(s). Ask an Admin to cancel it.");
                return;
            }

            if (!Messages.Confirm("Cancel the appointment of " + clientName + " on " +
                appointmentDate.ToString("MMM dd, yyyy") +
                "?\n\nThe money paid upfront is forfeited."))
            {
                return;
            }

            if (insideWindow)
            {
                Messages.Warn("This appointment is less than " + AppointmentRepository.NoticeDays +
                    " days away, so an Admin must type a reason for the cancellation.");
            }

            string reason = string.Empty;
            string heading = insideWindow
                ? "Reason for cancelling inside " + AppointmentRepository.NoticeDays + " days (required)"
                : "Reason for cancelling (optional)";

            if (!AppointmentForm.AskReason(this, heading, out reason)) return;

            if (insideWindow && string.IsNullOrWhiteSpace(reason))
            {
                Messages.Warn("A reason is required because this appointment is inside the " +
                    AppointmentRepository.NoticeDays + "-day window.");
                return;
            }

            // A package session can drag its later sessions along, so the desk is asked about them
            // BEFORE anything is written. Without later sessions the flow is exactly as it was.
            DataTable laterSessions;
            try
            {
                laterSessions = AppointmentRepository.GetLaterSessions(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            bool includeLater = false;
            int laterCount = laterSessions.Rows.Count;

            if (laterCount > 0)
            {
                // The question needs the session's own name and number, which the package line of
                // this appointment carries. Session 0 means the appointment is not a package row.
                string serviceName = "This service";
                int sessionNumber = 0;
                int totalSessions = 0;

                DataTable packageLines;
                try
                {
                    packageLines = AppointmentRepository.GetLinesForBooking(appointmentId);
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                foreach (DataRow line in packageLines.Rows)
                {
                    if (Db.Get<int>(line[AppointmentRepository.ColPackageId]) <= 0) continue;
                    if (Db.Get<int>(line[AppointmentRepository.ColTotalSessions]) <= 1) continue;

                    serviceName = Db.Get<string>(line[AppointmentRepository.ColServiceName]);
                    sessionNumber = Db.Get<int>(line[AppointmentRepository.ColSessionNumber]);
                    totalSessions = Db.Get<int>(line[AppointmentRepository.ColTotalSessions]);
                    break;
                }

                if (sessionNumber <= 1)
                {
                    // Session 1 carries the money of the whole package, so it cannot be left behind.
                    // The repository refuses that anyway, which is why there is no "only this one".
                    DialogResult choice = MessageBox.Show(
                        serviceName + " session 1 of " + totalSessions +
                        " carries the payment of the package, so cancelling it also cancels its " +
                        laterCount + " later session(s).",
                        Messages.Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    if (choice != DialogResult.OK) return;

                    includeLater = true;
                }
                else
                {
                    DialogResult choice = MessageBox.Show(
                        serviceName + " session " + sessionNumber + " of " + totalSessions +
                        " has " + laterCount + " later session(s)." +
                        "\n\nYes = cancel this session and all later sessions" +
                        "\nNo = cancel only this session" +
                        "\nCancel = do nothing",
                        Messages.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button3);

                    if (choice == DialogResult.Cancel) return;

                    includeLater = choice == DialogResult.Yes;
                }
            }

            try
            {
                AppointmentRepository.Cancel(appointmentId, reason, includeLater);
            }
            catch (Exception ex)
            {
                Messages.Warn(ex.Message);
                LoadAppointments(appointmentId);
                return;
            }

            if (includeLater)
            {
                Messages.Info("The appointment was cancelled and the rooms were released. " +
                    (1 + laterCount) + " sessions were cancelled.");
            }
            else
            {
                Messages.Info("The appointment was cancelled and the rooms were released.");
            }

            LoadAppointments(appointmentId);
        }

        private void dgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            int appointmentId = CurrentAppointmentId();
            if (appointmentId > 0) ShowDetails(appointmentId);
            else ClearDetails();
        }

        private void dgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // A double click on a row does nothing on purpose: the details are already on screen.
            // (Rescheduling stays on its own button, so nobody moves an appointment by accident.)
        }

        // ---------------------------------------------------------------- grid colors

        private void dgvAppointments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string column = dgvAppointments.Columns[e.ColumnIndex].Name;
            if (column != "colStatus" && column != "colPayment") return;

            // Status colors come from Theme, so every screen uses the same green / amber / red.
            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            // A selected row keeps white text on its highlight, where the color would be unreadable.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        private void dgvDetailServices_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvDetailServices.Columns[e.ColumnIndex].Name != "colDetailStatus") return;

            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            e.CellStyle.SelectionForeColor = Color.White;
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
