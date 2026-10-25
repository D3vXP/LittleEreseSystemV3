using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Writes the audit trail (who did what, in which module).
    /// </summary>
    internal static class ActivityRepository
    {
        /// <summary>
        /// Adds one row to ActivityLog.
        /// WHY it swallows every error: an audit entry is nice to have, but it must never stop
        /// the user from saving a client or a payment, so a failure here is silently ignored.
        /// </summary>
        public static void Log(int userId, string action, string module, string description)
        {
            try
            {
                Db.Execute(
                    "INSERT INTO ActivityLog (user_id, action, module, description) " +
                    "VALUES (@user_id, @action, @module, @description)",
                    Db.P("@user_id", userId),
                    Db.P("@action", action),
                    Db.P("@module", module),
                    Db.P("@description", description));
            }
            catch
            {
                // Logging must not break the app (no user_id, no table, database offline, ...).
            }
        }

        // ---------------------------------------------------------------------
        // Activity Log screen (Admin only)
        // ---------------------------------------------------------------------

        /// <summary>
        /// The audit rows the Activity Log page shows, newest first.
        ///   from / to - the picked dates; the whole "to" day is included
        ///   userId    - 0 means every user
        ///   module    - "All" (or empty) means every module
        ///   search    - matches the action OR the description ("" = everything)
        /// Returns log_id, logged_at, full_name, action, module, description.
        /// </summary>
        public static DataTable GetLog(DateTime from, DateTime to, int userId, string module, string search)
        {
            // The "to" day must be compared with the NEXT day at midnight, otherwise an entry
            // written at 3 PM on the last day would fall outside the range.
            DateTime toExclusive = to.Date.AddDays(1);

            string term = search == null ? string.Empty : search.Trim();
            string selectedModule = string.IsNullOrEmpty(module) ? "All" : module;

            return Db.Query(
                "SELECT log_id, logged_at, full_name, action, module, description " +
                "FROM ActivityLog " +
                "INNER JOIN Users ON Users.user_id = ActivityLog.user_id " +
                "WHERE logged_at >= @from AND logged_at < @to " +
                "  AND (@user_id = 0 OR ActivityLog.user_id = @user_id) " +
                "  AND (@module = 'All' OR ActivityLog.module = @module) " +
                "  AND (@search = '' OR action LIKE @like OR description LIKE @like) " +
                "ORDER BY logged_at DESC, log_id DESC",
                Db.P("@from", from.Date),
                Db.P("@to", toExclusive),
                Db.P("@user_id", userId),
                Db.P("@module", selectedModule),
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"));
        }

        /// <summary>
        /// The distinct module names that were ever logged, for the Module filter.
        /// An empty table is fine (a brand-new database) - the page then only offers "All".
        /// </summary>
        public static DataTable GetModules()
        {
            return Db.Query(
                "SELECT DISTINCT module FROM ActivityLog " +
                "WHERE module IS NOT NULL AND module <> '' " +
                "ORDER BY module");
        }
    }
}
