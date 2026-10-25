namespace LittleEreseSystemV3.Controls
{
    partial class ucFileMaintenance
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpBackup = new System.Windows.Forms.GroupBox();
            this.tlpBackup = new System.Windows.Forms.TableLayoutPanel();
            this.lblBackupInfo = new System.Windows.Forms.Label();
            this.lblBackupFolder = new System.Windows.Forms.Label();
            this.txtBackupFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseBackupFolder = new System.Windows.Forms.Button();
            this.lblLastBackup = new System.Windows.Forms.Label();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.grpRestore = new System.Windows.Forms.GroupBox();
            this.tlpRestore = new System.Windows.Forms.TableLayoutPanel();
            this.lblRestoreWarning = new System.Windows.Forms.Label();
            this.lblRestoreFile = new System.Windows.Forms.Label();
            this.txtRestoreFile = new System.Windows.Forms.TextBox();
            this.btnBrowseRestoreFile = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.grpBackup.SuspendLayout();
            this.tlpBackup.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.tlpRestore.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.grpBackup, 0, 0);
            this.tlpMain.Controls.Add(this.grpRestore, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(1000, 640);
            this.tlpMain.TabIndex = 0;
            // 
            // grpBackup
            // 
            this.grpBackup.BackColor = System.Drawing.Color.White;
            this.grpBackup.Controls.Add(this.tlpBackup);
            this.grpBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBackup.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.grpBackup.Location = new System.Drawing.Point(15, 15);
            this.grpBackup.Margin = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this.grpBackup.Name = "grpBackup";
            this.grpBackup.Padding = new System.Windows.Forms.Padding(14, 10, 14, 12);
            this.grpBackup.Size = new System.Drawing.Size(482, 301);
            this.grpBackup.TabIndex = 0;
            this.grpBackup.TabStop = false;
            this.grpBackup.Text = "Backup database";
            // 
            // tlpBackup
            // 
            this.tlpBackup.ColumnCount = 2;
            this.tlpBackup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tlpBackup.Controls.Add(this.lblBackupInfo, 0, 0);
            this.tlpBackup.Controls.Add(this.lblBackupFolder, 0, 1);
            this.tlpBackup.Controls.Add(this.txtBackupFolder, 0, 2);
            this.tlpBackup.Controls.Add(this.btnBrowseBackupFolder, 1, 2);
            this.tlpBackup.Controls.Add(this.lblLastBackup, 0, 3);
            this.tlpBackup.Controls.Add(this.btnBackupNow, 0, 4);
            this.tlpBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackup.Location = new System.Drawing.Point(14, 10);
            this.tlpBackup.Name = "tlpBackup";
            this.tlpBackup.RowCount = 5;
            this.tlpBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackup.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBackup.Size = new System.Drawing.Size(454, 279);
            this.tlpBackup.TabIndex = 0;
            // 
            // lblBackupInfo
            // 
            this.lblBackupInfo.AutoSize = true;
            this.tlpBackup.SetColumnSpan(this.lblBackupInfo, 2);
            this.lblBackupInfo.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.lblBackupInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackupInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblBackupInfo.Location = new System.Drawing.Point(0, 0);
            this.lblBackupInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblBackupInfo.MaximumSize = new System.Drawing.Size(440, 0);
            this.lblBackupInfo.Name = "lblBackupInfo";
            this.lblBackupInfo.Size = new System.Drawing.Size(440, 30);
            this.lblBackupInfo.TabIndex = 0;
            this.lblBackupInfo.Text = "Saves a copy of the whole database to the backup folder. Keep a copy on a USB drive or another computer as well.";
            // 
            // lblBackupFolder
            // 
            this.lblBackupFolder.AutoSize = true;
            this.lblBackupFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackupFolder.Location = new System.Drawing.Point(0, 43);
            this.lblBackupFolder.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.lblBackupFolder.Name = "lblBackupFolder";
            this.lblBackupFolder.Size = new System.Drawing.Size(102, 19);
            this.lblBackupFolder.TabIndex = 1;
            this.lblBackupFolder.Text = "Backup folder";
            // 
            // txtBackupFolder
            // 
            this.txtBackupFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBackupFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBackupFolder.Location = new System.Drawing.Point(0, 68);
            this.txtBackupFolder.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.txtBackupFolder.Name = "txtBackupFolder";
            this.txtBackupFolder.ReadOnly = true;
            this.txtBackupFolder.Size = new System.Drawing.Size(350, 25);
            this.txtBackupFolder.TabIndex = 2;
            // 
            // btnBrowseBackupFolder
            // 
            this.btnBrowseBackupFolder.BackColor = System.Drawing.Color.White;
            this.btnBrowseBackupFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseBackupFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseBackupFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnBrowseBackupFolder.FlatAppearance.BorderSize = 1;
            this.btnBrowseBackupFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnBrowseBackupFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnBrowseBackupFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseBackupFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBackupFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnBrowseBackupFolder.Location = new System.Drawing.Point(358, 68);
            this.btnBrowseBackupFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnBrowseBackupFolder.Name = "btnBrowseBackupFolder";
            this.btnBrowseBackupFolder.Size = new System.Drawing.Size(96, 29);
            this.btnBrowseBackupFolder.TabIndex = 3;
            this.btnBrowseBackupFolder.Text = "Browse...";
            this.btnBrowseBackupFolder.UseVisualStyleBackColor = false;
            this.btnBrowseBackupFolder.Click += new System.EventHandler(this.btnBrowseBackupFolder_Click);
            // 
            // lblLastBackup
            // 
            this.lblLastBackup.AutoSize = true;
            this.tlpBackup.SetColumnSpan(this.lblLastBackup, 2);
            this.lblLastBackup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblLastBackup.Location = new System.Drawing.Point(0, 100);
            this.lblLastBackup.Margin = new System.Windows.Forms.Padding(0, 3, 0, 8);
            this.lblLastBackup.Name = "lblLastBackup";
            this.lblLastBackup.Size = new System.Drawing.Size(104, 15);
            this.lblLastBackup.TabIndex = 4;
            this.lblLastBackup.Text = "Last backup: never";
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnBackupNow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackupNow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnBackupNow.FlatAppearance.BorderSize = 1;
            this.btnBackupNow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            this.btnBackupNow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            this.btnBackupNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupNow.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackupNow.ForeColor = System.Drawing.Color.White;
            this.btnBackupNow.Location = new System.Drawing.Point(0, 126);
            this.btnBackupNow.Margin = new System.Windows.Forms.Padding(0);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(130, 34);
            this.btnBackupNow.TabIndex = 5;
            this.btnBackupNow.Text = "Backup Now";
            this.btnBackupNow.UseVisualStyleBackColor = false;
            this.btnBackupNow.Click += new System.EventHandler(this.btnBackupNow_Click);
            // 
            // grpRestore
            // 
            this.grpRestore.BackColor = System.Drawing.Color.White;
            this.grpRestore.Controls.Add(this.tlpRestore);
            this.grpRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRestore.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRestore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.grpRestore.Location = new System.Drawing.Point(503, 15);
            this.grpRestore.Margin = new System.Windows.Forms.Padding(6, 0, 0, 6);
            this.grpRestore.Name = "grpRestore";
            this.grpRestore.Padding = new System.Windows.Forms.Padding(14, 10, 14, 12);
            this.grpRestore.Size = new System.Drawing.Size(482, 301);
            this.grpRestore.TabIndex = 1;
            this.grpRestore.TabStop = false;
            this.grpRestore.Text = "Restore database";
            // 
            // tlpRestore
            // 
            this.tlpRestore.ColumnCount = 2;
            this.tlpRestore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRestore.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tlpRestore.Controls.Add(this.lblRestoreWarning, 0, 0);
            this.tlpRestore.Controls.Add(this.lblRestoreFile, 0, 1);
            this.tlpRestore.Controls.Add(this.txtRestoreFile, 0, 2);
            this.tlpRestore.Controls.Add(this.btnBrowseRestoreFile, 1, 2);
            this.tlpRestore.Controls.Add(this.btnRestore, 0, 3);
            this.tlpRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRestore.Location = new System.Drawing.Point(14, 10);
            this.tlpRestore.Name = "tlpRestore";
            this.tlpRestore.RowCount = 4;
            this.tlpRestore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRestore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRestore.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpRestore.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRestore.Size = new System.Drawing.Size(454, 279);
            this.tlpRestore.TabIndex = 0;
            // 
            // lblRestoreWarning
            // 
            this.lblRestoreWarning.AutoSize = true;
            this.tlpRestore.SetColumnSpan(this.lblRestoreWarning, 2);
            this.lblRestoreWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestoreWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblRestoreWarning.Location = new System.Drawing.Point(0, 0);
            this.lblRestoreWarning.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblRestoreWarning.MaximumSize = new System.Drawing.Size(440, 0);
            this.lblRestoreWarning.Name = "lblRestoreWarning";
            this.lblRestoreWarning.Size = new System.Drawing.Size(440, 34);
            this.lblRestoreWarning.TabIndex = 0;
            this.lblRestoreWarning.Text = "Restoring replaces ALL current data with the contents of the backup file.";
            // 
            // lblRestoreFile
            // 
            this.lblRestoreFile.AutoSize = true;
            this.lblRestoreFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestoreFile.Location = new System.Drawing.Point(0, 47);
            this.lblRestoreFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.lblRestoreFile.Name = "lblRestoreFile";
            this.lblRestoreFile.Size = new System.Drawing.Size(84, 19);
            this.lblRestoreFile.TabIndex = 1;
            this.lblRestoreFile.Text = "Backup file";
            // 
            // txtRestoreFile
            // 
            this.txtRestoreFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRestoreFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRestoreFile.Location = new System.Drawing.Point(0, 72);
            this.txtRestoreFile.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.txtRestoreFile.Name = "txtRestoreFile";
            this.txtRestoreFile.ReadOnly = true;
            this.txtRestoreFile.Size = new System.Drawing.Size(350, 25);
            this.txtRestoreFile.TabIndex = 2;
            // 
            // btnBrowseRestoreFile
            // 
            this.btnBrowseRestoreFile.BackColor = System.Drawing.Color.White;
            this.btnBrowseRestoreFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseRestoreFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseRestoreFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnBrowseRestoreFile.FlatAppearance.BorderSize = 1;
            this.btnBrowseRestoreFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnBrowseRestoreFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnBrowseRestoreFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseRestoreFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseRestoreFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnBrowseRestoreFile.Location = new System.Drawing.Point(358, 72);
            this.btnBrowseRestoreFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnBrowseRestoreFile.Name = "btnBrowseRestoreFile";
            this.btnBrowseRestoreFile.Size = new System.Drawing.Size(96, 29);
            this.btnBrowseRestoreFile.TabIndex = 3;
            this.btnBrowseRestoreFile.Text = "Browse...";
            this.btnBrowseRestoreFile.UseVisualStyleBackColor = false;
            this.btnBrowseRestoreFile.Click += new System.EventHandler(this.btnBrowseRestoreFile_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnRestore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestore.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnRestore.FlatAppearance.BorderSize = 1;
            this.btnRestore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(47)))), ((int)(((byte)(34)))));
            this.btnRestore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(80)))), ((int)(((byte)(64)))));
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(0, 130);
            this.btnRestore.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(150, 34);
            this.btnRestore.TabIndex = 4;
            this.btnRestore.Text = "Restore Database";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // ucFileMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.Controls.Add(this.tlpMain);
            this.Name = "ucFileMaintenance";
            this.Size = new System.Drawing.Size(1000, 640);
            this.Load += new System.EventHandler(this.ucFileMaintenance_Load);
            this.tlpMain.ResumeLayout(false);
            this.grpBackup.ResumeLayout(false);
            this.tlpBackup.ResumeLayout(false);
            this.tlpBackup.PerformLayout();
            this.grpRestore.ResumeLayout(false);
            this.tlpRestore.ResumeLayout(false);
            this.tlpRestore.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox grpBackup;
        private System.Windows.Forms.TableLayoutPanel tlpBackup;
        private System.Windows.Forms.Label lblBackupInfo;
        private System.Windows.Forms.Label lblBackupFolder;
        private System.Windows.Forms.TextBox txtBackupFolder;
        private System.Windows.Forms.Button btnBrowseBackupFolder;
        private System.Windows.Forms.Label lblLastBackup;
        private System.Windows.Forms.Button btnBackupNow;
        private System.Windows.Forms.GroupBox grpRestore;
        private System.Windows.Forms.TableLayoutPanel tlpRestore;
        private System.Windows.Forms.Label lblRestoreWarning;
        private System.Windows.Forms.Label lblRestoreFile;
        private System.Windows.Forms.TextBox txtRestoreFile;
        private System.Windows.Forms.Button btnBrowseRestoreFile;
        private System.Windows.Forms.Button btnRestore;
    }
}
