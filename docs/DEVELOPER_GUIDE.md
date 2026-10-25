# Developer guide - Little Erese's Appointment and Billing System

For the junior developer who picks this project up. Read `PROJECT_RULES.md` first: it lists the
hard limits (C# 7.3, .NET Framework 4.8, no NuGet, no SQL inside forms). This guide explains how
the code is put together and how to add the things you will most likely be asked to add.

---

## 1. How the app is put together, in plain words

```
LoginForm -> MainForm (sidebar + pnlContent)
                |
                +-- Controls\uc*.cs        one page per module (Dashboard, Clients, ...)
                |       |
                |       +-- Forms\*EditForm.cs   dialogs (add / edit client, book appointment...)
                |       |
                |       +-- Data\*Repository.cs  the only place that writes SQL
                |               |
                |               +-- Data\Db.cs   opens connections, runs queries, transactions
                |
                +-- Helpers\  Theme, Validators, Messages, GridHelper, Session, DesignTime ...
```

Rules that keep this simple:

1. **Forms and user controls never contain SQL.** A page asks a repository for a `DataTable`, sets
   it as the `DataSource` of its grid, and the grid columns (`DataPropertyName`) pick the values up.
2. **A repository never shows a message box.** It returns data or throws; the page catches the
   exception and shows it with `Messages.DbError(ex)`.
3. **Every value is a parameter** (`Db.P("@client_id", id)`). Never paste text into SQL.
4. **`Db.Get<T>(row["col"])` reads values** so a NULL in the database never throws.
5. **All input rules live in `Helpers\Validators.cs`.** No form writes its own regex.
6. **`Helpers\Session` remembers who is signed in.** `Session.IsAdmin` decides what Staff may see.
7. **`Helpers\DesignTime.IsDesignMode` keeps the Visual Studio Designer happy** - every Load event
   that touches the database starts with `if (DesignTime.IsDesignMode) return;`.

### Naming rules

| Thing | Rule | Example |
|---|---|---|
| Form | PascalCase, ends with `Form` | `ClientEditForm` |
| Page (UserControl) | `uc` + module | `ucAppointments` |
| Repository | `Xxx` + `Repository` | `BillingRepository` |
| Buttons | `btn` + what it does | `btnSave`, `btnAddPackage` |
| Text boxes | `txt` | `txtSearch` |
| Labels | `lbl` | `lblBalance` |
| Grids | `dgv` | `dgvClients` |
| Combo boxes | `cbo` | `cboStatus` |
| Date pickers | `dtp` | `dtpFrom` |
| Panels / layout | `pnl` / `tlp` / `flp` | `pnlDetails`, `tlpBody` |
| Grid columns | `col` + grid + field | `colQueueBalance` |
| Private field | `_camelCase` | `_loading` |
| Constants | `PascalCase` | `MaxFailedAttempts` |

---

## 2. Steps for the usual jobs

### A. Add a new field to an existing form

The database column must already exist (`Database\LittleEreses_Database.sql` is the source of
truth - never invent columns). Then:

1. **Designer** - open `Xxx.Designer.cs` in the Designer, drop the label + text box, name them
   (`lblX` / `txtX`), and wire nothing. Keep the visual order the same as the TabIndex order.
   In this project the Designer files are edited by hand as text sometimes; if you do, use the
   pattern of the neighbouring controls and add the field declaration at the bottom.
2. **Repository** - add the column to the `SELECT` (and to `INSERT` / `UPDATE` with a parameter),
   so the grid or the dialog actually carries the value.
3. **Page / dialog** - read the control in `Load` (fill it) and in the save handler (validate it
   and pass it to the repository).
4. **Validation** - if the value has rules, put the rule in `Validators` (see B) and mark the
   control with the `ErrorProvider`.
5. Build, `--smoketest`, `--preview` the screen.

### B. Add a validation or a regex

Everything goes in `Helpers\Validators.cs`:

```csharp
// POLISH: 2-50 characters ... (say WHY the rule is what it is)
public const string NameRegex = @"^[A-Za-z .'\-]{2,50}$";

public static bool IsValidName(string value) { ... }
```

Use it in the form's save handler:

```csharp
if (!Validators.IsValidPhone(txtContact.Text))
{
    Validators.SetError(errorProvider1, txtContact, "Enter a contact number like 09171234567.");
    return;
}
Validators.ClearError(errorProvider1, txtContact);
```

Do not write `Regex.IsMatch(...)` inside a form - the next screen would use a slightly different
rule and the two would disagree.

### C. Add a new report

1. **Repository** - add a method to `Data\ReportRepository.cs` that returns a `DataTable` with clear
   column aliases:
   ```csharp
   /// <summary>Returns revenue per staff member between two dates (to_date is inclusive).</summary>
   public static DataTable GetRevenueByStaff(DateTime from, DateTime to)
   {
       return Db.Query(@"SELECT u.full_name AS staff_name, SUM(p.amount_paid) AS revenue
                         FROM Payments p ... AND p.payment_date < @to_date + 1
                         GROUP BY u.full_name ORDER BY revenue DESC",
           Db.P("@from_date", from.Date), Db.P("@to_date", to.Date));
   }
   ```
   Note `payment_date < @to_date + 1`: that is how the other reports make the whole "to" day count.
2. **Designer** - add a `DataGridView` (or a tab) to `ucReports.Designer.cs`, name it
   `dgvXxx`, set `AutoGenerateColumns = false` and add columns whose **`DataPropertyName` matches
   the aliases exactly**.
3. **Page** - in `ucReports.cs` load it in `Generate` (`dgvXxx.DataSource = ReportRepository.Get...`),
   inside the existing `try / catch (Messages.DbError)`.
4. **Smoke test** - add one read-only check in `Helpers\SmokeTest.cs`.

### D. Add a new page to the sidebar

1. Add `Controls\ucMyPage.cs` + `.Designer.cs` (copy an existing page as the start), and **add both
   files to `LittleEreseSystemV3.csproj`** (`<Compile Include=...>` with `DependentUpon` for the
   Designer file). The project is old-style; a file that is not listed is not compiled.
2. `MainForm.Designer.cs`: copy an existing sidebar button, name it `btnNavMyPage`, set the caption
   and the icon slot.
3. `MainForm.cs`:
   - add `private const string PageMyPage = "MyPage";`
   - add a `case` to `CreatePage` that returns `new ucMyPage()`
   - add the caption to `PageTitle`
   - add a `btnNavMyPage_Click` that calls `Navigate(PageMyPage)`
   - add the button to `NavButtonFor(pageKey)` and to the `NavButtons()` list (highlighting and
     collapsing the sidebar) and give it its icon in `LoadImages` (`SetNavIcon(...)`)
   - if it is Admin only, add it to `IsAdminPage` and to `ApplyRoleVisibility`
4. Build and open `--preview ucMyPage` to check the layout.

### E. Change colours and fonts

`Helpers\Theme.cs` is the only place:

```csharp
public static readonly Color Primary = Color.FromArgb(0xA4, 0x7B, 0x2B);
public static readonly Font HeadingFont = new Font("Segoe UI Semibold", 16F, ...);
```

Two rules:

- **Designer files cannot call Theme** (the Designer runs no code), so the Designer files hold the
  same colour as a literal: `System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ...)`. If you
  change a colour, change it in `Theme.cs` **and** in the Designer files (search the old ARGB).
- Runtime styling (in a `Load` event) always uses `Theme.StylePrimaryButton`, `StyleSecondaryButton`,
  `StyleDangerButton`, `StyleGrid` and `Theme.StatusColor(status)`.

### F. Avoid the yellow screen in the Visual Studio Designer

The Designer draws a form by running its constructor and `InitializeComponent`, so anything slow or
failing there becomes the yellow error page:

- `if (DesignTime.IsDesignMode) return;` at the top of every `Load` event (and any method it calls
  that touches the database, a file or `Session`).
- **No database, file or `Session` calls in a constructor.** A dialog gets its data in `Load` from
  the public properties the caller set (`EditId`, `ClientId`, `AmountDue`, ...).
- Nothing in a `.Designer.cs` except what the Designer itself would write. No `Image =` (images load
  at runtime through `IconHelper`), no `if`, no repository call.
- Every event wired in `InitializeComponent` must have a method with that exact name in the `.cs`.
  A missing handler is a build error, but a handler that throws is a yellow screen at runtime.
- Run `powershell -ExecutionPolicy Bypass -File tools\DesignerAudit.ps1` before you commit: it
  checks all five rules above for every Designer file.

### G. Use the smoke test and the preview tool

```text
bin\Debug\LittleEreseSystemV3.exe --smoketest
```
Builds every form and user control (catches a broken Designer file), runs the read-only database
checks and writes `smoketest.log`. The last line is `SUMMARY: n passed, n failed, n skipped`; the
exit code is 1 when something failed. **Everything must pass before a task is finished.**

```text
bin\Debug\LittleEreseSystemV3.exe --preview ucBilling --role Staff
```
Shows one screen off-screen, saves `bin\Debug\previews\ucBilling.png` and exits. Use it to look for
overlaps, cut-off text and controls that do not fit at 1240 x 600. Dialogs are previewed by their
own class name (`--preview ClientEditForm`).

Add up to three read-only checks to `Helpers\SmokeTest.cs` for every repository method you add, for
example:

```csharp
private static void CheckClientSearch()
{
    const string label = "ClientRepository.Search(\"\", \"All\")";
    try
    {
        DataTable clients = ClientRepository.Search("", "All");
        Pass(label + " rows = " + clients.Rows.Count);
    }
    catch (Exception ex)
    {
        Fail(label, ex);
    }
}
```

Then call it from `CheckRepositories()` (or the matching `Check...` group) so it actually runs.
Never make a smoke-test check write to the database - it runs on the shop's real data.

---

## 3. Troubleshooting

| Problem | What it means | What to do |
|---|---|---|
| The Designer shows the **yellow screen** ("The designer could not be shown...") | The constructor or `InitializeComponent` threw: usually a database call, a missing handler or a hand-edited Designer line. | Read the message under the yellow box (or run `tools\DesignerAudit.ps1`), move the work into `Load`, and guard it with `DesignTime.IsDesignMode`. Close and reopen the file. |
| **CS0103 - "The name 'xxx' does not exist in the current context"** right after the `.csproj` was edited | The `.csproj` lists a file twice, lists a file that no longer exists, or forgot to list a new one. The old-style project builds exactly what it lists. | Right-click the project -> **Reload Project**, then check the `<Compile Include=...>` lines. Clean + rebuild. |
| **"Cannot reach SQL Server LocalDB"** when the app starts | LocalDB is not running or the connection string is wrong. | Open `App.config`, check `LittleEreseDb` -> `(localdb)\MSSQLLocalDB;Initial Catalog=LittleEresesDB`. Run `sqllocaldb info` in a console; if the instance is missing, run `sqllocaldb create MSSQLLocalDB`. |
| **"The database 'LittleEresesDB' was not found"** (error 4060) | The schema script was never run on this machine. | Run `Database\LittleEreses_Database.sql`, then `Database\SeedData.sql` in SSMS against `(localdb)\MSSQLLocalDB`. |
| LocalDB is **not installed** at all (`sqllocaldb` is not recognized) | It is a separate component of the SQL Server / Visual Studio installer. | Run the Visual Studio Installer -> Modify -> Individual components -> "SQL Server Express LocalDB", or install "SQL Server Express LocalDB" from Microsoft. |
| A dialog opens with **empty combo boxes** | Data is loaded in `Load` and the guard `if (DesignTime.IsDesignMode) return;` was hit (running inside Visual Studio at design time), or the repository call failed silently. | Run the app normally (F5) and watch for the error message; if it shows, `Messages.DbError` will name the table or column. |
| A page **never appears** for Staff | It is an Admin-only page. | `MainForm.ApplyRoleVisibility` hides the button and `MainForm.Navigate` refuses the key. Both are on purpose; change them together. |
| A grid shows **empty cells** | The column's `DataPropertyName` does not match the query alias. | Compare the alias in the repository with the Designer; the smoke test checks the column names of the main queries. |
| The app closes **without a message** | The exception happened outside the UI thread, or the process was killed. | Look at `bin\Debug\error.log` - Program.cs writes the full exception there with a timestamp. |
| **Printing** does nothing | No default printer is installed on the machine. | Install one or use "Microsoft Print to PDF". `GridPrinter` / `ReceiptPrinter` never throw. |

---

## 4. Before you call a task finished

1. Close the running app.
2. Delete `bin` and `obj`, then
   `msbuild LittleEreseSystemV3.csproj /t:Rebuild /p:Configuration=Debug /nologo /v:minimal`
   -> **0 errors, 0 warnings**.
3. `bin\Debug\LittleEreseSystemV3.exe --smoketest` -> everything PASS.
4. `powershell -ExecutionPolicy Bypass -File tools\DesignerAudit.ps1` -> no problems.
5. Start the app once, click the page you changed, close it, check `error.log` is empty.
6. Update `PROGRESS.md` with what you did, what is next and any known issue.

