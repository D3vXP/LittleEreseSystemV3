using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucClients : UserControl
    {
        // True while the screen fills its own controls, so the selection and filter events
        // do not reload the grid or the details card twice.
        private bool _loading;

        public ucClients()
        {
            InitializeComponent();
        }

        private void ucClients_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StyleGrid(dgvClients);
            Theme.StyleGrid(dgvPackages);
            Theme.StylePrimaryButton(btnAdd);
            Theme.StyleSecondaryButton(btnEdit);
            Theme.StyleSecondaryButton(btnRefresh);
            Theme.StyleSecondaryButton(btnAddPackage);

            // Both roles may add and edit clients (the counter staff do this every day),
            // so no button is hidden here. There is no delete: client records are kept forever.

            _loading = true;
            try
            {
                cboConsent.SelectedIndex = 0; // "All"
            }
            finally
            {
                _loading = false;
            }

            // A package that ran out of time must never look usable, so this runs on every visit.
            ExpirePackages();

            LoadClients(0);
        }

        /// <summary>
        /// Marks the packages whose expiry date has passed as 'Expired'.
        /// WHY before loading: the grid and the details card then show the true status right away.
        /// </summary>
        private void ExpirePackages()
        {
            try
            {
                ClientRepository.ExpireOldPackages();
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>
        /// Reloads the client grid. keepClientId is selected again afterwards (0 = nothing),
        /// so the row the user just added or edited stays highlighted.
        /// </summary>
        private void LoadClients(int keepClientId)
        {
            string consent = cboConsent.SelectedItem == null ? "All" : cboConsent.SelectedItem.ToString();

            // _loading stops SelectionChanged from filling the card twice while the grid rebinds.
            _loading = true;
            try
            {
                try
                {
                    dgvClients.DataSource = ClientRepository.Search(txtSearch.Text, consent);
                }
                catch (Exception ex)
                {
                    Messages.DbError(ex);
                    return;
                }

                // The helper puts the current cell on a VISIBLE column (never the hidden id column).
                dgvClients.ClearSelection();
                GridHelper.SelectRowById(dgvClients, "colClientId", keepClientId);
            }
            finally
            {
                _loading = false;
            }

            UpdateCount();

            // No selection (or a filtered-out row) means the card must not show a stale client.
            int clientId = CurrentClientId();
            if (clientId > 0) ShowDetails(clientId);
            else ClearDetails();
        }

        private void UpdateCount()
        {
            int count = dgvClients.Rows.Count;
            lblCount.Text = count + (count == 1 ? " client" : " clients");
        }

        private int CurrentClientId()
        {
            return GridHelper.GetSelectedId(dgvClients, "colClientId");
        }

        // ---------------------------------------------------------------- details card

        /// <summary>Fills the right-hand card with the selected client. Id 0 clears it.</summary>
        private void ShowDetails(int clientId)
        {
            if (clientId <= 0)
            {
                ClearDetails();
                return;
            }

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
                // The row is gone (filtered out or removed elsewhere), so nothing is shown for it.
                ClearDetails();
                return;
            }

            // "Last, First Middle" - the same wording the grid shows, so the two never disagree.
            string middleName = Db.Get<string>(row["middle_name"]);
            lblDetailName.Text = Db.Get<string>(row["last_name"]) + ", " + Db.Get<string>(row["first_name"]) +
                (string.IsNullOrWhiteSpace(middleName) ? string.Empty : " " + middleName);

            lblDetailContact.Text = "Contact number: " + ValueOrDash(Db.Get<string>(row["contact_number"]));

            DateTime? birth = Db.Get<DateTime?>(row["birth_date"]);
            lblDetailBirth.Text = birth.HasValue
                ? "Birth date: " + birth.Value.ToString("MMM dd, yyyy") + "   -   Age: " + AgeText(birth.Value)
                : "Birth date: -";

            string gender = Db.Get<string>(row["gender"]);
            string address = ValueOrDash(Db.Get<string>(row["address"]));
            lblDetailAddress.Text = "Address: " + address +
                (string.IsNullOrWhiteSpace(gender) ? string.Empty : "   -   " + gender);

            // Green badge when the signed paper form is filed, amber while it is still missing.
            // Red is reserved for real errors, so a missing form never paints the card red.
            bool signed = Db.Get<bool>(row["consent_status"]);
            lblDetailConsent.Text = signed ? "Consent signed" : "Consent form not signed";
            lblDetailConsent.BackColor = signed ? Theme.Success : Theme.Warning;
            lblDetailConsent.ForeColor = signed ? Color.White : Theme.TextDark;

            txtDetailAllergies.Text = Notes(Db.Get<string>(row["allergies"]));

            // The health box follows the paper form: the ticked checkboxes first, then the
            // "other conditions" free text under them on its own line.
            string healthText = ClientRepository.JoinHealthConditions(
                ClientRepository.SplitHealthConditions(Db.Get<string>(row["health_conditions"])));
            string otherText = Db.Get<string>(row["medical_history"]);
            if (healthText.Length == 0 && string.IsNullOrWhiteSpace(otherText))
            {
                txtDetailMedical.Text = Notes(string.Empty);
            }
            else
            {
                string otherLine = string.IsNullOrWhiteSpace(otherText)
                    ? string.Empty
                    : "Other: " + otherText.Trim();
                if (healthText.Length == 0) txtDetailMedical.Text = otherLine;
                else if (otherLine.Length == 0) txtDetailMedical.Text = healthText;
                else txtDetailMedical.Text = healthText + Environment.NewLine + otherLine;
            }

            txtDetailTreatment.Text = Notes(Db.Get<string>(row["treatment_history"]));

            txtDetailClinicHistory.Text = ClinicHistoryText(clientId);

            LoadPackages(clientId);

            btnEdit.Enabled = true;
            btnAddPackage.Enabled = true;
        }

        /// <summary>Empties the card and switches off the actions that need a client.</summary>
        private void ClearDetails()
        {
            lblDetailName.Text = "Select a client";
            lblDetailContact.Text = "Contact number: -";
            lblDetailBirth.Text = "Birth date: -";
            lblDetailAddress.Text = "Address: -";

            lblDetailConsent.Text = "No client selected";
            lblDetailConsent.BackColor = Theme.Surface;
            lblDetailConsent.ForeColor = Theme.TextMuted;

            txtDetailAllergies.Text = string.Empty;
            txtDetailMedical.Text = string.Empty;
            txtDetailTreatment.Text = string.Empty;
            txtDetailClinicHistory.Text = string.Empty;

            dgvPackages.DataSource = null;

            btnEdit.Enabled = false;
            btnAddPackage.Enabled = false;
        }

        /// <summary>The shop's own treatment history, one "date - service - staff" line per visit.</summary>
        private static string ClinicHistoryText(int clientId)
        {
            DataTable history;
            try
            {
                history = ClientRepository.GetClinicHistory(clientId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return Notes(string.Empty);
            }

            if (history == null || history.Rows.Count == 0) return Notes(string.Empty);

            StringBuilder text = new StringBuilder();
            foreach (DataRow visit in history.Rows)
            {
                if (text.Length > 0) text.AppendLine();

                DateTime visitDate = Db.Get<DateTime>(visit["visit_date"]);
                text.Append(visitDate.ToString("MMM dd, yyyy"));
                text.Append(" - ");
                text.Append(Db.Get<string>(visit["service_name"]));

                // A package line carries "Session 2 of 4"; a plain service has no session text.
                string session = Db.Get<string>(visit["session_text"]);
                if (!string.IsNullOrWhiteSpace(session)) text.Append(" (" + session + ")");

                text.Append(" - " + Db.Get<string>(visit["staff_name"]));
            }

            return text.ToString();
        }

        private void LoadPackages(int clientId)
        {
            try
            {
                dgvPackages.DataSource = ClientRepository.GetPackages(clientId);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>Age in whole years ("34 years old"), or "less than a year" for a baby.</summary>
        private static string AgeText(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            int years = today.Year - birthDate.Year;
            // The birthday of this year has not happened yet, so one year must come off.
            if (birthDate.Date > today.AddYears(-years)) years--;

            if (years <= 0) return "less than a year";
            return years + (years == 1 ? " year old" : " years old");
        }

        private static string ValueOrDash(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        /// <summary>A read-only notes box shows a dash instead of an empty box, which looks broken.</summary>
        private static string Notes(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        // ---------------------------------------------------------------- toolbar actions

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (ClientEditForm form = new ClientEditForm())
            {
                form.EditId = 0;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                // The new client is the one the user wants to see, so the grid selects it.
                LoadClients(form.SavedClientId);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int clientId = CurrentClientId();
            if (clientId <= 0) return;

            using (ClientEditForm form = new ClientEditForm())
            {
                form.EditId = clientId;
                if (form.ShowDialog(this) != DialogResult.OK) return;

                // Editing may change the name or the consent tick, so the row is re-selected.
                LoadClients(form.SavedClientId);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ExpirePackages();
            LoadClients(CurrentClientId());
        }

        private void btnAddPackage_Click(object sender, EventArgs e)
        {
            int clientId = CurrentClientId();
            if (clientId <= 0) return;

            using (PackageForm form = new PackageForm())
            {
                form.ClientId = clientId;
                if (form.ShowDialog(this) != DialogResult.OK) return;
            }

            // The new package must appear in the card, so the details are read again.
            ShowDetails(clientId);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadClients(0);
        }

        private void cboConsent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            LoadClients(0);
        }

        private void dgvClients_SelectionChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            ShowDetails(CurrentClientId());
        }

        private void dgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // A double click on the header or on the empty area under the rows changes nothing.
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }

        // ---------------------------------------------------------------- grid colors

        private void dgvClients_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvClients.Columns[e.ColumnIndex].Name != "colConsent") return;

            bool signed = string.Equals(e.Value.ToString(), "Signed", StringComparison.OrdinalIgnoreCase);

            // Green when the waiver is on file, amber while it is still pending.
            e.CellStyle.ForeColor = signed ? Theme.Success : Theme.Warning;
            // A selected row keeps white text on its highlight, so the color would be unreadable there.
            e.CellStyle.SelectionForeColor = Color.White;
        }

        private void dgvPackages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvPackages.Columns[e.ColumnIndex].Name != "colPackageStatus") return;

            // Status colors come from Theme, so every screen uses the same green / red / gray.
            e.CellStyle.ForeColor = Theme.StatusColor(e.Value.ToString());
            e.CellStyle.SelectionForeColor = Color.White;
        }
    }
}
