namespace LittleEreseSystemV3.Controls
{
    partial class ucBilling
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabBilling = new System.Windows.Forms.TabControl();
            this.tabCheckout = new System.Windows.Forms.TabPage();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlQueueGrid = new System.Windows.Forms.Panel();
            this.dgvQueue = new System.Windows.Forms.DataGridView();
            this.colQueueAppointmentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueuePayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueueBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlQueueToolbar = new System.Windows.Forms.Panel();
            this.tlpQueueToolbar = new System.Windows.Forms.TableLayoutPanel();
            this.flpQueueTop = new System.Windows.Forms.FlowLayoutPanel();
            this.lblQueueDate = new System.Windows.Forms.Label();
            this.dtpQueueDate = new System.Windows.Forms.DateTimePicker();
            this.btnQueueToday = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkAllBalances = new System.Windows.Forms.CheckBox();
            this.flpQueueFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblQueueSearch = new System.Windows.Forms.Label();
            this.txtQueueSearch = new System.Windows.Forms.TextBox();
            this.chkPaidList = new System.Windows.Forms.CheckBox();
            this.lblQueueCount = new System.Windows.Forms.Label();
            this.pnlBill = new System.Windows.Forms.Panel();
            this.tlpBill = new System.Windows.Forms.TableLayoutPanel();
            this.lblBillClient = new System.Windows.Forms.Label();
            this.lblBillSchedule = new System.Windows.Forms.Label();
            this.dgvBillLines = new System.Windows.Forms.DataGridView();
            this.colLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLinePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineSession = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineTech = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colLineStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpPromo = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPromo = new System.Windows.Forms.Label();
            this.cboPromo = new System.Windows.Forms.ComboBox();
            this.lblPromoNote = new System.Windows.Forms.Label();
            this.tlpTotals = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubtotalTitle = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblDiscountTitle = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblBalanceTitle = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.btnCollect = new System.Windows.Forms.Button();
            this.tlpBillButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnTakePayment = new System.Windows.Forms.Button();
            this.tabTransactions = new System.Windows.Forms.TabPage();
            this.pnlTranGrid = new System.Windows.Forms.Panel();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.colTranBillingId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranStaff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTranStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTranToolbar = new System.Windows.Forms.Panel();
            this.tlpTranToolbar = new System.Windows.Forms.TableLayoutPanel();
            this.flpTranFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.flpTranButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTotals = new System.Windows.Forms.Label();
            this.tabBilling.SuspendLayout();
            this.tabCheckout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlQueueGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).BeginInit();
            this.pnlQueueToolbar.SuspendLayout();
            this.tlpQueueToolbar.SuspendLayout();
            this.flpQueueTop.SuspendLayout();
            this.flpQueueFilters.SuspendLayout();
            this.pnlBill.SuspendLayout();
            this.tlpBill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillLines)).BeginInit();
            this.flpPromo.SuspendLayout();
            this.tlpTotals.SuspendLayout();
            this.tlpBillButtons.SuspendLayout();
            this.tabTransactions.SuspendLayout();
            this.pnlTranGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.pnlTranToolbar.SuspendLayout();
            this.tlpTranToolbar.SuspendLayout();
            this.flpTranFilters.SuspendLayout();
            this.flpTranButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabBilling
            // 
            this.tabBilling.Controls.Add(this.tabCheckout);
            this.tabBilling.Controls.Add(this.tabTransactions);
            this.tabBilling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabBilling.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabBilling.Location = new System.Drawing.Point(0, 0);
            this.tabBilling.Name = "tabBilling";
            this.tabBilling.SelectedIndex = 0;
            this.tabBilling.Size = new System.Drawing.Size(1000, 640);
            this.tabBilling.TabIndex = 0;
            // 
            // tabCheckout
            // 
            this.tabCheckout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.tabCheckout.Controls.Add(this.splitMain);
            this.tabCheckout.Location = new System.Drawing.Point(4, 29);
            this.tabCheckout.Name = "tabCheckout";
            this.tabCheckout.Padding = new System.Windows.Forms.Padding(3);
            this.tabCheckout.Size = new System.Drawing.Size(992, 607);
            this.tabCheckout.TabIndex = 0;
            this.tabCheckout.Text = "Checkout";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(3, 3);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlQueueGrid);
            this.splitMain.Panel1.Controls.Add(this.pnlQueueToolbar);
            // The left panel must stay at least as wide as the queue toolbar's widest row
            // (the two checkboxes side by side), so nothing is ever cut off.
            this.splitMain.Panel1MinSize = 380;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlBill);
            this.splitMain.Panel2MinSize = 320;
            this.splitMain.Size = new System.Drawing.Size(986, 601);
            // The queue side is wide enough for the all-dates words ("Nov 19 - 3:00 PM - 3:30 PM",
            // "Aquino, Carlo Miguel"), so those rows are never cut off at 1366 x 768; the bill card
            // keeps its own width and scrolls when the window is smaller.
            this.splitMain.SplitterDistance = 660;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 0;
            // 
            // pnlQueueGrid
            // 
            this.pnlQueueGrid.BackColor = System.Drawing.Color.White;
            this.pnlQueueGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQueueGrid.Controls.Add(this.dgvQueue);
            this.pnlQueueGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQueueGrid.Location = new System.Drawing.Point(0, 76);
            this.pnlQueueGrid.Name = "pnlQueueGrid";
            this.pnlQueueGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlQueueGrid.Size = new System.Drawing.Size(394, 525);
            this.pnlQueueGrid.TabIndex = 1;
            // 
            // dgvQueue
            // 
            this.dgvQueue.AllowUserToAddRows = false;
            this.dgvQueue.AllowUserToDeleteRows = false;
            this.dgvQueue.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvQueue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvQueue.AutoGenerateColumns = false;
            this.dgvQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQueue.BackgroundColor = System.Drawing.Color.White;
            this.dgvQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvQueue.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvQueue.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvQueue.ColumnHeadersHeight = 34;
            this.dgvQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQueueAppointmentId,
            this.colQueueTime,
            this.colQueueClient,
            this.colQueueServices,
            this.colQueueStatus,
            this.colQueuePayment,
            this.colQueueBalance});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQueue.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQueue.EnableHeadersVisualStyles = false;
            this.dgvQueue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvQueue.Location = new System.Drawing.Point(1, 1);
            this.dgvQueue.MultiSelect = false;
            this.dgvQueue.Name = "dgvQueue";
            this.dgvQueue.ReadOnly = true;
            this.dgvQueue.RowHeadersVisible = false;
            this.dgvQueue.RowTemplate.Height = 30;
            this.dgvQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvQueue.Size = new System.Drawing.Size(390, 521);
            this.dgvQueue.TabIndex = 0;
            this.dgvQueue.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvQueue_CellFormatting);
            this.dgvQueue.SelectionChanged += new System.EventHandler(this.dgvQueue_SelectionChanged);
            // 
            // colQueueAppointmentId
            // 
            this.colQueueAppointmentId.DataPropertyName = "appointment_id";
            this.colQueueAppointmentId.HeaderText = "ID";
            this.colQueueAppointmentId.Name = "colQueueAppointmentId";
            this.colQueueAppointmentId.ReadOnly = true;
            this.colQueueAppointmentId.Visible = false;
            // 
            // colQueueTime
            // 
            this.colQueueTime.DataPropertyName = "time_range";
            this.colQueueTime.FillWeight = 30F;
            this.colQueueTime.HeaderText = "Time";
            this.colQueueTime.MinimumWidth = 176;
            this.colQueueTime.Name = "colQueueTime";
            this.colQueueTime.ReadOnly = true;
            this.colQueueTime.Width = 118;
            // 
            // colQueueClient
            // 
            this.colQueueClient.DataPropertyName = "client_name";
            this.colQueueClient.FillWeight = 22F;
            this.colQueueClient.HeaderText = "Client";
            this.colQueueClient.MinimumWidth = 150;
            this.colQueueClient.Name = "colQueueClient";
            this.colQueueClient.ReadOnly = true;
            this.colQueueClient.Width = 120;
            // 
            // colQueueServices
            // 
            this.colQueueServices.DataPropertyName = "services";
            this.colQueueServices.FillWeight = 18F;
            this.colQueueServices.HeaderText = "Services";
            this.colQueueServices.MinimumWidth = 84;
            this.colQueueServices.Name = "colQueueServices";
            this.colQueueServices.ReadOnly = true;
            this.colQueueServices.Width = 120;
            // 
            // colQueueStatus
            // 
            this.colQueueStatus.DataPropertyName = "status";
            this.colQueueStatus.FillWeight = 10F;
            this.colQueueStatus.HeaderText = "Status";
            this.colQueueStatus.MinimumWidth = 68;
            this.colQueueStatus.Name = "colQueueStatus";
            this.colQueueStatus.ReadOnly = true;
            this.colQueueStatus.Width = 80;
            // 
            // colQueuePayment
            // 
            this.colQueuePayment.DataPropertyName = "payment_status";
            this.colQueuePayment.FillWeight = 8F;
            this.colQueuePayment.HeaderText = "Payment";
            this.colQueuePayment.MinimumWidth = 72;
            this.colQueuePayment.Name = "colQueuePayment";
            this.colQueuePayment.ReadOnly = true;
            this.colQueuePayment.Width = 80;
            // 
            // colQueueBalance
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colQueueBalance.DefaultCellStyle = dataGridViewCellStyle4;
            this.colQueueBalance.DataPropertyName = "balance_due";
            this.colQueueBalance.FillWeight = 10F;
            this.colQueueBalance.HeaderText = "Balance";
            this.colQueueBalance.MinimumWidth = 80;
            this.colQueueBalance.Name = "colQueueBalance";
            this.colQueueBalance.ReadOnly = true;
            this.colQueueBalance.Width = 90;
            // 
            // pnlQueueToolbar
            // 
            this.pnlQueueToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlQueueToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlQueueToolbar.Controls.Add(this.tlpQueueToolbar);
            this.pnlQueueToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQueueToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlQueueToolbar.Name = "pnlQueueToolbar";
            this.pnlQueueToolbar.Size = new System.Drawing.Size(394, 118);
            this.pnlQueueToolbar.TabIndex = 0;
            // 
            // tlpQueueToolbar
            // 
            this.tlpQueueToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpQueueToolbar.ColumnCount = 2;
            this.tlpQueueToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tlpQueueToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpQueueToolbar.Controls.Add(this.lblQueueSearch, 0, 1);
            this.tlpQueueToolbar.Controls.Add(this.txtQueueSearch, 1, 1);
            this.tlpQueueToolbar.Controls.Add(this.flpQueueFilters, 0, 2);
            this.tlpQueueToolbar.Controls.Add(this.lblQueueCount, 0, 3);
            this.tlpQueueToolbar.Controls.Add(this.flpQueueTop, 0, 0);
            this.tlpQueueToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpQueueToolbar.Location = new System.Drawing.Point(0, 0);
            this.tlpQueueToolbar.Name = "tlpQueueToolbar";
            this.tlpQueueToolbar.RowCount = 4;
            this.tlpQueueToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpQueueToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpQueueToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpQueueToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpQueueToolbar.SetColumnSpan(this.flpQueueTop, 2);
            this.tlpQueueToolbar.SetColumnSpan(this.flpQueueFilters, 2);
            this.tlpQueueToolbar.SetColumnSpan(this.lblQueueCount, 2);
            this.tlpQueueToolbar.Size = new System.Drawing.Size(392, 116);
            this.tlpQueueToolbar.TabIndex = 0;
            // 
            // flpQueueTop
            // 
            this.flpQueueTop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpQueueTop.AutoSize = true;
            this.flpQueueTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpQueueTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpQueueTop.Controls.Add(this.lblQueueDate);
            this.flpQueueTop.Controls.Add(this.dtpQueueDate);
            this.flpQueueTop.Controls.Add(this.btnQueueToday);
            this.flpQueueTop.Controls.Add(this.btnRefresh);
            this.flpQueueTop.Location = new System.Drawing.Point(12, 5);
            this.flpQueueTop.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpQueueTop.Name = "flpQueueTop";
            this.flpQueueTop.Size = new System.Drawing.Size(334, 30);
            this.flpQueueTop.TabIndex = 0;
            this.flpQueueTop.WrapContents = false;
            // 
            // lblQueueSearch
            // 
            this.lblQueueSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblQueueSearch.AutoSize = true;
            this.lblQueueSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueueSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblQueueSearch.Location = new System.Drawing.Point(12, 46);
            this.lblQueueSearch.Margin = new System.Windows.Forms.Padding(12, 0, 6, 0);
            this.lblQueueSearch.Name = "lblQueueSearch";
            this.lblQueueSearch.Size = new System.Drawing.Size(43, 19);
            this.lblQueueSearch.TabIndex = 4;
            this.lblQueueSearch.Text = "Client";
            this.lblQueueSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtQueueSearch
            // 
            this.txtQueueSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQueueSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQueueSearch.Location = new System.Drawing.Point(61, 43);
            this.txtQueueSearch.Margin = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.txtQueueSearch.MinimumSize = new System.Drawing.Size(180, 25);
            this.txtQueueSearch.Name = "txtQueueSearch";
            this.txtQueueSearch.Size = new System.Drawing.Size(318, 25);
            this.txtQueueSearch.TabIndex = 5;
            this.txtQueueSearch.TextChanged += new System.EventHandler(this.txtQueueSearch_TextChanged);
            // 
            // flpQueueFilters
            // 
            this.flpQueueFilters.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpQueueFilters.AutoSize = true;
            this.flpQueueFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpQueueFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpQueueFilters.Controls.Add(this.chkAllBalances);
            this.flpQueueFilters.Controls.Add(this.chkPaidList);
            this.flpQueueFilters.Location = new System.Drawing.Point(12, 74);
            this.flpQueueFilters.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpQueueFilters.Name = "flpQueueFilters";
            this.flpQueueFilters.Size = new System.Drawing.Size(360, 21);
            this.flpQueueFilters.TabIndex = 6;
            this.flpQueueFilters.WrapContents = true;
            // 
            // lblQueueDate
            // 
            this.lblQueueDate.AutoSize = true;
            this.lblQueueDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueueDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblQueueDate.Location = new System.Drawing.Point(0, 6);
            this.lblQueueDate.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.lblQueueDate.Name = "lblQueueDate";
            this.lblQueueDate.Size = new System.Drawing.Size(38, 19);
            this.lblQueueDate.TabIndex = 0;
            this.lblQueueDate.Text = "Date";
            this.lblQueueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpQueueDate
            // 
            this.dtpQueueDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpQueueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpQueueDate.Location = new System.Drawing.Point(44, 3);
            this.dtpQueueDate.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.dtpQueueDate.Name = "dtpQueueDate";
            this.dtpQueueDate.Size = new System.Drawing.Size(132, 25);
            this.dtpQueueDate.TabIndex = 1;
            this.dtpQueueDate.ValueChanged += new System.EventHandler(this.dtpQueueDate_ValueChanged);
            // 
            // btnQueueToday
            // 
            this.btnQueueToday.BackColor = System.Drawing.Color.White;
            this.btnQueueToday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQueueToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnQueueToday.FlatAppearance.BorderSize = 1;
            this.btnQueueToday.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnQueueToday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnQueueToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueueToday.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueueToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnQueueToday.Location = new System.Drawing.Point(180, 0);
            this.btnQueueToday.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnQueueToday.Name = "btnQueueToday";
            this.btnQueueToday.Size = new System.Drawing.Size(74, 30);
            this.btnQueueToday.TabIndex = 2;
            this.btnQueueToday.Text = "Today";
            this.btnQueueToday.UseVisualStyleBackColor = false;
            this.btnQueueToday.Click += new System.EventHandler(this.btnQueueToday_Click);
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
            this.btnRefresh.Location = new System.Drawing.Point(258, 0);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkAllBalances
            // 
            this.chkAllBalances.AutoSize = true;
            this.chkAllBalances.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAllBalances.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllBalances.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.chkAllBalances.Location = new System.Drawing.Point(0, 0);
            this.chkAllBalances.Margin = new System.Windows.Forms.Padding(0, 1, 12, 0);
            this.chkAllBalances.Name = "chkAllBalances";
            this.chkAllBalances.Size = new System.Drawing.Size(160, 19);
            this.chkAllBalances.TabIndex = 0;
            this.chkAllBalances.Text = "Unpaid balances (all dates)";
            this.chkAllBalances.UseVisualStyleBackColor = true;
            this.chkAllBalances.CheckedChanged += new System.EventHandler(this.chkAllBalances_CheckedChanged);
            // 
            // chkPaidList
            // 
            this.chkPaidList.AutoSize = true;
            this.chkPaidList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPaidList.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPaidList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.chkPaidList.Location = new System.Drawing.Point(172, 0);
            this.chkPaidList.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.chkPaidList.Name = "chkPaidList";
            this.chkPaidList.Size = new System.Drawing.Size(188, 19);
            this.chkPaidList.TabIndex = 1;
            this.chkPaidList.Text = "Completed and paid (all dates)";
            this.chkPaidList.UseVisualStyleBackColor = true;
            this.chkPaidList.CheckedChanged += new System.EventHandler(this.chkPaidList_CheckedChanged);
            // 
            // lblQueueCount
            // 
            this.lblQueueCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblQueueCount.AutoSize = true;
            this.lblQueueCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueueCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblQueueCount.Location = new System.Drawing.Point(12, 100);
            this.lblQueueCount.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblQueueCount.Name = "lblQueueCount";
            this.lblQueueCount.Size = new System.Drawing.Size(81, 15);
            this.lblQueueCount.TabIndex = 7;
            this.lblQueueCount.Text = "0 in the queue";
            this.lblQueueCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBill
            // 
            this.pnlBill.AutoScroll = true;
            this.pnlBill.BackColor = System.Drawing.Color.White;
            this.pnlBill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBill.Controls.Add(this.tlpBill);
            this.pnlBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBill.Location = new System.Drawing.Point(0, 0);
            this.pnlBill.Name = "pnlBill";
            this.pnlBill.Padding = new System.Windows.Forms.Padding(16);
            this.pnlBill.Size = new System.Drawing.Size(586, 601);
            this.pnlBill.TabIndex = 0;
            // 
            // tlpBill
            // 
            this.tlpBill.AutoSize = true;
            this.tlpBill.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBill.BackColor = System.Drawing.Color.White;
            this.tlpBill.ColumnCount = 1;
            this.tlpBill.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBill.Controls.Add(this.lblBillClient, 0, 0);
            this.tlpBill.Controls.Add(this.lblBillSchedule, 0, 1);
            this.tlpBill.Controls.Add(this.dgvBillLines, 0, 2);
            this.tlpBill.Controls.Add(this.flpPromo, 0, 3);
            this.tlpBill.Controls.Add(this.lblPromoNote, 0, 4);
            this.tlpBill.Controls.Add(this.tlpTotals, 0, 5);
            this.tlpBill.Controls.Add(this.tlpBillButtons, 0, 6);
            this.tlpBill.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpBill.Location = new System.Drawing.Point(16, 16);
            this.tlpBill.Name = "tlpBill";
            this.tlpBill.RowCount = 7;
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tlpBill.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tlpBill.Size = new System.Drawing.Size(552, 490);
            this.tlpBill.TabIndex = 0;
            // 
            // tlpBillButtons
            // 
            this.tlpBillButtons.ColumnCount = 2;
            this.tlpBillButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBillButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBillButtons.Controls.Add(this.btnCollect, 0, 0);
            this.tlpBillButtons.Controls.Add(this.btnTakePayment, 1, 0);
            this.tlpBillButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBillButtons.Location = new System.Drawing.Point(0, 446);
            this.tlpBillButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBillButtons.Name = "tlpBillButtons";
            this.tlpBillButtons.RowCount = 1;
            this.tlpBillButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBillButtons.Size = new System.Drawing.Size(552, 50);
            this.tlpBillButtons.TabIndex = 6;
            // 
            // lblBillClient
            // 
            this.lblBillClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillClient.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblBillClient.Location = new System.Drawing.Point(0, 0);
            this.lblBillClient.Margin = new System.Windows.Forms.Padding(0);
            this.lblBillClient.Name = "lblBillClient";
            this.lblBillClient.Size = new System.Drawing.Size(552, 34);
            this.lblBillClient.TabIndex = 0;
            this.lblBillClient.Text = "Select an appointment";
            this.lblBillClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBillSchedule
            // 
            this.lblBillSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillSchedule.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblBillSchedule.Location = new System.Drawing.Point(0, 34);
            this.lblBillSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.lblBillSchedule.Name = "lblBillSchedule";
            this.lblBillSchedule.Size = new System.Drawing.Size(552, 24);
            this.lblBillSchedule.TabIndex = 1;
            this.lblBillSchedule.Text = "Schedule: -";
            this.lblBillSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvBillLines
            // 
            this.dgvBillLines.AllowUserToAddRows = false;
            this.dgvBillLines.AllowUserToDeleteRows = false;
            this.dgvBillLines.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dgvBillLines.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvBillLines.AutoGenerateColumns = false;
            this.dgvBillLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBillLines.BackgroundColor = System.Drawing.Color.White;
            this.dgvBillLines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvBillLines.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBillLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBillLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvBillLines.ColumnHeadersHeight = 30;
            this.dgvBillLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLineId,
            this.colLineService,
            this.colLineQty,
            this.colLinePrice,
            this.colLineSession,
            this.colLineTech,
            this.colLineStatus});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBillLines.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvBillLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBillLines.EnableHeadersVisualStyles = false;
            this.dgvBillLines.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvBillLines.Location = new System.Drawing.Point(0, 66);
            this.dgvBillLines.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.dgvBillLines.MultiSelect = false;
            this.dgvBillLines.Name = "dgvBillLines";
            this.dgvBillLines.RowHeadersVisible = false;
            this.dgvBillLines.RowTemplate.Height = 26;
            this.dgvBillLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBillLines.Size = new System.Drawing.Size(552, 150);
            this.dgvBillLines.TabIndex = 2;
            this.dgvBillLines.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBillLines_CellValueChanged);
            this.dgvBillLines.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvBillLines_CurrentCellDirtyStateChanged);
            // 
            // colLineId
            // 
            this.colLineId.DataPropertyName = "appointment_service_id";
            this.colLineId.HeaderText = "ID";
            this.colLineId.Name = "colLineId";
            this.colLineId.ReadOnly = true;
            this.colLineId.Visible = false;
            // 
            // colLineService
            // 
            this.colLineService.DataPropertyName = "service_name";
            this.colLineService.FillWeight = 30F;
            this.colLineService.HeaderText = "Service";
            this.colLineService.MinimumWidth = 90;
            this.colLineService.Name = "colLineService";
            this.colLineService.ReadOnly = true;
            this.colLineService.Width = 140;
            // 
            // colLineQty
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colLineQty.DefaultCellStyle = dataGridViewCellStyle8;
            this.colLineQty.DataPropertyName = "quantity";
            this.colLineQty.FillWeight = 8F;
            this.colLineQty.HeaderText = "Qty";
            this.colLineQty.MinimumWidth = 44;
            this.colLineQty.Name = "colLineQty";
            this.colLineQty.ReadOnly = true;
            this.colLineQty.Width = 50;
            // 
            // colLinePrice
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colLinePrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.colLinePrice.DataPropertyName = "price";
            this.colLinePrice.FillWeight = 15F;
            this.colLinePrice.HeaderText = "Price";
            this.colLinePrice.MinimumWidth = 70;
            this.colLinePrice.Name = "colLinePrice";
            this.colLinePrice.ReadOnly = true;
            this.colLinePrice.Width = 80;
            // 
            // colLineSession
            // 
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colLineSession.DefaultCellStyle = dataGridViewCellStyle10;
            this.colLineSession.DataPropertyName = "session_label";
            this.colLineSession.FillWeight = 15F;
            this.colLineSession.HeaderText = "Session";
            this.colLineSession.MinimumWidth = 70;
            this.colLineSession.Name = "colLineSession";
            this.colLineSession.ReadOnly = true;
            this.colLineSession.Width = 85;
            // 
            // colLineTech
            // 
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.colLineTech.DefaultCellStyle = dataGridViewCellStyle11;
            this.colLineTech.DataPropertyName = "technician_id";
            this.colLineTech.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.DropDownButton;
            this.colLineTech.FillWeight = 20F;
            this.colLineTech.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colLineTech.HeaderText = "Attending staff";
            this.colLineTech.MinimumWidth = 120;
            this.colLineTech.Name = "colLineTech";
            this.colLineTech.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLineTech.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLineTech.Width = 130;
            // 
            // colLineStatus
            // 
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colLineStatus.DefaultCellStyle = dataGridViewCellStyle12;
            this.colLineStatus.DataPropertyName = "line_status";
            this.colLineStatus.FillWeight = 12F;
            this.colLineStatus.HeaderText = "Status";
            this.colLineStatus.MinimumWidth = 70;
            this.colLineStatus.Name = "colLineStatus";
            this.colLineStatus.ReadOnly = true;
            this.colLineStatus.Width = 80;
            // 
            // flpPromo
            // 
            this.flpPromo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpPromo.AutoSize = true;
            this.flpPromo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPromo.BackColor = System.Drawing.Color.White;
            this.flpPromo.Controls.Add(this.lblPromo);
            this.flpPromo.Controls.Add(this.cboPromo);
            this.flpPromo.Location = new System.Drawing.Point(0, 224);
            this.flpPromo.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.flpPromo.Name = "flpPromo";
            this.flpPromo.Size = new System.Drawing.Size(336, 30);
            this.flpPromo.TabIndex = 3;
            this.flpPromo.WrapContents = false;
            // 
            // lblPromo
            // 
            this.lblPromo.AutoSize = true;
            this.lblPromo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblPromo.Location = new System.Drawing.Point(0, 6);
            this.lblPromo.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.lblPromo.Name = "lblPromo";
            this.lblPromo.Size = new System.Drawing.Size(48, 19);
            this.lblPromo.TabIndex = 0;
            this.lblPromo.Text = "Promo";
            this.lblPromo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPromo
            // 
            this.cboPromo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPromo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPromo.FormattingEnabled = true;
            this.cboPromo.Location = new System.Drawing.Point(54, 3);
            this.cboPromo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.cboPromo.Name = "cboPromo";
            this.cboPromo.Size = new System.Drawing.Size(282, 25);
            this.cboPromo.TabIndex = 1;
            this.cboPromo.SelectedIndexChanged += new System.EventHandler(this.cboPromo_SelectedIndexChanged);
            // 
            // lblPromoNote
            // 
            this.lblPromoNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPromoNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromoNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPromoNote.Location = new System.Drawing.Point(0, 254);
            this.lblPromoNote.Margin = new System.Windows.Forms.Padding(0);
            this.lblPromoNote.Name = "lblPromoNote";
            this.lblPromoNote.Size = new System.Drawing.Size(552, 32);
            this.lblPromoNote.TabIndex = 4;
            this.lblPromoNote.Text = "No promo applied.";
            this.lblPromoNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tlpTotals
            // 
            this.tlpTotals.BackColor = System.Drawing.Color.White;
            this.tlpTotals.ColumnCount = 2;
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpTotals.Controls.Add(this.lblSubtotalTitle, 0, 0);
            this.tlpTotals.Controls.Add(this.lblSubtotal, 1, 0);
            this.tlpTotals.Controls.Add(this.lblDiscountTitle, 0, 1);
            this.tlpTotals.Controls.Add(this.lblDiscount, 1, 1);
            this.tlpTotals.Controls.Add(this.lblPaidTitle, 0, 2);
            this.tlpTotals.Controls.Add(this.lblPaid, 1, 2);
            this.tlpTotals.Controls.Add(this.lblTotalTitle, 0, 3);
            this.tlpTotals.Controls.Add(this.lblTotal, 1, 3);
            this.tlpTotals.Controls.Add(this.lblBalanceTitle, 0, 4);
            this.tlpTotals.Controls.Add(this.lblBalance, 1, 4);
            this.tlpTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTotals.Location = new System.Drawing.Point(0, 294);
            this.tlpTotals.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpTotals.Name = "tlpTotals";
            this.tlpTotals.RowCount = 5;
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlpTotals.Size = new System.Drawing.Size(552, 140);
            this.tlpTotals.TabIndex = 5;
            // 
            // lblSubtotalTitle
            // 
            this.lblSubtotalTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtotalTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblSubtotalTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSubtotalTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubtotalTitle.Name = "lblSubtotalTitle";
            this.lblSubtotalTitle.Size = new System.Drawing.Size(358, 24);
            this.lblSubtotalTitle.TabIndex = 0;
            this.lblSubtotalTitle.Text = "Subtotal";
            this.lblSubtotalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSubtotal.Location = new System.Drawing.Point(358, 0);
            this.lblSubtotal.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(194, 24);
            this.lblSubtotal.TabIndex = 1;
            this.lblSubtotal.Text = "0.00";
            this.lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDiscountTitle
            // 
            this.lblDiscountTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblDiscountTitle.Location = new System.Drawing.Point(0, 24);
            this.lblDiscountTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblDiscountTitle.Name = "lblDiscountTitle";
            this.lblDiscountTitle.Size = new System.Drawing.Size(358, 24);
            this.lblDiscountTitle.TabIndex = 2;
            this.lblDiscountTitle.Text = "Discount";
            this.lblDiscountTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscount
            // 
            this.lblDiscount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDiscount.Location = new System.Drawing.Point(358, 24);
            this.lblDiscount.Margin = new System.Windows.Forms.Padding(0);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(194, 24);
            this.lblDiscount.TabIndex = 3;
            this.lblDiscount.Text = "0.00";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaidTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaidTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPaidTitle.Location = new System.Drawing.Point(0, 48);
            this.lblPaidTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(358, 24);
            this.lblPaidTitle.TabIndex = 4;
            this.lblPaidTitle.Text = "Paid so far";
            this.lblPaidTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPaid
            // 
            this.lblPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblPaid.Location = new System.Drawing.Point(358, 48);
            this.lblPaid.Margin = new System.Windows.Forms.Padding(0);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(194, 24);
            this.lblPaid.TabIndex = 5;
            this.lblPaid.Text = "0.00";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTotalTitle.Location = new System.Drawing.Point(0, 72);
            this.lblTotalTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(358, 26);
            this.lblTotalTitle.TabIndex = 6;
            this.lblTotalTitle.Text = "Total";
            this.lblTotalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTotal.Location = new System.Drawing.Point(358, 72);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(194, 26);
            this.lblTotal.TabIndex = 7;
            this.lblTotal.Text = "0.00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBalanceTitle
            // 
            this.lblBalanceTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBalanceTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.lblBalanceTitle.Location = new System.Drawing.Point(0, 98);
            this.lblBalanceTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblBalanceTitle.Name = "lblBalanceTitle";
            this.lblBalanceTitle.Size = new System.Drawing.Size(358, 42);
            this.lblBalanceTitle.TabIndex = 8;
            this.lblBalanceTitle.Text = "Balance due";
            this.lblBalanceTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBalance
            // 
            this.lblBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.lblBalance.Location = new System.Drawing.Point(358, 98);
            this.lblBalance.Margin = new System.Windows.Forms.Padding(0);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(194, 42);
            this.lblBalance.TabIndex = 9;
            this.lblBalance.Text = "0.00";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCollect
            // 
            this.btnCollect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnCollect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCollect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCollect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnCollect.FlatAppearance.BorderSize = 1;
            this.btnCollect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnCollect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnCollect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCollect.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCollect.ForeColor = System.Drawing.Color.White;
            this.btnCollect.Location = new System.Drawing.Point(0, 6);
            this.btnCollect.Margin = new System.Windows.Forms.Padding(0, 6, 4, 2);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(272, 42);
            this.btnCollect.TabIndex = 0;
            this.btnCollect.Text = "Collect and Complete";
            this.btnCollect.UseVisualStyleBackColor = false;
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            // 
            // btnTakePayment
            // 
            this.btnTakePayment.BackColor = System.Drawing.Color.White;
            this.btnTakePayment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTakePayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTakePayment.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTakePayment.FlatAppearance.BorderSize = 1;
            this.btnTakePayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnTakePayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnTakePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTakePayment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTakePayment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnTakePayment.Location = new System.Drawing.Point(280, 6);
            this.btnTakePayment.Margin = new System.Windows.Forms.Padding(4, 6, 0, 2);
            this.btnTakePayment.Name = "btnTakePayment";
            this.btnTakePayment.Size = new System.Drawing.Size(272, 42);
            this.btnTakePayment.TabIndex = 1;
            this.btnTakePayment.Text = "Take Payment";
            this.btnTakePayment.UseVisualStyleBackColor = false;
            this.btnTakePayment.Click += new System.EventHandler(this.btnTakePayment_Click);
            // 
            // tabTransactions
            // 
            this.tabTransactions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.tabTransactions.Controls.Add(this.pnlTranGrid);
            this.tabTransactions.Controls.Add(this.pnlTranToolbar);
            this.tabTransactions.Location = new System.Drawing.Point(4, 29);
            this.tabTransactions.Name = "tabTransactions";
            this.tabTransactions.Padding = new System.Windows.Forms.Padding(3);
            this.tabTransactions.Size = new System.Drawing.Size(992, 607);
            this.tabTransactions.TabIndex = 1;
            this.tabTransactions.Text = "Transactions";
            // 
            // pnlTranGrid
            // 
            this.pnlTranGrid.BackColor = System.Drawing.Color.White;
            this.pnlTranGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTranGrid.Controls.Add(this.dgvTransactions);
            this.pnlTranGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTranGrid.Location = new System.Drawing.Point(3, 95);
            this.pnlTranGrid.Name = "pnlTranGrid";
            this.pnlTranGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTranGrid.Size = new System.Drawing.Size(986, 509);
            this.pnlTranGrid.TabIndex = 1;
            // 
            // dgvTransactions
            // 
            this.dgvTransactions.AllowUserToAddRows = false;
            this.dgvTransactions.AllowUserToDeleteRows = false;
            this.dgvTransactions.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.White;
            this.dgvTransactions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvTransactions.AutoGenerateColumns = false;
            this.dgvTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransactions.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransactions.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTransactions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvTransactions.ColumnHeadersHeight = 34;
            this.dgvTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTranBillingId,
            this.colTranDate,
            this.colTranClient,
            this.colTranServices,
            this.colTranTotal,
            this.colTranPaid,
            this.colTranMethod,
            this.colTranRef,
            this.colTranStaff,
            this.colTranStatus});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransactions.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransactions.EnableHeadersVisualStyles = false;
            this.dgvTransactions.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvTransactions.Location = new System.Drawing.Point(1, 1);
            this.dgvTransactions.MultiSelect = false;
            this.dgvTransactions.Name = "dgvTransactions";
            this.dgvTransactions.ReadOnly = true;
            this.dgvTransactions.RowHeadersVisible = false;
            this.dgvTransactions.RowTemplate.Height = 30;
            this.dgvTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransactions.Size = new System.Drawing.Size(982, 505);
            this.dgvTransactions.TabIndex = 0;
            this.dgvTransactions.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTransactions_CellFormatting);
            // 
            // colTranBillingId
            // 
            this.colTranBillingId.DataPropertyName = "billing_id";
            this.colTranBillingId.HeaderText = "ID";
            this.colTranBillingId.Name = "colTranBillingId";
            this.colTranBillingId.ReadOnly = true;
            this.colTranBillingId.Visible = false;
            // 
            // colTranDate
            // 
            this.colTranDate.DataPropertyName = "payment_date";
            this.colTranDate.DefaultCellStyle.Format = "MMM dd, yyyy h:mm tt";
            this.colTranDate.FillWeight = 15F;
            this.colTranDate.HeaderText = "Date";
            this.colTranDate.MinimumWidth = 155;
            this.colTranDate.Name = "colTranDate";
            this.colTranDate.ReadOnly = true;
            this.colTranDate.Width = 160;
            // 
            // colTranClient
            // 
            this.colTranClient.DataPropertyName = "client_name";
            this.colTranClient.FillWeight = 14F;
            this.colTranClient.HeaderText = "Client";
            this.colTranClient.MinimumWidth = 90;
            this.colTranClient.Name = "colTranClient";
            this.colTranClient.ReadOnly = true;
            this.colTranClient.Width = 130;
            // 
            // colTranServices
            // 
            this.colTranServices.DataPropertyName = "services";
            this.colTranServices.FillWeight = 18F;
            this.colTranServices.HeaderText = "Services";
            this.colTranServices.MinimumWidth = 90;
            this.colTranServices.Name = "colTranServices";
            this.colTranServices.ReadOnly = true;
            this.colTranServices.Width = 150;
            // 
            // colTranTotal
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colTranTotal.DefaultCellStyle = dataGridViewCellStyle16;
            this.colTranTotal.DataPropertyName = "total_amount";
            this.colTranTotal.FillWeight = 11F;
            this.colTranTotal.HeaderText = "Total";
            this.colTranTotal.MinimumWidth = 80;
            this.colTranTotal.Name = "colTranTotal";
            this.colTranTotal.ReadOnly = true;
            this.colTranTotal.Width = 90;
            // 
            // colTranPaid
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N2";
            dataGridViewCellStyle17.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colTranPaid.DefaultCellStyle = dataGridViewCellStyle17;
            this.colTranPaid.DataPropertyName = "amount_paid";
            this.colTranPaid.FillWeight = 11F;
            this.colTranPaid.HeaderText = "Paid";
            this.colTranPaid.MinimumWidth = 80;
            this.colTranPaid.Name = "colTranPaid";
            this.colTranPaid.ReadOnly = true;
            this.colTranPaid.Width = 90;
            // 
            // colTranMethod
            // 
            this.colTranMethod.DataPropertyName = "payment_method";
            this.colTranMethod.FillWeight = 10F;
            this.colTranMethod.HeaderText = "Method";
            this.colTranMethod.MinimumWidth = 80;
            this.colTranMethod.Name = "colTranMethod";
            this.colTranMethod.ReadOnly = true;
            this.colTranMethod.Width = 100;
            // 
            // colTranRef
            // 
            this.colTranRef.DataPropertyName = "reference_number";
            this.colTranRef.FillWeight = 12F;
            this.colTranRef.HeaderText = "Reference";
            this.colTranRef.MinimumWidth = 90;
            this.colTranRef.Name = "colTranRef";
            this.colTranRef.ReadOnly = true;
            this.colTranRef.Width = 110;
            // 
            // colTranStaff
            // 
            this.colTranStaff.DataPropertyName = "staff_names";
            this.colTranStaff.FillWeight = 14F;
            this.colTranStaff.HeaderText = "Staff";
            this.colTranStaff.MinimumWidth = 90;
            this.colTranStaff.Name = "colTranStaff";
            this.colTranStaff.ReadOnly = true;
            this.colTranStaff.Width = 130;
            // 
            // colTranStatus
            // 
            this.colTranStatus.DataPropertyName = "payment_status";
            this.colTranStatus.FillWeight = 10F;
            this.colTranStatus.HeaderText = "Status";
            this.colTranStatus.MinimumWidth = 80;
            this.colTranStatus.Name = "colTranStatus";
            this.colTranStatus.ReadOnly = true;
            this.colTranStatus.Width = 90;
            // 
            // pnlTranToolbar
            // 
            this.pnlTranToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlTranToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTranToolbar.Controls.Add(this.tlpTranToolbar);
            this.pnlTranToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTranToolbar.Location = new System.Drawing.Point(3, 3);
            this.pnlTranToolbar.Name = "pnlTranToolbar";
            this.pnlTranToolbar.Size = new System.Drawing.Size(986, 92);
            this.pnlTranToolbar.TabIndex = 0;
            // 
            // tlpTranToolbar
            // 
            this.tlpTranToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpTranToolbar.ColumnCount = 2;
            this.tlpTranToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTranToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTranToolbar.Controls.Add(this.flpTranFilters, 0, 0);
            this.tlpTranToolbar.Controls.Add(this.flpTranButtons, 0, 1);
            this.tlpTranToolbar.Controls.Add(this.lblTotals, 1, 1);
            this.tlpTranToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTranToolbar.Location = new System.Drawing.Point(0, 0);
            this.tlpTranToolbar.Name = "tlpTranToolbar";
            this.tlpTranToolbar.RowCount = 2;
            this.tlpTranToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpTranToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpTranToolbar.Size = new System.Drawing.Size(984, 90);
            this.tlpTranToolbar.TabIndex = 0;
            // 
            // flpTranFilters
            // 
            this.flpTranFilters.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpTranFilters.AutoSize = true;
            this.flpTranFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTranFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpTranFilters.Controls.Add(this.lblFrom);
            this.flpTranFilters.Controls.Add(this.dtpFrom);
            this.flpTranFilters.Controls.Add(this.lblTo);
            this.flpTranFilters.Controls.Add(this.dtpTo);
            this.flpTranFilters.Controls.Add(this.lblSearch);
            this.flpTranFilters.Controls.Add(this.txtSearch);
            this.flpTranFilters.Controls.Add(this.lblMethod);
            this.flpTranFilters.Controls.Add(this.cboMethod);
            this.flpTranFilters.Location = new System.Drawing.Point(12, 8);
            this.flpTranFilters.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpTranFilters.Name = "flpTranFilters";
            this.flpTranFilters.Size = new System.Drawing.Size(808, 30);
            this.flpTranFilters.TabIndex = 0;
            this.flpTranFilters.WrapContents = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblFrom.Location = new System.Drawing.Point(0, 6);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 19);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(46, 3);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(120, 25);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTo.Location = new System.Drawing.Point(170, 6);
            this.lblTo.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(23, 19);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTo
            // 
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(199, 3);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(120, 25);
            this.dtpTo.TabIndex = 3;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSearch.Location = new System.Drawing.Point(335, 6);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(43, 19);
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "Client";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(384, 3);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(180, 25);
            this.txtSearch.TabIndex = 5;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblMethod.Location = new System.Drawing.Point(580, 6);
            this.lblMethod.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(57, 19);
            this.lblMethod.TabIndex = 6;
            this.lblMethod.Text = "Method";
            this.lblMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboMethod
            // 
            this.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMethod.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "All",
            "Cash",
            "GCash",
            "Bank Transfer"});
            this.cboMethod.Location = new System.Drawing.Point(643, 3);
            this.cboMethod.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(160, 25);
            this.cboMethod.TabIndex = 7;
            // 
            // flpTranButtons
            // 
            this.flpTranButtons.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpTranButtons.AutoSize = true;
            this.flpTranButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTranButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpTranButtons.Controls.Add(this.btnSearch);
            this.flpTranButtons.Location = new System.Drawing.Point(12, 53);
            this.flpTranButtons.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpTranButtons.Name = "flpTranButtons";
            this.flpTranButtons.Size = new System.Drawing.Size(94, 30);
            this.flpTranButtons.TabIndex = 1;
            this.flpTranButtons.WrapContents = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSearch.FlatAppearance.BorderSize = 1;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(0, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 30);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTotals
            // 
            this.lblTotals.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotals.AutoSize = true;
            this.lblTotals.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotals.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTotals.Location = new System.Drawing.Point(742, 61);
            this.lblTotals.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.Size = new System.Drawing.Size(230, 19);
            this.lblTotals.TabIndex = 2;
            this.lblTotals.Text = "0 transactions - 0.00";
            this.lblTotals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ucBilling
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.tabBilling);
            this.Name = "ucBilling";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucBilling_Load);
            this.tabBilling.ResumeLayout(false);
            this.tabCheckout.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlQueueGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQueue)).EndInit();
            this.pnlQueueToolbar.ResumeLayout(false);
            this.pnlQueueToolbar.PerformLayout();
            this.tlpQueueToolbar.ResumeLayout(false);
            this.tlpQueueToolbar.PerformLayout();
            this.flpQueueTop.ResumeLayout(false);
            this.flpQueueTop.PerformLayout();
            this.flpQueueFilters.ResumeLayout(false);
            this.flpQueueFilters.PerformLayout();
            this.pnlBill.ResumeLayout(false);
            this.tlpBill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillLines)).EndInit();
            this.flpPromo.ResumeLayout(false);
            this.flpPromo.PerformLayout();
            this.tlpTotals.ResumeLayout(false);
            this.tlpBillButtons.ResumeLayout(false);
            this.tabTransactions.ResumeLayout(false);
            this.pnlTranGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.pnlTranToolbar.ResumeLayout(false);
            this.tlpTranToolbar.ResumeLayout(false);
            this.tlpTranToolbar.PerformLayout();
            this.flpTranFilters.ResumeLayout(false);
            this.flpTranFilters.PerformLayout();
            this.flpTranButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabBilling;
        private System.Windows.Forms.TabPage tabCheckout;
        private System.Windows.Forms.TabPage tabTransactions;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlQueueGrid;
        private System.Windows.Forms.DataGridView dgvQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueAppointmentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueuePayment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueueBalance;
        private System.Windows.Forms.Panel pnlQueueToolbar;
        private System.Windows.Forms.TableLayoutPanel tlpQueueToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpQueueTop;
        private System.Windows.Forms.Label lblQueueDate;
        private System.Windows.Forms.DateTimePicker dtpQueueDate;
        private System.Windows.Forms.Button btnQueueToday;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkAllBalances;
        private System.Windows.Forms.FlowLayoutPanel flpQueueFilters;
        private System.Windows.Forms.Label lblQueueSearch;
        private System.Windows.Forms.TextBox txtQueueSearch;
        private System.Windows.Forms.CheckBox chkPaidList;
        private System.Windows.Forms.Label lblQueueCount;
        private System.Windows.Forms.Panel pnlBill;
        private System.Windows.Forms.TableLayoutPanel tlpBill;
        private System.Windows.Forms.Label lblBillClient;
        private System.Windows.Forms.Label lblBillSchedule;
        private System.Windows.Forms.DataGridView dgvBillLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLinePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineSession;
        private System.Windows.Forms.DataGridViewComboBoxColumn colLineTech;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineStatus;
        private System.Windows.Forms.FlowLayoutPanel flpPromo;
        private System.Windows.Forms.Label lblPromo;
        private System.Windows.Forms.ComboBox cboPromo;
        private System.Windows.Forms.Label lblPromoNote;
        private System.Windows.Forms.TableLayoutPanel tlpTotals;
        private System.Windows.Forms.Label lblSubtotalTitle;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblDiscountTitle;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblBalanceTitle;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Button btnCollect;
        private System.Windows.Forms.TableLayoutPanel tlpBillButtons;
        private System.Windows.Forms.Button btnTakePayment;
        private System.Windows.Forms.Panel pnlTranGrid;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranBillingId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranStaff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTranStatus;
        private System.Windows.Forms.Panel pnlTranToolbar;
        private System.Windows.Forms.TableLayoutPanel tlpTranToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpTranFilters;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.FlowLayoutPanel flpTranButtons;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblTotals;
    }
}
