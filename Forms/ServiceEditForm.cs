using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class ServiceEditForm : Form
    {
        /// <summary>One entry of the category combo box.</summary>
        private sealed class CategoryItem
        {
            public int Id;
            public string Name;

            public CategoryItem(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>0 = adding a new service. The caller sets this before ShowDialog().</summary>
        public int EditId { get; set; }

        /// <summary>The service that was saved (the new id after an Add), so the grid can select it.</summary>
        public int SavedServiceId { get; private set; }

        public ServiceEditForm()
        {
            InitializeComponent();
        }

        private void ServiceEditForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            FillCategories();
            cboStatus.SelectedIndex = 0; // "Active" for a new service
            nudInterval.Enabled = false;  // only a package (more than 1 session) has an interval
            chkAssessment.Checked = false; // a new service needs no nurse assessment until the owner says so

            if (EditId > 0) LoadService(EditId);
        }

        /// <summary>Fills the category combo box from the database.</summary>
        private void FillCategories()
        {
            cboCategory.Items.Clear();

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

        /// <summary>Puts an existing service into the fields, for editing.</summary>
        private void LoadService(int serviceId)
        {
            DataRow row;
            try
            {
                row = ServiceRepository.GetById(serviceId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                Messages.Warn("This service no longer exists.");
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            Text = "Edit Service";
            lblTitle.Text = "Edit Service";

            SelectCategory(Db.Get<int>(row["category_id"]));
            txtName.Text = Db.Get<string>(row["service_name"]);
            txtDescription.Text = Db.Get<string>(row["description"]);
            txtPrice.Text = Db.Get<decimal>(row["price"]).ToString("0.00");
            nudDuration.Value = ClampNumeric(nudDuration, Db.Get<int>(row["duration_minutes"]));
            nudSessions.Value = ClampNumeric(nudSessions, Db.Get<int>(row["default_total_sessions"]));
            nudBusy.Value = ClampNumeric(nudBusy, Db.Get<int>(row["staff_busy_minutes"], 0));
            nudInterval.Value = ClampNumeric(nudInterval, Db.Get<int>(row["session_interval_days"], 7));
            UpdateIntervalState();
            // requires_assessment is a BIT NOT NULL DEFAULT 0, so a missing value means "no".
            chkAssessment.Checked = Db.Get<bool>(row["requires_assessment"], false);

            string status = Db.Get<string>(row["status"], "Active");
            cboStatus.SelectedItem = status;
            if (cboStatus.SelectedIndex < 0) cboStatus.SelectedIndex = 0;
        }

        private void SelectCategory(int categoryId)
        {
            for (int i = 0; i < cboCategory.Items.Count; i++)
            {
                CategoryItem item = cboCategory.Items[i] as CategoryItem;
                if (item != null && item.Id == categoryId)
                {
                    cboCategory.SelectedIndex = i;
                    return;
                }
            }
        }

        /// <summary>Keeps a value inside the limits of a NumericUpDown instead of throwing.</summary>
        private static decimal ClampNumeric(NumericUpDown box, int value)
        {
            decimal number = value;
            if (number < box.Minimum) number = box.Minimum;
            if (number > box.Maximum) number = box.Maximum;
            return number;
        }

        /// <summary>
        /// The days between sessions only mean something for a package, so the box follows the
        /// sessions count: 1 session (a single visit) disables it.
        /// </summary>
        private void UpdateIntervalState()
        {
            nudInterval.Enabled = Convert.ToInt32(nudSessions.Value) > 1;
        }

        private void nudSessions_ValueChanged(object sender, EventArgs e)
        {
            UpdateIntervalState();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidateFields()) return;

            try
            {
                CategoryItem category = cboCategory.SelectedItem as CategoryItem;
                string name = txtName.Text.Trim();
                string description = txtDescription.Text.Trim();

                decimal price;
                Validators.TryMoney(txtPrice.Text, out price);

                int duration = Convert.ToInt32(nudDuration.Value);
                int sessions = Convert.ToInt32(nudSessions.Value);
                int busy = Convert.ToInt32(nudBusy.Value);
                int interval = Convert.ToInt32(nudInterval.Value);
                bool assessment = chkAssessment.Checked;
                string status = cboStatus.SelectedItem.ToString();

                if (EditId > 0)
                {
                    ServiceRepository.Update(EditId, category.Id, name, description, price,
                        duration, sessions, status, busy, interval, assessment);
                    SavedServiceId = EditId;

                    ActivityRepository.Log(Session.UserId, "Edit", "Services", name);
                }
                else
                {
                    SavedServiceId = ServiceRepository.Add(category.Id, name, description, price,
                        duration, sessions, status, busy, interval, assessment);

                    ActivityRepository.Log(Session.UserId, "Add", "Services", name);
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

            // 1. A category must be picked (the list is filled in Load).
            if (cboCategory.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboCategory, "Choose a category.");
                ok = false;
            }

            // 2. The name must be a real name, unique inside its category.
            string name = txtName.Text.Trim();
            if (!Validators.IsLengthBetween(name, 2, 100))
            {
                Validators.SetError(errorProvider1, txtName, "Enter a service name of 2 to 100 characters.");
                ok = false;
            }
            else
            {
                CategoryItem category = cboCategory.SelectedItem as CategoryItem;
                if (category != null)
                {
                    try
                    {
                        if (ServiceRepository.ServiceNameExists(name, category.Id, EditId > 0 ? EditId : 0))
                        {
                            Validators.SetError(errorProvider1, txtName,
                                "A service with this name already exists in this category.");
                            ok = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Messages.DbError(ex);
                        return false;
                    }
                }
            }

            // 3. The price must be money above zero.
            if (!Validators.IsPositiveMoney(txtPrice.Text))
            {
                Validators.SetError(errorProvider1, txtPrice, "Enter a price greater than 0 (for example 1500.00).");
                ok = false;
            }

            // 4. The duration must sit on the 30-minute grid the booking screen uses.
            //    The box only offers 30..240 in steps of 30, but the rule is checked anyway so a
            //    stored value that came from elsewhere cannot slip through unnoticed.
            int duration = Convert.ToInt32(nudDuration.Value);
            if (!Validators.IsValidDuration(duration))
            {
                Validators.SetError(errorProvider1, nudDuration, Validators.DurationStepMessage);
                ok = false;
            }

            // 5. Staff busy is optional (0 = empty); when given it must fit inside the service.
            int busy = Convert.ToInt32(nudBusy.Value);
            if (!Validators.IsValidStaffBusy(busy, duration))
            {
                Validators.SetError(errorProvider1, nudBusy,
                    "The staff busy time must be 5 minutes at least and not longer than the service.");
                ok = false;
            }

            // 6. A package has 1 to 30 sessions.
            if (!Validators.IsValidSessions(Convert.ToInt32(nudSessions.Value)))
            {
                Validators.SetError(errorProvider1, nudSessions, "Enter 1 to 30 sessions.");
                ok = false;
            }

            return ok;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
