namespace LittleEreseSystemV3.Forms
{
    partial class ClientEditForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.chkConsentSigned = new System.Windows.Forms.CheckBox();
            this.txtTreatmentHistory = new System.Windows.Forms.TextBox();
            this.lblTreatmentHistory = new System.Windows.Forms.Label();
            this.txtMedicalHistory = new System.Windows.Forms.TextBox();
            this.lblMedicalHistory = new System.Windows.Forms.Label();
            this.txtAllergies = new System.Windows.Forms.TextBox();
            this.lblAllergies = new System.Windows.Forms.Label();
            this.clbHealth = new System.Windows.Forms.CheckedListBox();
            this.lblHealth = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.lblContact = new System.Windows.Forms.Label();
            this.cboGender = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();
            this.lblMiddleName = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlFooterLine = new System.Windows.Forms.Panel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(560, 56);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Client";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 55);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(560, 1);
            this.pnlHeaderLine.TabIndex = 1;
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlContent.Controls.Add(this.chkConsentSigned);
            this.pnlContent.Controls.Add(this.txtTreatmentHistory);
            this.pnlContent.Controls.Add(this.lblTreatmentHistory);
            this.pnlContent.Controls.Add(this.txtMedicalHistory);
            this.pnlContent.Controls.Add(this.lblMedicalHistory);
            this.pnlContent.Controls.Add(this.clbHealth);
            this.pnlContent.Controls.Add(this.lblHealth);
            this.pnlContent.Controls.Add(this.txtAllergies);
            this.pnlContent.Controls.Add(this.lblAllergies);
            this.pnlContent.Controls.Add(this.txtAddress);
            this.pnlContent.Controls.Add(this.lblAddress);
            this.pnlContent.Controls.Add(this.txtContact);
            this.pnlContent.Controls.Add(this.lblContact);
            this.pnlContent.Controls.Add(this.cboGender);
            this.pnlContent.Controls.Add(this.lblGender);
            this.pnlContent.Controls.Add(this.lblAge);
            this.pnlContent.Controls.Add(this.dtpBirthDate);
            this.pnlContent.Controls.Add(this.lblBirthDate);
            this.pnlContent.Controls.Add(this.txtLastName);
            this.pnlContent.Controls.Add(this.lblLastName);
            this.pnlContent.Controls.Add(this.txtMiddleName);
            this.pnlContent.Controls.Add(this.lblMiddleName);
            this.pnlContent.Controls.Add(this.txtFirstName);
            this.pnlContent.Controls.Add(this.lblFirstName);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 56);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(560, 640);
            this.pnlContent.TabIndex = 1;
            // 
            // chkConsentSigned
            // 
            this.chkConsentSigned.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkConsentSigned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.chkConsentSigned.Location = new System.Drawing.Point(16, 646);
            this.chkConsentSigned.Name = "chkConsentSigned";
            this.chkConsentSigned.Size = new System.Drawing.Size(520, 24);
            this.chkConsentSigned.TabIndex = 21;
            this.chkConsentSigned.Text = "Consent / waiver form signed (paper form)";
            this.chkConsentSigned.UseVisualStyleBackColor = true;
            // 
            // lblHealth
            // 
            this.lblHealth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHealth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblHealth.Location = new System.Drawing.Point(16, 354);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(520, 18);
            this.lblHealth.TabIndex = 15;
            this.lblHealth.Text = "Health conditions (tick all that apply)";
            this.lblHealth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clbHealth
            // 
            this.clbHealth.CheckOnClick = true;
            this.clbHealth.ColumnWidth = 240;
            this.clbHealth.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clbHealth.FormattingEnabled = true;
            this.clbHealth.Location = new System.Drawing.Point(16, 372);
            this.clbHealth.MultiColumn = true;
            this.clbHealth.Name = "clbHealth";
            this.clbHealth.Size = new System.Drawing.Size(520, 150);
            this.clbHealth.TabIndex = 16;
            // 
            // txtTreatmentHistory
            // 
            this.txtTreatmentHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTreatmentHistory.Location = new System.Drawing.Point(16, 598);
            this.txtTreatmentHistory.Multiline = true;
            this.txtTreatmentHistory.Name = "txtTreatmentHistory";
            this.txtTreatmentHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTreatmentHistory.Size = new System.Drawing.Size(520, 46);
            this.txtTreatmentHistory.TabIndex = 20;
            // 
            // lblTreatmentHistory
            // 
            this.lblTreatmentHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTreatmentHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblTreatmentHistory.Location = new System.Drawing.Point(16, 580);
            this.lblTreatmentHistory.Name = "lblTreatmentHistory";
            this.lblTreatmentHistory.Size = new System.Drawing.Size(520, 18);
            this.lblTreatmentHistory.TabIndex = 19;
            this.lblTreatmentHistory.Text = "Treatments in the past 6 months (other clinics)";
            this.lblTreatmentHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMedicalHistory
            // 
            this.txtMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMedicalHistory.Location = new System.Drawing.Point(16, 544);
            this.txtMedicalHistory.Multiline = true;
            this.txtMedicalHistory.Name = "txtMedicalHistory";
            this.txtMedicalHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMedicalHistory.Size = new System.Drawing.Size(520, 46);
            this.txtMedicalHistory.TabIndex = 18;
            // 
            // lblMedicalHistory
            // 
            this.lblMedicalHistory.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblMedicalHistory.Location = new System.Drawing.Point(16, 526);
            this.lblMedicalHistory.Name = "lblMedicalHistory";
            this.lblMedicalHistory.Size = new System.Drawing.Size(520, 18);
            this.lblMedicalHistory.TabIndex = 17;
            this.lblMedicalHistory.Text = "Other medical conditions";
            this.lblMedicalHistory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAllergies
            // 
            this.txtAllergies.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAllergies.Location = new System.Drawing.Point(16, 306);
            this.txtAllergies.Multiline = true;
            this.txtAllergies.Name = "txtAllergies";
            this.txtAllergies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAllergies.Size = new System.Drawing.Size(520, 46);
            this.txtAllergies.TabIndex = 14;
            // 
            // lblAllergies
            // 
            this.lblAllergies.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllergies.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblAllergies.Location = new System.Drawing.Point(16, 288);
            this.lblAllergies.Name = "lblAllergies";
            this.lblAllergies.Size = new System.Drawing.Size(520, 18);
            this.lblAllergies.TabIndex = 13;
            this.lblAllergies.Text = "Allergies";
            this.lblAllergies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddress.Location = new System.Drawing.Point(16, 261);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(520, 25);
            this.txtAddress.TabIndex = 12;
            // 
            // lblAddress
            // 
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblAddress.Location = new System.Drawing.Point(16, 242);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(520, 18);
            this.lblAddress.TabIndex = 11;
            this.lblAddress.Text = "Address";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContact
            // 
            this.txtContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContact.Location = new System.Drawing.Point(16, 215);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(240, 25);
            this.txtContact.TabIndex = 10;
            // 
            // lblContact
            // 
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblContact.Location = new System.Drawing.Point(16, 196);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(520, 18);
            this.lblContact.TabIndex = 9;
            this.lblContact.Text = "Contact number";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboGender
            // 
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboGender.FormattingEnabled = true;
            this.cboGender.Items.AddRange(new object[] {
            "Female",
            "Male",
            "Other"});
            this.cboGender.Location = new System.Drawing.Point(16, 169);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(160, 25);
            this.cboGender.TabIndex = 8;
            // 
            // lblGender
            // 
            this.lblGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblGender.Location = new System.Drawing.Point(16, 150);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(520, 18);
            this.lblGender.TabIndex = 7;
            this.lblGender.Text = "Gender";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAge
            // 
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblAge.Location = new System.Drawing.Point(232, 129);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(304, 18);
            this.lblAge.TabIndex = 6;
            this.lblAge.Text = "Age: -";
            this.lblAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpBirthDate
            // 
            this.dtpBirthDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new System.Drawing.Point(16, 123);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.ShowCheckBox = true;
            this.dtpBirthDate.Size = new System.Drawing.Size(200, 25);
            this.dtpBirthDate.TabIndex = 5;
            this.dtpBirthDate.ValueChanged += new System.EventHandler(this.dtpBirthDate_ValueChanged);
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblBirthDate.Location = new System.Drawing.Point(16, 104);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(520, 18);
            this.lblBirthDate.TabIndex = 4;
            this.lblBirthDate.Text = "Birth date";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLastName
            // 
            this.txtLastName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(370, 31);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(165, 25);
            this.txtLastName.TabIndex = 3;
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLastName.Location = new System.Drawing.Point(370, 12);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(165, 18);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Last name";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(16, 31);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(165, 25);
            this.txtFirstName.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblFirstName.Location = new System.Drawing.Point(16, 12);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(165, 18);
            this.lblFirstName.TabIndex = 0;
            this.lblFirstName.Text = "First name";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMiddleName
            // 
            this.txtMiddleName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMiddleName.Location = new System.Drawing.Point(193, 31);
            this.txtMiddleName.MaxLength = 50;
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Size = new System.Drawing.Size(165, 25);
            this.txtMiddleName.TabIndex = 2;
            // 
            // lblMiddleName
            // 
            this.lblMiddleName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMiddleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblMiddleName.Location = new System.Drawing.Point(193, 12);
            this.lblMiddleName.Name = "lblMiddleName";
            this.lblMiddleName.Size = new System.Drawing.Size(165, 18);
            this.lblMiddleName.TabIndex = 1;
            this.lblMiddleName.Text = "Middle name (optional)";
            this.lblMiddleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.pnlFooterLine);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 696);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(560, 64);
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
            this.btnCancel.Location = new System.Drawing.Point(344, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
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
            this.btnSave.Location = new System.Drawing.Point(448, 15);
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
            this.pnlFooterLine.Size = new System.Drawing.Size(560, 1);
            this.pnlFooterLine.TabIndex = 2;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ClientEditForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 700);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Client";
            this.Load += new System.EventHandler(this.ClientEditForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblMiddleName;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.ComboBox cboGender;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtContact;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblAllergies;
        private System.Windows.Forms.TextBox txtAllergies;
        private System.Windows.Forms.Label lblHealth;
        private System.Windows.Forms.CheckedListBox clbHealth;
        private System.Windows.Forms.Label lblMedicalHistory;
        private System.Windows.Forms.TextBox txtMedicalHistory;
        private System.Windows.Forms.Label lblTreatmentHistory;
        private System.Windows.Forms.TextBox txtTreatmentHistory;
        private System.Windows.Forms.CheckBox chkConsentSigned;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlFooterLine;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}