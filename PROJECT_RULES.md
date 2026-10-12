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
