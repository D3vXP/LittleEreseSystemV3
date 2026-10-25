using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Backup and restore of the whole LittleEresesDB database (Admin only).
    /// WHY this is a repository: it is the only place that is allowed to write raw SQL,
    /// and BACKUP / RESTORE cannot be expressed with the normal Db helpers.
    /// Every method throws InvalidOperationException with a sentence a normal user can read.
    /// </summary>
    internal static class MaintenanceRepository
    {
        // Only letters, digits and the underscore are allowed inside [brackets], so a strange
        // database name can never break (or inject into) the BACKUP / RESTORE statement.
        private static readonly Regex SafeName = new Regex("^[A-Za-z0-9_]+$");

        // The error SQL Server raises when the service account cannot write to the folder.
        private const int CannotOpenBackupDevice = 3201;

        /// <summary>
        /// Writes a full backup of the database to <paramref name="filePath"/>.
        /// WITH INIT, FORMAT overwrites an older file of the same name completely.
        /// </summary>
        public static void BackupDatabase(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("Choose the folder where the backup should be saved.");
            }

            try
            {
                string database = DatabaseName();

                // The path is a parameter, never pasted into the SQL text.
                Db.Execute(
                    "BACKUP DATABASE [" + database + "] TO DISK = @path " +
                    "WITH INIT, FORMAT, NAME = 'LittleEresesDB backup'",
                    Db.P("@path", filePath));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Describe(ex));
            }
        }

        /// <summary>
        /// Asks SQL Server to read the whole backup file and check it, without restoring anything.
        /// </summary>
        public static void VerifyBackup(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("Choose the backup file first.");
            }

            try
            {
                Db.Execute(
                    "RESTORE VERIFYONLY FROM DISK = @path",
                    Db.P("@path", filePath));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Describe(ex));
            }
        }

        /// <summary>
        /// Replaces the current database with the contents of the backup file.
        /// WHY the special route: a database cannot be restored while the app is connected to it,
        /// so the work is done from "master". Every other connection is dropped first (the pool is
        /// cleared), the database is put in SINGLE_USER mode, restored and ALWAYS put back to
        /// MULTI_USER - even when the restore fails, otherwise the shop could never open it again.
        /// </summary>
        public static void RestoreDatabase(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("Choose the backup file first.");
            }

            try
            {
                string database = DatabaseName();

                // The same server and the same security, but connected to master.
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Db.ConnectionString);
                builder.InitialCatalog = "master";

                // Every pooled connection to the database must be gone before SINGLE_USER,
                // otherwise the ALTER DATABASE below waits forever.
                SqlConnection.ClearAllPools();

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    try
                    {
                        Execute(connection, false, "ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                        Execute(connection, true, "RESTORE DATABASE [" + database + "] FROM DISK = @path WITH REPLACE", filePath);
                    }
                    finally
                    {
                        // The app must be able to connect again whatever happened above.
                        try
                        {
                            Execute(connection, false, "ALTER DATABASE [" + database + "] SET MULTI_USER");
                        }
                        catch
                        {
                            // Nothing more can be done here; the original error is more useful.
                        }
                    }
                }

                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(Describe(ex));
            }
        }

        /// <summary>
        /// The name, the server and the size of the database for the System information card.
        /// Returns one row: db_name, server_name, size_mb.
        /// </summary>
        public static DataRow GetDatabaseInfo()
        {
            DataTable table = Db.Query(
                "SELECT DB_NAME() AS db_name, " +
                "       CAST(SERVERPROPERTY('ServerName') AS NVARCHAR(128)) AS server_name, " +
                "       CAST(ISNULL((SELECT SUM(size) * 8.0 / 1024.0 FROM sys.master_files " +
                "            WHERE database_id = DB_ID() AND type_desc = 'ROWS'), 0) AS DECIMAL(18,2)) AS size_mb");

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// How many rows every user table holds, biggest tables last (alphabetical, so the list
        /// does not jump around after a backup). Returns table_name and row_count.
        /// </summary>
        public static DataTable GetTableCounts()
        {
            // index_id 0 is a heap, 1 the clustered index - together they are exactly one row
            // per partition, so the sum is the real record count.
            return Db.Query(
                "SELECT t.name AS table_name, SUM(p.rows) AS row_count " +
                "FROM sys.tables t " +
                "INNER JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1) " +
                "WHERE t.is_ms_shipped = 0 " +
                "GROUP BY t.name " +
                "ORDER BY t.name");
        }

        /// <summary>
        /// The *.bak files in a folder, newest first. Returns file_name, size_mb and created_at.
        /// A missing folder simply gives an empty table (the page then shows "No backup yet").
        /// </summary>
        public static DataTable ListBackups(string folder)
        {
            DataTable table = new DataTable();
            table.Columns.Add("file_name", typeof(string));
            table.Columns.Add("size_mb", typeof(decimal));
            table.Columns.Add("created_at", typeof(DateTime));

            if (string.IsNullOrEmpty(folder) || !Directory.Exists(folder)) return table;

            string[] files;
            try
            {
                files = Directory.GetFiles(folder, "*.bak");
            }
            catch (Exception)
            {
                // A folder we may not read (a network drive, a protected folder): show nothing.
                return table;
            }

            // LastWriteTime, not CreationTime: a backup copied from another computer keeps the
            // moment it was written, which is the date the owner cares about.
            // POLISH: switch to CreationTime if the owner prefers that.
            DataTable rows = table.Clone();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo info = new FileInfo(files[i]);
                DataRow row = rows.NewRow();
                row["file_name"] = info.Name;
                row["size_mb"] = Math.Round((decimal)info.Length / 1048576m, 2);
                row["created_at"] = info.LastWriteTime;
                rows.Rows.Add(row);
            }

            DataView view = rows.DefaultView;
            view.Sort = "created_at DESC";
            return view.ToTable();
        }

        /// <summary>The database name from the connection string, checked before it goes in brackets.</summary>
        private static string DatabaseName()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Db.ConnectionString);
            string name = builder.InitialCatalog;

            if (string.IsNullOrEmpty(name) || !SafeName.IsMatch(name))
            {
                throw new InvalidOperationException("The database name in App.config is not valid.");
            }

            return name;
        }

        /// <summary>Runs one command on the already open master connection.</summary>
        private static void Execute(SqlConnection connection, bool withPath, string sql, string filePath = null)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                // RESTORE can take minutes on a big database; the default 30 seconds can be too short.
                command.CommandTimeout = 0;

                if (withPath)
                {
                    command.Parameters.Add(new SqlParameter("@path", filePath));
                }

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Turns a SQL Server problem into the sentence the owner should read.
        /// Error 3201 and "access denied" both mean the same thing: SQL Server may not write there.
        /// </summary>
        private static string Describe(Exception ex)
        {
            SqlException sql = ex as SqlException;

            if (sql != null)
            {
                if (sql.Number == CannotOpenBackupDevice || LooksLikeAccessDenied(sql.Message))
                {
                    return "SQL Server could not write to that folder. Choose a folder inside your Documents folder.";
                }

                return Db.Describe(ex);
            }

            if (ex is UnauthorizedAccessException || LooksLikeAccessDenied(ex.Message))
            {
                return "SQL Server could not write to that folder. Choose a folder inside your Documents folder.";
            }

            return ex.Message;
        }

        private static bool LooksLikeAccessDenied(string message)
        {
            if (string.IsNullOrEmpty(message)) return false;

            return message.IndexOf("denied", StringComparison.OrdinalIgnoreCase) >= 0
                || message.IndexOf("access is denied", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
