namespace LittleEreseSystemV3.Forms
{
    partial class PromotionEditForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLeft = new System.Windows.Forms.TableLayoutPanel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblConditions = new System.Windows.Forms.Label();
            this.txtConditions = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.tlpRight = new System.Windows.Forms.TableLayoutPanel();
            this.grpServices = new System.Windows.Forms.GroupBox();
            this.tlpServices = new System.Windows.Forms.TableLayoutPanel();
            this.chkApplyAll = new System.Windows.Forms.CheckBox();
            this.lblServiceSearch = new System.Windows.Forms.Label();
            this.txtServiceSearch = new System.Windows.Forms.TextBox();
            this.lblServiceCategory = new System.Windows.Forms.Label();
            this.cboServiceCategory = new System.Windows.Forms.ComboBox();
            this.dgvServicePick = new System.Windows.Forms.DataGridView();
            this.colPickCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPickServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPickService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPickCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPickPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSelectedCount = new System.Windows.Forms.Label();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.tlpPreview = new System.Windows.Forms.TableLayoutPanel();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.colPrevService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrevRegular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrevPromo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrevSaving = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRegularTitle = new System.Windows.Forms.Label();
            this.lblRegularTotal = new System.Windows.Forms.Label();
            this.lblDropTitle = new System.Windows.Forms.Label();
            this.lblDropTotal = new System.Windows.Forms.Label();
            this.lblPromoTitle = new System.Windows.Forms.Label();
            this.lblPromoTotal = new System.Windows.Forms.Label();
            this.lblPreviewNote = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlFooterLine = new System.Windows.Forms.Panel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tlpLeft.SuspendLayout();
            this.tlpRight.SuspendLayout();
            this.grpServices.SuspendLayout();
            this.tlpServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServicePick)).BeginInit();
            this.grpPreview.SuspendLayout();
            this.tlpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
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
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Promotion";
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
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlContent.Controls.Add(this.tlpMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 56);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(16);
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Size = new System.Drawing.Size(980, 520);
            this.pnlContent.TabIndex = 1;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.tlpMain.Controls.Add(this.tlpLeft, 0, 0);
            this.tlpMain.Controls.Add(this.tlpRight, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(16, 16);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(948, 488);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpLeft
            // 
            this.tlpLeft.ColumnCount = 1;
            this.tlpLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Controls.Add(this.lblName, 0, 0);
            this.tlpLeft.Controls.Add(this.txtName, 0, 1);
            this.tlpLeft.Controls.Add(this.lblConditions, 0, 2);
            this.tlpLeft.Controls.Add(this.txtConditions, 0, 3);
            this.tlpLeft.Controls.Add(this.lblType, 0, 4);
            this.tlpLeft.Controls.Add(this.cboType, 0, 5);
            this.tlpLeft.Controls.Add(this.lblValue, 0, 6);
            this.tlpLeft.Controls.Add(this.txtValue, 0, 7);
            this.tlpLeft.Controls.Add(this.lblHint, 0, 8);
            this.tlpLeft.Controls.Add(this.lblStart, 0, 9);
            this.tlpLeft.Controls.Add(this.dtpStart, 0, 10);
            this.tlpLeft.Controls.Add(this.lblEnd, 0, 11);
            this.tlpLeft.Controls.Add(this.dtpEnd, 0, 12);
            this.tlpLeft.Controls.Add(this.lblStatus, 0, 13);
            this.tlpLeft.Controls.Add(this.cboStatus, 0, 14);
            this.tlpLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLeft.Location = new System.Drawing.Point(0, 0);
            this.tlpLeft.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLeft.Name = "tlpLeft";
            this.tlpLeft.RowCount = 16;
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeft.Size = new System.Drawing.Size(398, 488);
            this.tlpLeft.TabIndex = 0;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblName.Location = new System.Drawing.Point(0, 10);
            this.lblName.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(108, 19);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Promotion name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(0, 31);
            this.txtName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(398, 25);
            this.txtName.TabIndex = 1;
            // 
            // lblConditions
            // 
            this.lblConditions.AutoSize = true;
            this.lblConditions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConditions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblConditions.Location = new System.Drawing.Point(0, 68);
            this.lblConditions.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblConditions.Name = "lblConditions";
            this.lblConditions.Size = new System.Drawing.Size(76, 19);
            this.lblConditions.TabIndex = 2;
            this.lblConditions.Text = "Conditions";
            this.lblConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConditions
            // 
            this.txtConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConditions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConditions.Location = new System.Drawing.Point(0, 89);
            this.txtConditions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtConditions.Multiline = true;
            this.txtConditions.Name = "txtConditions";
            this.txtConditions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConditions.Size = new System.Drawing.Size(398, 70);
            this.txtConditions.TabIndex = 3;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblType.Location = new System.Drawing.Point(0, 171);
            this.lblType.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(91, 19);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Discount type";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboType
            // 
            this.cboType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "Percentage",
            "Fixed"});
            this.cboType.Location = new System.Drawing.Point(0, 192);
            this.cboType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(398, 25);
            this.cboType.TabIndex = 5;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblValue.Location = new System.Drawing.Point(0, 229);
            this.lblValue.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(41, 19);
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "Value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtValue
            // 
            this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValue.Location = new System.Drawing.Point(0, 250);
            this.txtValue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(398, 25);
            this.txtValue.TabIndex = 7;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // lblHint
            // 
            this.lblHint.AutoSize = true;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblHint.Location = new System.Drawing.Point(0, 287);
            this.lblHint.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(173, 15);
            this.lblHint.TabIndex = 8;
            this.lblHint.Text = "Promos need full payment upfront.";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblStart.Location = new System.Drawing.Point(0, 314);
            this.lblStart.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(64, 19);
            this.lblStart.TabIndex = 9;
            this.lblStart.Text = "Start date";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStart
            // 
            this.dtpStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(0, 335);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(398, 25);
            this.dtpStart.TabIndex = 10;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblEnd.Location = new System.Drawing.Point(0, 372);
            this.lblEnd.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(59, 19);
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "End date";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpEnd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(0, 393);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(398, 25);
            this.dtpEnd.TabIndex = 12;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblStatus.Location = new System.Drawing.Point(0, 430);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 19);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboStatus
            // 
            this.cboStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Active",
            "Inactive"});
            this.cboStatus.Location = new System.Drawing.Point(0, 451);
            this.cboStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(398, 25);
            this.cboStatus.TabIndex = 14;
            // 
            // tlpRight
            // 
            this.tlpRight.ColumnCount = 1;
            this.tlpRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRight.Controls.Add(this.grpServices, 0, 0);
            this.tlpRight.Controls.Add(this.grpPreview, 0, 1);
            this.tlpRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRight.Location = new System.Drawing.Point(398, 0);
            this.tlpRight.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRight.Name = "tlpRight";
            this.tlpRight.RowCount = 2;
            this.tlpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56F));
            this.tlpRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tlpRight.Size = new System.Drawing.Size(550, 488);
            this.tlpRight.TabIndex = 1;
            // 
            // grpServices
            // 
            this.grpServices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.grpServices.Controls.Add(this.tlpServices);
            this.grpServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpServices.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpServices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.grpServices.Location = new System.Drawing.Point(16, 0);
            this.grpServices.Margin = new System.Windows.Forms.Padding(16, 0, 0, 8);
            this.grpServices.Name = "grpServices";
            this.grpServices.Padding = new System.Windows.Forms.Padding(10, 6, 10, 8);
            this.grpServices.Size = new System.Drawing.Size(534, 265);
            this.grpServices.TabIndex = 0;
            this.grpServices.TabStop = false;
            this.grpServices.Text = "Services in this promotion";
            // 
            // tlpServices
            // 
            this.tlpServices.ColumnCount = 2;
            this.tlpServices.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpServices.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServices.Controls.Add(this.chkApplyAll, 0, 0);
            this.tlpServices.Controls.Add(this.lblServiceSearch, 0, 1);
            this.tlpServices.Controls.Add(this.txtServiceSearch, 1, 1);
            this.tlpServices.Controls.Add(this.lblServiceCategory, 0, 2);
            this.tlpServices.Controls.Add(this.cboServiceCategory, 1, 2);
            this.tlpServices.Controls.Add(this.dgvServicePick, 0, 3);
            this.tlpServices.Controls.Add(this.lblSelectedCount, 0, 4);
            this.tlpServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpServices.Location = new System.Drawing.Point(10, 24);
            this.tlpServices.Margin = new System.Windows.Forms.Padding(0);
            this.tlpServices.Name = "tlpServices";
            this.tlpServices.RowCount = 5;
            this.tlpServices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpServices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpServices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpServices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServices.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpServices.SetColumnSpan(this.chkApplyAll, 2);
            this.tlpServices.SetColumnSpan(this.dgvServicePick, 2);
            this.tlpServices.SetColumnSpan(this.lblSelectedCount, 2);
            this.tlpServices.Size = new System.Drawing.Size(514, 233);
            this.tlpServices.TabIndex = 0;
            // 
            // chkApplyAll
            // 
            this.chkApplyAll.AutoSize = true;
            this.chkApplyAll.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkApplyAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.chkApplyAll.Location = new System.Drawing.Point(0, 4);
            this.chkApplyAll.Margin = new System.Windows.Forms.Padding(0, 4, 0, 6);
            this.chkApplyAll.Name = "chkApplyAll";
            this.chkApplyAll.Size = new System.Drawing.Size(290, 23);
            this.chkApplyAll.TabIndex = 15;
            this.chkApplyAll.Text = "Apply to all services (no specific services)";
            this.chkApplyAll.UseVisualStyleBackColor = true;
            this.chkApplyAll.CheckedChanged += new System.EventHandler(this.chkApplyAll_CheckedChanged);
            // 
            // lblServiceSearch
            // 
            this.lblServiceSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblServiceSearch.AutoSize = true;
            this.lblServiceSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblServiceSearch.Location = new System.Drawing.Point(0, 36);
            this.lblServiceSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 2);
            this.lblServiceSearch.Name = "lblServiceSearch";
            this.lblServiceSearch.Size = new System.Drawing.Size(53, 19);
            this.lblServiceSearch.TabIndex = 16;
            this.lblServiceSearch.Text = "Search";
            this.lblServiceSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtServiceSearch
            // 
            this.txtServiceSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtServiceSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceSearch.Location = new System.Drawing.Point(63, 33);
            this.txtServiceSearch.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.txtServiceSearch.Name = "txtServiceSearch";
            this.txtServiceSearch.Size = new System.Drawing.Size(451, 25);
            this.txtServiceSearch.TabIndex = 17;
            this.txtServiceSearch.TextChanged += new System.EventHandler(this.txtServiceSearch_TextChanged);
            // 
            // lblServiceCategory
            // 
            this.lblServiceCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblServiceCategory.AutoSize = true;
            this.lblServiceCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblServiceCategory.Location = new System.Drawing.Point(0, 70);
            this.lblServiceCategory.Margin = new System.Windows.Forms.Padding(0, 0, 10, 2);
            this.lblServiceCategory.Name = "lblServiceCategory";
            this.lblServiceCategory.Size = new System.Drawing.Size(69, 19);
            this.lblServiceCategory.TabIndex = 18;
            this.lblServiceCategory.Text = "Category";
            this.lblServiceCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboServiceCategory
            // 
            this.cboServiceCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboServiceCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServiceCategory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboServiceCategory.FormattingEnabled = true;
            this.cboServiceCategory.Location = new System.Drawing.Point(63, 67);
            this.cboServiceCategory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.cboServiceCategory.Name = "cboServiceCategory";
            this.cboServiceCategory.Size = new System.Drawing.Size(451, 25);
            this.cboServiceCategory.TabIndex = 19;
            this.cboServiceCategory.SelectedIndexChanged += new System.EventHandler(this.cboServiceCategory_SelectedIndexChanged);
            // 
            // dgvServicePick
            // 
            this.dgvServicePick.AllowUserToAddRows = false;
            this.dgvServicePick.AllowUserToDeleteRows = false;
            this.dgvServicePick.AllowUserToResizeRows = false;
            this.dgvServicePick.AutoGenerateColumns = false;
            this.dgvServicePick.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvServicePick.BackgroundColor = System.Drawing.Color.White;
            this.dgvServicePick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvServicePick.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvServicePick.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServicePick.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvServicePick.ColumnHeadersHeight = 30;
            this.dgvServicePick.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPickCheck,
            this.colPickServiceId,
            this.colPickService,
            this.colPickCategory,
            this.colPickPrice});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServicePick.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvServicePick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServicePick.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvServicePick.EnableHeadersVisualStyles = false;
            this.dgvServicePick.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvServicePick.Location = new System.Drawing.Point(0, 100);
            this.dgvServicePick.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.dgvServicePick.MultiSelect = false;
            this.dgvServicePick.Name = "dgvServicePick";
            this.dgvServicePick.RowHeadersVisible = false;
            this.dgvServicePick.RowTemplate.Height = 28;
            this.dgvServicePick.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServicePick.Size = new System.Drawing.Size(514, 111);
            this.dgvServicePick.TabIndex = 20;
            this.dgvServicePick.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServicePick_CellValueChanged);
            this.dgvServicePick.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvServicePick_CurrentCellDirtyStateChanged);
            // 
            // colPickCheck
            // 
            this.colPickCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPickCheck.DataPropertyName = "is_selected";
            this.colPickCheck.HeaderText = "";
            this.colPickCheck.Name = "colPickCheck";
            this.colPickCheck.Width = 40;
            // 
            // colPickServiceId
            // 
            this.colPickServiceId.DataPropertyName = "service_id";
            this.colPickServiceId.HeaderText = "ID";
            this.colPickServiceId.Name = "colPickServiceId";
            this.colPickServiceId.ReadOnly = true;
            this.colPickServiceId.Visible = false;
            // 
            // colPickService
            // 
            this.colPickService.DataPropertyName = "service_name";
            this.colPickService.HeaderText = "Service";
            this.colPickService.Name = "colPickService";
            this.colPickService.ReadOnly = true;
            // 
            // colPickCategory
            // 
            this.colPickCategory.DataPropertyName = "category_name";
            this.colPickCategory.HeaderText = "Category";
            this.colPickCategory.Name = "colPickCategory";
            this.colPickCategory.ReadOnly = true;
            // 
            // colPickPrice
            // 
            this.colPickPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPickPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPickPrice.DataPropertyName = "price";
            this.colPickPrice.HeaderText = "Price";
            this.colPickPrice.Name = "colPickPrice";
            this.colPickPrice.ReadOnly = true;
            this.colPickPrice.Width = 90;
            // 
            // lblSelectedCount
            // 
            this.lblSelectedCount.AutoSize = true;
            this.lblSelectedCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblSelectedCount.Location = new System.Drawing.Point(0, 215);
            this.lblSelectedCount.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Size = new System.Drawing.Size(110, 15);
            this.lblSelectedCount.TabIndex = 21;
            this.lblSelectedCount.Text = "0 services selected";
            this.lblSelectedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpPreview
            // 
            this.grpPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.grpPreview.Controls.Add(this.tlpPreview);
            this.grpPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPreview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.grpPreview.Location = new System.Drawing.Point(16, 273);
            this.grpPreview.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Padding = new System.Windows.Forms.Padding(10, 6, 10, 8);
            this.grpPreview.Size = new System.Drawing.Size(534, 215);
            this.grpPreview.TabIndex = 1;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Price preview";
            // 
            // tlpPreview
            // 
            this.tlpPreview.ColumnCount = 2;
            this.tlpPreview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPreview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPreview.Controls.Add(this.dgvPreview, 0, 0);
            this.tlpPreview.Controls.Add(this.lblRegularTitle, 0, 1);
            this.tlpPreview.Controls.Add(this.lblRegularTotal, 1, 1);
            this.tlpPreview.Controls.Add(this.lblDropTitle, 0, 2);
            this.tlpPreview.Controls.Add(this.lblDropTotal, 1, 2);
            this.tlpPreview.Controls.Add(this.lblPromoTitle, 0, 3);
            this.tlpPreview.Controls.Add(this.lblPromoTotal, 1, 3);
            this.tlpPreview.Controls.Add(this.lblPreviewNote, 0, 4);
            this.tlpPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPreview.Location = new System.Drawing.Point(10, 24);
            this.tlpPreview.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPreview.Name = "tlpPreview";
            this.tlpPreview.RowCount = 5;
            this.tlpPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpPreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpPreview.SetColumnSpan(this.dgvPreview, 2);
            this.tlpPreview.SetColumnSpan(this.lblPreviewNote, 2);
            this.tlpPreview.Size = new System.Drawing.Size(514, 183);
            this.tlpPreview.TabIndex = 0;
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AllowUserToResizeRows = false;
            this.dgvPreview.AutoGenerateColumns = false;
            this.dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreview.BackgroundColor = System.Drawing.Color.White;
            this.dgvPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvPreview.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPreview.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreview.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPreview.ColumnHeadersHeight = 30;
            this.dgvPreview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPrevService,
            this.colPrevRegular,
            this.colPrevPromo,
            this.colPrevSaving});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPreview.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.EnableHeadersVisualStyles = false;
            this.dgvPreview.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvPreview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.dgvPreview.MultiSelect = false;
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.RowTemplate.Height = 26;
            this.dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreview.Size = new System.Drawing.Size(514, 72);
            this.dgvPreview.TabIndex = 22;
            // 
            // colPrevService
            // 
            this.colPrevService.DataPropertyName = "service_name";
            this.colPrevService.HeaderText = "Service";
            this.colPrevService.Name = "colPrevService";
            this.colPrevService.ReadOnly = true;
            // 
            // colPrevRegular
            // 
            this.colPrevRegular.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPrevRegular.DataPropertyName = "regular_price";
            this.colPrevRegular.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPrevRegular.DefaultCellStyle.Format = "N2";
            this.colPrevRegular.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPrevRegular.HeaderText = "Regular price";
            this.colPrevRegular.Name = "colPrevRegular";
            this.colPrevRegular.ReadOnly = true;
            this.colPrevRegular.Width = 112;
            // 
            // colPrevPromo
            // 
            this.colPrevPromo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPrevPromo.DataPropertyName = "promo_price";
            this.colPrevPromo.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPrevPromo.DefaultCellStyle.Format = "N2";
            this.colPrevPromo.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPrevPromo.HeaderText = "Promo price";
            this.colPrevPromo.Name = "colPrevPromo";
            this.colPrevPromo.ReadOnly = true;
            this.colPrevPromo.Width = 104;
            // 
            // colPrevSaving
            // 
            this.colPrevSaving.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPrevSaving.DataPropertyName = "saving";
            this.colPrevSaving.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPrevSaving.DefaultCellStyle.Format = "N2";
            this.colPrevSaving.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPrevSaving.HeaderText = "You save";
            this.colPrevSaving.Name = "colPrevSaving";
            this.colPrevSaving.ReadOnly = true;
            this.colPrevSaving.Width = 88;
            // 
            // lblRegularTitle
            // 
            this.lblRegularTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRegularTitle.AutoSize = true;
            this.lblRegularTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegularTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblRegularTitle.Location = new System.Drawing.Point(0, 80);
            this.lblRegularTitle.Margin = new System.Windows.Forms.Padding(0, 2, 10, 2);
            this.lblRegularTitle.Name = "lblRegularTitle";
            this.lblRegularTitle.Size = new System.Drawing.Size(82, 19);
            this.lblRegularTitle.TabIndex = 23;
            this.lblRegularTitle.Text = "Regular total";
            // 
            // lblRegularTotal
            // 
            this.lblRegularTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegularTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegularTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblRegularTotal.Location = new System.Drawing.Point(92, 78);
            this.lblRegularTotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblRegularTotal.Name = "lblRegularTotal";
            this.lblRegularTotal.Size = new System.Drawing.Size(422, 23);
            this.lblRegularTotal.TabIndex = 24;
            this.lblRegularTotal.Text = "0.00";
            this.lblRegularTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDropTitle
            // 
            this.lblDropTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDropTitle.AutoSize = true;
            this.lblDropTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblDropTitle.Location = new System.Drawing.Point(0, 105);
            this.lblDropTitle.Margin = new System.Windows.Forms.Padding(0, 2, 10, 2);
            this.lblDropTitle.Name = "lblDropTitle";
            this.lblDropTitle.Size = new System.Drawing.Size(68, 19);
            this.lblDropTitle.TabIndex = 25;
            this.lblDropTitle.Text = "Price drop";
            // 
            // lblDropTotal
            // 
            this.lblDropTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDropTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(142)))), ((int)(((byte)(90)))));
            this.lblDropTotal.Location = new System.Drawing.Point(92, 103);
            this.lblDropTotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblDropTotal.Name = "lblDropTotal";
            this.lblDropTotal.Size = new System.Drawing.Size(422, 23);
            this.lblDropTotal.TabIndex = 26;
            this.lblDropTotal.Text = "0.00";
            this.lblDropTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPromoTitle
            // 
            this.lblPromoTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPromoTitle.AutoSize = true;
            this.lblPromoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromoTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPromoTitle.Location = new System.Drawing.Point(0, 130);
            this.lblPromoTitle.Margin = new System.Windows.Forms.Padding(0, 2, 10, 2);
            this.lblPromoTitle.Name = "lblPromoTitle";
            this.lblPromoTitle.Size = new System.Drawing.Size(76, 19);
            this.lblPromoTitle.TabIndex = 27;
            this.lblPromoTitle.Text = "Promo total";
            // 
            // lblPromoTotal
            // 
            this.lblPromoTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPromoTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPromoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblPromoTotal.Location = new System.Drawing.Point(92, 128);
            this.lblPromoTotal.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblPromoTotal.Name = "lblPromoTotal";
            this.lblPromoTotal.Size = new System.Drawing.Size(422, 23);
            this.lblPromoTotal.TabIndex = 28;
            this.lblPromoTotal.Text = "0.00";
            this.lblPromoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPreviewNote
            // 
            this.lblPreviewNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreviewNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPreviewNote.Location = new System.Drawing.Point(0, 149);
            this.lblPreviewNote.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.lblPreviewNote.Name = "lblPreviewNote";
            this.lblPreviewNote.Size = new System.Drawing.Size(514, 34);
            this.lblPreviewNote.TabIndex = 29;
            this.lblPreviewNote.Text = "Only the appointment lines of the selected services are discounted. A promotion without selected service applies to the whole bill.";
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
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.btnCancel.Location = new System.Drawing.Point(756, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSave.FlatAppearance.BorderSize = 1;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(868, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 34);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
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
            // PromotionEditForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(980, 640);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotionEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promotion";
            this.Load += new System.EventHandler(this.PromotionEditForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.tlpLeft.ResumeLayout(false);
            this.tlpLeft.PerformLayout();
            this.tlpRight.ResumeLayout(false);
            this.grpServices.ResumeLayout(false);
            this.tlpServices.ResumeLayout(false);
            this.tlpServices.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServicePick)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.tlpPreview.ResumeLayout(false);
            this.tlpPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpLeft;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblConditions;
        private System.Windows.Forms.TextBox txtConditions;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.TableLayoutPanel tlpRight;
        private System.Windows.Forms.GroupBox grpServices;
        private System.Windows.Forms.TableLayoutPanel tlpServices;
        private System.Windows.Forms.CheckBox chkApplyAll;
        private System.Windows.Forms.Label lblServiceSearch;
        private System.Windows.Forms.TextBox txtServiceSearch;
        private System.Windows.Forms.Label lblServiceCategory;
        private System.Windows.Forms.ComboBox cboServiceCategory;
        private System.Windows.Forms.DataGridView dgvServicePick;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPickCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickServiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPickPrice;
        private System.Windows.Forms.Label lblSelectedCount;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.TableLayoutPanel tlpPreview;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrevService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrevRegular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrevPromo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrevSaving;
        private System.Windows.Forms.Label lblRegularTitle;
        private System.Windows.Forms.Label lblRegularTotal;
        private System.Windows.Forms.Label lblDropTitle;
        private System.Windows.Forms.Label lblDropTotal;
        private System.Windows.Forms.Label lblPromoTitle;
        private System.Windows.Forms.Label lblPromoTotal;
        private System.Windows.Forms.Label lblPreviewNote;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlFooterLine;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
