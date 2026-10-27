using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Creates and fills the database named in the connection string when it does not exist yet,
    /// so a fresh clone runs without any manual SSMS work.
    /// WHY it connects to "master" first: a connection that names a missing database can never be
    /// opened, so the existence check has to happen on the server itself.
    /// It NEVER drops or alters an existing database: when DB_ID() finds the name, nothing happens.
    /// </summary>
    internal static class DatabaseBootstrapper
    {
        // Only letters, digits and the underscore are allowed inside the [brackets] of a database
        // name, so a strange name in App.config can never break (or inject into) the SQL text.
        private static readonly Regex SafeDatabaseName = new Regex("^[A-Za-z0-9_]+$");

        // The two scripts of the Database folder, in the order they must run.
        private const string SchemaScript = "LittleEreses_Database.sql";
        private const string SeedScript = "SeedData.sql";

        // The numbered update files, in the Database folder next to the exe.
        private const string MigrationsFolder = "Migrations";

        /// <summary>
        /// Makes sure the database exists and fills it from the two .sql scripts when it is new.
        /// Throws InvalidOperationException with a sentence a normal user can read on any failure.
        /// </summary>
        public static void EnsureDatabase()
        {
            string databaseName;
            SqlConnectionStringBuilder master = MasterConnection(out databaseName);

            using (SqlConnection connection = new SqlConnection(master.ConnectionString))
            {
                Open(connection);

                // An existing database is never touched - not one table, not one row.
                if (DatabaseExists(connection, databaseName)) return;

                CreateDatabase(connection, databaseName);
            }

            // The scripts run INSIDE the new database, so their CREATE TABLE statements land there.
            using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
            {
                Open(connection);
                RunScript(connection, SchemaScript);
                RunScript(connection, SeedScript);
            }
        }

        /// <summary>
        /// Runs the numbered update files of the Migrations folder on a database that already exists
        /// (a tester's LittleEresesDB), so new columns and tables arrive without deleting anything.
        /// Every file runs at most once: its name is written to SchemaVersion inside the same
        /// transaction, so a file that fails is rolled back completely and tried again next start.
        /// Nothing here ever drops or truncates a table.
        /// </summary>
        public static void ApplyMigrations()
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", MigrationsFolder);
            if (!Directory.Exists(folder)) return;

            string[] files = Directory.GetFiles(folder, "*.sql");
            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
            {
                Open(connection);
                EnsureSchemaVersionTable(connection);

                foreach (string path in files)
                {
                    string fileName = Path.GetFileName(path);

                    // A file already listed has run on this database before, so it is skipped.
                    if (MigrationApplied(connection, fileName)) continue;

                    RunMigration(connection, fileName, path);
                }
            }
        }

        /// <summary>
        /// The list of applied files. Its name is written with it, so a half-finished migration can
        /// simply be run again on the next start.
        /// </summary>
        private static void EnsureSchemaVersionTable(SqlConnection connection)
        {
            const string sql =
                "IF OBJECT_ID('dbo.SchemaVersion', 'U') IS NULL " +
                "CREATE TABLE dbo.SchemaVersion (" +
                "  script_name nvarchar(200) NOT NULL PRIMARY KEY, " +
                "  applied_at datetime2 NOT NULL CONSTRAINT DF_SchemaVersion_applied_at DEFAULT SYSUTCDATETIME())";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>True when the schema version table already holds this file name.</summary>
        private static bool MigrationApplied(SqlConnection connection, string fileName)
        {
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM dbo.SchemaVersion WHERE script_name = @script_name", connection))
            {
                command.Parameters.Add(Db.P("@script_name", fileName));
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        /// <summary>
        /// Runs one file batch by batch (the same GO splitter as the baseline script) and records the
        /// file in the SAME transaction, so the update is either fully applied and listed, or not at
        /// all. A failure stops the whole run with a sentence naming the file.
        /// </summary>
        private static void RunMigration(SqlConnection connection, string fileName, string path)
        {
            List<string> batches;
            try
            {
                batches = SplitOnGo(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Database update failed in " + fileName + ": " + ex.Message + " (looked in " + path + ")", ex);
            }

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (string batch in batches)
                    {
                        using (SqlCommand command = new SqlCommand(batch, connection, transaction))
                        {
                            // A big update file can take a while on a cold LocalDB.
                            command.CommandTimeout = 300;
                            command.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand command = new SqlCommand("INSERT INTO dbo.SchemaVersion (script_name) VALUES (@script_name)", connection, transaction))
                    {
                        command.Parameters.Add(Db.P("@script_name", fileName));
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // The rollback itself can fail on a broken connection; the first error is the
                    // one the user needs, so it is the one that is reported.
                    try { transaction.Rollback(); }
                    catch { /* ignored on purpose */ }

                    SqlException sql = ex as SqlException;
                    string reason = sql == null ? ex.Message : DescribeBootError(sql);
                    throw new InvalidOperationException(
                        "Database update failed in " + fileName + ": " + reason, ex);
                }
            }
        }

        /// <summary>
        /// The same server and the same security settings as App.config, but connected to "master",
        /// which is the one database that always exists.
        /// </summary>
        private static SqlConnectionStringBuilder MasterConnection(out string databaseName)
        {
            SqlConnectionStringBuilder builder;
            try
            {
                builder = new SqlConnectionStringBuilder(Db.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "The connection string LittleEreseDb in App.config cannot be read. " + ex.Message, ex);
            }

            databaseName = builder.InitialCatalog;

            if (string.IsNullOrEmpty(databaseName) || !SafeDatabaseName.IsMatch(databaseName))
            {
                throw new InvalidOperationException(
                    "The database name in App.config ('" + databaseName + "') may only contain letters, digits and the underscore.");
            }

            builder.InitialCatalog = "master";
            return builder;
        }

        /// <summary>Opens a connection and turns a "server not found" error into a plain sentence.</summary>
        private static void Open(SqlConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(DescribeBootError(ex), ex);
            }
        }

        /// <summary>DB_ID() is NULL when no database of that name exists on the server.</summary>
        private static bool DatabaseExists(SqlConnection connection, string databaseName)
        {
            using (SqlCommand command = new SqlCommand("SELECT DB_ID(@database_name)", connection))
            {
                command.Parameters.Add(Db.P("@database_name", databaseName));
                object result = command.ExecuteScalar();
                return result != null && result != DBNull.Value;
            }
        }

        /// <summary>Creates an empty database. The name is validated above, so the brackets are safe.</summary>
        private static void CreateDatabase(SqlConnection connection, string databaseName)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("CREATE DATABASE [" + databaseName + "]", connection))
                {
                    // A LocalDB instance can be slow on the first start of the day.
                    command.CommandTimeout = 120;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(DescribeBootError(ex), ex);
            }
        }

        /// <summary>
        /// Runs one .sql file against the new database, batch by batch (one batch = one GO).
        /// The database-level lines (USE, CREATE / ALTER / DROP DATABASE and the DB_ID guard in
        /// front of them) are dropped: the connection already points at the right database and this
        /// class must never create or change another one.
        /// </summary>
        private static void RunScript(SqlConnection connection, string fileName)
        {
            string path = ScriptPath(fileName);
            string text;

            try
            {
                text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "The database script '" + fileName + "' could not be read from '" + path + "'. " +
                    "Build the project again so the Database folder is copied next to the program.", ex);
            }

            foreach (string batch in SplitOnGo(text))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(batch, connection))
                    {
                        // A cold LocalDB can take a while to write 18 tables.
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw new InvalidOperationException(
                        "The database was created but '" + fileName + "' could not be loaded. " +
                        DescribeBootError(ex), ex);
                }
            }
        }

        /// <summary>
        /// The lines of the file grouped into batches. A line holding nothing but GO is the
        /// separator, and the database-level lines are left out (see RunScript).
        /// </summary>
        private static List<string> SplitOnGo(string text)
        {
            List<string> batches = new List<string>();
            StringBuilder batch = new StringBuilder();

            string[] lines = (text ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            foreach (string line in lines)
            {
                string trimmed = line.Trim();

                if (trimmed.Equals("GO", StringComparison.OrdinalIgnoreCase))
                {
                    AddBatch(batches, batch);
                    batch.Length = 0;
                    continue;
                }

                if (IsDatabaseLevelLine(trimmed)) continue;

                batch.AppendLine(line);
            }

            AddBatch(batches, batch);
            return batches;
        }

        /// <summary>Adds one batch, unless it holds nothing but comments and blank lines.</summary>
        private static void AddBatch(List<string> batches, StringBuilder batch)
        {
            string text = batch.ToString().Trim();
            if (text.Length > 0) batches.Add(text);
        }

        /// <summary>
        /// True for the lines that belong to the whole server or to another database: USE,
        /// CREATE / ALTER / DROP DATABASE and the "IF DB_ID(...) IS NULL" guard in front of them.
        /// The schema file carries them so it can also be run by hand in SSMS.
        /// </summary>
        private static bool IsDatabaseLevelLine(string trimmedLine)
        {
            if (trimmedLine.Length == 0) return false;

            if (trimmedLine.StartsWith("USE ", StringComparison.OrdinalIgnoreCase)) return true;
            if (trimmedLine.StartsWith("CREATE DATABASE", StringComparison.OrdinalIgnoreCase)) return true;
            if (trimmedLine.StartsWith("ALTER DATABASE", StringComparison.OrdinalIgnoreCase)) return true;
            if (trimmedLine.StartsWith("DROP DATABASE", StringComparison.OrdinalIgnoreCase)) return true;
            if (trimmedLine.StartsWith("IF DB_ID(", StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        /// <summary>The script next to the exe (the .csproj copies the Database folder there).</summary>
        private static string ScriptPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", fileName);
        }

        /// <summary>
        /// The startup errors a user can really hit, in plain English. Everything else keeps the
        /// wording of Db.Describe, so the app has only one voice for database problems.
        /// </summary>
        private static string DescribeBootError(SqlException ex)
        {
            if (ex == null) return "The database could not be prepared.";

            switch (ex.Number)
            {
                case 53:
                case 26:
                case -1:
                case 2:
                    return "LocalDB was not found. Install SQL Server Express LocalDB from the Visual Studio Installer.";
                case 18456:
                    return "SQL Server LocalDB refused the login. Check that the Windows account you are signed in with may open (localdb)\\MSSQLLocalDB.";
                default:
                    return Db.Describe(ex);
            }
        }
    }
}
