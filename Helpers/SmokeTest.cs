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
        private const int ExpectedTables = 17;
        private const int ExpectedServices = 77;
        private const int ExpectedMinUsers = 8;

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
            CheckCount("Services rows", "SELECT COUNT(*) FROM Services", ExpectedServices);

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
                    services.Rows.Count + " (expected 77)";
                if (services.Rows.Count == ExpectedServices) Pass(message);
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
