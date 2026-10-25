using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// The only place that opens database connections.
    /// All repositories call these helpers and always pass values as parameters (never string-concatenate SQL).
    /// Never call these from a constructor or from design-time code (see DesignTime.IsDesignMode).
    /// </summary>
    internal static class Db
    {
        private const string ConnectionName = "LittleEreseDb";

        /// <summary>The connection string read from App.config (name "LittleEreseDb").</summary>
        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName];
                if (settings == null)
                {
                    throw new InvalidOperationException("Connection string '" + ConnectionName + "' is missing in App.config.");
                }
                return settings.ConnectionString;
            }
        }

        /// <summary>Creates a closed connection to the database. Callers use it inside using (...).</summary>
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>Shortcut for creating a parameter. A null value is stored as NULL.</summary>
        public static SqlParameter P(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>Runs a SELECT and returns the rows as a DataTable (ready for a DataGridView).</summary>
        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>Runs INSERT / UPDATE / DELETE and returns the number of affected rows.</summary>
        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>Runs a query that returns a single value (for example SELECT SCOPE_IDENTITY()).</summary>
        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = CreateConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                connection.Open();
                return command.ExecuteScalar();
            }
        }

        // ---------------------------------------------------------------------
        // Transaction helpers
        // WHY: booking one appointment writes rows in six tables. If the app stops in the
        // middle (power cut, bad data, an error near the end) the appointment would be left
        // half saved. These helpers let a repository do all of it as ONE unit of work.
        // ---------------------------------------------------------------------

        /// <summary>
        /// Opens a connection, begins a transaction, runs the work, commits it, and rolls the
        /// whole thing back when anything inside throws. The exception is rethrown, so the
        /// caller can still show the user a message.
        /// Usage:
        ///     Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
        ///     {
        ///         Db.Execute(connection, transaction, "INSERT INTO ...", Db.P("@x", 1));
        ///     });
        /// </summary>
        public static void InTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            if (work == null) return;

            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        work(connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        // The rollback itself can fail (the connection may already be gone).
                        // The original error is the one the user needs to see, so it is rethrown.
                        try { transaction.Rollback(); }
                        catch { /* ignored on purpose */ }
                        throw;
                    }
                }
            }
        }

        /// <summary>Runs a SELECT inside an already open transaction (see InTransaction).</summary>
        public static DataTable Query(SqlConnection connection, SqlTransaction transaction, string sql, params SqlParameter[] parameters)
        {
            // A connection of null means "no transaction here", so the normal helper is used.
            if (connection == null) return Query(sql, parameters);

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>Runs INSERT / UPDATE / DELETE inside an already open transaction.</summary>
        public static int Execute(SqlConnection connection, SqlTransaction transaction, string sql, params SqlParameter[] parameters)
        {
            if (connection == null) return Execute(sql, parameters);

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>Runs a single-value query inside an already open transaction.</summary>
        public static object Scalar(SqlConnection connection, SqlTransaction transaction, string sql, params SqlParameter[] parameters)
        {
            if (connection == null) return Scalar(sql, parameters);

            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Reads a value safely. NULL / DBNull returns the fallback instead of throwing InvalidCastException.
        /// Example: int id = Db.Get&lt;int&gt;(row["client_id"]);
        /// </summary>
        public static T Get<T>(object value, T fallback = default(T))
        {
            if (value == null || value == DBNull.Value) return fallback;
            if (value is T) return (T)value;
            Type target = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return (T)Convert.ChangeType(value, target);
        }

        /// <summary>Checks that the database is reachable. Used by the login screen and the demo seeder.</summary>
        public static bool CanConnect(out string error)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                }
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = Describe(ex);
                return false;
            }
        }

        /// <summary>Turns common SQL Server errors into messages a normal user can understand.</summary>
        public static string Describe(Exception ex)
        {
            SqlException sql = ex as SqlException;
            if (sql == null) return ex.Message;

            switch (sql.Number)
            {
                case 2627:
                case 2601:
                    return "This record already exists (duplicate value).";
                case 547:
                    return "This action conflicts with related records or an allowed value. Check the selected items and values.";
                case 515:
                    return "A required field is empty.";
                case 8152:
                    return "One of the entered values is too long.";
                case 4060:
                    return "The database 'LittleEresesDB' was not found. Run Database\\LittleEreses_Database.sql first.";
                case 18456:
                    return "Database login failed.";
                case 53:
                case 26:
                case -1:
                case 2:
                    return "Cannot reach SQL Server LocalDB. Check that LocalDB is installed and the connection string in App.config is correct.";
                default:
                    return "Database error (" + sql.Number + "): " + sql.Message;
            }
        }
    }
}
