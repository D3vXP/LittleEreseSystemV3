using System;
using System.Data;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class LoginForm : Form
    {
        // POLISH: the owner may want a different number of tries or a different waiting time.
        private const int MaxFailedAttempts = 5;
        private const int LockoutSeconds = 30;

        // How many wrong passwords in a row, and until when the login is blocked.
        // Kept in memory on purpose: closing the app also clears the block.
        private int _failedAttempts;
        private DateTime _lockedUntil = DateTime.MinValue;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            // Images are loaded here and not in the Designer, because the Designer cannot read
            // the Assets folder.
            picSide.Image = IconHelper.Photo("ImageSpaLogoMiddle");
            picLogo.Image = IconHelper.Photo("BusinessLogoIcon");

            Theme.StylePrimaryButton(btnLogin);

            btnShowPassword.Font = Theme.IconFont;
            btnShowPassword.Text = IconHelper.Glyph.View;

            lblError.Text = string.Empty;
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = (txtUsername.Text ?? string.Empty).Trim();
            string password = txtPassword.Text ?? string.Empty;

            lblError.Text = string.Empty;

            // Too many wrong tries: refuse to even ask the database until the waiting time is over.
            int secondsLeft = LockoutSecondsLeft();
            if (secondsLeft > 0)
            {
                lblError.Text = "Too many failed attempts. Try again in " + secondsLeft + " seconds.";
                return;
            }

            if (username.Length == 0 || password.Length == 0)
            {
                lblError.Text = "Enter your username and password.";
                txtUsername.Focus();
                return;
            }

            DataRow user;
            try
            {
                user = UserRepository.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
                return;
            }

            if (user == null)
            {
                RegisterFailedAttempt();
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Correct password: clear the counter and remember who signed in.
            _failedAttempts = 0;
            _lockedUntil = DateTime.MinValue;

            int userId = Db.Get<int>(user["user_id"]);
            string fullName = Db.Get<string>(user["full_name"]);
            string role = Db.Get<string>(user["role"]);

            Session.Start(userId, fullName, username, role);
            ActivityRepository.Log(userId, "Login", "Authentication", "User logged in");

            OpenMainForm();
        }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            // One click switches between the dots and the readable password.
            bool wasHidden = txtPassword.UseSystemPasswordChar;

            txtPassword.UseSystemPasswordChar = !wasHidden;
            btnShowPassword.Text = wasHidden ? IconHelper.Glyph.Hide : IconHelper.Glyph.View;

            // Keep the caret where the user was typing.
            txtPassword.Focus();
            txtPassword.SelectionStart = txtPassword.TextLength;
        }

        /// <summary>Shows the main screen and hides the login screen.</summary>
        private void OpenMainForm()
        {
            MainForm main = new MainForm();
            main.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);

            Hide();
            main.Show();
        }

        /// <summary>
        /// After the main screen closes: a log out comes back here, closing the window exits the app.
        /// </summary>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm main = sender as MainForm;

            if (main != null && main.LoggedOut)
            {
                Session.End();

                txtUsername.Clear();
                txtPassword.Clear();
                lblError.Text = string.Empty;

                Show();
                txtUsername.Focus();
            }
            else
            {
                Application.Exit();
            }
        }

        /// <summary>Counts one wrong password and starts the waiting time after too many.</summary>
        private void RegisterFailedAttempt()
        {
            _failedAttempts++;

            if (_failedAttempts >= MaxFailedAttempts)
            {
                _lockedUntil = DateTime.Now.AddSeconds(LockoutSeconds);
                _failedAttempts = 0;

                lblError.Text = "Too many failed attempts. Try again in " + LockoutSeconds + " seconds.";
            }
            else
            {
                lblError.Text = "Invalid username or password.";
            }
        }

        /// <summary>Seconds the user still has to wait, or 0 when the login is open.</summary>
        private int LockoutSecondsLeft()
        {
            if (_lockedUntil <= DateTime.Now) return 0;

            return (int)Math.Ceiling((_lockedUntil - DateTime.Now).TotalSeconds);
        }
    }
}
