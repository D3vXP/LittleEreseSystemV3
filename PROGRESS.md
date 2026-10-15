# PROGRESS.md

Update this file at the end of every task.

## Done
- Project scaffold: 9 Forms, 12 UserControls (WinForms), Data and Helpers classes created empty.
- Database script (Database/LittleEreses_Database.sql) and price list (Database/SeedData.sql) prepared.
- Fix pack applied: WinForms UserControls, clean .csproj, App.config connection string, DesignTime helper, Db helper.
- Step 1 (this task): Helpers filled in - Theme (colors, fonts, button/grid styles, status colors),
  IconHelper (Assets\Images loading, scaling, tinting, cached, plus MDL2 Glyph codes),
  Messages, Session, PasswordHasher (PBKDF2-SHA256), Validators (regex rules + ErrorProvider helpers).
- Program.cs now catches UI-thread and AppDomain exceptions, shows Messages.Error(Db.Describe(ex))
  and appends the full exception to error.log next to the exe.
- PROJECT_RULES.md got the "Additions (Step 1)" section.
- Clean rebuild: 0 errors, 0 warnings. App started (blank LoginForm) and was closed again.

## Next
- Step 2: build the LoginForm (design + wiring to UserRepository + Session) so a demo admin can sign in.
- Then MainForm shell (sidebar navigation) and the Dashboard with ucCalendarMonth.

## Known issues
- None yet. Helpers compile but are not yet used by any form, so no runtime behaviour to check.
