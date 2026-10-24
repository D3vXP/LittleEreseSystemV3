using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// Reports page (Admin only): sales, top services, commissions and appointment counters for a
    /// chosen period. Every number comes from ReportRepository; this screen only picks the range,
    /// shows the tables and exports or prints the one the owner is looking at.
    /// </summary>
    public partial class ucReports : UserControl
    {
        // Money in the project format: peso sign + #,##0.00.
        private const string MoneyFormat = "\u20B1#,##0.00";

        // How many services the "Top Services" tab lists.
        private const int TopServiceCount = 25;

        // True while the screen sets its own controls, so the change events do not run twice.
        private bool _loading;

        public ucReports()
        {
            InitializeComponent();
        }

        private void ucReports_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            // Sales figures are the owner's business. MainForm already hides this page for a Staff
            // user; this is the second check the rules ask for.
            if (!Session.IsAdmin)
            {
                ClearAll();
                // The message only appears for a signed-in user: the smoke test and the preview
                // tool build every screen without one, and a modal box would stop them.
                if (Session.IsLoggedIn) Messages.Warn("Reports are only available to the owner account.");
                return;
            }

            Theme.StyleGrid(dgvSales);
            Theme.StyleGrid(dgvSalesByMethod);
            Theme.StyleGrid(dgvTopServices);
            Theme.StyleGrid(dgvCommissionReport);
            Theme.StyleGrid(dgvAppointmentStatus);
            Theme.StyleGrid(dgvAppointmentSource);

            Theme.StylePrimaryButton(btnGenerate);
            Theme.StyleSecondaryButton(btnExport);
            Theme.StyleSecondaryButton(btnPrint);

            // The Designer only says "N2"; money shows the peso sign at runtime as everywhere else.
            colSalesGross.DefaultCellStyle.Format = MoneyFormat;
            colSalesDiscount.DefaultCellStyle.Format = MoneyFormat;
            colSalesNet.DefaultCellStyle.Format = MoneyFormat;
            colMethodAmount.DefaultCellStyle.Format = MoneyFormat;
            colTopRevenue.DefaultCellStyle.Format = MoneyFormat;
            colRepTotal.DefaultCellStyle.Format = MoneyFormat;
            colRepUnpaid.DefaultCellStyle.Format = MoneyFormat;
            colRepPaid.DefaultCellStyle.Format = MoneyFormat;

            _loading = true;
            try
            {
                // "This Month" is the preset the owner reaches for most often.
                cboPreset.SelectedIndex = 2;
            }
            finally
            {
                _loading = false;
            }

            ApplyPreset();
            Generate();
        }

        // ---------------------------------------------------------------- the period

        private void cboPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            ApplyPreset();
        }

        /// <summary>
        /// Moves the two date pickers to the chosen preset. Only "Custom" lets the owner type a
        /// date; the other presets keep the pickers locked, so a range always matches the caption.
        /// </summary>
        private void ApplyPreset()
        {
            string preset = SelectedPreset();
            DateTime today = DateTime.Today;

            DateTime from = today;
            DateTime to = today;

            if (preset == "This Week")
            {
                // The week starts on Monday, as in the shop's paper calendar.
                int offset = ((int)today.DayOfWeek + 6) % 7;
                from = today.AddDays(-offset);
            }
            else if (preset == "This Month")
            {
                from = new DateTime(today.Year, today.Month, 1);
            }
            else if (preset == "Last Month")
            {
                DateTime firstOfThisMonth = new DateTime(today.Year, today.Month, 1);
                from = firstOfThisMonth.AddMonths(-1);
                to = firstOfThisMonth.AddDays(-1);
            }

            _loading = true;
            try
            {
                dtpFrom.Value = from;
                dtpTo.Value = to;

                bool custom = preset == "Custom";
                dtpFrom.Enabled = custom;
                dtpTo.Enabled = custom;
            }
            finally
            {
                _loading = false;
            }
        }

        private string SelectedPreset()
        {
            return cboPreset.SelectedItem == null ? "This Month" : cboPreset.SelectedItem.ToString();
        }

        // ---------------------------------------------------------------- generating

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                Messages.Warn("The 'From' date must be before the 'To' date.");
                return;
            }

            Generate();
        }

        /// <summary>
        /// Loads all four tabs at once, so switching tabs is instant and the numbers of every tab
        /// always describe the same period.
        /// </summary>
        private void Generate()
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            try
            {
                dgvSales.DataSource = ReportRepository.GetSalesByCategory(from, to);
                dgvSalesByMethod.DataSource = ReportRepository.GetPaymentsByMethod(from, to);
                dgvTopServices.DataSource = ReportRepository.GetTopServices(from, to, TopServiceCount);
                dgvCommissionReport.DataSource = ReportRepository.GetCommissionReport(from, to);
                dgvAppointmentStatus.DataSource = ReportRepository.GetAppointmentStatusCounts(from, to);
                dgvAppointmentSource.DataSource = ReportRepository.GetAppointmentSourceCounts(from, to);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            dgvSales.ClearSelection();
            dgvSalesByMethod.ClearSelection();
            dgvTopServices.ClearSelection();
            dgvCommissionReport.ClearSelection();
            dgvAppointmentStatus.ClearSelection();
            dgvAppointmentSource.ClearSelection();

            FillTotals();
        }

        /// <summary>
        /// The four headline numbers. Every label reads "caption  amount", so an empty period shows
        /// a zero instead of a blank line.
        /// </summary>
        private void FillTotals()
        {
            decimal netSales = 0m;
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;
                netSales += Db.Get<decimal>(row.Cells["colSalesNet"].Value);
            }

            decimal commission = 0m;
            foreach (DataGridViewRow row in dgvCommissionReport.Rows)
            {
                if (row.IsNewRow) continue;
                commission += Db.Get<decimal>(row.Cells["colRepTotal"].Value);
            }

            lblSalesTotal.Text = "Net sales " + netSales.ToString(MoneyFormat);
            lblCommissionTotal.Text = "Total commission " + commission.ToString(MoneyFormat);

            int completed = 0;
            int cancelled = 0;
            int rescheduled = 0;
            int total = 0;

            foreach (DataGridViewRow row in dgvAppointmentStatus.Rows)
            {
                if (row.IsNewRow) continue;

                string label = Db.Get<string>(row.Cells["colStatusName"].Value);
                int count = Db.Get<int>(row.Cells["colStatusTotal"].Value);

                // The four appointment statuses add up to the headline number; a reschedule is a
                // history event and is shown in its own card, not counted twice.
                if (string.Equals(label, "Rescheduled", StringComparison.OrdinalIgnoreCase)) rescheduled = count;
                else total += count;

                if (string.Equals(label, "Completed", StringComparison.OrdinalIgnoreCase)) completed = count;
                if (string.Equals(label, "Cancelled", StringComparison.OrdinalIgnoreCase)) cancelled = count;
            }

            lblTotalAppointments.Text = total.ToString();
            lblCompletedCount.Text = completed.ToString();
            lblCancelledCount.Text = cancelled.ToString();
            lblRescheduledCount.Text = rescheduled.ToString();
        }

        // ---------------------------------------------------------------- export and print

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridView grid = GridForSelectedTab();
            CsvExporter.Export(grid, FileNameForSelectedTab());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GridPrinter.Print(GridForSelectedTab(), TitleForSelectedTab());
        }

        /// <summary>
        /// The grid the owner is looking at. The Sales tab has two grids and the totals one is a
        /// detail of the other, so the sales grid itself is the one exported and printed.
        /// </summary>
        private DataGridView GridForSelectedTab()
        {
            TabPage page = tabReports.SelectedTab;
            if (page == null) return dgvSales;

            if (page == tabTopServices) return dgvTopServices;
            if (page == tabCommission) return dgvCommissionReport;
            if (page == tabAppointments) return dgvAppointmentStatus;

            return dgvSales;
        }

        private string TitleForSelectedTab()
        {
            TabPage page = tabReports.SelectedTab;
            if (page == null) return "Sales report";

            if (page == tabTopServices) return "Top services";
            if (page == tabCommission) return "Commission report";
            if (page == tabAppointments) return "Appointments report";

            return "Sales report";
        }

        private string FileNameForSelectedTab()
        {
            TabPage page = tabReports.SelectedTab;
            string caption = page == null ? "sales" : page.Text.Trim().ToLowerInvariant().Replace(" ", "-");

            return caption + "-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" +
                dtpTo.Value.ToString("yyyyMMdd");
        }

        // ---------------------------------------------------------------- cells and roles

        private void tabReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            // All four tabs are loaded together by Generate(), so switching a tab must fetch nothing.
            // The comment stays because the Designer wires the event and a reader looks for it here.
            if (_loading) return;
        }

        private void dgvReports_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView grid = sender as DataGridView;
            if (grid == null || grid.Columns[e.ColumnIndex] == null) return;

            // Money is already formatted by the column style; this makes sure the text is right
            // aligned even if a column style was ever reset.
            string column = grid.Columns[e.ColumnIndex].Name;

            if (column == "colSalesGross" || column == "colSalesDiscount" || column == "colSalesNet" ||
                column == "colMethodAmount" || column == "colTopRevenue" ||
                column == "colRepTotal" || column == "colRepUnpaid" || column == "colRepPaid")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // A selected row keeps white text on its highlight in every grid.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>Empties the page for a user who is not allowed to see it.</summary>
        private void ClearAll()
        {
            _loading = true;
            try
            {
                dgvSales.DataSource = null;
                dgvSalesByMethod.DataSource = null;
                dgvTopServices.DataSource = null;
                dgvCommissionReport.DataSource = null;
                dgvAppointmentStatus.DataSource = null;
                dgvAppointmentSource.DataSource = null;

                cboPreset.Enabled = false;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                btnGenerate.Enabled = false;
                btnExport.Enabled = false;
                btnPrint.Enabled = false;
            }
            finally
            {
                _loading = false;
            }

            lblSalesTotal.Text = "Net sales " + 0m.ToString(MoneyFormat);
            lblCommissionTotal.Text = "Total commission " + 0m.ToString(MoneyFormat);
            lblTotalAppointments.Text = "0";
            lblCompletedCount.Text = "0";
            lblCancelledCount.Text = "0";
            lblRescheduledCount.Text = "0";
        }
    }
}
