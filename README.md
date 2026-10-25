# Little Erese's Appointment and Billing System

Capstone project - STI College Tarlac.

An offline Windows desktop app for **Little Erese's Aesthetics and Spa** (San Nicolas Poblacion,
Concepcion, Tarlac). It keeps the client list, the appointment book, the packages, the promotions,
the bills and the staff commissions of a small spa in one place, on one computer, without an
internet connection.

Two roles share the same login:

| Role | Can do |
|---|---|
| **Admin** (owner) | Everything: dashboard, appointments, clients, services, promotions, billing, commissions, reports, staff accounts, file maintenance (backup / restore), activity log, own password. |
| **Staff** | Dashboard (no sales amounts), appointments, clients (view / add / edit), services and promotions (view only), billing, own password. No commissions, reports, staff accounts, file maintenance or activity log. |

## Requirements

- Windows 10 or 11 (tested on 64-bit).
- **.NET Framework 4.8** (already installed on up-to-date Windows).
- **SQL Server LocalDB** - comes with Visual Studio, or install "SQL Server Express LocalDB".
- **Visual Studio** with the ".NET desktop development" workload (C# 7.3, old-style .csproj).
- A screen of at least **1280 x 720** logical pixels; the layout is designed for 1366 x 768 and up.

## Setup

1. Install the requirements above.
2. Create the database: open **SSMS** (or sqlcmd) against the server (localdb)\MSSQLLocalDB and run,
   in this order:
   - Database\LittleEreses_Database.sql (creates the database LittleEresesDB and its 18 tables)
   - Database\SeedData.sql (service catalogue, categories, rooms and the demo accounts)

   An existing database created before the promotion-to-services feature only needs
   Database\Migration_01_PromotionServices.sql once.
3. Open LittleEreseSystemV3.slnx in Visual Studio, build (Ctrl+Shift+B) and run (F5).
4. Sign in. On the first start of a database with **no clients** the app loads realistic demo data
   (12 clients, bookings for the past and the next two weeks, bills, commissions) so every screen
   has something to show. It never deletes anything.

The connection string is LittleEreseDb in App.config. Change it there if your LocalDB instance has
another name.

## Demo accounts

| Username | Password | Role |
|---|---|---|
| admin | Admin@123 | Admin |
| aesthetician1 ... aesthetician4 | Staff@123 | Staff |
| therapist1, therapist2 (massage) | Staff@123 | Staff |
| nurse1 | Staff@123 | Staff |

Change them in **Staff Accounts** before the system is used for real, and set
EnableDemoSeed = false in Data\DemoDataSeeder.cs so the demo data is never loaded again.

## Command-line switches

    LittleEreseSystemV3.exe                       normal start (login screen)
    LittleEreseSystemV3.exe --smoketest           builds every form and control, checks the database,
                                                  writes smoketest.log, exits with 0 (all passed) or 1
    LittleEreseSystemV3.exe --preview <ClassName> [--role Admin|Staff]
                                                  draws one screen into previews\<ClassName>.png and
                                                  exits (checks a layout without clicking through it)

Example: bin\Debug\LittleEreseSystemV3.exe --preview ucBilling --role Staff

## Folder map

    LittleEreseSystemV3.slnx        solution
    LittleEreseSystemV3.csproj      old-style project: every .cs file is listed here
    App.config                      connection string LittleEreseDb
    Program.cs                      entry point, the two command-line switches, the error log

    Forms\                          the windows: LoginForm, MainForm (sidebar shell),
                                    AppointmentForm, ClientEditForm, PackageForm, PaymentForm,
                                    PromotionEditForm, ServiceEditForm, StaffEditForm
    Controls\                       the pages docked inside MainForm: ucDashboard, ucAppointments,
                                    ucClients, ucServices, ucPromotions, ucBilling, ucCommissions,
                                    ucReports, ucStaffAccounts, ucFileMaintenance, ucActivityLog,
                                    ucSettings, plus ucCalendarMonth
    Data\                           every SQL statement: Db (connections) and the repositories
                                    (Client, Appointment, Billing, Service, Promotion, User,
                                    Commission, Report, Activity, Maintenance) plus DemoDataSeeder
    Helpers\                        Theme (colours / fonts), Validators (all input rules), Messages,
                                    GridHelper, GridPrinter, CsvExporter, ReceiptPrinter, IconHelper,
                                    Session, DesignTime, PasswordHasher, SmokeTest, PreviewTool
    Assets\                         the PNG icons and the login image (loaded at runtime)
    Database\                       LittleEreses_Database.sql (schema, SOURCE OF TRUTH), SeedData.sql,
                                    Migration_01_PromotionServices.sql, ResetDemoData.sql
    Properties\                     AssemblyInfo, Resources, Settings (generated - do not hand-edit)
    docs\                           POLISH_LIST.md, DEVELOPER_GUIDE.md, screenshots\
    tools\                          DesignerAudit.ps1 (checks every *.Designer.cs, not in the .csproj)

## Tests and checks

- --smoketest - builds every form and user control (a broken Designer file is caught early) and runs
  read-only checks on the database. The last line is
  SUMMARY: <n> passed, <n> failed, <n> skipped.
- powershell -ExecutionPolicy Bypass -File tools\DesignerAudit.ps1 - checks every *.Designer.cs for
  the mistakes that give the Visual Studio Designer a yellow screen. Exits with code 1 when it finds
  a problem.
- docs\screenshots\ - the current picture of every screen, rendered by --preview.

## Limitations (as stated in the capstone proposal)

This is a single-premise, offline record system. It deliberately does **not** do:

- **No inventory management** - products and retail stock are not tracked.
- **No automatic SMS or email** - reminders and confirmations are sent by the staff (text, Messenger
  or phone call) outside the system.
- **No payroll** - commissions are computed and marked as paid, but salaries, deductions and
  payslips are not part of it.
- **No POS hardware** - no cash drawer, barcode scanner, card terminal or receipt printer setup;
  receipts print on a normal Windows printer (or PDF).
- **No digital consent forms** - the consent / waiver stays a paper form; the system only records
  whether it has been signed.
- **No online payment verification** - GCash and bank transfers are typed in and ticked by the staff
  by hand; the system never contacts GCash or a bank.
- **Single-premise, offline use** - one computer and one LocalDB instance, no cloud sync, no
  multi-branch, no web or mobile access.

## Documentation

- PROJECT_RULES.md - the coding and design rules (read this before changing anything).
- PROGRESS.md - what has been built, step by step, with the known issues of each step.
- docs\DEVELOPER_GUIDE.md - how the code is organised and how to add things safely.
- docs\POLISH_LIST.md - every // POLISH: comment (the spots the owner may want to adjust).
