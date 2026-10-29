using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class ClientEditForm : Form
    {
        // The oldest a client may be. Anything above this is a typo in the birth date.
        private const int MaximumAge = 120;

        /// <summary>0 = adding a new client. The caller sets this before ShowDialog().</summary>
        public int EditId { get; set; }

        /// <summary>The client that was saved (the new id after an Add), so the grid can select it.</summary>
        public int SavedClientId { get; private set; }

        public ClientEditForm()
        {
            InitializeComponent();
        }

        private void ClientEditForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            // Gender has no sensible default, so nothing is picked until the user picks it.
            cboGender.SelectedIndex = -1;

            // A new client starts with an empty (unchecked) birth date, and Save refuses it,
            // because the shop must be able to check the minimum age.
            // The value is set BEFORE the tick is cleared, because assigning Value on a control
            // that is not checked would otherwise leave the box looking filled in.
            dtpBirthDate.Value = DateTime.Today;
            dtpBirthDate.Checked = false;

            if (EditId > 0) LoadClient(EditId);

            UpdateAge();
        }

        /// <summary>Puts an existing client into the fields, for editing.</summary>
        private void LoadClient(int clientId)
        {
            DataRow row;
            try
            {
                row = ClientRepository.GetById(clientId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (row == null)
            {
                Messages.Warn("This client no longer exists.");
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            Text = "Edit Client";
            lblTitle.Text = "Edit Client";

            txtFirstName.Text = Db.Get<string>(row["first_name"]);
            txtLastName.Text = Db.Get<string>(row["last_name"]);

            DateTime? birth = Db.Get<DateTime?>(row["birth_date"]);
            if (birth.HasValue)
            {
                dtpBirthDate.Value = birth.Value;
                dtpBirthDate.Checked = true;
            }
            else
            {
                dtpBirthDate.Checked = false;
            }

            string gender = Db.Get<string>(row["gender"]);
            cboGender.SelectedItem = gender;
            if (cboGender.SelectedIndex < 0 && !string.IsNullOrEmpty(gender))
            {
                // An old row may hold a word the list no longer has; showing it is better than losing it.
                cboGender.Items.Add(gender);
                cboGender.SelectedItem = gender;
            }

            txtContact.Text = Db.Get<string>(row["contact_number"]);
            txtAddress.Text = Db.Get<string>(row["address"]);
            txtAllergies.Text = Db.Get<string>(row["allergies"]);
            txtMedicalHistory.Text = Db.Get<string>(row["medical_history"]);
            txtTreatmentHistory.Text = Db.Get<string>(row["treatment_history"]);
            chkConsentSigned.Checked = Db.Get<bool>(row["consent_status"]);
        }

        /// <summary>
        /// Keeps the "Age: ..." line beside the birth date in step with the picker.
        /// An unchecked picker means the birth date is unknown.
        /// </summary>
        private void dtpBirthDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateAge();
        }

        private void UpdateAge()
        {
            if (!dtpBirthDate.Checked)
            {
                lblAge.Text = "Age: -";
                return;
            }

            DateTime today = DateTime.Today;
            DateTime birth = dtpBirthDate.Value.Date;

            if (birth > today)
            {
                lblAge.Text = "Age: -";
                return;
            }

            // The birthday counting lives in Validators, so the label and the age rule agree.
            lblAge.Text = "Age: " + Validators.AgeOn(birth, today);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidateFields()) return;

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string contact = txtContact.Text.Trim();
            string gender = cboGender.SelectedItem == null ? string.Empty : cboGender.SelectedItem.ToString();

            DateTime? birthDate = dtpBirthDate.Checked ? (DateTime?)dtpBirthDate.Value.Date : null;

            // Two family members may share one number, so a duplicate is a question, not a block.
            try
            {
                if (ClientRepository.ContactExists(contact, EditId > 0 ? EditId : 0))
                {
                    if (!Messages.Confirm("A client with this contact number already exists. Save anyway?"))
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            try
            {
                if (EditId > 0)
                {
                    ClientRepository.Update(EditId, firstName, lastName, birthDate, gender, contact,
                        txtAddress.Text, txtAllergies.Text, txtMedicalHistory.Text,
                        string.Empty, txtTreatmentHistory.Text, chkConsentSigned.Checked);
                    SavedClientId = EditId;

                    ActivityRepository.Log(Session.UserId, "Edit", "Clients",
                        lastName + ", " + firstName);
                }
                else
                {
                    SavedClientId = ClientRepository.Add(firstName, lastName, birthDate, gender, contact,
                        txtAddress.Text, txtAllergies.Text, txtMedicalHistory.Text,
                        string.Empty, txtTreatmentHistory.Text, chkConsentSigned.Checked);

                    ActivityRepository.Log(Session.UserId, "Add", "Clients",
                        lastName + ", " + firstName);
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

            // 1. First and last name must be real names (letters, spaces, dots, apostrophes, hyphens).
            if (!Validators.IsValidName(txtFirstName.Text))
            {
                Validators.SetError(errorProvider1, txtFirstName, "Enter the client's first name.");
                ok = false;
            }

            if (!Validators.IsValidName(txtLastName.Text))
            {
                Validators.SetError(errorProvider1, txtLastName, "Enter the client's last name.");
                ok = false;
            }

            // 2. The contact number is the shop's only way to reach a guest, so it is required.
            if (!Validators.IsValidPhone(txtContact.Text))
            {
                Validators.SetError(errorProvider1, txtContact, "Enter a mobile number like 09171234567.");
                ok = false;
            }

            // 3. The birth date is REQUIRED, because the shop cannot check the minimum age without it.
            if (!dtpBirthDate.Checked)
            {
                Validators.SetError(errorProvider1, dtpBirthDate, "Birth date is required.");
                ok = false;
            }
            else
            {
                DateTime birth = dtpBirthDate.Value.Date;
                if (birth > DateTime.Today)
                {
                    Validators.SetError(errorProvider1, dtpBirthDate, "The birth date cannot be in the future.");
                    ok = false;
                }
                else if (birth < DateTime.Today.AddYears(-MaximumAge))
                {
                    Validators.SetError(errorProvider1, dtpBirthDate,
                        "Please check the birth date: nobody is older than " + MaximumAge + ".");
                    ok = false;
                }
                // 4. The minimum age is a business rule, so the SAME check runs here and in
                //    ClientRepository. The repository message is shown here, so the wording
                //    can never drift apart between the two places.
                else if (!Validators.IsAtLeastAge(birth, Validators.MinimumClientAge, DateTime.Today))
                {
                    Validators.SetError(errorProvider1, dtpBirthDate, Validators.MinimumClientAgeMessage);
                    ok = false;
                }
            }

            // 5. Gender must be chosen (it is used for the treatment notes).
            if (cboGender.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboGender, "Choose the client's gender.");
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
