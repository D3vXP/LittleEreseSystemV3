using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            Money.ApplyTo(colPrevPromo);
            Money.ApplyTo(colPrevSaving);
            Money.ApplyTo(colPickPrice);

            _loading = true;
            try
            {
                cboType.SelectedIndex = 0; // "Percentage" by default
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

                // A brand-new promotion starts as "applies to all services"; an existing one keeps
                // its saved links.
                chkApplyAll.Checked = _selectedServiceIds.Count == 0;
            }
            finally
            {
                _loading = false;
            }

            ApplyApplyAllState();
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

            string type = Db.Get<string>(row["discount_type"], "Percentage");
            cboType.SelectedItem = type;
            if (cboType.SelectedIndex < 0) cboType.SelectedIndex = 0;

            txtValue.Text = Db.Get<decimal>(row["discount_value"]).ToString("0.00");

            DateTime start = Db.Get<DateTime>(row["start_date"], DateTime.Today);
            DateTime end = Db.Get<DateTime>(row["end_date"], start);
            dtpStart.Value = start;
            dtpEnd.Value = end;

            string status = Db.Get<string>(row["status"], "Active");
            cboStatus.SelectedItem = status;
            if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// "Apply to all services" locks the picker, because a promotion without linked services
        /// applies to the whole bill.
        /// </summary>
        private void ApplyApplyAllState()
        {
            bool applyAll = chkApplyAll.Checked;

            txtServiceSearch.Enabled = !applyAll;
            cboServiceCategory.Enabled = !applyAll;
            dgvServicePick.Enabled = !applyAll;

            if (!applyAll) return;

            // Nothing is ticked while it applies to everything, so the count cannot lie.
            lblSelectedCount.Text = "Applies to every service";
            dgvServicePick.ClearSelection();
        }

        /// <summary>The "N services selected" line under the pick grid.</summary>
        private void UpdateSelectedCount()
        {
            if (chkApplyAll.Checked) return;

            int count = _selectedServiceIds.Count;
            lblSelectedCount.Text = count + (count == 1 ? " service selected" : " services selected");
        }

        /// <summary>
        /// Rebuilds the price preview and the three totals from the ticked services, the discount
        /// type and the value. Every calculation goes through PromotionRepository.ComputeDiscount,
        /// so the formula is never written a second time.
        /// </summary>
        private void UpdatePreview()
        {
            DataTable preview = new DataTable();
            preview.Columns.Add("service_name", typeof(string));
            preview.Columns.Add("regular_price", typeof(decimal));
            preview.Columns.Add("promo_price", typeof(object));
            preview.Columns.Add("saving", typeof(object));

            bool applyAll = chkApplyAll.Checked;
            string type = cboType.SelectedItem == null ? "Percentage" : cboType.SelectedItem.ToString();

            decimal value;
            if (!Validators.TryMoney(txtValue.Text, out value)) value = 0m;

            // The totals are meaningless while the promotion applies to the whole bill, because the
            // bill is not known in this dialog.
            if (applyAll)
            {
                dgvPreview.DataSource = preview;
                dgvPreview.ClearSelection();
                lblPreviewNote.Text = "Applies to every service on the bill.";
                ShowTotals(false);
                lblRegularTotal.Text = string.Empty;
                lblDropTotal.Text = string.Empty;
                lblPromoTotal.Text = string.Empty;
                return;
            }

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
                    line["promo_price"] = DBNull.Value;
                    line["saving"] = DBNull.Value;

                    if (string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase))
                    {
                        // One line of the appointment: the price of the service times the percentage.
                        decimal drop = PromotionRepository.ComputeDiscount(type, value, price);
                        line["promo_price"] = price - drop;
                        line["saving"] = drop;
                    }

                    preview.Rows.Add(line);
                }
            }

            decimal totalDrop = PromotionRepository.ComputeDiscount(type, value, regularTotal);

            // A fixed amount is taken off ONCE per appointment, so it only makes sense inside a
            // single service row when that one service IS the whole selection.
            if (!string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase) && shown == 1)
            {
                DataRow only = preview.Rows[0];
                decimal price = Db.Get<decimal>(only["regular_price"]);
                only["promo_price"] = price - totalDrop;
                only["saving"] = totalDrop;
            }

            dgvPreview.DataSource = preview;
            dgvPreview.ClearSelection();

            lblRegularTotal.Text = Money.Text(regularTotal);
            lblDropTotal.Text = Money.Text(totalDrop);
            lblPromoTotal.Text = Money.Text(regularTotal - totalDrop);

            ShowTotals(true);

            if (shown == 0)
            {
                lblPreviewNote.Text = "Tick at least one service to see the prices.";
            }
            else if (string.Equals(type, "Percentage", StringComparison.OrdinalIgnoreCase))
            {
                lblPreviewNote.Text = "Only the appointment lines of the selected services are discounted.";
            }
            else
            {
                lblPreviewNote.Text = "A fixed discount is taken off once per appointment.";
            }
        }

        /// <summary>Hides the totals block while the promotion applies to the whole bill.</summary>
        private void ShowTotals(bool visible)
        {
            lblRegularTitle.Visible = visible;
            lblRegularTotal.Visible = visible;
            lblDropTitle.Visible = visible;
            lblDropTotal.Visible = visible;
            lblPromoTitle.Visible = visible;
            lblPromoTotal.Visible = visible;
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
                string type = cboType.SelectedItem.ToString();

                decimal value;
                Validators.TryMoney(txtValue.Text, out value);

                DateTime start = dtpStart.Value.Date;
                DateTime end = dtpEnd.Value.Date;
                string status = cboStatus.SelectedItem.ToString();

                // "Applies to all services" is stored as NO links at all.
                List<int> serviceIds = chkApplyAll.Checked
                    ? new List<int>()
                    : new List<int>(_selectedServiceIds);

                // The promotion AND its links are written in one transaction inside the repository,
                // so a half saved promotion can never reach the database.
                SavedPromoId = PromotionRepository.SavePromotion(
                    EditId, name, conditions, type, value, start, end, status, serviceIds);

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

            // 2. A discount type must be chosen.
            if (cboType.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboType, "Choose a discount type.");
                ok = false;
            }

            // 3. The value must be money above zero, and at most 100 for a percentage.
            decimal value;
            if (!Validators.IsPositiveMoney(txtValue.Text) || !Validators.TryMoney(txtValue.Text, out value))
            {
                Validators.SetError(errorProvider1, txtValue, "Enter a value greater than 0.");
                ok = false;
            }
            else if (cboType.SelectedIndex >= 0
                && string.Equals(cboType.SelectedItem.ToString(), "Percentage", StringComparison.OrdinalIgnoreCase)
                && !Validators.IsValidPercentage(value))
            {
                Validators.SetError(errorProvider1, txtValue, "A percentage must not be more than 100.");
                ok = false;
            }
            else if (cboType.SelectedIndex >= 0
                && string.Equals(cboType.SelectedItem.ToString(), "Fixed", StringComparison.OrdinalIgnoreCase)
                && !chkApplyAll.Checked
                && _selectedServiceIds.Count > 0
                && value > RegularTotalOfSelected())
            {
                // A fixed amount can never be more than the services it is taken from.
                Validators.SetError(errorProvider1, txtValue,
                    "A fixed discount cannot be more than the regular total of the selected services (" +
                    Money.Text(RegularTotalOfSelected()) + ").");
                ok = false;
            }

            // 4. Either the promotion applies to every service, or at least one is ticked.
            if (!chkApplyAll.Checked && _selectedServiceIds.Count == 0)
            {
                Validators.SetError(errorProvider1, chkApplyAll,
                    "Tick at least one service, or apply the promotion to all services.");
                ok = false;
            }

            // 5. The end date may not be before the start date.
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

        private void chkApplyAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            errorProvider1.Clear();
            ApplyApplyAllState();
            UpdateSelectedCount();
            UpdatePreview();
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

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            UpdatePreview();
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            UpdatePreview();
        }
    }
}
