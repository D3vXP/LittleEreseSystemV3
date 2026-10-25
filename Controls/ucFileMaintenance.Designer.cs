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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.grpSystem = new System.Windows.Forms.GroupBox();
            this.tlpSystem = new System.Windows.Forms.TableLayoutPanel();
            this.lblDbName = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDbSize = new System.Windows.Forms.Label();
            this.pnlTableCounts = new System.Windows.Forms.Panel();
            this.dgvTableCounts = new System.Windows.Forms.DataGridView();
            this.colTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRowCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flpSystemButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshInfo = new System.Windows.Forms.Button();
            this.btnOpenBackupFolder = new System.Windows.Forms.Button();
            this.grpBackups = new System.Windows.Forms.GroupBox();
            this.pnlBackups = new System.Windows.Forms.Panel();
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.colBackupFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBackupSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBackupDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpMain.SuspendLayout();
            this.grpBackup.SuspendLayout();
            this.tlpBackup.SuspendLayout();
            this.grpRestore.SuspendLayout();
            this.tlpRestore.SuspendLayout();
            this.grpSystem.SuspendLayout();
            this.tlpSystem.SuspendLayout();
            this.pnlTableCounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableCounts)).BeginInit();
            this.flpSystemButtons.SuspendLayout();
            this.grpBackups.SuspendLayout();
            this.pnlBackups.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.grpBackup, 0, 0);
            this.tlpMain.Controls.Add(this.grpRestore, 1, 0);
            this.tlpMain.Controls.Add(this.grpSystem, 0, 1);
            this.tlpMain.Controls.Add(this.grpBackups, 1, 1);
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
            // grpSystem
            // 
            this.grpSystem.BackColor = System.Drawing.Color.White;
            this.grpSystem.Controls.Add(this.tlpSystem);
            this.grpSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSystem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSystem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.grpSystem.Location = new System.Drawing.Point(15, 328);
            this.grpSystem.Margin = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.grpSystem.Name = "grpSystem";
            this.grpSystem.Padding = new System.Windows.Forms.Padding(14, 10, 14, 12);
            this.grpSystem.Size = new System.Drawing.Size(482, 295);
            this.grpSystem.TabIndex = 2;
            this.grpSystem.TabStop = false;
            this.grpSystem.Text = "System information";
            // 
            // tlpSystem
            // 
            this.tlpSystem.ColumnCount = 1;
            this.tlpSystem.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystem.Controls.Add(this.lblDbName, 0, 0);
            this.tlpSystem.Controls.Add(this.lblServer, 0, 1);
            this.tlpSystem.Controls.Add(this.lblDbSize, 0, 2);
            this.tlpSystem.Controls.Add(this.pnlTableCounts, 0, 3);
            this.tlpSystem.Controls.Add(this.flpSystemButtons, 0, 4);
            this.tlpSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSystem.Location = new System.Drawing.Point(14, 10);
            this.tlpSystem.Name = "tlpSystem";
            this.tlpSystem.RowCount = 5;
            this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystem.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tlpSystem.Size = new System.Drawing.Size(454, 273);
            this.tlpSystem.TabIndex = 0;
            // 
            // lblDbName
            // 
            this.lblDbName.AutoSize = true;
            this.lblDbName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDbName.Location = new System.Drawing.Point(0, 0);
            this.lblDbName.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblDbName.Name = "lblDbName";
            this.lblDbName.Size = new System.Drawing.Size(120, 15);
            this.lblDbName.TabIndex = 0;
            this.lblDbName.Text = "Database: -";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblServer.Location = new System.Drawing.Point(0, 19);
            this.lblServer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(120, 15);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Server: -";
            // 
            // lblDbSize
            // 
            this.lblDbSize.AutoSize = true;
            this.lblDbSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDbSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(114)))), ((int)(((byte)(102)))));
            this.lblDbSize.Location = new System.Drawing.Point(0, 38);
            this.lblDbSize.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblDbSize.Name = "lblDbSize";
            this.lblDbSize.Size = new System.Drawing.Size(120, 15);
            this.lblDbSize.TabIndex = 2;
            this.lblDbSize.Text = "Database size: -";
            // 
            // pnlTableCounts
            // 
            this.pnlTableCounts.BackColor = System.Drawing.Color.White;
            this.pnlTableCounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTableCounts.Controls.Add(this.dgvTableCounts);
            this.pnlTableCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTableCounts.Location = new System.Drawing.Point(0, 61);
            this.pnlTableCounts.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlTableCounts.Name = "pnlTableCounts";
            this.pnlTableCounts.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTableCounts.Size = new System.Drawing.Size(454, 170);
            this.pnlTableCounts.TabIndex = 3;
            // 
            // dgvTableCounts
            // 
            this.dgvTableCounts.AllowUserToAddRows = false;
            this.dgvTableCounts.AllowUserToDeleteRows = false;
            this.dgvTableCounts.AllowUserToResizeRows = false;
            this.dgvTableCounts.AutoGenerateColumns = false;
            this.dgvTableCounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTableCounts.BackgroundColor = System.Drawing.Color.White;
            this.dgvTableCounts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTableCounts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTableCounts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTableCounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTableCounts.ColumnHeadersHeight = 30;
            this.dgvTableCounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTableName,
            this.colRowCount});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTableCounts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTableCounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableCounts.EnableHeadersVisualStyles = false;
            this.dgvTableCounts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvTableCounts.Location = new System.Drawing.Point(1, 1);
            this.dgvTableCounts.MultiSelect = false;
            this.dgvTableCounts.Name = "dgvTableCounts";
            this.dgvTableCounts.ReadOnly = true;
            this.dgvTableCounts.RowHeadersVisible = false;
            this.dgvTableCounts.RowTemplate.Height = 28;
            this.dgvTableCounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableCounts.Size = new System.Drawing.Size(450, 166);
            this.dgvTableCounts.TabIndex = 0;
            // 
            // colTableName
            // 
            this.colTableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTableName.DataPropertyName = "table_name";
            this.colTableName.HeaderText = "Table";
            this.colTableName.MinimumWidth = 140;
            this.colTableName.Name = "colTableName";
            this.colTableName.ReadOnly = true;
            // 
            // colRowCount
            // 
            this.colRowCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRowCount.DataPropertyName = "row_count";
            this.colRowCount.HeaderText = "Records";
            this.colRowCount.MinimumWidth = 80;
            this.colRowCount.Name = "colRowCount";
            this.colRowCount.ReadOnly = true;
            this.colRowCount.Width = 110;
            // 
            // flpSystemButtons
            // 
            this.flpSystemButtons.AutoSize = false;
            this.flpSystemButtons.Controls.Add(this.btnRefreshInfo);
            this.flpSystemButtons.Controls.Add(this.btnOpenBackupFolder);
            this.flpSystemButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSystemButtons.Location = new System.Drawing.Point(0, 239);
            this.flpSystemButtons.Margin = new System.Windows.Forms.Padding(0);
            this.flpSystemButtons.Name = "flpSystemButtons";
            this.flpSystemButtons.Size = new System.Drawing.Size(454, 34);
            this.flpSystemButtons.TabIndex = 4;
            this.flpSystemButtons.WrapContents = false;
            // 
            // btnRefreshInfo
            // 
            this.btnRefreshInfo.BackColor = System.Drawing.Color.White;
            this.btnRefreshInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefreshInfo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRefreshInfo.FlatAppearance.BorderSize = 1;
            this.btnRefreshInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnRefreshInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnRefreshInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnRefreshInfo.Location = new System.Drawing.Point(0, 0);
            this.btnRefreshInfo.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnRefreshInfo.Name = "btnRefreshInfo";
            this.btnRefreshInfo.Size = new System.Drawing.Size(96, 34);
            this.btnRefreshInfo.TabIndex = 0;
            this.btnRefreshInfo.Text = "Refresh";
            this.btnRefreshInfo.UseVisualStyleBackColor = false;
            this.btnRefreshInfo.Click += new System.EventHandler(this.btnRefreshInfo_Click);
            // 
            // btnOpenBackupFolder
            // 
            this.btnOpenBackupFolder.BackColor = System.Drawing.Color.White;
            this.btnOpenBackupFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenBackupFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnOpenBackupFolder.FlatAppearance.BorderSize = 1;
            this.btnOpenBackupFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.btnOpenBackupFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(243)))), ((int)(((byte)(234)))));
            this.btnOpenBackupFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenBackupFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenBackupFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(123)))), ((int)(((byte)(43)))));
            this.btnOpenBackupFolder.Location = new System.Drawing.Point(104, 0);
            this.btnOpenBackupFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenBackupFolder.Name = "btnOpenBackupFolder";
            this.btnOpenBackupFolder.Size = new System.Drawing.Size(172, 34);
            this.btnOpenBackupFolder.TabIndex = 1;
            this.btnOpenBackupFolder.Text = "Open Backup Folder";
            this.btnOpenBackupFolder.UseVisualStyleBackColor = false;
            this.btnOpenBackupFolder.Click += new System.EventHandler(this.btnOpenBackupFolder_Click);
            // 
            // grpBackups
            // 
            this.grpBackups.BackColor = System.Drawing.Color.White;
            this.grpBackups.Controls.Add(this.pnlBackups);
            this.grpBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBackups.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBackups.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            this.grpBackups.Location = new System.Drawing.Point(503, 328);
            this.grpBackups.Margin = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.grpBackups.Name = "grpBackups";
            this.grpBackups.Padding = new System.Windows.Forms.Padding(14, 10, 14, 12);
            this.grpBackups.Size = new System.Drawing.Size(482, 295);
            this.grpBackups.TabIndex = 3;
            this.grpBackups.TabStop = false;
            this.grpBackups.Text = "Recent backups";
            // 
            // pnlBackups
            // 
            this.pnlBackups.BackColor = System.Drawing.Color.White;
            this.pnlBackups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackups.Controls.Add(this.dgvBackups);
            this.pnlBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackups.Location = new System.Drawing.Point(14, 10);
            this.pnlBackups.Name = "pnlBackups";
            this.pnlBackups.Padding = new System.Windows.Forms.Padding(1);
            this.pnlBackups.Size = new System.Drawing.Size(454, 273);
            this.pnlBackups.TabIndex = 0;
            // 
            // dgvBackups
            // 
            this.dgvBackups.AllowUserToAddRows = false;
            this.dgvBackups.AllowUserToDeleteRows = false;
            this.dgvBackups.AllowUserToResizeRows = false;
            this.dgvBackups.AutoGenerateColumns = false;
            this.dgvBackups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBackups.BackgroundColor = System.Drawing.Color.White;
            this.dgvBackups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBackups.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBackups.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(89)))), ((int)(((byte)(28)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBackups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBackups.ColumnHeadersHeight = 34;
            this.dgvBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBackupFile,
            this.colBackupSize,
            this.colBackupDate});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBackups.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBackups.EnableHeadersVisualStyles = false;
            this.dgvBackups.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(218)))), ((int)(((byte)(198)))));
            this.dgvBackups.Location = new System.Drawing.Point(1, 1);
            this.dgvBackups.MultiSelect = false;
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.ReadOnly = true;
            this.dgvBackups.RowHeadersVisible = false;
            this.dgvBackups.RowTemplate.Height = 30;
            this.dgvBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBackups.Size = new System.Drawing.Size(450, 269);
            this.dgvBackups.TabIndex = 0;
            // 
            // colBackupFile
            // 
            this.colBackupFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBackupFile.DataPropertyName = "file_name";
            this.colBackupFile.HeaderText = "File";
            this.colBackupFile.MinimumWidth = 180;
            this.colBackupFile.Name = "colBackupFile";
            this.colBackupFile.ReadOnly = true;
            // 
            // colBackupSize
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((byte)(34)));
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colBackupSize.DefaultCellStyle = dataGridViewCellStyle5;
            this.colBackupSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBackupSize.DataPropertyName = "size_mb";
            this.colBackupSize.HeaderText = "Size (MB)";
            this.colBackupSize.MinimumWidth = 90;
            this.colBackupSize.Name = "colBackupSize";
            this.colBackupSize.ReadOnly = true;
            this.colBackupSize.Width = 100;
            // 
            // colBackupDate
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(38)))), ((int)(((byte)(34)))));
            dataGridViewCellStyle6.Format = "MMM dd, yyyy h:mm tt";
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(157)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colBackupDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colBackupDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colBackupDate.DataPropertyName = "created_at";
            this.colBackupDate.HeaderText = "Created";
            this.colBackupDate.MinimumWidth = 160;
            this.colBackupDate.Name = "colBackupDate";
            this.colBackupDate.ReadOnly = true;
            this.colBackupDate.Width = 165;
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
            this.grpSystem.ResumeLayout(false);
            this.tlpSystem.ResumeLayout(false);
            this.tlpSystem.PerformLayout();
            this.pnlTableCounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableCounts)).EndInit();
            this.flpSystemButtons.ResumeLayout(false);
            this.flpSystemButtons.PerformLayout();
            this.grpBackups.ResumeLayout(false);
            this.pnlBackups.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
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
        private System.Windows.Forms.GroupBox grpSystem;
        private System.Windows.Forms.TableLayoutPanel tlpSystem;
        private System.Windows.Forms.Label lblDbName;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblDbSize;
        private System.Windows.Forms.Panel pnlTableCounts;
        private System.Windows.Forms.DataGridView dgvTableCounts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowCount;
        private System.Windows.Forms.FlowLayoutPanel flpSystemButtons;
        private System.Windows.Forms.Button btnRefreshInfo;
        private System.Windows.Forms.Button btnOpenBackupFolder;
        private System.Windows.Forms.GroupBox grpBackups;
        private System.Windows.Forms.Panel pnlBackups;
        private System.Windows.Forms.DataGridView dgvBackups;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBackupDate;
    }
}
