namespace LittleEreseSystemV3.Controls
{
    partial class ucSettings
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
            this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.tlpColumn = new System.Windows.Forms.TableLayoutPanel();
            this.pnlProfile = new System.Windows.Forms.Panel();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.grpPassword = new System.Windows.Forms.GroupBox();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.lblPasswordHint = new System.Windows.Forms.Label();
            this.txtConfirmNewPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmNew = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblNew = new System.Windows.Forms.Label();
            this.txtCurrentPassword = new System.Windows.Forms.TextBox();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.pnlAbout = new System.Windows.Forms.Panel();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblAppVersion = new System.Windows.Forms.Label();
            this.lblAppName = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tlpRoot.SuspendLayout();
            this.tlpColumn.SuspendLayout();
            this.pnlProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.grpPassword.SuspendLayout();
            this.pnlAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 3;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 640F));
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRoot.Controls.Add(this.tlpColumn, 1, 1);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Location = new System.Drawing.Point(0, 0);
            this.tlpRoot.Name = "tlpRoot";
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRoot.Size = new System.Drawing.Size(1000, 640);
            this.tlpRoot.TabIndex = 0;
            // 
            // tlpColumn
            // 
            this.tlpColumn.ColumnCount = 1;
            this.tlpColumn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpColumn.Controls.Add(this.pnlProfile, 0, 0);
            this.tlpColumn.Controls.Add(this.grpPassword, 0, 1);
            this.tlpColumn.Controls.Add(this.pnlAbout, 0, 2);
            this.tlpColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpColumn.Location = new System.Drawing.Point(180, 20);
            this.tlpColumn.Name = "tlpColumn";
            this.tlpColumn.RowCount = 4;
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tlpColumn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpColumn.Size = new System.Drawing.Size(640, 600);
            this.tlpColumn.TabIndex = 0;
            // 
            // pnlProfile
            // 
            this.pnlProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProfile.Controls.Add(this.lblContact);
            this.pnlProfile.Controls.Add(this.lblUsername);
            this.pnlProfile.Controls.Add(this.lblRole);
            this.pnlProfile.Controls.Add(this.lblFullName);
            this.pnlProfile.Controls.Add(this.picAvatar);
            this.pnlProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProfile.Location = new System.Drawing.Point(0, 0);
            this.pnlProfile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.pnlProfile.Name = "pnlProfile";
            this.pnlProfile.Size = new System.Drawing.Size(640, 134);
            this.pnlProfile.TabIndex = 0;
            // 
            // lblContact
            // 
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblContact.Location = new System.Drawing.Point(130, 102);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(420, 20);
            this.lblContact.TabIndex = 4;
            this.lblContact.Text = "Contact";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUsername
            // 
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblUsername.Location = new System.Drawing.Point(130, 78);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(420, 20);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRole
            // 
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.lblRole.Location = new System.Drawing.Point(130, 54);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(420, 20);
            this.lblRole.TabIndex = 2;
            this.lblRole.Text = "Role";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblFullName.Location = new System.Drawing.Point(130, 22);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(480, 26);
            this.lblFullName.TabIndex = 1;
            this.lblFullName.Text = "Full Name";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picAvatar
            // 
            this.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAvatar.Location = new System.Drawing.Point(20, 19);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(96, 96);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picAvatar.TabIndex = 0;
            this.picAvatar.TabStop = false;
            // 
            // grpPassword
            // 
            this.grpPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpPassword.Controls.Add(this.btnChangePassword);
            this.grpPassword.Controls.Add(this.lblPasswordHint);
            this.grpPassword.Controls.Add(this.txtConfirmNewPassword);
            this.grpPassword.Controls.Add(this.lblConfirmNew);
            this.grpPassword.Controls.Add(this.txtNewPassword);
            this.grpPassword.Controls.Add(this.lblNew);
            this.grpPassword.Controls.Add(this.txtCurrentPassword);
            this.grpPassword.Controls.Add(this.lblCurrent);
            this.grpPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.grpPassword.Location = new System.Drawing.Point(0, 150);
            this.grpPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.grpPassword.Name = "grpPassword";
            this.grpPassword.Size = new System.Drawing.Size(640, 254);
            this.grpPassword.TabIndex = 1;
            this.grpPassword.TabStop = false;
            this.grpPassword.Text = "Change Password";
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnChangePassword.FlatAppearance.BorderSize = 1;
            this.btnChangePassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnChangePassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.ForeColor = System.Drawing.Color.White;
            this.btnChangePassword.Location = new System.Drawing.Point(430, 194);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(180, 34);
            this.btnChangePassword.TabIndex = 7;
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // lblPasswordHint
            // 
            this.lblPasswordHint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblPasswordHint.Location = new System.Drawing.Point(20, 202);
            this.lblPasswordHint.Name = "lblPasswordHint";
            this.lblPasswordHint.Size = new System.Drawing.Size(390, 16);
            this.lblPasswordHint.TabIndex = 6;
            this.lblPasswordHint.Text = "At least 8 characters with a letter and a number.";
            this.lblPasswordHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtConfirmNewPassword
            // 
            this.txtConfirmNewPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmNewPassword.Location = new System.Drawing.Point(20, 156);
            this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
            this.txtConfirmNewPassword.Size = new System.Drawing.Size(280, 25);
            this.txtConfirmNewPassword.TabIndex = 5;
            this.txtConfirmNewPassword.UseSystemPasswordChar = true;
            // 
            // lblConfirmNew
            // 
            this.lblConfirmNew.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblConfirmNew.Location = new System.Drawing.Point(20, 136);
            this.lblConfirmNew.Name = "lblConfirmNew";
            this.lblConfirmNew.Size = new System.Drawing.Size(240, 18);
            this.lblConfirmNew.TabIndex = 4;
            this.lblConfirmNew.Text = "Confirm new password";
            this.lblConfirmNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPassword.Location = new System.Drawing.Point(20, 101);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(280, 25);
            this.txtNewPassword.TabIndex = 3;
            this.txtNewPassword.UseSystemPasswordChar = true;
            // 
            // lblNew
            // 
            this.lblNew.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblNew.Location = new System.Drawing.Point(20, 81);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(240, 18);
            this.lblNew.TabIndex = 2;
            this.lblNew.Text = "New password";
            this.lblNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrentPassword
            // 
            this.txtCurrentPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentPassword.Location = new System.Drawing.Point(20, 46);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.Size = new System.Drawing.Size(280, 25);
            this.txtCurrentPassword.TabIndex = 1;
            this.txtCurrentPassword.UseSystemPasswordChar = true;
            // 
            // lblCurrent
            // 
            this.lblCurrent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblCurrent.Location = new System.Drawing.Point(20, 26);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(240, 18);
            this.lblCurrent.TabIndex = 0;
            this.lblCurrent.Text = "Current password";
            this.lblCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlAbout
            // 
            this.pnlAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAbout.Controls.Add(this.lblDatabase);
            this.pnlAbout.Controls.Add(this.lblAppVersion);
            this.pnlAbout.Controls.Add(this.lblAppName);
            this.pnlAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAbout.Location = new System.Drawing.Point(0, 420);
            this.pnlAbout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAbout.Name = "pnlAbout";
            this.pnlAbout.Size = new System.Drawing.Size(640, 130);
            this.pnlAbout.TabIndex = 2;
            // 
            // lblDatabase
            // 
            this.lblDatabase.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblDatabase.Location = new System.Drawing.Point(20, 80);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(600, 20);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "Database";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblAppVersion.Location = new System.Drawing.Point(20, 54);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(300, 20);
            this.lblAppVersion.TabIndex = 1;
            this.lblAppVersion.Text = "Version";
            this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppName
            // 
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblAppName.Location = new System.Drawing.Point(20, 18);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(600, 26);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "Little Erese\'s Appointment and Billing System";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ucSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.tlpRoot);
            this.Name = "ucSettings";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucSettings_Load);
            this.tlpRoot.ResumeLayout(false);
            this.tlpColumn.ResumeLayout(false);
            this.pnlProfile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.grpPassword.ResumeLayout(false);
            this.grpPassword.PerformLayout();
            this.pnlAbout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.TableLayoutPanel tlpColumn;
        private System.Windows.Forms.Panel pnlProfile;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.GroupBox grpPassword;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.TextBox txtCurrentPassword;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblConfirmNew;
        private System.Windows.Forms.TextBox txtConfirmNewPassword;
        private System.Windows.Forms.Label lblPasswordHint;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Panel pnlAbout;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblAppVersion;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
