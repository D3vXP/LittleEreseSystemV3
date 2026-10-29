namespace LittleEreseSystemV3.Controls
{
    partial class ucClients
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
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.tlpToolbar = new System.Windows.Forms.TableLayoutPanel();
            this.flpFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblConsent = new System.Windows.Forms.Label();
            this.cboConsent = new System.Windows.Forms.ComboBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.colClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBirth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastVisit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tlpDetails = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetailName = new System.Windows.Forms.Label();
            this.lblDetailContact = new System.Windows.Forms.Label();
            this.lblDetailBirth = new System.Windows.Forms.Label();
            this.lblDetailAddress = new System.Windows.Forms.Label();
            this.lblDetailConsent = new System.Windows.Forms.Label();
            this.lblAllergiesTitle = new System.Windows.Forms.Label();
            this.txtDetailAllergies = new System.Windows.Forms.TextBox();
            this.lblMedicalTitle = new System.Windows.Forms.Label();
            this.txtDetailMedical = new System.Windows.Forms.TextBox();
            this.lblTreatmentTitle = new System.Windows.Forms.Label();
            this.txtDetailTreatment = new System.Windows.Forms.TextBox();
            this.lblClinicHistoryTitle = new System.Windows.Forms.Label();
            this.txtDetailClinicHistory = new System.Windows.Forms.TextBox();
            this.lblPackagesTitle = new System.Windows.Forms.Label();
            this.dgvPackages = new System.Windows.Forms.DataGridView();
            this.colPackageId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageSessions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageExpiry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddPackage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            this.tlpToolbar.SuspendLayout();
            this.flpFilters.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.tlpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).BeginInit();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlGrid);
            this.splitMain.Panel1.Controls.Add(this.pnlToolbar);
            this.splitMain.Panel1MinSize = 400;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            this.splitMain.Panel2MinSize = 320;
            this.splitMain.Size = new System.Drawing.Size(1000, 640);
            this.splitMain.SplitterDistance = 619;
            this.splitMain.TabIndex = 0;
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlToolbar.Controls.Add(this.tlpToolbar);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Size = new System.Drawing.Size(619, 92);
            this.pnlToolbar.TabIndex = 0;
            // 
            // tlpToolbar
            // 
            this.tlpToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpToolbar.ColumnCount = 2;
            this.tlpToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpToolbar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpToolbar.Controls.Add(this.flpFilters, 0, 0);
            this.tlpToolbar.Controls.Add(this.lblCount, 1, 0);
            this.tlpToolbar.Controls.Add(this.flpButtons, 0, 1);
            this.tlpToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpToolbar.Location = new System.Drawing.Point(0, 0);
            this.tlpToolbar.Name = "tlpToolbar";
            this.tlpToolbar.RowCount = 2;
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpToolbar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpToolbar.SetColumnSpan(this.flpButtons, 2);
            this.tlpToolbar.Size = new System.Drawing.Size(619, 92);
            this.tlpToolbar.TabIndex = 0;
            // 
            // flpFilters
            // 
            this.flpFilters.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpFilters.AutoSize = true;
            this.flpFilters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpFilters.Controls.Add(this.lblSearch);
            this.flpFilters.Controls.Add(this.txtSearch);
            this.flpFilters.Controls.Add(this.lblConsent);
            this.flpFilters.Controls.Add(this.cboConsent);
            this.flpFilters.Location = new System.Drawing.Point(12, 10);
            this.flpFilters.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.flpFilters.Name = "flpFilters";
            this.flpFilters.Size = new System.Drawing.Size(373, 25);
            this.flpFilters.TabIndex = 0;
            this.flpFilters.WrapContents = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblSearch.Location = new System.Drawing.Point(0, 3);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(50, 19);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(56, 0);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(140, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblConsent
            // 
            this.lblConsent.AutoSize = true;
            this.lblConsent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConsent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblConsent.Location = new System.Drawing.Point(206, 3);
            this.lblConsent.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            this.lblConsent.Name = "lblConsent";
            this.lblConsent.Size = new System.Drawing.Size(59, 19);
            this.lblConsent.TabIndex = 2;
            this.lblConsent.Text = "Consent";
            this.lblConsent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboConsent
            // 
            this.cboConsent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConsent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboConsent.FormattingEnabled = true;
            this.cboConsent.Items.AddRange(new object[] {
            "All",
            "Signed",
            "Not signed"});
            this.cboConsent.Location = new System.Drawing.Point(271, 0);
            this.cboConsent.Margin = new System.Windows.Forms.Padding(0);
            this.cboConsent.Name = "cboConsent";
            this.cboConsent.Size = new System.Drawing.Size(102, 25);
            this.cboConsent.TabIndex = 3;
            this.cboConsent.SelectedIndexChanged += new System.EventHandler(this.cboConsent_SelectedIndexChanged);
            // 
            // lblCount
            // 
            this.lblCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblCount.Location = new System.Drawing.Point(553, 15);
            this.lblCount.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(54, 15);
            this.lblCount.TabIndex = 4;
            this.lblCount.Text = "0 clients";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flpButtons
            // 
            this.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.flpButtons.AutoSize = true;
            this.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.flpButtons.Controls.Add(this.btnAdd);
            this.flpButtons.Controls.Add(this.btnEdit);
            this.flpButtons.Controls.Add(this.btnRefresh);
            this.flpButtons.Location = new System.Drawing.Point(393, 53);
            this.flpButtons.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(214, 32);
            this.flpButtons.TabIndex = 5;
            this.flpButtons.WrapContents = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAdd.FlatAppearance.BorderSize = 1;
            this.btnAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(96, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add Client";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnEdit.FlatAppearance.BorderSize = 1;
            this.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnEdit.Location = new System.Drawing.Point(100, 0);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(60, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
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
            this.btnRefresh.Location = new System.Drawing.Point(164, 0);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(76, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.dgvClients);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 92);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(1);
            this.pnlGrid.Size = new System.Drawing.Size(619, 548);
            this.pnlGrid.TabIndex = 1;
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.AutoGenerateColumns = false;
            this.dgvClients.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClients.BackgroundColor = System.Drawing.Color.White;
            this.dgvClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvClients.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvClients.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvClients.ColumnHeadersHeight = 34;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClientId,
            this.colName,
            this.colContact,
            this.colBirth,
            this.colGender,
            this.colConsent,
            this.colLastVisit});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClients.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClients.EnableHeadersVisualStyles = false;
            this.dgvClients.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvClients.Location = new System.Drawing.Point(1, 1);
            this.dgvClients.MultiSelect = false;
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersVisible = false;
            this.dgvClients.RowTemplate.Height = 30;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(615, 544);
            this.dgvClients.TabIndex = 0;
            this.dgvClients.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClients_CellDoubleClick);
            this.dgvClients.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvClients_CellFormatting);
            this.dgvClients.SelectionChanged += new System.EventHandler(this.dgvClients_SelectionChanged);
            // 
            // colClientId
            // 
            this.colClientId.DataPropertyName = "client_id";
            this.colClientId.HeaderText = "ID";
            this.colClientId.Name = "colClientId";
            this.colClientId.ReadOnly = true;
            this.colClientId.Visible = false;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "full_name";
            this.colName.FillWeight = 24F;
            this.colName.HeaderText = "Client";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colContact
            // 
            this.colContact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colContact.DataPropertyName = "contact_number";
            this.colContact.FillWeight = 16F;
            this.colContact.HeaderText = "Contact";
            this.colContact.Name = "colContact";
            this.colContact.ReadOnly = true;
            // 
            // colBirth
            // 
            this.colBirth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBirth.DataPropertyName = "birth_date";
            this.colBirth.DefaultCellStyle.Format = "MMM dd, yyyy";
            this.colBirth.FillWeight = 13F;
            this.colBirth.HeaderText = "Birth Date";
            this.colBirth.Name = "colBirth";
            this.colBirth.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGender.DataPropertyName = "gender";
            this.colGender.FillWeight = 11F;
            this.colGender.HeaderText = "Gender";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            // 
            // colConsent
            // 
            this.colConsent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colConsent.DataPropertyName = "consent_text";
            this.colConsent.FillWeight = 14F;
            this.colConsent.HeaderText = "Consent";
            this.colConsent.Name = "colConsent";
            this.colConsent.ReadOnly = true;
            // 
            // colLastVisit
            // 
            this.colLastVisit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLastVisit.DataPropertyName = "last_visit";
            this.colLastVisit.DefaultCellStyle.Format = "MMM dd, yyyy";
            this.colLastVisit.FillWeight = 12F;
            this.colLastVisit.HeaderText = "Last Visit";
            this.colLastVisit.Name = "colLastVisit";
            this.colLastVisit.ReadOnly = true;
            // 
            // pnlDetails
            // 
            this.pnlDetails.AutoScroll = true;
            this.pnlDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.tlpDetails);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(10, 10, 10, 6);
            this.pnlDetails.Size = new System.Drawing.Size(377, 640);
            this.pnlDetails.TabIndex = 0;
            // 
            // tlpDetails
            // 
            this.tlpDetails.AutoSize = true;
            this.tlpDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tlpDetails.ColumnCount = 1;
            this.tlpDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetails.Controls.Add(this.lblDetailName, 0, 0);
            this.tlpDetails.Controls.Add(this.lblDetailContact, 0, 1);
            this.tlpDetails.Controls.Add(this.lblDetailBirth, 0, 2);
            this.tlpDetails.Controls.Add(this.lblDetailAddress, 0, 3);
            this.tlpDetails.Controls.Add(this.lblDetailConsent, 0, 4);
            this.tlpDetails.Controls.Add(this.lblAllergiesTitle, 0, 5);
            this.tlpDetails.Controls.Add(this.txtDetailAllergies, 0, 6);
            this.tlpDetails.Controls.Add(this.lblMedicalTitle, 0, 7);
            this.tlpDetails.Controls.Add(this.txtDetailMedical, 0, 8);
            this.tlpDetails.Controls.Add(this.lblTreatmentTitle, 0, 9);
            this.tlpDetails.Controls.Add(this.txtDetailTreatment, 0, 10);
            this.tlpDetails.Controls.Add(this.lblClinicHistoryTitle, 0, 11);
            this.tlpDetails.Controls.Add(this.txtDetailClinicHistory, 0, 12);
            this.tlpDetails.Controls.Add(this.lblPackagesTitle, 0, 13);
            this.tlpDetails.Controls.Add(this.dgvPackages, 0, 14);
            this.tlpDetails.Controls.Add(this.btnAddPackage, 0, 15);
            this.tlpDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDetails.Location = new System.Drawing.Point(16, 16);
            this.tlpDetails.Name = "tlpDetails";
            this.tlpDetails.Padding = new System.Windows.Forms.Padding(16, 12, 16, 8);
            this.tlpDetails.RowCount = 16;
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpDetails.Size = new System.Drawing.Size(345, 596);
            this.tlpDetails.TabIndex = 0;
            // 
            // lblDetailName
            // 
            this.lblDetailName.AutoSize = true;
            this.lblDetailName.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailName.Location = new System.Drawing.Point(16, 22);
            this.lblDetailName.Margin = new System.Windows.Forms.Padding(0, 6, 0, 4);
            this.lblDetailName.Name = "lblDetailName";
            this.lblDetailName.Size = new System.Drawing.Size(133, 30);
            this.lblDetailName.TabIndex = 0;
            this.lblDetailName.Text = "Select a client";
            // 
            // lblDetailContact
            // 
            this.lblDetailContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailContact.Location = new System.Drawing.Point(16, 56);
            this.lblDetailContact.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailContact.Name = "lblDetailContact";
            this.lblDetailContact.Size = new System.Drawing.Size(313, 22);
            this.lblDetailContact.TabIndex = 1;
            this.lblDetailContact.Text = "Contact number: -";
            this.lblDetailContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailBirth
            // 
            this.lblDetailBirth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailBirth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailBirth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailBirth.Location = new System.Drawing.Point(16, 78);
            this.lblDetailBirth.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailBirth.Name = "lblDetailBirth";
            this.lblDetailBirth.Size = new System.Drawing.Size(313, 22);
            this.lblDetailBirth.TabIndex = 2;
            this.lblDetailBirth.Text = "Birth date: -";
            this.lblDetailBirth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailAddress
            // 
            this.lblDetailAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblDetailAddress.Location = new System.Drawing.Point(16, 100);
            this.lblDetailAddress.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetailAddress.Name = "lblDetailAddress";
            this.lblDetailAddress.Size = new System.Drawing.Size(313, 22);
            this.lblDetailAddress.TabIndex = 3;
            this.lblDetailAddress.Text = "Address: -";
            this.lblDetailAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailConsent
            // 
            this.lblDetailConsent.AutoSize = true;
            this.lblDetailConsent.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailConsent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblDetailConsent.Location = new System.Drawing.Point(16, 126);
            this.lblDetailConsent.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblDetailConsent.Name = "lblDetailConsent";
            this.lblDetailConsent.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.lblDetailConsent.Size = new System.Drawing.Size(104, 25);
            this.lblDetailConsent.TabIndex = 4;
            this.lblDetailConsent.Text = "Consent: -";
            this.lblDetailConsent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAllergiesTitle
            // 
            this.lblAllergiesTitle.AutoSize = true;
            this.lblAllergiesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllergiesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblAllergiesTitle.Location = new System.Drawing.Point(16, 159);
            this.lblAllergiesTitle.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblAllergiesTitle.Name = "lblAllergiesTitle";
            this.lblAllergiesTitle.Size = new System.Drawing.Size(65, 19);
            this.lblAllergiesTitle.TabIndex = 5;
            this.lblAllergiesTitle.Text = "Allergies";
            // 
            // txtDetailAllergies
            // 
            this.txtDetailAllergies.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtDetailAllergies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailAllergies.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailAllergies.Location = new System.Drawing.Point(16, 172);
            this.txtDetailAllergies.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetailAllergies.Multiline = true;
            this.txtDetailAllergies.Name = "txtDetailAllergies";
            this.txtDetailAllergies.ReadOnly = true;
            this.txtDetailAllergies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailAllergies.Size = new System.Drawing.Size(313, 38);
            this.txtDetailAllergies.TabIndex = 6;
            // 
            // lblMedicalTitle
            // 
            this.lblMedicalTitle.AutoSize = true;
            this.lblMedicalTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblMedicalTitle.Location = new System.Drawing.Point(16, 228);
            this.lblMedicalTitle.Margin = new System.Windows.Forms.Padding(0, 8, 0, 2);
            this.lblMedicalTitle.Name = "lblMedicalTitle";
            this.lblMedicalTitle.Size = new System.Drawing.Size(113, 19);
            this.lblMedicalTitle.TabIndex = 7;
            this.lblMedicalTitle.Text = "Health conditions";
            // 
            // txtDetailMedical
            // 
            this.txtDetailMedical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtDetailMedical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailMedical.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailMedical.Location = new System.Drawing.Point(16, 249);
            this.txtDetailMedical.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetailMedical.Multiline = true;
            this.txtDetailMedical.Name = "txtDetailMedical";
            this.txtDetailMedical.ReadOnly = true;
            this.txtDetailMedical.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailMedical.Size = new System.Drawing.Size(313, 38);
            this.txtDetailMedical.TabIndex = 8;
            // 
            // lblTreatmentTitle
            // 
            this.lblTreatmentTitle.AutoSize = true;
            this.lblTreatmentTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTreatmentTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblTreatmentTitle.Location = new System.Drawing.Point(16, 305);
            this.lblTreatmentTitle.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblTreatmentTitle.Name = "lblTreatmentTitle";
            this.lblTreatmentTitle.Size = new System.Drawing.Size(277, 19);
            this.lblTreatmentTitle.TabIndex = 9;
            this.lblTreatmentTitle.Text = "Treatments at other clinics (past 6 months)";
            // 
            // txtDetailTreatment
            // 
            this.txtDetailTreatment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtDetailTreatment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailTreatment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailTreatment.Location = new System.Drawing.Point(16, 326);
            this.txtDetailTreatment.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetailTreatment.Multiline = true;
            this.txtDetailTreatment.Name = "txtDetailTreatment";
            this.txtDetailTreatment.ReadOnly = true;
            this.txtDetailTreatment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailTreatment.Size = new System.Drawing.Size(313, 38);
            this.txtDetailTreatment.TabIndex = 10;
            // 
            // lblClinicHistoryTitle
            // 
            this.lblClinicHistoryTitle.AutoSize = true;
            this.lblClinicHistoryTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClinicHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblClinicHistoryTitle.Location = new System.Drawing.Point(16, 382);
            this.lblClinicHistoryTitle.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblClinicHistoryTitle.Name = "lblClinicHistoryTitle";
            this.lblClinicHistoryTitle.Size = new System.Drawing.Size(250, 19);
            this.lblClinicHistoryTitle.TabIndex = 11;
            this.lblClinicHistoryTitle.Text = "Treatment history at Little Erese\'s";
            // 
            // txtDetailClinicHistory
            // 
            this.txtDetailClinicHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtDetailClinicHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetailClinicHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetailClinicHistory.Location = new System.Drawing.Point(16, 403);
            this.txtDetailClinicHistory.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetailClinicHistory.Multiline = true;
            this.txtDetailClinicHistory.Name = "txtDetailClinicHistory";
            this.txtDetailClinicHistory.ReadOnly = true;
            this.txtDetailClinicHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailClinicHistory.Size = new System.Drawing.Size(313, 38);
            this.txtDetailClinicHistory.TabIndex = 12;
            // 
            // lblPackagesTitle
            // 
            this.lblPackagesTitle.AutoSize = true;
            this.lblPackagesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackagesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.lblPackagesTitle.Location = new System.Drawing.Point(16, 435);
            this.lblPackagesTitle.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblPackagesTitle.Name = "lblPackagesTitle";
            this.lblPackagesTitle.Size = new System.Drawing.Size(71, 19);
            this.lblPackagesTitle.TabIndex = 13;
            this.lblPackagesTitle.Text = "Packages";
            // 
            // dgvPackages
            // 
            this.dgvPackages.AllowUserToAddRows = false;
            this.dgvPackages.AllowUserToDeleteRows = false;
            this.dgvPackages.AllowUserToResizeRows = false;
            this.dgvPackages.AutoGenerateColumns = false;
            this.dgvPackages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPackages.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvPackages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvPackages.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPackages.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPackages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPackages.ColumnHeadersHeight = 30;
            this.dgvPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPackageId,
            this.colPackageService,
            this.colPackageSessions,
            this.colPackageExpiry,
            this.colPackageStatus});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPackages.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackages.EnableHeadersVisualStyles = false;
            this.dgvPackages.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvPackages.Location = new System.Drawing.Point(16, 456);
            this.dgvPackages.Margin = new System.Windows.Forms.Padding(0);
            this.dgvPackages.MultiSelect = false;
            this.dgvPackages.Name = "dgvPackages";
            this.dgvPackages.ReadOnly = true;
            this.dgvPackages.RowHeadersVisible = false;
            this.dgvPackages.RowTemplate.Height = 26;
            this.dgvPackages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackages.Size = new System.Drawing.Size(313, 80);
            this.dgvPackages.TabIndex = 14;
            this.dgvPackages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPackages_CellFormatting);
            // 
            // colPackageId
            // 
            this.colPackageId.DataPropertyName = "package_id";
            this.colPackageId.HeaderText = "ID";
            this.colPackageId.Name = "colPackageId";
            this.colPackageId.ReadOnly = true;
            this.colPackageId.Visible = false;
            // 
            // colPackageService
            // 
            this.colPackageService.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackageService.DataPropertyName = "service_name";
            this.colPackageService.FillWeight = 34F;
            this.colPackageService.HeaderText = "Service";
            this.colPackageService.Name = "colPackageService";
            this.colPackageService.ReadOnly = true;
            // 
            // colPackageSessions
            // 
            this.colPackageSessions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPackageSessions.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPackageSessions.DataPropertyName = "sessions_label";
            this.colPackageSessions.FillWeight = 22F;
            this.colPackageSessions.HeaderText = "Sessions";
            this.colPackageSessions.Name = "colPackageSessions";
            this.colPackageSessions.ReadOnly = true;
            // 
            // colPackageExpiry
            // 
            this.colPackageExpiry.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackageExpiry.DataPropertyName = "expiry_date";
            this.colPackageExpiry.DefaultCellStyle.Format = "MMM dd, yyyy";
            this.colPackageExpiry.FillWeight = 26F;
            this.colPackageExpiry.HeaderText = "Expires";
            this.colPackageExpiry.Name = "colPackageExpiry";
            this.colPackageExpiry.ReadOnly = true;
            // 
            // colPackageStatus
            // 
            this.colPackageStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.colPackageStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPackageStatus.DataPropertyName = "status";
            this.colPackageStatus.FillWeight = 18F;
            this.colPackageStatus.HeaderText = "Status";
            this.colPackageStatus.Name = "colPackageStatus";
            this.colPackageStatus.ReadOnly = true;
            // 
            // btnAddPackage
            // 
            this.btnAddPackage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddPackage.BackColor = System.Drawing.Color.White;
            this.btnAddPackage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddPackage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnAddPackage.FlatAppearance.BorderSize = 1;
            this.btnAddPackage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnAddPackage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnAddPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPackage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPackage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnAddPackage.Location = new System.Drawing.Point(205, 544);
            this.btnAddPackage.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.btnAddPackage.Name = "btnAddPackage";
            this.btnAddPackage.Size = new System.Drawing.Size(116, 32);
            this.btnAddPackage.TabIndex = 13;
            this.btnAddPackage.Text = "Add Package";
            this.btnAddPackage.UseVisualStyleBackColor = false;
            this.btnAddPackage.Click += new System.EventHandler(this.btnAddPackage_Click);
            // 
            // ucClients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.splitMain);
            this.Name = "ucClients";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucClients_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            this.tlpToolbar.ResumeLayout(false);
            this.tlpToolbar.PerformLayout();
            this.flpFilters.ResumeLayout(false);
            this.flpFilters.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.tlpDetails.ResumeLayout(false);
            this.tlpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.TableLayoutPanel tlpToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpFilters;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblConsent;
        private System.Windows.Forms.ComboBox cboConsent;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBirth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastVisit;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tlpDetails;
        private System.Windows.Forms.Label lblDetailName;
        private System.Windows.Forms.Label lblDetailContact;
        private System.Windows.Forms.Label lblDetailBirth;
        private System.Windows.Forms.Label lblDetailAddress;
        private System.Windows.Forms.Label lblDetailConsent;
        private System.Windows.Forms.Label lblAllergiesTitle;
        private System.Windows.Forms.TextBox txtDetailAllergies;
        private System.Windows.Forms.Label lblMedicalTitle;
        private System.Windows.Forms.TextBox txtDetailMedical;
        private System.Windows.Forms.Label lblTreatmentTitle;
        private System.Windows.Forms.TextBox txtDetailTreatment;
        private System.Windows.Forms.Label lblClinicHistoryTitle;
        private System.Windows.Forms.TextBox txtDetailClinicHistory;
        private System.Windows.Forms.Label lblPackagesTitle;
        private System.Windows.Forms.DataGridView dgvPackages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageSessions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageExpiry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageStatus;
        private System.Windows.Forms.Button btnAddPackage;
    }
}
