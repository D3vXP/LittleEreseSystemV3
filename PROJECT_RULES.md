# PROJECT_RULES.md - Little Erese's Appointment and Billing System

Read this file, PROGRESS.md, Database/LittleEreses_Database.sql and Database/Migrations/*.sql at the start of EVERY chat. Follow every rule below.

## Project facts
- C# Windows Forms, .NET Framework 4.8, Visual Studio 2026, old-style .csproj (every file must be listed in it).
- Project / root namespace: LittleEreseSystemV3. Forms: LittleEreseSystemV3.Forms. Controls: LittleEreseSystemV3.Controls. Data: LittleEreseSystemV3.Data. Helpers: LittleEreseSystemV3.Helpers.
- Database: SQL Server LocalDB, server (localdb)\MSSQLLocalDB, database LittleEresesDB, Windows Authentication. The connection string "LittleEreseDb" lives in App.config.
- Offline desktop app, one computer, two roles: Admin and Staff.
- LittleEreses_Database.sql is the BASELINE (18 tables) and is never edited again. Every later schema change is a new numbered file in Database/Migrations, safe to run twice (IF COL_LENGTH / IF OBJECT_ID / IF NOT EXISTS), with no DROP and no data loss. Only a prompt that says so may add a migration.

## Language and library limits (the build breaks if you ignore these)
- C# 7.3 only. NO: using declarations (using var), switch expressions, ??=, "is not", ranges/indices, nullable reference types, records, default interface members, static local functions.
- Framework 4.8 APIs only. NO: Microsoft.Data.SqlClient, System.Text.Json, Span<T>, async Main, File.ReadAllTextAsync, Environment.ProcessPath.
- Database access: System.Data.SqlClient through the Db class only. Always use parameters. No Entity Framework, Dapper or any NuGet package unless the user approves.
- Passwords: Rfc2898DeriveBytes (PBKDF2) in PasswordHasher. Printing: PrintDocument. No Crystal Reports / ReportViewer.

## Designer safety (most important)
- Every form/control is a partial class with .cs + .Designer.cs. Never rename, delete or recreate existing files, classes or namespaces.
- Keep InitializeComponent, the components field and Dispose in every Designer.cs. Controls are created and added ONLY in InitializeComponent, with SuspendLayout()/ResumeLayout(false) around it, and declared as fields at the bottom of the Designer.cs.
- Control names are unique per form. Every event wired in a Designer.cs must have a method with that exact name in the .cs file.
- NEVER touch the database, files or Session inside a constructor or before the first Load. Put data loading in the Load event and start it with: if (DesignTime.IsDesignMode) return;
- No code in a Designer.cs other than what the Visual Studio designer would generate.
- Layout uses Dock, Anchor, TableLayoutPanel and FlowLayoutPanel, not fixed positions only. Screens must work at 1366x768.
- Images are loaded at runtime from the Assets folder through IconHelper. Do NOT hand-edit any .resx file.
- If a new .cs file is truly needed, add it to the .csproj (<Compile Include>), keep UI files with DependentUpon, and tell the user.

## Code style (for junior developers)
- One class per file. Short methods. Comments explain WHY, in plain English.
- Control name prefixes: btn, txt, lbl, cbo, dgv, dtp, chk, pnl, pic, tab, lst, nud, rdo, grp, tlp.
- Business rules and validations live in ONE place each: Validators (input rules and regex) and the Repository classes (database rules). No SQL inside forms.
- Lists come back from repositories as DataTable. Use Db.Get<T>() for NULL-safe reads.
- Show errors with Messages and Db.Describe(ex). Never let an unhandled exception reach the user.
- Mark spots a human may want to adjust with: // POLISH: <what can be adjusted>

## Build and clean-up rules
- After every task: close any running copy of the app, run a clean build, fix every error AND every warning, run the app once, then close it. Report the result.
- You may delete only the bin, obj and .vs folders. Never delete anything else.
- Never run DROP DATABASE, DROP TABLE or any destructive SQL without asking the user. The baseline script runs only if the database does not exist; migrations run automatically at startup.
- If the same error is still there after 3 attempts, stop and report it instead of trying new ideas.
- Do not change files outside the task. Do not refactor unrelated code.

## At the end of every task
- Update PROGRESS.md: what was finished, what is next, known issues.
- Tell the user which forms/controls to open in the Designer to check.

## Additions (Step 1)
- Designer.cs files set colors with literal Color.FromArgb values copied from Theme so the Designer preview matches runtime. Theme methods are for runtime styling in Load events only.
- Exception to "controls only in InitializeComponent": items generated from data (for example the day cells of the calendar) may be created in code, inside a method that runs after Load and starts with: if (DesignTime.IsDesignMode) return; Never create fixed layout controls in code.
- Controls\ucCalendarMonth already exists for the Dashboard month calendar. It is a read-only overview; booking and rescheduling stay in the Appointments module.
- Never run git commands that change the repository (commit, add, reset, checkout, clean, stash, push, pull). Reading git status or git diff is fine.
- Save tokens: do not re-read files you already read in this chat, read only the files the task needs, and summarize build output (error/warning counts and the first few messages) instead of pasting all of it.
- Demo accounts: admin / Admin@123. Staff accounts use the password Staff@123.

## Additions (Step 2)
### Standard clean build (do this at the end of every prompt)
Close any running copy of the app. Delete only bin and obj (also .vs if it is not locked). Build: msbuild LittleEreseSystemV3.csproj /t:Rebuild /p:Configuration=Debug /nologo /v:minimal (find MSBuild with vswhere). Required: 0 errors, 0 warnings. Then run bin\Debug\LittleEreseSystemV3.exe --smoketest and report the SUMMARY line (everything must PASS).
### Layout prompts (Designer work only)
- Edit only the files named in the prompt (.cs and .Designer.cs). No database, file or Session access and no logic. Every event wired in InitializeComponent gets an empty handler in the .cs with the exact signature and the comment // TODO(logic prompt).
- Every form/control keeps a public parameterless constructor that only calls InitializeComponent().
- DataGridView: AutoGenerateColumns = false; columns are defined in the Designer with Name, HeaderText and DataPropertyName set; ID columns Visible = false; ReadOnly = true unless said otherwise.
- Do not set Image properties in the Designer (images load at runtime). Use literal Color.FromArgb values equal to Theme colors, and give buttons/grids the same look as Theme.StylePrimaryButton / StyleSecondaryButton / StyleDangerButton / StyleGrid so the Designer preview matches runtime.
- Use Dock, Anchor, TableLayoutPanel and FlowLayoutPanel so screens work at 1366x768. Pages (UserControls) have BackColor AppBack and a default size of 1000x640; the MainForm docks them to Fill.
- After building, run bin\Debug\LittleEreseSystemV3.exe --preview <ClassName>, open previews\<ClassName>.png, and fix overlaps, truncated text and misaligned controls (maximum 2 fix rounds).
- Toolbar and form labels use AutoSize = true; no text may be truncated in the previews. In dialogs, visual top-to-bottom order must equal TabIndex order. Status colors do not apply to selected grid rows.
### Logic prompts (code-behind and repositories)
- Do not restructure the Designer layout. Small Designer fixes (a missing control or column) are allowed and must be mentioned in the report.
- Query column aliases must match the DataPropertyName values in the Designer exactly.
- Load data in the Load event, guarded by: if (DesignTime.IsDesignMode) return;
- Repository methods use Db helpers and parameters only and return DataTable / DataRow / scalars. Forms contain no SQL. Wrap repository calls in try/catch and show Messages.DbError(ex).
- All input validation uses Validators and an ErrorProvider (errorProvider1). Add new patterns to Validators, never inline.
- Log important actions with ActivityRepository.Log(Session.UserId, action, module, description).
- Edit forms (ClientEditForm, ServiceEditForm, ...) keep a parameterless constructor and expose public properties (for example public int EditId, 0 = new) that the caller sets before ShowDialog(); data loads in the Load event.
- Add // POLISH: comments where I may want to adjust text, limits or rules.
- Extend Helpers\SmokeTest.cs with up to 3 READ-ONLY checks for the repository methods you add.
- Finish with the standard clean build and update PROGRESS.md.
- Never set DataGridView.CurrentCell to a cell of a hidden column. Re-select rows with GridHelper.SelectRowById(grid, idColumnName, id) and read the selected id with GridHelper.GetSelectedId(grid, idColumnName).
### Roles
- Admin: everything.
- Staff: Dashboard, Appointments, Clients (view/add/edit), Services and Promotions (view only; Promotions shows only Active ones valid today), Billing, Settings (own password).
- Staff must NOT see: Commissions, Reports, Staff Accounts, File Maintenance, Activity Log, or sales amounts on the Dashboard. Hide those navigation buttons and controls, and also check Session.IsAdmin before running admin actions in code.
### Database value lists (CHECK constraints - use exactly these strings)
Users.role: Admin | Staff. Users.status: Active | Inactive. Users.staff_type: Aesthetician | Massage Therapist | Nurse | Other (NULL for Admin). Services.status: Active | Archived. Promotions.discount_type: Percentage | Fixed | Promo Price. Promotions.status: Active | Inactive | Expired. Rooms.status: Available | Unavailable. ClientPackages.status: Active | Completed | Expired. Appointments.booking_source: Walk-in | Messenger | Text | Phone. Appointments.status: Upcoming | Ongoing | Completed | Cancelled | Rescheduled. AppointmentServices.status: Pending | Completed | Cancelled. AppointmentStaff.status: Assigned | Completed | Reassigned. AppointmentRooms.status: Reserved | In Use | Released. AppointmentHistory.change_type: Created | Rescheduled | Cancelled | Status Change. Billing.downpayment_status: None | Held | Applied | Forfeited. Billing.payment_status: Unpaid | Partial | Paid | Forfeited. Payments.payment_type: Downpayment | Final. Payments.payment_method: Cash | GCash | Bank Transfer. Payments.verification_status: Pending | Verified | Rejected. Commissions.payout_status: Unpaid | Paid.
Table PromotionServices (promo_service_id, promo_id, service_id) links a promotion to the services it applies to.
### Business rules
- Business: Little Erese's Aesthetics and Spa, San Nicolas Poblacion, Concepcion, Tarlac. Money: EVERY amount (grid, label, message, receipt) is formatted by Helpers\Money (en-PH, peso sign, two decimals, thousands separator) - never by a private format constant in a screen. Dates: MMM dd, yyyy. Times: h:mm tt.
- Opening hours: aesthetic services 10:00 AM to 8:00 PM; massage services (category name starts with "Massage") 1:00 PM to 10:00 PM. Every service LINE has its own start time and must finish inside the opening hours of its own category. Appointments.start_time is the earliest line start and end_time the latest line end. Start times are on 30-minute marks.
- Capacity per day: at most 30 non-cancelled appointments and at most 10 that contain a massage service.
- Rooms: Aesthetic Lobby = 10 chairs (Botox, Glutathione, Slimming, Nails), Diode Machine Lobby = 3 diode rooms (Hair Removal and Whitening), Facial Room = 3 facial beds (Facial, Face Beautification), Massage Room = 2 (all massage). Linked in RoomServiceCategories. // POLISH: owner confirms the numbers. Every appointment service line has its own start_time / end_time / busy_end_time (NULL = the appointment's times). A room cannot hold more overlapping LINES than its capacity. A technician is busy from the line start to busy_end_time (a gluta drip keeps the room for the whole drip but the aesthetician only 15 minutes). A client cannot have two overlapping lines. All checks live in AvailabilityRepository. Technicians are Users with role Staff and status Active, filtered by Users.staff_type through UserRepository.GetActiveStaffFor(categoryName): a category whose name starts with "Massage" shows the 'Massage Therapist' staff, every other category shows the 'Aesthetician' staff, and a staff typed 'Other' or not typed at all appears in every list. A nurse (staff_type 'Nurse') is NEVER a technician - nurses come from UserRepository.GetActiveNurses (the assessment dropdown). A Staff account always has a job type (the form asks for it with "Choose the job type." and UserRepository refuses an empty one); an Admin has none (NULL).
- Every service duration is a multiple of 30 minutes. A service may have staff_busy_minutes (the technician is busy only that long, the room is busy for the whole duration): gluta drips use 15.
- Services.requires_assessment (bit): the booking asks for the assessing nurse (AppointmentServices.assessed_by). Default set for Botox, Glutathione, Slimming and PRP/Vampire; the owner changes it on the service form.
- Booking: Every appointment needs a 50% down payment (a promo needs the full total), taken in Billing right after the booking. The booking dialog itself takes no money: saving it opens the payment dialog as the next screen (closing it without paying means "pay later"). Consent must be signed before a booking. A service with requires_assessment needs the assessing nurse (assessed_by).
- The booking dialog (Forms\AppointmentForm): every service LINE has its own start time, picked from cboLineTime, which only lists the times that really work (AvailabilityRepository.GetFreeStarts: past and busy times are simply not in the list). A new line is placed on the first free start on or after the end of the latest placed line (PlaceLine); a line whose time stops fitting (another date, client or technician) loses it and is placed again (RevalidateLines). Valid promotions appear as "[PROMO] <promo name>" entries of the category filter; picking one and pressing Add adds ALL its services as separate lines and only ONE promotion is allowed per appointment. A package line shows its weekly plan ("<service>: Session 1 of 4 on <MMM dd> at <time>, then ... same time.") worked out with AvailabilityRepository.PlanSessions; a session that cannot be booked reads "<MMM dd> (Session n) is not available: <problem>".
- Promos: a promotion is a set of services sold for a promotional price (discount_type 'Promo Price'). Valid: status Active and today between start and end. The booking must contain every service of the promo; the discount is regular total minus promo price, once per appointment, always through PromotionRepository.CalculateDiscount. Only ONE promotion per appointment (the booking dialog refuses a second one until the promo lines are removed). The booking dialog lists every valid promotion as a "[PROMO] <name>" entry of the category filter; picking it and pressing Add adds ALL its services as separate lines (quantity 1, regular price, promo_id set, the name shown as "[PROMO] <service name>"), and removing one of those lines removes the whole promotion. Old Percentage / Fixed promos still work. Promos need FULL payment upfront (no installments): when a promo is applied, the amount due now is the full total_amount, saved as a Payments row of type Final, and Billing.payment_status becomes 'Paid'.
- Cancel / reschedule: must be requested at least 3 days before the appointment date. Inside 3 days only an Admin may do it, and must type a reason. A cancellation always forfeits the money paid upfront: Billing.downpayment_status 'Forfeited' (or payment_status 'Forfeited' for a fully prepaid promo). A reschedule keeps the downpayment 'Held', the appointment stays 'Upcoming', and an AppointmentHistory row with change_type 'Rescheduled' stores the old date, old time and reason. A reschedule moves ALL the service lines of the appointment by the same amount: newStart is the new start of the EARLIEST line, the other lines keep their distance from it, and each line's start_time / end_time / busy_end_time is rewritten before the rooms are planned again line by line (the technicians stay as they are). A package session moves only that session by default (the 4-argument Reschedule); the later-sessions overload (Reschedule with moveLaterSessions true) keeps the weekly plan by moving every later session by the same date and time delta.
- Minimum age: a client must be at least 15 years old ON THE DAY OF SAVING, counting birthdays correctly (not days / 365). The birth date is REQUIRED: an unticked picker is the error "Birth date is required." and a client under 15 shows "For safety reasons, we do not accept clients under 15 years old." The rule is enforced in ClientEditForm AND in ClientRepository (both Add and Update), so no other code path can save a younger client. Validators.AgeOn / Validators.IsAtLeastAge hold the counting and MinimumClientAge = 15. Existing clients are never deleted.
- Cancelled and rescheduled appointments are not work: they appear ONLY on the Appointments page, and only when the status filter is "All" or that exact status. The default filter there is "Active" (everything except Cancelled and Rescheduled), and the Dashboard Today / Selected Day grids AND the Dashboard month calendar (AppointmentRepository.GetMonthAppointments) use the same working list too, so a day cell and its "Selected Day" list always show the same appointments. Billing and the daily capacity counts already skip cancelled ones.
- Billing: subtotal = sum of line prices (price at booking time). total_amount = subtotal - discount_amount. balance_due = total_amount - SUM(amount_paid of Payments whose verification_status <> 'Rejected'). payment_status: Unpaid (no payments), Partial, Paid (balance_due = 0 OR total_amount = 0).
- AppointmentServices.start_time / end_time / busy_end_time are the times of ONE service line. When they are NULL (old rows, demo rows) the code uses the appointment's own start and end.
- Payments: a payment can be taken at any time and on any date. The first payment is at least 50% of the total (promo: the whole total); later payments are any amount up to the balance. Completing the service pays the rest. A bill of 0.00 (a later session of a package) is 'Paid'.
- Packages: Services.price is the price of the WHOLE package when default_total_sessions > 1. The package price is charged on the first session line; later sessions of that package have price 0.00. When a client books a service with default_total_sessions > 1 and has no Active package for it, a ClientPackages row is created automatically. Completing a package session adds 1 to sessions_completed; at total_sessions the package becomes 'Completed'. A package is booked as weekly sessions: all sessions are created together, same time, session_interval_days apart (7). The first session carries the whole package price, the others 0.00. Labels read 'Session x of y' ('Session 1 of 1' for a single service). A cancelled session leaves a free number that can be booked again. The later sessions are created in the SAME transaction as the first one (Appointments, AppointmentServices, AppointmentStaff, AppointmentRooms, Billing and AppointmentHistory for each), so the remaining weeks are already scheduled; this weekly-session check runs ONLY for a NEW booking (appointmentId 0) and never during a reschedule, because the later sessions already exist as their own appointments; when one of those sessions cannot be placed the whole booking is refused and rolled back (ValidateBooking reports "<service> session <n> on <MMM dd, yyyy>: <problem>").
- A rescheduled session must stay at least session_interval_days (7 by default) after the previous session and before the next one of the same package. The rule reads the other non-cancelled sessions of the same package (AppointmentRepository.PackageSessionProblems, called by Reschedule) and refuses the move with one plain sentence per failing line, ending in "Move the other sessions first.". Cancelling or rescheduling a package session can include all the later sessions. Session 1 carries the payment, so it can only be cancelled together with them.
- Commission: created when a service line is completed at billing, for the attending staff of that line: 10% (commission_rate 10.00) of the line's net amount (line price minus its proportional share of the discount). For package lines it is 10% of Services.price divided by total_sessions. POLISH: confirm the package rule with the owner.

- Payment methods: Cash, GCash, Bank Transfer. GCash/Bank Transfer need a reference number and a "payment verified" tick before the payment is saved as 'Verified'. The system never calls any bank or GCash API.

## Additions (Step 3)
### Screen size (applies to every layout prompt)
The system must work on common 16:9 monitors (1366x768, 1600x900, 1920x1080 at 100 to 150 percent Windows scaling). The smallest supported size is 1280x720 logical pixels (about 1280x680 usable after the taskbar), which is also what a 1920x1080 monitor at 150 percent scaling gives. Every page must work inside a content area of about 1240 x 600 (a maximized MainForm minus the 240 px sidebar and the 64 px header). Dialogs must be at most 1000 x 640 logical pixels; if the content is taller, put it inside a scrollable panel. If a prompt gives bigger sizes, shrink them to fit these limits. Preview images are rendered off-screen, so the monitor the app opens on must not matter.

## Additions (Step 4) - final state
The system is COMPLETE (every module is built: Dashboard, Appointments, Clients, Services,
Promotions, Billing, Commissions, Reports, Staff Accounts, File Maintenance, Activity Log,
Settings). A task from now on is usually a small change, so these rules apply.

### The four checks before a task is called finished
1. Standard clean build: 0 errors, 0 warnings.
2. `bin\Debug\LittleEreseSystemV3.exe --smoketest` - every check PASS (it is read-only and never
   writes to the shop's data).
3. `powershell -ExecutionPolicy Bypass -File tools\DesignerAudit.ps1` - no problems (exit code 0).
   It checks every *.Designer.cs in Forms\ and Controls\: every `this.X = new` has a field
   declaration, every declared control field is instantiated, every wired handler has a method in
   the sibling .cs, there is exactly one InitializeComponent plus the components field and Dispose,
   and no Image / Db / Session / repository code appears in a Designer file.
4. Start the app once, confirm it stays open, close it, and check that bin\Debug\error.log is empty.

### Two tools that are NOT part of the .csproj
- `tools\DesignerAudit.ps1` (PowerShell, read-only) - see above.
- `docs\screenshots\` - the PNG of every screen, produced with `--preview <ClassName>` (plus
  `--role Staff` for ucDashboard, ucBilling and ucAppointments). Regenerate them after a layout
  change.

### Where the documentation lives now
- `README.md` - what the system is, requirements, setup, demo accounts, the two switches, the
  folder map and the limitations exactly as in the capstone proposal.
- `docs\DEVELOPER_GUIDE.md` - the architecture in plain words, the naming rules, how to add a field /
  a validation / a report / a sidebar page, Theme, the Designer yellow screen, the smoke test and
  the preview tool, and a troubleshooting table.
- `docs\POLISH_LIST.md` - every `// POLISH:` comment with its file and line. Keep it in step when a
  POLISH comment is added or removed.
- `Database\ResetDemoData.sql` - empties the demo tables in foreign-key order (ActivityLog ...
  Promotions) WITHOUT touching Users, Services, ServiceCategories, Rooms or RoomServiceCategories.
  It deletes real records too, so never run it without the owner's go-ahead.

### House rules learned in this step
- No `// TODO(logic prompt)` may remain in any file: either the handler is implemented, or the
  wiring (the Designer line AND the method) is removed together.
- Keep `// POLISH:` comments - they are the owner's adjustment points (see docs\POLISH_LIST.md).
- Every public method / property of a Data\ or Helpers\ class carries a one-line `/// <summary>`.
- Unused using directives, private fields and variables are removed (the IDE greys them out; CS8019
  is reported for unused usings).
- A Designer file stays exactly what the Visual Studio Designer would generate. When a layout must
  change, change the smallest Designer property (one column width, one splitter distance, one row
  height) and take a fresh preview.
- The smoke test and every `--preview` run must leave no file behind except bin\Debug\previews\ and
  smoketest.log, which .gitignore already covers.

## Additions (Step 31) - the sidebar
`Forms\MainForm.Designer.cs` builds the sidebar buttons (pnlNav) and `Forms\MainForm.cs` decides who
sees them. The Admin menu is grouped, the Staff menu has no group it cannot use:
- Dashboard has NO heading. Then STAFF MANAGEMENT (Staff Accounts, Commissions), OPERATIONS
  (Appointments, Billing, Clients, Services, Promotions, Reports) and FILES (File Maintenance,
  Activity Log). Settings stays in the header, not in the list.
- Staff sees Dashboard + OPERATIONS only. `ApplyRoleVisibility` hides the Admin buttons AND the two
  headings that hold only Admin buttons (STAFF MANAGEMENT, FILES), so a heading is never left alone.
- A heading is a Label (lblNavStaffManagement, lblNavOperations, lblNavFiles), not a Button: it is
  never clickable and never highlighted. SmallFont, Theme.TextMuted, 16 px left padding, a 26 px
  height and a 6 px top padding give it the gap above and separate it from the buttons.
- Sizes: a nav button is 36 px high (was 48) and a heading 26 px, so the whole Admin menu fits at
  1280 x 720 (Dashboard + 3 headings + 11 buttons = 432 px inside the 602 px pnlNav). The headings
  are hidden while the sidebar is collapsed, because the icon strip has no room for words.
- pnlNav is NOT AutoScroll: the fit rule above must keep being true. If a future page is added and
  the menu no longer fits, make pnlNav.AutoScroll true and leave the horizontal scrollbar off.
