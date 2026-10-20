# PROJECT_RULES.md - Little Erese's Appointment and Billing System

Read this file, PROGRESS.md and Database/LittleEreses_Database.sql at the start of EVERY chat. Follow every rule below.

## Project facts
- C# Windows Forms, .NET Framework 4.8, Visual Studio 2026, old-style .csproj (every file must be listed in it).
- Project / root namespace: LittleEreseSystemV3. Forms: LittleEreseSystemV3.Forms. Controls: LittleEreseSystemV3.Controls. Data: LittleEreseSystemV3.Data. Helpers: LittleEreseSystemV3.Helpers.
- Database: SQL Server LocalDB, server (localdb)\MSSQLLocalDB, database LittleEresesDB, Windows Authentication. The connection string "LittleEreseDb" lives in App.config.
- Offline desktop app, one computer, two roles: Admin and Staff.
- Database/LittleEreses_Database.sql is the SOURCE OF TRUTH for tables and columns. Never invent, rename or drop columns. If you think the schema must change, STOP and tell the user first.

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
- Never run DROP DATABASE, DROP TABLE or any destructive SQL without asking the user. Run the schema script only if LittleEresesDB does not exist.
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
Users.role: Admin | Staff. Users.status: Active | Inactive. Services.status: Active | Archived. Promotions.discount_type: Percentage | Fixed. Promotions.status: Active | Inactive | Expired. Rooms.status: Available | Unavailable. ClientPackages.status: Active | Completed | Expired. Appointments.booking_source: Walk-in | Messenger | Text | Phone. Appointments.status: Upcoming | Ongoing | Completed | Cancelled | Rescheduled. AppointmentServices.status: Pending | Completed | Cancelled. AppointmentStaff.status: Assigned | Completed | Reassigned. AppointmentRooms.status: Reserved | In Use | Released. AppointmentHistory.change_type: Created | Rescheduled | Cancelled | Status Change. Billing.downpayment_status: None | Held | Applied | Forfeited. Billing.payment_status: Unpaid | Partial | Paid | Forfeited. Payments.payment_type: Downpayment | Final. Payments.payment_method: Cash | GCash | Bank Transfer. Payments.verification_status: Pending | Verified | Rejected. Commissions.payout_status: Unpaid | Paid.
### Business rules
- Business: Little Erese's Aesthetics and Spa, San Nicolas Poblacion, Concepcion, Tarlac. Money format: PHP peso sign with #,##0.00. Dates: MMM dd, yyyy. Times: h:mm tt.
- Opening hours: aesthetic services 10:00 AM to 8:00 PM; massage services (category name starts with "Massage") 1:00 PM to 10:00 PM. Appointment end time = start + sum of duration_minutes of its services; it must finish inside the opening hours of every service in it. Start times are on 30-minute marks.
- Capacity per day: at most 30 non-cancelled appointments and at most 10 that contain a massage service.
- Rooms: each appointment service needs a room linked to its category in RoomServiceCategories; a room cannot have more overlapping assigned services than its capacity. A technician cannot have two overlapping services. Technicians are Users with role Staff and status Active.
- Booking: a non-walk-in booking (Messenger, Text, Phone) needs a 50% downpayment of the subtotal, saved as a Payments row of type Downpayment, Billing.downpayment_status 'Held'. Walk-ins need no downpayment ('None').
- Promos: only Status Active and today between start_date and end_date. Percentage = subtotal x value / 100; Fixed = value; the discount never exceeds the subtotal. Promos need FULL payment upfront (no installments): when a promo is applied, the amount due now is the full total_amount, saved as a Payments row of type Final, and Billing.payment_status becomes 'Paid'.
- Cancel / reschedule: must be requested at least 3 days before the appointment date. Inside 3 days only an Admin may do it, and must type a reason. A cancellation always forfeits the money paid upfront: Billing.downpayment_status 'Forfeited' (or payment_status 'Forfeited' for a fully prepaid promo). A reschedule keeps the downpayment 'Held', the appointment stays 'Upcoming', and an AppointmentHistory row with change_type 'Rescheduled' stores the old date, old time and reason.
- Billing: subtotal = sum of line prices (price at booking time). total_amount = subtotal - discount_amount. balance_due = total_amount - SUM(amount_paid of Payments whose verification_status <> 'Rejected'). payment_status: Unpaid (no payments), Partial, Paid (balance_due = 0).
- Packages: Services.price is the price of the WHOLE package when default_total_sessions > 1. The package price is charged on the first session line; later sessions of that package have price 0.00. When a client books a service with default_total_sessions > 1 and has no Active package for it, a ClientPackages row is created automatically. Completing a package session adds 1 to sessions_completed; at total_sessions the package becomes 'Completed'.
- Commission: created when a service line is completed at billing, for the attending staff of that line: 10% (commission_rate 10.00) of the line's net amount (line price minus its proportional share of the discount). For package lines it is 10% of Services.price divided by total_sessions. POLISH: confirm the package rule with the owner.
- Payment methods: Cash, GCash, Bank Transfer. GCash/Bank Transfer need a reference number and a "payment verified" tick before the payment is saved as 'Verified'. The system never calls any bank or GCash API.

## Additions (Step 3)
### Screen size (applies to every layout prompt)
The system must work on common 16:9 monitors (1366x768, 1600x900, 1920x1080 at 100 to 150 percent Windows scaling). The smallest supported size is 1280x720 logical pixels (about 1280x680 usable after the taskbar), which is also what a 1920x1080 monitor at 150 percent scaling gives. Every page must work inside a content area of about 1240 x 600 (a maximized MainForm minus the 240 px sidebar and the 64 px header). Dialogs must be at most 1000 x 640 logical pixels; if the content is taller, put it inside a scrollable panel. If a prompt gives bigger sizes, shrink them to fit these limits. Preview images are rendered off-screen, so the monitor the app opens on must not matter.
