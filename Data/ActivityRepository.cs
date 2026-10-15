using System;

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
    }
}
