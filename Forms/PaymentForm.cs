using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    /// <summary>
    /// The payment dialog of the Billing screen: it collects how much is paid, how it was paid and
    /// the reference. The dialog itself never saves anything - Checkout does that when the desk
    /// completes a service, and TakePayment does it when a payment is taken on its own.
    /// </summary>
    public partial class PaymentForm : Form
    {
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

        /// <summary>
        /// The smallest amount this payment may be: 50% of the total for the first payment (the
        /// whole total for a promo), 1.00 for a later payment. 0 or a value above AmountDue means
        /// AmountDue, which is the "complete the service" case.
        /// </summary>
        public decimal MinimumAmount
        {
            get
            {
                if (_minimumAmount <= 0m || _minimumAmount > AmountDue) return AmountDue;
                return _minimumAmount;
            }
            set { _minimumAmount = value; }
        }

        /// <summary>Shown in the title when the caller wants another wording (else the default stays).</summary>
        public string HeaderText { get; set; }

        /// <summary>What the client pays now. Filled in when Confirm is pressed.</summary>
        public decimal AmountToPay { get; private set; }

        // Raw value of MinimumAmount; the property above applies the 0 / above-balance rule.
        private decimal _minimumAmount;

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
            lblAmountDue.Text = Money.Text(AmountDue);

            if (!string.IsNullOrWhiteSpace(HeaderText)) lblTitle.Text = HeaderText;

            SetupAmountToPay();

            // Cash is the normal case, so it is picked and the change row is already filled in.
            cboMethod.SelectedIndex = 0;
            ApplyMethod();
        }

        /// <summary>
        /// Prefills "amount to pay now" with the smallest allowed amount. When the smallest amount
        /// already equals the balance there is nothing to choose (completing the service), so the
        /// box is read-only and the "pay in full" button is hidden - the dialog then behaves
        /// exactly as it did before.
        /// </summary>
        private void SetupAmountToPay()
        {
            decimal minimum = MinimumAmount;
            bool fixedAmount = minimum >= AmountDue;

            // The other money boxes show a plain number (no peso sign), so this one does too.
            txtAmountToPay.Text = minimum.ToString("N2", Money.Culture);
            txtAmountToPay.ReadOnly = fixedAmount;
            btnPayFull.Visible = !fixedAmount;

            if (fixedAmount)
            {
                lblMinimumNote.Text = string.Empty;
            }
            else if (minimum >= AmountDue * 0.5m)
            {
                // The first payment of a bill: 50% of the total, or the whole total for a promo.
                lblMinimumNote.Text = "The first payment must be at least " + Money.Text(minimum) + ".";
            }
            else
            {
                lblMinimumNote.Text = "Any amount up to the balance.";
            }
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

        private void txtAmountToPay_TextChanged(object sender, EventArgs e)
        {
            UpdateChange();
        }

        /// <summary>
        /// The amount typed in "amount to pay now", or the smallest allowed amount while the box
        /// holds something that is not a number (so the change row never shows a wrong figure).
        /// </summary>
        private decimal CurrentAmountToPay()
        {
            decimal amount;
            return Validators.TryMoney(txtAmountToPay.Text, out amount) ? amount : MinimumAmount;
        }

        private void UpdateChange()
        {
            decimal amount;
            if (!Validators.TryMoney(txtTendered.Text, out amount))
            {
                lblChange.Text = Money.Text(0m);
                return;
            }

            decimal change = amount - CurrentAmountToPay();
            if (change < 0m) change = 0m;

            lblChange.Text = Money.Text(change);
        }

        // ---------------------------------------------------------------- buttons

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            AmountToPay = CurrentAmountToPay();

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

        /// <summary>Fills the box with the balance, so the client settles the bill today.</summary>
        private void btnPayFull_Click(object sender, EventArgs e)
        {
            txtAmountToPay.Text = AmountDue.ToString("N2", Money.Culture);
            UpdateChange();
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

            decimal amountToPay;
            if (!Validators.TryMoney(txtAmountToPay.Text, out amountToPay))
            {
                Validators.SetError(errorProvider1, txtAmountToPay, "Type how much the client pays now.");
                valid = false;
            }
            else if (amountToPay < MinimumAmount)
            {
                Validators.SetError(errorProvider1, txtAmountToPay,
                    "This payment must be at least " + Money.Text(MinimumAmount) + ".");
                valid = false;
            }
            else if (amountToPay > AmountDue)
            {
                Validators.SetError(errorProvider1, txtAmountToPay,
                    "This payment cannot be more than the balance of " + Money.Text(AmountDue) + ".");
                valid = false;
            }
            else
            {
                Validators.ClearError(errorProvider1, txtAmountToPay);
            }

            if (string.Equals(method, "Cash", StringComparison.OrdinalIgnoreCase))
            {
                Validators.ClearError(errorProvider1, txtReference);

                decimal tendered;
                if (!Validators.TryMoney(txtTendered.Text, out tendered))
                {
                    Validators.SetError(errorProvider1, txtTendered, "Type the amount the client handed over.");
                    valid = false;
                }
                else if (tendered < amountToPay)
                {
                    Validators.SetError(errorProvider1, txtTendered,
                        "The amount must be at least " + Money.Text(amountToPay) + ".");
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

        // ---------------------------------------------------------------- taking one payment

        /// <summary>
        /// Takes ONE payment for an appointment - a down payment, a part of the balance or the
        /// rest of it - on any date. This is the whole "take a payment" flow of the Billing screen:
        /// it reads the bill, works out the smallest allowed amount, shows this dialog and saves
        /// the payment through BillingRepository.RecordPayment. Returns true when money was saved.
        /// </summary>
        public static bool TakePayment(IWin32Window owner, int appointmentId, string headerText)
        {
            try
            {
                DataRow bill = BillingRepository.GetBillHeader(appointmentId);
                if (bill == null)
                {
                    Messages.Info("Nothing is left to pay for this appointment.");
                    return false;
                }

                decimal balance = Db.Get<decimal>(bill["balance_due"]);
                if (balance <= 0m)
                {
                    Messages.Info("Nothing is left to pay for this appointment.");
                    return false;
                }

                decimal subtotal = Db.Get<decimal>(bill["subtotal"]);
                decimal discount = Db.Get<decimal>(bill["discount_amount"]);
                decimal paidSoFar = Db.Get<decimal>(bill["paid_so_far"]);

                // The first payment of the bill must reach the down payment (50% of the total, or
                // the whole total for a promo); every later payment may be as small as 1.00.
                decimal minimum = paidSoFar == 0m
                    ? AppointmentRepository.DownpaymentDue(subtotal, discount)
                    : 1.00m;
                if (minimum > balance) minimum = balance;

                string clientName = Db.Get<string>(bill["client_name"], "-");

                using (PaymentForm form = new PaymentForm())
                {
                    form.AmountDue = balance;
                    form.ClientName = clientName;
                    form.HeaderText = string.IsNullOrWhiteSpace(headerText) ? "Take Payment" : headerText;
                    form.MinimumAmount = minimum;

                    if (form.ShowDialog(owner) != DialogResult.OK) return false;

                    int billingId;
                    try
                    {
                        billingId = BillingRepository.RecordPayment(appointmentId, form.AmountToPay,
                            form.Method, form.Reference, form.Verified, form.Tendered);
                    }
                    catch (InvalidOperationException ex)
                    {
                        // The repository spells out the rule it could not apply, so it is a warning.
                        Messages.Warn(ex.Message);
                        return false;
                    }

                    // The bill has just been updated, so the left-over balance is read again.
                    decimal left = form.AmountDue - form.AmountToPay;
                    if (left < 0m) left = 0m;

                    string done = "Payment of " + Money.Text(form.AmountToPay) +
                        " received. Balance left: " + Money.Text(left) + ".";

                    if (string.Equals(form.Method, "Cash", StringComparison.OrdinalIgnoreCase) &&
                        form.Tendered > form.AmountToPay)
                    {
                        done += " Change: " + Money.Text(form.Tendered - form.AmountToPay) + ".";
                    }

                    Messages.Info(done);
                    return billingId > 0;
                }
            }
            catch (InvalidOperationException ex)
            {
                Messages.Warn(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return false;
            }
        }
    }
}
