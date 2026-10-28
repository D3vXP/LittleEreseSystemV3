using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using LittleEreseSystemV3.Controls;
using LittleEreseSystemV3.Data;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// A built-in health check, started with: LittleEreseSystemV3.exe --smoketest
    /// It builds every form and user control once and checks the database, so a broken
    /// designer file or a missing table is found before the owner clicks anything.
    /// It writes smoketest.log next to the exe and exits with code 0 (all good) or 1.
    /// It never shows a window on screen.
    /// </summary>
    internal static class SmokeTest
    {
        // The baseline script makes 18 tables, but a later migration may add more (SchemaVersion
        // already is one), so this is a MINIMUM and never an exact number.
        private const int MinimumTables = 18;
        // The seed data has 77 services, but the owner adds more through the app, so only the
        // seeded minimum is checked (like the Users count below).
        private const int ExpectedMinServices = 77;
        private const int ExpectedMinUsers = 8;
        // The seeder creates 1 admin + 7 staff, so at least this many Active technicians must exist.
        private const int ExpectedMinActiveStaff = 7;

        private static readonly StringBuilder _log = new StringBuilder();
        private static int _passed;
        private static int _failed;
        private static int _skipped;

        /// <summary>
        /// Runs every check, prints the report and ends the process (0 = all passed, 1 = a failure).
        /// </summary>
        public static void Run()
        {
            Line("Little Erese's smoke test - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            // Run the seeder first so this check also works on a brand-new database.
            // Only the demo ACCOUNTS are created here; the demo business data is NOT written by the
            // smoke test (it would change the shop's own records), so these checks stay read-only.
            try
            {
                DemoDataSeeder.EnsureDemoUsers();
                Line("Seed: OK");
            }
            catch (Exception ex)
            {
                Line("Seed: FAILED - " + ex.Message);
            }

            Line("");
            BuildEveryFormAndControl();

            Line("");
            CheckDatabase();

            Line("");
            CheckLogin();

            Line("");
            CheckRepositories();

            Line("");
            Line("SUMMARY: " + _passed + " passed, " + _failed + " failed, " + _skipped + " skipped");

            WriteLog();

            Environment.Exit(_failed == 0 ? 0 : 1);
        }

        /// <summary>
        /// Constructs every Form in LittleEreseSystemV3.Forms and every UserControl in
        /// LittleEreseSystemV3.Controls. Only constructing is tested (no Show), so nothing
        /// appears on screen. Controls without a public parameterless constructor are skipped.
        /// </summary>
        private static void BuildEveryFormAndControl()
        {
            List<Type> types = new List<Type>();

            foreach (Type type in typeof(SmokeTest).Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface) continue;

                bool isForm = typeof(Form).IsAssignableFrom(type)
                    && type.Namespace == "LittleEreseSystemV3.Forms";
                bool isControl = typeof(UserControl).IsAssignableFrom(type)
                    && type.Namespace == "LittleEreseSystemV3.Controls";

                if (!isForm && !isControl) continue;

                if (type.GetConstructor(Type.EmptyTypes) == null)
                {
                    Skipped(type.Name);
                    continue;
                }

                types.Add(type);
            }

            // Alphabetical order keeps the log easy to compare between runs.
            types.Sort(delegate (Type a, Type b) { return string.CompareOrdinal(a.FullName, b.FullName); });

            foreach (Type type in types)
            {
                object instance = null;
                try
                {
                    instance = Activator.CreateInstance(type);
                    Pass(type.Name);
                }
                catch (Exception ex)
                {
                    Exception real = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                    Fail(type.Name, real);
                }
                finally
                {
                    IDisposable disposable = instance as IDisposable;
                    if (disposable != null)
                    {
                        try { disposable.Dispose(); }
                        catch { /* a failed Dispose must not stop the smoke test */ }
                    }
                }
            }
        }

        /// <summary>Connection, expected table count and the seeded row counts.</summary>
        private static void CheckDatabase()
        {
            string error;
            if (Db.CanConnect(out error)) Pass("Db.CanConnect");
            else Fail("Db.CanConnect - " + error);

            CheckScalarAtLeast("Base tables = ", MinimumTables,
                "SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped = 0");

            // Services grows while the owner uses the app, so only the minimum is checked.
            try
            {
                int services = Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Services"));
                if (services >= ExpectedMinServices) Pass("Services rows = " + services + " (expected at least " + ExpectedMinServices + ")");
                else Fail("Services rows = " + services + " (expected at least " + ExpectedMinServices + ")");
            }
            catch (Exception ex)
            {
                Fail("Services rows", ex);
            }

            // Users grows while the owner uses the app, so only the minimum is checked.
            try
            {
                int users = Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Users"));
                if (users >= ExpectedMinUsers) Pass("Users rows = " + users + " (expected at least " + ExpectedMinUsers + ")");
                else Fail("Users rows = " + users + " (expected at least " + ExpectedMinUsers + ")");
            }
            catch (Exception ex)
            {
                Fail("Users rows", ex);
            }
        }

        /// <summary>The three logins the demo seeder is supposed to make possible.</summary>
        private static void CheckLogin()
        {
            CheckAuthenticate("admin / Admin@123 works", "admin", "Admin@123", true);
            CheckAuthenticate("admin / wrong-password is refused", "admin", "wrong-password", false);
            CheckAuthenticate("nurse1 / Staff@123 works", "nurse1", "Staff@123", true);
        }

        private static void CheckAuthenticate(string label, string username, string password, bool expectRow)
        {
            try
            {
                DataRow row = UserRepository.Authenticate(username, password);
                bool gotRow = row != null;

                if (gotRow == expectRow) Pass(label);
                else Fail(label + " (got " + (gotRow ? "a row" : "null") + ")");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// Read-only checks for the Service / Promotion repositories added with the Services and
        /// Promotions logic. Nothing is written or changed here.
        /// </summary>
        private static void CheckRepositories()
        {
            try
            {
                DataTable categories = ServiceRepository.GetCategories();
                string message = "ServiceRepository.GetCategories rows = " +
                    categories.Rows.Count + " (expected 10)";
                if (categories.Rows.Count == 10) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("ServiceRepository.GetCategories", ex);
            }

            try
            {
                DataTable services = ServiceRepository.GetServices("", 0, "Active");
                string message = "ServiceRepository.GetServices(\"\", 0, \"Active\") rows = " +
                    services.Rows.Count + " (expected at least " + ExpectedMinServices + ")";
                if (services.Rows.Count >= ExpectedMinServices) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("ServiceRepository.GetServices", ex);
            }

            try
            {
                DataTable promos = PromotionRepository.GetPromotions("", "All");
                Pass("PromotionRepository.GetPromotions(\"\", \"All\") rows = " + promos.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail("PromotionRepository.GetPromotions", ex);
            }

            // The three read-only Staff Accounts checks (Step 9). Nothing is written or changed here.
            try
            {
                DataTable users = UserRepository.GetUsers("", "All");
                string message = "UserRepository.GetUsers(\"\", \"All\") rows = " +
                    users.Rows.Count + " (expected at least " + ExpectedMinUsers + ")";
                if (users.Rows.Count >= ExpectedMinUsers) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("UserRepository.GetUsers", ex);
            }

            try
            {
                int admins = UserRepository.CountActiveAdmins();
                string message = "UserRepository.CountActiveAdmins() = " + admins + " (expected at least 1)";
                if (admins >= 1) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("UserRepository.CountActiveAdmins", ex);
            }

            try
            {
                DataTable staff = UserRepository.GetActiveStaff();
                string message = "UserRepository.GetActiveStaff() rows = " +
                    staff.Rows.Count + " (expected at least " + ExpectedMinActiveStaff + ")";
                if (staff.Rows.Count >= ExpectedMinActiveStaff) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("UserRepository.GetActiveStaff", ex);
            }

            // The three read-only promotion-to-services checks. Nothing is written or changed here.
            CheckComputeDiscount("Percentage", 15m, 1000m, 150.00m);
            CheckComputeDiscount("Fixed", 500m, 300m, 300.00m);
            CheckNoServiceLinks();

            // The three read-only Clients checks (Step 14). Nothing is written or changed here.
            CheckClientSearch();
            CheckClientCombo();
            CheckEmptyPackageList();

            // The three read-only Appointment checks (Step 16). Nothing is written or changed here.
            CheckAppointmentDayList();
            CheckTimeSlots();
            CheckBookingWithoutClient();

            // The three read-only Billing checks (Step 18). Nothing is written or changed here.
            CheckBillingQueue();
            CheckBillingTransactions();
            CheckBillingHeaderWithoutBill();

            // The three read-only Commission / Report checks (Step 20). Nothing is written here.
            CheckCommissionSummary();
            CheckReportSalesByCategory();
            CheckReportAppointmentStatusCounts();

            // The three read-only Dashboard checks (Step 22). Nothing is written here.
            CheckMonthAppointments();
            CheckDashboardStats();
            CheckCalendarCells();

            // The three read-only Activity Log / File Maintenance checks (Step 26).
            // Nothing is written, and a backup or a restore is NEVER run here.
            CheckActivityLog();
            CheckMaintenanceTableCounts();
            CheckMaintenanceDatabaseInfo();

            // The six read-only demo-data consistency checks (Step 27). Nothing is written here.
            CheckDemoData();

            // The two read-only migration checks. The smoke test never applies a migration itself
            // (it must not change the shop's database), so start the app once first.
            CheckSchemaVersionTable();
            CheckMigrationsListed();
            CheckMigration002Columns();
            CheckMigration003Data();
            CheckMigration004Data();

            // The read-only availability checks (Step 48). Nothing is written here.
            CheckAvailability();

            // A 'Promo Price' promotion without its price could never give a discount, so the
            // stored data is checked here (read-only).
            CheckScalarZero("Promotions of type 'Promo Price' with a NULL promo_price",
                "SELECT COUNT(*) FROM Promotions WHERE discount_type = 'Promo Price' AND promo_price IS NULL");

            // The two read-only minimum-age checks. Nothing is written: the rejected call stops
            // inside ClientRepository BEFORE its INSERT, so no row is created and none has to
            // be cleaned up afterwards (that is why the smoke test stays read-only here too).
            CheckClientMinimumAge();

            // The three read-only job-type checks. Nothing is written or changed here.
            CheckStaffForCategory();

            // The read-only service-scheduling check. Nothing is written or changed here.
            CheckServiceScheduleColumns();
        }

        /// <summary>
        /// ServiceRepository.GetById must hand back the two columns the edit form saves
        /// (staff_busy_minutes, session_interval_days). Read-only: only a SELECT is run.
        /// </summary>
        private static void CheckServiceScheduleColumns()
        {
            try
            {
                DataRow row = ServiceRepository.GetById(1);
                string label = "ServiceRepository.GetById returns busy minutes and interval";

                if (row == null)
                {
                    Fail(label + " (no service with id 1)");
                    return;
                }

                if (row.Table.Columns.Contains("staff_busy_minutes")
                    && row.Table.Columns.Contains("session_interval_days"))
                {
                    Pass(label);
                }
                else
                {
                    Fail(label + " (a column is missing)");
                }
            }
            catch (Exception ex)
            {
                Fail("ServiceRepository.GetById schedule columns", ex);
            }
        }

        /// <summary>
        /// Read-only consistency checks on the demo data, so a bad seeder run is found at once:
        /// no negative balance, one commission per finished service, package sessions in range,
        /// nothing finishing after 10:00 PM, total = subtotal - discount, and enough finished visits.
        /// </summary>
        private static void CheckDemoData()
        {
            CheckScalarZero("Demo data: no Billing row has balance_due < 0",
                "SELECT COUNT(*) FROM Billing WHERE balance_due < 0");

            CheckScalarZero("Demo data: every Completed appointment has at least one Commissions row",
                "SELECT COUNT(*) FROM Appointments a " +
                "WHERE a.status = 'Completed' " +
                "  AND NOT EXISTS (SELECT 1 FROM AppointmentServices asv " +
                "                   INNER JOIN Commissions cm ON cm.appointment_service_id = asv.appointment_service_id " +
                "                  WHERE asv.appointment_id = a.appointment_id)");

            CheckScalarZero("Demo data: every ClientPackages row has 0 <= sessions_completed <= total_sessions",
                "SELECT COUNT(*) FROM ClientPackages " +
                "WHERE sessions_completed < 0 OR sessions_completed > total_sessions");

            CheckScalarZero("Demo data: no non-cancelled appointment ends after 10:00 PM",
                "SELECT COUNT(*) FROM Appointments " +
                "WHERE status <> 'Cancelled' AND end_time > CAST('22:00:00' AS time)");

            CheckScalarZero("Demo data: every Billing.total_amount = subtotal - discount_amount",
                "SELECT COUNT(*) FROM Billing " +
                "WHERE total_amount <> subtotal - discount_amount");

            CheckScalarAtLeast("Demo data: Completed appointments = ", 25,
                "SELECT COUNT(*) FROM Appointments WHERE status = 'Completed'");
        }

        /// <summary>PASS when the query returns exactly 0 (the rule holds everywhere).</summary>
        private static void CheckScalarZero(string label, string sql)
        {
            try
            {
                int count = Convert.ToInt32(Db.Scalar(sql));
                string message = label + " (got " + count + " bad rows)";

                if (count == 0) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>PASS when the query returns at least <paramref name="minimum"/>.</summary>
        private static void CheckScalarAtLeast(string label, int minimum, string sql)
        {
            try
            {
                int count = Convert.ToInt32(Db.Scalar(sql));
                string message = label + count + " (expected at least " + minimum + ")";

                if (count >= minimum) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// The job type of a staff account decides which technician list a service category sees
        /// (PROJECT_RULES.md, "Technicians"). Read-only: nothing is written.
        /// </summary>
        private static void CheckStaffForCategory()
        {
            CheckStaffFor("GetActiveStaffFor(\"Botox\")", "Botox");
            CheckStaffFor("GetActiveStaffFor(\"Massage - Feet\")", "Massage - Feet");

            try
            {
                DataTable nurses = UserRepository.GetActiveNurses();
                string message = "UserRepository.GetActiveNurses() rows = " +
                    nurses.Rows.Count + " (expected at least 1)";
                if (nurses.Rows.Count >= 1) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("UserRepository.GetActiveNurses", ex);
            }
        }

        private static void CheckStaffFor(string label, string categoryName)
        {
            try
            {
                DataTable staff = UserRepository.GetActiveStaffFor(categoryName);
                string message = "UserRepository." + label + " rows = " +
                    staff.Rows.Count + " (expected at least 1)";
                if (staff.Rows.Count >= 1) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail("UserRepository.GetActiveStaffFor", ex);
            }
        }

        /// <summary>
        /// The minimum age of 15 (PROJECT_RULES.md) must be enforced by ClientRepository itself, so no
        /// screen can bypass it. Both calls are refused BEFORE the INSERT, so nothing is written.
        /// </summary>
        private static void CheckClientMinimumAge()
        {
            DateTime today = DateTime.Today;

            // 14 years old TODAY (the birthday is today, so the counting itself is exercised).
            CheckClientAgeRefused("Client age: 14 years old today is rejected",
                today.AddYears(-14));

            // The same birthday one day later makes the person exactly 15, so it must be accepted.
            CheckClientAgeAccepted("Client age: exactly 15 today is accepted",
                today.AddYears(-15));
        }

        /// <summary>
        /// PASS when ClientRepository.Add refuses this birth date with the minimum-age sentence.
        /// WHY Add and not only the private guard: the save method itself must refuse, and it stops
        /// BEFORE the INSERT, so the smoke test stays read-only and no row needs cleaning up.
        /// </summary>
        private static void CheckClientAgeRefused(string label, DateTime birthDate)
        {
            try
            {
                ClientRepository.Add("Test", "Smoke Age", birthDate, "Female", "09170000000",
                    "", "", "", "", false);
                Fail(label + " (the save was allowed)");
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == Validators.MinimumClientAgeMessage) Pass(label + " - " + ex.Message);
                else Fail(label + " (wrong message: " + ex.Message + ")");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// PASS when ClientRepository allows this birth date. The guard is called directly here so the
        /// check really writes nothing; Add runs the very same guard before its INSERT.
        /// </summary>
        private static void CheckClientAgeAccepted(string label, DateTime birthDate)
        {
            try
            {
                ClientRepository.EnsureClientIsOldEnough(birthDate);
                Pass(label);
            }
            catch (Exception ex)
            {
                Fail(label + " (" + ex.Message + ")");
            }
        }

        /// <summary>
        /// The migration runner keeps its list in SchemaVersion, so that table must exist.
        /// </summary>
        private static void CheckSchemaVersionTable()
        {
            const string label = "SchemaVersion exists";
            try
            {
                object found = Db.Scalar("SELECT OBJECT_ID('dbo.SchemaVersion', 'U')");

                if (found != null && found != DBNull.Value) Pass(label);
                else Fail(label + " (the table is missing - start the app once so ApplyMigrations runs)");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// Every file in Database\Migrations must be listed in SchemaVersion, otherwise the shop
        /// database is missing an update the code expects. "At least" style: a listed file that is
        /// no longer on disk is not a problem.
        /// </summary>
        private static void CheckMigrationsListed()
        {
            const string label = "Every migration file is listed in SchemaVersion";
            try
            {
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "Migrations");

                List<string> missing = new List<string>();
                int files = 0;

                if (Directory.Exists(folder))
                {
                    string[] paths = Directory.GetFiles(folder, "*.sql");
                    Array.Sort(paths, StringComparer.OrdinalIgnoreCase);

                    foreach (string path in paths)
                    {
                        files++;
                        string fileName = Path.GetFileName(path);

                        object listed = Db.Scalar(
                            "SELECT COUNT(*) FROM dbo.SchemaVersion WHERE script_name = @script_name",
                            Db.P("@script_name", fileName));

                        if (Convert.ToInt32(listed) == 0) missing.Add(fileName);
                    }
                }

                string message = label + " (files on disk = " + files + ", missing = " + missing.Count + ")";

                if (missing.Count == 0) Pass(message);
                else Fail(message + " - not applied: " + string.Join(", ", missing.ToArray()) +
                    " (start the app once so ApplyMigrations runs)");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// The nine columns of migration 002 must exist, otherwise the build expects a schema the
        /// shop database does not have. Read-only; "at least" style because a later file may add more.
        /// </summary>
        private static void CheckMigration002Columns()
        {
            CheckScalarAtLeast("Migration 002 columns exist = ", 9,
                "SELECT COUNT(*) FROM (VALUES " +
                "('Users','staff_type'),('Services','staff_busy_minutes')," +
                "('Services','session_interval_days'),('AppointmentServices','start_time')," +
                "('AppointmentServices','end_time'),('AppointmentServices','busy_end_time')," +
                "('AppointmentServices','assessed_by'),('Promotions','promo_price')," +
                "('Clients','health_conditions')) v(t, c) " +
                "WHERE COL_LENGTH('dbo.' + v.t, v.c) IS NOT NULL");
        }

        /// <summary>
        /// The data of migration 003: every duration on the 30-minute grid, the three Tirzepatide
        /// packages and the room/category links. Read-only; "at least" style for the added rows.
        /// </summary>
        private static void CheckMigration003Data()
        {
            CheckScalarZero("Services with a duration that is not a multiple of 30",
                "SELECT COUNT(*) FROM Services WHERE duration_minutes % 30 <> 0");

            CheckScalarAtLeast("Tirzepatide packages = ", 3,
                "SELECT COUNT(*) FROM Services " +
                "WHERE service_name LIKE 'Tirzepatide (%' AND default_total_sessions IN (4,6,8)");

            CheckScalarAtLeast("Room category links = ", 10,
                "SELECT COUNT(*) FROM RoomServiceCategories");
        }

        /// <summary>
        /// Migration 004: the requires_assessment column must exist and the invasive services must
        /// already be flagged, otherwise the booking screen would never ask for the nurse.
        /// Read-only; "at least" style for both checks.
        /// </summary>
        private static void CheckMigration004Data()
        {
            CheckScalarAtLeast("Migration 004 column exists = ", 1,
                "SELECT COUNT(*) FROM sys.columns " +
                "WHERE object_id = OBJECT_ID('dbo.Services') AND name = 'requires_assessment'");

            CheckScalarAtLeast("Services that need an assessment = ", 1,
                "SELECT COUNT(*) FROM Services WHERE requires_assessment = 1");
        }

        /// <summary>GetLog for the last 7 days must run and come back with the columns the grid needs.</summary>
        private static void CheckActivityLog()
        {
            const string label = "ActivityRepository.GetLog(last 7 days)";
            try
            {
                DataTable log = ActivityRepository.GetLog(DateTime.Today.AddDays(-7), DateTime.Today, 0, "All", "");

                // The grid binds by name, so a renamed alias would show empty cells instead of failing.
                bool hasColumns = log.Columns.Contains("log_id")
                    && log.Columns.Contains("logged_at")
                    && log.Columns.Contains("full_name")
                    && log.Columns.Contains("action")
                    && log.Columns.Contains("module")
                    && log.Columns.Contains("description");

                string message = label + " rows = " + log.Rows.Count + " (6 columns)";
                if (hasColumns) Pass(message);
                else Fail(message + " - a column is missing");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// The baseline script makes 18 tables and a migration may add more, so this is "at least".
        /// </summary>
        private static void CheckMaintenanceTableCounts()
        {
            string label = "MaintenanceRepository.GetTableCounts returns at least " + MinimumTables + " tables";
            try
            {
                DataTable counts = MaintenanceRepository.GetTableCounts();
                string message = label + " (got " + counts.Rows.Count + ")";
                if (counts.Rows.Count >= MinimumTables) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The System information card needs one row with the three values.</summary>
        private static void CheckMaintenanceDatabaseInfo()
        {
            const string label = "MaintenanceRepository.GetDatabaseInfo returns a row";
            try
            {
                DataRow info = MaintenanceRepository.GetDatabaseInfo();
                if (info != null) Pass(label);
                else Fail(label + " (no row)");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>GetQueue(today) must list a day (usually empty) without an error.</summary>
        private static void CheckBillingQueue()
        {
            const string label = "BillingRepository.GetQueue(today)";
            try
            {
                DataTable queue = BillingRepository.GetQueue(DateTime.Today);
                Pass(label + " rows = " + queue.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The Transactions tab starts on the current month.</summary>
        private static void CheckBillingTransactions()
        {
            const string label = "BillingRepository.GetTransactions(first day of month, today, \"\", \"All\")";
            try
            {
                DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DataTable payments = BillingRepository.GetTransactions(from, DateTime.Today, "", "All");
                Pass(label + " rows = " + payments.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>An appointment id that does not exist has no bill, and that must not throw.</summary>
        private static void CheckBillingHeaderWithoutBill()
        {
            const string label = "BillingRepository.GetBillHeader(0) returns no row";
            try
            {
                DataRow header = BillingRepository.GetBillHeader(0);
                if (header == null) Pass(label);
                else Fail(label + " (got a row)");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The Commissions page opens on this month; the query must run without an error.</summary>
        private static void CheckCommissionSummary()
        {
            const string label = "CommissionRepository.GetSummary(this month, all staff, all payouts)";
            try
            {
                DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DataTable summary = CommissionRepository.GetSummary(from, DateTime.Today, 0, "All");
                Pass(label + " rows = " + summary.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The Sales tab of the Reports page.</summary>
        private static void CheckReportSalesByCategory()
        {
            const string label = "ReportRepository.GetSalesByCategory(this month)";
            try
            {
                DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DataTable sales = ReportRepository.GetSalesByCategory(from, DateTime.Today);
                Pass(label + " rows = " + sales.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// The Appointments tab always shows the same five rows (zeros included), so the grid never
        /// looks broken on a quiet month.
        /// </summary>
        private static void CheckReportAppointmentStatusCounts()
        {
            const string label = "ReportRepository.GetAppointmentStatusCounts(this month) returns 5 rows";
            try
            {
                DateTime from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DataTable counts = ReportRepository.GetAppointmentStatusCounts(from, DateTime.Today);
                string message = label + " (got " + counts.Rows.Count + ")";

                if (counts.Rows.Count == 5) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The month query of the Dashboard calendar must run for the current month.</summary>
        private static void CheckMonthAppointments()
        {
            const string label = "AppointmentRepository.GetMonthAppointments(this month)";
            try
            {
                DataTable month = AppointmentRepository.GetMonthAppointments(DateTime.Today.Year, DateTime.Today.Month);
                Pass(label + " rows = " + month.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The five KPI numbers must come back as one row.</summary>
        private static void CheckDashboardStats()
        {
            const string label = "ReportRepository.GetDashboardStats(today, true) returns one row";
            try
            {
                DataRow stats = ReportRepository.GetDashboardStats(DateTime.Today, true);

                if (stats == null) Fail(label + " (no row)");
                else Pass(label);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// An empty month must still fill the calendar with its 42 day cells. The cells are built
        /// in code (the "items generated from data" exception), so this is the check that they
        /// really are created without a parent window.
        /// </summary>
        private static void CheckCalendarCells()
        {
            const string label = "ucCalendarMonth.LoadMonth(empty table) builds 42 cells";
            try
            {
                DataTable empty = AppointmentRepository.GetMonthAppointments(DateTime.Today.Year, DateTime.Today.Month).Clone();

                using (ucCalendarMonth calendar = new ucCalendarMonth())
                {
                    calendar.LoadMonth(DateTime.Today, empty);

                    int cells = CountCells(calendar);

                    if (cells == 42) Pass(label);
                    else Fail(label + " (got " + cells + ")");
                }
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>Counts the direct children of the day grid (the day cells themselves).</summary>
        private static int CountCells(ucCalendarMonth calendar)
        {
            TableLayoutPanel grid = FindGrid(calendar);
            return grid == null ? 0 : grid.Controls.Count;
        }

        /// <summary>The day grid is the table named tlpDays (the weekday strip is a table too).</summary>
        private static TableLayoutPanel FindGrid(ucCalendarMonth calendar)
        {
            foreach (Control child in calendar.Controls)
            {
                if (child is TableLayoutPanel && child.Name == "tlpDays") return child as TableLayoutPanel;
            }

            return null;
        }

        /// <summary>GetByDate must list a day (usually empty) without an error.</summary>
        private static void CheckAppointmentDayList()
        {
            const string label = "AppointmentRepository.GetByDate(today, \"All\", \"\")";
            try
            {
                DataTable day = AppointmentRepository.GetByDate(DateTime.Today, "All", "");
                Pass(label + " rows = " + day.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The booking screen offers a start time every 30 minutes from 10:00 AM to 9:30 PM.</summary>
        private static void CheckTimeSlots()
        {
            const string label = "AppointmentRepository.GetTimeSlots() returns 24 slots";
            try
            {
                List<TimeSpan> slots = AppointmentRepository.GetTimeSlots();
                string message = label + " (got " + slots.Count + ")";

                if (slots.Count == 24) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>A booking without a client must be refused with a readable problem.</summary>
        private static void CheckBookingWithoutClient()
        {
            const string label = "AppointmentRepository.ValidateBooking without a client reports a problem";
            try
            {
                // An empty service list: the missing client and the missing services are both
                // reported, which is exactly what the desk sees on a fresh dialog.
                DataTable lines = new DataTable();
                lines.Columns.Add("service_id", typeof(int));
                lines.Columns.Add("quantity", typeof(int));
                lines.Columns.Add("price", typeof(decimal));
                lines.Columns.Add("duration_minutes", typeof(int));
                lines.Columns.Add("technician_id", typeof(int));

                List<string> problems = AppointmentRepository.ValidateBooking(0, 0,
                    DateTime.Today.AddDays(1), new TimeSpan(14, 0, 0), "Walk-in", lines, 0,
                    0m, "Cash", string.Empty, false);

                string message = label + " (got " + problems.Count + " problem(s))";

                if (problems.Count > 0) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>Search("", "All") must list the clients without an error.</summary>
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

        /// <summary>The booking screen needs the client drop-down list.</summary>
        private static void CheckClientCombo()
        {
            const string label = "ClientRepository.GetForCombo()";
            try
            {
                DataTable clients = ClientRepository.GetForCombo();
                Pass(label + " rows = " + clients.Rows.Count);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>Client id 0 means "nothing selected", so the package list must be empty (not null).</summary>
        private static void CheckEmptyPackageList()
        {
            const string label = "ClientRepository.GetPackages(0) returns an empty table";
            try
            {
                DataTable packages = ClientRepository.GetPackages(0);
                if (packages != null && packages.Rows.Count == 0) Pass(label);
                else Fail(label + " (got " + (packages == null ? "null" : packages.Rows.Count + " rows") + ")");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The ONE discount formula lives in PromotionRepository.ComputeDiscount.</summary>
        private static void CheckComputeDiscount(string type, decimal value, decimal subtotal, decimal expected)
        {
            string label = "PromotionRepository.ComputeDiscount(\"" + type + "\", " +
                value.ToString("0.00") + ", " + subtotal.ToString("0.00") + ")";
            try
            {
                decimal actual = PromotionRepository.ComputeDiscount(type, value, subtotal);
                string message = label + " = " + actual.ToString("0.00") +
                    " (expected " + expected.ToString("0.00") + ")";
                if (actual == expected) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>A new promotion (id 0) has no linked services, so the list must be empty.</summary>
        private static void CheckNoServiceLinks()
        {
            const string label = "PromotionRepository.GetServiceIds(0) returns an empty list";
            try
            {
                List<int> ids = PromotionRepository.GetServiceIds(0);
                if (ids != null && ids.Count == 0) Pass(label);
                else Fail(label + " (got " + (ids == null ? "null" : ids.Count + " ids") + ")");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        private static void Pass(string name)
        {
            _passed++;
            Line("PASS " + name);
        }

        private static void Fail(string name)
        {
            _failed++;
            Line("FAIL " + name);
        }

        private static void Fail(string name, Exception ex)
        {
            _failed++;
            Line("FAIL " + name + " - " + ex.Message);

            string stack = ex.StackTrace ?? "";
            Line("     " + ex.GetType().Name + " at " + stack.Split('\n')[0].Trim());
        }

        private static void Skipped(string name)
        {
            _skipped++;
            Line("SKIPPED " + name + " (no public parameterless constructor)");
        }

        /// <summary>
        /// The booking screen only offers the start times that really work, so these checks run
        /// AvailabilityRepository on a day with nothing booked (today + 400 days). Read-only:
        /// only SELECT queries run, so the shop's data is never changed.
        /// </summary>
        private static void CheckAvailability()
        {
            DateTime emptyDay = DateTime.Today.AddDays(400);

            int facialId = FindServiceId("Facial", false);
            int massageId = FindServiceId("Massage - Feet", false);

            CheckFreeStarts("Free starts of a Facial service on an empty day", facialId, emptyDay,
                new TimeSpan(10, 0, 0), AppointmentRepository.AestheticClosing);
            CheckFreeStarts("Free starts of a massage service on an empty day", massageId, emptyDay,
                new TimeSpan(13, 0, 0), AppointmentRepository.MassageClosing);
            CheckFreeStartsInThePast(facialId);
            CheckFreeStartsToday(facialId);
            CheckPickRoom(facialId, emptyDay);
            CheckPickTechnician(facialId, emptyDay);
            CheckGlutaBusyEnd();
            CheckPlanSessions(emptyDay, new TimeSpan(10, 0, 0));
            CheckSessionText(2, 4, "Session 2 of 4");
            CheckSessionText(0, 0, "Session 1 of 1");
        }

        private static void Line(string text)
        {
            _log.AppendLine(text);
        }

        /// <summary>
        /// The lowest Active service id of a category, or 0 when the category has none.
        /// onlyWithBusyMinutes keeps only a service whose technician leaves before the room
        /// (a gluta drip), which is what the busy-end check needs.
        /// </summary>
        private static int FindServiceId(string categoryName, bool onlyWithBusyMinutes)
        {
            string sql =
@"SELECT TOP 1 s.service_id
FROM Services s
INNER JOIN ServiceCategories c ON c.category_id = s.category_id
WHERE c.category_name = @name AND s.status = 'Active'";

            if (onlyWithBusyMinutes) sql += " AND s.staff_busy_minutes > 0";

            object id = Db.Scalar(sql + " ORDER BY s.service_id", Db.P("@name", categoryName));

            return id == null || id == DBNull.Value ? 0 : Convert.ToInt32(id);
        }

        /// <summary>
        /// On an empty day a service must fill the whole opening hours: the first start is the
        /// opening time, every start is on the half hour, and none is too late to finish in time.
        /// </summary>
        private static void CheckFreeStarts(string label, int serviceId, DateTime date,
            TimeSpan expectedFirst, TimeSpan closing)
        {
            try
            {
                if (serviceId == 0)
                {
                    Fail(label + " (no Active service of that category)");
                    return;
                }

                DataRow service = AppointmentRepository.GetServiceInfo(null, null, serviceId);
                int duration = Db.Get<int>(service["duration_minutes"]);
                // One session is booked here: the package size has nothing to do with the length
                // of one service line.
                const int quantity = 1;

                List<TimeSpan> starts = AvailabilityRepository.GetFreeStarts(null, null, date,
                    serviceId, quantity, 0, 0, 0, null);

                string message = label + " (got " + starts.Count + " start(s))";

                if (starts.Count < 1)
                {
                    Fail(message);
                    return;
                }

                if (starts[0] != expectedFirst)
                {
                    Fail(message + " first " + AppointmentRepository.Clock(starts[0]) + " is not " +
                        AppointmentRepository.Clock(expectedFirst));
                    return;
                }

                bool lastTooLate = false;
                foreach (TimeSpan start in starts)
                {
                    if (start.TotalMinutes % AvailabilityRepository.SlotMinutes != 0) lastTooLate = true;

                    if (AvailabilityRepository.LineEnd(start, duration, quantity) > closing)
                    {
                        lastTooLate = true;
                    }
                }

                if (lastTooLate) Fail(message + " a start is off the half hour or ends after closing");
                else Pass(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>A day that has already gone by can never offer a start time.</summary>
        private static void CheckFreeStartsInThePast(int serviceId)
        {
            const string label = "Free starts in the past";
            try
            {
                if (serviceId == 0)
                {
                    Fail(label + " (no Active Facial service)");
                    return;
                }

                List<TimeSpan> starts = AvailabilityRepository.GetFreeStarts(null, null,
                    DateTime.Today.AddDays(-1), serviceId, 1, 0, 0, 0, null);

                string message = label + " (got " + starts.Count + ")";

                if (starts.Count == 0) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The booking screen never offers a time that has already gone by today.</summary>
        private static void CheckFreeStartsToday(int serviceId)
        {
            const string label = "Free starts today are all in the future";
            try
            {
                if (serviceId == 0)
                {
                    Fail(label + " (no Active Facial service)");
                    return;
                }

                TimeSpan now = DateTime.Now.TimeOfDay;
                List<TimeSpan> starts = AvailabilityRepository.GetFreeStarts(null, null,
                    DateTime.Today, serviceId, 1, 0, 0, 0, null);

                bool allFuture = true;
                foreach (TimeSpan start in starts)
                {
                    if (start <= now) allFuture = false;
                }

                string message = label + " (got " + starts.Count + " start(s))";

                if (allFuture) Pass(message);
                else Fail(message + " a start is not in the future");
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>A Facial room is linked in RoomServiceCategories, so one must be free.</summary>
        private static void CheckPickRoom(int serviceId, DateTime date)
        {
            const string label = "PickRoom finds a Facial room on an empty day";
            try
            {
                if (serviceId == 0)
                {
                    Fail(label + " (no Active Facial service)");
                    return;
                }

                DataRow service = AppointmentRepository.GetServiceInfo(null, null, serviceId);
                int duration = Db.Get<int>(service["duration_minutes"]);
                TimeSpan start = new TimeSpan(11, 0, 0);
                TimeSpan end = AvailabilityRepository.LineEnd(start, duration, 1);

                string roomName;
                int roomId = AvailabilityRepository.PickRoom(null, null, date,
                    Db.Get<int>(service["category_id"]), start, end, 0, out roomName);

                string message = label + " (room " + roomId + " '" + roomName + "')";

                if (roomId > 0) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The seeded aestheticians have nothing booked that day, so one is picked.</summary>
        private static void CheckPickTechnician(int serviceId, DateTime date)
        {
            const string label = "PickTechnician finds an aesthetician on an empty day";
            try
            {
                if (serviceId == 0)
                {
                    Fail(label + " (no Active Facial service)");
                    return;
                }

                int userId = AvailabilityRepository.PickTechnician(null, null, date, serviceId, 1,
                    new TimeSpan(11, 0, 0), 0);

                string message = label + " (user " + userId + ")";

                if (userId > 0) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// A gluta drip keeps the room for the whole drip but the aesthetician only for
        /// staff_busy_minutes, so the busy end is earlier than the room end.
        /// </summary>
        private static void CheckGlutaBusyEnd()
        {
            const string label = "A gluta drip keeps the chair but not the aesthetician";
            try
            {
                int glutaId = FindServiceId("Glutathione", true);

                if (glutaId == 0)
                {
                    Skipped(label);
                    return;
                }

                DataRow service = AppointmentRepository.GetServiceInfo(null, null, glutaId);
                int duration = Db.Get<int>(service["duration_minutes"]);
                int staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);

                // No drip with a short busy time in this database, so there is nothing to prove.
                if (staffBusyMinutes <= 0)
                {
                    Skipped(label);
                    return;
                }

                TimeSpan start = new TimeSpan(14, 0, 0);
                TimeSpan end = AvailabilityRepository.LineEnd(start, duration, 1);
                // A drip is sold one at a time, so one unit is enough to prove the rule.
                TimeSpan busyEnd = AvailabilityRepository.LineBusyEnd(start, duration,
                    staffBusyMinutes, 1);

                string message = label + " (room until " + AppointmentRepository.Clock(end) +
                    ", technician free at " + AppointmentRepository.Clock(busyEnd) + ")";

                if (busyEnd < end) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>
        /// A package is booked as weekly sessions: three more sessions follow the first one, each
        /// session_interval_days later, and on an empty day every one of them fits.
        /// </summary>
        private static void CheckPlanSessions(DateTime firstDate, TimeSpan start)
        {
            const string label = "PlanSessions plans 3 weekly Tirzepatide sessions";
            try
            {
                int serviceId = FindTirzepatideServiceId();

                if (serviceId == 0)
                {
                    Skipped(label);
                    return;
                }

                DataTable plan = AvailabilityRepository.PlanSessions(null, null, 0, serviceId, firstDate,
                    start, 1, 4, 0);

                string message = label + " (got " + plan.Rows.Count + " row(s))";

                if (plan.Rows.Count != 3)
                {
                    Fail(message);
                    return;
                }

                for (int index = 0; index < plan.Rows.Count; index++)
                {
                    DataRow row = plan.Rows[index];
                    int expectedNumber = 2 + index;
                    // Session 2 is 7 days after the first one, so the step is number - 1.
                    DateTime expectedDate = firstDate.AddDays(7 * (expectedNumber - 1));

                    bool numberOk = Db.Get<int>(row["session_number"]) == expectedNumber;
                    bool dateOk = Db.Get<DateTime>(row["session_date"]).Date == expectedDate.Date;
                    bool okOk = Db.Get<bool>(row["ok"]);

                    if (!numberOk || !dateOk || !okOk)
                    {
                        Fail(message + " row " + expectedNumber + " is " +
                            Db.Get<int>(row["session_number"]) + " on " +
                            Db.Get<DateTime>(row["session_date"]).ToString("yyyy-MM-dd") + ", ok = " +
                            okOk + " - " + Db.Get<string>(row["problem"]));
                        return;
                    }
                }

                Pass(message + " on " + plan.Rows[2]["session_date"]);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>The lowest Active "Tirzepatide (4 sessions)" service id, or 0 when it is gone.</summary>
        private static int FindTirzepatideServiceId()
        {
            object id = Db.Scalar(
                "SELECT TOP 1 service_id FROM Services " +
                "WHERE service_name = @name AND status = 'Active' ORDER BY service_id",
                Db.P("@name", "Tirzepatide (4 sessions)"));

            return id == null || id == DBNull.Value ? 0 : Convert.ToInt32(id);
        }

        /// <summary>The session label of a package line, with "Session 1 of 1" for a plain service.</summary>
        private static void CheckSessionText(int sessionNumber, int totalSessions, string expected)
        {
            string label = "AppointmentRepository.SessionText(" + sessionNumber + ", " +
                totalSessions + ")";
            try
            {
                string actual = AppointmentRepository.SessionText(sessionNumber, totalSessions);
                string message = label + " = \"" + actual + "\" (expected \"" + expected + "\")";

                if (actual == expected) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
            }
        }

        /// <summary>Overwrites smoketest.log on every run. Never throws (a missing log must not hide the result).</summary>
        private static void WriteLog()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smoketest.log");
                File.WriteAllText(path, _log.ToString());
            }
            catch
            {
                // The exit code still tells the caller whether the checks passed.
            }
        }
    }
}
