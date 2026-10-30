using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    /// <summary>
    /// Shown when a NEW client carries the first and last name of somebody already in the book.
    /// The two persons are shown side by side, so the front desk can compare the birth date, the
    /// contact number and the address and decide: this is a different person ("Add anyway"), or it
    /// is the same guest and the existing record is used instead of writing a second one.
    /// Read-only: this screen never writes to the database.
    /// </summary>
    public partial class DuplicateClientForm : Form
    {
        /// <summary>What the user decided about the duplicate name.</summary>
        public enum DuplicateAction
        {
            /// <summary>It is a different person: the caller saves the new client as usual.</summary>
            AddAnyway,
            /// <summary>It is the same person: the caller keeps the existing record (ExistingClientId).</summary>
            UseExisting,
            /// <summary>Nothing is saved; the caller stays on the edit form with the typed data.</summary>
            GoBack
        }

        /// <summary>What the user decided. AddAnyway until a button is pressed.</summary>
        public DuplicateAction ChosenAction { get; private set; }

        /// <summary>The client that is used again with "Use existing client" (0 when the id is unknown).</summary>
        public int ExistingClientId { get; private set; }

        /// <summary>
        /// The typed data, one row with the columns of ClientRepository.FindSameName:
        /// full_name, birth_date, gender, contact_number, address. The caller sets this before ShowDialog().
        /// </summary>
        public DataRow NewClient { get; set; }

        /// <summary>
        /// The clients that already carry that name (ClientRepository.FindSameName).
        /// The caller sets this before ShowDialog(); the first row is shown on the right.
        /// </summary>
        public DataTable Candidates { get; set; }

        public DuplicateClientForm()
        {
            InitializeComponent();
        }

        private void DuplicateClientForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnAddAnyway);
            Theme.StyleSecondaryButton(btnUseExisting);
            Theme.StyleSecondaryButton(btnGoBack);

            // The left card shows what was typed, so the two cards can be compared field by field.
            FillCard(lblNewName, lblNewBirth, lblNewGender, lblNewContact, lblNewAddress, NewClient);

            // The right card shows one candidate. The combo is only visible when there is a real
            // choice to make; with a single match that one person is simply shown.
            bool hasChoice = Candidates != null && Candidates.Rows.Count > 1;

            cboExisting.DisplayMember = "full_name";
            cboExisting.ValueMember = "client_id";
            cboExisting.DataSource = Candidates;

            pnlPick.Visible = hasChoice;

            if (Candidates != null && Candidates.Rows.Count > 0)
            {
                cboExisting.SelectedIndex = 0;
                FillCard(lblExistingName, lblExistingBirth, lblExistingGender, lblExistingContact,
                    lblExistingAddress, Candidates.Rows[0]);
            }

            lblCount.Text = hasChoice
                ? Candidates.Rows.Count + " clients already have this name."
                : "One client already has this name.";
        }

        private void cboExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            // The combo belongs to the right card: the card must follow the chosen candidate.
            DataRowView view = cboExisting.SelectedItem as DataRowView;
            if (view == null) return;

            FillCard(lblExistingName, lblExistingBirth, lblExistingGender, lblExistingContact,
                lblExistingAddress, view.Row);

            ExistingClientId = Db.Get<int>(view.Row["client_id"]);
        }

        private void btnAddAnyway_Click(object sender, EventArgs e)
        {
            ChosenAction = DuplicateAction.AddAnyway;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnUseExisting_Click(object sender, EventArgs e)
        {
            // The id of the row on screen, so the caller can use that record even when the combo
            // was never touched (with a single match the combo is hidden).
            DataRowView view = cboExisting.SelectedItem as DataRowView;
            ExistingClientId = view == null ? 0 : Db.Get<int>(view.Row["client_id"]);

            if (ExistingClientId <= 0)
            {
                Messages.Info("Please pick the existing client from the list.");
                return;
            }

            ChosenAction = DuplicateAction.UseExisting;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            ChosenAction = DuplicateAction.GoBack;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Writes one person into one card. A field the book does not have shows a dash, because an
        /// empty line next to a filled one looks like a value the user still has to fix.
        /// </summary>
        private static void FillCard(Label name, Label birth, Label gender, Label contact, Label address, DataRow row)
        {
            if (row == null)
            {
                name.Text = "-";
                birth.Text = "-";
                gender.Text = "-";
                contact.Text = "-";
                address.Text = "-";
                return;
            }

            name.Text = Value(row, "full_name");

            DateTime? birthDate = Db.Get<DateTime?>(row["birth_date"]);
            birth.Text = birthDate.HasValue
                ? "Birth date: " + birthDate.Value.ToString("MMM dd, yyyy")
                : "Birth date: -";

            gender.Text = "Gender: " + Value(row, "gender");
            contact.Text = "Contact number: " + Value(row, "contact_number");
            address.Text = "Address: " + Value(row, "address");
        }

        /// <summary>One column of the row as text, or a dash when the book has nothing there.</summary>
        private static string Value(DataRow row, string column)
        {
            string text = row.Table.Columns.Contains(column) ? Db.Get<string>(row[column]) : null;
            return string.IsNullOrWhiteSpace(text) ? "-" : text;
        }
    }
}
