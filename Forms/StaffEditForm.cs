using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class StaffEditForm : Form
    {
        /// <summary>
        /// 0 = adding a new account. Any other value is the account being edited.
        /// The caller sets this before ShowDialog().
        /// </summary>
        public int EditId { get; set; }

        /// <summary>
        /// True when the Admin only wants to set a new password for an existing account.
        /// Only the two password boxes stay usable and the title becomes "Reset Password".
        /// </summary>
        public bool PasswordOnly { get; set; }

        /// <summary>The account that was saved (the new id after an Add), so the grid can select it.</summary>
        public int SavedUserId { get; private set; }

        public StaffEditForm()
        {
            InitializeComponent();
        }

        private void StaffEditForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Style();

            cboStatus.SelectedIndex = 0; // "Active" for a new account
            cboRole.SelectedIndex = 1;   // "Staff" is the normal choice for a new account

            if (EditId > 0) LoadUser(EditId);

            if (PasswordOnly) ApplyPasswordOnlyMode();
            else ApplyRoleToStaffType();
        }

        /// <summary>Applies the shared Theme styles (the Designer keeps the same colors as literals).</summary>
        private void Style()
        {
            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);
        }

        /// <summary>Fills the fields of an existing account.</summary>
        private void LoadUser(int userId)
        {
            DataRow row;
            try
            {
                row = UserRepository.GetById(userId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                Messages.Warn("This account no longer exists.");
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            Text = "Edit Staff Account";
            lblTitle.Text = "Edit Staff Account";

            txtFullName.Text = Db.Get<string>(row["full_name"]);
            txtUsername.Text = Db.Get<string>(row["username"]);
            txtContact.Text = Db.Get<string>(row["contact_number"]);

            // The username identifies the account and is never changed, so it is read-only when editing.
            txtUsername.ReadOnly = true;
            txtUsername.TabStop = false;

            SelectComboText(cboRole, Db.Get<string>(row["role"], "Staff"));
            SelectComboText(cboStatus, Db.Get<string>(row["status"], "Active"));
            SelectStaffType(Db.Get<string>(row["staff_type"]));

            // Editing: leaving both password boxes empty keeps the current password.
            lblPassword.Text = "New password (optional)";
            lblConfirm.Text = "Confirm new password";
        }

        /// <summary>Selects a combo box entry by its text, without throwing when it is missing.</summary>
        private static void SelectComboText(ComboBox box, string text)
        {
            box.SelectedItem = text;
            if (box.SelectedIndex < 0) box.SelectedIndex = 0;
        }

        /// <summary>Selects the stored job type; an account without one starts on the first entry.</summary>
        private void SelectStaffType(string staffType)
        {
            cboStaffType.SelectedItem = staffType;
            if (cboStaffType.SelectedIndex < 0) cboStaffType.SelectedIndex = 0;
        }

        /// <summary>
        /// An Admin has no job type, so the combo is disabled and empty for that role; a Staff
        /// account must pick one (Validators.StaffTypeRequiredMessage says why).
        /// </summary>
        private void ApplyRoleToStaffType()
        {
            bool isAdmin = string.Equals(Convert.ToString(cboRole.SelectedItem), "Admin",
                StringComparison.OrdinalIgnoreCase);

            cboStaffType.Enabled = !isAdmin;
            lblStaffType.Enabled = !isAdmin;

            if (isAdmin) cboStaffType.SelectedIndex = -1;
            else if (cboStaffType.SelectedIndex < 0) cboStaffType.SelectedIndex = 0;
        }

        private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyRoleToStaffType();
        }

        /// <summary>
        /// Reset Password mode: everything except the two password boxes is locked, so a password
        /// reset can never change a name, a role or a status by accident.
        /// </summary>
        private void ApplyPasswordOnlyMode()
        {
            Text = "Reset Password";
            lblTitle.Text = "Reset Password";

            cboStatus.Enabled = false;
            txtFullName.ReadOnly = true;
            txtUsername.ReadOnly = true;
            cboRole.Enabled = false;
            cboStaffType.Enabled = false;
            txtContact.ReadOnly = true;
            lblStatus.Enabled = false;
            lblFullName.Enabled = false;
            lblUsername.Enabled = false;
            lblRole.Enabled = false;
            lblStaffType.Enabled = false;
            lblContact.Enabled = false;

            // In this mode a password is not optional any more, so the labels say what is expected.
            lblPassword.Text = "New password";
            lblConfirm.Text = "Confirm new password";
            txtPassword.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (PasswordOnly)
            {
                SavePasswordOnly();
                return;
            }

            if (!ValidateFields()) return;

            try
            {
                string fullName = txtFullName.Text.Trim();
                string contact = txtContact.Text.Trim();
                string role = cboRole.SelectedItem.ToString();
                string status = cboStatus.SelectedItem.ToString();
                string staffType = SelectedStaffType();

                if (EditId > 0)
                {
                    UserRepository.Update(EditId, fullName, role, contact, status, staffType);
                    SavedUserId = EditId;

                    // A blank password box means "keep the current password".
                    if (Validators.IsRequired(txtPassword.Text))
                    {
                        UserRepository.SetPassword(EditId, txtPassword.Text);
                    }

                    ActivityRepository.Log(Session.UserId, "Edit", "Staff Accounts", fullName);
                }
                else
                {
                    SavedUserId = UserRepository.Create(fullName, txtUsername.Text.Trim(),
                        txtPassword.Text, role, contact, staffType);

                    ActivityRepository.Log(Session.UserId, "Add", "Staff Accounts", fullName);
                }
            }
            catch (InvalidOperationException ex)
            {
                // Business rule (own account / last active Admin): friendly message, not a database error.
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

        /// <summary>Reset Password mode: check the two boxes and store the new password.</summary>
        private void SavePasswordOnly()
        {
            if (!ValidatePasswords(true)) return;

            try
            {
                UserRepository.SetPassword(EditId, txtPassword.Text);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            // The password itself is never written to the audit trail.
            ActivityRepository.Log(Session.UserId, "Reset Password", "Staff Accounts",
                "Password reset for " + txtFullName.Text.Trim());

            SavedUserId = EditId;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>Shows the red error icons and returns false when something is still wrong.</summary>
        private bool ValidateFields()
        {
            bool ok = true;

            // 1. The full name must look like a name.
            string fullName = txtFullName.Text.Trim();
            if (!Validators.IsValidName(fullName))
            {
                Validators.SetError(errorProvider1, txtFullName, "Enter a valid full name.");
                ok = false;
            }

            // 2. A new account needs a username in the agreed shape and it must still be free.
            if (EditId <= 0)
            {
                string username = txtUsername.Text.Trim();
                if (!Validators.IsValidUsername(username))
                {
                    Validators.SetError(errorProvider1, txtUsername,
                        "Use 3 to 20 letters, digits or underscores (no spaces).");
                    ok = false;
                }
                else
                {
                    try
                    {
                        if (UserRepository.UsernameExists(username))
                        {
                            Validators.SetError(errorProvider1, txtUsername, "This username is already taken.");
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

            // 3. A role must be picked (the list is filled in Load).
            if (cboRole.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboRole, "Choose a role.");
                ok = false;
            }

            // 4. A Staff account needs a job type; an Admin has none (the box is empty and disabled).
            if (cboStaffType.Enabled && cboStaffType.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboStaffType, Validators.StaffTypeRequiredMessage);
                ok = false;
            }

            // 5. The contact number must be a Philippine mobile number.
            // POLISH: relax this if the shop also allows landline numbers.
            if (!Validators.IsValidPhone(txtContact.Text))
            {
                Validators.SetError(errorProvider1, txtContact, "Enter a contact number like 09171234567.");
                ok = false;
            }

            // 6. The password: required for a new account, optional when editing.
            if (!ValidatePasswords(EditId <= 0)) ok = false;

            return ok;
        }

        /// <summary>
        /// The job type to save: the picked one for a Staff account, null for an Admin
        /// (UserRepository keeps the column NULL for an Admin regardless).
        /// </summary>
        private string SelectedStaffType()
        {
            if (!cboStaffType.Enabled || cboStaffType.SelectedIndex < 0) return null;
            return cboStaffType.SelectedItem.ToString();
        }

        /// <summary>
        /// Checks the password and its confirmation. When required is false an empty password
        /// means "keep the current one" and is accepted.
        /// </summary>
        private bool ValidatePasswords(bool required)
        {
            bool ok = true;
            string password = txtPassword.Text;

            if (!Validators.IsRequired(password))
            {
                if (required)
                {
                    Validators.SetError(errorProvider1, txtPassword, "Enter a password.");
                    ok = false;
                }
            }
            else if (!Validators.IsValidPassword(password))
            {
                Validators.SetError(errorProvider1, txtPassword,
                    "At least 8 characters with a letter and a number.");
                ok = false;
            }

            // The second box must match, whether the password is required or optional.
            if (!string.Equals(password, txtConfirmPassword.Text, StringComparison.Ordinal))
            {
                Validators.SetError(errorProvider1, txtConfirmPassword, "The two passwords do not match.");
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
