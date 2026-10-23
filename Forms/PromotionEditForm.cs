using System;
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

        public PromotionEditForm()
        {
            InitializeComponent();
        }

        private void PromotionEditForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            cboType.SelectedIndex = 0; // "Percentage" by default
            cboStatus.SelectedIndex = 0; // "Active"

            // A new promo starts today and runs for a month; the owner can change both.
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today.AddMonths(1);

            if (EditId > 0) LoadPromotion(EditId);
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

                if (EditId > 0)
                {
                    PromotionRepository.Update(EditId, name, conditions, type, value, start, end, status);
                    SavedPromoId = EditId;

                    ActivityRepository.Log(Session.UserId, "Edit", "Promotions", name);
                }
                else
                {
                    SavedPromoId = PromotionRepository.Add(name, conditions, type, value, start, end, status);

                    ActivityRepository.Log(Session.UserId, "Add", "Promotions", name);
                }
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

        private void chkApplyAll_CheckedChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void txtServiceSearch_TextChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void cboServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void dgvServicePick_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void dgvServicePick_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // TODO(logic prompt)
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }
    }
}
