using System;
using System.Collections.Generic;
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

        /// <summary>
        /// True when "Use existing client" was chosen for a duplicate name: nothing was inserted,
        /// and SavedClientId holds the client already on file. The callers treat that id the same way.
        /// </summary>
        public bool UsedExisting { get; private set; }

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

            // The tick list comes from the ONE list in ClientRepository, so a new condition
            // is added there and appears here without touching this form.
            clbHealth.Items.Clear();
            clbHealth.Items.AddRange(ClientRepository.HealthConditionOptions);

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
            txtMiddleName.Text = Db.Get<string>(row["middle_name"]);
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
            TickHealthConditions(Db.Get<string>(row["health_conditions"]));
            txtMedicalHistory.Text = Db.Get<string>(row["medical_history"]);
            txtTreatmentHistory.Text = Db.Get<string>(row["treatment_history"]);
            chkConsentSigned.Checked = Db.Get<bool>(row["consent_status"]);
        }

        /// <summary>Ticks the boxes this client's stored health_conditions already holds.</summary>
        private void TickHealthConditions(string stored)
        {
            foreach (string condition in ClientRepository.SplitHealthConditions(stored))
            {
                int index = clbHealth.Items.IndexOf(condition);
                // An old row may hold a wording the list no longer has; a missing tick is
                // better than adding a value the list does not know about.
                if (index >= 0) clbHealth.SetItemChecked(index, true);
            }
        }

        /// <summary>The ticked items as the comma separated text the Clients column stores.</summary>
        private string CheckedHealthConditions()
        {
            List<string> ticked = new List<string>();
            foreach (object item in clbHealth.CheckedItems)
            {
                ticked.Add(item.ToString());
            }

            return ClientRepository.JoinHealthConditions(ticked);
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
            string middleName = txtMiddleName.Text.Trim();
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
                    ClientRepository.Update(EditId, firstName, middleName, lastName, birthDate, gender, contact,
                        txtAddress.Text, txtAllergies.Text, txtMedicalHistory.Text,
                        CheckedHealthConditions(), txtTreatmentHistory.Text, chkConsentSigned.Checked);
                    SavedClientId = EditId;

                    ActivityRepository.Log(Session.UserId, "Edit", "Clients",
                        lastName + ", " + firstName);
                }
                else
                {
                    // A new guest whose first and last name is already in the book may be the SAME
                    // person, so the two are shown side by side before a second record is written.
                    if (!ConfirmSameName(firstName, middleName, lastName, birthDate, gender, contact)) return;

                    // "Use existing client" means the record already on file is the one wanted.
                    if (UsedExisting)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }

                    SavedClientId = ClientRepository.Add(firstName, middleName, lastName, birthDate, gender, contact,
                        txtAddress.Text, txtAllergies.Text, txtMedicalHistory.Text,
                        CheckedHealthConditions(), txtTreatmentHistory.Text, chkConsentSigned.Checked);

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

        /// <summary>
        /// Looks for clients that already carry the typed first and last name and, when there are
        /// any, shows them beside the new client. Returns false when the user chose "Go back", so
        /// the edit form stays open with the typed data and nothing is saved.
        /// On "Add anyway" the method returns true and the caller inserts as usual; on
        /// "Use existing client" it sets UsedExisting and SavedClientId and the caller stops.
        /// </summary>
        private bool ConfirmSameName(string firstName, string middleName, string lastName,
            DateTime? birthDate, string gender, string contact)
        {
            DataTable matches;
            try
            {
                matches = ClientRepository.FindSameName(firstName, middleName, lastName, EditId);
            }
            catch (Exception ex)
            {
                // The warning is a help, not a rule, so a failed look-up must never block a save.
                Messages.DbError(ex);
                return true;
            }

            if (matches == null || matches.Rows.Count == 0) return true;

            using (DuplicateClientForm form = new DuplicateClientForm())
            {
                form.NewClient = NewClientRow(firstName, middleName, lastName, birthDate, gender, contact);
                form.Candidates = matches;

                form.ShowDialog(this);

                // The window closed without a decision (the X) means "do not save": the user must
                // make the choice on purpose, so the edit form stays open with the typed data.
                if (form.DialogResult != DialogResult.OK) return false;

                if (form.ChosenAction == DuplicateClientForm.DuplicateAction.AddAnyway) return true;

                if (form.ChosenAction == DuplicateClientForm.DuplicateAction.UseExisting)
                {
                    UsedExisting = true;
                    SavedClientId = form.ExistingClientId;

                    ActivityRepository.Log(Session.UserId, "Use existing", "Clients",
                        lastName + ", " + firstName + " matched client_id " + form.ExistingClientId);

                    return true;
                }
            }

            // Go back (or the window was closed): the user keeps working in the edit form.
            return false;
        }

        /// <summary>
        /// The typed data in the shape ClientRepository.FindSameName returns, so the two cards of
        /// the duplicate window are filled by the same code no matter where the row comes from.
        /// </summary>
        private DataRow NewClientRow(string firstName, string middleName, string lastName,
            DateTime? birthDate, string gender, string contact)
        {
            DataTable table = new DataTable();
            table.Columns.Add("client_id", typeof(int));
            table.Columns.Add("full_name", typeof(string));
            table.Columns.Add("birth_date", typeof(DateTime));
            table.Columns.Add("gender", typeof(string));
            table.Columns.Add("contact_number", typeof(string));
            table.Columns.Add("address", typeof(string));

            DataRow row = table.NewRow();
            row["client_id"] = 0;
            row["full_name"] = lastName + ", " + firstName +
                (string.IsNullOrWhiteSpace(middleName) ? string.Empty : " " + middleName.Trim());
            row["birth_date"] = birthDate.HasValue ? (object)birthDate.Value.Date : DBNull.Value;
            row["gender"] = gender;
            row["contact_number"] = contact;
            row["address"] = txtAddress.Text;
            table.Rows.Add(row);

            return row;
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

            // The middle name is OPTIONAL (two clients may share a first and last name), so an
            // empty box is fine, but a filled box must still be a real name.
            if (!Validators.IsOptionalName(txtMiddleName.Text))
            {
                Validators.SetError(errorProvider1, txtMiddleName,
                    "The middle name may only have letters, spaces, hyphens, dots and apostrophes.");
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
