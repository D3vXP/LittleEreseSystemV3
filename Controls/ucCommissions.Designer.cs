namespace LittleEreseSystemV3.Controls
{
    partial class ucCommissions
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
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.tlpToolbar = new System.Windows.Forms.TableLayoutPanel();
            this.flpFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblStaff = new System.Windows.Forms.Label();
            this.cboStaff = new System.Windows.Forms.ComboBox();
            this.lblPayout = new System.Windows.Forms.Label();
            this.cboPayout = new System.Windows.Forms.ComboBox();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnMarkPaid = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.tlpSummary = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTotalEarned = new System.Windows.Forms.Panel();
            this.lblTotalEarned = new System.Windows.Forms.Label();
            this.lblTotalEarnedTitle = new System.Windows.Forms.Label();
            this.pnlUnpaid = new System.Windows.Forms.Panel();
            this.lblUnpaid = new System.Windows.Forms.Label();
            this.lblUnpaidTitle = new System.Windows.Forms.Label();
            this.pnlPaid = new System.Windows.Forms.Panel();
            this.lblPaid = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlSummaryGrid = new System.Windows.Forms.Panel();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.colSumUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumStaff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumUnpaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetailsGrid = new System.Windows.Forms.Panel();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.colDetCommissionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetStaff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlToolbar.SuspendLayout();
            this.tlpToolbar.SuspendLayout();
            this.flpFilters.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.tlpSummary.SuspendLayout();
            this.pnlTotalEarned.SuspendLayout();
            this.pnlUnpaid.SuspendLayout();
            this.pnlPaid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlSummaryGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            this.pnlDetailsGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlToolbar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.tlpToolbar.ColumnCount = 1;
            this.tlpToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpToolbar.Controls.Add(this.flpFilters, 0, 0);
            this.tlpToolbar.Controls.Add(this.flpButtons, 0, 1);
            this.tlpToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpToolbar.Location = new System.Drawing.Point(0, 0);
            this.tlpToolbar.Name = "tlpToolbar";
            this.tlpToolbar.RowCount = 2;
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpToolbar.Size = new System.Drawing.Size(998, 90);
            this.tlpToolbar.TabIndex = 0;
            // 
            // flpFilters
            // 
            this.flpFilters.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpFilters.AutoSize = true;
            this.flpFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpFilters.Controls.Add(this.lblFrom);
            this.flpFilters.Controls.Add(this.dtpFrom);
            this.flpFilters.Controls.Add(this.lblTo);
            this.flpFilters.Controls.Add(this.dtpTo);
            this.flpFilters.Controls.Add(this.lblStaff);
            this.flpFilters.Controls.Add(this.cboStaff);
            this.flpFilters.Controls.Add(this.lblPayout);
            this.flpFilters.Controls.Add(this.cboPayout);
            this.flpFilters.Location = new System.Drawing.Point(12, 8);
            this.flpFilters.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpFilters.Name = "flpFilters";
            this.flpFilters.Size = new System.Drawing.Size(702, 30);
            this.flpFilters.TabIndex = 0;
            this.flpFilters.WrapContents = false;
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
            this.dtpFrom.Size = new System.Drawing.Size(124, 25);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTo.Location = new System.Drawing.Point(174, 6);
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
            this.dtpTo.Location = new System.Drawing.Point(203, 3);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(124, 25);
            this.dtpTo.TabIndex = 3;
            // 
            // lblStaff
            // 
            this.lblStaff.AutoSize = true;
            this.lblStaff.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStaff.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblStaff.Location = new System.Drawing.Point(343, 6);
            this.lblStaff.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblStaff.Name = "lblStaff";
            this.lblStaff.Size = new System.Drawing.Size(37, 19);
            this.lblStaff.TabIndex = 4;
            this.lblStaff.Text = "Staff";
            this.lblStaff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboStaff
            // 
            this.cboStaff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStaff.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStaff.FormattingEnabled = true;
            this.cboStaff.Location = new System.Drawing.Point(386, 3);
            this.cboStaff.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            this.cboStaff.Name = "cboStaff";
            this.cboStaff.Size = new System.Drawing.Size(180, 25);
            this.cboStaff.TabIndex = 5;
            // 
            // lblPayout
            // 
            this.lblPayout.AutoSize = true;
            this.lblPayout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblPayout.Location = new System.Drawing.Point(582, 6);
            this.lblPayout.Margin = new System.Windows.Forms.Padding(12, 6, 6, 0);
            this.lblPayout.Name = "lblPayout";
            this.lblPayout.Size = new System.Drawing.Size(48, 19);
            this.lblPayout.TabIndex = 6;
            this.lblPayout.Text = "Payout";
            this.lblPayout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPayout
            // 
            this.cboPayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPayout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPayout.FormattingEnabled = true;
            this.cboPayout.Items.AddRange(new object[] {
            "All",
            "Unpaid",
            "Paid"});
            this.cboPayout.Location = new System.Drawing.Point(636, 3);
            this.cboPayout.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.cboPayout.Name = "cboPayout";
            this.cboPayout.Size = new System.Drawing.Size(120, 25);
            this.cboPayout.TabIndex = 7;
            // 
            // flpButtons
            // 
            this.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpButtons.AutoSize = true;
            this.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpButtons.Controls.Add(this.btnSearch);
            this.flpButtons.Controls.Add(this.btnMarkPaid);
            this.flpButtons.Controls.Add(this.btnExport);
            this.flpButtons.Controls.Add(this.btnPrint);
            this.flpButtons.Location = new System.Drawing.Point(12, 53);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(480, 30);
            this.flpButtons.TabIndex = 1;
            this.flpButtons.WrapContents = false;
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
            // btnMarkPaid
            // 
            this.btnMarkPaid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnMarkPaid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMarkPaid.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnMarkPaid.FlatAppearance.BorderSize = 1;
            this.btnMarkPaid.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnMarkPaid.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnMarkPaid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMarkPaid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMarkPaid.ForeColor = System.Drawing.Color.White;
            this.btnMarkPaid.Location = new System.Drawing.Point(94, 0);
            this.btnMarkPaid.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnMarkPaid.Name = "btnMarkPaid";
            this.btnMarkPaid.Size = new System.Drawing.Size(176, 30);
            this.btnMarkPaid.TabIndex = 1;
            this.btnMarkPaid.Text = "Mark Selected as Paid";
            this.btnMarkPaid.UseVisualStyleBackColor = false;
            this.btnMarkPaid.Click += new System.EventHandler(this.btnMarkPaid_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.White;
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnExport.FlatAppearance.BorderSize = 1;
            this.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnExport.Location = new System.Drawing.Point(274, 0);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 30);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrint.FlatAppearance.BorderSize = 1;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnPrint.Location = new System.Drawing.Point(388, 0);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(92, 30);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // pnlSummary
            // 
            this.pnlSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlSummary.Controls.Add(this.tlpSummary);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummary.Location = new System.Drawing.Point(0, 92);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlSummary.Size = new System.Drawing.Size(1000, 96);
            this.pnlSummary.TabIndex = 1;
            // 
            // tlpSummary
            // 
            this.tlpSummary.ColumnCount = 3;
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpSummary.Controls.Add(this.pnlTotalEarned, 0, 0);
            this.tlpSummary.Controls.Add(this.pnlUnpaid, 1, 0);
            this.tlpSummary.Controls.Add(this.pnlPaid, 2, 0);
            this.tlpSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSummary.Location = new System.Drawing.Point(12, 8);
            this.tlpSummary.Name = "tlpSummary";
            this.tlpSummary.RowCount = 1;
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSummary.Size = new System.Drawing.Size(976, 80);
            this.tlpSummary.TabIndex = 0;
            // 
            // pnlTotalEarned
            // 
            this.pnlTotalEarned.BackColor = System.Drawing.Color.White;
            this.pnlTotalEarned.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotalEarned.Controls.Add(this.lblTotalEarned);
            this.pnlTotalEarned.Controls.Add(this.lblTotalEarnedTitle);
            this.pnlTotalEarned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotalEarned.Location = new System.Drawing.Point(0, 0);
            this.pnlTotalEarned.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.pnlTotalEarned.Name = "pnlTotalEarned";
            this.pnlTotalEarned.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlTotalEarned.Size = new System.Drawing.Size(319, 80);
            this.pnlTotalEarned.TabIndex = 0;
            // 
            // lblTotalEarned
            // 
            this.lblTotalEarned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalEarned.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEarned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTotalEarned.Location = new System.Drawing.Point(12, 27);
            this.lblTotalEarned.Name = "lblTotalEarned";
            this.lblTotalEarned.Size = new System.Drawing.Size(293, 43);
            this.lblTotalEarned.TabIndex = 1;
            this.lblTotalEarned.Text = "0.00";
            this.lblTotalEarned.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalEarnedTitle
            // 
            this.lblTotalEarnedTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalEarnedTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalEarnedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblTotalEarnedTitle.Location = new System.Drawing.Point(12, 8);
            this.lblTotalEarnedTitle.Name = "lblTotalEarnedTitle";
            this.lblTotalEarnedTitle.Size = new System.Drawing.Size(293, 19);
            this.lblTotalEarnedTitle.TabIndex = 0;
            this.lblTotalEarnedTitle.Text = "Total commission earned";
            this.lblTotalEarnedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUnpaid
            // 
            this.pnlUnpaid.BackColor = System.Drawing.Color.White;
            this.pnlUnpaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUnpaid.Controls.Add(this.lblUnpaid);
            this.pnlUnpaid.Controls.Add(this.lblUnpaidTitle);
            this.pnlUnpaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUnpaid.Location = new System.Drawing.Point(331, 0);
            this.pnlUnpaid.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.pnlUnpaid.Name = "pnlUnpaid";
            this.pnlUnpaid.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlUnpaid.Size = new System.Drawing.Size(313, 80);
            this.pnlUnpaid.TabIndex = 1;
            // 
            // lblUnpaid
            // 
            this.lblUnpaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnpaid.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnpaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblUnpaid.Location = new System.Drawing.Point(12, 27);
            this.lblUnpaid.Name = "lblUnpaid";
            this.lblUnpaid.Size = new System.Drawing.Size(287, 43);
            this.lblUnpaid.TabIndex = 1;
            this.lblUnpaid.Text = "0.00";
            this.lblUnpaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUnpaidTitle
            // 
            this.lblUnpaidTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUnpaidTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnpaidTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblUnpaidTitle.Location = new System.Drawing.Point(12, 8);
            this.lblUnpaidTitle.Name = "lblUnpaidTitle";
            this.lblUnpaidTitle.Size = new System.Drawing.Size(287, 19);
            this.lblUnpaidTitle.TabIndex = 0;
            this.lblUnpaidTitle.Text = "Unpaid";
            this.lblUnpaidTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPaid
            // 
            this.pnlPaid.BackColor = System.Drawing.Color.White;
            this.pnlPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPaid.Controls.Add(this.lblPaid);
            this.pnlPaid.Controls.Add(this.lblPaidTitle);
            this.pnlPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPaid.Location = new System.Drawing.Point(656, 0);
            this.pnlPaid.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.pnlPaid.Name = "pnlPaid";
            this.pnlPaid.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlPaid.Size = new System.Drawing.Size(320, 80);
            this.pnlPaid.TabIndex = 2;
            // 
            // lblPaid
            // 
            this.lblPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPaid.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(142)))), ((int)(((byte)(90)))));
            this.lblPaid.Location = new System.Drawing.Point(12, 27);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(294, 43);
            this.lblPaid.TabIndex = 1;
            this.lblPaid.Text = "0.00";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPaidTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaidTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPaidTitle.Location = new System.Drawing.Point(12, 8);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(294, 19);
            this.lblPaidTitle.TabIndex = 0;
            this.lblPaidTitle.Text = "Paid";
            this.lblPaidTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 188);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlSummaryGrid);
            this.splitMain.Panel1MinSize = 130;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlDetailsGrid);
            this.splitMain.Panel2MinSize = 160;
            this.splitMain.Size = new System.Drawing.Size(1000, 452);
            this.splitMain.SplitterDistance = 196;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 2;
            // 
            // pnlSummaryGrid
            // 
            this.pnlSummaryGrid.BackColor = System.Drawing.Color.White;
            this.pnlSummaryGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummaryGrid.Controls.Add(this.dgvSummary);
            this.pnlSummaryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSummaryGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlSummaryGrid.Name = "pnlSummaryGrid";
            this.pnlSummaryGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSummaryGrid.Size = new System.Drawing.Size(1000, 196);
            this.pnlSummaryGrid.TabIndex = 0;
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvSummary.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSummary.AutoGenerateColumns = false;
            this.dgvSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgvSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSummary.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSummary.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSummary.ColumnHeadersHeight = 34;
            this.dgvSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSumUserId,
            this.colSumStaff,
            this.colSumCount,
            this.colSumTotal,
            this.colSumUnpaid,
            this.colSumPaid});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSummary.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSummary.EnableHeadersVisualStyles = false;
            this.dgvSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvSummary.Location = new System.Drawing.Point(1, 1);
            this.dgvSummary.MultiSelect = false;
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.RowTemplate.Height = 30;
            this.dgvSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSummary.Size = new System.Drawing.Size(996, 192);
            this.dgvSummary.TabIndex = 0;
            this.dgvSummary.SelectionChanged += new System.EventHandler(this.dgvSummary_SelectionChanged);
            // 
            // colSumUserId
            // 
            this.colSumUserId.DataPropertyName = "user_id";
            this.colSumUserId.HeaderText = "ID";
            this.colSumUserId.Name = "colSumUserId";
            this.colSumUserId.ReadOnly = true;
            this.colSumUserId.Visible = false;
            // 
            // colSumStaff
            // 
            this.colSumStaff.DataPropertyName = "full_name";
            this.colSumStaff.FillWeight = 34F;
            this.colSumStaff.HeaderText = "Staff";
            this.colSumStaff.MinimumWidth = 120;
            this.colSumStaff.Name = "colSumStaff";
            this.colSumStaff.ReadOnly = true;
            this.colSumStaff.Width = 260;
            // 
            // colSumCount
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSumCount.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSumCount.DataPropertyName = "services_count";
            this.colSumCount.FillWeight = 12F;
            this.colSumCount.HeaderText = "Services";
            this.colSumCount.MinimumWidth = 80;
            this.colSumCount.Name = "colSumCount";
            this.colSumCount.ReadOnly = true;
            this.colSumCount.Width = 95;
            // 
            // colSumTotal
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSumTotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSumTotal.DataPropertyName = "total_commission";
            this.colSumTotal.FillWeight = 20F;
            this.colSumTotal.HeaderText = "Total commission";
            this.colSumTotal.MinimumWidth = 120;
            this.colSumTotal.Name = "colSumTotal";
            this.colSumTotal.ReadOnly = true;
            this.colSumTotal.Width = 150;
            // 
            // colSumUnpaid
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSumUnpaid.DefaultCellStyle = dataGridViewCellStyle6;
            this.colSumUnpaid.DataPropertyName = "unpaid_amount";
            this.colSumUnpaid.FillWeight = 17F;
            this.colSumUnpaid.HeaderText = "Unpaid";
            this.colSumUnpaid.MinimumWidth = 100;
            this.colSumUnpaid.Name = "colSumUnpaid";
            this.colSumUnpaid.ReadOnly = true;
            this.colSumUnpaid.Width = 130;
            // 
            // colSumPaid
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSumPaid.DefaultCellStyle = dataGridViewCellStyle7;
            this.colSumPaid.DataPropertyName = "paid_amount";
            this.colSumPaid.FillWeight = 17F;
            this.colSumPaid.HeaderText = "Paid";
            this.colSumPaid.MinimumWidth = 100;
            this.colSumPaid.Name = "colSumPaid";
            this.colSumPaid.ReadOnly = true;
            this.colSumPaid.Width = 130;
            // 
            // pnlDetailsGrid
            // 
            this.pnlDetailsGrid.BackColor = System.Drawing.Color.White;
            this.pnlDetailsGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetailsGrid.Controls.Add(this.dgvDetails);
            this.pnlDetailsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailsGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlDetailsGrid.Name = "pnlDetailsGrid";
            this.pnlDetailsGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlDetailsGrid.Size = new System.Drawing.Size(1000, 250);
            this.pnlDetailsGrid.TabIndex = 0;
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDetails.AutoGenerateColumns = false;
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDetails.ColumnHeadersHeight = 34;
            this.dgvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetCommissionId,
            this.colDetDate,
            this.colDetStaff,
            this.colDetClient,
            this.colDetService,
            this.colDetBase,
            this.colDetRate,
            this.colDetAmount,
            this.colDetStatus});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetails.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.EnableHeadersVisualStyles = false;
            this.dgvDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvDetails.Location = new System.Drawing.Point(1, 1);
            this.dgvDetails.MultiSelect = true;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.RowTemplate.Height = 30;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(996, 246);
            this.dgvDetails.TabIndex = 0;
            this.dgvDetails.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDetails_CellFormatting);
            // 
            // colDetCommissionId
            // 
            this.colDetCommissionId.DataPropertyName = "commission_id";
            this.colDetCommissionId.HeaderText = "ID";
            this.colDetCommissionId.Name = "colDetCommissionId";
            this.colDetCommissionId.ReadOnly = true;
            this.colDetCommissionId.Visible = false;
            // 
            // colDetDate
            // 
            this.colDetDate.DataPropertyName = "service_date";
            this.colDetDate.DefaultCellStyle.Format = "MMM dd, yyyy";
            this.colDetDate.FillWeight = 13F;
            this.colDetDate.HeaderText = "Date";
            this.colDetDate.MinimumWidth = 100;
            this.colDetDate.Name = "colDetDate";
            this.colDetDate.ReadOnly = true;
            this.colDetDate.Width = 110;
            // 
            // colDetStaff
            // 
            this.colDetStaff.DataPropertyName = "staff_name";
            this.colDetStaff.FillWeight = 14F;
            this.colDetStaff.HeaderText = "Staff";
            this.colDetStaff.MinimumWidth = 100;
            this.colDetStaff.Name = "colDetStaff";
            this.colDetStaff.ReadOnly = true;
            this.colDetStaff.Width = 130;
            // 
            // colDetClient
            // 
            this.colDetClient.DataPropertyName = "client_name";
            this.colDetClient.FillWeight = 14F;
            this.colDetClient.HeaderText = "Client";
            this.colDetClient.MinimumWidth = 100;
            this.colDetClient.Name = "colDetClient";
            this.colDetClient.ReadOnly = true;
            this.colDetClient.Width = 130;
            // 
            // colDetService
            // 
            this.colDetService.DataPropertyName = "service_name";
            this.colDetService.FillWeight = 20F;
            this.colDetService.HeaderText = "Service";
            this.colDetService.MinimumWidth = 120;
            this.colDetService.Name = "colDetService";
            this.colDetService.ReadOnly = true;
            this.colDetService.Width = 180;
            // 
            // colDetBase
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colDetBase.DefaultCellStyle = dataGridViewCellStyle11;
            this.colDetBase.DataPropertyName = "base_amount";
            this.colDetBase.FillWeight = 12F;
            this.colDetBase.HeaderText = "Service amount";
            this.colDetBase.MinimumWidth = 110;
            this.colDetBase.Name = "colDetBase";
            this.colDetBase.ReadOnly = true;
            this.colDetBase.Width = 120;
            // 
            // colDetRate
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            dataGridViewCellStyle12.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colDetRate.DefaultCellStyle = dataGridViewCellStyle12;
            this.colDetRate.DataPropertyName = "commission_rate";
            this.colDetRate.FillWeight = 8F;
            this.colDetRate.HeaderText = "Rate %";
            this.colDetRate.MinimumWidth = 70;
            this.colDetRate.Name = "colDetRate";
            this.colDetRate.ReadOnly = true;
            this.colDetRate.Width = 75;
            // 
            // colDetAmount
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N2";
            dataGridViewCellStyle13.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colDetAmount.DefaultCellStyle = dataGridViewCellStyle13;
            this.colDetAmount.DataPropertyName = "commission_amount";
            this.colDetAmount.FillWeight = 12F;
            this.colDetAmount.HeaderText = "Commission";
            this.colDetAmount.MinimumWidth = 100;
            this.colDetAmount.Name = "colDetAmount";
            this.colDetAmount.ReadOnly = true;
            this.colDetAmount.Width = 115;
            // 
            // colDetStatus
            // 
            this.colDetStatus.DataPropertyName = "payout_status";
            this.colDetStatus.FillWeight = 10F;
            this.colDetStatus.HeaderText = "Payout";
            this.colDetStatus.MinimumWidth = 80;
            this.colDetStatus.Name = "colDetStatus";
            this.colDetStatus.ReadOnly = true;
            this.colDetStatus.Width = 95;
            // 
            // ucCommissions
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "ucCommissions";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucCommissions_Load);
            this.pnlToolbar.ResumeLayout(false);
            this.tlpToolbar.ResumeLayout(false);
            this.tlpToolbar.PerformLayout();
            this.flpFilters.ResumeLayout(false);
            this.flpFilters.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.pnlSummary.ResumeLayout(false);
            this.tlpSummary.ResumeLayout(false);
            this.pnlTotalEarned.ResumeLayout(false);
            this.pnlUnpaid.ResumeLayout(false);
            this.pnlPaid.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlSummaryGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            this.pnlDetailsGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TableLayoutPanel tlpToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpFilters;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblStaff;
        private System.Windows.Forms.ComboBox cboStaff;
        private System.Windows.Forms.Label lblPayout;
        private System.Windows.Forms.ComboBox cboPayout;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnMarkPaid;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.TableLayoutPanel tlpSummary;
        private System.Windows.Forms.Panel pnlTotalEarned;
        private System.Windows.Forms.Label lblTotalEarned;
        private System.Windows.Forms.Label lblTotalEarnedTitle;
        private System.Windows.Forms.Panel pnlUnpaid;
        private System.Windows.Forms.Label lblUnpaid;
        private System.Windows.Forms.Label lblUnpaidTitle;
        private System.Windows.Forms.Panel pnlPaid;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlSummaryGrid;
        private System.Windows.Forms.DataGridView dgvSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumStaff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumUnpaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumPaid;
        private System.Windows.Forms.Panel pnlDetailsGrid;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetCommissionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetStaff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetStatus;
    }
}
