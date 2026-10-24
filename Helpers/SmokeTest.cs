using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
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
        // The schema now has 18 tables (PromotionServices was added to link a promo to services).
        private const int ExpectedTables = 18;
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

        public static void Run()
        {
            Line("Little Erese's smoke test - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            // Run the seeder first so this check also works on a brand-new database.
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

            CheckCount("Base tables", "SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped = 0", ExpectedTables);

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

        private static void CheckCount(string label, string sql, int expected)
        {
            try
            {
                int actual = Convert.ToInt32(Db.Scalar(sql));
                string message = label + " = " + actual + " (expected " + expected + ")";
                if (actual == expected) Pass(message);
                else Fail(message);
            }
            catch (Exception ex)
            {
                Fail(label, ex);
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

        private static void Line(string text)
        {
            _log.AppendLine(text);
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
