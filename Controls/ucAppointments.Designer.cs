namespace LittleEreseSystemV3.Controls
{
    partial class ucAppointments
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.tlpToolbar = new System.Windows.Forms.TableLayoutPanel();
            this.flpNav = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrevDay = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnNextDay = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSummary = new System.Windows.Forms.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnTakePayment = new System.Windows.Forms.Button();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.btnCancelAppointment = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.colAppointmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTech = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tlpDetails = new System.Windows.Forms.TableLayoutPanel();
            this.tlpDetailInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetailClient = new System.Windows.Forms.Label();
            this.lblDetailSchedule = new System.Windows.Forms.Label();
            this.lblDetailPayment = new System.Windows.Forms.Label();
            this.dgvDetailServices = new System.Windows.Forms.DataGridView();
            this.colDetailService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailStaff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpNotes = new System.Windows.Forms.TableLayoutPanel();
            this.lblNotesTitle = new System.Windows.Forms.Label();
            this.txtDetailNotes = new System.Windows.Forms.TextBox();
            this.pnlToolbar.SuspendLayout();
            this.tlpToolbar.SuspendLayout();
            this.flpNav.SuspendLayout();
            this.flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.tlpDetails.SuspendLayout();
            this.tlpDetailInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailServices)).BeginInit();
            this.tlpNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlToolbar.Controls.Add(this.tlpToolbar);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(1000, 92);
            this.pnlToolbar.TabIndex = 0;
            // 
            // tlpToolbar
            // 
            this.tlpToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpToolbar.ColumnCount = 2;
            this.tlpToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpToolbar.Controls.Add(this.flpNav, 0, 0);
            this.tlpToolbar.Controls.Add(this.flpButtons, 0, 1);
            this.tlpToolbar.Controls.Add(this.lblSummary, 1, 1);
            this.tlpToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpToolbar.Location = new System.Drawing.Point(0, 0);
            this.tlpToolbar.Name = "tlpToolbar";
            this.tlpToolbar.RowCount = 2;
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpToolbar.Size = new System.Drawing.Size(1000, 92);
            this.tlpToolbar.TabIndex = 0;
            // 
            // flpNav
            // 
            this.flpNav.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpNav.AutoSize = true;
            this.flpNav.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpNav.Controls.Add(this.btnPrevDay);
            this.flpNav.Controls.Add(this.dtpDate);
            this.flpNav.Controls.Add(this.btnNextDay);
            this.flpNav.Controls.Add(this.btnToday);
            this.flpNav.Controls.Add(this.lblStatus);
            this.flpNav.Controls.Add(this.cboStatus);
            this.flpNav.Controls.Add(this.lblSearch);
            this.flpNav.Controls.Add(this.txtSearch);
            this.flpNav.Location = new System.Drawing.Point(12, 8);
            this.flpNav.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpNav.Name = "flpNav";
            this.flpNav.Size = new System.Drawing.Size(706, 30);
            this.flpNav.TabIndex = 0;
            this.flpNav.WrapContents = false;
            // 
            // btnPrevDay
            // 
            this.btnPrevDay.BackColor = System.Drawing.Color.White;
            this.btnPrevDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevDay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrevDay.FlatAppearance.BorderSize = 1;
            this.btnPrevDay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnPrevDay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnPrevDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevDay.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrevDay.Location = new System.Drawing.Point(0, 0);
            this.btnPrevDay.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnPrevDay.Name = "btnPrevDay";
            this.btnPrevDay.Size = new System.Drawing.Size(34, 30);
            this.btnPrevDay.TabIndex = 0;
            this.btnPrevDay.Text = "\uE76B";
            this.btnPrevDay.UseVisualStyleBackColor = false;
            this.btnPrevDay.Click += new System.EventHandler(this.btnPrevDay_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(38, 3);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(130, 25);
            this.dtpDate.TabIndex = 1;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnNextDay
            // 
            this.btnNextDay.BackColor = System.Drawing.Color.White;
            this.btnNextDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextDay.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNextDay.FlatAppearance.BorderSize = 1;
            this.btnNextDay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnNextDay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnNextDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextDay.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextDay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNextDay.Location = new System.Drawing.Point(172, 0);
            this.btnNextDay.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnNextDay.Name = "btnNextDay";
            this.btnNextDay.Size = new System.Drawing.Size(34, 30);
            this.btnNextDay.TabIndex = 2;
            this.btnNextDay.Text = "\uE76C";
            this.btnNextDay.UseVisualStyleBackColor = false;
            this.btnNextDay.Click += new System.EventHandler(this.btnNextDay_Click);
            // 
            // btnToday
            // 
            this.btnToday.BackColor = System.Drawing.Color.White;
            this.btnToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnToday.FlatAppearance.BorderSize = 1;
            this.btnToday.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnToday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToday.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnToday.Location = new System.Drawing.Point(210, 0);
            this.btnToday.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(74, 30);
            this.btnToday.TabIndex = 3;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = false;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblStatus.Location = new System.Drawing.Point(300, 6);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 19);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Active",
            "All",
            "Upcoming",
            "Ongoing",
            "Completed",
            "Cancelled"});
            this.cboStatus.Location = new System.Drawing.Point(353, 3);
            this.cboStatus.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(132, 25);
            this.cboStatus.TabIndex = 5;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSearch.Location = new System.Drawing.Point(497, 6);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(43, 19);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "Client";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(546, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(160, 25);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // flpButtons
            // 
            this.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpButtons.AutoSize = true;
            this.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpButtons.Controls.Add(this.btnNew);
            this.flpButtons.Controls.Add(this.btnTakePayment);
            this.flpButtons.Controls.Add(this.btnReschedule);
            this.flpButtons.Controls.Add(this.btnCancelAppointment);
            this.flpButtons.Controls.Add(this.btnRefresh);
            this.flpButtons.Location = new System.Drawing.Point(12, 54);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(612, 30);
            this.flpButtons.TabIndex = 1;
            this.flpButtons.WrapContents = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnNew.FlatAppearance.BorderSize = 1;
            this.btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(0, 0);
            this.btnNew.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(136, 30);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New Appointment";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnTakePayment
            // 
            this.btnTakePayment.BackColor = System.Drawing.Color.White;
            this.btnTakePayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTakePayment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTakePayment.FlatAppearance.BorderSize = 1;
            this.btnTakePayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnTakePayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnTakePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTakePayment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTakePayment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTakePayment.Location = new System.Drawing.Point(140, 0);
            this.btnTakePayment.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnTakePayment.Name = "btnTakePayment";
            this.btnTakePayment.Size = new System.Drawing.Size(122, 30);
            this.btnTakePayment.TabIndex = 1;
            this.btnTakePayment.Text = "Take Payment";
            this.btnTakePayment.UseVisualStyleBackColor = false;
            this.btnTakePayment.Click += new System.EventHandler(this.btnTakePayment_Click);
            // 
            // btnReschedule
            // 
            this.btnReschedule.BackColor = System.Drawing.Color.White;
            this.btnReschedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReschedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnReschedule.FlatAppearance.BorderSize = 1;
            this.btnReschedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnReschedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnReschedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReschedule.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReschedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnReschedule.Location = new System.Drawing.Point(266, 0);
            this.btnReschedule.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnReschedule.Name = "btnReschedule";
            this.btnReschedule.Size = new System.Drawing.Size(106, 30);
            this.btnReschedule.TabIndex = 2;
            this.btnReschedule.Text = "Reschedule";
            this.btnReschedule.UseVisualStyleBackColor = false;
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);
            // 
            // btnCancelAppointment
            // 
            this.btnCancelAppointment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnCancelAppointment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelAppointment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnCancelAppointment.FlatAppearance.BorderSize = 1;
            this.btnCancelAppointment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(47)))), ((int)(((byte)(34)))));
            this.btnCancelAppointment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(80)))), ((int)(((byte)(64)))));
            this.btnCancelAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAppointment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelAppointment.ForeColor = System.Drawing.Color.White;
            this.btnCancelAppointment.Location = new System.Drawing.Point(376, 0);
            this.btnCancelAppointment.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnCancelAppointment.Name = "btnCancelAppointment";
            this.btnCancelAppointment.Size = new System.Drawing.Size(152, 30);
            this.btnCancelAppointment.TabIndex = 3;
            this.btnCancelAppointment.Text = "Cancel Appointment";
            this.btnCancelAppointment.UseVisualStyleBackColor = false;
            this.btnCancelAppointment.Click += new System.EventHandler(this.btnCancelAppointment_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRefresh.FlatAppearance.BorderSize = 1;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRefresh.Location = new System.Drawing.Point(532, 0);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblSummary.Location = new System.Drawing.Point(806, 62);
            this.lblSummary.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(182, 15);
            this.lblSummary.TabIndex = 2;
            this.lblSummary.Text = "0 appointments - 0 massage";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 92);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlGrid);
            this.splitMain.Panel1MinSize = 180;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            this.splitMain.Panel2MinSize = 150;
            this.splitMain.Size = new System.Drawing.Size(1000, 548);
            this.splitMain.SplitterDistance = 300;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.dgvAppointments);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Size = new System.Drawing.Size(1000, 328);
            this.pnlGrid.TabIndex = 0;
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvAppointments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAppointments.AutoGenerateColumns = false;
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.BackgroundColor = System.Drawing.Color.White;
            this.dgvAppointments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAppointments.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAppointments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAppointments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAppointments.ColumnHeadersHeight = 34;
            this.dgvAppointments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAppointmentId,
            this.colTime,
            this.colClient,
            this.colServices,
            this.colTech,
            this.colRoom,
            this.colSource,
            this.colStatus,
            this.colPayment});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAppointments.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAppointments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAppointments.EnableHeadersVisualStyles = false;
            this.dgvAppointments.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvAppointments.Location = new System.Drawing.Point(1, 1);
            this.dgvAppointments.MultiSelect = false;
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.RowHeadersVisible = false;
            this.dgvAppointments.RowTemplate.Height = 30;
            this.dgvAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppointments.Size = new System.Drawing.Size(996, 324);
            this.dgvAppointments.TabIndex = 0;
            this.dgvAppointments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellDoubleClick);
            this.dgvAppointments.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAppointments_CellFormatting);
            this.dgvAppointments.SelectionChanged += new System.EventHandler(this.dgvAppointments_SelectionChanged);
            // 
            // colAppointmentId
            // 
            this.colAppointmentId.DataPropertyName = "appointment_id";
            this.colAppointmentId.HeaderText = "ID";
            this.colAppointmentId.Name = "colAppointmentId";
            this.colAppointmentId.ReadOnly = true;
            this.colAppointmentId.Visible = false;
            // 
            // colTime
            // 
            this.colTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTime.DataPropertyName = "time_range";
            this.colTime.FillWeight = 13F;
            this.colTime.HeaderText = "Time";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colClient
            // 
            this.colClient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colClient.DataPropertyName = "client_name";
            this.colClient.FillWeight = 17F;
            this.colClient.HeaderText = "Client";
            this.colClient.Name = "colClient";
            this.colClient.ReadOnly = true;
            // 
            // colServices
            // 
            this.colServices.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colServices.DataPropertyName = "services";
            this.colServices.FillWeight = 27F;
            this.colServices.HeaderText = "Services";
            this.colServices.Name = "colServices";
            this.colServices.ReadOnly = true;
            // 
            // colTech
            // 
            this.colTech.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTech.DataPropertyName = "staff_names";
            this.colTech.FillWeight = 16F;
            this.colTech.HeaderText = "Technician";
            this.colTech.Name = "colTech";
            this.colTech.ReadOnly = true;
            // 
            // colRoom
            // 
            this.colRoom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRoom.DataPropertyName = "room_names";
            this.colRoom.FillWeight = 11F;
            this.colRoom.HeaderText = "Room";
            this.colRoom.Name = "colRoom";
            this.colRoom.ReadOnly = true;
            // 
            // colSource
            // 
            this.colSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSource.DataPropertyName = "booking_source";
            this.colSource.FillWeight = 9F;
            this.colSource.HeaderText = "Source";
            this.colSource.Name = "colSource";
            this.colSource.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStatus.DataPropertyName = "status";
            this.colStatus.FillWeight = 11F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colPayment
            // 
            this.colPayment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPayment.DataPropertyName = "payment_status";
            this.colPayment.FillWeight = 11F;
            this.colPayment.HeaderText = "Payment";
            this.colPayment.Name = "colPayment";
            this.colPayment.ReadOnly = true;
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.White;
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.tlpDetails);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(16);
            this.pnlDetails.Size = new System.Drawing.Size(1000, 214);
            this.pnlDetails.TabIndex = 0;
            // 
            // tlpDetails
            // 
            this.tlpDetails.BackColor = System.Drawing.Color.White;
            this.tlpDetails.ColumnCount = 2;
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 356F));
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetails.Controls.Add(this.tlpDetailInfo, 0, 0);
            this.tlpDetails.Controls.Add(this.dgvDetailServices, 1, 0);
            this.tlpDetails.Controls.Add(this.tlpNotes, 1, 1);
            this.tlpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetails.Location = new System.Drawing.Point(16, 16);
            this.tlpDetails.Name = "tlpDetails";
            this.tlpDetails.RowCount = 2;
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetails.SetRowSpan(this.tlpDetailInfo, 2);
            this.tlpDetails.Size = new System.Drawing.Size(966, 180);
            this.tlpDetails.TabIndex = 0;
            // 
            // tlpDetailInfo
            // 
            this.tlpDetailInfo.BackColor = System.Drawing.Color.White;
            this.tlpDetailInfo.ColumnCount = 1;
            this.tlpDetailInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetailInfo.Controls.Add(this.lblDetailClient, 0, 0);
            this.tlpDetailInfo.Controls.Add(this.lblDetailSchedule, 0, 1);
            this.tlpDetailInfo.Controls.Add(this.lblDetailPayment, 0, 2);
            this.tlpDetailInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetailInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpDetailInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDetailInfo.Name = "tlpDetailInfo";
            this.tlpDetailInfo.RowCount = 3;
            this.tlpDetailInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpDetailInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpDetailInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpDetailInfo.Size = new System.Drawing.Size(356, 180);
            this.tlpDetailInfo.TabIndex = 0;
            // 
            // lblDetailClient
            // 
            this.lblDetailClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailClient.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailClient.Location = new System.Drawing.Point(0, 0);
            this.lblDetailClient.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailClient.Name = "lblDetailClient";
            this.lblDetailClient.Size = new System.Drawing.Size(356, 32);
            this.lblDetailClient.TabIndex = 0;
            this.lblDetailClient.Text = "Select an appointment";
            this.lblDetailClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailSchedule
            // 
            this.lblDetailSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailSchedule.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailSchedule.Location = new System.Drawing.Point(0, 32);
            this.lblDetailSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailSchedule.Name = "lblDetailSchedule";
            this.lblDetailSchedule.Size = new System.Drawing.Size(356, 24);
            this.lblDetailSchedule.TabIndex = 1;
            this.lblDetailSchedule.Text = "Schedule: -";
            this.lblDetailSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPayment
            // 
            this.lblDetailPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPayment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailPayment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailPayment.Location = new System.Drawing.Point(0, 56);
            this.lblDetailPayment.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailPayment.Name = "lblDetailPayment";
            this.lblDetailPayment.Size = new System.Drawing.Size(356, 24);
            this.lblDetailPayment.TabIndex = 2;
            this.lblDetailPayment.Text = "Payment: -";
            this.lblDetailPayment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvDetailServices
            // 
            this.dgvDetailServices.AllowUserToAddRows = false;
            this.dgvDetailServices.AllowUserToDeleteRows = false;
            this.dgvDetailServices.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDetailServices.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDetailServices.AutoGenerateColumns = false;
            this.dgvDetailServices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetailServices.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetailServices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvDetailServices.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDetailServices.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetailServices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDetailServices.ColumnHeadersHeight = 30;
            this.dgvDetailServices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailService,
            this.colDetailQty,
            this.colDetailPrice,
            this.colDetailStaff,
            this.colDetailStatus});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetailServices.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDetailServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetailServices.EnableHeadersVisualStyles = false;
            this.dgvDetailServices.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvDetailServices.Location = new System.Drawing.Point(368, 0);
            this.dgvDetailServices.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.dgvDetailServices.MultiSelect = false;
            this.dgvDetailServices.Name = "dgvDetailServices";
            this.dgvDetailServices.ReadOnly = true;
            this.dgvDetailServices.RowHeadersVisible = false;
            this.dgvDetailServices.RowTemplate.Height = 26;
            this.dgvDetailServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetailServices.Size = new System.Drawing.Size(598, 122);
            this.dgvDetailServices.TabIndex = 1;
            this.dgvDetailServices.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDetailServices_CellFormatting);
            // 
            // colDetailService
            // 
            this.colDetailService.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDetailService.DataPropertyName = "service_name";
            this.colDetailService.FillWeight = 32F;
            this.colDetailService.HeaderText = "Service";
            this.colDetailService.Name = "colDetailService";
            this.colDetailService.ReadOnly = true;
            // 
            // colDetailQty
            // 
            this.colDetailQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colDetailQty.DefaultCellStyle = dataGridViewCellStyle7;
            this.colDetailQty.DataPropertyName = "quantity";
            this.colDetailQty.FillWeight = 9F;
            this.colDetailQty.HeaderText = "Qty";
            this.colDetailQty.MinimumWidth = 44;
            this.colDetailQty.Name = "colDetailQty";
            this.colDetailQty.ReadOnly = true;
            // 
            // colDetailPrice
            // 
            this.colDetailPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDetailPrice.DataPropertyName = "price";
            this.colDetailPrice.DefaultCellStyle.Format = "N2";
            this.colDetailPrice.FillWeight = 16F;
            this.colDetailPrice.HeaderText = "Price";
            this.colDetailPrice.MinimumWidth = 70;
            this.colDetailPrice.Name = "colDetailPrice";
            this.colDetailPrice.ReadOnly = true;
            // 
            // colDetailStaff
            // 
            this.colDetailStaff.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDetailStaff.DataPropertyName = "staff_name";
            this.colDetailStaff.FillWeight = 25F;
            this.colDetailStaff.HeaderText = "Technician";
            this.colDetailStaff.MinimumWidth = 80;
            this.colDetailStaff.Name = "colDetailStaff";
            this.colDetailStaff.ReadOnly = true;
            // 
            // colDetailStatus
            // 
            this.colDetailStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colDetailStatus.DefaultCellStyle = dataGridViewCellStyle8;
            this.colDetailStatus.DataPropertyName = "status";
            this.colDetailStatus.FillWeight = 18F;
            this.colDetailStatus.HeaderText = "Status";
            this.colDetailStatus.MinimumWidth = 70;
            this.colDetailStatus.Name = "colDetailStatus";
            this.colDetailStatus.ReadOnly = true;
            // 
            // tlpNotes
            // 
            this.tlpNotes.BackColor = System.Drawing.Color.White;
            this.tlpNotes.ColumnCount = 1;
            this.tlpNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNotes.Controls.Add(this.lblNotesTitle, 0, 0);
            this.tlpNotes.Controls.Add(this.txtDetailNotes, 0, 1);
            this.tlpNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpNotes.Location = new System.Drawing.Point(368, 130);
            this.tlpNotes.Margin = new System.Windows.Forms.Padding(12, 8, 0, 0);
            this.tlpNotes.Name = "tlpNotes";
            this.tlpNotes.RowCount = 2;
            this.tlpNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNotes.Size = new System.Drawing.Size(598, 50);
            this.tlpNotes.TabIndex = 2;
            // 
            // lblNotesTitle
            // 
            this.lblNotesTitle.AutoSize = true;
            this.lblNotesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblNotesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblNotesTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotesTitle.Name = "lblNotesTitle";
            this.lblNotesTitle.Size = new System.Drawing.Size(48, 19);
            this.lblNotesTitle.TabIndex = 0;
            this.lblNotesTitle.Text = "Notes";
            // 
            // txtDetailNotes
            // 
            this.txtDetailNotes.BackColor = System.Drawing.Color.White;
            this.txtDetailNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailNotes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailNotes.Location = new System.Drawing.Point(0, 22);
            this.txtDetailNotes.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetailNotes.Multiline = true;
            this.txtDetailNotes.Name = "txtDetailNotes";
            this.txtDetailNotes.ReadOnly = true;
            this.txtDetailNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailNotes.Size = new System.Drawing.Size(598, 28);
            this.txtDetailNotes.TabIndex = 1;
            // 
            // ucAppointments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "ucAppointments";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucAppointments_Load);
            this.pnlToolbar.ResumeLayout(false);
            this.tlpToolbar.ResumeLayout(false);
            this.tlpToolbar.PerformLayout();
            this.flpNav.ResumeLayout(false);
            this.flpNav.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.tlpDetails.ResumeLayout(false);
            this.tlpDetailInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetailServices)).EndInit();
            this.tlpNotes.ResumeLayout(false);
            this.tlpNotes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TableLayoutPanel tlpToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpNav;
        private System.Windows.Forms.Button btnPrevDay;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnNextDay;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnTakePayment;
        private System.Windows.Forms.Button btnReschedule;
        private System.Windows.Forms.Button btnCancelAppointment;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAppointmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTech;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayment;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tlpDetails;
        private System.Windows.Forms.TableLayoutPanel tlpDetailInfo;
        private System.Windows.Forms.Label lblDetailClient;
        private System.Windows.Forms.Label lblDetailSchedule;
        private System.Windows.Forms.Label lblDetailPayment;
        private System.Windows.Forms.DataGridView dgvDetailServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailStaff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailStatus;
        private System.Windows.Forms.TableLayoutPanel tlpNotes;
        private System.Windows.Forms.Label lblNotesTitle;
        private System.Windows.Forms.TextBox txtDetailNotes;
    }
}
