using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// A read-only month calendar used by the Dashboard. The coloured day cells are created at
    /// runtime (they come from the data), so tlpDays stays empty in the Designer.
    ///
    /// The control only shows the month: booking and rescheduling still happen in the
    /// Appointments module, which is why a day or an appointment only RAISES an event here.
    /// </summary>
    public partial class ucCalendarMonth : UserControl
    {
        // Six whole weeks, so the height of a month never jumps.
        private const int DaysInGrid = 42;

        // A cell is about 70 px tall, which has room for three single lines plus "+N more".
        private const int MaxLinesPerCell = 3;
        private const int MaxTooltipLines = 8;

        // The day starts to look busy at 20 appointments and is full at the daily limit.
        // POLISH: move BusyFrom if the owner wants the amber warning earlier or later.
        private const int BusyFrom = 20;

        // ---- Cell colours (light tints of the brand colours, so the dark text stays readable) ----
        private static readonly Color TodayBack = Color.FromArgb(0xF7, 0xF0, 0xE1);
        private static readonly Color SelectedBack = Color.FromArgb(0xEC, 0xDC, 0xBA);
        private static readonly Color HoverBack = Color.FromArgb(0xFA, 0xF6, 0xED);
        private static readonly Color BusyBack = Color.FromArgb(0xFC, 0xEB, 0xC8);   // amber: filling up
        private static readonly Color FullBack = Color.FromArgb(0xF9, 0xDE, 0xDB);   // red: full

        // The appointment rows of the month, grouped by day (one entry per day that has any).
        private readonly Dictionary<DateTime, List<DataRow>> _byDate = new Dictionary<DateTime, List<DataRow>>();

        // How many non-cancelled appointments each day has, for the capacity colours.
        private readonly Dictionary<DateTime, int> _activeCount = new Dictionary<DateTime, int>();

        // The 42 day cells built by BuildCells, so they can be disposed when the month changes.
        private readonly List<Panel> _cells = new List<Panel>();

        private ToolTip _tooltip;
        private Panel _hoverCell;

        private DateTime _visibleMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        private DateTime? _selectedDate;

        /// <summary>Raised when a day is clicked (that cell becomes the selected day).</summary>
        public event Action<DateTime> DaySelected;

        /// <summary>Raised when the empty part of a day is double-clicked.</summary>
        public event Action<DateTime> DayOpened;

        /// <summary>Raised when an appointment line of a day is double-clicked.</summary>
        public event Action<int> AppointmentOpened;

        /// <summary>Raised by the prev / next / today buttons, so the caller reloads the data.</summary>
        public event Action<DateTime> MonthChanged;

        public ucCalendarMonth()
        {
            InitializeComponent();
        }

        /// <summary>The month on screen, always the first day of that month.</summary>
        public DateTime VisibleMonth
        {
            get { return _visibleMonth; }
        }

        /// <summary>The clicked day, or null when the user has not picked one yet.</summary>
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
        }

        protected override void OnLoad(EventArgs e)
        {
            // The cells repaint while the pointer moves over the grid, so the whole control is
            // double buffered to stop the flicker.
            DoubleBuffered = true;
            base.OnLoad(e);
        }

        // ------------------------------------------------------------------
        // Public API
        // ------------------------------------------------------------------

        /// <summary>
        /// Shows one month. <paramref name="appointments"/> comes from
        /// AppointmentRepository.GetMonthAppointments (appointment_id, appointment_date,
        /// start_time_text, client_name, services, staff_names, status, payment_status).
        /// The 42 cells are built here from that data, never in the Designer.
        /// </summary>
        public void LoadMonth(DateTime month, DataTable appointments)
        {
            if (DesignTime.IsDesignMode) return;

            _visibleMonth = FirstDayOf(month);

            GroupAppointments(appointments);

            lblMonth.Text = _visibleMonth.ToString("MMMM yyyy");

            BuildCells();
            RefreshCells();
        }

        // ------------------------------------------------------------------
        // Building the grid
        // ------------------------------------------------------------------

        /// <summary>
        /// Throws the old cells away and builds 42 new ones: six weeks starting on the Sunday on
        /// or before the first day of the month.
        /// </summary>
        private void BuildCells()
        {
            tlpDays.SuspendLayout();
            try
            {
                DisposeCells();

                DateTime first = GridStart();

                for (int index = 0; index < DaysInGrid; index++)
                {
                    DateTime date = first.AddDays(index);

                    Panel cell = BuildCell(date);
                    _cells.Add(cell);

                    tlpDays.Controls.Add(cell, index % 7, index / 7);
                }
            }
            finally
            {
                tlpDays.ResumeLayout();
            }
        }

        /// <summary>One day cell: the day number, a count badge, the lines and a tooltip.</summary>
        private Panel BuildCell(DateTime date)
        {
            List<DataRow> rows = RowsFor(date);
            int count = ActiveCountFor(date);

            Panel cell = new Panel();
            cell.Dock = DockStyle.Fill;
            cell.Margin = new Padding(1);
            cell.Tag = date;                 // the day this cell shows
            cell.Paint += Cell_Paint;

            // ---- The head: day number on the left, count badge on the right ----
            Panel head = new Panel();
            head.Dock = DockStyle.Top;
            head.Height = 18;

            Label dayNumber = new Label();
            dayNumber.Dock = DockStyle.Fill;
            dayNumber.Padding = new Padding(4, 0, 0, 0);
            dayNumber.TextAlign = ContentAlignment.MiddleLeft;
            dayNumber.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular);
            dayNumber.Text = date.Day.ToString();

            Label badge = new Label();
            badge.Dock = DockStyle.Right;
            badge.Width = 22;
            badge.TextAlign = ContentAlignment.MiddleCenter;
            badge.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Regular);
            badge.ForeColor = Color.White;
            badge.BackColor = Theme.PrimaryLight;
            badge.Text = count.ToString();
            badge.Visible = count > 0;

            head.Controls.Add(dayNumber);
            head.Controls.Add(badge);

            // ---- The appointment lines, stacked top down ----
            // A plain panel is used instead of a FlowLayoutPanel: the lines are anchored left and
            // right, so they always follow the width of the cell without any measuring code.
            Panel lines = new Panel();
            lines.Dock = DockStyle.Fill;
            lines.Margin = new Padding(0);
            lines.Padding = new Padding(0);

            FillLines(lines, rows);

            // The lines are added first and the head last, so the head keeps the top edge.
            cell.Controls.Add(lines);
            cell.Controls.Add(head);

            // Every child forwards its mouse events, so hover and click work anywhere in the cell.
            ForwardMouseEvents(cell);

            ShowTooltip(cell, TooltipText(date, rows));

            return cell;
        }

        /// <summary>The appointment lines of one cell: at most three, then "+N more".</summary>
        private void FillLines(Panel lines, List<DataRow> rows)
        {
            List<Label> labels = new List<Label>();

            int shown = 0;
            foreach (DataRow row in rows)
            {
                if (shown == MaxLinesPerCell) break;

                labels.Add(BuildLineLabel(row));
                shown++;
            }

            if (rows.Count > shown)
            {
                Label more = SmallLabel("+" + (rows.Count - shown) + " more");
                more.ForeColor = Theme.TextMuted;
                more.Font = new Font("Segoe UI", 7.5F, FontStyle.Italic);
                labels.Add(more);
            }

            // Docked children stack from the BOTTOM of the z-order upwards, so the first line has
            // to be added last to end up on top of the cell.
            for (int index = labels.Count - 1; index >= 0; index--)
            {
                lines.Controls.Add(labels[index]);
            }
        }

        /// <summary>One appointment line: "10:00 AM Client name", coloured by its status.</summary>
        private Label BuildLineLabel(DataRow row)
        {
            string status = Db.Get<string>(row["status"]);
            string text = Db.Get<string>(row["start_time_text"]) + " " + Db.Get<string>(row["client_name"]);

            Label line = SmallLabel(text);
            line.Tag = Db.Get<int>(row["appointment_id"]);

            // A cancelled visit still shows, but greyed out, because it explains a gap in the day.
            line.ForeColor = IsCancelled(status)
                ? Theme.TextMuted
                : Theme.StatusColor(status);

            line.MouseDoubleClick += Line_MouseDoubleClick;

            return line;
        }

        /// <summary>
        /// A small single line label that takes the whole width of its cell: docking does the
        /// measuring, so nothing has to be resized by hand when the window changes.
        /// </summary>
        private static Label SmallLabel(string text)
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Dock = DockStyle.Top;
            label.Height = 16;
            label.Margin = new Padding(0);
            label.Padding = new Padding(4, 0, 0, 0);
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            label.AutoEllipsis = true;       // a long client name ends with "..." instead of running out
            label.Text = text;
            return label;
        }

        /// <summary>Disposes the day cells of the month that was on screen before.</summary>
        private void DisposeCells()
        {
            if (_tooltip != null) _tooltip.RemoveAll();

            tlpDays.Controls.Clear();

            foreach (Panel cell in _cells)
            {
                cell.Dispose();
            }

            _cells.Clear();
            _hoverCell = null;
        }

        // ------------------------------------------------------------------
        // Colours
        // ------------------------------------------------------------------

        /// <summary>Repaints every cell, which is what a click, a hover or a month change needs.</summary>
        private void RefreshCells()
        {
            foreach (Panel cell in _cells)
            {
                ApplyCellColors(cell);
            }
        }

        /// <summary>
        /// Decides the background of one cell: normal, busy, full, today, selected or hovered.
        /// A full day stays red even when it is selected, because "full" is the warmer warning.
        /// </summary>
        private void ApplyCellColors(Panel cell)
        {
            DateTime date = (DateTime)cell.Tag;
            int count = ActiveCountFor(date);

            Color background = Theme.Surface;

            if (count >= AppointmentRepository.MaxAppointmentsPerDay) background = FullBack;
            else if (count >= BusyFrom) background = BusyBack;

            if (IsToday(date)) background = TodayBack;
            if (IsSelected(date)) background = SelectedBack;
            if (count >= AppointmentRepository.MaxAppointmentsPerDay && IsSelected(date)) background = FullBack;
            if (cell == _hoverCell && !IsSelected(date)) background = HoverBack;

            cell.BackColor = background;

            // The day number follows the state of its cell.
            Label dayNumber = DayNumberLabel(cell);
            if (dayNumber != null) dayNumber.ForeColor = DayNumberColor(date);

            cell.Invalidate();   // the selected cell paints its own border
        }

        /// <summary>
        /// The day number colour: muted for another month, gold for today and the selection.
        /// </summary>
        private Color DayNumberColor(DateTime date)
        {
            if (IsSelected(date)) return Theme.PrimaryDark;
            if (IsToday(date)) return Theme.Primary;
            if (date.Month != _visibleMonth.Month) return Theme.TextMuted;
            return Theme.TextDark;
        }

        /// <summary>The selected cell gets a gold outline, so it reads as the chosen day.</summary>
        private void Cell_Paint(object sender, PaintEventArgs e)
        {
            Panel cell = sender as Panel;
            if (cell == null || !IsSelected((DateTime)cell.Tag)) return;

            using (Pen pen = new Pen(Theme.Primary, 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, cell.Width - 1, cell.Height - 1);
            }
        }

        // ------------------------------------------------------------------
        // Mouse handling
        // ------------------------------------------------------------------

        /// <summary>Every child of a cell gets the same handlers, so a click never falls through.</summary>
        private void ForwardMouseEvents(Control root)
        {
            root.MouseEnter += Cell_MouseEnter;
            root.MouseLeave += Cell_MouseLeave;
            root.MouseClick += Cell_MouseClick;
            root.MouseDoubleClick += Cell_MouseDoubleClick;

            foreach (Control child in root.Controls)
            {
                // An appointment line keeps its OWN double-click (it opens that appointment), so it
                // only forwards the hover and the single click.
                if (child.Tag is int)
                {
                    child.MouseEnter += Cell_MouseEnter;
                    child.MouseLeave += Cell_MouseLeave;
                    child.MouseClick += Cell_MouseClick;
                    continue;
                }

                ForwardMouseEvents(child);
            }
        }

        private void Cell_MouseEnter(object sender, EventArgs e)
        {
            Panel cell = CellOf(sender as Control);
            if (cell == null || cell == _hoverCell) return;

            Panel previous = _hoverCell;
            _hoverCell = cell;

            if (previous != null) ApplyCellColors(previous);
            ApplyCellColors(cell);
        }

        private void Cell_MouseLeave(object sender, EventArgs e)
        {
            Panel cell = CellOf(sender as Control);
            if (cell == null || cell != _hoverCell) return;

            // MouseLeave also fires when the pointer moves onto a child of the cell, so the
            // highlight only goes away when the pointer has really left the cell.
            if (cell.ClientRectangle.Contains(cell.PointToClient(Cursor.Position))) return;

            _hoverCell = null;
            ApplyCellColors(cell);
        }

        private void Cell_MouseClick(object sender, MouseEventArgs e)
        {
            Panel cell = CellOf(sender as Control);
            if (cell == null) return;

            SelectDay((DateTime)cell.Tag);
        }

        private void Cell_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Panel cell = CellOf(sender as Control);
            if (cell == null) return;

            // A double-click on the empty part of a day asks for that day in the Appointments page.
            if (DayOpened != null) DayOpened((DateTime)cell.Tag);
        }

        private void Line_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Label line = sender as Label;
            if (line == null || !(line.Tag is int)) return;

            if (AppointmentOpened != null) AppointmentOpened((int)line.Tag);
        }

        /// <summary>Selects a day, paints the grid again and tells the caller.</summary>
        private void SelectDay(DateTime date)
        {
            _selectedDate = date.Date;
            RefreshCells();

            if (DaySelected != null) DaySelected(_selectedDate.Value);
        }

        /// <summary>The cell a control belongs to (walks up the parents and reads the Tag).</summary>
        private static Panel CellOf(Control control)
        {
            Control current = control;

            while (current != null)
            {
                Panel panel = current as Panel;
                if (panel != null && panel.Tag is DateTime) return panel;

                current = current.Parent;
            }

            return null;
        }

        /// <summary>The day-number label inside a cell (the label of its head panel).</summary>
        private static Label DayNumberLabel(Panel cell)
        {
            foreach (Control child in cell.Controls)
            {
                Panel head = child as Panel;
                if (head == null) continue;

                foreach (Control grandChild in head.Controls)
                {
                    Label label = grandChild as Label;
                    if (label != null && !(label.Tag is int)) return label;
                }
            }

            return null;
        }

        /// <summary>One tooltip for the whole control, disposed together with it.</summary>
        private void ShowTooltip(Control control, string text)
        {
            if (_tooltip == null)
            {
                _tooltip = new ToolTip();
                _tooltip.InitialDelay = 350;
                _tooltip.ReshowDelay = 100;
                _tooltip.AutoPopDelay = 15000;

                // The tooltip is not a control, so nothing else would ever dispose it.
                Disposed += Calendar_Disposed;
            }

            _tooltip.SetToolTip(control, text);
        }

        private void Calendar_Disposed(object sender, EventArgs e)
        {
            if (_tooltip == null) return;

            _tooltip.Dispose();
            _tooltip = null;
        }

        /// <summary>
        /// The hover text of a day: one line per appointment
        /// ("10:00 AM - Client - Services - Technician - Status"), at most eight of them.
        /// </summary>
        private static string TooltipText(DateTime date, List<DataRow> rows)
        {
            if (rows.Count == 0) return "No appointments";

            StringBuilder text = new StringBuilder();
            text.Append(date.ToString("MMM dd, yyyy"));

            int shown = 0;
            foreach (DataRow row in rows)
            {
                if (shown == MaxTooltipLines) break;

                text.AppendLine();
                text.Append(Db.Get<string>(row["start_time_text"]));
                text.Append(" - ").Append(Db.Get<string>(row["client_name"]));
                text.Append(" - ").Append(Db.Get<string>(row["services"]));
                text.Append(" - ").Append(Db.Get<string>(row["staff_names"]));
                text.Append(" - ").Append(Db.Get<string>(row["status"]));

                shown++;
            }

            if (rows.Count > shown)
            {
                text.AppendLine();
                text.Append("+").Append(rows.Count - shown).Append(" more");
            }

            return text.ToString();
        }

        // ------------------------------------------------------------------
        // Data helpers
        // ------------------------------------------------------------------

        /// <summary>Groups the month rows by day and counts the non-cancelled ones per day.</summary>
        private void GroupAppointments(DataTable appointments)
        {
            _byDate.Clear();
            _activeCount.Clear();

            if (appointments == null) return;

            foreach (DataRow row in appointments.Rows)
            {
                object rawDate = row["appointment_date"];
                if (rawDate == null || rawDate == DBNull.Value) continue;

                DateTime date = Convert.ToDateTime(rawDate).Date;

                List<DataRow> rows;
                if (!_byDate.TryGetValue(date, out rows))
                {
                    rows = new List<DataRow>();
                    _byDate[date] = rows;
                }

                rows.Add(row);

                if (!IsCancelled(Db.Get<string>(row["status"])))
                {
                    _activeCount[date] = ActiveCountFor(date) + 1;
                }
            }
        }

        private static bool IsCancelled(string status)
        {
            return string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase)
                || string.Equals(status, "Canceled", StringComparison.OrdinalIgnoreCase);
        }

        private List<DataRow> RowsFor(DateTime date)
        {
            List<DataRow> rows;
            return _byDate.TryGetValue(date.Date, out rows) ? rows : new List<DataRow>();
        }

        private int ActiveCountFor(DateTime date)
        {
            int count;
            return _activeCount.TryGetValue(date.Date, out count) ? count : 0;
        }

        private bool IsToday(DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        private bool IsSelected(DateTime date)
        {
            return _selectedDate.HasValue && _selectedDate.Value.Date == date.Date;
        }

        private static DateTime FirstDayOf(DateTime month)
        {
            return new DateTime(month.Year, month.Month, 1);
        }

        /// <summary>The Sunday on or before the first day of the visible month.</summary>
        private DateTime GridStart()
        {
            int offset = (int)_visibleMonth.DayOfWeek;   // Sunday = 0
            return _visibleMonth.AddDays(-offset);
        }

        // ------------------------------------------------------------------
        // Month buttons
        // ------------------------------------------------------------------

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            ChangeMonth(_visibleMonth.AddMonths(-1));
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            ChangeMonth(_visibleMonth.AddMonths(1));
        }

        private void btnTodayMonth_Click(object sender, EventArgs e)
        {
            ChangeMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
        }

        /// <summary>
        /// Moves the header to another month and asks the caller for that month's data. The cells
        /// themselves are only rebuilt when LoadMonth arrives, so the screen never shows a month
        /// without its appointments.
        /// </summary>
        private void ChangeMonth(DateTime month)
        {
            _visibleMonth = FirstDayOf(month);
            lblMonth.Text = _visibleMonth.ToString("MMMM yyyy");

            // A selection of the previous month no longer makes sense on the new grid.
            _selectedDate = null;

            if (MonthChanged != null) MonthChanged(_visibleMonth);
        }
    }
}
