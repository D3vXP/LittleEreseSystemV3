namespace LittleEreseSystemV3.Controls
{
    partial class ucCalendarMonth
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCalHeader = new System.Windows.Forms.Panel();
            this.tlpCalHeader = new System.Windows.Forms.TableLayoutPanel();
            this.btnPrevMonth = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.btnTodayMonth = new System.Windows.Forms.Button();
            this.flpLegend = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLegendUpcoming = new System.Windows.Forms.Label();
            this.lblLegendUpcomingText = new System.Windows.Forms.Label();
            this.lblLegendOngoing = new System.Windows.Forms.Label();
            this.lblLegendOngoingText = new System.Windows.Forms.Label();
            this.lblLegendCompleted = new System.Windows.Forms.Label();
            this.lblLegendCompletedText = new System.Windows.Forms.Label();
            this.lblLegendCancelled = new System.Windows.Forms.Label();
            this.lblLegendCancelledText = new System.Windows.Forms.Label();
            this.lblLegendRescheduled = new System.Windows.Forms.Label();
            this.lblLegendRescheduledText = new System.Windows.Forms.Label();
            this.tlpWeekdays = new System.Windows.Forms.TableLayoutPanel();
            this.lblDaySun = new System.Windows.Forms.Label();
            this.lblDayMon = new System.Windows.Forms.Label();
            this.lblDayTue = new System.Windows.Forms.Label();
            this.lblDayWed = new System.Windows.Forms.Label();
            this.lblDayThu = new System.Windows.Forms.Label();
            this.lblDayFri = new System.Windows.Forms.Label();
            this.lblDaySat = new System.Windows.Forms.Label();
            this.tlpDays = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCalHeader.SuspendLayout();
            this.tlpCalHeader.SuspendLayout();
            this.flpLegend.SuspendLayout();
            this.tlpWeekdays.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCalHeader
            // 
            this.pnlCalHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlCalHeader.Controls.Add(this.tlpCalHeader);
            this.pnlCalHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCalHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlCalHeader.Name = "pnlCalHeader";
            this.pnlCalHeader.Size = new System.Drawing.Size(760, 56);
            this.pnlCalHeader.TabIndex = 0;
            // 
            // tlpCalHeader
            // 
            this.tlpCalHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpCalHeader.ColumnCount = 4;
            this.tlpCalHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpCalHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCalHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpCalHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlpCalHeader.Controls.Add(this.btnPrevMonth, 0, 0);
            this.tlpCalHeader.Controls.Add(this.lblMonth, 1, 0);
            this.tlpCalHeader.Controls.Add(this.btnNextMonth, 2, 0);
            this.tlpCalHeader.Controls.Add(this.btnTodayMonth, 3, 0);
            this.tlpCalHeader.Controls.Add(this.flpLegend, 0, 1);
            this.tlpCalHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCalHeader.Location = new System.Drawing.Point(0, 0);
            this.tlpCalHeader.Name = "tlpCalHeader";
            this.tlpCalHeader.RowCount = 2;
            this.tlpCalHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpCalHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCalHeader.SetColumnSpan(this.flpLegend, 4);
            this.tlpCalHeader.Size = new System.Drawing.Size(760, 56);
            this.tlpCalHeader.TabIndex = 0;
            // 
            // btnPrevMonth
            // 
            this.btnPrevMonth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrevMonth.BackColor = System.Drawing.Color.White;
            this.btnPrevMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrevMonth.FlatAppearance.BorderSize = 1;
            this.btnPrevMonth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnPrevMonth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnPrevMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevMonth.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrevMonth.Location = new System.Drawing.Point(1, 13);
            this.btnPrevMonth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnPrevMonth.Name = "btnPrevMonth";
            this.btnPrevMonth.Size = new System.Drawing.Size(34, 30);
            this.btnPrevMonth.TabIndex = 0;
            this.btnPrevMonth.Text = "\uE76B";
            this.btnPrevMonth.UseVisualStyleBackColor = false;
            this.btnPrevMonth.Click += new System.EventHandler(this.btnPrevMonth_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblMonth.Location = new System.Drawing.Point(36, 0);
            this.lblMonth.Margin = new System.Windows.Forms.Padding(0);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(188, 56);
            this.lblMonth.TabIndex = 1;
            this.lblMonth.Text = "October 2026";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNextMonth.BackColor = System.Drawing.Color.White;
            this.btnNextMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNextMonth.FlatAppearance.BorderSize = 1;
            this.btnNextMonth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnNextMonth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnNextMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextMonth.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNextMonth.Location = new System.Drawing.Point(226, 13);
            this.btnNextMonth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(34, 30);
            this.btnNextMonth.TabIndex = 2;
            this.btnNextMonth.Text = "\uE76C";
            this.btnNextMonth.UseVisualStyleBackColor = false;
            this.btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);
            // 
            // btnTodayMonth
            // 
            this.btnTodayMonth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTodayMonth.BackColor = System.Drawing.Color.White;
            this.btnTodayMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTodayMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTodayMonth.FlatAppearance.BorderSize = 1;
            this.btnTodayMonth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnTodayMonth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnTodayMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTodayMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTodayMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTodayMonth.Location = new System.Drawing.Point(264, 13);
            this.btnTodayMonth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnTodayMonth.Name = "btnTodayMonth";
            this.btnTodayMonth.Size = new System.Drawing.Size(72, 30);
            this.btnTodayMonth.TabIndex = 3;
            this.btnTodayMonth.Text = "Today";
            this.btnTodayMonth.UseVisualStyleBackColor = false;
            this.btnTodayMonth.Click += new System.EventHandler(this.btnTodayMonth_Click);
            // 
            // flpLegend
            // 
            this.flpLegend.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flpLegend.AutoSize = true;
            this.flpLegend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpLegend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpLegend.Controls.Add(this.lblLegendUpcoming);
            this.flpLegend.Controls.Add(this.lblLegendUpcomingText);
            this.flpLegend.Controls.Add(this.lblLegendOngoing);
            this.flpLegend.Controls.Add(this.lblLegendOngoingText);
            this.flpLegend.Controls.Add(this.lblLegendCompleted);
            this.flpLegend.Controls.Add(this.lblLegendCompletedText);
            this.flpLegend.Controls.Add(this.lblLegendCancelled);
            this.flpLegend.Controls.Add(this.lblLegendCancelledText);
            this.flpLegend.Controls.Add(this.lblLegendRescheduled);
            this.flpLegend.Controls.Add(this.lblLegendRescheduledText);
            this.flpLegend.Location = new System.Drawing.Point(344, 19);
            this.flpLegend.Margin = new System.Windows.Forms.Padding(6, 0, 12, 0);
            this.flpLegend.Name = "flpLegend";
            this.flpLegend.Size = new System.Drawing.Size(390, 18);
            this.flpLegend.TabIndex = 4;
            this.flpLegend.WrapContents = false;
            // 
            // lblLegendUpcoming
            // 
            this.lblLegendUpcoming.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.lblLegendUpcoming.Location = new System.Drawing.Point(0, 3);
            this.lblLegendUpcoming.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.lblLegendUpcoming.Name = "lblLegendUpcoming";
            this.lblLegendUpcoming.Size = new System.Drawing.Size(12, 12);
            this.lblLegendUpcoming.TabIndex = 0;
            // 
            // lblLegendUpcomingText
            // 
            this.lblLegendUpcomingText.AutoSize = true;
            this.lblLegendUpcomingText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegendUpcomingText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLegendUpcomingText.Location = new System.Drawing.Point(16, 1);
            this.lblLegendUpcomingText.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            this.lblLegendUpcomingText.Name = "lblLegendUpcomingText";
            this.lblLegendUpcomingText.Size = new System.Drawing.Size(50, 13);
            this.lblLegendUpcomingText.TabIndex = 1;
            this.lblLegendUpcomingText.Text = "Upcoming";
            // 
            // lblLegendOngoing
            // 
            this.lblLegendOngoing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(125)))), ((int)(((byte)(168)))));
            this.lblLegendOngoing.Location = new System.Drawing.Point(76, 3);
            this.lblLegendOngoing.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.lblLegendOngoing.Name = "lblLegendOngoing";
            this.lblLegendOngoing.Size = new System.Drawing.Size(12, 12);
            this.lblLegendOngoing.TabIndex = 2;
            // 
            // lblLegendOngoingText
            // 
            this.lblLegendOngoingText.AutoSize = true;
            this.lblLegendOngoingText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegendOngoingText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLegendOngoingText.Location = new System.Drawing.Point(92, 1);
            this.lblLegendOngoingText.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            this.lblLegendOngoingText.Name = "lblLegendOngoingText";
            this.lblLegendOngoingText.Size = new System.Drawing.Size(45, 13);
            this.lblLegendOngoingText.TabIndex = 3;
            this.lblLegendOngoingText.Text = "Ongoing";
            // 
            // lblLegendCompleted
            // 
            this.lblLegendCompleted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(142)))), ((int)(((byte)(90)))));
            this.lblLegendCompleted.Location = new System.Drawing.Point(147, 3);
            this.lblLegendCompleted.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.lblLegendCompleted.Name = "lblLegendCompleted";
            this.lblLegendCompleted.Size = new System.Drawing.Size(12, 12);
            this.lblLegendCompleted.TabIndex = 4;
            // 
            // lblLegendCompletedText
            // 
            this.lblLegendCompletedText.AutoSize = true;
            this.lblLegendCompletedText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegendCompletedText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLegendCompletedText.Location = new System.Drawing.Point(163, 1);
            this.lblLegendCompletedText.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            this.lblLegendCompletedText.Name = "lblLegendCompletedText";
            this.lblLegendCompletedText.Size = new System.Drawing.Size(56, 13);
            this.lblLegendCompletedText.TabIndex = 5;
            this.lblLegendCompletedText.Text = "Completed";
            // 
            // lblLegendCancelled
            // 
            this.lblLegendCancelled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblLegendCancelled.Location = new System.Drawing.Point(229, 3);
            this.lblLegendCancelled.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.lblLegendCancelled.Name = "lblLegendCancelled";
            this.lblLegendCancelled.Size = new System.Drawing.Size(12, 12);
            this.lblLegendCancelled.TabIndex = 6;
            // 
            // lblLegendCancelledText
            // 
            this.lblLegendCancelledText.AutoSize = true;
            this.lblLegendCancelledText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegendCancelledText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLegendCancelledText.Location = new System.Drawing.Point(245, 1);
            this.lblLegendCancelledText.Margin = new System.Windows.Forms.Padding(0, 1, 10, 0);
            this.lblLegendCancelledText.Name = "lblLegendCancelledText";
            this.lblLegendCancelledText.Size = new System.Drawing.Size(52, 13);
            this.lblLegendCancelledText.TabIndex = 7;
            this.lblLegendCancelledText.Text = "Cancelled";
            // 
            // lblLegendRescheduled
            // 
            this.lblLegendRescheduled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(142)))), ((int)(((byte)(4)))));
            this.lblLegendRescheduled.Location = new System.Drawing.Point(307, 3);
            this.lblLegendRescheduled.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.lblLegendRescheduled.Name = "lblLegendRescheduled";
            this.lblLegendRescheduled.Size = new System.Drawing.Size(12, 12);
            this.lblLegendRescheduled.TabIndex = 8;
            // 
            // lblLegendRescheduledText
            // 
            this.lblLegendRescheduledText.AutoSize = true;
            this.lblLegendRescheduledText.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLegendRescheduledText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLegendRescheduledText.Location = new System.Drawing.Point(323, 1);
            this.lblLegendRescheduledText.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.lblLegendRescheduledText.Name = "lblLegendRescheduledText";
            this.lblLegendRescheduledText.Size = new System.Drawing.Size(67, 13);
            this.lblLegendRescheduledText.TabIndex = 9;
            this.lblLegendRescheduledText.Text = "Rescheduled";
            // 
            // tlpWeekdays
            // 
            this.tlpWeekdays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.tlpWeekdays.ColumnCount = 7;
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpWeekdays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tlpWeekdays.Controls.Add(this.lblDaySun, 0, 0);
            this.tlpWeekdays.Controls.Add(this.lblDayMon, 1, 0);
            this.tlpWeekdays.Controls.Add(this.lblDayTue, 2, 0);
            this.tlpWeekdays.Controls.Add(this.lblDayWed, 3, 0);
            this.tlpWeekdays.Controls.Add(this.lblDayThu, 4, 0);
            this.tlpWeekdays.Controls.Add(this.lblDayFri, 5, 0);
            this.tlpWeekdays.Controls.Add(this.lblDaySat, 6, 0);
            this.tlpWeekdays.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpWeekdays.Location = new System.Drawing.Point(0, 56);
            this.tlpWeekdays.Name = "tlpWeekdays";
            this.tlpWeekdays.RowCount = 1;
            this.tlpWeekdays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWeekdays.Size = new System.Drawing.Size(760, 32);
            this.tlpWeekdays.TabIndex = 1;
            // 
            // lblDaySun
            // 
            this.lblDaySun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDaySun.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDaySun.ForeColor = System.Drawing.Color.White;
            this.lblDaySun.Location = new System.Drawing.Point(0, 0);
            this.lblDaySun.Margin = new System.Windows.Forms.Padding(0);
            this.lblDaySun.Name = "lblDaySun";
            this.lblDaySun.Size = new System.Drawing.Size(108, 32);
            this.lblDaySun.TabIndex = 0;
            this.lblDaySun.Text = "Sun";
            this.lblDaySun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayMon
            // 
            this.lblDayMon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayMon.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayMon.ForeColor = System.Drawing.Color.White;
            this.lblDayMon.Location = new System.Drawing.Point(108, 0);
            this.lblDayMon.Margin = new System.Windows.Forms.Padding(0);
            this.lblDayMon.Name = "lblDayMon";
            this.lblDayMon.Size = new System.Drawing.Size(108, 32);
            this.lblDayMon.TabIndex = 1;
            this.lblDayMon.Text = "Mon";
            this.lblDayMon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayTue
            // 
            this.lblDayTue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayTue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayTue.ForeColor = System.Drawing.Color.White;
            this.lblDayTue.Location = new System.Drawing.Point(216, 0);
            this.lblDayTue.Margin = new System.Windows.Forms.Padding(0);
            this.lblDayTue.Name = "lblDayTue";
            this.lblDayTue.Size = new System.Drawing.Size(108, 32);
            this.lblDayTue.TabIndex = 2;
            this.lblDayTue.Text = "Tue";
            this.lblDayTue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayWed
            // 
            this.lblDayWed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayWed.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayWed.ForeColor = System.Drawing.Color.White;
            this.lblDayWed.Location = new System.Drawing.Point(324, 0);
            this.lblDayWed.Margin = new System.Windows.Forms.Padding(0);
            this.lblDayWed.Name = "lblDayWed";
            this.lblDayWed.Size = new System.Drawing.Size(108, 32);
            this.lblDayWed.TabIndex = 3;
            this.lblDayWed.Text = "Wed";
            this.lblDayWed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayThu
            // 
            this.lblDayThu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayThu.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayThu.ForeColor = System.Drawing.Color.White;
            this.lblDayThu.Location = new System.Drawing.Point(432, 0);
            this.lblDayThu.Margin = new System.Windows.Forms.Padding(0);
            this.lblDayThu.Name = "lblDayThu";
            this.lblDayThu.Size = new System.Drawing.Size(108, 32);
            this.lblDayThu.TabIndex = 4;
            this.lblDayThu.Text = "Thu";
            this.lblDayThu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayFri
            // 
            this.lblDayFri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayFri.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayFri.ForeColor = System.Drawing.Color.White;
            this.lblDayFri.Location = new System.Drawing.Point(540, 0);
            this.lblDayFri.Margin = new System.Windows.Forms.Padding(0);
            this.lblDayFri.Name = "lblDayFri";
            this.lblDayFri.Size = new System.Drawing.Size(108, 32);
            this.lblDayFri.TabIndex = 5;
            this.lblDayFri.Text = "Fri";
            this.lblDayFri.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDaySat
            // 
            this.lblDaySat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDaySat.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDaySat.ForeColor = System.Drawing.Color.White;
            this.lblDaySat.Location = new System.Drawing.Point(648, 0);
            this.lblDaySat.Margin = new System.Windows.Forms.Padding(0);
            this.lblDaySat.Name = "lblDaySat";
            this.lblDaySat.Size = new System.Drawing.Size(112, 32);
            this.lblDaySat.TabIndex = 6;
            this.lblDaySat.Text = "Sat";
            this.lblDaySat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpDays
            // 
            this.tlpDays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.tlpDays.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpDays.ColumnCount = 7;
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpDays.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tlpDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDays.Location = new System.Drawing.Point(0, 88);
            this.tlpDays.Name = "tlpDays";
            this.tlpDays.RowCount = 6;
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpDays.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66665F));
            this.tlpDays.Size = new System.Drawing.Size(760, 432);
            this.tlpDays.TabIndex = 2;
            // 
            // ucCalendarMonth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.tlpDays);
            this.Controls.Add(this.tlpWeekdays);
            this.Controls.Add(this.pnlCalHeader);
            this.Name = "ucCalendarMonth";
            this.Size = new System.Drawing.Size(760, 520);
            this.pnlCalHeader.ResumeLayout(false);
            this.tlpCalHeader.ResumeLayout(false);
            this.tlpCalHeader.PerformLayout();
            this.flpLegend.ResumeLayout(false);
            this.flpLegend.PerformLayout();
            this.tlpWeekdays.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCalHeader;
        private System.Windows.Forms.TableLayoutPanel tlpCalHeader;
        private System.Windows.Forms.Button btnPrevMonth;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Button btnTodayMonth;
        private System.Windows.Forms.FlowLayoutPanel flpLegend;
        private System.Windows.Forms.Label lblLegendUpcoming;
        private System.Windows.Forms.Label lblLegendUpcomingText;
        private System.Windows.Forms.Label lblLegendOngoing;
        private System.Windows.Forms.Label lblLegendOngoingText;
        private System.Windows.Forms.Label lblLegendCompleted;
        private System.Windows.Forms.Label lblLegendCompletedText;
        private System.Windows.Forms.Label lblLegendCancelled;
        private System.Windows.Forms.Label lblLegendCancelledText;
        private System.Windows.Forms.Label lblLegendRescheduled;
        private System.Windows.Forms.Label lblLegendRescheduledText;
        private System.Windows.Forms.TableLayoutPanel tlpWeekdays;
        private System.Windows.Forms.Label lblDaySun;
        private System.Windows.Forms.Label lblDayMon;
        private System.Windows.Forms.Label lblDayTue;
        private System.Windows.Forms.Label lblDayWed;
        private System.Windows.Forms.Label lblDayThu;
        private System.Windows.Forms.Label lblDayFri;
        private System.Windows.Forms.Label lblDaySat;
        private System.Windows.Forms.TableLayoutPanel tlpDays;
    }
}
