using System;
using System.Windows.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    /// <summary>
    /// The payment dialog of the Billing screen: it only collects how the balance was paid.
    /// The money itself is saved by BillingRepository.Checkout after this dialog returns OK, so
    /// nothing here touches the database.
    /// </summary>
    public partial class PaymentForm : Form
    {
        // Money in the project format: peso sign + #,##0.00.
        private const string MoneyFormat = "\u20B1#,##0.00";

        /// <summary>What the client still owes. The caller sets it before ShowDialog().</summary>
        public decimal AmountDue { get; set; }

        /// <summary>Shown at the top, so the desk can read the name back to the client.</summary>
        public string ClientName { get; set; }

        /// <summary>The chosen method: Cash, GCash or Bank Transfer.</summary>
        public string Method { get; private set; }

        /// <summary>The GCash / bank reference (empty for cash).</summary>
        public string Reference { get; private set; }

        /// <summary>The "I checked it in the app" tick.</summary>
        public bool Verified { get; private set; }

        /// <summary>The cash handed over by the client (0 for GCash / bank).</summary>
        public decimal Tendered { get; private set; }

        public PaymentForm()
        {
            InitializeComponent();
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnConfirm);
            Theme.StyleSecondaryButton(btnCancel);

            lblClientName.Text = string.IsNullOrWhiteSpace(ClientName) ? "-" : ClientName;
            lblAmountDue.Text = AmountDue.ToString(MoneyFormat);

            // Cash is the normal case, so it is picked and the change row is already filled in.
            cboMethod.SelectedIndex = 0;
            ApplyMethod();
        }

        // ---------------------------------------------------------------- the method

        private void cboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;
            ApplyMethod();
        }

        /// <summary>
        /// Cash needs the amount handed over, GCash and bank transfers need a reference and the
        /// "verified" tick. The other boxes are cleared and switched off so nothing is half filled.
        /// </summary>
        private void ApplyMethod()
        {
            string method = SelectedMethod();
            bool cash = string.Equals(method, "Cash", StringComparison.OrdinalIgnoreCase);

            txtTendered.Enabled = cash;
            chkVerified.Enabled = !cash;
            txtReference.Enabled = !cash;

            if (cash)
            {
                chkVerified.Checked = false;
                txtReference.Text = string.Empty;
            }
            else
            {
                txtTendered.Text = string.Empty;
            }

            Validators.ClearError(errorProvider1, txtTendered);
            Validators.ClearError(errorProvider1, txtReference);

            UpdateChange();
        }

        private string SelectedMethod()
        {
            return cboMethod.SelectedItem == null ? "Cash" : cboMethod.SelectedItem.ToString();
        }

        /// <summary>
        /// The change label follows what is typed. It never goes negative: while the amount is too
        /// small the label simply says nothing is handed over yet.
        /// </summary>
        private void txtTendered_TextChanged(object sender, EventArgs e)
        {
            UpdateChange();
        }

        private void UpdateChange()
        {
            decimal amount;
            if (!Validators.TryMoney(txtTendered.Text, out amount))
            {
                lblChange.Text = 0m.ToString(MoneyFormat);
                return;
            }

            decimal change = amount - AmountDue;
            if (change < 0m) change = 0m;

            lblChange.Text = change.ToString(MoneyFormat);
        }

        // ---------------------------------------------------------------- buttons

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            Method = SelectedMethod();
            Reference = string.Equals(Method, "Cash", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : txtReference.Text.Trim();
            Verified = chkVerified.Checked;

            decimal tendered;
            Tendered = Validators.TryMoney(txtTendered.Text, out tendered) ? tendered : 0m;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Cash: a real number that reaches the amount due. GCash / bank: a reference number and
        /// the tick, because the system never checks a bank or GCash account by itself.
        /// </summary>
        private bool ValidateInput()
        {
            bool valid = true;

            string method = SelectedMethod();

            if (string.Equals(method, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                Validators.ClearError(errorProvider1, txtReference);

                decimal tendered;
                if (!Validators.TryMoney(txtTendered.Text, out tendered))
                {
                    Validators.SetError(errorProvider1, txtTendered, "Type the amount the client handed over.");
                    valid = false;
                }
                else if (tendered < AmountDue)
                {
                    Validators.SetError(errorProvider1, txtTendered,
                        "The amount must be at least " + AmountDue.ToString(MoneyFormat) + ".");
                    valid = false;
                }
                else
                {
                    Validators.ClearError(errorProvider1, txtTendered);
                }
            }
            else
            {
                Validators.ClearError(errorProvider1, txtTendered);

                if (!Validators.IsValidReference(txtReference.Text))
                {
                    Validators.SetError(errorProvider1, txtReference,
                        "Type the " + method + " reference number (6 to 30 letters or digits).");
                    valid = false;
                }
                else
                {
                    Validators.ClearError(errorProvider1, txtReference);
                }

                if (!chkVerified.Checked)
                {
                    Validators.SetError(errorProvider1, chkVerified,
                        "Tick this box after you check the payment in the GCash / bank app.");
                    valid = false;
                }
                else
                {
                    Validators.ClearError(errorProvider1, chkVerified);
                }
            }

            return valid;
        }
    }
}
