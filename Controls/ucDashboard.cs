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
    /// The Dashboard page: five KPI cards on top, the month calendar on the left and the
    /// "Today" / "Selected Day" lists on the right.
    ///
    /// It only READS: every number comes from ReportRepository / AppointmentRepository, so the
    /// Dashboard can never disagree with the modules it summarises.
    /// </summary>
    public partial class ucDashboard : UserControl
    {
        // The day the "Selected Day" tab is showing (the calendar raises it on every click).
        private DateTime _selectedDay = DateTime.Today;

        // Refreshes the cards and the calendar while the page stays open, so the desk sees a
        // booking that someone else just took.
        private Timer _refreshTimer;

        public ucDashboard()
        {
            InitializeComponent();
        }

        // ------------------------------------------------------------------
        // Load
        // ------------------------------------------------------------------

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            StyleControls();
            LoadIcons();
            ApplyRoleLayout();
            HookCalendar();

            RefreshAll(true);

            StartRefreshTimer();
        }

        /// <summary>Runtime styling, which the Designer could not do.</summary>
        private void StyleControls()
        {
            Theme.StyleGrid(dgvToday);
            Theme.StyleGrid(dgvDay);

            // The pictures are 30 px inside a 36 px box, so they are centred instead of glued
            // to the top left corner.
            picCardToday.SizeMode = PictureBoxSizeMode.CenterImage;
            picCardRemaining.SizeMode = PictureBoxSizeMode.CenterImage;
            picCardCompleted.SizeMode = PictureBoxSizeMode.CenterImage;
            picCardSales.SizeMode = PictureBoxSizeMode.CenterImage;
            picCardPackages.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        /// <summary>The five card icons. A missing file only leaves an empty box.</summary>
        private void LoadIcons()
        {
            picCardToday.Image = IconHelper.Load("CalendarDarkIcon", 30);
            picCardRemaining.Image = IconHelper.Load("ClockIcon", 30);
            picCardCompleted.Image = IconHelper.Load("CheckIcon", 30);
            picCardSales.Image = IconHelper.Load("BillingIcon", 30);
            picCardPackages.Image = IconHelper.Load("ServicesIcon", 30);
        }

        /// <summary>
        /// Staff must not see sales amounts, so the Sales card is removed and the four remaining
        /// cards share the whole width instead of leaving a hole.
        /// </summary>
        private void ApplyRoleLayout()
        {
            if (Session.IsAdmin) return;

            pnlCardSales.Visible = false;

            // The hidden card still owns its column, so that column is squeezed to nothing and
            // the other four are widened from 20% to 25%.
            tlpKpi.ColumnStyles[3] = new ColumnStyle(SizeType.Absolute, 0F);

            tlpKpi.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 25F);
            tlpKpi.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 25F);
            tlpKpi.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 25F);
            tlpKpi.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 25F);
        }

        /// <summary>The calendar raises these events; the page decides what to do with them.</summary>
        private void HookCalendar()
        {
            calMonth.DaySelected += calMonth_DaySelected;
            calMonth.DayOpened += calMonth_DayOpened;
            calMonth.AppointmentOpened += calMonth_AppointmentOpened;
            calMonth.MonthChanged += calMonth_MonthChanged;
        }

        // ------------------------------------------------------------------
        // Refreshing
        // ------------------------------------------------------------------

        /// <summary>
        /// Reloads everything the page shows. showErrors is false for the timer, because a
        /// message box that pops up on its own every minute would make the app unusable.
        /// </summary>
        private void RefreshAll(bool showErrors)
        {
            DateTime today = DateTime.Today;

            try
            {
                ShowCards(today);
                ShowTodayGrid(today);
                ShowMonth(calMonth.VisibleMonth);
            }
            catch (Exception ex)
            {
                if (showErrors) Messages.DbError(ex);
            }
        }

        /// <summary>The five KPI cards. The sales amount is only asked for when it is allowed.</summary>
        private void ShowCards(DateTime today)
        {
            DataRow stats = ReportRepository.GetDashboardStats(today, Session.IsAdmin);

            lblCardTodayValue.Text = Db.Get<int>(stats["today_total"]).ToString();
            lblCardRemainingValue.Text = Db.Get<int>(stats["remaining"]).ToString();
            lblCardCompletedValue.Text = Db.Get<int>(stats["completed"]).ToString();
            lblCardPackagesValue.Text = Db.Get<int>(stats["active_packages"]).ToString();

            lblCardSalesValue.Text = Money(Db.Get<decimal>(stats["today_sales"]));
        }

        /// <summary>The "Today" tab: the appointments of today and its heading.</summary>
        private void ShowTodayGrid(DateTime today)
        {
            dgvToday.DataSource = AppointmentRepository.GetByDate(today, "All", string.Empty);

            // POLISH: the heading follows the prompt wording; shorten it here if the owner prefers.
            lblTodayHeading.Text = "Today - " + today.ToString("MMM dd, yyyy") +
                " (" + dgvToday.Rows.Count + " appointments)";
        }

        /// <summary>One whole month for the calendar (a single query, not one per day).</summary>
        private void ShowMonth(DateTime month)
        {
            DataTable appointments = AppointmentRepository.GetMonthAppointments(month.Year, month.Month);
            calMonth.LoadMonth(month, appointments);
        }

        // ------------------------------------------------------------------
        // Calendar events
        // ------------------------------------------------------------------

        /// <summary>A clicked day fills the "Selected Day" tab and switches to it.</summary>
        private void calMonth_DaySelected(DateTime date)
        {
            ShowSelectedDay(date);
            tabSide.SelectedTab = tabDay;
        }

        /// <summary>A double-clicked empty day opens that day in the Appointments page.</summary>
        private void calMonth_DayOpened(DateTime date)
        {
            OpenAppointmentsPage(date);
        }

        /// <summary>
        /// A double-clicked appointment line opens the Appointments page on the day of that
        /// appointment. The Appointments page has no public way to pre-select a single booking,
        /// so the right day is the closest it can get.
        /// </summary>
        private void calMonth_AppointmentOpened(int appointmentId)
        {
            DateTime date = DateOfAppointment(appointmentId);
            if (date == DateTime.MinValue) return;

            OpenAppointmentsPage(date);
        }

        /// <summary>The prev / next / today buttons: only the month data is reloaded.</summary>
        private void calMonth_MonthChanged(DateTime month)
        {
            try
            {
                ShowMonth(month);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>Fills the "Selected Day" grid and its heading.</summary>
        private void ShowSelectedDay(DateTime date)
        {
            try
            {
                dgvDay.DataSource = AppointmentRepository.GetByDate(date, "All", string.Empty);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            _selectedDay = date.Date;

            lblDayHeading.Text = date.ToString("MMMM dd, yyyy") + " - " +
                dgvDay.Rows.Count + " appointments";
        }

        // ------------------------------------------------------------------
        // Grids
        // ------------------------------------------------------------------

        /// <summary>Status is the last column of the "Today" grid and is shown in its status colour.</summary>
        private void dgvToday_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvToday.Columns[e.ColumnIndex].Name != "colTodayStatus") return;

            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            // A selected row keeps white text on its highlight, where the colour is unreadable.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>Status and Payment of the "Selected Day" grid both carry a status colour.</summary>
        private void dgvDay_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string column = dgvDay.Columns[e.ColumnIndex].Name;
            if (column != "colDayPayment") return;

            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            e.CellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>A double-clicked row goes to the same place as a double-clicked calendar line.</summary>
        private void dgvDay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int appointmentId = AppointmentIdOf(dgvDay, e.RowIndex);
            if (appointmentId <= 0) return;

            calMonth_AppointmentOpened(appointmentId);
        }

        private void btnOpenInAppointments_Click(object sender, EventArgs e)
        {
            OpenAppointmentsPage(_selectedDay);
        }

        // ------------------------------------------------------------------
        // Navigation
        // ------------------------------------------------------------------

        /// <summary>Asks the MainForm to show the Appointments page on that day.</summary>
        private void OpenAppointmentsPage(DateTime date)
        {
            MainForm main = FindForm() as MainForm;
            if (main == null) return;

            main.OpenAppointments(date);
        }

        /// <summary>The day of one appointment, or DateTime.MinValue when it cannot be read.</summary>
        private static DateTime DateOfAppointment(int appointmentId)
        {
            if (appointmentId <= 0) return DateTime.MinValue;

            try
            {
                DataRow row = AppointmentRepository.GetById(appointmentId);
                if (row == null) return DateTime.MinValue;

                return Db.Get<DateTime>(row["appointment_date"]).Date;
            }
            catch (Exception)
            {
                // Nothing to open when the row cannot be read; the click simply does nothing.
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// The appointment_id of a grid row. The id column is not on screen (it is not needed
        /// there), so the value is read from the bound row instead of from a hidden column.
        /// </summary>
        private static int AppointmentIdOf(DataGridView grid, int rowIndex)
        {
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count) return 0;

            DataRowView view = grid.Rows[rowIndex].DataBoundItem as DataRowView;
            if (view == null) return 0;

            return Db.Get<int>(view["appointment_id"]);
        }

        // ------------------------------------------------------------------
        // Timer
        // ------------------------------------------------------------------

        /// <summary>Refreshes the page once a minute while it is open.</summary>
        private void StartRefreshTimer()
        {
            if (_refreshTimer != null) return;

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 60000;   // 60 seconds
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();

            // A timer must never outlive its page. The control's Disposed event is used, so the
            // generated Dispose in the Designer file stays exactly as the Designer wrote it.
            Disposed += Dashboard_Disposed;
        }

        private void Dashboard_Disposed(object sender, EventArgs e)
        {
            DisposeRefreshTimer();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshAll(false);
        }

        /// <summary>Called by Dispose in the Designer: a timer must never outlive its page.</summary>
        private void DisposeRefreshTimer()
        {
            if (_refreshTimer == null) return;

            _refreshTimer.Stop();
            _refreshTimer.Tick -= RefreshTimer_Tick;
            _refreshTimer.Dispose();
            _refreshTimer = null;
        }

        // ------------------------------------------------------------------
        // Small helpers
        // ------------------------------------------------------------------

        /// <summary>Money in the project format: peso sign + #,##0.00.</summary>
        private static string Money(decimal amount)
        {
            return "\u20B1" + amount.ToString("N2");
        }
    }
}
