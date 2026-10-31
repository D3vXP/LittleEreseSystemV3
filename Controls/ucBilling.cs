using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// Billing page: the day's queue on the left, the bill of the selected appointment on the
    /// right (Checkout tab) and the list of payments already taken (Transactions tab).
    /// Every money rule lives in BillingRepository; this screen only decides what is shown and
    /// which buttons are allowed.
    /// </summary>
    public partial class ucBilling : UserControl
    {
        // The bill of the selected appointment (see BillingRepository.GetBillHeader), or null.
        private DataRow _bill;

        // True while the screen fills its own controls, so the change events do not run twice.
        private bool _loading;

        public ucBilling()
        {
            InitializeComponent();
        }

        private void ucBilling_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvQueue);
            Theme.StyleGrid(dgvBillLines);
            Theme.StyleGrid(dgvTransactions);

            // The bill grid is the one place where the attending staff can be changed, so the
            // grid itself stays editable and only that column is writable (the rest is ReadOnly).
            dgvBillLines.ReadOnly = false;

            // A staff id that is no longer Active is not in the drop-down list any more, and the
            // grid would otherwise answer with its own raw .NET error dialog.
            dgvBillLines.DataError += delegate { };

            Theme.StyleSecondaryButton(btnQueueToday);
            Theme.StyleSecondaryButton(btnRefresh);
            Theme.StylePrimaryButton(btnCollect);
            Theme.StyleSecondaryButton(btnTakePayment);
            Theme.StylePrimaryButton(btnSearch);

            // The Designer only says "N2"; Money is the one formatter of the app.
            Money.ApplyTo(colQueueBalance);
            Money.ApplyTo(colLinePrice);
            Money.ApplyTo(colTranTotal);
            Money.ApplyTo(colTranPaid);

            _loading = true;
            try
            {
                dtpQueueDate.Value = DateTime.Today;

                // Staff only see today's payments; an Admin starts on the whole month.
                if (Session.IsAdmin)
                {
                    dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    dtpTo.Value = DateTime.Today;
                }
                else
                {
                    dtpFrom.Value = DateTime.Today;
                    dtpTo.Value = DateTime.Today;
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                }

                cboMethod.SelectedIndex = 0;   // "All"
                LoadStaffColumn();
                LoadPromoCombo();
            }
            finally
            {
                _loading = false;
            }

            LoadQueue(0);
            LoadTransactions();
        }

        // ---------------------------------------------------------------- combo boxes

        /// <summary>
        /// The attending staff list of the bill grid: "Select staff" (id 0) plus every Active
        /// technician. Built once, because the same list serves every line.
        /// </summary>
        private void LoadStaffColumn()
        {
            try
            {
                DataTable staff = UserRepository.GetActiveStaff();

                DataTable items = new DataTable();
                items.Columns.Add("user_id", typeof(int));
                items.Columns.Add("full_name", typeof(string));
                items.Rows.Add(0, "Select staff");

                foreach (DataRow row in staff.Rows)
                {
                    items.Rows.Add(Db.Get<int>(row["user_id"]), Db.Get<string>(row["full_name"]));
                }

                colLineTech.DataSource = items;
                colLineTech.DisplayMember = "full_name";
                colLineTech.ValueMember = "user_id";
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>
        /// The promotion list: "No promo" (id 0) plus every promotion that is Active and valid
        /// today. The caption already says what the promotion gives, so the desk does not have to
        /// open another screen.
        /// </summary>
        private void LoadPromoCombo()
        {
            _loading = true;
            try
            {
                DataTable items = new DataTable();
                items.Columns.Add("promo_id", typeof(int));
                items.Columns.Add("display_name", typeof(string));
                items.Rows.Add(0, "No promo");

                foreach (DataRow row in PromotionRepository.GetActiveValid().Rows)
                {
                    int promoId = Db.Get<int>(row["promo_id"]);
                    string type = Db.Get<string>(row["discount_type"], "Percentage");
                    decimal value = Db.Get<decimal>(row["discount_value"]);

                    string amount;
                    if (string.Equals(type, "Promo Price", StringComparison.OrdinalIgnoreCase))
                    {
                        // A Promo Price promo is a package: the caption is its price, not a discount.
                        amount = Money.Text(Db.Get<decimal>(row["promo_price"]));
                    }
                    else if (string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase))
                    {
                        amount = value.ToString("0.##") + " % off";
                    }
                    else
                    {
                        amount = Money.Text(value) + " off";
                    }

                    string caption = Db.Get<string>(row["promo_name"]) + " - " + amount;

                    items.Rows.Add(promoId, caption);
                }

                cboPromo.DataSource = items;
                cboPromo.DisplayMember = "display_name";
                cboPromo.ValueMember = "promo_id";
                cboPromo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
            finally
            {
                _loading = false;
            }
        }

        private int SelectedPromoId()
        {
            if (cboPromo.SelectedValue == null) return 0;
            return Db.Get<int>(cboPromo.SelectedValue);
        }

        /// <summary>Points the promotion box at one promotion id (0 = "No promo").</summary>
        private void SelectPromo(int promoId)
        {
            _loading = true;
            try
            {
                cboPromo.SelectedValue = promoId;
                // A promotion that is no longer valid today is not in the list, so the box falls
                // back to "No promo"; the note under it still names the promotion of the bill.
                if (Db.Get<int>(cboPromo.SelectedValue) != promoId) cboPromo.SelectedIndex = 0;
            }
            finally
            {
                _loading = false;
            }
        }

        // ---------------------------------------------------------------- the queue

        /// <summary>
        /// Reloads the queue of the chosen day. keepAppointmentId is selected again afterwards
        /// (0 = the first row), so a row does not jump away after a payment.
        /// </summary>
        private void LoadQueue(int keepAppointmentId)
        {
            int appointmentId = 0;

            _loading = true;
            try
            {
                // Three modes: the day queue is the default; a search text or a ticked filter
                // switches to the all-dates list (newest date first).
                bool listMode = AllDatesMode();

                try
                {
                    dgvQueue.DataSource = listMode
                        ? BillingRepository.GetList(txtQueueSearch.Text, chkAllBalances.Checked, chkPaidList.Checked)
                        : BillingRepository.GetQueue(dtpQueueDate.Value.Date);
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                dgvQueue.ClearSelection();

                if (keepAppointmentId > 0)
                {
                    GridHelper.SelectRowById(dgvQueue, "colQueueAppointmentId", keepAppointmentId);
                    appointmentId = keepAppointmentId;
                }

                if (appointmentId <= 0 && dgvQueue.Rows.Count > 0)
                {
                    dgvQueue.Rows[0].Selected = true;
                    appointmentId = Db.Get<int>(dgvQueue.Rows[0].Cells["colQueueAppointmentId"].Value);
                }
            }
            finally
            {
                _loading = false;
            }

            // The two lists are counted in their own words: the day queue or every open balance.
            lblQueueCount.Text = QueueCountText(dgvQueue.Rows.Count);

            // The date picker only makes sense for the day queue.
            dtpQueueDate.Enabled = !AllDatesMode();
            btnQueueToday.Enabled = dtpQueueDate.Enabled;

            if (appointmentId > 0) ShowBill(appointmentId);
            else ClearBill();
        }

        /// <summary>
        /// True when the grid must list the appointments of ANY date: something was typed in the
        /// search box, or at least one of the two filters is ticked. Otherwise the grid is the
        /// plain day queue of dtpQueueDate.
        /// </summary>
        private bool AllDatesMode()
        {
            return txtQueueSearch.Text.Trim().Length > 0 || chkAllBalances.Checked || chkPaidList.Checked;
        }

        /// <summary>
        /// The count line in the words of the mode that is shown ("12 in the queue",
        /// "3 unpaid / partial bills for 'maria'", "5 completed and paid bills (all dates)").
        /// </summary>
        private string QueueCountText(int rows)
        {
            if (!AllDatesMode()) return rows + " in the queue";

            string term = txtQueueSearch.Text.Trim();
            // POLISH: the wording of the count line.
            string what;
            if (chkAllBalances.Checked && chkPaidList.Checked) what = "appointments";
            else if (chkAllBalances.Checked) what = "unpaid / partial bills";
            else if (chkPaidList.Checked) what = "completed and paid bills";
            else what = "appointments";

            string text = rows + " " + what;
            if (term.Length > 0) text += " for '" + term + "'";
            else text += " (all dates)";

            return text;
        }

        private int CurrentAppointmentId()
        {
            return GridHelper.GetSelectedId(dgvQueue, "colQueueAppointmentId");
        }

        /// <summary>
        /// Opens this page on one appointment (used when a row of the Appointments page is double
        /// clicked). A cancelled or rescheduled appointment has nothing to pay, so it is only
        /// announced. The date picker is moved without loading twice: _loading swallows the change
        /// events of the tick and the picker, and LoadQueue runs once at the end with that
        /// appointment as the row to select.
        /// </summary>
        public void ShowAppointment(int appointmentId)
        {
            if (appointmentId <= 0) return;

            DataRow row;
            try
            {
                row = BillingRepository.GetAppointmentStatus(appointmentId);
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

            string status = Db.Get<string>(row["status"]);
            if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(status, "Rescheduled", StringComparison.OrdinalIgnoreCase))
            {
                Messages.Info("This appointment was cancelled, so there is nothing to pay.");
                return;
            }

            DateTime date = Db.Get<DateTime>(row["appointment_date"]).Date;

            _loading = true;
            try
            {
                // The queue goes back to its default day mode, so a search or a tick leaves no trace.
                txtQueueSearch.Text = string.Empty;
                chkAllBalances.Checked = false;
                chkPaidList.Checked = false;

                if (date >= dtpQueueDate.MinDate && date <= dtpQueueDate.MaxDate) dtpQueueDate.Value = date;
                else dtpQueueDate.Value = DateTime.Today;
            }
            finally
            {
                _loading = false;
            }

            LoadQueue(appointmentId);
        }

        // ---------------------------------------------------------------- the bill card

        /// <summary>Fills the bill card with the bill of one appointment (0 clears it).</summary>
        private void ShowBill(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                ClearBill();
                return;
            }

            DataRow header;
            try
            {
                header = BillingRepository.GetBillHeader(appointmentId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (header == null)
            {
                ClearBill();
                return;
            }

            _bill = header;

            string status = Db.Get<string>(header["appointment_status"]);

            lblBillClient.Text = Db.Get<string>(header["client_name"], "-");

            lblBillSchedule.Text = "Schedule: " +
                Db.Get<DateTime>(header["appointment_date"]).ToString("MMM dd, yyyy") + "   " +
                Clock(ReadTime(header["start_time"])) + " - " + Clock(ReadTime(header["end_time"])) +
                "   (" + status + ")";

            _loading = true;
            try
            {
                try
                {
                    dgvBillLines.DataSource = BillingRepository.GetBillLines(appointmentId);
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                SelectPromo(Db.Get<int>(header["promo_id"]));
            }
            finally
            {
                _loading = false;
            }

            ShowTotals(header);
            UpdateControls(header);
        }

        /// <summary>The five money rows under the lines.</summary>
        private void ShowTotals(DataRow header)
        {
            lblSubtotal.Text = Money.Text(Db.Get<decimal>(header["subtotal"]));
            lblDiscount.Text = Money.Text(Db.Get<decimal>(header["discount_amount"]));
            lblPaid.Text = Money.Text(Db.Get<decimal>(header["paid_so_far"]));
            lblTotal.Text = Money.Text(Db.Get<decimal>(header["total_amount"]));
            lblBalance.Text = Money.Text(Db.Get<decimal>(header["balance_due"]));
        }

        /// <summary>
        /// The promo note and the two buttons. A promotion may only be chosen while the bill has
        /// none, nothing has been paid and the appointment is not finished: a promotion is paid in
        /// full, so it can never be added on top of an installment.
        /// </summary>
        private void UpdateControls(DataRow header)
        {
            int promoId = Db.Get<int>(header["promo_id"]);
            decimal paid = Db.Get<decimal>(header["paid_so_far"]);
            string status = Db.Get<string>(header["appointment_status"]);

            bool completed = string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase);
            bool cancelled = string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase);

            cboPromo.Enabled = promoId <= 0 && paid <= 0m && !completed && !cancelled;
            dgvBillLines.ReadOnly = completed || cancelled;

            if (promoId > 0)
            {
                lblPromoNote.Text = "Promotion: " + Db.Get<string>(header["promo_name"], "-") +
                    ", -" + Money.Text(Db.Get<decimal>(header["discount_amount"])) +
                    ". A promotion is always paid in full.";
            }
            else if (paid > 0m)
            {
                lblPromoNote.Text = "No promo. A promo can no longer be added because money was already paid.";
            }
            else if (completed || cancelled)
            {
                lblPromoNote.Text = "No promo. This appointment is " + status.ToLowerInvariant() + ".";
            }
            else
            {
                lblPromoNote.Text = "No promo applied. Applying one recalculates the total and needs full payment.";
            }

            btnCollect.Enabled = !completed && !cancelled;

            // A payment can be taken at any time on a bill that still owes something.
            btnTakePayment.Enabled = !completed && !cancelled &&
                Db.Get<decimal>(header["balance_due"]) > 0m;
        }

        /// <summary>Empties the bill card and switches off both buttons.</summary>
        private void ClearBill()
        {
            _bill = null;

            lblBillClient.Text = "Select an appointment";
            lblBillSchedule.Text = "Schedule: -";
            lblPromoNote.Text = "No promo applied.";

            _loading = true;
            try
            {
                dgvBillLines.DataSource = null;
                cboPromo.Enabled = false;
                SelectPromo(0);
            }
            finally
            {
                _loading = false;
            }

            lblSubtotal.Text = Money.Text(0m);
            lblDiscount.Text = Money.Text(0m);
            lblPaid.Text = Money.Text(0m);
            lblTotal.Text = Money.Text(0m);
            lblBalance.Text = Money.Text(0m);

            btnCollect.Enabled = false;
            btnTakePayment.Enabled = false;
        }

        // ---------------------------------------------------------------- checkout actions

        private void btnCollect_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0 || _bill == null) return;

            string status = Db.Get<string>(_bill["appointment_status"]);
            if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                Messages.Warn("This appointment is already " + status.ToLowerInvariant() + ".");
                return;
            }

            if (Db.Get<DateTime>(_bill["appointment_date"]).Date > DateTime.Today)
            {
                Messages.Warn("This appointment is on a future date, so it cannot be checked out yet.");
                return;
            }

            // Every service line needs the person who performed it: the commission and the
            // treatment history are written for that person.
            Dictionary<int, int> technicians = new Dictionary<int, int>();
            int missing = 0;

            foreach (DataGridViewRow row in dgvBillLines.Rows)
            {
                if (row.IsNewRow) continue;

                int lineId = Db.Get<int>(row.Cells["colLineId"].Value);
                int technicianId = Db.Get<int>(row.Cells["colLineTech"].Value);

                if (lineId <= 0) continue;

                if (technicianId <= 0) missing++;
                else technicians[lineId] = technicianId;
            }

            if (missing > 0)
            {
                Messages.Warn("Choose the attending staff member for every service line first " +
                    "(" + missing + " line(s) still say 'Select staff').");
                return;
            }

            decimal balance = Db.Get<decimal>(_bill["balance_due"]);
            string clientName = Db.Get<string>(_bill["client_name"], "-");

            string method = string.Empty;
            string reference = string.Empty;
            bool verified = false;
            decimal tendered = 0m;

            if (balance > 0m)
            {
                using (PaymentForm form = new PaymentForm())
                {
                    form.AmountDue = balance;
                    form.ClientName = clientName;

                    if (form.ShowDialog(this) != DialogResult.OK) return;

                    method = form.Method;
                    reference = form.Reference;
                    verified = form.Verified;
                    tendered = form.Tendered;
                }
            }
            else
            {
                if (!Messages.Confirm("Nothing is left to pay for " + clientName +
                    ". Complete this appointment?"))
                {
                    return;
                }
            }

            try
            {
                BillingRepository.Checkout(appointmentId, technicians, method, reference, verified, tendered);
            }
            catch (InvalidOperationException ex)
            {
                // The repository spells out the rule it could not apply, so it is a warning.
                Messages.Warn(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            // The desk copies this message onto the paper receipt, so it names the amount paid,
            // the method, the change (cash only) and the balance that is still due.
            string done = "The appointment of " + clientName + " is completed.";
            if (balance > 0m)
            {
                done += " Paid " + Money.Text(balance) + " by " + method + ".";
            }
            else
            {
                done += " Nothing was left to pay.";
            }

            if (string.Equals(method, "Cash", StringComparison.OrdinalIgnoreCase) && tendered > balance)
            {
                done += " Change: " + Money.Text(tendered - balance) + ".";
            }

            done += " Balance still due: " + Money.Text(0m) + ".";

            Messages.Info(done);

            LoadQueue(appointmentId);
        }

        /// <summary>
        /// Takes a payment on its own (no completion): a down payment at any time, a part of the
        /// balance or the rest of it. The queue is filled again with the same appointment selected.
        /// </summary>
        private void btnTakePayment_Click(object sender, EventArgs e)
        {
            int appointmentId = CurrentAppointmentId();
            if (appointmentId <= 0) return;

            PaymentForm.TakePayment(this, appointmentId, "Payment - appointment #" + appointmentId);

            LoadQueue(appointmentId);
        }

        private void dgvBillLines_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // The staff drop-down only reports a change once the edit is committed.
            if (dgvBillLines.IsCurrentCellDirty)
            {
                dgvBillLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvBillLines_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0) return;

            // Choosing a different staff member changes who earns the commission, but not the
            // money of the bill, so the totals do not need to be recalculated here.
        }

        // ---------------------------------------------------------------- promo

        private void cboPromo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading || _bill == null) return;

            int previousId = Db.Get<int>(_bill["promo_id"]);
            int selectedId = SelectedPromoId();

            if (selectedId == previousId) return;

            int appointmentId = Db.Get<int>(_bill["appointment_id"]);

            string message;
            try
            {
                message = BillingRepository.ApplyPromo(appointmentId, selectedId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                SelectPromo(previousId);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                Messages.Warn(message);
                SelectPromo(previousId);
                return;
            }

            Messages.Info("The promotion was applied. The whole total is now due.");
            ShowBill(appointmentId);
        }

        // ---------------------------------------------------------------- queue and transactions

        private void dtpQueueDate_ValueChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadQueue(0);
        }

        private void btnQueueToday_Click(object sender, EventArgs e)
        {
            _loading = true;
            try { dtpQueueDate.Value = DateTime.Today; }
            finally { _loading = false; }

            LoadQueue(0);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadQueue(CurrentAppointmentId());
        }

        private void chkAllBalances_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadQueue(0);
        }

        /// <summary>The second filter ticks freely (no mutual exclusion) and reloads right away.</summary>
        private void chkPaidList_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadQueue(0);
        }

        /// <summary>
        /// Typing in the search box reloads the list at once. The appointment that is already
        /// selected stays selected while it is still listed, so the bill card does not jump away.
        /// </summary>
        private void txtQueueSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadQueue(CurrentAppointmentId());
        }

        private void dgvQueue_SelectionChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            int appointmentId = CurrentAppointmentId();
            if (appointmentId > 0) ShowBill(appointmentId);
            else ClearBill();
        }

        private void dgvQueue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string column = dgvQueue.Columns[e.ColumnIndex].Name;
            if (column != "colQueueStatus" && column != "colQueuePayment") return;

            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            // A selected row keeps white text on its highlight, where the colour is unreadable.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>
        /// Loads the payments of the chosen range. Staff are locked to today (the dates are
        /// disabled), an Admin may search any month.
        /// </summary>
        private void LoadTransactions()
        {
            string method = cboMethod.SelectedItem == null ? "All" : cboMethod.SelectedItem.ToString();

            try
            {
                dgvTransactions.DataSource = BillingRepository.GetTransactions(
                    dtpFrom.Value.Date, dtpTo.Value.Date, txtSearch.Text, method);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            dgvTransactions.ClearSelection();

            decimal total = 0m;
            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                if (row.IsNewRow) continue;
                total += Db.Get<decimal>(row.Cells["colTranPaid"].Value);
            }

            lblTotals.Text = dgvTransactions.Rows.Count +
                (dgvTransactions.Rows.Count == 1 ? " transaction - " : " transactions - ") +
                Money.Text(total);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                Messages.Warn("The 'From' date must be before the 'To' date.");
                return;
            }

            LoadTransactions();
        }

        private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvTransactions.Columns[e.ColumnIndex].Name != "colTranStatus") return;

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
