# PROGRESS.md

Update this file at the end of every task.

## Done
- Project scaffold: 9 Forms, 12 UserControls (WinForms), Data and Helpers classes created empty.
- Database script (Database/LittleEreses_Database.sql) and price list (Database/SeedData.sql) prepared.
- Fix pack applied: WinForms UserControls, clean .csproj, App.config connection string, DesignTime helper, Db helper.
- Step 1: Helpers filled in - Theme (colors, fonts, button/grid styles, status colors),
  IconHelper (Assets\Images loading, scaling, tinting, cached, plus MDL2 Glyph codes),
  Messages, Session, PasswordHasher (PBKDF2-SHA256), Validators (regex rules + ErrorProvider helpers).
- Program.cs catches UI-thread and AppDomain exceptions, shows Messages.Error(Db.Describe(ex))
  and appends the full exception to error.log next to the exe.
- PROJECT_RULES.md got the "Additions (Step 1)" and "Additions (Step 2)" sections.
- Step 2: UserRepository (Authenticate / UsernameExists / Create / SetPassword),
  ActivityRepository.Log (never throws), DemoDataSeeder (gives the placeholder admin a real
  password, creates 7 demo staff accounts, safe to run many times).
- Program.cs: startup order is --smoketest, Db.CanConnect, DemoDataSeeder, LoginForm.
- Helpers\SmokeTest.cs: "LittleEreseSystemV3.exe --smoketest" checks the UI classes, the tables,
  the service count and the demo logins, writes smoketest.log and exits with 0 or 1.
- Helpers\PreviewTool.cs: "LittleEreseSystemV3.exe --preview <ClassName> [--role Admin|Staff]"
  signs in a preview user, shows the form (or hosts a UserControl in a 1180x720 form), waits
  ~800 ms, writes previews\<ClassName>.png next to the exe and exits 0; on any error it writes
  previews\<ClassName>.error.txt and exits 1. Startup order: --smoketest, --preview, normal flow.
- Step 3: LoginForm layout (920x580, FixedSingle, CenterScreen) and MainForm shell
  (1280x720, MinimumSize 1100x650) done, plus a role aware sidebar. Previews checked.
- Step 4 (this task): LoginForm and MainForm are now working screens, not shells.
  - LoginForm fix: the username and the password field are now identical. The border lives on a
    host panel (new pnlUsernameBox + the existing pnlPassword), both 340 x 34 px with BackColor
    Surface and BorderStyle FixedSingle, holding a borderless BodyFont text box with 8 px left
    padding; btnShowPassword (36 px) stays at the right edge inside pnlPassword. The login card is
    340 px wide (same width as the Login button) and its rows are fixed heights, so nothing is
    cut off. Every control name, TabIndex and event wiring is unchanged.
  - LoginForm logic: images (ImageSpaLogoMiddle, BusinessLogoIcon) and Theme.StylePrimaryButton;
    btnShowPassword uses Theme.IconFont + Glyph.View and toggles UseSystemPasswordChar and the
    glyph (View/Hide); empty fields, wrong password and the lock out (5 failed tries in a row,
    then 30 seconds, shown as a countdown in lblError) are all reported in lblError, no popup.
    On success: Session.Start, ActivityRepository.Log("Login"), MainForm shown and the login
    form hidden. When MainForm closes: LoggedOut -> Session.End and the login form comes back
    with empty boxes, otherwise Application.Exit().
  - MainForm logic: public bool LoggedOut; Load sets WindowState Maximized, loads picBrand and
    every sidebar icon through IconHelper.Tinted(..., Theme.SidebarText, 22) (missing files just
    stay blank), sets the header icons/pictures and lblUserName/lblUserRole from Session, hides
    the five admin-only navigation buttons for Staff and opens Dashboard.
    public void Navigate(key) accepts Dashboard, Appointments, Clients, Services, Promotions,
    Billing, Commissions, Reports, Staff, FileMaintenance, ActivityLog and Settings: it builds a
    NEW UserControl, docks it Fill, disposes the previous page, sets lblPageTitle and paints the
    active nav button Theme.Primary. Admin pages refuse to open for a non-admin. Page creation is
    wrapped in try/catch (Messages.Error) so a broken page cannot close the app.
    btnMenuToggle collapses the sidebar between 240 and 68 px (texts kept in a dictionary, brand
    and version hidden while collapsed); btnSettings opens Settings; btnLogout asks for
    confirmation, logs "Logout" and closes with LoggedOut = true.
  - Sidebar layout fix found in the preview: WinForms centers a button's image + text as one
    group, so the 56 px left padding pushed every caption to the middle of the button. The nav
    buttons now use Padding 16 px with ImageAlign MiddleLeft and ImageBeforeText (set in code with
    the other runtime styling), so the icons line up in one column and the captions start right
    after them.
- Step 5 (this task): previews are never cropped and the sidebar icons got their gap.
  - PROJECT_RULES.md got the "Additions (Step 3) - Screen size" section (16:9 monitors, 1280x720
    logical minimum, content area about 1240 x 600, dialogs at most 1000 x 640, previews are
    rendered off screen so the monitor must not matter).
  - Helpers\PreviewTool.cs: the preview now always shows the whole window.
    * The thread asks Windows for "unaware" DPI (SetThreadDpiAwarenessContext), so a 150% monitor
      no longer lays the screens out 50% bigger. Previews are now the real logical sizes.
    * The window is shown off screen (Location -32000,-32000, no task bar button) and a form that
      maximizes itself (MainForm) is put back to its normal size before the picture, so the PNG is
      the whole window: LoginForm 932x604, MainForm 1296x759.
    * The PNG is drawn with Control.DrawToBitmap (whole window, title bar included). If that comes
      out blank (checked with a coarse color count) it falls back to copying the screen, and only
      then is the window shrunk to the working area, because a screen copy cannot be bigger.
    * The host form for a UserControl is now 1240 x 600 (the real content area). Usage
      (--preview <ClassName> [--role Admin|Staff]) and previews\<ClassName>.png are unchanged.
  - MainForm: SetNavIcon puts each 22 px icon on a transparent bitmap 10 px wider, so a visible gap
    sits between the icon and the caption (WinForms draws image and text with no space of its own,
    and widening the button padding moved both). Missing icon files still just leave a blank spot.
  - Forms\LoginForm.Designer.cs was rewritten for this, with every control name, TabIndex and event
    wiring kept: the login card container was restructured (new pnlCardCenter plus one small row
    panel per line) because a TableLayoutPanel with AutoSize = true kept squashing the rows - that is
    the real cause of the earlier "the two input boxes look different" problem (a 1 px high subtitle
    and a 16 px high field). Every row now has a fixed height, so the two input hosts are exactly
    340 x 34 and their text boxes exactly 322 x 18 (username) and 286 x 18 (password) at y = 7.
    The form is 916 x 558 client (932 x 604 window), keeps its 340 px card and 2:1 side photo.
  - The card is 426 px tall (sum of its fixed rows), so nothing is stretched and nothing is cut off.
- Standard clean build (bin and obj deleted first): 0 errors, 0 warnings.
  --smoketest: 29 passed, 0 failed, 0 skipped.
- Previews checked: LoginForm (both fields identical, logo and photo loaded, nothing cut off) and
  MainForm as Admin and as Staff (correct button order, visible icon/caption gap, header complete).

- Step 6 (this task): Layout only (Designer work) for the Services and Promotions module.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucServices (.cs + .Designer.cs): pnlToolbar (Dock Top, 56 px, Surface) with
    lblSearch / txtSearch / lblCategory / cboCategory (DropDownList) / lblStatus / cboStatus
    (All, Active, Archived) on the left, a right-aligned lblCount ("0 services", SmallFont,
    TextMuted) and btnAdd (primary) / btnEdit (secondary) / btnArchive (danger) / btnRefresh
    (secondary) on the right. Below it a card pnlGrid (Dock Fill, Surface, BorderStyle
    FixedSingle, 1 px padding) holds dgvServices (Dock Fill, Theme.StyleGrid look copied as
    literals, AutoGenerateColumns = false). Columns: colServiceId (hidden, service_id),
    colCategory, colService, colPrice (N2, right aligned), colDuration, colSessions, colStatus.
    Fill weights 20/30/14/16/10/14 make the price fit at 1240 px width.
  - Controls\ucPromotions (.cs + .Designer.cs): same toolbar, with btnToggle (danger, "Deactivate")
    instead of Archive, cboStatus items All, Active, Inactive, Expired and lblCount
    "0 promotions". Columns: colPromoId (hidden, promo_id), colName (fixed 220 px),
    colType / colValue (N2, right aligned) / colStart / colEnd (MMM dd, yyyy) / colStatus
    (fixed 90-120 px) and colConditions (DataPropertyName description, AutoSizeMode Fill, so it
    takes the remaining width). Fixed widths were chosen so the header "Promotion" is never cut.
  - Forms\ServiceEditForm (.cs + .Designer.cs): client 460 x 521 (the 480 x 560 in the prompt was
    shrunk to satisfy the "dialogs at most 1000 x 640" rule of Additions (Step 3) and to fit the
    content in one screen), FixedDialog, CenterParent, no minimize/maximize, AcceptButton btnSave,
    CancelButton + DialogResult.Cancel on btnCancel, errorProvider1. White header with
    lblTitle "Service" (HeadingFont) + 1 px Border line, AppBack content (cboCategory, txtName,
    txtDescription multi-line 90 px with vertical scrollbars, txtPrice, muted lblHint "For packages,
    enter the price of the whole package.", nudDuration 5-480 step 5 = 60, nudSessions 1-30 = 1,
    cboStatus Active/Archived) and a white footer with btnSave (primary) and btnCancel (secondary).
  - Forms\PromotionEditForm (.cs + .Designer.cs): client 460 x 545 (same dialog settings),
    lblTitle "Promotion", txtName, txtConditions multi-line 90 px, cboType Percentage/Fixed,
    txtValue, muted lblHint "Promos need full payment upfront.", dtpStart / dtpEnd (Format Short,
    side by side), cboStatus Active/Inactive, btnSave / btnCancel.
  - Layout fixes found in the previews: ucPromotions "Refresh" was cut at 72 px, so the toolbar
    buttons were re-spaced (Refresh 72 px, Deactivate 84 px, Add Promotion 116 px) so nothing is
    truncated; the Services toolbar was re-spaced the same way (button block and lblCount moved
    left, search/category/status fields slightly narrower) so the count label no longer collides
    with the Add button; both dialogs were re-laid out to 460 px client width.
  - LittleEreseSystemV3.csproj was NOT edited (every file already existed and was listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 29 passed, 0 failed, 0 skipped.
  --preview checked for ucServices, ucPromotions, ServiceEditForm, PromotionEditForm (nothing
  overlaps, no truncated text, no misaligned control).

- Step 7 (this task): the LOGIC for the Services and Promotions module (code-behind + repositories).
  Designer layout untouched; no small Designer fix was needed.
  - Data\ServiceRepository.cs (new content): GetCategories(), GetServices(search, categoryId, status)
    (categoryId 0 = all, status "All" = all, search matches service OR category name; returns
    service_id, category_name, service_name, description, price, duration_minutes,
    default_total_sessions, status), GetById(id) (DataRow or null), GetActiveForBooking(),
    ServiceNameExists(name, categoryId, excludeId), Add(...) (returns the new service_id),
    Update(...), SetStatus(id, status). A service name is unique INSIDE its category; the check
    runs in the form and again in Add/Update (a friendly InvalidOperationException). Services are
    never deleted.
  - Data\PromotionRepository.cs (new content): RefreshExpired() (Active + end_date < today ->
    'Expired'), GetPromotions(search, status), GetById(id), GetActiveValid() (Active and today
    between start_date and end_date, for Billing / Appointments later), PromoNameExists(...),
    Add(...) (returns the new promo_id), Update(...), SetStatus(id, status). Promo names are unique
    (checked in the form and again in Add/Update). Promotions are never deleted.
  - Helpers\Validators.cs: added TryMoney (money parse used by the forms) and IsLengthBetween
    (trimmed length, e.g. service/promo names 2..100) and IsValidPercentage (> 0 and <= 100).
    No existing rule was changed.
  - Controls\ucServices.cs: Load styles the grid + buttons with Theme, hides btnAdd / btnEdit /
    btnArchive for a Staff user (view only), fills cboCategory with "All categories" + the real
    categories and starts cboStatus on "Active". Search and both filters reload (guarded by a
    _loading flag so filling the combos does not double-load). Add / Edit open ServiceEditForm
    (EditId set before ShowDialog) and reload with the saved row selected. Double-click = Edit
    (Admin only). btnArchive flips between "Archive" (Active rows) and "Restore" (Archived rows)
    via CellFormatting-free SelectionChanged, confirms with Messages.Confirm, calls SetStatus and
    logs "Add" / "Edit" / "Archive" / "Restore" with the service name through
    ActivityRepository.Log. CellFormatting colors the Status cell with Theme.StatusColor and
    appends " min" to the duration.
  - Controls\ucPromotions.cs: Staff see only GetActiveValid() and the status filter + all action
    buttons are hidden. Admin gets the full list; Load and Refresh call RefreshExpired() first.
    btnToggle reads "Deactivate" (Active), "Activate" (Inactive) and is disabled for Expired rows.
    Add / Edit open PromotionEditForm; CellFormatting colors the status and shows the value as
    "15 %" for Percentage or as money for Fixed. Every Add / Edit / Activate / Deactivate is logged.
  - Forms\ServiceEditForm.cs / PromotionEditForm.cs: both keep the parameterless constructor and
    expose EditId (0 = new) plus SavedServiceId / SavedPromoId (the caller selects that row after
    saving). Load styles the buttons, fills the category list / defaults, and for EditId > 0 loads
    the row and retitles to "Edit Service" / "Edit Promotion". Save validates with Validators +
    errorProvider1 (category + name 2..100 + unique, price money > 0; promo: name + type + value > 0,
    percentage <= 100, end >= start, and a NEW promo may not end in the past), then Add or Update
    in a try/catch, sets DialogResult.OK and closes. Cancel returns Cancel.
  - Helpers\SmokeTest.cs: three read-only checks (GetCategories = 10 rows,
    GetServices("", 0, "Active") = 77 rows, GetPromotions("", "All") runs without error).
  - LittleEreseSystemV3.csproj was NOT edited (all files already listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 32 passed, 0 failed, 0 skipped (the 3 new repository checks all PASS).
  --preview checked for ucServices (grid + filters populated, count "77 services"), ucPromotions
  (empty until a promo is added - no promo seed data exists yet), ServiceEditForm (category "Botox"
  loaded, hint text visible) and PromotionEditForm (dates default to today / +1 month).

- Step 8 (this task): Layout only (Designer work) for the Staff Accounts module and Settings.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucStaffAccounts (.cs + .Designer.cs): same page look as ucServices. pnlToolbar
    (Dock Top, 56 px, Surface) with lblSearch / txtSearch (140 px) / lblStatus / cboStatus
    (All, Active, Inactive) on the left, lblCount ("0 staff accounts", SmallFont, TextMuted)
    right after them and btnAdd (primary, "Add Staff") / btnEdit (secondary) / btnToggleStatus
    (danger, "Deactivate") / btnResetPassword (secondary, 120 px) / btnRefresh (secondary, 72 px)
    right aligned. Below it the card pnlGrid (Dock Fill, Surface, FixedSingle, 1 px padding) holds
    dgvStaff (Dock Fill, Theme.StyleGrid look copied as literals, AutoGenerateColumns = false).
    Columns: colUserId (hidden, user_id), colFullName (fill 26, full_name), colUsername (fill 18,
    username), colRole (fill 10, role), colContact (fill 18, contact_number), colStatus (fill 12,
    status), colCreated (fill 16, created_at, format "MMM dd, yyyy"). The button block starts at
    x = 504 so lblCount never collides with it at 1240 px width (the same trick as ucServices).
  - Forms\StaffEditForm (.cs + .Designer.cs): FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnSave, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1.
    The prompt's 480 x 620 was shrunk to a 460 px client width so it matches ServiceEditForm /
    PromotionEditForm and the "dialogs at most 1000 x 640" rule. White header with lblTitle
    "Staff Account" (HeadingFont) + 1 px Border line, AppBack content and a white footer with
    btnSave (primary) and btnCancel (secondary). Fields: cboStatus (Active, Inactive),
    txtFullName, txtUsername, cboRole (Admin, Staff), txtContact, txtPassword and
    txtConfirmPassword (both UseSystemPasswordChar), muted lblPasswordHint "At least 8 characters
    with a letter and a number." under them, leaving some free space above the footer.
  - Controls\ucSettings (.cs + .Designer.cs): a centered 640 px column on AppBack, built with a
    3-column tlpRoot (50% / 640 px / 50%) that holds a tlpColumn (one row per card, 20 px side
    padding). Cards: pnlProfile (Surface, FixedSingle) with picAvatar 96x96 (SizeMode Zoom),
    lblFullName (HeadingFont size 12 semibold, TextDark), lblRole (Primary), lblUsername and
    lblContact (TextMuted); grpPassword (Surface, caption "Change Password") with lblCurrent /
    txtCurrentPassword, lblNew / txtNewPassword, lblConfirmNew / txtConfirmNewPassword (all
    UseSystemPasswordChar), muted lblPasswordHint and btnChangePassword (primary look, 180 px)
    at the bottom right; pnlAbout (Surface, FixedSingle) with lblAppName "Little Erese's
    Appointment and Billing System", lblAppVersion "Version" and lblDatabase "Database" (both
    filled at runtime by the logic prompt). errorProvider1 sits on the UserControl.
    POLISH: the version and database strings are placeholder text until the logic prompt.
  - Layout fixes found in the previews: StaffEditForm had too much empty space under the title
    (the first draft put the rows at y >= 124), so the fields were moved up to a 22 px row rhythm
    starting at y = 60 (cboStatus) - every label/field pair is 22 px apart and the hint line sits
    12 px under the last field, so nothing overlaps or is cut.
  - LittleEreseSystemV3.csproj was NOT edited (every file already existed and was listed).
- Standard clean build (bin, obj and .vs\iolys deleted first): 0 errors, 0 warnings.
  --smoketest: 32 passed, 0 failed, 0 skipped.
  --preview checked for ucStaffAccounts (toolbar fits, all five buttons readable, grid columns
  visible), StaffEditForm (no overlap, hint visible, footer complete) and ucSettings (640 px
  column centered, three cards, nothing cut off). The app was started once and closed again.

## Next
- Step 9: the LOGIC for Staff Accounts (UserRepository list/create/update/status/password reset)
  and Settings (own password change), then the Dashboard screen (ucDashboard with ucCalendarMonth)
  and the remaining modules (Appointments, Clients, Billing, Commissions, Reports,
  File Maintenance, Activity Log). ucStaffAccounts, StaffEditForm and ucSettings are still
  layout only, so opening those pages shows the shell with an empty grid and placeholder texts.
- POLISH items left for the owner: the lock out limits (5 tries / 30 seconds) in LoginForm and
  the "Log out now?" wording.

## Known issues
- None blocking. The page UserControls are still empty shells, so switching pages shows an empty
  content area (this is expected until each page is built).
- The Promotions list is empty at first because the seed data has no Promotions rows; add one with
  "Add Promotion" to see the grid, the status colors and the "15 %" / money formatting. POLISH:
  the owner may want 2-3 sample promos in Database\SeedData.sql.
- Demo staff accounts still have the placeholder names "Aesthetician 1".."Nurse 1" (the schema
  has no job-title column) - rename them in Staff Accounts when it is built.
- The smoke test only constructs the forms (no Show), so it does not cover the new event handlers;
  they were checked with --preview and by starting the app.
