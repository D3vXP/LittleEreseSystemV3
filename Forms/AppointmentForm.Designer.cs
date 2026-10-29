namespace LittleEreseSystemV3.Forms
{
    partial class AppointmentForm
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

        #region Windows Form Designer generated code

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlScroll = new System.Windows.Forms.Panel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.flpClient = new System.Windows.Forms.FlowLayoutPanel();
            this.lblClient = new System.Windows.Forms.Label();
            this.cboClient = new System.Windows.Forms.ComboBox();
            this.btnNewClient = new System.Windows.Forms.Button();
            this.lblClientInfo = new System.Windows.Forms.Label();
            this.flpSchedule = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblSource = new System.Windows.Forms.Label();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.lblSlotInfo = new System.Windows.Forms.Label();
            this.flpCatalogHeader = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.cboCategoryFilter = new System.Windows.Forms.ComboBox();
            this.pnlCatalog = new System.Windows.Forms.Panel();
            this.dgvCatalog = new System.Windows.Forms.DataGridView();
            this.colCatServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCatName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCatPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCatDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpServiceButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddService = new System.Windows.Forms.Button();
            this.btnRemoveService = new System.Windows.Forms.Button();
            this.pnlSelected = new System.Windows.Forms.Panel();
            this.dgvSelected = new System.Windows.Forms.DataGridView();
            this.colSelServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelTechnician = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelNurse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpLine = new System.Windows.Forms.GroupBox();
            this.lblLineService = new System.Windows.Forms.Label();
            this.lblLineTime = new System.Windows.Forms.Label();
            this.cboLineTime = new System.Windows.Forms.ComboBox();
            this.lblLineTech = new System.Windows.Forms.Label();
            this.cboLineTech = new System.Windows.Forms.ComboBox();
            this.lblLineNurse = new System.Windows.Forms.Label();
            this.cboLineNurse = new System.Windows.Forms.ComboBox();
            this.lblLineNote = new System.Windows.Forms.Label();
            this.lblSessionPlan = new System.Windows.Forms.Label();
            this.lblPromoNote = new System.Windows.Forms.Label();
            this.flpSubtotal = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.flpDiscount = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.flpTotal = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.flpRequired = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlFooterLine = new System.Windows.Forms.Panel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlScroll.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.flpClient.SuspendLayout();
            this.flpSchedule.SuspendLayout();
            this.flpCatalogHeader.SuspendLayout();
            this.pnlCatalog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalog)).BeginInit();
            this.flpServiceButtons.SuspendLayout();
            this.pnlSelected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).BeginInit();
            this.flpSubtotal.SuspendLayout();
            this.flpDiscount.SuspendLayout();
            this.flpTotal.SuspendLayout();
            this.flpRequired.SuspendLayout();
            this.grpLine.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(980, 56);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(500, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "New Appointment";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 55);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(980, 1);
            this.pnlHeaderLine.TabIndex = 1;
            // 
            // pnlScroll
            // 
            this.pnlScroll.AutoScroll = true;
            this.pnlScroll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlScroll.Controls.Add(this.tlpContent);
            this.pnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScroll.Location = new System.Drawing.Point(0, 56);
            this.pnlScroll.Name = "pnlScroll";
            this.pnlScroll.Size = new System.Drawing.Size(980, 520);
            this.pnlScroll.TabIndex = 1;
            // 
            // tlpContent
            // 
            this.tlpContent.AutoSize = true;
            this.tlpContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.tlpContent.ColumnCount = 6;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpContent.Controls.Add(this.flpClient, 0, 0);
            this.tlpContent.SetColumnSpan(this.flpClient, 3);
            this.tlpContent.Controls.Add(this.flpSchedule, 3, 0);
            this.tlpContent.SetColumnSpan(this.flpSchedule, 3);
            this.tlpContent.Controls.Add(this.lblClientInfo, 0, 1);
            this.tlpContent.SetColumnSpan(this.lblClientInfo, 3);
            this.tlpContent.Controls.Add(this.lblSlotInfo, 3, 1);
            this.tlpContent.SetColumnSpan(this.lblSlotInfo, 3);
            this.tlpContent.Controls.Add(this.flpCatalogHeader, 0, 2);
            this.tlpContent.SetColumnSpan(this.flpCatalogHeader, 3);
            this.tlpContent.Controls.Add(this.flpServiceButtons, 3, 2);
            this.tlpContent.SetColumnSpan(this.flpServiceButtons, 3);
            this.tlpContent.Controls.Add(this.pnlCatalog, 0, 3);
            this.tlpContent.SetColumnSpan(this.pnlCatalog, 6);
            this.tlpContent.Controls.Add(this.pnlSelected, 0, 4);
            this.tlpContent.SetColumnSpan(this.pnlSelected, 6);
            this.tlpContent.Controls.Add(this.grpLine, 0, 5);
            this.tlpContent.SetColumnSpan(this.grpLine, 6);
            this.tlpContent.Controls.Add(this.flpSubtotal, 0, 6);
            this.tlpContent.SetColumnSpan(this.flpSubtotal, 2);
            this.tlpContent.Controls.Add(this.flpDiscount, 2, 6);
            this.tlpContent.SetColumnSpan(this.flpDiscount, 2);
            this.tlpContent.Controls.Add(this.flpTotal, 4, 6);
            this.tlpContent.SetColumnSpan(this.flpTotal, 2);
            this.tlpContent.Controls.Add(this.flpRequired, 0, 7);
            this.tlpContent.SetColumnSpan(this.flpRequired, 6);
            this.tlpContent.Controls.Add(this.lblPromoNote, 0, 8);
            this.tlpContent.SetColumnSpan(this.lblPromoNote, 6);
            this.tlpContent.Controls.Add(this.lblNotes, 0, 9);
            this.tlpContent.SetColumnSpan(this.lblNotes, 6);
            this.tlpContent.Controls.Add(this.txtNotes, 0, 10);
            this.tlpContent.SetColumnSpan(this.txtNotes, 6);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(12);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new System.Windows.Forms.Padding(12);
            this.tlpContent.RowCount = 11;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpContent.Size = new System.Drawing.Size(963, 596);
            this.tlpContent.TabIndex = 0;
            // 
            // flpClient
            // 
            this.flpClient.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpClient.AutoSize = true;
            this.flpClient.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpClient.Controls.Add(this.lblClient);
            this.flpClient.Controls.Add(this.cboClient);
            this.flpClient.Controls.Add(this.btnNewClient);
            this.flpClient.Location = new System.Drawing.Point(12, 0);
            this.flpClient.Margin = new System.Windows.Forms.Padding(0);
            this.flpClient.Name = "flpClient";
            this.flpClient.Size = new System.Drawing.Size(438, 30);
            this.flpClient.TabIndex = 0;
            this.flpClient.WrapContents = false;
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblClient.Location = new System.Drawing.Point(0, 8);
            this.lblClient.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(41, 19);
            this.lblClient.TabIndex = 0;
            this.lblClient.Text = "Client";
            this.lblClient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboClient
            // 
            this.cboClient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboClient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboClient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboClient.FormattingEnabled = true;
            this.cboClient.Location = new System.Drawing.Point(49, 3);
            this.cboClient.Margin = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.cboClient.Name = "cboClient";
            this.cboClient.Size = new System.Drawing.Size(270, 25);
            this.cboClient.TabIndex = 1;
            this.cboClient.SelectedIndexChanged += new System.EventHandler(this.cboClient_SelectedIndexChanged);
            // 
            // btnNewClient
            // 
            this.btnNewClient.BackColor = System.Drawing.Color.White;
            this.btnNewClient.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewClient.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNewClient.FlatAppearance.BorderSize = 1;
            this.btnNewClient.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnNewClient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnNewClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewClient.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnNewClient.Location = new System.Drawing.Point(331, 0);
            this.btnNewClient.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewClient.Name = "btnNewClient";
            this.btnNewClient.Size = new System.Drawing.Size(87, 30);
            this.btnNewClient.TabIndex = 2;
            this.btnNewClient.Text = "+ New";
            this.btnNewClient.UseVisualStyleBackColor = false;
            this.btnNewClient.Click += new System.EventHandler(this.btnNewClient_Click);
            // 
            // lblClientInfo
            // 
            this.lblClientInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClientInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblClientInfo.Location = new System.Drawing.Point(12, 42);
            this.lblClientInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lblClientInfo.Name = "lblClientInfo";
            this.lblClientInfo.Size = new System.Drawing.Size(469, 20);
            this.lblClientInfo.TabIndex = 2;
            this.lblClientInfo.Text = "Consent status and medical alerts appear here.";
            this.lblClientInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpSchedule
            // 
            this.flpSchedule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpSchedule.AutoSize = true;
            this.flpSchedule.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpSchedule.Controls.Add(this.lblDate);
            this.flpSchedule.Controls.Add(this.dtpDate);
            this.flpSchedule.Controls.Add(this.lblSource);
            this.flpSchedule.Controls.Add(this.cboSource);
            this.flpSchedule.Location = new System.Drawing.Point(481, 0);
            this.flpSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.flpSchedule.Name = "flpSchedule";
            this.flpSchedule.Size = new System.Drawing.Size(394, 30);
            this.flpSchedule.TabIndex = 1;
            this.flpSchedule.WrapContents = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDate.Location = new System.Drawing.Point(0, 8);
            this.lblDate.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(37, 19);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDate
            // 
            this.dtpDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(45, 3);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(0, 3, 16, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(130, 25);
            this.dtpDate.TabIndex = 1;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSource.Location = new System.Drawing.Point(191, 8);
            this.lblSource.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(49, 19);
            this.lblSource.TabIndex = 2;
            this.lblSource.Text = "Source";
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboSource
            // 
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Items.AddRange(new object[] {
            "Walk-in",
            "Messenger",
            "Text",
            "Phone"});
            this.cboSource.Location = new System.Drawing.Point(248, 3);
            this.cboSource.Margin = new System.Windows.Forms.Padding(0, 3, 16, 0);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(130, 25);
            this.cboSource.TabIndex = 3;
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSource_SelectedIndexChanged);
            // 
            // lblSlotInfo
            // 
            this.lblSlotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSlotInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlotInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(142)))), ((int)(((byte)(4)))));
            this.lblSlotInfo.Location = new System.Drawing.Point(481, 42);
            this.lblSlotInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lblSlotInfo.Name = "lblSlotInfo";
            this.lblSlotInfo.Size = new System.Drawing.Size(470, 20);
            this.lblSlotInfo.TabIndex = 3;
            this.lblSlotInfo.Text = "Availability and opening hours are checked here.";
            // 
            // flpCatalogHeader
            // 
            this.flpCatalogHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpCatalogHeader.AutoSize = true;
            this.flpCatalogHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpCatalogHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpCatalogHeader.Controls.Add(this.lblCategoryFilter);
            this.flpCatalogHeader.Controls.Add(this.cboCategoryFilter);
            this.flpCatalogHeader.Location = new System.Drawing.Point(12, 62);
            this.flpCatalogHeader.Margin = new System.Windows.Forms.Padding(0);
            this.flpCatalogHeader.Name = "flpCatalogHeader";
            this.flpCatalogHeader.Size = new System.Drawing.Size(370, 30);
            this.flpCatalogHeader.TabIndex = 4;
            this.flpCatalogHeader.WrapContents = false;
            // 
            // lblCategoryFilter
            // 
            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblCategoryFilter.Location = new System.Drawing.Point(0, 8);
            this.lblCategoryFilter.Margin = new System.Windows.Forms.Padding(0, 8, 8, 0);
            this.lblCategoryFilter.Name = "lblCategoryFilter";
            this.lblCategoryFilter.Size = new System.Drawing.Size(66, 19);
            this.lblCategoryFilter.TabIndex = 0;
            this.lblCategoryFilter.Text = "Category";
            this.lblCategoryFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCategoryFilter
            // 
            this.cboCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategoryFilter.FormattingEnabled = true;
            this.cboCategoryFilter.Location = new System.Drawing.Point(74, 3);
            this.cboCategoryFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.cboCategoryFilter.Name = "cboCategoryFilter";
            this.cboCategoryFilter.Size = new System.Drawing.Size(296, 25);
            this.cboCategoryFilter.TabIndex = 1;
            this.cboCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.cboCategoryFilter_SelectedIndexChanged);
            // 
            // pnlCatalog
            // 
            this.pnlCatalog.BackColor = System.Drawing.Color.White;
            this.pnlCatalog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCatalog.Controls.Add(this.dgvCatalog);
            this.pnlCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCatalog.Location = new System.Drawing.Point(12, 92);
            this.pnlCatalog.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCatalog.Name = "pnlCatalog";
            this.pnlCatalog.Padding = new System.Windows.Forms.Padding(1);
            this.pnlCatalog.Size = new System.Drawing.Size(939, 112);
            this.pnlCatalog.TabIndex = 6;
            // 
            // dgvCatalog
            // 
            this.dgvCatalog.AllowUserToAddRows = false;
            this.dgvCatalog.AllowUserToDeleteRows = false;
            this.dgvCatalog.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dgvCatalog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCatalog.AutoGenerateColumns = false;
            this.dgvCatalog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCatalog.BackgroundColor = System.Drawing.Color.White;
            this.dgvCatalog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCatalog.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCatalog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCatalog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCatalog.ColumnHeadersHeight = 30;
            this.dgvCatalog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCatServiceId,
            this.colCatName,
            this.colCatPrice,
            this.colCatDuration});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCatalog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCatalog.EnableHeadersVisualStyles = false;
            this.dgvCatalog.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvCatalog.Location = new System.Drawing.Point(1, 1);
            this.dgvCatalog.MultiSelect = false;
            this.dgvCatalog.Name = "dgvCatalog";
            this.dgvCatalog.ReadOnly = true;
            this.dgvCatalog.RowHeadersVisible = false;
            this.dgvCatalog.RowTemplate.Height = 26;
            this.dgvCatalog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCatalog.Size = new System.Drawing.Size(935, 110);
            this.dgvCatalog.TabIndex = 0;
            this.dgvCatalog.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCatalog_CellDoubleClick);
            // 
            // colCatServiceId
            // 
            this.colCatServiceId.DataPropertyName = "service_id";
            this.colCatServiceId.HeaderText = "ID";
            this.colCatServiceId.Name = "colCatServiceId";
            this.colCatServiceId.ReadOnly = true;
            this.colCatServiceId.Visible = false;
            // 
            // colCatName
            // 
            this.colCatName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCatName.DataPropertyName = "service_name";
            this.colCatName.FillWeight = 58F;
            this.colCatName.HeaderText = "Service";
            this.colCatName.MinimumWidth = 200;
            this.colCatName.Name = "colCatName";
            this.colCatName.ReadOnly = true;
            // 
            // colCatPrice
            // 
            this.colCatPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCatPrice.DataPropertyName = "price";
            this.colCatPrice.DefaultCellStyle.Format = "N2";
            this.colCatPrice.FillWeight = 22F;
            this.colCatPrice.HeaderText = "Price";
            this.colCatPrice.MinimumWidth = 100;
            this.colCatPrice.Name = "colCatPrice";
            this.colCatPrice.ReadOnly = true;
            // 
            // colCatDuration
            // 
            this.colCatDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCatDuration.DataPropertyName = "duration_minutes";
            this.colCatDuration.FillWeight = 20F;
            this.colCatDuration.HeaderText = "Min";
            this.colCatDuration.MinimumWidth = 70;
            this.colCatDuration.Name = "colCatDuration";
            this.colCatDuration.ReadOnly = true;
            // 
            // flpServiceButtons
            // 
            this.flpServiceButtons.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flpServiceButtons.AutoSize = true;
            this.flpServiceButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpServiceButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpServiceButtons.Controls.Add(this.btnAddService);
            this.flpServiceButtons.Controls.Add(this.btnRemoveService);
            this.flpServiceButtons.Location = new System.Drawing.Point(755, 62);
            this.flpServiceButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpServiceButtons.Name = "flpServiceButtons";
            this.flpServiceButtons.Size = new System.Drawing.Size(196, 30);
            this.flpServiceButtons.TabIndex = 5;
            this.flpServiceButtons.WrapContents = false;
            // 
            // btnAddService
            // 
            this.btnAddService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnAddService.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddService.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAddService.FlatAppearance.BorderSize = 1;
            this.btnAddService.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAddService.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnAddService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddService.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddService.ForeColor = System.Drawing.Color.White;
            this.btnAddService.Location = new System.Drawing.Point(0, 0);
            this.btnAddService.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAddService.Name = "btnAddService";
            this.btnAddService.Size = new System.Drawing.Size(90, 30);
            this.btnAddService.TabIndex = 0;
            this.btnAddService.Text = "Add >";
            this.btnAddService.UseVisualStyleBackColor = false;
            this.btnAddService.Click += new System.EventHandler(this.btnAddService_Click);
            // 
            // btnRemoveService
            // 
            this.btnRemoveService.BackColor = System.Drawing.Color.White;
            this.btnRemoveService.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveService.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRemoveService.FlatAppearance.BorderSize = 1;
            this.btnRemoveService.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnRemoveService.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnRemoveService.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveService.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveService.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRemoveService.Location = new System.Drawing.Point(98, 0);
            this.btnRemoveService.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemoveService.Name = "btnRemoveService";
            this.btnRemoveService.Size = new System.Drawing.Size(98, 30);
            this.btnRemoveService.TabIndex = 1;
            this.btnRemoveService.Text = "Remove";
            this.btnRemoveService.UseVisualStyleBackColor = false;
            this.btnRemoveService.Click += new System.EventHandler(this.btnRemoveService_Click);
            // 
            // pnlSelected
            // 
            this.pnlSelected.BackColor = System.Drawing.Color.White;
            this.pnlSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelected.Controls.Add(this.dgvSelected);
            this.pnlSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelected.Location = new System.Drawing.Point(12, 204);
            this.pnlSelected.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSelected.Name = "pnlSelected";
            this.pnlSelected.Padding = new System.Windows.Forms.Padding(1);
            this.pnlSelected.Size = new System.Drawing.Size(939, 126);
            this.pnlSelected.TabIndex = 7;
            // 
            // dgvSelected
            // 
            this.dgvSelected.AllowUserToAddRows = false;
            this.dgvSelected.AllowUserToDeleteRows = false;
            this.dgvSelected.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(248)))), ((int)(((byte)(241)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dgvSelected.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSelected.AutoGenerateColumns = false;
            this.dgvSelected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelected.BackgroundColor = System.Drawing.Color.White;
            this.dgvSelected.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSelected.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSelected.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSelected.ColumnHeadersHeight = 30;
            this.dgvSelected.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelServiceId,
            this.colSelName,
            this.colSelQty,
            this.colSelPrice,
            this.colSelDuration,
            this.colSelStart,
            this.colSelEnd,
            this.colSelTechnician,
            this.colSelNurse,
            this.colSelPackage});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSelected.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelected.EnableHeadersVisualStyles = false;
            this.dgvSelected.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvSelected.Location = new System.Drawing.Point(1, 1);
            this.dgvSelected.MultiSelect = false;
            this.dgvSelected.Name = "dgvSelected";
            this.dgvSelected.RowHeadersVisible = false;
            this.dgvSelected.RowTemplate.Height = 28;
            this.dgvSelected.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelected.Size = new System.Drawing.Size(935, 124);
            this.dgvSelected.TabIndex = 0;
            this.dgvSelected.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelected_CellValueChanged);
            this.dgvSelected.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSelected_CurrentCellDirtyStateChanged);
            // 
            // colSelServiceId
            // 
            this.colSelServiceId.DataPropertyName = "service_id";
            this.colSelServiceId.HeaderText = "ID";
            this.colSelServiceId.Name = "colSelServiceId";
            this.colSelServiceId.ReadOnly = true;
            this.colSelServiceId.Visible = false;
            // 
            // colSelName
            // 
            this.colSelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelName.DataPropertyName = "service_name";
            this.colSelName.FillWeight = 30F;
            this.colSelName.HeaderText = "Service";
            this.colSelName.MinimumWidth = 150;
            this.colSelName.Name = "colSelName";
            this.colSelName.ReadOnly = true;
            // 
            // colSelQty
            // 
            this.colSelQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSelQty.DefaultCellStyle = dataGridViewCellStyle7;
            this.colSelQty.DataPropertyName = "quantity";
            this.colSelQty.FillWeight = 8F;
            this.colSelQty.HeaderText = "Qty";
            this.colSelQty.MinimumWidth = 48;
            this.colSelQty.Name = "colSelQty";
            // 
            // colSelPrice
            // 
            this.colSelPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelPrice.DataPropertyName = "price";
            this.colSelPrice.DefaultCellStyle.Format = "N2";
            this.colSelPrice.FillWeight = 14F;
            this.colSelPrice.HeaderText = "Price";
            this.colSelPrice.MinimumWidth = 90;
            this.colSelPrice.Name = "colSelPrice";
            this.colSelPrice.ReadOnly = true;
            // 
            // colSelDuration
            // 
            this.colSelDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colSelDuration.DefaultCellStyle = dataGridViewCellStyle8;
            this.colSelDuration.DataPropertyName = "duration_minutes";
            this.colSelDuration.FillWeight = 8F;
            this.colSelDuration.HeaderText = "Min";
            this.colSelDuration.MinimumWidth = 52;
            this.colSelDuration.Name = "colSelDuration";
            this.colSelDuration.ReadOnly = true;
            // 
            // colSelPackage
            // 
            this.colSelStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelStart.DataPropertyName = "start_text";
            this.colSelStart.FillWeight = 10F;
            this.colSelStart.HeaderText = "Start";
            this.colSelStart.MinimumWidth = 78;
            this.colSelStart.Name = "colSelStart";
            this.colSelStart.ReadOnly = true;
            // 
            // colSelEnd
            // 
            this.colSelEnd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelEnd.DataPropertyName = "end_text";
            this.colSelEnd.FillWeight = 10F;
            this.colSelEnd.HeaderText = "End";
            this.colSelEnd.MinimumWidth = 78;
            this.colSelEnd.Name = "colSelEnd";
            this.colSelEnd.ReadOnly = true;
            // 
            // colSelTechnician
            // 
            this.colSelTechnician.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelTechnician.DataPropertyName = "technician_name";
            this.colSelTechnician.FillWeight = 14F;
            this.colSelTechnician.HeaderText = "Technician";
            this.colSelTechnician.MinimumWidth = 110;
            this.colSelTechnician.Name = "colSelTechnician";
            this.colSelTechnician.ReadOnly = true;
            // 
            // colSelNurse
            // 
            this.colSelNurse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelNurse.DataPropertyName = "nurse_name";
            this.colSelNurse.FillWeight = 14F;
            this.colSelNurse.HeaderText = "Nurse";
            this.colSelNurse.MinimumWidth = 110;
            this.colSelNurse.Name = "colSelNurse";
            this.colSelNurse.ReadOnly = true;
            // 
            // colSelPackage
            // 
            this.colSelPackage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelPackage.DataPropertyName = "package_label";
            this.colSelPackage.FillWeight = 10F;
            this.colSelPackage.HeaderText = "Session";
            this.colSelPackage.MinimumWidth = 80;
            this.colSelPackage.Name = "colSelPackage";
            this.colSelPackage.ReadOnly = true;
            // 
            // grpLine
            // 
            this.grpLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.grpLine.Controls.Add(this.lblLineService);
            this.grpLine.Controls.Add(this.lblLineTime);
            this.grpLine.Controls.Add(this.cboLineTime);
            this.grpLine.Controls.Add(this.lblLineTech);
            this.grpLine.Controls.Add(this.cboLineTech);
            this.grpLine.Controls.Add(this.lblLineNurse);
            this.grpLine.Controls.Add(this.cboLineNurse);
            this.grpLine.Controls.Add(this.lblLineNote);
            this.grpLine.Controls.Add(this.lblSessionPlan);
            this.grpLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLine.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.grpLine.Location = new System.Drawing.Point(12, 330);
            this.grpLine.Margin = new System.Windows.Forms.Padding(0);
            this.grpLine.Name = "grpLine";
            this.grpLine.Size = new System.Drawing.Size(939, 126);
            this.grpLine.TabIndex = 8;
            this.grpLine.TabStop = false;
            this.grpLine.Text = "Selected service";
            // 
            // lblLineService
            // 
            this.lblLineService.AutoSize = true;
            this.lblLineService.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineService.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblLineService.Location = new System.Drawing.Point(12, 20);
            this.lblLineService.Name = "lblLineService";
            this.lblLineService.Size = new System.Drawing.Size(171, 19);
            this.lblLineService.TabIndex = 0;
            this.lblLineService.Text = "No service selected";
            // 
            // lblLineTime
            // 
            this.lblLineTime.AutoSize = true;
            this.lblLineTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLineTime.Location = new System.Drawing.Point(12, 40);
            this.lblLineTime.Name = "lblLineTime";
            this.lblLineTime.Size = new System.Drawing.Size(62, 15);
            this.lblLineTime.TabIndex = 1;
            this.lblLineTime.Text = "Start time";
            // 
            // cboLineTime
            // 
            this.cboLineTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLineTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLineTime.FormattingEnabled = true;
            this.cboLineTime.Location = new System.Drawing.Point(12, 57);
            this.cboLineTime.Name = "cboLineTime";
            this.cboLineTime.Size = new System.Drawing.Size(150, 25);
            this.cboLineTime.TabIndex = 2;
            // 
            // lblLineTech
            // 
            this.lblLineTech.AutoSize = true;
            this.lblLineTech.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineTech.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLineTech.Location = new System.Drawing.Point(190, 40);
            this.lblLineTech.Name = "lblLineTech";
            this.lblLineTech.Size = new System.Drawing.Size(60, 15);
            this.lblLineTech.TabIndex = 3;
            this.lblLineTech.Text = "Technician";
            // 
            // cboLineTech
            // 
            this.cboLineTech.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLineTech.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLineTech.FormattingEnabled = true;
            this.cboLineTech.Location = new System.Drawing.Point(190, 57);
            this.cboLineTech.Name = "cboLineTech";
            this.cboLineTech.Size = new System.Drawing.Size(260, 25);
            this.cboLineTech.TabIndex = 4;
            // 
            // lblLineNurse
            // 
            this.lblLineNurse.AutoSize = true;
            this.lblLineNurse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineNurse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLineNurse.Location = new System.Drawing.Point(480, 40);
            this.lblLineNurse.Name = "lblLineNurse";
            this.lblLineNurse.Size = new System.Drawing.Size(111, 15);
            this.lblLineNurse.TabIndex = 5;
            this.lblLineNurse.Text = "Assessed by (nurse)";
            // 
            // cboLineNurse
            // 
            this.cboLineNurse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLineNurse.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboLineNurse.FormattingEnabled = true;
            this.cboLineNurse.Location = new System.Drawing.Point(480, 57);
            this.cboLineNurse.Name = "cboLineNurse";
            this.cboLineNurse.Size = new System.Drawing.Size(260, 25);
            this.cboLineNurse.TabIndex = 6;
            // 
            // lblLineNote
            // 
            this.lblLineNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLineNote.Location = new System.Drawing.Point(12, 88);
            this.lblLineNote.Name = "lblLineNote";
            this.lblLineNote.Size = new System.Drawing.Size(915, 16);
            this.lblLineNote.TabIndex = 7;
            this.lblLineNote.Text = "Select a service above to set its own start time, its technician and, when the " +
    "service needs an assessment, the nurse who assessed the client.";
            // 
            // lblSessionPlan
            // 
            this.lblSessionPlan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSessionPlan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblSessionPlan.Location = new System.Drawing.Point(12, 106);
            this.lblSessionPlan.Name = "lblSessionPlan";
            this.lblSessionPlan.Size = new System.Drawing.Size(915, 16);
            this.lblSessionPlan.TabIndex = 8;
            // 
            // lblPromoNote
            // 
            this.lblPromoNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPromoNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromoNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPromoNote.Location = new System.Drawing.Point(12, 508);
            this.lblPromoNote.Margin = new System.Windows.Forms.Padding(0);
            this.lblPromoNote.Name = "lblPromoNote";
            this.lblPromoNote.Size = new System.Drawing.Size(939, 20);
            this.lblPromoNote.TabIndex = 13;
            this.lblPromoNote.Text = "The promotion note shows which services it applies to.";
            this.lblPromoNote.Visible = false;
            // 
            // flpSubtotal
            // 
            this.flpSubtotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpSubtotal.AutoSize = true;
            this.flpSubtotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSubtotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpSubtotal.Controls.Add(this.lblSubtotal);
            this.flpSubtotal.Location = new System.Drawing.Point(12, 457);
            this.flpSubtotal.Margin = new System.Windows.Forms.Padding(0);
            this.flpSubtotal.Name = "flpSubtotal";
            this.flpSubtotal.Size = new System.Drawing.Size(121, 24);
            this.flpSubtotal.TabIndex = 9;
            this.flpSubtotal.WrapContents = false;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSubtotal.Location = new System.Drawing.Point(0, 2);
            this.lblSubtotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(121, 19);
            this.lblSubtotal.TabIndex = 0;
            this.lblSubtotal.Text = "Subtotal: -";
            this.lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpDiscount
            // 
            this.flpDiscount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpDiscount.AutoSize = true;
            this.flpDiscount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpDiscount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpDiscount.Controls.Add(this.lblDiscount);
            this.flpDiscount.Location = new System.Drawing.Point(325, 457);
            this.flpDiscount.Margin = new System.Windows.Forms.Padding(0);
            this.flpDiscount.Name = "flpDiscount";
            this.flpDiscount.Size = new System.Drawing.Size(124, 24);
            this.flpDiscount.TabIndex = 10;
            this.flpDiscount.WrapContents = false;
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDiscount.Location = new System.Drawing.Point(0, 2);
            this.lblDiscount.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(124, 19);
            this.lblDiscount.TabIndex = 0;
            this.lblDiscount.Text = "Discount: -";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpTotal
            // 
            this.flpTotal.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpTotal.AutoSize = true;
            this.flpTotal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpTotal.Controls.Add(this.lblTotal);
            this.flpTotal.Location = new System.Drawing.Point(638, 456);
            this.flpTotal.Margin = new System.Windows.Forms.Padding(0);
            this.flpTotal.Name = "flpTotal";
            this.flpTotal.Size = new System.Drawing.Size(94, 26);
            this.flpTotal.TabIndex = 11;
            this.flpTotal.WrapContents = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTotal.Location = new System.Drawing.Point(0, 2);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(84, 19);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Total: -";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpRequired
            // 
            this.flpRequired.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpRequired.AutoSize = true;
            this.flpRequired.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpRequired.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.flpRequired.Controls.Add(this.lblRequired);
            this.flpRequired.Location = new System.Drawing.Point(12, 482);
            this.flpRequired.Margin = new System.Windows.Forms.Padding(0);
            this.flpRequired.Name = "flpRequired";
            this.flpRequired.Size = new System.Drawing.Size(170, 26);
            this.flpRequired.TabIndex = 12;
            this.flpRequired.WrapContents = false;
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequired.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(142)))), ((int)(((byte)(90)))));
            this.lblRequired.Location = new System.Drawing.Point(0, 3);
            this.lblRequired.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(170, 19);
            this.lblRequired.TabIndex = 0;
            this.lblRequired.Text = "Down payment due after saving";
            this.lblRequired.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNotes
            // 
            this.lblNotes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblNotes.Location = new System.Drawing.Point(12, 528);
            this.lblNotes.Margin = new System.Windows.Forms.Padding(0);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(48, 19);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(12, 548);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(0);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(939, 36);
            this.txtNotes.TabIndex = 15;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.pnlFooterLine);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 576);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(980, 64);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnCancel.FlatAppearance.BorderSize = 1;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnCancel.Location = new System.Drawing.Point(728, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSave.FlatAppearance.BorderSize = 1;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(832, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 34);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Book Appointment";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlFooterLine
            // 
            this.pnlFooterLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.pnlFooterLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFooterLine.Location = new System.Drawing.Point(0, 0);
            this.pnlFooterLine.Name = "pnlFooterLine";
            this.pnlFooterLine.Size = new System.Drawing.Size(980, 1);
            this.pnlFooterLine.TabIndex = 2;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AppointmentForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(980, 640);
            this.Controls.Add(this.pnlScroll);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppointmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Appointment";
            this.Load += new System.EventHandler(this.AppointmentForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlScroll.ResumeLayout(false);
            this.pnlScroll.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.flpClient.ResumeLayout(false);
            this.flpClient.PerformLayout();
            this.flpSchedule.ResumeLayout(false);
            this.flpSchedule.PerformLayout();
            this.flpCatalogHeader.ResumeLayout(false);
            this.flpCatalogHeader.PerformLayout();
            this.pnlCatalog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatalog)).EndInit();
            this.flpServiceButtons.ResumeLayout(false);
            this.pnlSelected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelected)).EndInit();
            this.grpLine.ResumeLayout(false);
            this.grpLine.PerformLayout();
            this.flpSubtotal.ResumeLayout(false);
            this.flpSubtotal.PerformLayout();
            this.flpDiscount.ResumeLayout(false);
            this.flpDiscount.PerformLayout();
            this.flpTotal.ResumeLayout(false);
            this.flpTotal.PerformLayout();
            this.flpRequired.ResumeLayout(false);
            this.flpRequired.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlScroll;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.FlowLayoutPanel flpClient;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cboClient;
        private System.Windows.Forms.Button btnNewClient;
        private System.Windows.Forms.Label lblClientInfo;
        private System.Windows.Forms.FlowLayoutPanel flpSchedule;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label lblSlotInfo;
        private System.Windows.Forms.FlowLayoutPanel flpCatalogHeader;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cboCategoryFilter;
        private System.Windows.Forms.Panel pnlCatalog;
        private System.Windows.Forms.DataGridView dgvCatalog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCatServiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCatName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCatPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCatDuration;
        private System.Windows.Forms.FlowLayoutPanel flpServiceButtons;
        private System.Windows.Forms.Button btnAddService;
        private System.Windows.Forms.Button btnRemoveService;
        private System.Windows.Forms.Panel pnlSelected;
        private System.Windows.Forms.DataGridView dgvSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelServiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelTechnician;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelNurse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelPackage;
        private System.Windows.Forms.GroupBox grpLine;
        private System.Windows.Forms.Label lblLineService;
        private System.Windows.Forms.Label lblLineTime;
        private System.Windows.Forms.ComboBox cboLineTime;
        private System.Windows.Forms.Label lblLineTech;
        private System.Windows.Forms.ComboBox cboLineTech;
        private System.Windows.Forms.Label lblLineNurse;
        private System.Windows.Forms.ComboBox cboLineNurse;
        private System.Windows.Forms.Label lblLineNote;
        private System.Windows.Forms.Label lblSessionPlan;
        private System.Windows.Forms.Label lblPromoNote;
        private System.Windows.Forms.FlowLayoutPanel flpSubtotal;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.FlowLayoutPanel flpDiscount;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.FlowLayoutPanel flpTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.FlowLayoutPanel flpRequired;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlFooterLine;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
