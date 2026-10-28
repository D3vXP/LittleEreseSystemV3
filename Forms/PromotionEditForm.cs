using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class PromotionEditForm : Form
    {
        /// <summary>0 = adding a new promotion. The caller sets this before ShowDialog().</summary>
        public int EditId { get; set; }

        /// <summary>The promotion that was saved (the new id after an Add), so the grid can select it.</summary>
        public int SavedPromoId { get; private set; }

        private const string AllCategories = "All categories";

        // The one discount type this dialog saves (PROJECT_RULES.md: a promotion is a set of
        // services sold for a promotional price).
        private const string PromotionPriceType = "Promo Price";

        // The ticked service ids live here, NOT in the grid, so a tick survives the search and
        // the category filter (the grid only shows the services that match them right now).
        private readonly HashSet<int> _selectedServiceIds = new HashSet<int>();

        // Every Active service (service_id, service_name, category_name, price). The grid shows a
        // filtered copy of this table.
        private DataTable _pickMaster;

        private bool _loading;      // true while the form fills its own controls
        private bool _loadingGrid;  // true while the pick grid is being (re)bound

        public PromotionEditForm()
        {
            InitializeComponent();
        }

        private void PromotionEditForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            // The money columns (the designer only sets N2) go through the one formatter of the app.
            Money.ApplyTo(colPrevRegular);
            Money.ApplyTo(colPickPrice);

            _loading = true;
            try
            {
                cboStatus.SelectedIndex = 0; // "Active"

                // A new promo starts today and runs for a month; the owner can change both.
                dtpStart.Value = DateTime.Today;
                dtpEnd.Value = DateTime.Today.AddMonths(1);

                LoadCategories();
                LoadPickList(EditId);

                if (EditId > 0)
                {
                    LoadPromotion(EditId);
                }
            }
            finally
            {
                _loading = false;
            }

            UpdateSelectedCount();
            UpdatePreview();
        }

        /// <summary>Fills the category filter with "All categories" and the real category names.</summary>
        private void LoadCategories()
        {
            cboServiceCategory.Items.Clear();
            cboServiceCategory.Items.Add(AllCategories);

            try
            {
                DataTable categories = ServiceRepository.GetCategories();
                foreach (DataRow row in categories.Rows)
                {
                    cboServiceCategory.Items.Add(Db.Get<string>(row["category_name"]));
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            cboServiceCategory.SelectedIndex = 0;
        }

        /// <summary>
        /// Loads every Active service for the pick grid and remembers which ones are already linked
        /// to this promotion. promoId 0 (a new promotion) has no links yet.
        /// </summary>
        private void LoadPickList(int promoId)
        {
            _selectedServiceIds.Clear();
            _pickMaster = null;

            try
            {
                _pickMaster = PromotionRepository.GetPickList(promoId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            foreach (DataRow row in _pickMaster.Rows)
            {
                if (Db.Get<bool>(row["is_selected"]))
                {
                    _selectedServiceIds.Add(Db.Get<int>(row["service_id"]));
                }
            }

            ApplyServiceFilter();
        }

        /// <summary>
        /// Puts the services that match the search box and the category filter into the grid, and
        /// ticks the ones in the selected set. A COPY is bound, so ticking a row never changes the
        /// master list and the ticks survive a filter change.
        /// </summary>
        private void ApplyServiceFilter()
        {
            if (_pickMaster == null) return;

            string search = txtServiceSearch.Text.Trim();
            string category = cboServiceCategory.SelectedItem == null
                ? AllCategories
                : cboServiceCategory.SelectedItem.ToString();

            DataTable view = _pickMaster.Clone();

            foreach (DataRow row in _pickMaster.Rows)
            {
                string serviceName = Db.Get<string>(row["service_name"]);
                string categoryName = Db.Get<string>(row["category_name"]);

                // The search matches the service name or its category, like the Services screen.
                if (search.Length > 0
                    && serviceName.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0
                    && categoryName.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                if (!string.Equals(category, AllCategories, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(categoryName, category, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                DataRow copy = view.NewRow();
                copy["service_id"] = row["service_id"];
                copy["service_name"] = row["service_name"];
                copy["category_name"] = row["category_name"];
                copy["price"] = row["price"];
                copy["is_selected"] = _selectedServiceIds.Contains(Db.Get<int>(row["service_id"]));
                view.Rows.Add(copy);
            }

            _loadingGrid = true;
            try
            {
                dgvServicePick.DataSource = view;
                dgvServicePick.ClearSelection();
            }
            finally
            {
                _loadingGrid = false;
            }
        }

        /// <summary>Puts an existing promotion into the fields, for editing.</summary>
        private void LoadPromotion(int promoId)
        {
            DataRow row;
            try
            {
                row = PromotionRepository.GetById(promoId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                Messages.Warn("This promotion no longer exists.");
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            Text = "Edit Promotion";
            lblTitle.Text = "Edit Promotion";

            txtName.Text = Db.Get<string>(row["promo_name"]);
            txtConditions.Text = Db.Get<string>(row["description"]);

            // Every promotion is saved as a promotional price now, so an old Percentage or Fixed
            // promotion asks for one here and is converted when it is saved (see btnSave_Click).
            txtPromoPrice.Text = Db.Get<decimal>(row["promo_price"]).ToString("0.00");

            DateTime start = Db.Get<DateTime>(row["start_date"], DateTime.Today);
            DateTime end = Db.Get<DateTime>(row["end_date"], start);
            dtpStart.Value = start;
            dtpEnd.Value = end;

            string status = Db.Get<string>(row["status"], "Active");
            cboStatus.SelectedItem = status;
            if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;

            UpdatePreview();
        }

        /// <summary>The "N services selected" line under the pick grid.</summary>
        private void UpdateSelectedCount()
        {
            int count = _selectedServiceIds.Count;
            lblSelectedCount.Text = count + (count == 1 ? " service selected" : " services selected");
        }

        /// <summary>
        /// Rebuilds the price preview from the ticked services and the promotional price typed in
        /// txtPromoPrice: the regular total, what the customer saves, and a short note.
        /// The saving is the regular total minus the promo price and is never below zero.
        /// </summary>
        private void UpdatePreview()
        {
            DataTable preview = new DataTable();
            preview.Columns.Add("service_name", typeof(string));
            preview.Columns.Add("regular_price", typeof(decimal));

            decimal regularTotal = 0m;
            int shown = 0;

            if (_pickMaster != null)
            {
                foreach (DataRow row in _pickMaster.Rows)
                {
                    int serviceId = Db.Get<int>(row["service_id"]);
                    if (!_selectedServiceIds.Contains(serviceId)) continue;

                    string serviceName = Db.Get<string>(row["service_name"]);
                    decimal price = Db.Get<decimal>(row["price"]);
                    regularTotal += price;
                    shown++;

                    DataRow line = preview.NewRow();
                    line["service_name"] = serviceName;
                    line["regular_price"] = price;

                    preview.Rows.Add(line);
                }
            }

            dgvPreview.DataSource = preview;
            dgvPreview.ClearSelection();

            // The saving is what the customer keeps: the regular total minus the promotional price.
            decimal promoPrice;
            bool hasPrice = Validators.TryMoney(txtPromoPrice.Text, out promoPrice) && promoPrice > 0m;
            decimal saving = hasPrice && regularTotal > promoPrice ? regularTotal - promoPrice : 0m;

            lblRegularTotal.Text = Money.Text(regularTotal);
            lblSavingAmount.Text = Money.Text(saving);

            if (shown == 0)
            {
                lblPreviewNote.Text = "Tick at least one service to see the prices.";
            }
            else if (!hasPrice)
            {
                lblPreviewNote.Text = "Type the promotional price of these services.";
            }
            else
            {
                // POLISH: reword this sentence if the owner prefers a shorter note.
                lblPreviewNote.Text = "All the services above for " + Money.Text(promoPrice) + ".";
            }
        }

        /// <summary>The regular (undiscounted) total of the ticked services.</summary>
        private decimal RegularTotalOfSelected()
        {
            decimal total = 0m;
            if (_pickMaster == null) return total;

            foreach (DataRow row in _pickMaster.Rows)
            {
                if (!_selectedServiceIds.Contains(Db.Get<int>(row["service_id"]))) continue;
                total += Db.Get<decimal>(row["price"]);
            }

            return total;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidateFields()) return;

            try
            {
                string name = txtName.Text.Trim();
                string conditions = txtConditions.Text.Trim();

                // Every promotion saved from this dialog is a promotional price for a set of
                // services. An old Percentage / Fixed promotion is converted when it is saved,
                // because the owner has to type the promotional price before that can happen.
                string type = PromotionPriceType;
                decimal value = 0m;

                decimal promoPrice;
                Validators.TryMoney(txtPromoPrice.Text, out promoPrice);

                DateTime start = dtpStart.Value.Date;
                DateTime end = dtpEnd.Value.Date;
                string status = cboStatus.SelectedItem.ToString();

                List<int> serviceIds = new List<int>(_selectedServiceIds);

                // The promotion AND its links are written in one transaction inside the repository,
                // so a half saved promotion can never reach the database.
                SavedPromoId = PromotionRepository.SavePromotion(
                    EditId, name, conditions, type, value, promoPrice, start, end, status, serviceIds);

                ActivityRepository.Log(Session.UserId,
                    EditId > 0 ? "Edit Promotion" : "Add Promotion",
                    "Promotions",
                    name + " (" + serviceIds.Count + " linked " +
                    (serviceIds.Count == 1 ? "service" : "services") + ")");
            }
            catch (InvalidOperationException ex)
            {
                // A friendly repository rule (for example a duplicate name) is a warning, not a crash.
                Messages.Warn(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>Shows the red error icons and returns false when something is still wrong.</summary>
        private bool ValidateFields()
        {
            bool ok = true;

            // 1. The name is required and unique.
            string name = txtName.Text.Trim();
            if (!Validators.IsLengthBetween(name, 2, 100))
            {
                Validators.SetError(errorProvider1, txtName, "Enter a promotion name of 2 to 100 characters.");
                ok = false;
            }
            else
            {
                try
                {
                    if (PromotionRepository.PromoNameExists(name, EditId > 0 ? EditId : 0))
                    {
                        Validators.SetError(errorProvider1, txtName,
                            "A promotion with this name already exists.");
                        ok = false;
                    }
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return false;
                }
            }

            // 2. At least one service must be ticked, because a promotion is a set of services.
            if (_selectedServiceIds.Count == 0)
            {
                Validators.SetError(errorProvider1, dgvServicePick,
                    "Tick at least one service.");
                ok = false;
            }

            // 3. The promotional price must be above 0 and lower than what the services cost.
            decimal promoPrice;
            if (!Validators.TryMoney(txtPromoPrice.Text, out promoPrice) || promoPrice <= 0m)
            {
                Validators.SetError(errorProvider1, txtPromoPrice,
                    "Enter the promotional price of these services.");
                ok = false;
            }
            else if (_selectedServiceIds.Count > 0 && promoPrice >= RegularTotalOfSelected())
            {
                Validators.SetError(errorProvider1, txtPromoPrice,
                    "The promo price must be lower than the regular price of the services (" +
                    Money.Text(RegularTotalOfSelected()) + ").");
                ok = false;
            }

            // 4. The end date may not be before the start date.
            if (dtpEnd.Value.Date < dtpStart.Value.Date)
            {
                Validators.SetError(errorProvider1, dtpEnd, "The end date must be on or after the start date.");
                ok = false;
            }
            else if (EditId == 0 && dtpEnd.Value.Date < DateTime.Today)
            {
                // A brand-new promo that already ended would never show up, so it is refused.
                Validators.SetError(errorProvider1, dtpEnd, "A new promotion cannot end in the past.");
                ok = false;
            }

            return ok;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtServiceSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            ApplyServiceFilter();
        }

        private void cboServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            ApplyServiceFilter();
        }

        private void dgvServicePick_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // A check box needs the edit committed, otherwise CellValueChanged never fires and the
            // tick would only be counted when the user leaves the row.
            if (!dgvServicePick.IsCurrentCellDirty) return;

            DataGridViewCheckBoxCell checkCell = dgvServicePick.CurrentCell as DataGridViewCheckBoxCell;
            if (checkCell == null) return;

            dgvServicePick.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvServicePick_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || _loadingGrid) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvServicePick.Columns[e.ColumnIndex].Name != "colPickCheck") return;

            DataGridViewRow row = dgvServicePick.Rows[e.RowIndex];
            int serviceId = Db.Get<int>(row.Cells["colPickServiceId"].Value);
            if (serviceId <= 0) return;

            if (Db.Get<bool>(row.Cells["colPickCheck"].Value))
            {
                _selectedServiceIds.Add(serviceId);
            }
            else
            {
                _selectedServiceIds.Remove(serviceId);
            }

            errorProvider1.Clear();
            UpdateSelectedCount();
            UpdatePreview();
        }

        private void txtPromoPrice_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            // The price preview follows the typed price, so the owner sees the saving at once.
            errorProvider1.SetError(txtPromoPrice, string.Empty);
            UpdatePreview();
        }
    }
}
