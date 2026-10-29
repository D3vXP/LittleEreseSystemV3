using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class PackageForm : Form
    {
        /// <summary>One entry of the package service drop-down list.</summary>
        private sealed class ServiceItem
        {
            public int Id;
            public string Name;
            public decimal Price;
            public int DefaultSessions;

            public ServiceItem(int id, string name, decimal price, int defaultSessions)
            {
                Id = id;
                Name = name;
                Price = price;
                DefaultSessions = defaultSessions;
            }

            // The combo shows the price too, so the staff member can confirm it without leaving the form.
            public override string ToString()
            {
                return Name + " - " + Money.Text(Price);
            }
        }

        /// <summary>The client who buys the package. The caller sets this before ShowDialog().</summary>
        public int ClientId { get; set; }

        /// <summary>The package that was saved, so the caller can show it.</summary>
        public int SavedPackageId { get; private set; }

        public PackageForm()
        {
            InitializeComponent();
        }

        private void PackageForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnSave);
            Theme.StyleSecondaryButton(btnCancel);

            lblPurchase.Text = "Purchase date";
            lblHint.Text = "The package price is charged on the first session.";

            dtpPurchaseDate.Value = DateTime.Today;

            // The expiry date is optional, so it starts switched off. The value is set BEFORE the
            // tick is cleared, because assigning Value to an unchecked picker ticks it again.
            dtpExpiryDate.Value = DateTime.Today.AddMonths(6);
            dtpExpiryDate.Checked = false;

            lblClientName.Text = ClientName();
            FillServices();
            UpdatePrice();
        }

        /// <summary>The client's name for the read-only line, or "-" when it cannot be read.</summary>
        private string ClientName()
        {
            if (ClientId <= 0) return "-";

            try
            {
                DataRow row = ClientRepository.GetById(ClientId);
                if (row == null) return "-";

                return Db.Get<string>(row["last_name"]) + ", " + Db.Get<string>(row["first_name"]);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return "-";
            }
        }

        /// <summary>
        /// Fills the service list with the Active services that really are packages (more than one
        /// session). A one-session service cannot be a package, so it is not offered here; the rule
        /// itself is enforced again by ClientRepository.AddPackage.
        /// </summary>
        private void FillServices()
        {
            cboService.Items.Clear();

            try
            {
                DataTable table = ServiceRepository.GetActiveForBooking();
                foreach (DataRow row in table.Rows)
                {
                    ServiceItem item = new ServiceItem(
                        Db.Get<int>(row["service_id"]),
                        Db.Get<string>(row["service_name"]),
                        Db.Get<decimal>(row["price"]),
                        Db.Get<int>(row["default_total_sessions"], 1));

                    if (item.DefaultSessions > 1) cboService.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }

            if (cboService.Items.Count > 0) cboService.SelectedIndex = 0;
        }

        private void cboService_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServiceItem item = cboService.SelectedItem as ServiceItem;
            if (item == null) return;

            // The service remembers how many sessions its package normally has.
            nudTotalSessions.Value = ClampNumeric(nudTotalSessions, item.DefaultSessions);
            UpdatePrice();
        }

        /// <summary>Shows the price of the selected package, or "-" when nothing is selected.</summary>
        private void UpdatePrice()
        {
            ServiceItem item = cboService.SelectedItem as ServiceItem;
            lblPriceValue.Text = item == null ? "-" : Money.Text(item.Price);
        }

        /// <summary>Keeps a value inside the limits of a NumericUpDown instead of throwing.</summary>
        private static decimal ClampNumeric(NumericUpDown box, int value)
        {
            decimal number = value;
            if (number < box.Minimum) number = box.Minimum;
            if (number > box.Maximum) number = box.Maximum;
            return number;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidateFields()) return;

            ServiceItem item = cboService.SelectedItem as ServiceItem;
            int sessions = Convert.ToInt32(nudTotalSessions.Value);
            DateTime purchase = dtpPurchaseDate.Value.Date;
            DateTime? expiry = dtpExpiryDate.Checked ? (DateTime?)dtpExpiryDate.Value.Date : null;

            // A second package for the same service is allowed (a guest may buy ahead), but the
            // staff member should notice, because the first one must be finished first.
            try
            {
                if (ClientRepository.HasActivePackage(ClientId, item.Id))
                {
                    if (!Messages.Confirm("This client already has an active package for this service. Add another?"))
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
                SavedPackageId = ClientRepository.AddPackage(ClientId, item.Id, sessions, purchase, expiry);

                ActivityRepository.Log(Session.UserId, "Add Package", "Clients",
                    item.Name + " x" + sessions + " for " + lblClientName.Text);
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

            if (ClientId <= 0)
            {
                Messages.Warn("Choose a client first.");
                return false;
            }

            if (cboService.SelectedIndex < 0)
            {
                Validators.SetError(errorProvider1, cboService, "Choose the package service.");
                ok = false;
            }

            // An expiry date is optional, but it must come after the purchase date to make sense.
            if (dtpExpiryDate.Checked && dtpExpiryDate.Value.Date <= dtpPurchaseDate.Value.Date)
            {
                Validators.SetError(errorProvider1, dtpExpiryDate, "The expiry date must be after the purchase date.");
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
