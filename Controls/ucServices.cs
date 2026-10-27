using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucServices : UserControl
    {
        /// <summary>One entry of the category filter. Id 0 stands for "All categories".</summary>
        private sealed class CategoryItem
        {
            public int Id;
            public string Name;

            public CategoryItem(int id, string name)
            {
                Id = id;
                Name = name;
            }

            // The combo box shows this text, so the dropdown needs no DisplayMember wiring.
            public override string ToString()
            {
                return Name;
            }
        }

        // True while the screen fills its own controls, so the filter events do not reload twice.
        private bool _loading;

        public ucServices()
        {
            InitializeComponent();
        }

        private void ucServices_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvServices);
            Theme.StylePrimaryButton(btnAdd);
            Theme.StyleSecondaryButton(btnEdit);
            Theme.StyleSecondaryButton(btnRefresh);
            Theme.StyleDangerButton(btnArchive);

            // The Designer only says "N2"; Money is the one formatter of the app.
            Money.ApplyTo(colPrice);

            // Staff only look at the price list, so the actions are hidden.
            if (!Session.IsAdmin)
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnArchive.Visible = false;
            }

            _loading = true;
            try
            {
                FillCategories();
                cboStatus.SelectedIndex = 1; // "Active" is what the owner normally wants to see
            }
            finally
            {
                _loading = false;
            }

            LoadGrid(0);
        }

        /// <summary>Fills the category filter with "All categories" plus every category.</summary>
        private void FillCategories()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add(new CategoryItem(0, "All categories"));

            try
            {
                DataTable table = ServiceRepository.GetCategories();
                foreach (DataRow row in table.Rows)
                {
                    cboCategory.Items.Add(new CategoryItem(
                        Db.Get<int>(row["category_id"]),
                        Db.Get<string>(row["category_name"])));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// Reloads the grid from the database. keepServiceId is selected again afterwards (0 = none),
        /// so the row the user just edited or archived stays highlighted.
        /// </summary>
        private void LoadGrid(int keepServiceId)
        {
            try
            {
                dgvServices.DataSource = ServiceRepository.GetServices(
                    txtSearch.Text, SelectedCategoryId(), SelectedStatus());
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            SelectService(keepServiceId);
            UpdateCount();
            UpdateButtons();
        }

        private int SelectedCategoryId()
        {
            CategoryItem item = cboCategory.SelectedItem as CategoryItem;
            return item == null ? 0 : item.Id;
        }

        private string SelectedStatus()
        {
            return cboStatus.SelectedItem == null ? "All" : cboStatus.SelectedItem.ToString();
        }

        private void SelectService(int serviceId)
        {
            dgvServices.ClearSelection();
            // Selecting through the helper keeps the current cell on a VISIBLE column - setting it to
            // the hidden id column (Cells[0]) throws InvalidOperationException.
            GridHelper.SelectRowById(dgvServices, "colServiceId", serviceId);
        }

        private void UpdateCount()
        {
            lblCount.Text = dgvServices.Rows.Count + (dgvServices.Rows.Count == 1 ? " service" : " services");
        }

        /// <summary>Enables Edit / Archive only when a row is selected, and flips Archive to Restore for archived rows.</summary>
        private void UpdateButtons()
        {
            DataGridViewRow row = dgvServices.CurrentRow;
            bool hasRow = row != null;

            bool archived = hasRow && string.Equals(
                Db.Get<string>(row.Cells["colStatus"].Value), "Archived", StringComparison.OrdinalIgnoreCase);

            btnEdit.Enabled = hasRow;
            btnArchive.Enabled = hasRow;
            btnArchive.Text = archived ? "Restore" : "Archive";
        }

        private int CurrentServiceId()
        {
            return GridHelper.GetSelectedId(dgvServices, "colServiceId");
        }

        private string CurrentServiceName()
        {
            DataGridViewRow row = dgvServices.CurrentRow;
            if (row == null) return string.Empty;
            return Db.Get<string>(row.Cells["colService"].Value);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (ServiceEditForm form = new ServiceEditForm())
            {
                form.EditId = 0;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The new service is the one the user should see, so the grid selects it.
                LoadGrid(form.SavedServiceId);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int serviceId = CurrentServiceId();
            if (serviceId <= 0) return;

            using (ServiceEditForm form = new ServiceEditForm())
            {
                form.EditId = serviceId;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                LoadGrid(form.SavedServiceId);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            int serviceId = CurrentServiceId();
            if (serviceId <= 0) return;

            string name = CurrentServiceName();
            DataGridViewRow row = dgvServices.CurrentRow;
            string status = Db.Get<string>(row.Cells["colStatus"].Value);
            bool archived = string.Equals(status, "Archived", StringComparison.OrdinalIgnoreCase);

            string newStatus = archived ? "Active" : "Archived";
            string question = archived
                ? "Restore '" + name + "' and make it bookable again?"
                : "Archive '" + name + "'? It will no longer be offered for booking.";

            if (!Messages.Confirm(question)) return;

            try
            {
                ServiceRepository.SetStatus(serviceId, newStatus);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            ActivityRepository.Log(Session.UserId, archived ? "Restore" : "Archive", "Services", name);
            LoadGrid(serviceId);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGrid(CurrentServiceId());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadGrid(0);
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadGrid(0);
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadGrid(0);
        }

        private void dgvServices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // A double click on the header or the empty area below the rows changes nothing.
            if (e.RowIndex < 0) return;
            if (!Session.IsAdmin) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }

        private void dgvServices_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void dgvServices_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            string column = dgvServices.Columns[e.ColumnIndex].Name;

            if (column == "colStatus")
            {
                // Status colors come from Theme so every screen uses the same green / red / gray.
                // A selected row keeps white text on its highlight, so the status color would be unreadable.
                e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
                e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            else if (column == "colDuration")
            {
                // "60 min" reads better than a bare number in the Duration column.
                e.Value = e.Value + " min";
                e.FormattingApplied = true;
            }
        }
    }
}
