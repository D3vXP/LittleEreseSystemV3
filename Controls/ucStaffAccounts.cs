using System;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucStaffAccounts : UserControl
    {
        // True while the screen fills its own controls, so the filter events do not reload twice.
        private bool _loading;

        public ucStaffAccounts()
        {
            InitializeComponent();
        }

        private void ucStaffAccounts_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            // Managing other people's accounts is an Admin job. MainForm already hides this page
            // for a Staff user; this is the second check the rules ask for.
            if (!Session.IsAdmin)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnToggleStatus.Visible = false;
                btnResetPassword.Visible = false;
                txtSearch.Enabled = false;
                cboStatus.Enabled = false;
                lblCount.Text = "0 staff accounts";

                Messages.Warn("Only an Admin can manage staff accounts.");
                return;
            }

            Theme.StyleGrid(dgvStaff);
            Theme.StylePrimaryButton(btnAdd);
            Theme.StyleSecondaryButton(btnEdit);
            Theme.StyleSecondaryButton(btnResetPassword);
            Theme.StyleSecondaryButton(btnRefresh);
            Theme.StyleDangerButton(btnToggleStatus);

            _loading = true;
            try
            {
                cboStatus.SelectedIndex = 0; // "All" - an Admin wants to see everyone, including inactive
            }
            finally
            {
                _loading = false;
            }

            LoadGrid(0);
        }

        /// <summary>
        /// Reloads the grid from the database. keepUserId is selected again afterwards (0 = none),
        /// so the row the Admin just saved stays highlighted.
        /// </summary>
        private void LoadGrid(int keepUserId)
        {
            try
            {
                dgvStaff.DataSource = UserRepository.GetUsers(txtSearch.Text, SelectedStatus());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            SelectUser(keepUserId);
            UpdateCount();
            UpdateButtons();
        }

        private string SelectedStatus()
        {
            return cboStatus.SelectedItem == null ? "All" : cboStatus.SelectedItem.ToString();
        }

        private void SelectUser(int userId)
        {
            dgvStaff.ClearSelection();
            // Selecting through the helper keeps the current cell on a VISIBLE column - setting it to
            // the hidden id column (Cells[0]) throws InvalidOperationException.
            GridHelper.SelectRowById(dgvStaff, "colUserId", userId);
        }

        private void UpdateCount()
        {
            int count = dgvStaff.Rows.Count;
            lblCount.Text = count + (count == 1 ? " staff account" : " staff accounts");
        }

        /// <summary>
        /// Enables Edit / Deactivate / Reset only when a row is selected, and makes the danger
        /// button say what it will do to the selected account.
        /// </summary>
        private void UpdateButtons()
        {
            DataGridViewRow row = dgvStaff.CurrentRow;
            bool hasRow = row != null;

            bool inactive = hasRow && string.Equals(
                Db.Get<string>(row.Cells["colStatus"].Value), "Inactive", StringComparison.OrdinalIgnoreCase);

            btnEdit.Enabled = hasRow;
            btnToggleStatus.Enabled = hasRow;
            btnResetPassword.Enabled = hasRow;

            btnToggleStatus.Text = inactive ? "Activate" : "Deactivate";
        }

        private int CurrentUserId()
        {
            return GridHelper.GetSelectedId(dgvStaff, "colUserId");
        }

        private string CurrentFullName()
        {
            DataGridViewRow row = dgvStaff.CurrentRow;
            if (row == null) return string.Empty;
            return Db.Get<string>(row.Cells["colFullName"].Value);
        }

        private string CurrentStatus()
        {
            DataGridViewRow row = dgvStaff.CurrentRow;
            if (row == null) return string.Empty;
            return Db.Get<string>(row.Cells["colStatus"].Value);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (StaffEditForm form = new StaffEditForm())
            {
                form.EditId = 0;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The new account is the one the Admin should see, so the grid selects it.
                LoadGrid(form.SavedUserId);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int userId = CurrentUserId();
            if (userId <= 0) return;

            using (StaffEditForm form = new StaffEditForm())
            {
                form.EditId = userId;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                LoadGrid(form.SavedUserId);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            int userId = CurrentUserId();
            if (userId <= 0) return;

            using (StaffEditForm form = new StaffEditForm())
            {
                form.EditId = userId;
                form.PasswordOnly = true; // only the two password boxes stay usable
                if (form.ShowDialog(this) != DialogResult.OK) return;

                LoadGrid(form.SavedUserId);
            }
        }

        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            int userId = CurrentUserId();
            if (userId <= 0) return;

            string name = CurrentFullName();
            bool inactive = string.Equals(CurrentStatus(), "Inactive", StringComparison.OrdinalIgnoreCase);
            string newStatus = inactive ? "Active" : "Inactive";

            string question = inactive
                ? "Activate '" + name + "'? They will be able to sign in again."
                : "Deactivate '" + name + "'? They will no longer be able to sign in.";
            if (!Messages.Confirm(question)) return;

            try
            {
                UserRepository.SetStatus(userId, newStatus);
            }
            catch (InvalidOperationException ex)
            {
                // Business rule (own account / last active Admin): the message is already friendly.
                Messages.Warn(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            ActivityRepository.Log(Session.UserId, inactive ? "Activate" : "Deactivate",
                "Staff Accounts", name);

            LoadGrid(userId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGrid(CurrentUserId());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadGrid(0);
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadGrid(0);
        }

        private void dgvStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // A double click on the header or the empty area below the rows changes nothing.
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }

        private void dgvStaff_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void dgvStaff_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvStaff.Columns[e.ColumnIndex].Name == "colStatus")
            {
                // Status colors come from Theme so every screen uses the same green / red.
                // A selected row keeps white text on its highlight, so the status color would be unreadable.
                e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
                e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
        }
    }
}
