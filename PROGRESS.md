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
- Step 2: UserRepository (Authenticate / UsernameExists / Create / SetPassword),
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
- Step 3 (this task): PROJECT_RULES.md got the "Additions (Step 2)" section (standard clean build,
  layout and logic prompt rules, roles, database value lists, business rules).
- LoginForm layout finished (920x580, FixedSingle, CenterScreen, no maximize):
  tlpMain 50/50 split, pnlSide with picSide + lblTagline, pnlForm with a 340 px centered card
  (picLogo, Welcome Back, Sign in to continue, Username, Password + show-password eye button,
  error label, gold Login button, footer). AcceptButton = btnLogin.
- MainForm shell finished (1280x720, MinimumSize 1100x650, CenterScreen):
  pnlSidebar 240 px (pnlBrand + picBrand, pnlNav with the 11 nav buttons in the exact order
  Dashboard ... Activity Log, lblVersion), pnlHeader 64 px with btnMenuToggle, lblPageTitle,
  user area (picUser + name/role, btnSettings, btnLogout) and the 1 px pnlHeaderLine,
  pnlContent (AppBack, padding 16) that stays empty - pages are created at runtime.
  toolTip1 set on the menu, settings and log out buttons.
- NEW Helpers\PreviewTool.cs (registered in the .csproj): "LittleEreseSystemV3.exe --preview
  <ClassName> [--role Admin|Staff]" signs in a preview user, shows the form (or hosts a
  UserControl in a 1180x720 form), waits ~800 ms, writes previews\<ClassName>.png next to the
  exe and exits 0; on any error it writes previews\<ClassName>.error.txt and exits 1.
  Program.cs startup order is now --smoketest, --preview, normal flow.
- Clean rebuild after deleting bin, obj and .vs: 0 errors, 0 warnings.
  --smoketest: 29 passed, 0 failed, 0 skipped.
- Previews checked. Fixed in round 1: (1) LoginForm - the login card was cut off at the bottom,
  the row margins now add up to 540 px inside the 580 px form; (2) MainForm - the sidebar nav
  buttons were in reverse order (Dock Top stacks the last added control on top), so the
  Controls.Add order was flipped and the order now matches the design; (3) MainForm - the
  settings/log out buttons were aligned outside the right edge of the header, the user area
  now has a Dock.Right pnlUserTools container so the icons sit inside the header with a
  12 px margin.

## Next
- Step 4: LoginForm logic (UserRepository.Authenticate + Session.Start, error label instead of
  a message box, show/hide password) and MainForm logic (run-time icons for the sidebar and
  header buttons, page switching inside pnlContent, role based hiding of the admin buttons,
  settings and log out).
- Then the Dashboard with ucCalendarMonth.

## Known issues
- None. LoginForm and MainForm are still empty shells: layout only, every event handler body is
  "// TODO(logic prompt)", so signing in from the UI is not possible yet (only the smoke test
  can sign in). Demo staff accounts still have the placeholder names "Aesthetician 1".."Nurse 1"
  (the schema has no job-title column) - rename them in Staff Accounts when it is built.
- picUser, picBrand, picSide and picLogo are intentionally empty in the previews: images are
  loaded at runtime in the Load events, which the next (logic) prompt adds.
