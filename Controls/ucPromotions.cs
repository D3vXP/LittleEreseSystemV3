using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucPromotions : UserControl
    {
        // True while the screen fills its own controls, so the filter events do not reload twice.
        private bool _loading;

        public ucPromotions()
        {
            InitializeComponent();
        }

        private void ucPromotions_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvPromotions);
            Theme.StylePrimaryButton(btnAdd);
            Theme.StyleSecondaryButton(btnEdit);
            Theme.StyleSecondaryButton(btnRefresh);
            Theme.StyleDangerButton(btnToggle);

            // Staff may only see the promotions they can offer today, and they cannot change anything.
            if (!Session.IsAdmin)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnToggle.Visible = false;
                lblStatus.Visible = false;
                cboStatus.Visible = false;
            }

            try
            {
                // Move yesterday's promotions to 'Expired' before anything is shown,
                // so the owner never sees an active promo that has already ended.
                PromotionRepository.RefreshExpired();
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            _loading = true;
            try
            {
                cboStatus.SelectedIndex = 1; // "Active"
            }
            finally
            {
                _loading = false;
            }

            LoadGrid(0);
        }

        /// <summary>
        /// Reloads the grid. keepPromoId is selected again afterwards (0 = none) so the row the user
        /// just edited or deactivated stays highlighted.
        /// Staff get GetActiveValid() instead of the full list.
        /// </summary>
        private void LoadGrid(int keepPromoId)
        {
            try
            {
                if (Session.IsAdmin)
                {
                    dgvPromotions.DataSource = PromotionRepository.GetPromotions(
                        txtSearch.Text, SelectedStatus());
                }
                else
                {
                    dgvPromotions.DataSource = PromotionRepository.GetActiveValid();
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            SelectPromotion(keepPromoId);
            UpdateCount();
            UpdateButtons();
        }

        private string SelectedStatus()
        {
            return cboStatus.SelectedItem == null ? "All" : cboStatus.SelectedItem.ToString();
        }

        private void SelectPromotion(int promoId)
        {
            dgvPromotions.ClearSelection();
            // Selecting through the helper keeps the current cell on a VISIBLE column - the old code
            // used Cells[1], which breaks as soon as the first visible column changes.
            GridHelper.SelectRowById(dgvPromotions, "colPromoId", promoId);
        }

        private void UpdateCount()
        {
            int count = dgvPromotions.Rows.Count;
            lblCount.Text = count + (count == 1 ? " promotion" : " promotions");
        }

        /// <summary>
        /// The toggle button follows the selected row: "Deactivate" for Active, "Activate" for
        /// Inactive, and disabled for Expired (an expired promo can only be edited before it runs again).
        /// </summary>
        private void UpdateButtons()
        {
            DataGridViewRow row = dgvPromotions.CurrentRow;
            bool hasRow = row != null;

            string status = hasRow ? Db.Get<string>(row.Cells["colStatus"].Value) : string.Empty;

            btnEdit.Enabled = hasRow;

            if (!hasRow || string.Equals(status, "Expired", StringComparison.OrdinalIgnoreCase))
            {
                btnToggle.Enabled = false;
                btnToggle.Text = "Deactivate";
                return;
            }

            bool active = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase);
            btnToggle.Enabled = true;
            btnToggle.Text = active ? "Deactivate" : "Activate";
        }

        private int CurrentPromoId()
        {
            return GridHelper.GetSelectedId(dgvPromotions, "colPromoId");
        }

        private string CurrentPromoName()
        {
            DataGridViewRow row = dgvPromotions.CurrentRow;
            if (row == null) return string.Empty;
            return Db.Get<string>(row.Cells["colName"].Value);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (PromotionEditForm form = new PromotionEditForm())
            {
                form.EditId = 0;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                LoadGrid(form.SavedPromoId);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int promoId = CurrentPromoId();
            if (promoId <= 0) return;

            using (PromotionEditForm form = new PromotionEditForm())
            {
                form.EditId = promoId;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                LoadGrid(form.SavedPromoId);
            }
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            int promoId = CurrentPromoId();
            if (promoId <= 0) return;

            string name = CurrentPromoName();
            DataGridViewRow row = dgvPromotions.CurrentRow;
            string status = Db.Get<string>(row.Cells["colStatus"].Value);
            bool active = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase);

            string newStatus = active ? "Inactive" : "Active";
            string question = active
                ? "Deactivate '" + name + "'? Staff will no longer be able to apply it."
                : "Activate '" + name + "'?";

            if (!Messages.Confirm(question)) return;

            try
            {
                PromotionRepository.SetStatus(promoId, newStatus);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            ActivityRepository.Log(Session.UserId, newStatus == "Active" ? "Activate" : "Deactivate",
                "Promotions", name);
            LoadGrid(promoId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // A refresh also re-checks which promotions have just expired.
            try
            {
                PromotionRepository.RefreshExpired();
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            LoadGrid(CurrentPromoId());
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

        private void dgvPromotions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (!Session.IsAdmin) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }

        private void dgvPromotions_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void dgvPromotions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string column = dgvPromotions.Columns[e.ColumnIndex].Name;

            if (column == "colStatus")
            {
                e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
            else if (column == "colValue")
            {
                // A percentage shown as "15.00" looks like money, so the type decides the wording.
                string type = Db.Get<string>(dgvPromotions.Rows[e.RowIndex].Cells["colType"].Value);
                decimal amount = Db.Get<decimal>(e.Value);

                if (string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase))
                {
                    e.Value = amount.ToString("0.##", CultureInfo.InvariantCulture) + " %";
                }
                else
                {
                    e.Value = amount.ToString("N2", CultureInfo.CurrentCulture);
                }

                e.FormattingApplied = true;
            }
        }
    }
}
