namespace LittleEreseSystemV3.Forms
{
    partial class LoginForm
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSide = new System.Windows.Forms.Panel();
            this.picSide = new System.Windows.Forms.PictureBox();
            this.lblTagline = new System.Windows.Forms.Label();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.tlpFormOuter = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLoginCard = new System.Windows.Forms.TableLayoutPanel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.pnlUsernameBox = new System.Windows.Forms.Panel();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.pnlPassword = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnShowPassword = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.pnlSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSide)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.tlpFormOuter.SuspendLayout();
            this.tlpLoginCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlUsernameBox.SuspendLayout();
            this.pnlPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.pnlSide, 0, 0);
            this.tlpMain.Controls.Add(this.pnlForm, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(920, 580);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlSide
            // 
            this.pnlSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(36)))), ((int)(((byte)(32)))));
            this.pnlSide.Controls.Add(this.picSide);
            this.pnlSide.Controls.Add(this.lblTagline);
            this.pnlSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSide.Location = new System.Drawing.Point(0, 0);
            this.pnlSide.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSide.Name = "pnlSide";
            this.pnlSide.Size = new System.Drawing.Size(460, 580);
            this.pnlSide.TabIndex = 0;
            // 
            // picSide
            // 
            // The spa photo stays inside the dark side panel: Zoom keeps the aspect ratio.
            this.picSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(36)))), ((int)(((byte)(32)))));
            this.picSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSide.Location = new System.Drawing.Point(0, 0);
            this.picSide.Margin = new System.Windows.Forms.Padding(0);
            this.picSide.Name = "picSide";
            this.picSide.Size = new System.Drawing.Size(460, 524);
            this.picSide.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSide.TabIndex = 0;
            this.picSide.TabStop = false;
            // 
            // lblTagline
            // 
            this.lblTagline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(36)))), ((int)(((byte)(32)))));
            this.lblTagline.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTagline.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTagline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(239)))), ((int)(((byte)(224)))));
            this.lblTagline.Location = new System.Drawing.Point(0, 524);
            this.lblTagline.Margin = new System.Windows.Forms.Padding(0);
            this.lblTagline.Name = "lblTagline";
            this.lblTagline.Size = new System.Drawing.Size(460, 56);
            this.lblTagline.TabIndex = 1;
            this.lblTagline.Text = "Enhancing your true beauty.";
            this.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlForm
            // 
            this.pnlForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlForm.Controls.Add(this.tlpFormOuter);
            this.pnlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForm.Location = new System.Drawing.Point(460, 0);
            this.pnlForm.Margin = new System.Windows.Forms.Padding(0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(460, 580);
            this.pnlForm.TabIndex = 1;
            // 
            // tlpFormOuter
            // 
            // Two empty side columns keep the login card in the middle of the right half.
            this.tlpFormOuter.ColumnCount = 3;
            this.tlpFormOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFormOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.tlpFormOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFormOuter.Controls.Add(this.tlpLoginCard, 1, 0);
            this.tlpFormOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFormOuter.Location = new System.Drawing.Point(0, 0);
            this.tlpFormOuter.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFormOuter.Name = "tlpFormOuter";
            this.tlpFormOuter.RowCount = 1;
            this.tlpFormOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFormOuter.Size = new System.Drawing.Size(460, 580);
            this.tlpFormOuter.TabIndex = 0;
            // 
            // tlpLoginCard
            // 
            this.tlpLoginCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlpLoginCard.AutoSize = true;
            this.tlpLoginCard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpLoginCard.ColumnCount = 1;
            this.tlpLoginCard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.tlpLoginCard.Controls.Add(this.picLogo, 0, 0);
            this.tlpLoginCard.Controls.Add(this.lblTitle, 0, 1);
            this.tlpLoginCard.Controls.Add(this.lblSubtitle, 0, 2);
            this.tlpLoginCard.Controls.Add(this.lblUsername, 0, 3);
            this.tlpLoginCard.Controls.Add(this.pnlUsernameBox, 0, 4);
            this.tlpLoginCard.Controls.Add(this.lblPassword, 0, 5);
            this.tlpLoginCard.Controls.Add(this.pnlPassword, 0, 6);
            this.tlpLoginCard.Controls.Add(this.lblError, 0, 7);
            this.tlpLoginCard.Controls.Add(this.btnLogin, 0, 8);
            this.tlpLoginCard.Controls.Add(this.lblFooter, 0, 9);
            this.tlpLoginCard.Location = new System.Drawing.Point(60, 25);
            this.tlpLoginCard.Margin = new System.Windows.Forms.Padding(0, 16, 0, 16);
            this.tlpLoginCard.Name = "tlpLoginCard";
            this.tlpLoginCard.RowCount = 10;
            // Fixed row heights keep the card exactly as tall as the sum of its parts,
            // so the fields never look squashed and the whole card fits inside the form.
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tlpLoginCard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpLoginCard.Size = new System.Drawing.Size(340, 480);
            this.tlpLoginCard.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picLogo.Location = new System.Drawing.Point(115, 0);
            this.picLogo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(110, 110);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 128);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(340, 38);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Welcome Back";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 172);
            this.lblSubtitle.Margin = new System.Windows.Forms.Padding(0, 0, 0, 26);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(340, 20);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Sign in to continue";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblUsername.Location = new System.Drawing.Point(0, 218);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(340, 20);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUsernameBox
            // 
            // The border lives on this panel and not on the text box, so the username and the
            // password field get exactly the same height, border and inner spacing.
            this.pnlUsernameBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlUsernameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUsernameBox.Controls.Add(this.txtUsername);
            this.pnlUsernameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUsernameBox.Location = new System.Drawing.Point(0, 244);
            this.pnlUsernameBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.pnlUsernameBox.Name = "pnlUsernameBox";
            this.pnlUsernameBox.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.pnlUsernameBox.Size = new System.Drawing.Size(340, 34);
            this.pnlUsernameBox.TabIndex = 4;
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(8, 4);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(0);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(322, 24);
            this.txtUsername.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.lblPassword.Location = new System.Drawing.Point(0, 296);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(340, 20);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPassword
            // 
            this.pnlPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPassword.Controls.Add(this.txtPassword);
            this.pnlPassword.Controls.Add(this.btnShowPassword);
            this.pnlPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPassword.Location = new System.Drawing.Point(0, 322);
            this.pnlPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.pnlPassword.Name = "pnlPassword";
            this.pnlPassword.Padding = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.pnlPassword.Size = new System.Drawing.Size(340, 34);
            this.pnlPassword.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(8, 4);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(294, 24);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnShowPassword
            // 
            // Shows or hides the typed password. The glyph is set in the Load event.
            this.btnShowPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShowPassword.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowPassword.FlatAppearance.BorderSize = 0;
            this.btnShowPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowPassword.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.btnShowPassword.Location = new System.Drawing.Point(302, 4);
            this.btnShowPassword.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(36, 24);
            this.btnShowPassword.TabIndex = 1;
            this.btnShowPassword.TabStop = false;
            this.btnShowPassword.Text = "\uE7B3";
            this.btnShowPassword.UseVisualStyleBackColor = false;
            this.btnShowPassword.Click += new System.EventHandler(this.btnShowPassword_Click);
            // 
            // lblError
            // 
            // Message area for "invalid password", the lock out countdown and empty fields.
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblError.Location = new System.Drawing.Point(0, 372);
            this.lblError.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(340, 40);
            this.lblError.TabIndex = 7;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnLogin.FlatAppearance.BorderSize = 1;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(0, 422);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(340, 44);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblFooter
            // 
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblFooter.Location = new System.Drawing.Point(0, 484);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(340, 18);
            this.lblFooter.TabIndex = 9;
            this.lblFooter.Text = "Little Erese\'s Aesthetics and Spa";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(920, 580);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Little Erese\'s Aesthetics and Spa - Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.pnlSide.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSide)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.tlpFormOuter.ResumeLayout(false);
            this.tlpFormOuter.PerformLayout();
            this.tlpLoginCard.ResumeLayout(false);
            this.tlpLoginCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlUsernameBox.ResumeLayout(false);
            this.pnlUsernameBox.PerformLayout();
            this.pnlPassword.ResumeLayout(false);
            this.pnlPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlSide;
        private System.Windows.Forms.PictureBox picSide;
        private System.Windows.Forms.Label lblTagline;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.TableLayoutPanel tlpFormOuter;
        private System.Windows.Forms.TableLayoutPanel tlpLoginCard;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Panel pnlUsernameBox;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel pnlPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnShowPassword;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblFooter;
    }
}
