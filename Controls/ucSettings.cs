using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucSettings : UserControl
    {
        public ucSettings()
        {
            InitializeComponent();
        }

        private void ucSettings_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            Theme.StylePrimaryButton(btnChangePassword);

            picAvatar.Image = IconHelper.Load("UserIconYellowishColorTransparent", 96);

            // Who is signed in comes from Session; the contact number is the only field
            // that is not remembered there, so it is read from the database.
            lblFullName.Text = Session.FullName;
            lblRole.Text = Session.Role + " account";
            lblUsername.Text = "Username: " + Session.Username;
            lblContact.Text = "Contact: " + CurrentContact();

            lblAppVersion.Text = "Version " + AppVersionText();

            // The database name is read from the connection string so the About card can never
            // show a name that does not match the database the app is really using.
            lblDatabase.Text = "Database: " + DatabaseName();
        }

        /// <summary>Reads the signed-in user's contact number. Returns "-" when there is none.</summary>
        private string CurrentContact()
        {
            try
            {
                DataRow row = UserRepository.GetById(Session.UserId);
                if (row == null) return "-";

                string contact = Db.Get<string>(row["contact_number"]);
                // POLISH: change the "-" placeholder if you prefer an empty line.
                return Validators.IsRequired(contact) ? contact : "-";
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return "-";
            }
        }

        /// <summary>The version of the running program, straight from the assembly.</summary>
        private static string AppVersionText()
        {
            try
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                // POLISH: use ToString() instead of ToString(3) to also show the build number.
                return version == null ? "1.0.0" : version.ToString(3);
            }
            catch
            {
                return "1.0.0";
            }
        }

        /// <summary>The database name taken out of the connection string in App.config.</summary>
        private static string DatabaseName()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Db.ConnectionString);
                return string.IsNullOrEmpty(builder.InitialCatalog) ? "(unknown)" : builder.InitialCatalog;
            }
            catch
            {
                // A malformed connection string must not break the Settings page.
                return "(unknown)";
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidatePasswords()) return;

            bool changed;
            try
            {
                changed = UserRepository.ChangeOwnPassword(
                    Session.UserId, txtCurrentPassword.Text, txtNewPassword.Text);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (!changed)
            {
                Validators.SetError(errorProvider1, txtCurrentPassword, "Current password is incorrect.");
                Messages.Warn("Current password is incorrect.");
                txtCurrentPassword.Focus();
                txtCurrentPassword.SelectAll();
                return;
            }

            ActivityRepository.Log(Session.UserId, "Change Password", "Settings", "Own password changed");

            ClearBoxes();
            Messages.Info("Your password has been changed. Use the new one the next time you sign in.");
        }

        /// <summary>Shows the red error icons and returns false when something is still wrong.</summary>
        private bool ValidatePasswords()
        {
            bool ok = true;

            // 1. All three boxes must be filled in.
            if (!Validators.Require(errorProvider1, txtCurrentPassword, "Enter your current password."))
            {
                ok = false;
            }

            if (!Validators.Require(errorProvider1, txtNewPassword, "Enter a new password."))
            {
                ok = false;
            }

            if (!Validators.Require(errorProvider1, txtConfirmNewPassword, "Repeat the new password."))
            {
                ok = false;
            }

            // Everything below would be confusing while the boxes are still empty.
            if (!ok) return false;

            // 2. The new password must follow the agreed rule.
            if (!Validators.IsValidPassword(txtNewPassword.Text))
            {
                Validators.SetError(errorProvider1, txtNewPassword,
                    "At least 8 characters with a letter and a number.");
                ok = false;
            }

            // 3. The two new boxes must match.
            if (!string.Equals(txtNewPassword.Text, txtConfirmNewPassword.Text, StringComparison.Ordinal))
            {
                Validators.SetError(errorProvider1, txtConfirmNewPassword,
                    "The two new passwords do not match.");
                ok = false;
            }

            // 4. The new password must differ from the current one.
            if (string.Equals(txtNewPassword.Text, txtCurrentPassword.Text, StringComparison.Ordinal))
            {
                Validators.SetError(errorProvider1, txtNewPassword,
                    "The new password must be different from the current one.");
                ok = false;
            }

            return ok;
        }

        private void ClearBoxes()
        {
            txtCurrentPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmNewPassword.Clear();
            errorProvider1.Clear();
        }
    }
}
