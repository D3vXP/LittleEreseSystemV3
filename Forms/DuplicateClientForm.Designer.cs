namespace LittleEreseSystemV3.Forms
{
    partial class DuplicateClientForm
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
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnGoBack = new System.Windows.Forms.Button();
            this.btnUseExisting = new System.Windows.Forms.Button();
            this.btnAddAnyway = new System.Windows.Forms.Button();
            this.pnlFooterLine = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.grpNew = new System.Windows.Forms.GroupBox();
            this.tlpNew = new System.Windows.Forms.TableLayoutPanel();
            this.lblNewName = new System.Windows.Forms.Label();
            this.lblNewBirth = new System.Windows.Forms.Label();
            this.lblNewGender = new System.Windows.Forms.Label();
            this.lblNewContact = new System.Windows.Forms.Label();
            this.lblNewAddress = new System.Windows.Forms.Label();
            this.pnlExistingHost = new System.Windows.Forms.Panel();
            this.grpExisting = new System.Windows.Forms.GroupBox();
            this.tlpExisting = new System.Windows.Forms.TableLayoutPanel();
            this.lblExistingName = new System.Windows.Forms.Label();
            this.lblExistingBirth = new System.Windows.Forms.Label();
            this.lblExistingGender = new System.Windows.Forms.Label();
            this.lblExistingContact = new System.Windows.Forms.Label();
            this.lblExistingAddress = new System.Windows.Forms.Label();
            this.pnlPick = new System.Windows.Forms.Panel();
            this.lblPick = new System.Windows.Forms.Label();
            this.cboExisting = new System.Windows.Forms.ComboBox();
            this.lblIntro = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.tlpCards.SuspendLayout();
            this.grpNew.SuspendLayout();
            this.tlpNew.SuspendLayout();
            this.pnlExistingHost.SuspendLayout();
            this.grpExisting.SuspendLayout();
            this.tlpExisting.SuspendLayout();
            this.pnlPick.SuspendLayout();
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
            this.pnlHeader.Size = new System.Drawing.Size(760, 56);
            this.pnlHeader.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(560, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "This name already exists";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlHeaderLine
            //
            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 55);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(760, 1);
            this.pnlHeaderLine.TabIndex = 1;
            //
            // pnlFooter
            //
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlFooter.Controls.Add(this.lblCount);
            this.pnlFooter.Controls.Add(this.btnGoBack);
            this.pnlFooter.Controls.Add(this.btnUseExisting);
            this.pnlFooter.Controls.Add(this.btnAddAnyway);
            this.pnlFooter.Controls.Add(this.pnlFooterLine);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 496);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(760, 64);
            this.pnlFooter.TabIndex = 2;
            //
            // lblCount
            //
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblCount.Location = new System.Drawing.Point(0, 1);
            this.lblCount.Name = "lblCount";
            this.lblCount.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblCount.Size = new System.Drawing.Size(292, 63);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "One client already has this name.";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnGoBack
            //
            this.btnGoBack.BackColor = System.Drawing.Color.White;
            this.btnGoBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGoBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnGoBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnGoBack.FlatAppearance.BorderSize = 1;
            this.btnGoBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnGoBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnGoBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGoBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnGoBack.Location = new System.Drawing.Point(644, 15);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(100, 34);
            this.btnGoBack.TabIndex = 2;
            this.btnGoBack.Text = "Go back";
            this.btnGoBack.UseVisualStyleBackColor = false;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            //
            // btnUseExisting
            //
            this.btnUseExisting.BackColor = System.Drawing.Color.White;
            this.btnUseExisting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUseExisting.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnUseExisting.FlatAppearance.BorderSize = 1;
            this.btnUseExisting.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnUseExisting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnUseExisting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUseExisting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseExisting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnUseExisting.Location = new System.Drawing.Point(448, 15);
            this.btnUseExisting.Name = "btnUseExisting";
            this.btnUseExisting.Size = new System.Drawing.Size(184, 34);
            this.btnUseExisting.TabIndex = 1;
            this.btnUseExisting.Text = "Use existing client";
            this.btnUseExisting.UseVisualStyleBackColor = false;
            this.btnUseExisting.Click += new System.EventHandler(this.btnUseExisting_Click);
            //
            // btnAddAnyway
            //
            this.btnAddAnyway.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnAddAnyway.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddAnyway.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAddAnyway.FlatAppearance.BorderSize = 1;
            this.btnAddAnyway.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnAddAnyway.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnAddAnyway.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAnyway.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAnyway.ForeColor = System.Drawing.Color.White;
            this.btnAddAnyway.Location = new System.Drawing.Point(300, 15);
            this.btnAddAnyway.Name = "btnAddAnyway";
            this.btnAddAnyway.Size = new System.Drawing.Size(136, 34);
            this.btnAddAnyway.TabIndex = 0;
            this.btnAddAnyway.Text = "Add anyway";
            this.btnAddAnyway.UseVisualStyleBackColor = false;
            this.btnAddAnyway.Click += new System.EventHandler(this.btnAddAnyway_Click);
            //
            // pnlFooterLine
            //
            this.pnlFooterLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.pnlFooterLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFooterLine.Location = new System.Drawing.Point(0, 0);
            this.pnlFooterLine.Name = "pnlFooterLine";
            this.pnlFooterLine.Size = new System.Drawing.Size(760, 1);
            this.pnlFooterLine.TabIndex = 3;
            //
            // pnlContent
            //
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlContent.Controls.Add(this.tlpCards);
            this.pnlContent.Controls.Add(this.lblIntro);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 56);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(760, 440);
            this.pnlContent.TabIndex = 1;
            //
            // lblIntro
            //
            this.lblIntro.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIntro.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblIntro.Location = new System.Drawing.Point(0, 0);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Padding = new System.Windows.Forms.Padding(16, 12, 16, 4);
            this.lblIntro.Size = new System.Drawing.Size(760, 52);
            this.lblIntro.TabIndex = 0;
            this.lblIntro.Text = "A client with the same first and last name is already in the book. Compare the two cards: if this is a different person, add the new client; if it is the same guest, use the record that is already there.";
            //
            // tlpCards
            //
            this.tlpCards.ColumnCount = 2;
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCards.Controls.Add(this.grpNew, 0, 0);
            this.tlpCards.Controls.Add(this.pnlExistingHost, 1, 0);
            this.tlpCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCards.Location = new System.Drawing.Point(0, 52);
            this.tlpCards.Name = "tlpCards";
            this.tlpCards.Padding = new System.Windows.Forms.Padding(16, 0, 16, 12);
            this.tlpCards.RowCount = 1;
            this.tlpCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCards.Size = new System.Drawing.Size(760, 388);
            this.tlpCards.TabIndex = 1;
            //
            // grpNew
            //
            this.grpNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpNew.Controls.Add(this.tlpNew);
            this.grpNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.grpNew.Location = new System.Drawing.Point(19, 0);
            this.grpNew.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.grpNew.Name = "grpNew";
            this.grpNew.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.grpNew.Size = new System.Drawing.Size(353, 376);
            this.grpNew.TabIndex = 0;
            this.grpNew.TabStop = false;
            this.grpNew.Text = "New client";
            //
            // tlpNew
            //
            this.tlpNew.ColumnCount = 1;
            this.tlpNew.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNew.Controls.Add(this.lblNewName, 0, 0);
            this.tlpNew.Controls.Add(this.lblNewBirth, 0, 1);
            this.tlpNew.Controls.Add(this.lblNewGender, 0, 2);
            this.tlpNew.Controls.Add(this.lblNewContact, 0, 3);
            this.tlpNew.Controls.Add(this.lblNewAddress, 0, 4);
            this.tlpNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpNew.Location = new System.Drawing.Point(10, 24);
            this.tlpNew.Name = "tlpNew";
            this.tlpNew.RowCount = 5;
            this.tlpNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNew.Size = new System.Drawing.Size(333, 342);
            this.tlpNew.TabIndex = 0;
            //
            // lblNewName
            //
            this.lblNewName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblNewName.Location = new System.Drawing.Point(0, 0);
            this.lblNewName.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(333, 48);
            this.lblNewName.TabIndex = 0;
            this.lblNewName.Text = "-";
            this.lblNewName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblNewBirth
            //
            this.lblNewBirth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewBirth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewBirth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblNewBirth.Location = new System.Drawing.Point(0, 48);
            this.lblNewBirth.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewBirth.Name = "lblNewBirth";
            this.lblNewBirth.Size = new System.Drawing.Size(333, 34);
            this.lblNewBirth.TabIndex = 1;
            this.lblNewBirth.Text = "Birth date: -";
            this.lblNewBirth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblNewGender
            //
            this.lblNewGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblNewGender.Location = new System.Drawing.Point(0, 82);
            this.lblNewGender.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewGender.Name = "lblNewGender";
            this.lblNewGender.Size = new System.Drawing.Size(333, 34);
            this.lblNewGender.TabIndex = 2;
            this.lblNewGender.Text = "Gender: -";
            this.lblNewGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblNewContact
            //
            this.lblNewContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblNewContact.Location = new System.Drawing.Point(0, 116);
            this.lblNewContact.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewContact.Name = "lblNewContact";
            this.lblNewContact.Size = new System.Drawing.Size(333, 34);
            this.lblNewContact.TabIndex = 3;
            this.lblNewContact.Text = "Contact number: -";
            this.lblNewContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblNewAddress
            //
            this.lblNewAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNewAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblNewAddress.Location = new System.Drawing.Point(0, 150);
            this.lblNewAddress.Margin = new System.Windows.Forms.Padding(0);
            this.lblNewAddress.Name = "lblNewAddress";
            this.lblNewAddress.Size = new System.Drawing.Size(333, 192);
            this.lblNewAddress.TabIndex = 4;
            this.lblNewAddress.Text = "Address: -";
            this.lblNewAddress.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // pnlExistingHost
            //
            this.pnlExistingHost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlExistingHost.Controls.Add(this.grpExisting);
            this.pnlExistingHost.Controls.Add(this.pnlPick);
            this.pnlExistingHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExistingHost.Location = new System.Drawing.Point(388, 0);
            this.pnlExistingHost.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.pnlExistingHost.Name = "pnlExistingHost";
            this.pnlExistingHost.Size = new System.Drawing.Size(353, 376);
            this.pnlExistingHost.TabIndex = 1;
            //
            // grpExisting
            //
            this.grpExisting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpExisting.Controls.Add(this.tlpExisting);
            this.grpExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpExisting.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpExisting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.grpExisting.Location = new System.Drawing.Point(0, 48);
            this.grpExisting.Name = "grpExisting";
            this.grpExisting.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.grpExisting.Size = new System.Drawing.Size(353, 328);
            this.grpExisting.TabIndex = 1;
            this.grpExisting.TabStop = false;
            this.grpExisting.Text = "Existing client";
            //
            // tlpExisting
            //
            this.tlpExisting.ColumnCount = 1;
            this.tlpExisting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExisting.Controls.Add(this.lblExistingName, 0, 0);
            this.tlpExisting.Controls.Add(this.lblExistingBirth, 0, 1);
            this.tlpExisting.Controls.Add(this.lblExistingGender, 0, 2);
            this.tlpExisting.Controls.Add(this.lblExistingContact, 0, 3);
            this.tlpExisting.Controls.Add(this.lblExistingAddress, 0, 4);
            this.tlpExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExisting.Location = new System.Drawing.Point(10, 24);
            this.tlpExisting.Name = "tlpExisting";
            this.tlpExisting.RowCount = 5;
            this.tlpExisting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpExisting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpExisting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpExisting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpExisting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExisting.Size = new System.Drawing.Size(333, 294);
            this.tlpExisting.TabIndex = 0;
            //
            // lblExistingName
            //
            this.lblExistingName.AutoSize = true;
            this.lblExistingName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExistingName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblExistingName.Location = new System.Drawing.Point(0, 0);
            this.lblExistingName.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingName.Name = "lblExistingName";
            this.lblExistingName.Size = new System.Drawing.Size(333, 48);
            this.lblExistingName.TabIndex = 0;
            this.lblExistingName.Text = "-";
            this.lblExistingName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblExistingBirth
            //
            this.lblExistingBirth.AutoSize = true;
            this.lblExistingBirth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExistingBirth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingBirth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblExistingBirth.Location = new System.Drawing.Point(0, 48);
            this.lblExistingBirth.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingBirth.Name = "lblExistingBirth";
            this.lblExistingBirth.Size = new System.Drawing.Size(333, 34);
            this.lblExistingBirth.TabIndex = 1;
            this.lblExistingBirth.Text = "Birth date: -";
            this.lblExistingBirth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblExistingGender
            //
            this.lblExistingGender.AutoSize = true;
            this.lblExistingGender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExistingGender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblExistingGender.Location = new System.Drawing.Point(0, 82);
            this.lblExistingGender.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingGender.Name = "lblExistingGender";
            this.lblExistingGender.Size = new System.Drawing.Size(333, 34);
            this.lblExistingGender.TabIndex = 2;
            this.lblExistingGender.Text = "Gender: -";
            this.lblExistingGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblExistingContact
            //
            this.lblExistingContact.AutoSize = true;
            this.lblExistingContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExistingContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblExistingContact.Location = new System.Drawing.Point(0, 116);
            this.lblExistingContact.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingContact.Name = "lblExistingContact";
            this.lblExistingContact.Size = new System.Drawing.Size(333, 34);
            this.lblExistingContact.TabIndex = 3;
            this.lblExistingContact.Text = "Contact number: -";
            this.lblExistingContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblExistingAddress
            //
            this.lblExistingAddress.AutoSize = true;
            this.lblExistingAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExistingAddress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExistingAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblExistingAddress.Location = new System.Drawing.Point(0, 150);
            this.lblExistingAddress.Margin = new System.Windows.Forms.Padding(0);
            this.lblExistingAddress.Name = "lblExistingAddress";
            this.lblExistingAddress.Size = new System.Drawing.Size(333, 144);
            this.lblExistingAddress.TabIndex = 4;
            this.lblExistingAddress.Text = "Address: -";
            this.lblExistingAddress.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // pnlPick
            //
            this.pnlPick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.pnlPick.Controls.Add(this.cboExisting);
            this.pnlPick.Controls.Add(this.lblPick);
            this.pnlPick.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPick.Location = new System.Drawing.Point(0, 0);
            this.pnlPick.Name = "pnlPick";
            this.pnlPick.Size = new System.Drawing.Size(353, 48);
            this.pnlPick.TabIndex = 0;
            //
            // lblPick
            //
            this.lblPick.AutoSize = true;
            this.lblPick.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPick.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPick.Location = new System.Drawing.Point(0, 14);
            this.lblPick.Name = "lblPick";
            this.lblPick.Size = new System.Drawing.Size(74, 19);
            this.lblPick.TabIndex = 0;
            this.lblPick.Text = "Which one";
            //
            // cboExisting
            //
            this.cboExisting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExisting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboExisting.FormattingEnabled = true;
            this.cboExisting.Location = new System.Drawing.Point(80, 10);
            this.cboExisting.Name = "cboExisting";
            this.cboExisting.Size = new System.Drawing.Size(273, 25);
            this.cboExisting.TabIndex = 1;
            this.cboExisting.SelectedIndexChanged += new System.EventHandler(this.cboExisting_SelectedIndexChanged);
            //
            // DuplicateClientForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.CancelButton = this.btnGoBack;
            this.ClientSize = new System.Drawing.Size(760, 560);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DuplicateClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "This name already exists";
            this.Load += new System.EventHandler(this.DuplicateClientForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.tlpCards.ResumeLayout(false);
            this.grpNew.ResumeLayout(false);
            this.tlpNew.ResumeLayout(false);
            this.tlpNew.PerformLayout();
            this.pnlExistingHost.ResumeLayout(false);
            this.grpExisting.ResumeLayout(false);
            this.tlpExisting.ResumeLayout(false);
            this.tlpExisting.PerformLayout();
            this.pnlPick.ResumeLayout(false);
            this.pnlPick.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlFooterLine;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnGoBack;
        private System.Windows.Forms.Button btnUseExisting;
        private System.Windows.Forms.Button btnAddAnyway;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.GroupBox grpNew;
        private System.Windows.Forms.TableLayoutPanel tlpNew;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.Label lblNewBirth;
        private System.Windows.Forms.Label lblNewGender;
        private System.Windows.Forms.Label lblNewContact;
        private System.Windows.Forms.Label lblNewAddress;
        private System.Windows.Forms.Panel pnlExistingHost;
        private System.Windows.Forms.GroupBox grpExisting;
        private System.Windows.Forms.TableLayoutPanel tlpExisting;
        private System.Windows.Forms.Label lblExistingName;
        private System.Windows.Forms.Label lblExistingBirth;
        private System.Windows.Forms.Label lblExistingGender;
        private System.Windows.Forms.Label lblExistingContact;
        private System.Windows.Forms.Label lblExistingAddress;
        private System.Windows.Forms.Panel pnlPick;
        private System.Windows.Forms.Label lblPick;
        private System.Windows.Forms.ComboBox cboExisting;
    }
}
