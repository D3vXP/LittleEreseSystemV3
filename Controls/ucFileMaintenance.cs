using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// File Maintenance (Admin only): backup and restore of the whole database.
    /// </summary>
    public partial class ucFileMaintenance : UserControl
    {
        // A restore has no undo, so every admin action here checks the role again on its own.
        private const string ModuleName = "File Maintenance";

        public ucFileMaintenance()
        {
            InitializeComponent();
        }

        private void ucFileMaintenance_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvTableCounts);
            Theme.StyleGrid(dgvBackups);
            Theme.StylePrimaryButton(btnBackupNow);
            Theme.StyleDangerButton(btnRestore);
            Theme.StyleSecondaryButton(btnBrowseBackupFolder);
            Theme.StyleSecondaryButton(btnBrowseRestoreFile);
            Theme.StyleSecondaryButton(btnRefreshInfo);
            Theme.StyleSecondaryButton(btnOpenBackupFolder);

            // MainForm already hides this page for a Staff user; this is the second check.
            if (!Session.IsAdmin)
            {
                DisableActions();
                // Only a signed-in user sees the box; the smoke test and the preview tool build
                // every screen without one, and a modal box would stop them.
                if (Session.IsLoggedIn) Messages.Warn("File maintenance is only available to the owner account.");
                return;
            }

            // Backups live in the owner's Documents folder by default, because SQL Server is
            // allowed to write there without any special permission.
            txtBackupFolder.Text = DefaultBackupFolder();

            RefreshEverything();
        }

        private void btnBrowseBackupFolder_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choose the folder where backups are saved.";
                dialog.ShowNewFolderButton = true;

                if (Directory.Exists(txtBackupFolder.Text)) dialog.SelectedPath = txtBackupFolder.Text;

                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // The chosen folder may be brand new (or empty), so make sure it exists
                    // before SQL Server is asked to write into it.
                    Directory.CreateDirectory(dialog.SelectedPath);
                    txtBackupFolder.Text = dialog.SelectedPath;
                    LoadBackups();
                }
                catch (Exception ex)
                {
                    Messages.Error("That folder cannot be used." + Environment.NewLine + ex.Message);
                }
            }
        }

        private void btnBackupNow_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            string folder = txtBackupFolder.Text;
            if (string.IsNullOrEmpty(folder))
            {
                Messages.Warn("Choose the backup folder first.");
                return;
            }

            try
            {
                Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                Messages.Error("That folder cannot be used." + Environment.NewLine + ex.Message);
                return;
            }

            // A name with the date and the time means backups never overwrite each other.
            string fileName = "LittleEreseBackup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
            string path = Path.Combine(folder, fileName);

            Cursor previous = Cursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                MaintenanceRepository.BackupDatabase(path);

                // A backup that SQL Server writes is not automatically readable, so it is
                // verified at once: catching a bad file now is far better than at restore time.
                MaintenanceRepository.VerifyBackup(path);

                decimal megabytes = Math.Round((decimal)new FileInfo(path).Length / 1048576m, 2);

                ActivityRepository.Log(Session.UserId, "Backup", ModuleName,
                    "Backup saved to " + path + " (" + megabytes.ToString("N2") + " MB)");

                LoadBackups();

                Messages.Info("Backup saved and verified." + Environment.NewLine +
                    path + Environment.NewLine + megabytes.ToString("N2") + " MB");
            }
            catch (Exception ex)
            {
                Messages.Error("The backup could not be made." + Environment.NewLine + ex.Message);
            }
            finally
            {
                Cursor = previous;
            }
        }

        private void btnBrowseRestoreFile_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Choose a backup file";
                dialog.Filter = "Backup file (*.bak)|*.bak|All files (*.*)|*.*";
                dialog.CheckFileExists = true;

                if (Directory.Exists(txtBackupFolder.Text)) dialog.InitialDirectory = txtBackupFolder.Text;

                if (dialog.ShowDialog() != DialogResult.OK) return;

                txtRestoreFile.Text = dialog.FileName;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            string path = txtRestoreFile.Text;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Messages.Warn("Choose a backup file first.");
                return;
            }

            Cursor previous = Cursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                // Never restore a file SQL Server cannot read: the check is quick and it stops
                // a half-finished restore before anything is touched.
                MaintenanceRepository.VerifyBackup(path);
            }
            catch (Exception ex)
            {
                Messages.Error("That backup file cannot be used." + Environment.NewLine + ex.Message);
                return;
            }
            finally
            {
                Cursor = previous;
            }

            // Two confirmations on purpose: a restore throws away every appointment, payment
            // and client that is not inside the backup file.
            if (!Messages.Confirm("Restore the database from:" + Environment.NewLine + path +
                Environment.NewLine + Environment.NewLine + "The current data will be replaced. Continue?"))
            {
                return;
            }

            if (!Messages.Confirm("This cannot be undone. ALL current appointments, clients and payments " +
                "will be replaced by the contents of the backup file." + Environment.NewLine + Environment.NewLine +
                "Restore now?"))
            {
                return;
            }

            Cursor previousRestore = Cursor;
            Cursor = Cursors.WaitCursor;
            try
            {
                MaintenanceRepository.RestoreDatabase(path);

                ActivityRepository.Log(Session.UserId, "Restore", ModuleName, "Database restored from " + path);

                Messages.Info("The database was restored from:" + Environment.NewLine + path +
                    Environment.NewLine + Environment.NewLine + "The app will now restart.");

                // The signed-in user and every cached row belong to the OLD database, so a
                // restart is the only safe way to carry on.
                Application.Restart();
            }
            catch (Exception ex)
            {
                Messages.Error("The database could not be restored." + Environment.NewLine + ex.Message);
            }
            finally
            {
                Cursor = previousRestore;
            }
        }

        private void btnRefreshInfo_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            RefreshEverything();
        }

        private void btnOpenBackupFolder_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            try
            {
                string folder = txtBackupFolder.Text;
                if (string.IsNullOrEmpty(folder)) folder = DefaultBackupFolder();

                Directory.CreateDirectory(folder);
                Process.Start("explorer.exe", "\"" + folder + "\"");
            }
            catch (Exception ex)
            {
                Messages.Error("The folder could not be opened." + Environment.NewLine + ex.Message);
            }
        }

        // -----------------------------------------------------------------
        // Small helpers (no SQL: everything goes through MaintenanceRepository)
        // -----------------------------------------------------------------

        /// <summary>Documents\LittleEreseBackups, created when it does not exist yet.</summary>
        private string DefaultBackupFolder()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(documents, "LittleEreseBackups");

            try
            {
                Directory.CreateDirectory(folder);
            }
            catch (Exception)
            {
                // A protected Documents folder is rare; the owner can still browse for another one.
            }

            return folder;
        }

        /// <summary>Reloads the three parts of the page that come from the database or the disk.</summary>
        private void RefreshEverything()
        {
            LoadDatabaseInfo();
            LoadTableCounts();
            LoadBackups();
        }

        private void LoadDatabaseInfo()
        {
            try
            {
                DataRow info = MaintenanceRepository.GetDatabaseInfo();
                if (info == null)
                {
                    lblDbName.Text = "Database: -";
                    lblServer.Text = "Server: -";
                    lblDbSize.Text = "Database size: -";
                    return;
                }

                decimal sizeMb = Db.Get<decimal>(info["size_mb"]);

                lblDbName.Text = "Database: " + Db.Get<string>(info["db_name"], "-");
                lblServer.Text = "Server: " + Db.Get<string>(info["server_name"], "-");
                lblDbSize.Text = "Database size: " + sizeMb.ToString("N2") + " MB";
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        private void LoadTableCounts()
        {
            try
            {
                dgvTableCounts.DataSource = MaintenanceRepository.GetTableCounts();
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>Also refreshes "Last backup: ..." from the newest file in the folder.</summary>
        private void LoadBackups()
        {
            try
            {
                DataTable backups = MaintenanceRepository.ListBackups(txtBackupFolder.Text);
                dgvBackups.DataSource = backups;

                if (backups.Rows.Count == 0)
                {
                    lblLastBackup.Text = "No backup yet";
                    return;
                }

                // ListBackups returns the newest file first.
                DateTime newest = Db.Get<DateTime>(backups.Rows[0]["created_at"]);
                lblLastBackup.Text = "Last backup: " + newest.ToString("MMM dd, yyyy h:mm tt");
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        private void DisableActions()
        {
            btnBrowseBackupFolder.Enabled = false;
            btnBackupNow.Enabled = false;
            btnBrowseRestoreFile.Enabled = false;
            btnRestore.Enabled = false;
            btnRefreshInfo.Enabled = false;
            btnOpenBackupFolder.Enabled = false;
        }
    }
}
