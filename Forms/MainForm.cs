using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LittleEreseSystemV3.Controls;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Forms
{
    public partial class MainForm : Form
    {
        // Sidebar widths: the full menu, and the collapsed strip that keeps only the icons.
        private const int SidebarExpandedWidth = 240;
        private const int SidebarCollapsedWidth = 68;

        // 16 px keeps the icon and the caption together. A wider indent pushed the caption to the
        // middle of the button, because a WinForms button centers text and image as one group.
        private const int ButtonTextPadding = 16;
        private const int ButtonIconPadding = 16;

        // The page keys used by Navigate. Kept as constants so a typo is a build error.
        private const string PageDashboard = "Dashboard";
        private const string PageAppointments = "Appointments";
        private const string PageClients = "Clients";
        private const string PageServices = "Services";
        private const string PagePromotions = "Promotions";
        private const string PageBilling = "Billing";
        private const string PageCommissions = "Commissions";
        private const string PageReports = "Reports";
        private const string PageStaff = "Staff";
        private const string PageFileMaintenance = "FileMaintenance";
        private const string PageActivityLog = "ActivityLog";
        private const string PageSettings = "Settings";

        // Remembers the caption of every navigation button, so collapsing the sidebar can clear
        // the texts and expanding it can put them back.
        private readonly Dictionary<Button, string> _navButtonTexts = new Dictionary<Button, string>();

        private bool _sidebarCollapsed;

        // The page on screen right now, so it can be disposed when another one is opened.
        private UserControl _currentPage;

        public bool LoggedOut { get; private set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (DesignTime.IsDesignMode) return;

            WindowState = FormWindowState.Maximized;

            LoadImages();
            ApplyRoleVisibility();

            lblUserName.Text = Session.FullName;
            lblUserRole.Text = Session.Role;

            Navigate(PageDashboard);
        }

        /// <summary>
        /// Opens a page inside pnlContent. Every accepted key builds a NEW instance of its
        /// UserControl, so a page always starts with fresh data.
        /// The previous page is disposed so a long session cannot fill the memory with old screens.
        /// </summary>
        public void Navigate(string pageKey)
        {
            if (string.IsNullOrEmpty(pageKey)) return;

            // Admin only pages stay hidden for Staff, and this is the second guard in code
            // (the first one is ApplyRoleVisibility).
            if (!Session.IsAdmin && IsAdminPage(pageKey)) return;

            UserControl page;
            try
            {
                page = CreatePage(pageKey);
            }
            catch (Exception ex)
            {
                // One broken page must never close the whole app.
                Messages.Error(ex.Message);
                return;
            }

            if (page == null) return;

            UserControl previous = _currentPage;

            pnlContent.SuspendLayout();

            // Detach and dispose before the new page is added, so for a moment there is
            // exactly one page inside pnlContent.
            if (previous != null)
            {
                pnlContent.Controls.Remove(previous);
                previous.Dispose();
            }

            page.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(page);
            _currentPage = page;

            pnlContent.ResumeLayout();

            lblPageTitle.Text = PageTitle(pageKey);
            HighlightNavButton(pageKey);
        }

        private void btnMenuToggle_Click(object sender, EventArgs e)
        {
            SetSidebarCollapsed(!_sidebarCollapsed);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Navigate(PageSettings);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!Messages.Confirm("Log out now?")) return;

            ActivityRepository.Log(Session.UserId, "Logout", "Authentication", "User logged out");

            LoggedOut = true;
            Close();
        }

        private void btnNavDashboard_Click(object sender, EventArgs e)
        {
            Navigate(PageDashboard);
        }

        private void btnNavAppointments_Click(object sender, EventArgs e)
        {
            Navigate(PageAppointments);
        }

        private void btnNavClients_Click(object sender, EventArgs e)
        {
            Navigate(PageClients);
        }

        private void btnNavServices_Click(object sender, EventArgs e)
        {
            Navigate(PageServices);
        }

        private void btnNavPromotions_Click(object sender, EventArgs e)
        {
            Navigate(PagePromotions);
        }

        private void btnNavBilling_Click(object sender, EventArgs e)
        {
            Navigate(PageBilling);
        }

        private void btnNavCommissions_Click(object sender, EventArgs e)
        {
            Navigate(PageCommissions);
        }

        private void btnNavReports_Click(object sender, EventArgs e)
        {
            Navigate(PageReports);
        }

        private void btnNavStaff_Click(object sender, EventArgs e)
        {
            Navigate(PageStaff);
        }

        private void btnNavFileMaintenance_Click(object sender, EventArgs e)
        {
            Navigate(PageFileMaintenance);
        }

        private void btnNavActivityLog_Click(object sender, EventArgs e)
        {
            Navigate(PageActivityLog);
        }

        // ------------------------------------------------------------------
        // Private helpers
        // ------------------------------------------------------------------

        /// <summary>One class per page key. Returns null for an unknown key.</summary>
        private static UserControl CreatePage(string pageKey)
        {
            switch (pageKey)
            {
                case PageDashboard:
                    return new ucDashboard();
                case PageAppointments:
                    return new ucAppointments();
                case PageClients:
                    return new ucClients();
                case PageServices:
                    return new ucServices();
                case PagePromotions:
                    return new ucPromotions();
                case PageBilling:
                    return new ucBilling();
                case PageCommissions:
                    return new ucCommissions();
                case PageReports:
                    return new ucReports();
                case PageStaff:
                    return new ucStaffAccounts();
                case PageFileMaintenance:
                    return new ucFileMaintenance();
                case PageActivityLog:
                    return new ucActivityLog();
                case PageSettings:
                    return new ucSettings();
                default:
                    return null;
            }
        }

        /// <summary>The words shown in the header for each page key.</summary>
        private static string PageTitle(string pageKey)
        {
            switch (pageKey)
            {
                case PageStaff:
                    return "Staff Accounts";
                case PageFileMaintenance:
                    return "File Maintenance";
                case PageActivityLog:
                    return "Activity Log";
                default:
                    return pageKey;
            }
        }

        /// <summary>Pages only the owner may open.</summary>
        private static bool IsAdminPage(string pageKey)
        {
            return pageKey == PageCommissions
                || pageKey == PageReports
                || pageKey == PageStaff
                || pageKey == PageFileMaintenance
                || pageKey == PageActivityLog;
        }

        /// <summary>Paints the button of the page on screen gold and the others in the sidebar color.</summary>
        private void HighlightNavButton(string pageKey)
        {
            Button active = NavButtonFor(pageKey);

            foreach (Button button in NavButtons())
            {
                button.BackColor = button == active ? Theme.Primary : Theme.SidebarBack;
                button.FlatAppearance.MouseOverBackColor = button == active ? Theme.PrimaryLight : Theme.SidebarHover;
                button.FlatAppearance.MouseDownBackColor = button == active ? Theme.PrimaryDark : Theme.SidebarHover;
            }
        }

        /// <summary>The navigation button that belongs to a page key (null for Settings).</summary>
        private Button NavButtonFor(string pageKey)
        {
            switch (pageKey)
            {
                case PageDashboard:
                    return btnNavDashboard;
                case PageAppointments:
                    return btnNavAppointments;
                case PageClients:
                    return btnNavClients;
                case PageServices:
                    return btnNavServices;
                case PagePromotions:
                    return btnNavPromotions;
                case PageBilling:
                    return btnNavBilling;
                case PageCommissions:
                    return btnNavCommissions;
                case PageReports:
                    return btnNavReports;
                case PageStaff:
                    return btnNavStaff;
                case PageFileMaintenance:
                    return btnNavFileMaintenance;
                case PageActivityLog:
                    return btnNavActivityLog;
                default:
                    return null;
            }
        }

        private IEnumerable<Button> NavButtons()
        {
            yield return btnNavDashboard;
            yield return btnNavAppointments;
            yield return btnNavClients;
            yield return btnNavServices;
            yield return btnNavPromotions;
            yield return btnNavBilling;
            yield return btnNavCommissions;
            yield return btnNavReports;
            yield return btnNavStaff;
            yield return btnNavFileMaintenance;
            yield return btnNavActivityLog;
        }

        /// <summary>Staff does not see the owner only modules at all.</summary>
        private void ApplyRoleVisibility()
        {
            if (Session.IsAdmin) return;

            btnNavCommissions.Visible = false;
            btnNavReports.Visible = false;
            btnNavStaff.Visible = false;
            btnNavFileMaintenance.Visible = false;
            btnNavActivityLog.Visible = false;
        }

        /// <summary>
        /// Collapses the sidebar to a strip of icons and expands it again.
        /// The button texts are kept in a dictionary, so nothing has to be retyped.
        /// </summary>
        private void SetSidebarCollapsed(bool collapsed)
        {
            foreach (Button button in NavButtons())
            {
                string text;
                if (!_navButtonTexts.TryGetValue(button, out text)) text = button.Text;

                if (collapsed)
                {
                    // Store the caption once so expanding does not lose it.
                    if (!_navButtonTexts.ContainsKey(button)) _navButtonTexts[button] = text;

                    button.Text = string.Empty;
                    button.Padding = new Padding(ButtonIconPadding, 0, 0, 0);
                }
                else
                {
                    button.Text = text;
                    button.Padding = new Padding(ButtonTextPadding, 0, 0, 0);
                }
            }

            pnlBrand.Visible = !collapsed;
            lblVersion.Visible = !collapsed;

            pnlSidebar.Width = collapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;

            _sidebarCollapsed = collapsed;
        }

        /// <summary>
        /// Loads every picture this shell needs. IconHelper returns null for a missing file,
        /// so an incomplete Assets folder only leaves a blank spot instead of crashing the app.
        /// </summary>
        private void LoadImages()
        {
            picBrand.Image = IconHelper.Photo("BusinessIcon");

            SetNavIcon(btnNavDashboard, "DashboardHouseIcon");
            SetNavIcon(btnNavAppointments, "CalendarDarkIcon");
            SetNavIcon(btnNavClients, "PeopleIcon");
            SetNavIcon(btnNavServices, "ServicesIcon");
            SetNavIcon(btnNavPromotions, "DiscountIcon");
            SetNavIcon(btnNavBilling, "BillingIcon");
            SetNavIcon(btnNavCommissions, "CrownIcon");
            SetNavIcon(btnNavReports, "ReportIcon");
            SetNavIcon(btnNavStaff, "StaffRoleIcon");
            SetNavIcon(btnNavFileMaintenance, "FileMaintenanceIcon");
            SetNavIcon(btnNavActivityLog, "InformationIcon");

            btnMenuToggle.Image = IconHelper.Load("MenuDarkIcon", 24);
            btnSettings.Image = IconHelper.Load("SettingsIcon", 22);
            btnLogout.Image = IconHelper.Load("LogoutIcon", 22);
            picUser.Image = IconHelper.Load("UserIconYellowishColorTransparent", 36);
        }

        /// <summary>
        /// One sidebar icon, repainted in the sidebar text color.
        /// ImageBeforeText is set here (and not in the Designer) with the other runtime styling.
        /// </summary>
        private static void SetNavIcon(Button button, string iconName)
        {
            button.Image = IconHelper.Tinted(iconName, Theme.SidebarText, 22);
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
        }
    }
}
