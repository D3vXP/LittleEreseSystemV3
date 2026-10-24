using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// Commissions page (Admin only): the money each staff member earned in a period, the rows
    /// behind it and the payout action. The 10 % rule itself lives in BillingRepository.Checkout;
    /// this screen only groups, shows and marks as paid.
    /// </summary>
    public partial class ucCommissions : UserControl
    {
        // Money in the project format: peso sign + #,##0.00.
        private const string MoneyFormat = "\u20B1#,##0.00";

        // True while the screen fills its own controls, so the selection events do not run twice.
        private bool _loading;

        // The staff id the detail grid is filtered to (0 = every staff member).
        private int _detailUserId;

        public ucCommissions()
        {
            InitializeComponent();
        }

        private void ucCommissions_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            // Managing payouts and seeing other people's earnings is an Admin job. MainForm already
            // hides this page for a Staff user; this is the second check the rules ask for.
            if (!Session.IsAdmin)
            {
                ClearAll();
                // The message only appears for a signed-in user: the smoke test and the preview
                // tool build every screen without one, and a modal box would stop them.
                if (Session.IsLoggedIn) Messages.Warn("Commissions are only available to the owner account.");
                return;
            }

            Theme.StyleGrid(dgvSummary);
            Theme.StyleGrid(dgvDetails);

            // The detail grid must allow several rows to be marked paid at once.
            dgvDetails.MultiSelect = true;

            Theme.StylePrimaryButton(btnSearch);
            Theme.StylePrimaryButton(btnMarkPaid);
            Theme.StyleSecondaryButton(btnExport);
            Theme.StyleSecondaryButton(btnPrint);

            // The Designer only says "N2"; money shows the peso sign at runtime as everywhere else.
            colSumTotal.DefaultCellStyle.Format = MoneyFormat;
            colSumUnpaid.DefaultCellStyle.Format = MoneyFormat;
            colSumPaid.DefaultCellStyle.Format = MoneyFormat;
            colDetBase.DefaultCellStyle.Format = MoneyFormat;
            colDetAmount.DefaultCellStyle.Format = MoneyFormat;

            _loading = true;
            try
            {
                dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtpTo.Value = DateTime.Today;

                LoadStaffCombo();

                // Unpaid first: the usual reason to open this page is "who do I still owe?".
                cboPayout.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
            finally
            {
                _loading = false;
            }

            LoadAll(0);
        }

        // ---------------------------------------------------------------- the filters

        /// <summary>"All staff" (id 0) plus every Active technician.</summary>
        private void LoadStaffCombo()
        {
            DataTable items = new DataTable();
            items.Columns.Add("user_id", typeof(int));
            items.Columns.Add("full_name", typeof(string));
            items.Rows.Add(0, "All staff");

            foreach (DataRow row in UserRepository.GetActiveStaff().Rows)
            {
                items.Rows.Add(Db.Get<int>(row["user_id"]), Db.Get<string>(row["full_name"]));
            }

            cboStaff.DataSource = items;
            cboStaff.DisplayMember = "full_name";
            cboStaff.ValueMember = "user_id";
            cboStaff.SelectedIndex = 0;
        }

        private int SelectedUserId()
        {
            if (cboStaff.SelectedValue == null) return 0;
            return Db.Get<int>(cboStaff.SelectedValue);
        }

        private string SelectedPayout()
        {
            return cboPayout.SelectedItem == null ? "All" : cboPayout.SelectedItem.ToString();
        }

        // ---------------------------------------------------------------- loading

        /// <summary>
        /// Reloads the summary grid, the three cards and the details of the staff member that was
        /// selected before (keepUserId 0 = no staff row selected, so all details show).
        /// </summary>
        private void LoadAll(int keepUserId)
        {
            _loading = true;
            try
            {
                DataTable summary;
                try
                {
                    summary = CommissionRepository.GetSummary(dtpFrom.Value.Date, dtpTo.Value.Date,
                        SelectedUserId(), SelectedPayout());
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                dgvSummary.DataSource = summary;
                dgvSummary.ClearSelection();

                if (keepUserId > 0) GridHelper.SelectRowById(dgvSummary, "colSumUserId", keepUserId);

                _detailUserId = keepUserId;
            }
            finally
            {
                _loading = false;
            }

            LoadSummaryCards();
            LoadDetails();
        }

        /// <summary>
        /// The three cards at the top. They add up the summary grid (not the details), so the owner
        /// sees the whole period even while one staff member is selected below.
        /// </summary>
        private void LoadSummaryCards()
        {
            decimal total = 0m;
            decimal unpaid = 0m;
            decimal paid = 0m;

            foreach (DataGridViewRow row in dgvSummary.Rows)
            {
                if (row.IsNewRow) continue;

                total += Db.Get<decimal>(row.Cells["colSumTotal"].Value);
                unpaid += Db.Get<decimal>(row.Cells["colSumUnpaid"].Value);
                paid += Db.Get<decimal>(row.Cells["colSumPaid"].Value);
            }

            lblTotalEarned.Text = total.ToString(MoneyFormat);
            lblUnpaid.Text = unpaid.ToString(MoneyFormat);
            lblPaid.Text = paid.ToString(MoneyFormat);
        }

        /// <summary>
        /// The single commission rows: the selected staff member only while a summary row is
        /// selected, otherwise every staff member of the period.
        /// </summary>
        private void LoadDetails()
        {
            try
            {
                dgvDetails.DataSource = CommissionRepository.GetDetails(dtpFrom.Value.Date,
                    dtpTo.Value.Date, _detailUserId, SelectedPayout());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            dgvDetails.ClearSelection();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                Messages.Warn("The 'From' date must be before the 'To' date.");
                return;
            }

            // A new search starts from the whole period, because the previously selected staff
            // member may have no rows in the new one.
            LoadAll(0);
        }

        private void dgvSummary_SelectionChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            // Selecting a staff row narrows the details; a click in the empty area below the rows
            // (or a new search) shows everybody again.
            int userId = GridHelper.GetSelectedId(dgvSummary, "colSumUserId");
            if (userId == _detailUserId) return;

            _detailUserId = userId;
            LoadDetails();
        }

        // ---------------------------------------------------------------- mark as paid

        private void btnMarkPaid_Click(object sender, EventArgs e)
        {
            List<int> commissionIds = new List<int>();
            decimal total = 0m;
            int alreadyPaid = 0;

            foreach (DataGridViewRow row in dgvDetails.SelectedRows)
            {
                if (row.IsNewRow) continue;

                string status = Db.Get<string>(row.Cells["colDetStatus"].Value);
                if (string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase))
                {
                    alreadyPaid++;
                    continue;
                }

                int id = Db.Get<int>(row.Cells["colDetCommissionId"].Value);
                if (id <= 0) continue;

                commissionIds.Add(id);
                total += Db.Get<decimal>(row.Cells["colDetAmount"].Value);
            }

            if (commissionIds.Count == 0)
            {
                Messages.Warn(alreadyPaid > 0
                    ? "The selected commission(s) are already marked as paid."
                    : "Select the unpaid commission rows you want to mark as paid.");
                return;
            }

            string question = "Mark " + commissionIds.Count + " commission(s) totalling " +
                total.ToString(MoneyFormat) + " as paid?";
            if (alreadyPaid > 0)
            {
                question += Environment.NewLine + alreadyPaid +
                    " selected row(s) are already paid and will be skipped.";
            }

            if (!Messages.Confirm(question)) return;

            int updated;
            try
            {
                updated = CommissionRepository.MarkPaid(commissionIds);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            ActivityRepository.Log(Session.UserId, "Commission payout", "Commissions",
                updated + " commission(s) marked as paid, " + total.ToString(MoneyFormat));

            Messages.Info(updated + " commission(s) marked as paid.");

            // The same staff member stays selected, so the owner can carry on with the next one.
            LoadAll(_detailUserId);
        }

        // ---------------------------------------------------------------- export and print

        private void btnExport_Click(object sender, EventArgs e)
        {
            CsvExporter.Export(dgvDetails, "commissions-" + RangeText());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GridPrinter.Print(dgvDetails, "Staff commissions");
        }

        /// <summary>The range as "yyyyMMdd-yyyyMMdd" for the file name of an export.</summary>
        private string RangeText()
        {
            return dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd");
        }

        // ---------------------------------------------------------------- cells and roles

        private void dgvDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvDetails.Columns[e.ColumnIndex].Name != "colDetStatus") return;

            string status = e.Value.ToString();
            e.CellStyle.ForeColor = string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase)
                ? Theme.Success
                : Theme.Warning;

            // A selected row keeps white text on its highlight, where the colour is unreadable.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>Empties the page for a user who is not allowed to see it.</summary>
        private void ClearAll()
        {
            _loading = true;
            try
            {
                dgvSummary.DataSource = null;
                dgvDetails.DataSource = null;
                cboStaff.Enabled = false;
                cboPayout.Enabled = false;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                btnSearch.Enabled = false;
                btnMarkPaid.Enabled = false;
                btnExport.Enabled = false;
                btnPrint.Enabled = false;
            }
            finally
            {
                _loading = false;
            }

            lblTotalEarned.Text = 0m.ToString(MoneyFormat);
            lblUnpaid.Text = 0m.ToString(MoneyFormat);
            lblPaid.Text = 0m.ToString(MoneyFormat);
        }
    }
}
