using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// Activity Log (Admin only): every important action the system recorded.
    /// </summary>
    public partial class ucActivityLog : UserControl
    {
        public ucActivityLog()
        {
            InitializeComponent();
        }

        private void ucActivityLog_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvLog);
            Theme.StylePrimaryButton(btnSearch);
            Theme.StyleSecondaryButton(btnExport);

            // Reading the audit trail of the whole shop is an Admin job. MainForm already hides
            // this page for a Staff user; this is the second check the rules ask for.
            if (!Session.IsAdmin)
            {
                cboUser.Enabled = false;
                cboModule.Enabled = false;
                txtSearch.Enabled = false;
                btnSearch.Enabled = false;
                btnExport.Enabled = false;
                dgvLog.Visible = false;
                // Only a signed-in user sees the box; the smoke test and the preview tool build
                // every screen without one, and a modal box would stop them.
                if (Session.IsLoggedIn) Messages.Warn("The activity log is only available to the owner account.");
                return;
            }

            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;

            FillUserFilter();
            FillModuleFilter();

            LoadLog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!Session.IsAdmin) return;

            CsvExporter.Export(
                dgvLog,
                "activity-log-" + dtpFrom.Value.ToString("yyyyMMdd") + "-" + dtpTo.Value.ToString("yyyyMMdd"));
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            // The box would otherwise beep: Enter is not a normal character here.
            e.SuppressKeyPress = true;
            ApplyFilters();
        }

        /// <summary>The User filter: "All users" followed by every account.</summary>
        private void FillUserFilter()
        {
            try
            {
                DataTable users = UserRepository.GetUsers("", "All");

                // A "0 = everybody" row is put on top so the page never needs a special case.
                DataRow all = users.NewRow();
                all["user_id"] = 0;
                all["full_name"] = "All users";
                users.Rows.InsertAt(all, 0);

                cboUser.DataSource = users;
                cboUser.DisplayMember = "full_name";
                cboUser.ValueMember = "user_id";
                cboUser.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>The Module filter: "All modules" followed by the distinct module names.</summary>
        private void FillModuleFilter()
        {
            try
            {
                DataTable modules = ActivityRepository.GetModules();

                cboModule.Items.Clear();
                cboModule.Items.Add("All modules");

                // The item itself is the text the query compares, "All modules" means no filter.
                for (int i = 0; i < modules.Rows.Count; i++)
                {
                    cboModule.Items.Add(Db.Get<string>(modules.Rows[i]["module"]));
                }

                cboModule.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>Reloads the grid and the count for the filters on screen.</summary>
        private void LoadLog()
        {
            try
            {
                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    Messages.Warn("The From date must be on or before the To date.");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                DataTable log = ActivityRepository.GetLog(
                    dtpFrom.Value,
                    dtpTo.Value,
                    SelectedUserId(),
                    SelectedModule(),
                    txtSearch.Text);

                dgvLog.DataSource = log;
                lblCount.Text = log.Rows.Count == 1 ? "1 entry" : log.Rows.Count + " entries";
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>True when the pickers hold a sensible range; the grid is only reloaded then.</summary>
        private void ApplyFilters()
        {
            if (!Session.IsAdmin) return;

            LoadLog();
        }

        /// <summary>The chosen user id, 0 = all users (also while the box is still empty).</summary>
        private int SelectedUserId()
        {
            return cboUser.SelectedValue == null ? 0 : Db.Get<int>(cboUser.SelectedValue);
        }

        /// <summary>The chosen module, "All" = every module.</summary>
        private string SelectedModule()
        {
            string module = cboModule.SelectedItem as string;

            if (string.IsNullOrEmpty(module) || module == "All modules") return "All";

            return module;
        }
    }
}
