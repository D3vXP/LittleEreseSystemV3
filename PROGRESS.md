# PROGRESS.md

Update this file at the end of every task.

- Step 37 (this task): LAYOUT ONLY of ucPromotions. The grid columns are now
  colPromoId (hidden, promo_id) / colName "Promotion" (promo_name) / colServices "Services"
  (services_text, Fill) / colRegularTotal "Regular price" (regular_total, money) /
  colPromoPrice "Promo price" (promo_price_text) / colStart "Start" / colEnd "End"
  (MMM dd, yyyy) / colStatus "Status". The old colAppliesTo ("Applies to"), colConditions
  ("Conditions"), colType ("Type") and colValue ("Value") columns were removed together with
  their Designer lines. Money.ApplyTo(colRegularTotal) was added in Load. PromotionRepository
  was NOT touched, so services_text / regular_total / promo_price_text are filled by the next
  prompt (S2-C3) and those cells stay empty for now.
  Commented out with "// TODO(logic prompt)" in Controls\ucPromotions.cs:
  the type / value / percentage reads and the discount sentence of UpdateSummary(), and the
  colValue branch (percentage wording) of dgvPromotions_CellFormatting. The summary strip now
  shows the description of the selected promotion plus the names of the services it covers; the
  SAVING is still missing because it needs promo_price, which PromotionRepository does not return
  yet (S2-C3). regularTotal is already summed from GetPickList for that step.
  PROJECT_RULES.md was NOT edited (no business rule or value list changed). The .csproj was NOT
  edited (no new file). Standard clean build 0 errors / 0 warnings, DesignerAudit 22 files exit 0,
  --preview ucPromotions: the 7 headers read Promotion / Services / Regular price / Promo price /
  Start / End / Status with no overlap and no empty column wrapper, 0 fix rounds.
- Step 36 (this task): LAYOUT ONLY of PromotionEditForm. The dialog now holds only the name, the
  description, the services covered, the promotional price, the two dates, the status and the price
  preview. No repository, validation or business rule was touched; the parts that cannot compile
  without the removed controls are commented out with "// TODO(logic prompt)" and listed below.
  - Forms\PromotionEditForm.Designer.cs: lblConditions now reads "Description" (same multiline box,
    same name and TabIndex). lblType / cboType / lblValue / txtValue are gone and were replaced by
    lblPromoPrice "Promotional price" + txtPromoPrice in the same two grid rows (tlpLeft shrank from
    16 to 14 rows, and the rows below moved up one slot). chkApplyAll is gone from tlpServices (5 to
    4 rows), the pick grid grew from 111 to 146 px and the search/category rows moved up to start at
    the top of the card. In the preview grid colPrevPromo and colPrevSaving are gone, so it holds
    Service + Regular price only, and its total row is now lblRegularTitle "Regular price of the
    services" / lblRegularTotal plus lblSaving "Customers save" / lblSavingAmount (the old
    lblDropTitle / lblDropTotal / lblPromoTitle / lblPromoTotal were removed). lblHint "Promos need
    full payment upfront." is unchanged. Visual order equals the new TabIndex order (0..12 on the
    left column, then the services card 13..18, then Save 0 / Cancel 1 in the footer).
  - Forms\PromotionEditForm.cs (only what the removed controls forced):
    txtPromoPrice_TextChanged was added as an EMPTY handler with "// TODO(logic prompt)";
    chkApplyAll_CheckedChanged, cboType_SelectedIndexChanged and txtValue_TextChanged were deleted
    together with their Designer wiring.
    Commented out with "// TODO(logic prompt)" because they used the removed controls:
    Money.ApplyTo(colPrevPromo) and Money.ApplyTo(colPrevSaving) in Load; cboType.SelectedIndex = 0
    in Load; the old chkApplyAll.Checked default in Load; cboType.SelectedItem / txtValue.Text in
    LoadPromotion; the body of ApplyApplyAllState() and the chkApplyAll guard in
    UpdateSelectedCount(); the applyAll/type/value block and the "applies to every service on the
    bill" branch in UpdatePreview(), plus lblDropTotal / lblPromoTotal; the four removed labels in
    ShowTotals(); the type/value read and the chkApplyAll link rule in btnSave_Click; validation
    rules 2 and 3 (discount type, value) in ValidateFields, where rule 4 now demands at least one
    ticked service.
    To keep the file compiling, UpdatePreview and btnSave_Click keep two PLAIN LOCALS
    (type = "Percentage", value = 0m and an empty-links list) marked "// TODO(logic prompt)" - they
    are placeholders for the logic prompt and produce no discount today.
  - PROJECT_RULES.md was NOT edited: this prompt changed no business rule or value list. The rules
    "Percentage / Fixed" and "No linked services = it applies to the whole bill" still describe the
    stored data of existing promotions, so they stay until the logic prompt replaces them.
  - LittleEreseSystemV3.csproj was NOT edited (no new file).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  tools\DesignerAudit.ps1: 22 files, no problems, exit 0. --preview PromotionEditForm: Promotion
  name / Description / Promotional price / the full-payment note / Start date / End date / Status on
  the left, the Services card (Search, Category, the tick grid, "0 services selected") and the Price
  preview (Service + Regular price, "Regular price of the services", "Customers save") on the right,
  no overlap and no truncated caption - no fix round was needed. The app was started once and
  closed; no error.log.
  - Known limits: the form is not functional for discounts until the logic prompt lands (saving
    writes discount_type 'Percentage' with value 0.00 and the ticked links). lblSavingAmount shows a
    plain "0.00" in the Designer, like every other money label there; Money.Text fills it at runtime.

- Step 35 (this task): the JOB TYPE of a staff account (Users.staff_type from migration 002) is now
  real: it is captured in StaffEditForm, shown in the Staff Accounts grid, and used to pick the
  technicians of a service category. AppointmentForm was deliberately NOT changed yet.
  - PART 1 - layout (no logic).
    Forms\StaffEditForm.Designer.cs: lblStaffType "Job type" + cboStaffType (DropDownList:
    Aesthetician, Massage Therapist, Nurse, Other) sit directly under the Role row. Adding one row
    needed the fields below to move up 22 px (Contact 236/258, Password 292/314, Confirm 348/370,
    hint 403) and the Status row to 430/452, so the dialog keeps its 460 x 620 size instead of
    growing. lblFullName also moved from y 16 to 14 and the two rows above Role were tightened
    (Username 68/90, Role 124/146), so there is no overlap and the visual order equals the
    TabIndex order: Full name 0, Username 2, Role 4, Job type 6, Contact 8, Password 10, Confirm
    12, hint 14, Status 15/16.
    Controls\ucStaffAccounts.Designer.cs: new colStaffType "Job type" / staff_type right after
    colRole (Fill, FillWeight 14; Role / Status / Created were trimmed to 10 / 12 / 16 so the seven
    visible columns still share the width at 1240 px).
  - PART 2 - logic.
    Data\UserRepository.cs: GetUsers and GetById also return staff_type. Create and Update take a
    staffType parameter and pass it through the new NormalizeStaffType(role, staffType): an Admin
    always gets NULL, a Staff account with an empty job type throws
    InvalidOperationException(Validators.StaffTypeRequiredMessage) before any SQL runs.
    New GetActiveStaffFor(categoryName): user_id, full_name of Active Staff whose staff_type is
    the category's type OR 'Other' OR NULL - a category whose name starts with "Massage" wants
    'Massage Therapist', everything else wants 'Aesthetician' (the prefix comes from
    AppointmentRepository.MassageCategoryPrefix, so one place owns it). POLISH comment in the code:
    nurses never appear there. New GetActiveNurses(): Active Staff with staff_type 'Nurse'.
    Helpers\Validators.cs: StaffTypeRequiredMessage = "Choose the job type." (the one sentence the
    form shows and the repository throws).
    Forms\StaffEditForm.cs: Load selects the stored job type (first entry when there is none);
    cboRole_SelectedIndexChanged calls ApplyRoleToStaffType, which disables and empties the combo
    while the role is Admin and re-enables it for Staff. ValidateFields stops with the message on
    cboStaffType when the combo is enabled but nothing is picked, and SelectedStaffType() sends
    null for an Admin. PasswordOnly mode also locks the two new controls.
    Data\DemoDataSeeder.cs: EnsureStaff now takes the job type (Aesthetician 1-4 ->
    'Aesthetician', Massage Therapist 1-2 -> 'Massage Therapist', Nurse 1 -> 'Nurse') and, for an
    account that already exists with staff_type IS NULL, writes it in place. That matters on a
    fresh clone, where the migration runs BEFORE the seeder.
    Helpers\SmokeTest.cs: three read-only checks - GetActiveStaffFor("Botox") >= 1 (got 4),
    GetActiveStaffFor("Massage - Feet") >= 1 (got 2), GetActiveNurses() >= 1 (got 1).
    PROJECT_RULES.md: the "Rooms / technicians" business rule now says technicians are Active Staff
    filtered by staff_type through GetActiveStaffFor, that 'Other' and NULL appear in every list,
    that a nurse is NEVER a technician (GetActiveNurses), and that a Staff account always has a job
    type while an Admin has none.
    LittleEreseSystemV3.csproj was NOT edited (no new file).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 70 passed, 0 failed, 0 skipped (the three new job-type checks PASS).
  tools\DesignerAudit.ps1: 22 files, no problems, exit 0. --preview StaffEditForm: Full name,
  Username, Role, Job type (Aesthetician), Contact number, Password, Confirm password, the hint and
  Status - nothing overlapping or truncated, no fix round needed. --preview ucStaffAccounts: the
  new "Job type" column after Role shows Aesthetician / Massage Therapist / Nurse and is empty for
  the System Administrator row (an Admin has no job type, by design). The app was started normally
  and closed; no error.log.
  - Known limits / assumptions: the job type of an existing account is stored in ONE column, so a
  staff member who does both jobs must pick 'Other' (which shows them in every list). AppointmentForm
  still uses GetActiveStaff() (every Active Staff) - switching it to GetActiveStaffFor(service
  category) is the next prompt. The demo seeder only fills staff_type when it is NULL, so a job type
  the owner changed by hand is never overwritten.

- Step 34 (this task): the SCHEMA of tester feedback round 1, applied to existing databases by the
  migration runner from Step 30. No screen, repository or business logic was changed.
  - Database\Migrations\002_feedback_round1_schema.sql (NEW FILE, exactly the content the prompt
    gave, written in three pieces): (1) Users.staff_type VARCHAR(30) NULL + CK_Users_StaffType
    (Aesthetician / Massage Therapist / Nurse / Other) + a one-time backfill from the full_name
    prefixes for role = 'Staff'; (2) Services.staff_busy_minutes (CK_Services_StaffBusy > 0) and
    Services.session_interval_days INT NOT NULL with DF_Services_Interval = 7 and
    CK_Services_Interval >= 1; (3) AppointmentServices.start_time / end_time / busy_end_time (TIME
    NULL) and assessed_by INT NULL with FK_AS_AssessedBy -> Users, then a CTE that fills the three
    times of every existing line by running the service durations from the appointment's own
    start_time (47 lines were filled on the shop database); (4) Promotions.promo_price
    DECIMAL(10,2) NULL + CK_Promotions_PromoPrice, the OLD autogenerated discount_type constraint
    is dropped (found by its definition, never by a hard-coded name) and CK_Promotions_DiscountType
    is added with the third value 'Promo Price'; (5) Clients.health_conditions VARCHAR(MAX) NULL.
    Every batch is guarded with IF COL_LENGTH / IF NOT EXISTS, every GO stays on its own line, so
    the file is safe to run twice. It contains no DROP TABLE / TRUNCATE / DELETE - the only DROP is
    the one check constraint on discount_type that has to be replaced to allow the third value, and
    the prompt asked for exactly that.
  - LittleEreseSystemV3.csproj was NOT edited: the wildcard Content item from Step 30
    (Database\Migrations\*.sql, CopyToOutputDirectory PreserveNewest) already copied 002 to
    bin\Debug\Database\Migrations\, so the runner picked it up.
  - Helpers\SmokeTest.cs: STEP 0 was already satisfied (CheckMaintenanceTableCounts has used
    counts.Rows.Count >= MinimumTables since Step 30 - it is "at least" already, no ">" was left).
    One new read-only check, CheckMigration002Columns: "Migration 002 columns exist = " with
    CheckScalarAtLeast 9, counting the nine (table, column) pairs through COL_LENGTH so a missing
    ALTER is found without touching a row.
  - PROJECT_RULES.md (same prompt): the value list line now has "Users.staff_type: Aesthetician |
    Massage Therapist | Nurse | Other (NULL for Admin)" and Promotions.discount_type includes
    'Promo Price'; two new business rules - AppointmentServices.start_time / end_time /
    busy_end_time belong to ONE service line and a NULL means "use the appointment's own start and
    end" (old rows, demo rows), and "the first payment is 'Downpayment', every later payment (even
    a partial one) is 'Final'".
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  The app was started ONCE: 002 ran and SchemaVersion holds 001_runner_check.sql (applied
  2026-10-27 10:07) and 002_feedback_round1_schema.sql (applied 2026-10-27 11:17). A SECOND start
  did not add a row - both scripts are still listed exactly ONCE with the same applied_at, so the
  runner is idempotent and the shop data was not touched. Backfill proof: staff_type is
  Aesthetician 4 / Massage Therapist 2 / Nurse 1 / NULL 1 (the admin), and 47 service lines now
  have their times. --smoketest: 67 passed, 0 failed, 0 skipped (the new "Migration 002 columns
  exist = 9" PASS, "Every migration file is listed in SchemaVersion (files on disk = 2, missing =
  0)" PASS). tools\DesignerAudit.ps1: 22 files, no problems, exit 0. The app was started normally
  twice and closed; no error.log.
  - Known limits / assumptions: the times backfill only fills lines whose start_time IS NULL, so a
    second run changes nothing; it assumes each line runs back to back inside its appointment, in
    appointment_service_id order (the same order the code uses). The discount_type constraint had
    to be replaced, not extended, because SQL Server has no "alter check" - the old one was found
    by its definition, so no hard-coded constraint name is needed. A fresh database still gets the
    baseline 18 tables first and then 001 + 002, exactly like an existing one.

- Step 33 (this task): two business rules - a MINIMUM CLIENT AGE of 15 and CANCELLED (and
  rescheduled) APPOINTMENTS DISAPPEARING from the working lists. No screen was redesigned.
  - PART 1 - minimum age 15. Helpers\Validators.cs (the one place for input rules) now holds
    MinimumClientAge = 15, MinimumClientAgeMessage ("For safety reasons, we do not accept clients
    under 15 years old."), AgeOn(birth, day) (whole years, counting the birthday with AddYears, not
    days / 365) and IsAtLeastAge(birth, minimum, day). The file also needed a new "using System"
    because DateTime was not in scope there.
  - Forms\ClientEditForm.cs: the birth date is REQUIRED now - an unticked picker shows
    "Birth date is required." on dtpBirthDate - and a client younger than 15 gets the sentence above
    from the same Validators constant. The old "optional birth date" block became
    required -> not in the future -> not older than 120 -> at least 15. UpdateAge() uses
    Validators.AgeOn, so the "Age: N" label and the rule can never disagree.
  - Data\ClientRepository.cs: Add(...) and Update(...) both call the new internal
    EnsureClientIsOldEnough(birthDate) BEFORE their SQL, which throws InvalidOperationException with
    those two sentences. That is the save method of the client class, so no other code path can write
    a client the shop does not accept. Existing clients are not touched or deleted.
    EnsureClientIsOldEnough is internal (not private) only so the smoke test can call the accepted
    case without writing a row.
  - Helpers\SmokeTest.cs: CheckClientMinimumAge with two checks - "Client age: 14 years old today is
    rejected" (calls ClientRepository.Add with today - 14 years; the guard throws BEFORE the INSERT,
    so nothing is written and nothing has to be cleaned up, which keeps the smoke test read-only) and
    "Client age: exactly 15 today is accepted" (calls the guard directly, so this one writes nothing
    at all).
  - PART 2 - cancelled appointments. Controls\ucAppointments.Designer.cs: "Active" is the FIRST item
    of cboStatus (before "All", "Upcoming", "Ongoing", "Completed", "Cancelled"). The Designer file
    only gained that one string; the look and the position of the box are unchanged.
    Controls\ucAppointments.cs: Load selects index 0 ("Active") and LoadAppointments falls back to
    "Active" when nothing is picked.
    Data\AppointmentRepository.GetByDate: "Active" means status NOT IN ('Cancelled','Rescheduled'),
    "All" still means every status and any other value is one exact status (one parameterised
    condition, no SQL built from the screen).
    Controls\ucDashboard.cs: both ShowTodayGrid and ShowSelectedDay now ask for "Active", so the two
    grids AND their headings stop counting cancelled and rescheduled visits. Billing
    (BillingRepository.GetQueue) and the capacity counts were NOT changed - they already skip
    cancelled ones.
  - ReportRepository.GetDashboardStats was checked and NOT changed: today_total already excludes
    'Cancelled' but STILL COUNTS 'Rescheduled' (remaining / completed / today_sales / active_packages
    do not count cancelled ones). Say the word if today_total must exclude Rescheduled too.
  - PROJECT_RULES.md: two new business rules - "Minimum age" and "Cancelled and rescheduled
    appointments are not work".
  - LittleEreseSystemV3.csproj was NOT edited (no new file).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 66 passed, 0 failed, 0 skipped (the two new age checks PASS; the age message compared
  is the exact sentence). tools\DesignerAudit.ps1: 22 files, no problems, exit 0.
  --preview ucAppointments: the Status box reads "Active" on open, the day grid shows the one
  non-cancelled appointment (10:00 AM, Bautista, Jenny Lyn, Upcoming) and the toolbar, the details
  card and the details grid are unchanged - no fix round was needed. The app was started normally
  once and closed; no error.log.
  - Known limits / assumptions: the rule counts the age on DateTime.Today (the day of saving), so a
    client who turns 15 tomorrow cannot be saved today. Old rows with a NULL birth date stay in the
    database and still open in ClientEditForm, but the form will not save them again until a birth
    date is entered (the picker is required) - that is the point of the rule, not a bug.
    GetByDate("Active") is the default of ONE screen only; the Appointments page still lets the user
    pick "All" or "Cancelled" on purpose, because that is the only place a cancelled visit is shown.

- Step 32 (this task): ONE money formatter for the whole app, so no screen can show 5,000.00
  while another shows the peso sign.
  - Helpers\Money.cs (NEW FILE, registered in LittleEreseSystemV3.csproj): internal static class
    Money with Culture = CultureInfo.GetCultureInfo("en-PH"), Format = "C2" (culture, two
    decimals, thousands separator), public static string Text(decimal) for labels / messages /
    the receipt, and public static void ApplyTo(DataGridViewColumn) which sets the column's
    DefaultCellStyle.Format to "C2" and its FormatProvider to the en-PH culture. This one class
    replaces the six private MoneyFormat constants and the four private Money() helpers that
    were copied into page after page.
  - Grids: ucAppointments (colDetailPrice), ucBilling (colQueueBalance, colLinePrice,
    colTranTotal, colTranPaid), ucCommissions (colSumTotal, colSumUnpaid, colSumPaid, colDetBase,
    colDetAmount - colDetRate stays a plain number because it is a percentage), ucReports
    (colSalesGross, colSalesDiscount, colSalesNet, colMethodAmount, colTopRevenue, colRepTotal,
    colRepUnpaid, colRepPaid), ucServices (colPrice - it showed 5,000.00 without the sign),
    AppointmentForm (colCatPrice, colSelPrice), PromotionEditForm (colPrevRegular, colPrevPromo,
    colPrevSaving, colPickPrice). Every one of them now shows the peso sign. ucPromotions
    colValue stays in its CellFormatting because a percentage promotion must read "15 %" and a
    fixed one Money.Text(value).
  - Labels and messages: the bill totals (ucBilling ShowTotals / ClearBill / the change line and
    the transactions total), the promo captions (ucBilling, AppointmentForm), the appointment
    payment summary and its tooltip text, the three commission cards, the payout question and its
    activity-log line, "Net sales" / "Total commission" in ucReports, the Dashboard sales card,
    the promotion summary strips (ucPromotions, PromotionEditForm), the PackageForm combo and
    price label, PaymentForm (amount due, change, the "at least" message) and the repository
    sentences of AppointmentRepository / BillingRepository all use Money.Text now.
  - Helpers\ReceiptPrinter.cs: the line detail, the line amount, Subtotal, Discount, Paid upfront
    and Total are printed with Money.Text, so a printed receipt and the Billing screen can never
    disagree.
  - The three "MB" texts in ucFileMaintenance were deliberately NOT touched: a file size is not
    money.
  - PROJECT_RULES.md: the business line now says every amount is formatted by Helpers\Money
    (en-PH, peso sign, two decimals, thousands separator) and never by a private format constant
    in a screen.
  - LittleEreseSystemV3.csproj WAS edited (one line added): <Compile Include="Helpers\Money.cs" />.
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 64 passed, 0 failed, 0 skipped. tools\DesignerAudit.ps1: 22 files, no problems,
  exit 0 (no Designer file was touched: the runtime format override is the documented way).
  The app was started normally and stayed open, then closed; no error.log.
  The four previews were looked at again: ucBilling (Balance 75.00, Subtotal / Discount / Paid so
  far / Total / Balance due), ucServices (the Price column, which showed 5,000.00 before),
  ucPromotions (the fixed promo value), AppointmentForm (both Price columns) - every amount now
  reads with the peso sign and no other prefix. ucReports and ucCommissions were checked too
  (Net sales / the three cards / all eight money columns).
  - Known limits / assumptions: Money.Text always uses the en-PH culture, so the amounts are the
    same whatever Windows language the shop's computer uses. "C2" already rounds to two decimals,
    so no screen rounds an amount itself. A new money column still needs one Money.ApplyTo(...)
    line in the page's Load event, because the Designer file must stay exactly what the Visual
    Studio Designer generates.
  - One thing to look at (NOT changed, it is text and not a formatted amount): the demo promotion
    "Beauty Bucket 500" has the description "PHP 500 off the whole bill." in
    Data\DemoDataSeeder.cs, so the Conditions column of ucPromotions shows the letters "PHP".
    Editing that sentence is the owner's call.

- Step 31 (this task): GROUPED SIDEBAR with category headings. No page, icon, caption, shortcut or
  role rule was changed - only the order and the grouping of the menu.
  - Forms\MainForm.Designer.cs: three new Labels in pnlNav - lblNavStaffManagement
    ("STAFF MANAGEMENT"), lblNavOperations ("OPERATIONS") and lblNavFiles ("FILES"). Each one is
    Dock Top, Height 26, SmallFont (Segoe UI 8.5 pt) with the Theme.TextMuted colour as a literal
    (122,114,102), Padding 16/6/0/0 (16 px left so the words line up with the button captions, 6 px
    top so the gap above the heading is visible) and SidebarBack as BackgroundColor. A Label cannot be
    clicked and is not in the NavButtons() list, so it is never highlighted or "selected".
    Every nav button went from 48 px to 36 px high (11 buttons) and the Dock Top z-order was
    rebuilt: Dashboard, STAFF MANAGEMENT, Staff Accounts, Commissions, OPERATIONS, Appointments,
    Billing, Clients, Services, Promotions, Reports, FILES, File Maintenance, Activity Log.
    Locations follow that order: 0, 36, 62, 98, 124, 160, 196, 232, 268, 304, 340, 376, 402, 438
    (each row is the height of the control above it, 36 for a button and 26 for a heading).
    TabIndex still follows reading order.
    Settings was NOT a sidebar item and stays in the header (it was never in pnlNav).
  - Forms\MainForm.cs: ApplyRoleVisibility() also hides lblNavStaffManagement and lblNavFiles for
    Staff, because those groups hold only owner screens and a heading with no button under it is a
    dead label. SetSidebarCollapsed() shows the headings only while the sidebar is expanded AND the
    user is an Admin (a 68 px icon strip has no room for words). Nothing else changed: page keys,
    CreatePage, PageTitle, IsAdminPage, HighlightNavButton, the icons and the role checks are the
    same code as before.
  - Fit rule: at 1280 x 720 the window is 720 px, pnlBrand 90 + lblVersion 28 + a 1 px header line
    leave pnlNav 602 px; the Admin menu is 14 rows = 474 px, so it fits with about 128 px to spare
    and NO scrolling is needed. pnlNav.AutoScroll was therefore left false (the rule allows making
    it vertical-only AutoScroll if it ever stops fitting).
  - PROJECT_RULES.md: new "Additions (Step 31) - the sidebar" section (the group order, the two
    headings Staff must not see, the fact that a heading is a Label and never clickable, the 36/26
    px sizes with the numbers behind the 1280 x 720 fit, and the AutoScroll fallback rule).
  - LittleEreseSystemV3.csproj was NOT edited (no new file; both MainForm files were already
    listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 64 passed, 0 failed, 0 skipped. tools\DesignerAudit.ps1: 22 files, no problems,
  exit 0. --preview MainForm (Admin): Dashboard alone on top, then STAFF MANAGEMENT / Staff
  Accounts / Commissions, OPERATIONS / Appointments / Billing / Clients / Services / Promotions /
  Reports, FILES / File Maintenance / Activity Log - every caption complete, nothing overlapping,
  the whole menu visible without a scrollbar. --preview MainForm --role Staff: Dashboard, then
  OPERATIONS with its six pages only; STAFF MANAGEMENT, FILES and Today's Sales card are all gone.
  No fix round was needed (no overlap, no truncated text). The app was started normally once and
  closed; no error.log.
  - Known limits / assumptions: the headings are 8.5 pt muted grey on the dark sidebar on purpose
    (POLISH: raise them to 9 pt or Theme.SidebarText if the owner wants them louder). The heading
    text is upper case in the Designer, so it is not localised by code. A future page must be added
    to the right group by hand in three places (Designer z-order, NavButtons(), NavButtonFor()).

- Step 30 (this task): UPGRADING EXISTING DATABASES - numbered migration files that are applied
  automatically at startup, so a tester's LittleEresesDB gets new columns and tables without
  anything being deleted.
  - Data\DatabaseBootstrapper.cs: new public ApplyMigrations(), plus four small private helpers
    (EnsureSchemaVersionTable, MigrationApplied, RunMigration, and the constant MigrationsFolder).
    It returns at once when Database\Migrations is not next to the exe, otherwise it creates
    dbo.SchemaVersion (script_name nvarchar(200) NOT NULL PRIMARY KEY, applied_at datetime2 NOT
    NULL DEFAULT SYSUTCDATETIME()) when OBJECT_ID says it is missing, then reads the *.sql files of
    that folder sorted by file name and skips every file already listed. A new file is split with
    the EXISTING SplitOnGo (so USE / CREATE DATABASE / the DB_ID guard lines are dropped by
    IsDatabaseLevelLine, exactly like the baseline script - no second splitter was written) and all
    of its batches run in ONE transaction together with the INSERT of its name into SchemaVersion.
    On any error that file is rolled back, the loop stops and the caller gets
    "Database update failed in <file>: <reason>" (SQL errors go through DescribeBootError, so
    LocalDB / login problems keep their plain sentences). Nothing drops or truncates anything.
    EnsureDatabase() is unchanged.
  - Program.cs: DatabaseBootstrapper.ApplyMigrations() is called on the line after EnsureDatabase(),
    inside the same try/catch (a failed update shows the message and the app stops, instead of
    running against a half-updated database). --smoketest and --preview still return before it.
  - Database\Migrations\001_runner_check.sql (NEW FILE): a comment plus SELECT 1 - it changes
    nothing and only proves that the runner finds the folder, splits on GO, runs the file in one
    transaction and records it.
  - LittleEreseSystemV3.csproj WAS edited (one item added, nothing else):
    <Content Include="Database\Migrations\*.sql"> with
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>, so every migration file,
    including the ones added later, is copied to bin\Debug\Database\Migrations\ automatically.
    The existing entries (LittleEreses_Database.sql, SeedData.sql, ResetDemoData.sql,
    Database\Migration_01_PromotionServices.sql as None) are untouched.
  - Helpers\SmokeTest.cs: ExpectedTables was renamed MinimumTables (18) and is now a MINIMUM,
    because a later migration may add another table. "Base tables" uses CheckScalarAtLeast with the
    label "Base tables = <n> (expected at least 18)", CheckMaintenanceTableCounts accepts
    counts.Rows.Count >= MinimumTables, and the now unused CheckCount helper was deleted. Two new
    read-only checks: "SchemaVersion exists" (OBJECT_ID) and "Every migration file is listed in
    SchemaVersion" (at-least style - it lists the files on disk that are not recorded and prints
    their names). The smoke test NEVER applies a migration, so the app must be started once before
    --smoketest or those two checks fail.
  - Data\MaintenanceRepository.GetTableCounts was deliberately NOT changed: SchemaVersion is a real
    table and may show on the File Maintenance page.
  - PROJECT_RULES.md: line 3 now also says to read Database/Migrations/*.sql; the SOURCE OF TRUTH
    line became "LittleEreses_Database.sql is the BASELINE (18 tables) and is never edited again.
    Every later schema change is a new numbered file in Database/Migrations, safe to run twice
    (IF COL_LENGTH / IF OBJECT_ID / IF NOT EXISTS), with no DROP and no data loss. Only a prompt
    that says so may add a migration." and the destructive-SQL line became "The baseline script
    runs only if the database does not exist; migrations run automatically at startup."
  - README.md: a short "Upgrading a database you already have" paragraph in Setup, the folder map
    now lists Database\Migrations\, and Database\Migration_01_PromotionServices.sql is described as
    only for a database made before the baseline had that table (that file itself was not touched).
  - Verification: (a) on the real shop database the app was started once - SchemaVersion appeared
    with 001_runner_check.sql and a second start did NOT duplicate the row (same applied_at,
    COUNT = 1), so existing data is untouched and the runner is idempotent; (b) on a THROW-AWAY copy
    of bin\Debug in %TEMP% (its .exe.config pointed at LittleEresesMigTest) a deliberately broken
    002_bad_test.sql was added: the app created the database, applied 001, then rolled 002 back
    completely - SchemaVersion held only 001_runner_check.sql and the dbo.RollbackProof table that
    its first batch had created was GONE, which proves the whole file runs in one transaction and
    that the error message is shown instead of a crash. The scratch database and folder were
    dropped and deleted afterwards; the shop database never saw the test file.
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest (after one normal start, as the two new checks require): 64 passed, 0 failed,
  0 skipped - "Base tables = 19 (expected at least 18)", "MaintenanceRepository.GetTableCounts
  returns at least 18 tables (got 19)", "SchemaVersion exists", "Every migration file is listed in
  SchemaVersion (files on disk = 1, missing = 0)". tools\DesignerAudit.ps1: 22 files, no problems,
  exit 0. The app was started normally and stayed open, then closed; no error.log.
  - Known limits / assumptions: the migration runner never creates the database (EnsureDatabase
    does) and never touches an existing table by itself - only a numbered file does, and only a
    prompt that says so may add one. A migration runs on EVERY database, including a fresh one, so
    a new file must be safe on an empty database too (IF OBJECT_ID / IF COL_LENGTH guards). Only
    *.sql files directly inside Database\Migrations are read, in plain file-name order, so the
    numbers must be zero-padded (001, 002, ... 010) to keep that order. A file that was recorded
    once is never re-run, so a mistake must be fixed with a NEW file, not by editing an old one.
    --smoketest cannot apply migrations (it is read-only), so start the app once first.

- Step 29 (this task): the database is created and filled AUTOMATICALLY on the first start, so a
  fresh clone needs no manual SSMS work.
  - Data\DatabaseBootstrapper.cs (NEW FILE, registered in LittleEreseSystemV3.csproj):
    internal static DatabaseBootstrapper with one public method, EnsureDatabase(). It builds the
    same connection as App.config (same server, same security) but with InitialCatalog = "master",
    because a connection that names a missing database can never be opened. The database name is
    read from the connection string and checked against ^[A-Za-z0-9_]+$ before it is used in
    brackets. It opens master, asks SELECT DB_ID(@name) and, when the name is NOT NULL, returns at
    once - an existing database is never dropped, altered or seeded, not one table, not one row.
    When it is NULL it runs CREATE DATABASE [name] (CommandTimeout 120), then opens the normal
    connection and runs Database\LittleEreses_Database.sql followed by Database\SeedData.sql, both
    read from the Database folder next to the exe. Each file is split on lines that contain only GO;
    the database-level lines (USE, CREATE / ALTER / DROP DATABASE and the "IF DB_ID(...) IS NULL"
    guard in front of them) are skipped, because the connection already points at the right database
    and the class must never create or change another one. Empty batches are dropped. Every failure
    becomes an InvalidOperationException with a readable sentence - "LocalDB was not found. Install
    SQL Server Express LocalDB from the Visual Studio Installer." for errors 53 / 26 / -1 / 2, the
    login sentence for 18456, "the script could not be read ... build again" when the file is
    missing, and Db.Describe(ex) for everything else, so the app keeps one voice.
  - Program.cs: DatabaseBootstrapper.EnsureDatabase() is the first thing the normal startup flow
    does after the two command-line switches (still before Db.CanConnect and before
    DemoDataSeeder.EnsureDemoUsers / EnsureDemoData), inside its own try/catch with Messages.Error
    plus return. --smoketest and --preview return before it, so the read-only switches still never
    touch the database. Nothing else in Program.cs changed.
  - LittleEreseSystemV3.csproj WAS edited (three changes, nothing else): (1) Data\DatabaseBootstrapper.cs
    added to the Data\ <Compile> list; (2) Database\LittleEreses_Database.sql and Database\SeedData.sql
    are now <Content> items with <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>, so
    exactly those two scripts are copied to bin\Debug\Database\; (3) the old wildcard
    <None Include="Database\*.sql" /> was replaced by <None Include="Database\Migration_01_PromotionServices.sql" />,
    otherwise the two new Content entries would also be listed as None items and the build would
    complain. Database\ResetDemoData.sql keeps its existing <Content> line without a copy rule, so it
    stays out of the output folder (it is never run by the app).
  - README.md: new "Quick start for testers" section (clone, open the .slnx, build, Ctrl+F5, the
    database is created automatically, demo logins admin / Admin@123 and nurse1 / Staff@123, the
    LocalDB requirement and how to check it with sqllocaldb info), plus two small updates in Setup
    step 2 (the scripts run automatically now) and in the folder map (DatabaseBootstrapper).
  - Verification: the whole path was proved on a THROW-AWAY database, never on the shop's one. The
    built bin\Debug was copied to %TEMP%\boottest, its .exe.config was pointed at
    "LittleEresesBootstrapTest" and the app was started: it created that database, ran both scripts
    and stayed open. Checked with sqlcmd: 18 base tables, 8 Users, 77 Services, and the demo seeder
    then wrote 12 Clients / 47 Appointments. A SECOND start found the database and did nothing
    (same counts, no duplicates). A deliberately invalid name ("Little-Erese Test") was rejected
    before any SQL ran. The scratch database was dropped and the scratch folder deleted afterwards,
    so nothing was left behind and the shop database was never touched.
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 61 passed, 1 failed - "Demo data: Completed appointments = 0 (expected at least 25)",
  the same expected failure reported since Step 27 (the shop database still holds its 2 old test
  clients, so DemoDataSeeder.EnsureDemoData() correctly does nothing).
  tools\DesignerAudit.ps1: 22 Designer files, no problems, exit code 0.
  --preview ucDashboard rendered, the app was started normally once and closed, no error.log.
  - Known limits / assumptions: the bootstrap only acts when the database is MISSING - an existing
    but empty or half-built database is left alone on purpose (the owner can run the scripts by hand
    in SSMS as before); the check runs on every start but is a single DB_ID() round trip, and it
    never runs under --smoketest or --preview.

- Step 28 (this task): FINAL CLEAN-UP - designer audit tool, code hygiene, polish list, screenshots,
  documentation and the final checks. No module, no behaviour and no feature was added.
  - tools\DesignerAudit.ps1 (NEW, PowerShell, NOT in the .csproj). For every *.Designer.cs under
    Forms\ and Controls\ it checks (a) every "this.<control> = new ..." has a field declaration,
    (b) every declared control field is instantiated, (c) every handler wired with
    "+= new ...(this.Y)" has a method Y in the sibling .cs, (d) exactly one InitializeComponent plus
    the components field plus Dispose(bool), and (e) no Image =, no Db./Session./Sql* and no
    repository call inside a Designer file. It prints one line per problem, a SUMMARY line and exits
    with code 1 when anything is wrong. It strips the text inside "..." first, so a caption such as
    "The package price is charged on the first session." is never mistaken for code (that bug was
    found on the first run and fixed). It only reports on names that start with a control prefix
    (btn/txt/lbl/cbo/dgv/...), so the form's own properties (this.ClientSize, this.Font) are not
    reported as missing fields. Verified against a deliberately broken Designer file in a scratch
    folder (4/4 problems reported, exit 1), then against the real project.
    RESULT on the real project: 22 files checked, 0 problems, exit 0 - nothing had to be fixed.
  - Code hygiene (no behaviour change):
    * 8 unused using directives removed - Controls\ucServices.cs (System.Drawing),
      Controls\ucReports.cs (System.Data), Controls\ucStaffAccounts.cs (System.Data),
      Helpers\Theme.cs (System), Helpers\Validators.cs (System), Helpers\CsvExporter.cs (System.Data),
      Helpers\PreviewTool.cs (System.Reflection) and Properties\AssemblyInfo.cs
      (System.Runtime.CompilerServices). Found with the Roslyn compiler that ships with Visual Studio
      (CS8019 "unnecessary using directive") compiled over the whole project - the list is exact, not
      a guess. A hand-written scanner was tried first and gave 79 false positives; it was deleted.
    * No unused private field or variable was found (the IDE/compiler reports none).
    * No "// TODO(logic prompt)" comment existed anywhere (grep over every .cs file), so no handler
      had to be implemented and no Designer wiring had to be removed.
    * Every public method / property of Data\ and Helpers\ now carries a one-line
      /// <summary>: added to Db.ConnectionString, Db.CreateConnection, DesignTime.IsDesignMode,
      Messages.Info/Warn/Error, Session.UserId/FullName/Username/Role/IsLoggedIn,
      SmokeTest.Run, Validators.IsValidPhone/IsValidName/IsValidUsername/IsValidReference (the last
      one also had its POLISH comment moved above the summary, where it belongs).
    * All 27 "// POLISH:" comments were kept.
  - docs\POLISH_LIST.md (NEW, not in the .csproj): a table of all 27 POLISH comments - file, line and
    text - grouped by folder (Controls, Data, Forms, Helpers), plus a short "where to look first"
    section. docs\ was created for it.
  - docs\screenshots\ (NEW folder, not in the .csproj): 25 PNGs - every Form (LoginForm, MainForm,
    AppointmentForm, ClientEditForm, PackageForm, PaymentForm, PromotionEditForm, ServiceEditForm,
    StaffEditForm), every UserControl (all 12 pages plus ucCalendarMonth) as Admin, and
    ucDashboard_Staff, ucBilling_Staff, ucAppointments_Staff as Staff. Every PNG was looked at.
  - Small Designer / layout fixes after looking at the Admin PNGs (2 fix rounds, listed below).
    - Controls\ucAppointments.Designer.cs: splitMain.SplitterDistance 328 -> 300 (the day grid got
      its height back and the details card moved up) and tlpDetails first RowStyle 122 -> 118, so
      the third service line is visible without scrolling and the Notes box is 2 lines instead of 1.
    - Controls\ucBilling.Designer.cs: splitMain.SplitterDistance 394 -> 470 (the queue grid was too
      narrow: "Time" showed "1:00 PM - 7..." and "Balance" cut the amount), colQueueTime is now a
      118 px Fill column with MinimumWidth 130, and the Client/Services/Status/Payment/Balance
      minimum widths were trimmed so nothing overlaps at 1240 px wide.
    - Controls\ucDashboard.Designer.cs: tlpBody columns 68/32 -> 60/40 (the Today grid was 320 px
      wide and cut every cell to "Cunana..."), colTodayTime / colDayTime are now fixed 148 px Time
      columns (MinimumWidth 148) and the other minimum widths were trimmed. Now "1:00 PM - 7:45 PM"
      and the client name are both readable.
    No control was added, renamed or removed and no event wiring was touched.
  - README.md rewritten: what the system is, the two roles, requirements (Windows, .NET Framework 4.8,
    SQL Server LocalDB, Visual Studio, 1280 x 720), setup (run Database\LittleEreses_Database.sql
    then Database\SeedData.sql, open the solution, build, run), the demo accounts (admin/Admin@123,
    aesthetician1..4 / therapist1..2 / nurse1 with Staff@123), the two command-line switches
    (--smoketest, --preview <ClassName> [--role Admin|Staff]), the folder map, the tests and the
    limitations exactly as in the capstone proposal (no inventory, no automatic SMS/email, no
    payroll, no POS hardware, no digital consent forms, no online payment verification,
    single-premise offline use).
  - docs\DEVELOPER_GUIDE.md (NEW): the architecture in plain words (LoginForm -> MainForm ->
    Controls -> Forms -> Data repositories -> Db), the naming rules table, how to add a field to an
    existing form (Designer, repository, validation, smoke test), how to add a validation or a regex
    (Validators), how to add a report (ReportRepository + Designer columns + Generate), how to add a
    page to the sidebar (Designer button, PageXxx constant, CreatePage, PageTitle, NavButtonFor,
    NavButtons, LoadImages icon, IsAdminPage/ApplyRoleVisibility, .csproj entry), how to change
    colours and fonts (Theme plus the literals in the Designer files), how to avoid the yellow screen
    (DesignTime.IsDesignMode, no database calls in constructors), how to use the smoke test and the
    preview tool, and a troubleshooting table (yellow screen, CS0103 after a .csproj edit -> reload
    the project, database connection errors, LocalDB not found, empty combo boxes, Admin-only page,
    empty grid cells, silent exit -> error.log, printing).
  - .gitignore: still ignores .vs/, bin/, obj/, *.user, *.suo, *.cache and *.bak, and now also
    error.log, smoketest.log and previews/. docs/screenshots is deliberately NOT ignored (checked
    with "git check-ignore docs/screenshots/ucBilling.png" -> exit 1, i.e. it can be committed).
  - PROJECT_RULES.md: added "Additions (Step 4) - final state" (the four checks before a task is
    finished, the two tools that are not in the .csproj, where the documentation lives now, and the
    house rules about TODO/POLISH/summaries/unused code/Designer discipline).
  - LittleEreseSystemV3.csproj was NOT edited (no new .cs file: tools\ and docs\ are not compiled).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 61 passed, 1 failed - "Demo data: Completed appointments = 0 (expected at least 25)".
  That one failure is expected and is the same one reported in Step 27: the shop database still
  holds the 2 old test clients, so DemoDataSeeder.EnsureDemoData() correctly does nothing. Run
  Database\ResetDemoData.sql in SSMS and start the app once to load the demo data; the check then
  passes. Nothing was deleted by this task.
  tools\DesignerAudit.ps1: 22 Designer files, no problems, exit code 0.
  The app was started normally and stayed open for 3 seconds, then closed; no error.log was written.
  - Known issues / what to look at: (1) the demo-data check above needs the reset script once;
    (2) the two Reports "Period" date pickers show "10/ 1/2026" with the "1" slightly apart in the
    preview - that is the Windows date picker drawing the focused date, not a layout problem;
    (3) the ucDashboard / ucBilling / ucAppointments grids still shorten very long client and service
    names ("Cunana...", "Barbie Ar...") because the pages are only 320-470 px wide at 1240 px; the
    full text is always visible in the details card or the bill panel next to them - widen the
    splitter if the owner prefers the list instead.
- Step 27 (this task): the realistic DEMO DATA, loaded once, automatically, when the database has
  no clients yet, plus six read-only consistency checks and the reset script.
  - Data\DemoDataSeeder.cs: EnsureDemoData() (new, next to the unchanged EnsureDemoUsers()).
    It does nothing when EnableDemoSeed is false or when Clients already has rows, never deletes
    anything, and writes the whole set inside ONE Db.InTransaction (SeedAll), so a stopped run
    leaves nothing half saved. Every id is looked up by NAME (staff by username, rooms by
    room_name, services by category + name, promo ids from the inserts) - no hard-coded numbers.
    A fixed Random seed (20260415) makes the data identical on every machine.
    Sections in the file: (1) 12 clients (Filipino names, 09xxxxxxxxx numbers, Concepcion / Tarlac
    addresses, birth dates, 9 consent signed / 3 not, 5 with allergies or medical notes),
    (2) 3 promotions - "Monthly Glow Promo" 15 % Active and covering today, "Beauty Bucket 500"
    Fixed 500 Active (a second real choice in Billing), "Summer Radiance Promo" 10 % Expired
    (ended last month), (3) the past two weeks: 3 ClientPackages (PRP/Vampire Facial 1 / 3 Active,
    PRP 3 / 3 Completed, PRP 2 / 3 Active - three different progress labels) on top of
    PastCompletedVisits = 25 finished visits spread over the last 14 days (3 quiet days with one
    appointment, 11 days with two), (4) today: 2 finished (10:00 AM and 11:30 AM), 1 Ongoing
    (1:00 PM) and 3 Upcoming (3:00, 4:30 and 6:00 PM), (5) 14 Upcoming over the next 14 days - one
    per day, the 4th day is a massage at 1:00 PM, every 5th day is a Walk-in so it has no deposit,
    the day-3 one carries an extra AppointmentHistory 'Rescheduled' row (old date + old time +
    reason), (6) 2 Cancelled visits (20 and 12 days ago, Messenger / Text) with the deposit
    forfeited (downpayment_status 'Forfeited', payment_status 'Forfeited') and their Created +
    Cancelled history rows, (7) the clients' treatment_history text and 8 ActivityLog rows spread
    over the past week (Login, Add Client, Book Appointment, Apply Promo, Start Service, Checkout,
    Commission payout, Backup).
    Everything obeys the business rules: starts only on 30-minute marks inside 10:00 AM - 8:00 PM
    (massage 1:00 PM - 10:00 PM), end = start + duration, a 30-minute clean-up break between two
    visits of a day (so no room is over capacity and no technician overlaps), the package price on
    session 1 and 0.00 afterwards, Commissions at 10 % of the net line amount (package lines: price
    / total_sessions) with about 70 % Unpaid / 30 % Paid + payout_date, and the money fields -
    subtotal, discount (through PromotionRepository.ComputeDiscount), total = subtotal - discount,
    balance_due = total - paid, payment_status Paid and Payments rows (Final, plus a Downpayment
    for a non-walk-in) mixed Cash / GCash / Bank Transfer with a reference for the two electronic
    methods, always 'Verified'. A promo bill is paid in FULL when the booking is made (a Final
    payment, downpayment_status 'Applied') exactly like AppointmentRepository.Create.
    Small readable helpers: InsertClient, InsertPackage, InsertAppointment, InsertLine, InsertStaff,
    InsertRoom, InsertCommission, InsertHistory, InsertActivity, CompletePackageSession,
    AddVisit / AddRandomVisit / TryStartTime (the day cursor), RoomForCategory (RoomServiceCategories
    first, then the shop's usual room by category), DiscountFor, PickPaymentMethod, ReferenceFor.
  - Program.cs: DemoDataSeeder.EnsureDemoData() is called right after EnsureDemoUsers(), inside the
    same try/catch (so a seed failure shows the normal database message and never stops the login).
  - Helpers\SmokeTest.cs: the six read-only checks (CheckDemoData + CheckScalarZero /
    CheckScalarAtLeast): no Billing row with balance_due < 0, every Completed appointment has at
    least one Commissions row, every ClientPackages row has 0 <= sessions_completed <=
    total_sessions, no non-cancelled appointment ends after 10:00 PM, every Billing.total_amount =
    subtotal - discount_amount, and at least 25 Completed appointments. The smoke test still only
    runs EnsureDemoUsers (it must not change the shop's own records).
  - Database\ResetDemoData.sql (NEW FILE, not in the .csproj - Database\*.sql is already a
    wildcard): SET XACT_ABORT ON, one transaction, DELETE in foreign-key order - ActivityLog,
    Commissions, Payments, AppointmentHistory, AppointmentRooms, AppointmentStaff,
    AppointmentServices, Billing, Appointments, ClientPackages, Clients, Promotions - no RESEED,
    and it never touches Users, Services, ServiceCategories, Rooms or RoomServiceCategories. It ends
    with a row-count proof SELECT. NOT run by the app or by this task.
  - LittleEreseSystemV3.csproj was NOT edited (DemoDataSeeder.cs and SmokeTest.cs were already
    listed, and Database\*.sql is a wildcard).
  - Verification: the seeder cannot run against the shop database yet (it already holds 2 clients
    from the earlier tests, and nothing may be deleted without the owner's go-ahead), so the whole
    seeding path was proved on a THROW-AWAY copy: a scratch database was built from
    LittleEreses_Database.sql + SeedData.sql, the app was started against it and the demo data
    appeared (12 Clients, 47 Appointments, 47 AppointmentServices / AppointmentStaff /
    AppointmentRooms / Billing, 58 Payments, 27 Commissions, 50 AppointmentHistory,
    3 ClientPackages, 3 Promotions, 8 ActivityLog; 27 Completed - 25 of them in the past 14 days,
    6 today, 14 Upcoming, 2 Cancelled = 47). A rule sweep on that copy returned 0 bad rows for:
    negative balance, Completed without commission, package sessions out of range, end after
    10:00 PM, total <> subtotal - discount, half-hour starts, aesthetic hours, massage hours,
    technicians overlapping, and the two daily capacities. --smoketest on the copy: 62 passed,
    0 failed (the six new checks PASS). The scratch database and its app copy were then DROPPED
    and deleted, so nothing was left behind.
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest on the real shop database: 61 passed, 1 failed - "Demo data: Completed appointments =
  0 (expected at least 25)". That is expected and correct: the demo data is not seeded yet because
  the Clients table already has rows. RUN Database\ResetDemoData.sql in SSMS (database
  LittleEresesDB) and start the app once; the demo data loads on that start and the last check
  turns PASS.
  - Known limits / assumptions: the catalogue has no multi-session laser service, so the third
    package uses the other multi-session service the catalogue has (today "Facial test", 8
    sessions) - the code searches for default_total_sessions > 1 and is marked // POLISH. The demo
    promotions are not linked to specific services (so the whole bill is eligible); a promotion
    with service links is deliberately skipped by the demo's DiscountFor, which uses the ONE
    formula PromotionRepository.ComputeDiscount because CalculateDiscount opens its own connection
    and would deadlock inside the seeding transaction. The demo rooms fall back to the shop's usual
    room by category name when RoomServiceCategories is still empty (POLISH: link the rooms and the
    fallback is never used). Payout dates are today at the latest. The 8 ActivityLog demo rows are
    illustrative - no backup file was created by them.

- Step 26 (this task): the LOGIC of the Activity Log and File Maintenance pages, plus the two
  repositories they need. The Designer layouts were NOT restructured; one small Designer fix
  (the Activity Log time column) is listed under "small Designer fix" below.
  - Data\ActivityRepository.cs (Log unchanged, read methods added):
    GetLog(from, to, userId, module, search) returns log_id, logged_at, full_name, action, module,
    description, newest first (logged_at DESC, log_id DESC). The range uses the NEXT midnight as the
    boundary, so the whole "to" day is included (the same rule the other reports use). userId 0 =
    every user, module "All" = every module, search matches action OR description. The alias
    full_name comes from INNER JOIN Users, which is exactly what colLogUser binds to.
    GetModules() returns the DISTINCT module names, sorted, for the filter.
  - Data\MaintenanceRepository.cs (was an empty class; now the only place with raw SQL for this):
    BackupDatabase(filePath) runs BACKUP DATABASE [<name>] TO DISK = @path WITH INIT, FORMAT,
    NAME = 'LittleEresesDB backup'. The database name is read from the "LittleEreseDb" connection
    string (SqlConnectionStringBuilder.InitialCatalog) and checked against ^[A-Za-z0-9_]+$ before
    it is placed inside the brackets; the path is always a parameter.
    VerifyBackup(filePath) runs RESTORE VERIFYONLY FROM DISK = @path.
    RestoreDatabase(filePath) opens its own connection to "master" (same server, same security -
    the connection string is copied and only InitialCatalog changed), calls
    SqlConnection.ClearAllPools(), then ALTER DATABASE ... SET SINGLE_USER WITH ROLLBACK IMMEDIATE,
    RESTORE DATABASE ... FROM DISK = @path WITH REPLACE, and ALWAYS in a finally block
    ALTER DATABASE ... SET MULTI_USER (a failed restore must not leave the shop locked out), then
    clears the pools again. CommandTimeout 0 because a restore can run for minutes.
    GetDatabaseInfo() returns db_name (DB_NAME()), server_name (SERVERPROPERTY('ServerName')) and
    size_mb (SUM(size) * 8 / 1024 from sys.master_files, ROWS files only).
    GetTableCounts() returns table_name + row_count for every user table (sys.tables joined to
    sys.partitions with index_id IN (0,1), GROUP BY name, ordered by name).
    ListBackups(folder) returns file_name, size_mb, created_at for *.bak, newest first
    (LastWriteTime - a copied backup keeps the moment it was written; marked // POLISH). A missing
    or unreadable folder gives an empty table, never an error.
    Every method rethrows InvalidOperationException with a readable sentence; error 3201 and any
    "access denied" text map to "SQL Server could not write to that folder. Choose a folder inside
    your Documents folder."
  - Controls\ucFileMaintenance.cs: Load styles the two grids and the six buttons with Theme, checks
    Session.IsAdmin (a Staff user gets the boxes disabled plus a warning, which is skipped when
    nobody is signed in so the smoke test and the preview tool still work), sets the default backup
    folder to Documents\LittleEreseBackups (Directory.CreateDirectory, so the folder exists before
    SQL Server writes into it) and refreshes everything.
    Browse (folder / file) uses FolderBrowserDialog and OpenFileDialog ("Backup file (*.bak)|*.bak").
    btnBackupNow builds LittleEreseBackup_yyyyMMdd_HHmmss.bak, shows the wait cursor, runs
    BackupDatabase then VerifyBackup (a fresh backup is checked at once, which is far better than
    finding out at restore time), reports the full path and the size in MB, logs "Backup" /
    "File Maintenance", reloads the recent list and lblLastBackup.
    btnRestore_Click needs a selected existing .bak, runs VerifyBackup FIRST, then asks TWO
    confirmations (the path + "The current data will be replaced", then "This cannot be undone.
    ALL current appointments, clients and payments will be replaced ..."), runs RestoreDatabase,
    logs "Restore", tells the user the app will restart and calls Application.Restart().
    btnRefreshInfo reloads the info, the table counts and the backups; btnOpenBackupFolder opens
    the folder with Process.Start("explorer.exe", "<folder>"). Every action re-checks
    Session.IsAdmin, every action is wrapped in try/catch with Messages.Error, and the wait cursor
    is always restored in a finally block.
    lblLastBackup reads "Last backup: MMM dd, yyyy h:mm tt" from the newest file, or "No backup yet".
  - Controls\ucActivityLog.cs: Load styles the grid and the two buttons with Theme, checks
    Session.IsAdmin (same rule as above), sets dtpFrom = 7 days ago and dtpTo = today, fills
    cboUser with "All users" (value 0) + UserRepository.GetUsers("", "All") and cboModule with
    "All modules" + ActivityRepository.GetModules(), then loads the grid and lblCount
    ("1 entry" / "N entries"). btnSearch_Click and Enter in txtSearch reload (Enter is swallowed so
    the box does not beep); a From date after the To date shows a warning instead of a query.
    btnExport_Click exports the grid with CsvExporter (file name activity-log-yyyyMMdd-yyyyMMdd).
    Every query runs behind Cursor.WaitCursor inside try/catch with Messages.DbError.
  - Helpers\SmokeTest.cs: three read-only checks - GetLog(last 7 days) returns the six columns the
    grid binds (checked by name, because a renamed alias would silently show empty cells),
    GetTableCounts returns 18 tables, GetDatabaseInfo returns a row. No backup / restore is run.
  - Small Designer fix: in ucActivityLog.Designer.cs colLogTime went from 175 px (MinimumWidth 170)
    to 200 px (MinimumWidth 195), because the preview showed "Oct 25, 2026 8:15:28 PM" cut to
    "8:15:28 ..." - the format is h:mm:ss tt and needs the extra 25 px.
  - Self-test (temporary, REMOVED again - no --backuptest switch and no BackupSelfTest method are
    left in Program.cs): a backup was written to the temp folder, verified with RESTORE VERIFYONLY,
    it appeared in ListBackups and the file was deleted again. Result: BACKUP OK (7,196,672 bytes),
    VERIFY OK, LIST rows = 1, SELFTEST PASSED, cleanup: file deleted = True. The restore was NEVER
    run automatically.
  - LittleEreseSystemV3.csproj was NOT edited (no new file).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 56 passed, 0 failed, 0 skipped.
  --preview ucActivityLog: From 7 days ago / To today / User "All users" / Module "All modules" /
  Search + Search + Export CSV + "91 entries", and the real log rows ("Aesthetician 1", "Login",
  "Authentication", "User logged in") with the full date and time after the column fix.
  --preview ucFileMaintenance: the backup card with the default folder and "No backup yet", the
  restore card with the red warning, System information reading "Database: LittleEresesDB",
  "Server: D3vic...", "Database size: 8.00 MB" and the 18-row table counts grid, and the empty
  Recent backups grid. The app was started once and closed again (no error.log).
  - Known limits / assumptions: the backup is verified right after it is written (an extra few
    seconds) because that is the only moment a bad file can still be replaced; the Recent backups
    date is the file's LastWriteTime (POLISH: CreationTime if the owner prefers); a restore always
    ends with Application.Restart(), so the login screen comes back; a Staff user opening either
    page only sees disabled controls (MainForm already hides both pages).

- Step 25 (this task): Layout only (Designer work), PART 2 of File Maintenance (Admin only):
  the bottom row of tlpMain. PART 1 (frame, backup card, restore card) is unchanged. No logic,
  no SQL, no file or Session access. The two wired events got empty handlers with
  "// TODO(logic prompt)".
  - Controls\ucFileMaintenance (.Designer.cs + .cs): grpSystem "System information" was added to
    tlpMain at (0, 1) and grpBackups "Recent backups" at (1, 1), so the 2 x 2 frame from PART 1 is
    now full. Both are GroupBoxes on Surface/white with the same Semibold 10 pt caption and the
    14/10/14/12 padding as the two top cards; grpSystem carries the mirror-image margin
    (0, 6, 6, 0) and grpBackups (6, 6, 0, 0), so the 12 px gaps between the four cards stay even.
    grpSystem holds tlpSystem (1 column, 5 rows): lblDbName ("Database: -", TextDark),
    lblServer ("Server: -", muted) and lblDbSize ("Database size: -", muted) are AutoSize 9 pt
    labels stacked on top; then pnlTableCounts (Surface, FixedSingle, 1 px padding, the %
    100F row so it takes every spare pixel) with dgvTableCounts; then a 34 px row with
    flpSystemButtons (WrapContents = false, AutoSize = false) holding btnRefreshInfo "Refresh"
    (96 x 34) and btnOpenBackupFolder "Open Backup Folder" (172 px), both with the secondary
    button look as literals.
    grpBackups holds pnlBackups (Surface, FixedSingle, 1 px padding, Dock Fill) with dgvBackups
    only - no buttons, as the prompt asked.
    dgvTableCounts and dgvBackups: AutoGenerateColumns = false, ReadOnly = true, the Theme.StyleGrid
    look copied as literals (gold PrimaryDark #7A591C header band, 30 / 34 px header height,
    Segoe UI 10 pt cells, gold selection, SingleHorizontal cell borders, no row headers,
    FullRowSelect, AutoSizeColumnsMode Fill).
    Columns: colTableName "Table" / table_name (Fill) and colRowCount "Records" / row_count
    (fixed 110 px); colBackupFile "File" / file_name (Fill), colBackupSize "Size (MB)" / size_mb
    (Format "N2", right aligned, fixed 100 px) and colBackupDate "Created" / created_at
    (Format "MMM dd, yyyy h:mm tt", fixed 165 px).
    Events wired: btnRefreshInfo_Click and btnOpenBackupFolder_Click (both new empty handlers in
    the .cs); nothing else changed in the .cs.
  - Fix rounds from the preview (2 used): (1) the first build failed twice on one line
    (dataGridViewCellStyle5.ForeColor had two extra closing parentheses) - the line was repaired
    with a small script that only balanced that line; (2) the "Open Backup Folder" button was the
    last row of a percent-free grid row and sat directly on the card's bottom padding, so the
    button row became a fixed 34 px row and flpSystemButtons AutoSize was set to false, which puts
    the two buttons in line with the grid above. The table-counts cell font was also set back to
    the 10 pt grid font and its row height to 28, matching the other grids.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 53 passed, 0 failed, 0 skipped. --preview ucFileMaintenance: the four white cards
  in a 2 x 2 grid, "System information" with Database / Server / Database size, the 2-column
  Table / Records grid and Refresh + Open Backup Folder underneath, "Recent backups" with the
  3-column File / Size (MB) / Created grid filling the whole card - every label complete, nothing
  overlapping or truncated, all four cards the same size at 1240 x 600. The app was started once
  and closed again (no error.log).
  - Known limits: lblDbName / lblServer / lblDbSize show "-" and both new grids are empty until the
    logic prompt fills them; the backup / restore cards still hold no values.

- Step 24: Layout only (Designer work), PART 1 of File Maintenance (Admin only):
  the frame, the backup card and the restore card. No logic, no SQL, no file or Session access.
  The five wired events got empty handlers with "// TODO(logic prompt)". Both files were empty
  shells before.
  - Controls\ucFileMaintenance (.Designer.cs + .cs): AppBack page, 1000 x 640, AutoScroll on
    the control. tlpMain (Dock Fill, 2 columns x 2 rows, both 50 %, Padding 12) holds the four
    cards; the 12 px gap is a 6 px margin on the two sides of each card, so the cards start at
    the same 12 px edge as the padding. Only the TOP row is built now; the bottom row stays
    empty on purpose (PART 2 later).
    Top left grpBackup "Backup database" (GroupBox, Surface/white, Semibold 10 pt caption,
    Padding 14/10/14/12) holds tlpBackup (2 columns: the rest + a fixed 96 px button column)
    with lblBackupInfo (muted 9 pt, MaximumSize 440 so it wraps to 2 lines), lblBackupFolder,
    txtBackupFolder (read-only, Dock Fill), btnBrowseBackupFolder "Browse..." (secondary look
    as literals, Dock Fill in its cell), lblLastBackup ("Last backup: never") and btnBackupNow
    "Backup Now" (primary look as literals, 130 x 34).
    Top right grpRestore "Restore database" has the same structure: lblRestoreWarning (Danger
    red, Semibold 9.5 pt, wraps, the exact sentence from the prompt), lblRestoreFile,
    txtRestoreFile (read-only), btnBrowseRestoreFile "Browse...", btnRestore
    "Restore Database" (danger look as literals, 150 x 34).
    Events wired: Load (ucFileMaintenance_Load), btnBrowseBackupFolder_Click, btnBackupNow_Click,
    btnBrowseRestoreFile_Click, btnRestore_Click.
  - Fix rounds from the preview (1 used): (1) the first draft declared a FlowLayoutPanel around
    each text box plus its Browse button; they were dropped before the first build because the
    table row already has a stretching cell and a fixed 96 px cell; (2) grpRestore had no gap on
    its left side (Margin 0), so both cards now use a 6 px margin and the 12 px gap shows evenly.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 53 passed, 0 failed, 0 skipped. --preview ucFileMaintenance: the two white cards
  side by side with "Backup folder" / Browse... / "Last backup: never" / "Backup Now" and
  "Backup file" / Browse... / the red restore warning / "Restore Database" - every label
  complete, nothing overlapping or truncated; the empty bottom half is the PART 2 card row.
  The app was started once and closed again (no error.log).
  - Known limits: the two text boxes are empty and lblLastBackup reads "Last backup: never"
    until the logic prompt fills them; the PART 2 cards (and their handlers) do not exist yet.

- Step 23: Layout only (Designer work) for the Activity Log page (Admin only).
  No logic, no SQL, no file or Session access: the four wired events got empty handlers with
  "// TODO(logic prompt)". Both files were empty shells before.
  - Controls\ucActivityLog (.Designer.cs + .cs): AppBack page, 1000 x 640.
    pnlToolbar (Dock Top, 56 px, Surface/white) holds tlpToolbar (one row, two columns: the
    filter strip and a fixed 300 px action strip). flpFilters (Dock Fill, no wrap) holds
    lblFrom + dtpFrom (Short, 116 px), lblTo + dtpTo, lblUser + cboUser (DropDownList, 140 px),
    lblModule + cboModule (DropDownList, 120 px) and lblSearch + txtSearch (180 px); every label
    is AutoSize with a 0/8/2/0 margin so it lines up with the 25 px pickers and boxes.
    tlpActions (3 columns 100/120/rest) holds btnSearch "Search" (primary look as literals),
    btnExport "Export CSV" (secondary look as literals) and the AutoSize lblCount "0 entries"
    (SmallFont 8.5 pt, TextMuted). Everything fits on ONE 56 px row at 1240 x 600, so the
    two-row fallback the prompt allowed was not needed.
    Below it the card pnlGrid (Dock Fill, Surface, FixedSingle, 1 px padding) with dgvLog
    (Dock Fill, the Theme.StyleGrid look copied as literals, AutoGenerateColumns = false,
    ReadOnly = true, AutoSizeColumnsMode Fill): colLogId (hidden, log_id),
    colLogTime "Date and time" / logged_at (Format "MMM dd, yyyy h:mm:ss tt", 175 px),
    colLogUser "User" / full_name (150 px), colLogAction "Action" / action (145 px),
    colLogModule "Module" / module (120 px) and colLogDescription "Description" / description
    (the only Fill column, so it takes the remaining width).
    Events wired: Load (ucActivityLog_Load), btnSearch_Click, btnExport_Click,
    txtSearch_KeyDown. btnExport_Click is used for the CSV button on purpose, so the empty
    handler is where the logic goes later.
  - Fix rounds from the preview (2 used): (1) the first full-width row pushed "Search" and
    "Export CSV" off the right edge at 1000 px, so the toolbar was re-laid out into the two
    columns described above (the buttons now sit in their own fixed 300 px strip and the
    filter strip gets the rest); (2) the count label was docked and its text floated to the
    middle of the row, so it is AutoSize again with a 4 px left margin beside "Export CSV".
    The grid headers were also switched to the Theme.StyleGrid column mode (fixed ID/time/user
    columns, Description Fill) so no caption is cut at 1240 px.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 53 passed, 0 failed, 0 skipped. --preview ucActivityLog: From / To / User /
  Module / Search on one row, Search + Export CSV + "0 entries", the 5 visible grid columns
  (Description taking the rest of the width) - nothing overlapping or truncated. The app was
  started once and closed again (no error.log).
  - Known limits: cboUser and cboModule are empty in the Designer (they are filled in the logic
    prompt, like the other pages); lblCount reads "0 entries" until then.

- Step 22 (this task): the LOGIC of the Dashboard and its month calendar. The Designer layouts were
  NOT restructured (no Designer fix was needed).
  - Data\AppointmentRepository.cs: added GetMonthAppointments(year, month) - ONE query for the whole
    month returning appointment_id, appointment_date, start_time_text ("10:00 AM"), client_name,
    services, staff_names, status, payment_status. The per-appointment comma lists are built with the
    same child queries GetByDate uses, so the two screens can never disagree. The old ChildNames was
    split into CollectChildNames (shared) + ChildNamesForRange (the new range variant); GetByDate
    behaves exactly as before. AddRef: no Designer/SQL change was needed.
  - Data\ReportRepository.cs: added GetDashboardStats(date, includeSales) - one row with today_total
    (non-cancelled), remaining (Upcoming/Ongoing), completed, today_sales (SUM amount_paid of
    Payments not Rejected, by payment_date < date+1) and active_packages (ClientPackages Active).
    includeSales = false puts a literal 0 in the query, so Staff never even asks for money.
  - Controls\ucCalendarMonth.cs: public VisibleMonth / SelectedDate, LoadMonth(month, table), events
    DaySelected / DayOpened / AppointmentOpened / MonthChanged. LoadMonth builds the 42 day cells at
    runtime (DesignTime guard, old cells disposed, SuspendLayout/ResumeLayout), each one a Panel with
    a day-number Label (other months muted), a count badge, up to three "10:00 AM Client" lines
    coloured by Theme.StatusColor (cancelled muted and not counted) and "+N more". Cell colours:
    Surface, today = light Primary tint, selected = stronger tint + Primary border (painted in
    Cell_Paint), 20 to 29 = light amber, 30 or more = light red. Hover highlights the cell; one
    ToolTip created in code is disposed with the control and lists up to 8 appointments
    ("time - client - services - technician - status", "+N more", "No appointments"). Every child of
    a cell forwards its mouse events; a double-click on a line raises AppointmentOpened, elsewhere
    DayOpened. DoubleBuffered is set in OnLoad. Small methods: BuildCells / BuildCell / FillLines /
    ApplyCellColors / ShowTooltip / TooltipText / GroupAppointments.
  - Controls\ucDashboard.cs: Load (DesignTime guard) styles both grids, loads the five icons through
    IconHelper.Load(name, 30) (CalendarDarkIcon, ClockIcon, CheckIcon, BillingIcon, ServicesIcon) and
    hides pnlCardSales for Staff, whose column is then squeezed to 0 px so the other four cards share
    the width. RefreshAll() fills the cards (sales as PHP peso), the Today grid and the calendar month;
    lblTodayHeading shows "MMM dd, yyyy - n appointments". Calendar events: MonthChanged reloads only
    (POLISH note in code: the heading reads "Today - MMM dd, yyyy (n appointments)".)
    the month, DaySelected fills dgvDay and switches to tabDay, DayOpened opens the Appointments page
    on that day, AppointmentOpened does the same for the appointment's date. dgvDay_CellDoubleClick
    behaves like AppointmentOpened. CellFormatting colours the Status (Today) / Payment (Selected Day)
    columns. A Timer created in code (60 s) calls RefreshAll; it is stopped and disposed through the
    control's Disposed event, because the Designer Dispose must stay untouched.
  - Forms\MainForm.cs: Navigate(string pageKey, object argument = null) and new
    public void OpenAppointments(DateTime date). A small ApplyArgument hands a DateTime to
    ucAppointments.StartDate BEFORE the page loads; every existing Navigate call still compiles and
    behaves as before (the argument is simply ignored by the other pages).
  - Helpers\SmokeTest.cs: three new read-only checks - GetMonthAppointments(this month) (1 row),
    GetDashboardStats(today, true) returns one row, and ucCalendarMonth.LoadMonth with an empty table
    builds 42 cells. Because the smoke test's BuildEveryFormAndControl already constructs the control,
    the calendar checks use the table named tlpDays. 53 passed.
  - LittleEreseSystemV3.csproj was NOT edited (no new file).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 53 passed, 0 failed, 0 skipped. --preview ucDashboard (Admin and Staff): the five /
  four cards, the calendar with the today tint, the count badge and the "1:00 PM Cunanan, J..." line,
  the right column with the Today grid - nothing overlapping or truncated. --preview
  ucCalendarMonth: the header, legend, weekday strip and the empty 7 x 6 frame (its cells only exist
  after LoadMonth, which the Dashboard calls). Two fix rounds: the appointment lines were moved from
  a FlowLayoutPanel to docked Labels (a flow panel clipped the first line), and their height/font
  was raised to 16 px / 8 pt so the text is not cut.
  - Known limits / assumptions: dgvDay has no Status column in the Designer, so only Payment is
    coloured there; the Appointments page has no public "select this row" API, so a double-clicked
    appointment only opens its DAY; the 60 s timer is disposed from the Disposed event.

- Step 21: Layout only (Designer work) for the Dashboard and its month calendar.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)". Both files were empty shells before.
  - Controls\ucCalendarMonth (.Designer.cs + .cs): BackColor Surface, 760 x 520.
    pnlCalHeader (Dock Top, 56 px) holds tlpCalHeader (one row, five columns): btnPrevMonth
    (34 x 30, secondary look, Font "Segoe MDL2 Assets" 12 pt, Text \uE76B), lblMonth (HeadingFont
    16 pt semibold, TextDark, centered in its cell, "October 2026"), btnNextMonth (same look,
    \uE76C), btnTodayMonth ("Today", 72 x 30, secondary look) and, on the right, flpLegend
    (AutoSize, WrapContents = false, right aligned) with five 12 x 12 color Labels plus a muted
    AutoSize text label each: lblLegendUpcoming (PrimaryLight) / lblLegendUpcomingText,
    lblLegendOngoing (Info) / lblLegendOngoingText, lblLegendCompleted (Success) /
    lblLegendCompletedText, lblLegendCancelled (Danger) / lblLegendCancelledText and
    lblLegendRescheduled (Warning) / lblLegendRescheduledText. The prompt listed four legend
    items and the Status palette has five, so the Rescheduled one was added to keep the legend
    honest (Cancel if the owner wants only four).
    tlpWeekdays (TableLayoutPanel, Dock Top, 32 px, 7 equal percent columns x 1 row): lblDaySun ..
    lblDaySat "Sun".."Sat", Dock Fill, centered, white Semibold 10 pt on a PrimaryDark background.
    tlpDays (Dock Fill, 7 equal columns x 6 equal percent rows, CellBorderStyle Single,
    BackColor Border) is EMPTY in the Designer on purpose - the day cells are generated from data
    in a later prompt (the "items generated from data" exception in PROJECT_RULES).
  - Controls\ucDashboard (.Designer.cs + .cs): AppBack page, 1000 x 640.
    tlpKpi (Dock Top, 104 px, 5 equal 20 % columns, the 12 px gap is the right margin of the four
    left cards) with pnlCardToday, pnlCardRemaining, pnlCardCompleted, pnlCardSales,
    pnlCardPackages. Every card is Surface + FixedSingle and holds pnlCard*Strip (Dock Left, 4 px,
    Primary), tlpCard* (Fill, 2 columns: text, 44 px for the icon; Padding 10/8/6/8) with the
    small title (SmallFont, TextMuted, 22 px row - "Today's Appointments", "Still to Come",
    "Completed Today", "Today's Sales", "Active Packages"), the big value (TitleFont 22 pt,
    TextDark - lblCardTodayValue "0", lblCardRemainingValue "0", lblCardCompletedValue "0",
    lblCardSalesValue "0.00", lblCardPackagesValue "0") and the 36 x 36 PictureBox
    picCardToday / picCardRemaining / picCardCompleted / picCardSales / picCardPackages (no Image
    in the Designer: images load at runtime through IconHelper, per the layout rules).
    tlpBody (Dock Fill, 2 columns 68 % / 32 %, one row, Padding 0/12/0/0 so it starts below the
    KPI row, MinimumSize 900 x 420 so the right column stays usable at 1366 x 768). Left cell:
    calMonth (the only user control allowed inside a Designer file), Dock Fill, 12 px right gap.
    Right cell: tabSide (Dock Fill) with tabToday "Today" (lblTodayHeading + dgvToday) and
    tabDay "Selected Day" (lblDayHeading, dgvDay, pnlDayFooter with btnOpenInAppointments
    "Open in Appointments", secondary look, right aligned under the grid). Both grids use the
    Theme.StyleGrid look as literals, AutoGenerateColumns = false, AutoSizeColumnsMode Fill.
    dgvToday: colTodayTime "Time" / time_range, colTodayClient "Client" / client_name,
    colTodayServices "Services" / services, colTodayStaff "Staff" / staff_names,
    colTodayStatus "Status" / status. dgvDay: the same four plus colDayPayment "Payment" /
    payment_status. Both grids scroll, so the 320 px column is readable at 1366 x 768.
    Events wired: Load, dgvToday_CellFormatting, dgvDay_CellFormatting, dgvDay_CellDoubleClick,
    btnOpenInAppointments_Click.
  - Fix rounds from the previews (2 used): (1) in ucCalendarMonth the five legend items did not
    fit beside the month in one 56 px row, so the header was re-laid out with the legend on its
    own right-aligned row under the month row (same 56 px, nothing truncated); (2) the glyph
    buttons and "Today" are 34 / 72 px and the legend text is 8 pt muted, so the month block and
    the legend no longer touch. The "Selected Day" tab was also previewed once by temporarily
    setting tabSide.SelectedIndex to 1 (the Designer value is back to 0 = Today).
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 50 passed, 0 failed, 0 skipped.
  --preview checked for ucCalendarMonth (the header with the glyph buttons and "Today", the
  centered "October 2026", the five legend items in their Status colors, the dark weekday strip
  and the empty 7 x 6 grid) and ucDashboard (the five KPI cards with their gold left strip, the
  calendar on the left and "Today" / "Selected Day" on the right with the 5-column grid - nothing
  overlapping or truncated). The app was started once and closed again (no error.log).

- Step 20: the LOGIC for the Commissions and Reports modules (two repositories, two new
  helper files, the two pages). The Designer layouts were NOT restructured and no Designer fix was
  needed; the .cs files of both pages were rewritten from their "// TODO(logic prompt)" shells.
  - Data\CommissionRepository.cs (was an empty class):
    Read: GetSummary(from, to, userId, payout) groups the commissions per staff member (user_id,
    full_name, services_count, total_commission, unpaid_amount, paid_amount; userId 0 = everybody;
    payout All | Unpaid | Paid). GetDetails(from, to, userId, payout) returns the single rows
    (commission_id, service_date, staff_name, client_name, service_name, base_amount,
    commission_rate, commission_amount, payout_status); base_amount is READ BACK as
    commission_amount * 100 / commission_rate (rate 0 falls back to the amount, no division), so the
    screen shows what was really paid and not a re-calculation with today's prices.
    Write: MarkPaid(List<int>) sets 'Paid' + payout_date = GETDATE() for Unpaid rows only (a second
    click can never overwrite an earlier payout date) and returns the rows that changed.
    WHY the date filter uses the appointment date: a commission belongs to the day the service was
    done, never to the day it was paid out. Every list of ids is one parameterised IN (...) list.
  - Data\ReportRepository.cs (was an empty class):
    One shared CTE (SalesLine) defines a sale: a not-created-but-completed service line of a
    Completed appointment inside the range, with gross = quantity x price and the line's
    PROPORTIONAL share of Billing.discount_amount (discount x line / subtotal). Category, service
    and grand total therefore always add up to the same number.
    GetSalesByCategory(from, to) -> category_name, services_count, gross_sales, discount_total,
    net_sales. GetPaymentsByMethod(from, to) -> payment_method, payments_count, amount from every
    payment that is not 'Rejected', dated by payment_date with the next day as the boundary so the
    whole "to" day belongs to the range. GetTopServices(from, to, top) -> rank_no, service_name,
    category_name, times_availed, revenue (ROW_NUMBER over times availed, then revenue).
    GetCommissionReport(from, to) DELEGATES to CommissionRepository.GetSummary(from, to, 0, "All"),
    so the report and the Commissions page can never disagree. GetAppointmentStatusCounts(from, to)
    always returns the same five rows (Upcoming, Ongoing, Completed, Cancelled from the appointments
    of the range + Rescheduled counted from AppointmentHistory change_type 'Rescheduled' by
    changed_at), zeros included, so the tab never looks broken. GetAppointmentSourceCounts(from, to)
    -> booking_source, total for the non-cancelled appointments.
    // POLISH: a sale is counted when the service is completed, not when the money arrives (a period
    // can show sales that are paid later). The Rescheduled counter counts the MOVES made in the
    // period, not the appointments that sit on a rescheduled date. Both are marked in the file.
  - Helpers\CsvExporter.cs (NEW FILE, registered in LittleEreseSystemV3.csproj):
    Export(grid, suggestedFileName) shows a SaveFileDialog and writes the VISIBLE columns and rows
    as UTF-8 WITH BOM (so Excel reads the peso sign), every field quoted with doubled inner quotes,
    header captions from HeaderText and cell text from FormattedValue (what the user sees). Empty
    grids are refused with a warning, the file name is stripped of invalid characters, the result is
    confirmed with Messages.Info(path) and nothing ever throws.
  - Helpers\GridPrinter.cs (NEW FILE, registered in LittleEreseSystemV3.csproj):
    Print(grid, title) builds a PrintDocument (landscape, 50-unit margins) and shows a
    PrintPreviewDialog. It repeats the title, "Little Erese's Aesthetics and Spa" and the print date
    on every page, draws the visible column captions and rows, spreads the paper width over the
    visible columns in proportion to their on-screen width and continues on a new page when the
    paper runs out. Hidden id columns are never printed. Nothing ever throws.
  - Controls\ucCommissions.cs: Admin only (a Staff user gets a warning and an empty page; the warning
    is skipped when nobody is signed in so the smoke test and the preview tool still work).
    Load styles both grids and the four buttons, dtpFrom = first day of this month, dtpTo = today,
    cboStaff = "All staff" + UserRepository.GetActiveStaff(), cboPayout defaults to "Unpaid" and
    loads. The three cards add up the SUMMARY grid (not the details), so the whole period stays
    visible while one staff member is selected. Selecting a staff row narrows dgvDetails through
    _detailUserId; clicking the empty area below the rows shows everybody again. btnSearch validates
    the range and reloads. btnMarkPaid collects the selected rows that are still Unpaid, asks
    Messages.Confirm with the count and the total, calls MarkPaid, logs "Commission payout" with
    ActivityRepository.Log, reloads and keeps the same staff member selected. btnExport exports the
    detail grid ("commissions-yyyyMMdd-yyyyMMdd"), btnPrint previews it. CellFormatting colours the
    payout cell Success (Paid) / Warning (Unpaid) and keeps white text on a selected row.
  - Controls\ucReports.cs: Admin only (same guard). Load styles the six grids and the three buttons,
    picks "This Month" and generates. cboPreset moves both pickers (This Week starts on MONDAY, This
    Month, Last Month, Today) and only "Custom" enables them, so the range always matches the caption.
    btnGenerate validates the range and loads ALL FOUR tabs in one go (6 queries), then fills the
    labels: lblSalesTotal "Net sales <sum of net_sales>", lblCommissionTotal "Total commission <sum>",
    and the four appointment cards (the four statuses add up to the headline; Rescheduled is a
    history event and is counted once, in its own card). btnExport exports the grid of the selected
    tab (Sales exports dgvSales, not the by-method grid) and btnPrint previews it; the file name is
    built from the tab caption and the range. dgvReports_CellFormatting is wired to the four money
    grids and keeps the money cells right aligned and the selected text white. Empty results show
    empty grids and zeros, never an error.
  - Helpers\SmokeTest.cs: three read-only checks (GetSummary for this month runs,
    GetSalesByCategory for this month runs, GetAppointmentStatusCounts returns exactly 5 rows).
    Nothing is written.
  - Data check (temporary, no file left behind): the seven new queries were also run against a
    throw-away completed appointment created inside a transaction that was ROLLED BACK, and the
    numbers were correct (gross 5,000.00, discount 500.00, net 4,500.00, one Cash payment of
    4,500.00, commission 450.00 on a base of 4,500.00, 1 Walk-in booking). Nothing was saved.
  - LittleEreseSystemV3.csproj WAS edited: five lines added - Data\CommissionRepository.cs,
    Data\ReportRepository.cs, Helpers\CsvExporter.cs, Helpers\GridPrinter.cs and (the line the
    previous step was missing) Helpers\ReceiptPrinter.cs. No file was renamed or removed.
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 50 passed, 0 failed, 0 skipped.
  --preview checked for ucCommissions (the two toolbar rows with From / To / Staff "All staff" /
  Payout "Unpaid", Search + Mark Selected as Paid + Export CSV + Print, the three cards reading
  P0.00 in dark, red and green, the 6-column staff summary and the 9-column detail grid - nothing
  overlapping or truncated) and ucReports ("This Month" with both pickers locked, Generate / Export
  CSV / Print, the four tabs, "Net sales P0.00", the 5-column sales grid, "Collected by payment
  method" with its 3-column grid). No fix round was needed. The app was started once and closed
  again (no error.log).

- Step 19 (this task): Layout only (Designer work) for the Commissions and Reports pages.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt. Both files were empty shells before.
  - Controls\ucCommissions (.Designer.cs + .cs): AppBack page, 1000 x 640, Admin only.
    pnlToolbar (Dock Top, 92 px, Surface, FixedSingle) holds tlpToolbar with two rows:
    row 1 flpFilters (lblFrom + dtpFrom, lblTo + dtpTo, lblStaff + cboStaff DropDownList 180 px,
    lblPayout + cboPayout with All / Unpaid / Paid; every label AutoSize), row 2 flpButtons
    (btnSearch primary, btnMarkPaid "Mark Selected as Paid" primary, btnExport "Export CSV" and
    btnPrint "Print" secondary).
    pnlSummary (Dock Top, 96 px, AppBack, 12/8 padding) holds tlpSummary with three equal cards:
    pnlTotalEarned / pnlUnpaid / pnlPaid, each a white FixedSingle panel with a muted 9 pt title
    (lblTotalEarnedTitle "Total commission earned", lblUnpaidTitle "Unpaid", lblPaidTitle "Paid")
    above a 16 pt Semibold value (lblTotalEarned dark, lblUnpaid red, lblPaid green).
    splitMain (Dock Fill, Orientation Horizontal, SplitterDistance 196 = about 43 % of the 452 px,
    Panel1MinSize 130, Panel2MinSize 160). Top card pnlSummaryGrid with dgvSummary
    (Theme.StyleGrid look as literals, AutoGenerateColumns = false): colSumUserId (hidden,
    user_id), colSumStaff "Staff" / full_name, colSumCount "Services" / services_count (centered),
    colSumTotal "Total commission" / total_commission (N2, right), colSumUnpaid "Unpaid" /
    unpaid_amount (N2, right), colSumPaid "Paid" / paid_amount (N2, right).
    Bottom card pnlDetailsGrid with dgvDetails (MultiSelect = true): colDetCommissionId (hidden,
    commission_id), colDetDate "Date" / service_date (MMM dd, yyyy), colDetStaff "Staff" /
    staff_name, colDetClient "Client" / client_name, colDetService "Service" / service_name,
    colDetBase "Service amount" / base_amount (N2, right), colDetRate "Rate %" / commission_rate
    (N2, right), colDetAmount "Commission" / commission_amount (N2, right), colDetStatus "Payout" /
    payout_status.
    Events wired: Load, btnSearch_Click, btnMarkPaid_Click, btnExport_Click, btnPrint_Click,
    dgvSummary_SelectionChanged, dgvDetails_CellFormatting.
  - Controls\ucReports (.Designer.cs + .cs): AppBack page, 1000 x 640, Admin only.
    pnlFilter (Dock Top, 74 px, Surface, FixedSingle) with the AutoSize flpFilter: lblPreset
    "Period" + cboPreset (Today / This Week / This Month / Last Month / Custom), lblFrom + dtpFrom,
    lblTo + dtpTo, btnGenerate "Generate" (primary), btnExport "Export CSV" and btnPrint "Print"
    (secondary).
    tabReports (Dock Fill) with four tabs (every tab is AppBack with 10/8 padding):
    tabSales "Sales" = lblSalesTotal (TitleFont 22 pt, Primary gold, "Net sales 0.00") on top,
    dgvSales filling the middle (colSalesCategory "Service category" / category_name;
    colSalesCount "Services" / services_count, centered; colSalesGross "Gross" / gross_sales;
    colSalesDiscount "Discounts" / discount_total; colSalesNet "Net sales" / net_sales - all N2 and
    right aligned) and pnlMethod (Dock Bottom, 208 px) with lblMethodTitle "Collected by payment
    method" (Semibold) and the smaller dgvSalesByMethod (colMethodName "Payment method" /
    payment_method; colMethodCount "Payments" / payments_count, centered; colMethodAmount
    "Amount" / amount, N2 right).
    tabTopServices "Top Services" = dgvTopServices filling the tab (colRank "#" / rank_no,
    centered; colTopService "Service" / service_name; colTopCategory "Category" / category_name;
    colTopCount "Times availed" / times_availed, centered; colTopRevenue "Revenue" / revenue,
    N2 right).
    tabCommission "Commissions" = lblCommissionTotal (TitleFont, Primary, "Total commission 0.00")
    on top and dgvCommissionReport (colRepStaff "Staff" / full_name; colRepCount "Services" /
    services_count, centered; colRepTotal "Total" / total_commission; colRepUnpaid "Unpaid" /
    unpaid_amount; colRepPaid "Paid" / paid_amount - N2 right).
    tabAppointments "Appointments" = pnlCards (Dock Top, 96 px) with tlpCards and four small white
    cards (lblTotalAppointmentsTitle "Appointments" / lblTotalAppointments dark,
    lblCompletedTitle "Completed" / lblCompletedCount green, lblCancelledTitle "Cancelled" /
    lblCancelledCount red, lblRescheduledTitle "Rescheduled" / lblRescheduledCount amber), then
    tlpApptGrids (Dock Fill, two 50 % columns with a 6 px gap) holding pnlStatusGrid with
    dgvAppointmentStatus (colStatusName "Status" / status_label; colStatusTotal "Total" / total,
    centered) and pnlSourceGrid with dgvAppointmentSource (colSourceName "Booking source" /
    booking_source; colSourceTotal "Total" / total, centered).
    Events wired: Load, cboPreset_SelectedIndexChanged, btnGenerate_Click, btnExport_Click,
    btnPrint_Click, tabReports_SelectedIndexChanged and ONE shared money-format handler
    dgvReports_CellFormatting, wired to all four money grids (dgvSales, dgvSalesByMethod,
    dgvTopServices, dgvCommissionReport) - the two Appointments grids hold counts only.
  - Fix rounds from the previews (0 used): both pages rendered with every label complete, no
    overlap and nothing truncated on the first try. The empty Staff / Payout drop-downs and the
    blank grids are expected: they are filled in the logic prompt. The three other Reports tabs
    were also previewed by temporarily setting tabReports.SelectedIndex to 1, 2 and 3 (the
    Designer value is back to 0 = Sales): Top Services, Commissions and Appointments (four cards +
    the Status and Booking source grids side by side) all clean at 1240 x 600.
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 47 passed, 0 failed, 0 skipped.
  --preview checked for ucCommissions (the two toolbar rows with From / To / Staff / Payout and
  Search / Mark Selected as Paid / Export CSV / Print, the three summary cards in red and green,
  the 6-column staff summary and the 9-column commission details grid - nothing overlapping or
  truncated) and ucReports (the filter strip with Period / From / To / Generate / Export CSV /
  Print, the four tabs, "Net sales 0.00", the 5-column sales grid and "Collected by payment
  method" with its 3-column grid). The app was started once and closed again (no error.log).

- Step 18 (this task): the LOGIC for the Billing module (repository, receipt printer, the page and
  the payment dialog). The Designer layouts were NOT restructured; the only Designer touch was
  temporarily setting tabBilling.SelectedIndex = 1 to take the Transactions preview, which is back
  to 0.
  - Data\BillingRepository.cs (the Step 16 methods are unchanged; everything below is new):
    READ: GetQueue(date) (appointment_id, time_range, client_name, services "Name x2", status,
    payment_status, balance_due; the non-cancelled appointments of that day, past days allowed on
    purpose), GetBillHeader(appointmentId) (client_name, appointment_date, start_time, end_time,
    appointment_status, billing_id, promo_id, promo_name, subtotal, discount_amount,
    total_amount, paid_so_far = every payment that is not 'Rejected', balance_due,
    payment_status; null when there is no bill), GetBillLines(appointmentId)
    (appointment_service_id, service_name, quantity, price, session_label "2 / 8" or empty,
    technician_id 0 when none was requested, line_status), GetTransactions(from, to, search,
    method) (billing_id, payment_date = the latest payment, client_name, services, total_amount,
    amount_paid = the sum of the non-rejected payments, payment_method of the latest payment,
    reference_number, staff_names, payment_status; only bills with at least one non-rejected
    payment; "All" = every method; the whole "to" day belongs to the range) and
    GetReceiptData(billingId) (the header fields repeated on every service line plus
    staff_names; empty table when the bill has no lines). Every comma list is built in C# with ONE
    query per list (no STRING_AGG), as in AppointmentRepository.
    ApplyPromo(appointmentId, promoId) returns an empty text or a readable sentence. It is refused
    when the bill already has a promo, when anything was paid (a promo is never an installment) or
    when the appointment is completed; the discount ALWAYS comes from
    PromotionRepository.CalculateDiscount, and discount / total / balance are written in one
    transaction. It logs "Apply Promo".
    Checkout(appointmentId, technicianByLine, method, reference, verified, tendered) returns the
    billing_id and does everything in ONE Db.InTransaction. It validates first (not Cancelled /
    Completed, date today or earlier, a technician on every line, and while balance > 0 a method,
    a reference + the tick for GCash / Bank Transfer, tendered >= balance for Cash), then
    (a) a 'Final' Payments row ('Verified') when money is still due, (b) the bill reads
    balance 0 / 'Paid' / downpayment 'Held' -> 'Applied', (c) AppointmentStaff per line (user_id
    replaced by the real technician, 'Completed', inserted when the line had none),
    (d) AppointmentServices 'Completed', (e) Appointments 'Completed', (f)
    ClientPackages.sessions_completed + 1 and 'Completed' at total_sessions, (g) one Commissions
    row per line (10 %, the proportional discount share taken off, package lines use
    price / total_sessions per the business rules, payout 'Unpaid', payment_id = the new payment or
    NULL), (h) the clients.treatment_history line "MMM dd, yyyy - Service name (Staff full name)"
    appended, (i) an AppointmentHistory 'Status Change' row and (j) ActivityLog "Checkout".
  - Helpers\ReceiptPrinter.cs (NEW FILE, registered in LittleEreseSystemV3.csproj as
    <Compile Include="Helpers\ReceiptPrinter.cs" />): internal static Print(billingId, preview)
    draws an 80 mm style receipt (Consolas, 280 printer units wide) with the business name and
    address, "OFFICIAL RECEIPT" + OR-yyyyMMdd-<billing_id>, the date, the client, each service line
    (name, "qty x price", amount), subtotal, discount, paid upfront, total, the method with its
    reference, the attending staff and "Thank you for trusting Little Erese's.". It shows a
    PrintPreviewDialog when preview is true and prints otherwise, and it never throws (Messages +
    Db.Describe on failure). POLISH marks on the header text.
  - Helpers\Validators.cs: added IsValidReference (letters and digits, 6 to 30 characters) with a
    // POLISH note. No existing rule changed.
  - Controls\ucBilling.cs: Load styles the three grids and the six buttons, dtpQueueDate = today,
    the Transactions range = this month (Admin) or today and locked (Staff), cboMethod "All", the
    technician column = "Select staff" + UserRepository.GetActiveStaff() and cboPromo = "No promo"
    + PromotionRepository.GetActiveValid() with the discount in the caption. A DataError handler
    keeps the staff drop-down from showing a raw .NET dialog.
    Selecting a queue row loads the bill (client, "Schedule: date  start - end (status)", the lines
    with the requested technician preselected, the five totals) and sets the buttons: the promo box
    only while the bill has no promo, nothing was paid and the appointment is not finished; the
    lines grid is locked once the appointment is completed / cancelled.
    cboPromo_SelectedIndexChanged calls ApplyPromo, warns with the message when it is refused and
    restores the previous choice. btnCollect validates the appointment and every line's staff,
    opens PaymentForm when a balance is left (or asks Messages.Confirm when nothing is due), calls
    Checkout, shows the change for cash, reloads the queue and the bill and offers the receipt with
    ReceiptPrinter.Print(billingId, true). btnPrintReceipt prints the selected bill.
    dgvQueue_CellFormatting colors Status and Payment with Theme.StatusColor (white on a selected
    row). Staff cannot search a range (the two pickers are disabled), btnSearch validates the range,
    lblTotals shows "<n> transactions - <sum of amount_paid>", btnReprint prints the selected row's
    receipt and dgvTransactions_CellFormatting colors the Status cell.
  - Forms\PaymentForm.cs: public AmountDue / ClientName in, Method / Reference / Verified /
    Tendered out. Load styles the two buttons, shows the name and the amount, picks Cash and fills
    the change row. Cash enables the tendered box; GCash / Bank Transfer enable the reference box
    and the tick and clear the other one. txtTendered_TextChanged keeps lblChange at "never
    negative". btnConfirm validates through Validators + errorProvider1 (cash needs a number
    >= AmountDue, GCash / bank need a valid reference and the tick) and then sets DialogResult.OK;
    btnCancel returns Cancel.
  - Helpers\SmokeTest.cs: the three read-only Billing checks (GetQueue(today) runs,
    GetTransactions(first day of month, today, "", "All") runs, GetBillHeader(0) returns no row).
    Nothing is written.
  - Self-test (temporary, REMOVED again - no --billingtest switch and no BillingSelfTest.cs are
    left in Program.cs or the .csproj): booked one walk-in appointment today for the first demo
    client with one normal service (Traptox, 5,000.00) and checked it out as Cash. Every check
    PASSED: Billing = balance 0.00 / Paid, one Payments row 'Final' / Cash / 5,000.00 / 'Verified',
    AppointmentServices 'Completed', Appointments 'Completed', Commissions = 500.00 (10 %) / rate
    10.00 / 'Unpaid' for the technician, and Clients.treatment_history gained
    "Oct 24, 2026 - Traptox (Aesthetician 1)". Every row was deleted again (Commissions, Payments,
    Billing, AppointmentHistory, AppointmentStaff, AppointmentRooms, AppointmentServices,
    Appointments, the treatment_history line and the ActivityLog rows) and the cleanup check
    reported nothing left behind; the database is back to 0 appointments.
  - Assumptions: a refund / rejected-payment screen is not part of this step (the Payments history
    stays as it is); Checkout stores the final payment as 'Verified' because the counter only
    confirms money it has seen (the tick is still required for GCash / the bank); the receipt shows
    the current date, which is what the counter hands to the client.
  - LittleEreseSystemV3.csproj WAS edited: one line added,
    <Compile Include="Helpers\ReceiptPrinter.cs" /> (the temporary self-test file was added and
    removed again).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 47 passed, 0 failed, 0 skipped.
  --preview checked for ucBilling (Checkout: the queue date / Today / Refresh and "0 in the queue",
  the 7-column queue grid and the bill card with "Select an appointment", the 7-column lines grid,
  the promo row, the five totals and the two buttons; Transactions, previewed by temporarily
  selecting tab 1: From / To / Client / Method, Search + Print Receipt,
  "0 transactions - P0.00" and the 9-column payments grid - nothing overlapping or truncated) and
  PaymentForm (the name, the amount due, the method, reference, tendered, change, the verification
  tick, the note and the footer, all inside 460 x 560). The app was started once and closed again
  (no error.log).

- Step 17: Layout only (Designer work) for the Billing page and the payment dialog.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt. Both files were empty shells before.
  - Controls\ucBilling (.Designer.cs + .cs): AppBack page, 1000 x 640, holding tabBilling
    (Dock Fill) with two tabs.
    Tab "Checkout" (tabCheckout) = splitMain (Vertical, SplitterDistance 394 = about 40 % of the
    986 px, Panel1MinSize 300, Panel2MinSize 340). LEFT panel: pnlQueueToolbar (Dock Top, 76 px,
    Surface, FixedSingle) with flpQueueTop (lblQueueDate "Date" AutoSize, dtpQueueDate Short
    132 px, btnQueueToday "Today", btnRefresh "Refresh", secondary look) and lblQueueCount
    ("0 in the queue", SmallFont, TextMuted); below it the card pnlQueueGrid (Surface,
    FixedSingle, 1 px padding) with dgvQueue (Dock Fill, Theme.StyleGrid look as literals,
    AutoGenerateColumns = false): colQueueAppointmentId (hidden, appointment_id), colQueueTime
    "Time" / time_range, colQueueClient "Client" / client_name, colQueueServices "Services" /
    services, colQueueStatus "Status" / status, colQueuePayment "Payment" / payment_status,
    colQueueBalance "Balance" / balance_due (N2, right aligned). Fill weights 17/22/24/13/12/14.
    RIGHT panel: the scrollable card pnlBill (Surface, FixedSingle, AutoScroll, 16 px padding)
    with the AutoSize tlpBill, one full-width row per block: lblBillClient (HeadingFont 16 pt,
    placeholder "Select an appointment"), lblBillSchedule (TextMuted, "Schedule: -"),
    dgvBillLines (150 px; Theme.StyleGrid literals; the ONLY editable column is the
    DataGridViewComboBoxColumn colLineTech "Attending staff" / technician_id) with colLineId
    (hidden, appointment_service_id), colLineService "Service" / service_name, colLineQty "Qty" /
    quantity (centered), colLinePrice "Price" / price (N2, right), colLineSession "Session" /
    session_label, colLineStatus "Status" / line_status; flpPromo (lblPromo + cboPromo DropDownList
    282 px), lblPromoNote (muted, 2 lines), tlpTotals (5 rows: Subtotal / Discount / "Paid so far" /
    Total / "Balance due" + lblBalance in TitleFont 22 pt Primary gold, right aligned), btnCollect
    "Collect Payment and Complete" (primary look, full width, 48 px) and btnPrintReceipt
    "Print Receipt" (secondary look).
    Tab "Transactions" (tabTransactions): pnlTranToolbar (Dock Top, 92 px, Surface, FixedSingle)
    with two rows - row 1 flpTranFilters (lblFrom + dtpFrom, lblTo + dtpTo, lblSearch "Client" +
    txtSearch 180 px, lblMethod + cboMethod with All / Cash / GCash / Bank Transfer, all labels
    AutoSize); row 2 flpTranButtons (btnSearch "Search" primary, btnReprint "Print Receipt"
    secondary) and the right-aligned lblTotals (BoldFont, "0 transactions - 0.00"). Below it the
    card pnlTranGrid with dgvTransactions (Theme.StyleGrid literals, AutoGenerateColumns = false):
    colTranBillingId (hidden, billing_id), colTranDate "Date" / payment_date (MMM dd, yyyy h:mm tt),
    colTranClient "Client" / client_name, colTranServices "Services" / services, colTranTotal
    "Total" / total_amount (N2, right), colTranPaid "Paid" / amount_paid (N2, right), colTranMethod
    "Method" / payment_method, colTranRef "Reference" / reference_number, colTranStaff "Staff" /
    staff_names, colTranStatus "Status" / payment_status.
    Events wired: Load, dtpQueueDate_ValueChanged, btnQueueToday_Click, btnRefresh_Click,
    dgvQueue_SelectionChanged, dgvQueue_CellFormatting, cboPromo_SelectedIndexChanged,
    dgvBillLines_CellValueChanged, dgvBillLines_CurrentCellDirtyStateChanged, btnCollect_Click,
    btnPrintReceipt_Click, btnSearch_Click, btnReprint_Click, dgvTransactions_CellFormatting.
  - Forms\PaymentForm (.Designer.cs + .cs): 460 x 560 client (the prompt's size, already inside
    the "dialogs at most 1000 x 640" rule), FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnConfirm, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1,
    white header with lblTitle "Collect Payment" (HeadingFont) + 1 px Border line, white footer,
    AutoScroll content so nothing can be cut on a 768 px screen.
    Content, top to bottom = TabIndex order: lblClientName (Semibold, "-"), lblAmountDueTitle
    "Amount due", lblAmountDue (TitleFont 22 pt, Primary gold), lblMethod + cboMethod
    (DropDownList, Cash / GCash / Bank Transfer), lblReference + txtReference, lblTendered +
    txtTendered, lblChangeTitle "Change" + lblChange (Success green, 14 pt), chkVerified
    "I verified this payment in the GCash / bank app", lblNote (muted, wraps: the system never
    contacts GCash or a bank), footer btnConfirm "Confirm Payment" (primary look, 144 px) and
    btnCancel "Cancel" (secondary look).
    Events wired: Load, btnConfirm_Click, btnCancel_Click, cboMethod_SelectedIndexChanged,
    txtTendered_TextChanged.
  - Fix rounds from the previews (2 used): (1) the queue date picker was 120 px wide and cut
    "10/24/2026", so dtpQueueDate is 132 px (Today / Refresh moved 12 px right); the Transactions
    toolbar filters were crowded, so dtpFrom / txtSearch / cboMethod were re-spaced to leave a
    gap before every label; (2) the Transactions "Date" column could not show
    "Oct 24, 2026 10:30 AM" at Fill weight 15, so colTranDate now has MinimumWidth 155 and
    Width 160. The Transactions tab was also previewed once by temporarily selecting it
    (Designer value is back to SelectedIndex 0 = Checkout) to confirm its layout.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin and obj deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 44 passed, 0 failed, 0 skipped.
  --preview checked for ucBilling (Checkout: the queue card with date / Today / Refresh and the
  7-column grid, the bill card with the heading, schedule, the 7-column lines grid, promo, note,
  the five totals and the two buttons; Transactions: the From / To / Client / Method row, Search +
  Print Receipt, the "0 transactions - 0.00" total and the 9-column grid) and PaymentForm (the
  amount due, method, reference, tendered, change, the verification tick, the note and the footer,
  nothing overlapping or truncated).
  The app was started once and closed again (no error.log).

- Step 16 (this task): the LOGIC for the Appointments module (transaction helpers, repositories,
  the day page and the booking / reschedule / cancel dialog). The Designer layout was NOT
  restructured; no small Designer fix was needed.
  - Data\Db.cs (additive only, no existing signature changed): InTransaction(work) opens a
    connection, begins a transaction, runs the work, commits it, rolls it back and rethrows when
    anything inside throws; plus Query / Execute / Scalar overloads that take
    (SqlConnection, SqlTransaction, sql, params) and fall back to the normal helper when the
    connection is null. WHY: booking writes rows in six tables and must never be left half saved.
  - Data\BillingRepository.cs (was an empty class): CreateBilling(conn, tx, ...) (returns the new
    billing_id), AddPayment(conn, tx, ...) (Payments row + the bill is re-balanced),
    GetByAppointment (bill + promo name + the paid total) and RefreshBalance(conn, tx, billingId),
    which is the ONE place the rule "balance = total - every payment that was not rejected" and the
    Unpaid / Partial / Paid status are written down.
  - Data\AppointmentRepository.cs (was an empty class): the booking rules as constants
    (30 per day, 10 massage per day, 3 days notice, massage categories start with "Massage",
    aesthetic 10:00 AM - 8:00 PM, massage 1:00 PM - 10:00 PM).
    READ: GetByDate(date, status, search) (appointment_id, time_range, client_name, services
    "Name x2", staff_names "Unassigned", room_names, booking_source, status, payment_status; the
    comma lists are built in C# so the app does not need STRING_AGG), GetServiceLines,
    GetById, GetTimeSlots (24 slots, 10:00 AM to 9:30 PM), GetRooms, GetRoomsForCategory,
    GetLinesForBooking (the lines in the shape Create expects), GetDayCounts (the toolbar strip),
    TryGetPackagePlan (which package row a booking will use), TotalDuration, Subtotal and
    RequiredNow. RequiredNow is the ONE place the money rule lives: a promotion is paid in FULL
    whatever the source, otherwise Messenger / Text / Phone pay 50% of the subtotal and a walk-in
    without a promotion pays 0 now.
    VALIDATION: ValidateBooking(...) returns a List<string> of readable problems: client chosen,
    at least one service, the date not in the past (today only while the start time is ahead),
    every service inside its category's opening hours and the end time too, the daily capacity
    (30 / 10 massage, ignoring the appointment being rescheduled), a fitting room for every line
    (auto-assigned from RoomServiceCategories, capacity aware), no overlapping service for a
    requested technician, the upfront amount / reference / verified tick for GCash and bank
    payments, and a promotion that is valid AND applies to at least one selected service
    (always through PromotionRepository.CalculateDiscount).
    WRITE (each in ONE transaction): Create (Appointments + AppointmentServices with the package
    rules - a new ClientPackages row when needed, the whole package price on session 1 and 0.00 on
    later sessions - AppointmentStaff 'Assigned', AppointmentRooms 'Reserved', Billing, the
    upfront Payments row (Downpayment, or Final for a promo; Verified only with the tick), the
    'Created' history row and the ActivityLog entry, and it returns the new appointment_id);
    Reschedule (re-validates itself out of the way, applies the 3-day Admin rule, updates the date
    and the times, re-assigns the rooms, keeps the deposit 'Held' and writes the 'Rescheduled'
    history row with the old date, the old time and the reason); Cancel (3-day Admin rule,
    'Cancelled' + cancelled_at, services 'Cancelled', rooms 'Released', the money forfeited
    (downpayment_status 'Forfeited', or payment_status 'Forfeited' for a prepaid promo), a package
    opened for this booking with no completed session set to 'Expired', and the 'Cancelled' history
    row with the reason); SetStatus ('Upcoming' -> 'Ongoing' only on the appointment's own day,
    with a 'Status Change' history row).
  - Controls\ucAppointments.cs: Load styles the two grids and the five buttons with Theme, starts
    the status filter on "All" and the date on today (or on the public StartDate the caller set)
    and loads the day. The date buttons and Today move the date; the status filter and the client
    search reload (a _loading flag stops the events from firing twice). Selecting a row fills the
    card (client + status, "MMM dd, yyyy - time range (source)", the payment line
    "Paid X of Y - Balance Z" with the promo and its discount when there is one, the notes and the
    service lines) and enables the buttons by status: Start only for an Upcoming appointment TODAY,
    Reschedule / Cancel only for Upcoming. CellFormatting colors Status and Payment with
    Theme.StatusColor (and keeps white on a selected row), and the detail grid colors its Status.
    btnNew opens AppointmentForm; Reschedule opens the same form with RescheduleId set and moves
    the page to the new day afterwards; Cancel warns first ("the money paid upfront is forfeited"),
    warns again inside the 3-day window and asks for the reason with AppointmentForm.AskReason;
    a Staff user is stopped by Messages.Warn inside that window. lblSummary shows
    "N appointments - N massage". A double click on a row does nothing (the details are already
    there and a booking must never move by accident). Staff may do everything on this page.
  - Forms\AppointmentForm.cs: Load fills cboClient (ClientRepository.GetForCombo), cboTime
    (GetTimeSlots), cboSource, cboCategoryFilter ("All categories" + the real ones, the catalog is
    filtered in memory), dgvCatalog (GetActiveForBooking), cboPromo ("No promo" + GetActiveValid
    with the discount in the caption), the technician combo column (GetActiveStaff + a blank
    "Any available") and cboPayMethod, and sets dtpDate.MinDate to today. btnNewClient opens
    ClientEditForm and selects the new client. Adding a service appends a line with quantity 1, its
    duration and the package label ("Session 2 of 3", "New package (1 of 3)") and price from
    AppointmentRepository.TryGetPackagePlan (a new package charges the whole price, a later session
    of an existing package is 0.00); a service already in the list only gets +1 quantity when it is
    not a package (a package is refused with a short message, because it has one price and a fixed
    number of sessions). Remove deletes the selected line (looked up by service id, because the
    grid shows a sorted view). Every change recalculates the end time ("Ends 3:00 PM (60 min)"),
    the subtotal, the discount (PromotionRepository.CalculateDiscount; lblPromoNote names the
    eligible services or shows why the promo gives nothing), the total, the amount required now and
    fills txtPayAmount with it; it also runs the live availability check into lblSlotInfo (green
    "This slot is free: ...", amber first problem, muted for the two "your turn" messages). A
    letter typed into Qty is put back to 1 (CellValueChanged, with a DataError handler so no raw
    .NET dialog appears). grpPayment is disabled when nothing is required and always disabled in
    reschedule mode; GCash / Bank Transfer enable the reference box and the verified tick and cash
    clears them. Save validates through errorProvider1 (client, at least one service, a numeric
    amount), then shows EVERY problem from ValidateBooking in one Messages.Warn, then Create +
    a success message + DialogResult.OK. Reschedule mode loads the appointment, shows its promo,
    opens on its own date and locks everything except dtpDate and cboTime, retitles to
    "Reschedule Appointment" / "Save Changes" and asks for a reason (AskReason) when an Admin moves
    it inside the 3-day window. AskReason is a small modal built in code (as the prompt asked),
    with a multiline box, OK and Cancel.
  - Helpers\SmokeTest.cs: the three read-only checks (GetByDate(today, "All", "") runs,
    GetTimeSlots() returns exactly 24 slots, ValidateBooking without a client reports a problem).
    Nothing is written.
  - Self-test (temporary, REMOVED again - no --apptest switch is left in Program.cs or SmokeTest):
    round 1 created a walk-in appointment 10 days ahead at 2:00 PM for an existing demo client with
    one non-package service, checked the Billing row (5000.00 / Unpaid / None), the 'Created'
    history row, the day list ("2:00 PM - 2:30 PM | Cunanan, Jhon Rey | Traptox | Unassigned | - |
    Unpaid"), rescheduled it 2 days later at 3:30 PM (date and times moved, 1 'Rescheduled' row,
    still 'Upcoming'), saw "Start Service" correctly refused for a future day, cancelled it and
    checked 'Cancelled' on the appointment and the service plus 1 'Cancelled' history row: PASS.
    Round 2 checked the money rules: walk-in required now 0.00, Messenger 2500.00, a Messenger
    booking with nothing paid was refused, its bill read downpayment 2500.00 / Held / Partial with
    one 'Downpayment' payment, GCash without a reference and a tick was refused, and the promo
    booking (15% of 5000 = 750 off, 4250 total) read 'Applied' / 'Paid' / balance 0.00 with one
    'Final' payment 'Verified': PASS. Round 3 drove the real dialog without saving (78 catalog rows,
    24 slots, the client's consent + allergy alert, Add produced "Barbie Arms Botox | qty 1 |
    5000.00 | 30 min", "Ends 2:30 PM (30 min)", "This slot is free", the promo note explained that
    the promo applies to other services, and a second Add only raised the quantity to 2 with
    "Ends 3:00 PM"): PASS.
    Every row the self-test created was deleted again (Appointments, Billing, Payments,
    AppointmentHistory, AppointmentRooms, AppointmentStaff, AppointmentServices) and the two
    ActivityLog rows it wrote were removed too. The database now has 0 appointments, 0 billing rows,
    0 payments, 0 history rows, 0 rooms, 0 appointment services. Nothing was left behind.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task; Db, BillingRepository and
    AppointmentRepository already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 44 passed, 0 failed, 0 skipped (the 3 new Appointment checks all PASS).
  --preview checked again for ucAppointments (the toolbar, the 9-column day grid reading
  "0 appointments - 0 massage" and the details card with "Select an appointment") and
  AppointmentForm (client row, schedule, the catalog with prices, the selected-services grid, the
  totals, the Payment group and the footer, nothing overlapping or truncated).
  The app was started once and closed again (no error.log).

- Step 15 (this task): Layout only (Designer work) for the Appointments page and the booking dialog.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt. Both files were empty shells before.
  - Controls\ucAppointments (.Designer.cs + .cs): AppBack page, 1000 x 640. pnlToolbar (Dock Top,
    92 px, Surface, two rows built with tlpToolbar) -
    row 1 flpNav (left): btnPrevDay + btnNextDay (glyph buttons, Font "Segoe MDL2 Assets" 12 pt,
    Text \uE76B / \uE76C, 34 x 30, secondary look), dtpDate (Format Short, 130 px),
    btnToday "Today", lblStatus + cboStatus (All, Upcoming, Ongoing, Completed, Cancelled,
    132 px), lblSearch "Client" + txtSearch (160 px, client name);
    row 2: flpButtons (btnNew "New Appointment" primary, btnStart "Start Service",
    btnReschedule "Reschedule", btnCancelAppointment "Cancel Appointment" danger, btnRefresh,
    all secondary except New and Cancel Appointment) and, right aligned in the same row,
    lblSummary (SmallFont, TextMuted, "0 appointments - 0 massage").
    Below it splitMain (Dock Fill, Orientation Horizontal, SplitterDistance 328 = about 60% of
    the 548 px body, Panel1MinSize 180, Panel2MinSize 150).
    Panel1: the card pnlGrid (Surface, FixedSingle, 1 px padding) with dgvAppointments (Dock Fill,
    Theme.StyleGrid look copied as literals, AutoGenerateColumns = false). Columns: colAppointmentId
    (hidden, appointment_id), colTime "Time" / time_range, colClient "Client" / client_name,
    colServices "Services" / services, colTech "Technician" / staff_names, colRoom "Room" /
    room_names, colSource "Source" / booking_source, colStatus "Status" / status, colPayment
    "Payment" / payment_status (Fill weights 13/17/27/16/11/9/11/11).
    Panel2: the details card pnlDetails (Surface, FixedSingle, 16 px padding) with tlpDetails
    (2 columns: 356 px fixed + rest) holding tlpDetailInfo (lblDetailClient BoldFont,
    lblDetailSchedule, lblDetailPayment, spanned over both rows), dgvDetailServices
    (colDetailService "Service" / service_name; colDetailQty "Qty" / quantity, centered;
    colDetailPrice "Price" / price N2; colDetailStaff "Technician" / staff_name;
    colDetailStatus "Status" / status) and below it tlpNotes (lblNotesTitle "Notes" +
    txtDetailNotes read-only multiline).
    Events wired: Load, btnPrevDay_Click, btnNextDay_Click, btnToday_Click, dtpDate_ValueChanged,
    cboStatus_SelectedIndexChanged, txtSearch_TextChanged, btnNew_Click, btnStart_Click,
    btnReschedule_Click, btnCancelAppointment_Click, btnRefresh_Click,
    dgvAppointments_SelectionChanged, dgvAppointments_CellFormatting,
    dgvAppointments_CellDoubleClick, dgvDetailServices_CellFormatting.
  - Forms\AppointmentForm (.Designer.cs + .cs): FixedDialog, CenterParent, no minimize/maximize,
    AcceptButton btnSave, CancelButton + DialogResult.Cancel on btnCancel, errorProvider1,
    white header (lblTitle "New Appointment" HeadingFont) + 1 px Border line, white footer and the
    AppBack content inside pnlScroll (AutoScroll = true). Client 980 x 640: the prompt's
    1040 x 720 was shrunk to satisfy the "dialogs at most 1000 x 640, taller content goes into a
    scrollable panel" rule of Additions (Step 3), and everything that does not fit on a
    1366 x 768 screen is reached with the scrollbar.
    Content (tlpContent, one full-width row per block, visual order = TabIndex order):
    flpClient (lblClient + cboClient 340 px with AutoCompleteMode SuggestAppend /
    AutoCompleteSource ListItems + btnNewClient "+ New"), lblClientInfo (muted: consent status and
    medical alerts), flpSchedule (lblDate + dtpDate, lblTime + cboTime DropDownList, lblSource +
    cboSource Walk-in / Messenger / Text / Phone, lblEndTime "Ends 11:30 AM"), lblSlotInfo
    (warning colored, 2 lines: availability), lblCategoryFilter + cboCategoryFilter,
    dgvCatalog (colCatServiceId hidden / service_id; colCatName "Service" / service_name;
    colCatPrice "Price" / price N2; colCatDuration "Min" / duration_minutes),
    btnAddService "Add >" (primary) + btnRemoveService "Remove", dgvSelected
    (colSelServiceId hidden / service_id; colSelName "Service" / service_name; colSelQty "Qty" /
    quantity; colSelPrice "Price" / price N2; colSelDuration "Min" / duration_minutes;
    colSelPackage "Package" / package_label; colSelTechnician a DataGridViewComboBoxColumn
    "Technician" / technician_id) - only Qty and Technician are editable,
    lblPromo + cboPromo (DropDownList), lblPromoNote (muted, 2 lines), lblSubtotal, lblDiscount,
    lblTotal (BoldFont), lblRequired (Success green: the amount required now),
    grpPayment "Payment now" (lblPayMethod + cboPayMethod Cash / GCash / Bank Transfer,
    lblPayReference + txtPayReference, lblPayAmount + txtPayAmount,
    chkPayVerified "I verified this payment in the GCash / bank app", muted lblPayNote),
    lblNotes + txtNotes (multiline, 78 px) and the footer btns (btnSave "Book Appointment" primary,
    btnCancel "Close" secondary).
    Events wired: Load, btnSave_Click, btnCancel_Click, btnNewClient_Click, btnAddService_Click,
    btnRemoveService_Click, cboClient_SelectedIndexChanged, dtpDate_ValueChanged,
    cboTime_SelectedIndexChanged, cboSource_SelectedIndexChanged,
    cboCategoryFilter_SelectedIndexChanged, cboPromo_SelectedIndexChanged,
    cboPayMethod_SelectedIndexChanged, dgvCatalog_CellDoubleClick, dgvSelected_CellValueChanged,
    dgvSelected_CurrentCellDirtyStateChanged.
  - Fix rounds from the previews (2 used): (1) on the page the toolbar TabIndex values 8/9 skipped
    numbers between the two rows - they are now 1 and 2, and lblTotal was 11 pt in the dialog
    (taller than its 26 px row), so it went back to BoldFont 10 pt; (2) "Book Appointment" was cut
    in the footer, so the footer buttons moved left and btnSave was widened to 132 px.
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 41 passed, 0 failed, 0 skipped.
  --preview checked for ucAppointments (two toolbar rows with the glyph buttons, date picker,
  Today, the status and client filters and the "0 appointments - 0 massage" summary, the 9-column
  day grid and the details card with "Select an appointment", Schedule, Payment, the 5-column
  service grid and Notes - nothing overlapping or truncated) and AppointmentForm (client row,
  schedule row with "Ends 11:30 AM", the two service grids, promo, totals, Payment now group,
  Notes and the complete footer, scrollbar present for 768 px screens).
  The app was started once and closed again (no error.log).

- Step 14 (this task): the LOGIC for the Clients module (repository + page + the two dialogs).
  The Designer layout was not restructured; two small runtime fixes are listed at the end.
  - Data\ClientRepository.cs (was an empty class): Search(text, consent) returns client_id,
    full_name ("Last, First"), contact_number, birth_date, gender, consent_text ('Signed' /
    'Pending' from consent_status) and last_visit (MAX appointment_date of a Completed
    appointment, NULL when there is none); text matches first name, last name, "Last, First" or
    the contact number; consent is All | Signed | Not signed. GetById (full row for the card),
    GetForCombo() (client_id + display_name "Last, First - contact", for the booking screen
    later), Add (returns the new client_id), Update (stamps updated_at), ContactExists(contact,
    excludeId), GetPackages(clientId) (package_id, service_name, sessions_label "3 / 8",
    expiry_date, status; client 0 = empty table), AddPackage(clientId, serviceId, totalSessions,
    purchaseDate, DateTime? expiry) (returns the new package_id, status 'Active'),
    HasActivePackage(clientId, serviceId) and ExpireOldPackages() (Active + expiry_date < today
    -> 'Expired', returns the row count). Every optional text box is stored as NULL, never as "".
    Clients are never deleted.
  - Controls\ucClients.cs: Load styles both grids and the four buttons with Theme, starts the
    consent filter on "All", calls ExpireOldPackages() and then loads the grid. Selecting a row
    fills the card from GetById: name ("Last, First"), contact, birth date + age, address +
    gender, the consent badge (green "Consent signed" / red "Consent form not signed"), the three
    read-only notes boxes and the packages grid. No selection (or a row that is no longer there)
    clears the card and switches off Edit / Add Package. Search, the consent filter and Refresh
    reload (a _loading flag stops the events from firing twice); Add and Edit open ClientEditForm
    with EditId set and re-select the saved row through GridHelper.SelectRowById; a double click
    is Edit. Add Package opens PackageForm with ClientId set and re-reads the card afterwards so
    the new package shows immediately. CellFormatting: the consent cell is Success (Signed) or
    Warning (Pending), the package Status cell uses Theme.StatusColor; both keep white text on a
    selected row. Staff may add and edit (no delete exists); Add / Edit / Add Package are logged.
  - Forms\ClientEditForm.cs: public EditId (0 = new) and SavedClientId for the caller. Load
    styles the buttons, clears the gender pick and the birth date tick, loads the row when
    EditId > 0 (title becomes "Edit Client") and refreshes the age. dtpBirthDate_ValueChanged
    updates lblAge ("Age: -" while the tick is off). Save validates with Validators +
    errorProvider1 (first and last name via IsValidName, contact via IsValidPhone, birth date not
    in the future and not more than 120 years ago when it is ticked, gender chosen) and asks
    Messages.Confirm("A client with this contact number already exists. Save anyway?") when
    ContactExists is true. Add / Update go through the repository and log "Add" / "Edit".
  - Forms\PackageForm.cs: public ClientId and SavedPackageId. Load shows the client name, fills
    cboService from ServiceRepository.GetActiveForBooking() with "Service - price" (services with
    default_total_sessions > 1 first, all Active services as a fallback when none is flagged as a
    package - POLISH comment left), and sets the purchase date to today with the expiry tick off.
    cboService_SelectedIndexChanged sets nudTotalSessions to the service's default_total_sessions
    and updates lblPriceValue (peso format). Save validates (a client is chosen, a service is
    chosen, a ticked expiry date must be after the purchase date), asks
    Messages.Confirm("This client already has an active package for this service. Add another?")
    when HasActivePackage is true, saves through AddPackage and logs "Add Package".
  - Helpers\SmokeTest.cs: the three read-only checks Search("", "All"), GetForCombo() and
    GetPackages(0) returns an empty table. Nothing is written.
  - Runtime fixes found in the previews (2 rounds, Designer files untouched):
    (1) in ClientEditForm the birth date picker showed a date although it was meant to be empty -
    the value is now set BEFORE the tick is cleared (assigning Value ticks an unchecked picker),
    and lblAge now shows "Age: -"; (2) the same order was applied to the PackageForm expiry picker
    (it is still unchecked in the preview, as intended). The Designer files were NOT edited.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task).
- Standard clean build (bin, obj and .vs deleted first, MSBuild /t:Rebuild): 0 errors, 0 warnings.
  --smoketest: 41 passed, 0 failed, 0 skipped (the 3 new Clients checks all PASS).
  --preview checked for ucClients (toolbar, count "0 clients", the 7-column grid and the details
  card with the "Select a client" placeholder, the gray "No client selected" badge and the
  packages grid - nothing overlapping or truncated), ClientEditForm (empty birth date with
  "Age: -", no truncation, footer complete) and PackageForm (client "-", the service list with
  prices, the package price, 8 sessions and the unchecked expiry box). The app was started once
  and closed again (no error.log).

- Step 13 (this task): Layout only (Designer work) for the Clients page and its two dialogs.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Controls\ucClients (.Designer.cs + .cs): the page is now a SplitContainer splitMain
    (Orientation Vertical, Panel1MinSize 400, Panel2MinSize 320, SplitterDistance 619 = about 62%
    of the 1000 px design width) on the AppBack background.
    LEFT panel (Panel1): pnlToolbar (Dock Top, 92 px, Surface, two rows built with tlpToolbar) -
    row 1 holds flpFilters (lblSearch + txtSearch 140 px, lblConsent + cboConsent 102 px with
    items All / Signed / Not signed) and the right-aligned lblCount ("0 clients", SmallFont,
    TextMuted); row 2 holds the right-aligned flpButtons (btnAdd "Add Client" primary,
    btnEdit "Edit" secondary, btnRefresh "Refresh" secondary). Below it the card pnlGrid
    (Dock Fill, Surface, FixedSingle, 1 px padding) with dgvClients (Dock Fill, Theme.StyleGrid
    look copied as literals, AutoGenerateColumns = false). Columns: colClientId (hidden,
    client_id), colName "Client" / full_name, colContact "Contact" / contact_number,
    colBirth "Birth Date" / birth_date (MMM dd, yyyy), colGender "Gender" / gender,
    colConsent "Consent" / consent_text, colLastVisit "Last Visit" / last_visit (MMM dd, yyyy).
    RIGHT panel (Panel2): the scrollable details card pnlDetails (Dock Fill, Surface, FixedSingle,
    AutoScroll, 16 px padding) holding the white tlpDetails (Dock Top, AutoSize) - lblDetailName
    (HeadingFont, placeholder "Select a client"), lblDetailContact, lblDetailBirth (birth date and
    age), lblDetailAddress, the badge-like lblDetailConsent (Semibold 9 pt, TextMuted, 10/4 px
    padding), lblAllergiesTitle "Allergies" + txtDetailAllergies (read-only, multiline),
    lblMedicalTitle "Medical history" + txtDetailMedical (read-only, multiline),
    lblTreatmentTitle "Treatment history (last 6 months)" + txtDetailTreatment (read-only,
    multiline), lblPackagesTitle "Packages" + dgvPackages (colPackageId hidden / package_id,
    colPackageService "Service" / service_name, colPackageSessions "Sessions" / sessions_label,
    colPackageExpiry "Expires" / expiry_date, colPackageStatus "Status" / status) and the
    right-aligned btnAddPackage "Add Package" (secondary).
    Events wired: Load, btnAdd_Click, btnEdit_Click, btnRefresh_Click, btnAddPackage_Click,
    txtSearch_TextChanged, cboConsent_SelectedIndexChanged, dgvClients_CellDoubleClick,
    dgvClients_SelectionChanged, dgvClients_CellFormatting, dgvPackages_CellFormatting.
  - Forms\ClientEditForm (.Designer.cs): the existing dialog was kept (560 x 640 client,
    FixedDialog, CenterParent, AcceptButton btnSave, CancelButton + DialogResult.Cancel on
    btnCancel, errorProvider1, white header + footer, AutoScroll content). Only the vertical
    rhythm was tightened so the whole form fits 768 px screens without scrolling: the fields now
    run from y = 31 (txtFirstName) to y = 424 (lblTreatmentHistory), the three multiline boxes are
    46 px tall and chkConsentSigned sits at y = 492 above the footer. Every control name, TabIndex,
    event wiring and the "Age: -" label beside the birth date are unchanged.
  - Forms\PackageForm (.Designer.cs): unchanged in this task (460 x 440, same dialog settings,
    lblClient + lblClientName, lblService + cboService (DropDownList), lblPrice + lblPriceValue,
    lblSessions + nudTotalSessions (1..30), lblPurchase + dtpPurchaseDate, lblExpiry +
    dtpExpiryDate (ShowCheckBox, unchecked), the muted lblHint and the footer). The preview was
    taken again to confirm it is still correct.
  - Fix rounds from the previews (2 used): (1) the toolbar controls were measured against the real
    619 px left panel - the four filter controls plus the count label and the three buttons cannot
    fit on one 619 px line without overlapping, so the buttons were moved to a second toolbar row
    (the toolbar is 92 px tall now) and the details card was re-laid out to start at 16,16 with
    48 px text boxes; (2) the ClientEditForm fields still ran past the footer, so the whole form
    was re-spaced (see above) and the AppBack details panel got a 1 px border so it reads as a card.
  - LittleEreseSystemV3.csproj was NOT edited (all four files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped.
  --preview checked for ucClients (split 62/38, toolbar and count on one card, 7 grid columns,
  details card complete with the packages grid and "Add Package", nothing overlapping or
  truncated), ClientEditForm (all fields plus the consent tick above the footer, nothing hidden
  under the scrollbar) and PackageForm (unchanged, unchecked expiry box, hint and footer visible).
  The app was started once and closed again.

- Step 12 (this task): Layout only (Designer work) for the Clients and Packages dialogs.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - Forms\ClientEditForm (.Designer.cs + .cs): the dialog is now 560 x 640 client (the prompt's
    560 x 760 was shrunk to satisfy the "dialogs at most 1000 x 640" rule of Additions (Step 3)),
    FixedDialog, CenterParent, no minimize/maximize, AcceptButton btnSave, CancelButton +
    DialogResult.Cancel on btnCancel, errorProvider1. White header with lblTitle "Client"
    (HeadingFont) + 1 px Border line, AppBack content and a white footer with btnSave (primary)
    and btnCancel (secondary). pnlContent has AutoScroll = true, so the fields that do not fit
    on a 768 px screen can be reached with the scrollbar. Fields, top to bottom = TabIndex order:
    lblFirstName + txtFirstName, lblLastName + txtLastName, lblBirthDate + dtpBirthDate
    (ShowCheckBox true, Format Short) with lblAge ("Age: -") to its right, lblGender + cboGender
    (DropDownList, Female / Male / Other), lblContact + txtContact, lblAddress + txtAddress,
    lblAllergies + txtAllergies (multiline 60 px, vertical scrollbar), lblMedicalHistory +
    txtMedicalHistory (multiline 60 px) and lblTreatmentHistory ("Treatments received in the last
    6 months") + txtTreatmentHistory (multiline 60 px), then chkConsentSigned
    "Consent / waiver form signed (paper form)". Events wired: Load, btnSave_Click,
    btnCancel_Click, dtpBirthDate_ValueChanged.
  - Forms\PackageForm (.Designer.cs + .cs): client 460 x 440 (as in the prompt), same dialog
    settings. lblTitle "Add Package", lblClient + lblClientName (read-only text, bold, "-" until
    the logic prompt), lblService + cboService (DropDownList), lblPrice + lblPriceValue (bold,
    "-"), lblSessions + nudTotalSessions (Min 1, Max 30, value 1), lblPurchase + dtpPurchaseDate,
    lblExpiry + dtpExpiryDate (ShowCheckBox true, Checked = false in the Designer so the preview
    shows the unchecked state) and the muted lblHint "The package price is charged on the first
    session." above the footer. Events wired: Load, btnSave_Click, btnCancel_Click,
    cboService_SelectedIndexChanged.
  - Fix rounds from the previews (2 used): (1) the smoke test failed with "Unable to cast
    DateTimePicker to ISupportInitialize" - the Designer called BeginInit/EndInit on the two
    DateTimePicker controls, which do not implement ISupportInitialize; those four lines were
    removed (NumericUpDown keeps its BeginInit/EndInit). (2) In ClientEditForm the 528 px wide
    fields and labels overlapped the AutoScroll vertical scrollbar, so they were narrowed to
    520 px (and lblAge to 304 px); the "Treatments received in the last 6 months" label is now
    fully visible at 560 x 640.
  - LittleEreseSystemV3.csproj was NOT edited (both files already existed and were listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped.
  --preview checked for ClientEditForm (scrollable content, no overlap, "Age: -" beside the birth
  date, nothing truncated) and PackageForm (all rows inside 460 x 440, unchecked expiry box,
  hint and footer visible). The app was started once and closed again.

- Step 11 (this task): the LOGIC for promotion-to-services (repository + the two screens + schema
  docs). No SQL was run by me: the PromotionServices table already existed in LittleEresesDB.
  - Data\PromotionRepository.cs: every old method still works (RefreshExpired, GetPromotions,
    GetById, GetActiveValid, PromoNameExists, Add, Update, SetStatus). Added:
    * ComputeDiscount(discountType, value, eligibleSubtotal) - the ONLY discount formula.
      Percentage = ROUND(subtotal x value / 100, 2); Fixed = MIN(value, subtotal); never negative
      and never above the subtotal.
    * GetServiceIds(promoId) - the linked service ids; empty list = applies to all (promoId 0 =
      empty).
    * GetPickList(promoId) - every Active service with service_id, service_name, category_name,
      price, is_selected (true when linked to promoId), ordered by category then name.
    * SavePromotion(...) - add/update the promotion AND replace its links in ONE transaction built
      privately in this class with its own SqlConnection + SqlTransaction (no other transaction
      helper). The unique-name check also runs inside that transaction; duplicate ids are skipped
      so the UNIQUE constraint is never hit. Add/Update keep their signatures and still work.
    * CalculateDiscount(promoId, lines, out message) - 0 + a readable message when the promo is
      not Active, not valid today, or has linked services that none of the lines match; otherwise
      the linked lines (or all lines when nothing is linked) are summed and ComputeDiscount runs.
    * GetPromotions / GetActiveValid now also return applies_to ("All services", or the first 3
      service names + " +N more"), built in C# (no STRING_AGG) by a private helper.
  - Forms\PromotionEditForm.cs: a HashSet<int> of ticked service ids (ticks survive the search and
    the category filter). Load fills cboServiceCategory ("All categories" + categories), loads
    GetPickList(EditId) (0 for a new promo), chkApplyAll is ticked when a new promo or an existing
    promo has no links. Ticked = the picker, search and category are disabled, the preview shows
    "Applies to every service on the bill." and the totals block is hidden. Ticking a row goes
    through CurrentCellDirtyStateChanged (CommitEdit) -> CellValueChanged, which updates the set,
    lblSelectedCount and the preview. The preview recalculates on every selection / type / value
    change through ComputeDiscount: Percentage shows regular, promo price = price - ROUND(price x
    value / 100, 2) and the saving per service; Fixed leaves the per-service promo columns empty
    and shows the totals, with "A fixed discount is taken off once per appointment." (the promo
    price is filled in the row when exactly one service is selected). Money uses the peso format
    (set in the Load event, the Designer keeps N2 only). New validation: at least one service when
    chkApplyAll is unticked, and a Fixed value may not exceed the regular total of the selected
    services. Save calls SavePromotion (one transaction) and logs "Add Promotion" / "Edit
    Promotion" with the number of linked services. A duplicate name from the repository is shown
    with Messages.Warn, not as a database error.
  - Controls\ucPromotions.cs: SelectionChanged (and every reload) fills lblPromoSummary. With
    linked services: "Applies to: A, B, C. Regular total P1 -> promo total P2 (save P3)" for a
    Percentage, or "P3 off the combined P1 of: A, B, C" for Fixed. With no links: "Applies to every
    service. 15 % off the whole bill." / "Applies to every service. PHP 100.00 off the whole bill.".
    Staff see the same summary; no selection clears it. The money comes from ComputeDiscount, so
    the formula is still in one place.
  - Database\LittleEreses_Database.sql: the CREATE TABLE PromotionServices was added right after
    Promotions (the same definition as the guarded script: two foreign keys + UNIQUE (promo_id,
    service_id)) and the header comment now says 18 tables.
  - Database\Migration_01_PromotionServices.sql (NEW FILE, already covered by the existing
    <None Include="Database\*.sql" /> in the .csproj): the guarded script
    (IF OBJECT_ID('dbo.PromotionServices') IS NULL BEGIN CREATE TABLE ... END). Text only, not run.
  - README.md: the setup step now mentions the migration for a database created before this
    feature and the 18 tables. PROGRESS.md: this entry.
  - Helpers\SmokeTest.cs: the three new read-only checks (ComputeDiscount Percentage 15 / 1000 =
    150.00, Fixed 500 / 300 = 300.00, GetServiceIds(0) is empty). ExpectedTables was already 18.
  - LittleEreseSystemV3.csproj was NOT edited.
- Standard clean build (bin and obj deleted first): 0 errors, 0 warnings.
  --smoketest: 38 passed, 0 failed, 0 skipped (the 3 new promotion checks all PASS).
  --preview checked for PromotionEditForm (two columns, picker locked while "Apply to all services"
  is ticked, preview note "Applies to every service on the bill.", footer visible) and ucPromotions
  ("Applies to" column filled, summary strip "Applies to every service. 15 % off the whole bill.").
  No layout fix was needed. The app was started once and closed again.

- Step 10 (this task): Layout only (Designer work) for the promotion-to-services feature.
  No logic, no SQL, no file or Session access: every wired event got an empty handler with
  "// TODO(logic prompt)", waiting for the logic prompt.
  - PROJECT_RULES.md: the "- Promos:" business rule was REPLACED with the new text (a promo may be
    linked to services through PromotionServices; no linked services = whole bill; with linked
    services only those appointment lines are eligible and the appointment must contain at least one
    of them; Percentage = eligible subtotal x value / 100; Fixed = value taken off ONCE per
    appointment and never above the eligible subtotal; ALWAYS compute the discount with
    PromotionRepository.CalculateDiscount; full payment upfront, Payments row of type Final,
    Billing.payment_status 'Paid'). One sentence was added to the Database value lists section:
    "Table PromotionServices (promo_service_id, promo_id, service_id) links a promotion to the
    services it applies to."
  - Forms\PromotionEditForm (.Designer.cs + .cs): the dialog is now 980 x 640 client (FixedDialog,
    CenterParent, AcceptButton btnSave, CancelButton btnCancel, errorProvider1 kept). The client was
    460 x 545 before; the prompt's 980 x 640 is inside the "dialogs at most 1000 x 640" rule.
    pnlHeader / pnlFooter are unchanged in look. pnlContent (16 px padding, AutoScroll just in case)
    holds the new tlpMain (2 columns 42% / 58%, the 16 px gap is the left margin of the right
    column). Every existing control name, event and field order was kept.
    LEFT (tlpLeft, one control per row, TabIndex order = visual order): lblName + txtName,
    lblConditions + txtConditions (70 px), lblType + cboType, lblValue + txtValue, lblHint
    "Promos need full payment upfront.", lblStart + dtpStart, lblEnd + dtpEnd, lblStatus + cboStatus.
    RIGHT (tlpRight, 56% / 44%): grpServices "Services in this promotion" with tlpServices -
    chkApplyAll "Apply to all services (no specific services)", lblServiceSearch + txtServiceSearch,
    lblServiceCategory + cboServiceCategory (DropDownList), dgvServicePick (AutoGenerateColumns =
    false; colPickCheck a DataGridViewCheckBoxColumn with an empty header, width 40, the ONLY
    editable column, DataPropertyName is_selected; colPickServiceId hidden / service_id;
    colPickService "Service" / service_name; colPickCategory "Category" / category_name;
    colPickPrice "Price" / price, N2, right aligned), lblSelectedCount "0 services selected" (muted);
    grpPreview "Price preview" with dgvPreview (colPrevService "Service" / service_name,
    colPrevRegular "Regular price" / regular_price, colPrevPromo "Promo price" / promo_price,
    colPrevSaving "You save" / saving, all N2 and read only) and the totals block lblRegularTitle +
    lblRegularTotal, lblDropTitle + lblDropTotal (Success green), lblPromoTitle + lblPromoTotal
    (BoldFont) and lblPreviewNote (muted, wraps).
    New empty handlers in the .cs (each with "// TODO(logic prompt)"):
    chkApplyAll_CheckedChanged, txtServiceSearch_TextChanged,
    cboServiceCategory_SelectedIndexChanged, dgvServicePick_CurrentCellDirtyStateChanged,
    dgvServicePick_CellValueChanged, cboType_SelectedIndexChanged, txtValue_TextChanged.
    btnSave and btnCancel keep their working logic - no existing behaviour was touched.
    Small Designer details: both grids get the Theme.StyleGrid look as literals
    (PrimaryDark header, PrimaryLight selection, Border grid lines).
  - Controls\ucPromotions (.Designer.cs): new column colAppliesTo "Applies to" / applies_to (130 px)
    between colStatus and colConditions; colConditions still AutoSizeMode.Fill (the remaining width).
    New summary strip pnlSummary (Dock Bottom, 56 px, Surface with a Border top line, 16 px side
    padding) with lblPromoSummary (Dock Fill, BodyFont, wraps - placeholder text "No promotion
    selected" until the logic prompt). Nothing else was changed; the .cs file was not touched.
  - Helpers\SmokeTest.cs: ExpectedTables 17 -> 18, because the database now really has 18 tables
    (PromotionServices exists in LittleEresesDB, verified with sys.tables). Without this the smoke
    test failed on correct data. Nothing else changed there.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task).
  - Fix rounds from the previews (2 used): (1) the left column was 15 px taller than the row, so
    cboStatus was pushed under the footer - the label top margins went from 10 px to 8 px, which
    brought the whole column inside 980 x 640; (2) in dgvPreview the headers "Regular price" /
    "Promo price" were cut, so the three money columns were widened to 112 / 104 / 88 px
    (the Service column fills the rest).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked for PromotionEditForm (two columns, both cards complete, no truncated label,
  nothing under the footer, 980 x 640) and ucPromotions (new "Applies to" column visible, summary
  strip at the bottom, toolbar unchanged). The app was started once and closed again.
- Bug fix (this task): truncated toolbar labels, wrong field order in StaffEditForm and
  unreadable status colors on selected rows.
  - STEP 1 (truncated "Searc"): in the toolbars of ucServices, ucPromotions and ucStaffAccounts the
    labels lblSearch / lblCategory / lblStatus and lblCount now use AutoSize = true (the fixed
    48 px width was cutting the last letter of "Search"), and the neighbours were re-spaced so no
    control overlaps: lblSearch x 16, txtSearch x 72 (110 px), lblCategory x 190 / cboCategory x 260
    (110 px), lblStatus x 378 / cboStatus x 432 (services) or x 244 (promotions) or x 274
    (staff accounts). lblCount is AutoSize and right-aligned near the button block (591 / 534 / 406).
  - STEP 2 (StaffEditForm order): the content panel was re-laid out so the visual top-to-bottom
    order equals the TabIndex order - Full name (Tab 0/1), Username (2/3), Role (4/5),
    Contact number (6/7), Password (8/9), Confirm password (10/11), the muted password hint (12),
    Status (13/14), then the footer Cancel / Save buttons. The empty gap under the title is gone
    (the first row now starts at y = 16) and the dialog is still 620 px tall (max 640). Every
    control name and event wiring is unchanged; PasswordOnly mode still looks right (it locks the
    other controls in code, so only the two password boxes stay usable).
  - STEP 3 (selected row readability): in the CellFormatting handlers of ucServices, ucPromotions
    and ucStaffAccounts the Status cell now uses the grid's selection foreground (White) when the
    row is selected instead of the status color, so the text stays readable on the highlight.
  - STEP 4 (duration header): ucServices column header "Duration (min)" is now "Duration"
    (the cells still show "30 min").
  - STEP 5: PROJECT_RULES.md - added to the Layout prompt rules: "Toolbar and form labels use
    AutoSize = true; no text may be truncated in the previews. In dialogs, visual top-to-bottom
    order must equal TabIndex order. Status colors do not apply to selected grid rows."
  - STEP 6: Helpers\SmokeTest.cs - every EXACT count check is now an "at least" check:
    Services >= 77 (expectedMinServices already existed) and GetServices("", 0, "Active") >= 77.
    No other check changed.
  - LittleEreseSystemV3.csproj was NOT edited (no new file in this task; GridHelper.cs was already
    registered in the previous task).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked (1 fix round): ucServices (Search label complete, "Duration" header, 77
  services, selected row readable), ucPromotions (labels complete, 1 promotion), ucStaffAccounts
  (labels complete, 9 staff accounts) and StaffEditForm (fields in the requested order, no gap
  under the title, hint visible, footer complete). The app was started once and closed again.
- Bug fix (this task): "Current cell cannot be set to an invisible cell" after Save in the edit forms.
  - Helpers\GridHelper.cs (NEW FILE, registered in LittleEreseSystemV3.csproj as
    <Compile Include="Helpers\GridHelper.cs" />): internal static class GridHelper with
    SelectRowById(grid, idColumnName, id) (finds the row by id, selects it and puts CurrentCell in the
    FIRST VISIBLE column, scrolls it into view inside a try/catch, returns false when the id is not
    found or there is no visible column) and GetSelectedId(grid, idColumnName) (id of the selected or
    current row, 0 when nothing is selected). Neither method ever throws.
  - Controls\ucServices.cs: SelectService and CurrentServiceId now use GridHelper (was
    dgvServices.CurrentCell = row.Cells[0], the hidden id column).
  - Controls\ucStaffAccounts.cs: SelectUser and CurrentUserId now use GridHelper (same Cells[0] bug).
  - Controls\ucPromotions.cs: SelectPromotion and CurrentPromoId now use GridHelper (it used
    Cells[1], which breaks as soon as the first visible column changes).
  - PROJECT_RULES.md: added to the Logic prompt rules: "Never set DataGridView.CurrentCell to a cell
    of a hidden column. Re-select rows with GridHelper.SelectRowById(grid, idColumnName, id) and read
    the selected id with GridHelper.GetSelectedId(grid, idColumnName)."
  - Helpers\SmokeTest.cs: the two exact "Services rows = 77" checks were changed to "at least 77"
    (like the Users check), because the owner has since added a real service through the app
    ("BOTOX Test"), so the smoke test was failing on correct data. No other check changed.
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped.
  --preview checked for ucStaffAccounts (9 rows selected/highlighted, no crash), ucServices (78 rows,
  selected row highlighted) and ucPromotions. The app was started once and closed again.
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

- Step 9 (this task): the LOGIC for Staff Accounts and Settings (code-behind + repository).
  Designer layout untouched; no small Designer fix was needed.
  - Data\UserRepository.cs keeps Authenticate / UsernameExists / Create / SetPassword and adds:
    GetUsers(search, status) (search matches full name OR username, status "All" = all; returns
    user_id, full_name, username, role, contact_number, status, created_at), GetById(id),
    Update(userId, fullName, role, contact, status), SetStatus(userId, status),
    CountActiveAdmins(), ChangeOwnPassword(userId, currentPassword, newPassword) -> bool (false
    when the current password is wrong) and GetActiveStaff() (role Staff + status Active, returns
    user_id, full_name for the technician drop-downs later).
    The "do not lock yourself out" rules live in one private helper used by Update AND SetStatus:
    you cannot change your own role, you cannot deactivate your own account, and the LAST Active
    Admin can neither be demoted or deactivated. Each rule throws an InvalidOperationException
    with a plain English message that the forms show as a warning (not as a database error).
    UsernameExists now compares with UPPER() on both sides so the unique username rule is
    case-insensitive whatever collation the database uses.
  - Controls\ucStaffAccounts.cs: Load exits early (and says so) unless Session.IsAdmin; it styles
    the grid + all five buttons with Theme, starts the status filter on "All" and loads the grid.
    Search and status reload with a _loading guard. Add / Edit open StaffEditForm (EditId set first,
    the saved row is selected again through SavedUserId). Reset Password opens the same form with
    PasswordOnly = true. SelectionChanged enables Edit / Deactivate / Reset and flips the danger
    button between "Deactivate" and "Activate"; ToggleStatus confirms first, then calls SetStatus.
    CellFormatting colors the Status cell with Theme.StatusColor. Every Add / Edit / Reset Password /
    Activate / Deactivate is logged with ActivityRepository.Log - the password is NEVER logged.
  - Forms\StaffEditForm.cs: public properties EditId (0 = new) and PasswordOnly, plus
    SavedUserId for the caller. Load styles the buttons, defaults to Active / Staff, loads the row
    when EditId > 0 (title "Edit Staff Account", username box read-only) and locks everything
    except the password boxes when PasswordOnly (title "Reset Password"). A new account needs a
    valid, still-free username; editing leaves the password boxes optional (blank = keep the
    current password). Validation uses Validators + errorProvider1 (name, username, role, mobile
    contact, password rule and the confirmation match) and saving goes through
    UserRepository.Create / Update / SetPassword.
  - Controls\ucSettings.cs: Load styles the button, loads picAvatar with
    IconHelper.Load("UserIconYellowishColorTransparent", 96), fills the profile card from Session
    plus the contact number from UserRepository.GetById, shows the assembly version
    (Assembly.GetName().Version, 3 parts) and the database name read out of the App.config
    connection string with SqlConnectionStringBuilder. btnChangePassword_Click validates (all three
    required, new password rule, confirmation equal, new different from current), calls
    ChangeOwnPassword and shows the success message - or "Current password is incorrect." on the
    box itself - then clears the boxes. "Change Password" is logged.
  - Helpers\SmokeTest.cs: three read-only checks (GetUsers("", "All") >= 8 rows,
    CountActiveAdmins() >= 1, GetActiveStaff() >= 7 rows).
  - LittleEreseSystemV3.csproj was NOT edited (all files already listed).
- Standard clean build (bin, obj and .vs deleted first): 0 errors, 0 warnings.
  --smoketest: 35 passed, 0 failed, 0 skipped (the 3 new Staff Accounts checks all PASS:
  GetUsers = 8 rows, CountActiveAdmins = 1, GetActiveStaff = 7 rows).
  --preview checked for ucStaffAccounts (8 rows with green Active status, count "8 staff accounts",
  toolbar complete and nothing truncated), StaffEditForm (no overlap, hint visible, footer complete)
  and ucSettings (avatar loaded, name / role / username / contact filled, "Version 1.0.0",
  "Database: LittleEresesDB"). The app was started once and closed again.

## Next
- The Clients module is COMPLETE (ucClients, ClientEditForm, PackageForm + ClientRepository).
  Next: the Billing / Appointments modules, which reuse ClientRepository.GetForCombo() to pick the
  guest and ClientRepository.HasActivePackage / AddPackage for the package rules (the package price
  is charged on the first session, later sessions of that package are 0.00).
- Promotions are complete (services pick list, price preview, one-transaction save, applies_to and
  the summary strip). What is left for the owner: the Billing / Appointments screens must call
  PromotionRepository.CalculateDiscount(promoId, lines, out message) when a promo is applied, and
  save the full payment upfront (Payments row of type Final, Billing.payment_status 'Paid').
- The Appointments module is COMPLETE for booking, rescheduling, cancelling and "Start Service"
  (AppointmentRepository + the transaction helpers in Db, ucAppointments, AppointmentForm). The
  promo discount and the full-payment-upfront rule are wired through
  PromotionRepository.CalculateDiscount, so the Appointments side of the promotions TODO above is
  done; the same helpers are ready for the Billing screen
  (BillingRepository.GetByAppointment + RefreshBalance).
- The Billing screen still needs its own prompt: BillingRepository currently only creates a bill and
  records a payment INSIDE a booking transaction, so it still needs the read methods of the Billing
  page, the PaymentForm logic and the Completing-a-service step (AppointmentServices 'Completed',
  the package session counter, the commission rows when a line is completed at billing).
- The Dashboard now has its LAYOUT (ucDashboard + ucCalendarMonth) but no logic yet: the KPI
  cards read 0, the calendar grid is empty (its day cells are generated from data) and the two
  lists are blank until the Dashboard logic prompt. File Maintenance and Activity Log still need
  their layout prompts. The Dashboard logic must fill the five cards, both lists and create the
  calendar day cells after Load (never in the Designer).
- ucClients has a layout but it was waiting for its logic prompt.
- POLISH items left for the owner: the lock out limits (5 tries / 30 seconds) in LoginForm,
  the "Log out now?" wording, the PhoneRegex (mobile numbers only), the "-" placeholder for an
  empty contact number in Settings and the version format (3 parts) in Settings.
- POLISH item from the Clients logic: PackageForm lists the services whose
  default_total_sessions is greater than 1 first and only falls back to every Active service when
  none is flagged as a package. Confirm with the owner which services may be sold as packages.

## Known issues
- The booking dialog cannot assign a room yet because RoomServiceCategories is EMPTY in the current
  database: every category has no room linked, so BuildRoomPlan silently assigns none (a POLISH
  comment says why). The owner must link the rooms of the shop to the categories in File
  Maintenance before the room rule and the "Room" column can show anything. Nothing else in the
  booking is affected (the self-test ran with no rooms and the appointment was created, cancelled
  and listed correctly).
- An appointment can only be started ("Start Service") on its own day, so a booking made for a
  future date shows the button switched off - that is the rule, not a bug.
- There is no way to move an appointment from 'Ongoing' to 'Completed' yet: that happens at
  Billing (the next module), which also completes the service lines, the package sessions and the
  commissions.
- The Clients grid has no rows yet because Database\SeedData.sql has no Clients rows, so the
  details card shows its placeholder ("Select a client") until the first client is added with
  "Add Client". This is expected, not a bug.
- "Last Visit" is empty for every client for the same reason (it needs a Completed appointment).
- A client with a birth date but no gender chosen cannot be saved (gender is required by the
  validation), so old rows without a gender must be completed when they are edited.
- The summary strip and the price preview show the FULL service price (Services.price) of the
  linked services, because a promotion is not tied to one appointment there. When Billing applies a
  promo it must use PromotionRepository.CalculateDiscount on the real appointment lines instead.
- None blocking. The remaining page UserControls are still empty shells, so switching to them
  shows an empty content area (this is expected until each page is built).
- The Promotions list is empty at first because the seed data has no Promotions rows; add one with
  "Add Promotion" to see the grid, the status colors and the "15 %" / money formatting. POLISH:
  the owner may want 2-3 sample promos in Database\SeedData.sql.
- Demo staff accounts still have the placeholder names "Aesthetician 1".."Nurse 1" (the schema
  has no job-title column) - rename them in Staff Accounts (now possible: Staff Accounts is live).
- Demo staff accounts have no contact number yet, so the Contact column is empty until the numbers
  are filled in through Staff Accounts / Edit.
- Every account (New or Edit) needs a phone-shaped contact number (09XXXXXXXXX), so an account
  without one must be given one when it is edited. POLISH: make the contact number optional if the
  owner prefers.
- The smoke test only constructs the forms (no Show), so it does not cover the new event handlers;
  they were checked with --preview and by starting the app.
- ucStaffAccounts refuses to run for a Staff user on purpose (it shows a short warning), and
  UserRepository blocks deactivating or demoting your own account and the last Active Admin, so a
  single-Admin shop can never lock itself out.


