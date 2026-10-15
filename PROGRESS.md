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
- PROJECT_RULES.md got the "Additions (Step 1)" section.
- Step 2 (this task): UserRepository (Authenticate / UsernameExists / Create / SetPassword),
  ActivityRepository.Log (never throws), DemoDataSeeder (gives the placeholder admin a real
  password, creates 7 demo staff accounts, safe to run many times).
- Program.cs: startup order is --smoketest, Db.CanConnect, DemoDataSeeder, LoginForm.
- NEW Helpers\SmokeTest.cs (registered in the .csproj next to the other Helpers entries):
  "LittleEreseSystemV3.exe --smoketest" constructs all 22 UI classes (9 forms + 13 user controls),
  checks 17 tables, 77 services, at least 8 users and the three demo logins, writes smoketest.log
  and exits with 0 or 1. It never shows a window.
- Database proven reachable: LittleEresesDB on (localdb)\MSSQLLocalDB, 17 tables, 77 services.
- Clean rebuild: 0 errors, 0 warnings. Smoke test: 29 passed, 0 failed, 0 skipped.
- App started normally (blank LoginForm), stayed open, closed again. No error.log.

## Next
- Step 3: build the LoginForm (design + wiring to UserRepository + Session) so a demo admin can sign in.
- Then MainForm shell (sidebar navigation) and the Dashboard with ucCalendarMonth.

## Known issues
- None. LoginForm is still the empty scaffold; login cannot be done from the UI yet, only from
  the smoke test. Demo staff accounts exist with the placeholder names "Aesthetician 1".."Nurse 1"
  (the schema has no job-title column) - rename them in Staff Accounts when it is built.
