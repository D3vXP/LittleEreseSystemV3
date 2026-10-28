using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about Services and their categories lives here.
    /// Forms call these methods only - they never write SQL themselves.
    /// Services are never deleted: an unused service is set to 'Archived' instead.
    /// </summary>
    internal static class ServiceRepository
    {
        /// <summary>All service categories (category_id, category_name), sorted by name.</summary>
        public static DataTable GetCategories()
        {
            return Db.Query(
                "SELECT category_id, category_name FROM ServiceCategories ORDER BY category_name");
        }

        /// <summary>
        /// The services for the grid.
        ///   search     - matches the service name OR the category name (empty = no search)
        ///   categoryId - 0 means "all categories"
        ///   status     - "All" means "every status"
        /// Returns service_id, category_name, service_name, description, price,
        /// duration_minutes, default_total_sessions, status.
        /// </summary>
        public static DataTable GetServices(string search, int categoryId, string status)
        {
            string sql =
                "SELECT s.service_id, c.category_name, s.service_name, s.description, s.price, " +
                "       s.duration_minutes, s.default_total_sessions, s.status, s.requires_assessment " +
                "FROM Services s " +
                "INNER JOIN ServiceCategories c ON c.category_id = s.category_id " +
                "WHERE (@search = '' OR s.service_name LIKE @like OR c.category_name LIKE @like) " +
                "  AND (@category_id = 0 OR s.category_id = @category_id) " +
                "  AND (@status = 'All' OR s.status = @status) " +
                "ORDER BY c.category_name, s.service_name";

            string term = search == null ? string.Empty : search.Trim();

            return Db.Query(sql,
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@category_id", categoryId),
                Db.P("@status", status ?? "All"));
        }

        /// <summary>One service row, or null when the id no longer exists.</summary>
        public static DataRow GetById(int serviceId)
        {
            DataTable table = Db.Query(
                "SELECT service_id, category_id, service_name, description, price, " +
                "       duration_minutes, default_total_sessions, status, " +
                "       staff_busy_minutes, session_interval_days, requires_assessment " +
                "FROM Services WHERE service_id = @service_id",
                Db.P("@service_id", serviceId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// The Active services (with their category name) used by the booking screen later.
        /// Same columns as GetServices so one grid layout can show both.
        /// </summary>
        public static DataTable GetActiveForBooking()
        {
            return Db.Query(
                "SELECT s.service_id, c.category_name, s.service_name, s.description, s.price, " +
                "       s.duration_minutes, s.default_total_sessions, s.status, s.requires_assessment " +
                "FROM Services s " +
                "INNER JOIN ServiceCategories c ON c.category_id = s.category_id " +
                "WHERE s.status = 'Active' " +
                "ORDER BY c.category_name, s.service_name");
        }

        /// <summary>
        /// True when another service in the SAME category already uses this name.
        /// A name only has to be unique inside its category (two categories may share a name).
        /// excludeServiceId lets an update keep its own name.
        /// </summary>
        public static bool ServiceNameExists(string serviceName, int categoryId, int excludeServiceId)
        {
            if (string.IsNullOrWhiteSpace(serviceName)) return false;

            object count = Db.Scalar(
                "SELECT COUNT(*) FROM Services " +
                "WHERE service_name = @service_name AND category_id = @category_id " +
                "  AND service_id <> @exclude_id",
                Db.P("@service_name", serviceName.Trim()),
                Db.P("@category_id", categoryId),
                Db.P("@exclude_id", excludeServiceId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Inserts a service and returns its new service_id.
        /// staffBusyMinutes 0 means "no separate busy time" and is stored as NULL (empty column).
        /// </summary>
        public static int Add(int categoryId, string serviceName, string description, decimal price,
            int durationMinutes, int defaultTotalSessions, string status,
            int staffBusyMinutes, int sessionIntervalDays, bool requiresAssessment)
        {
            string name = serviceName == null ? string.Empty : serviceName.Trim();

            // The database rule lives here too, so a caller that forgets the check cannot create a duplicate.
            if (ServiceNameExists(name, categoryId, 0))
            {
                throw new InvalidOperationException(
                    "A service with this name already exists in the selected category.");
            }

            object id = Db.Scalar(
                "INSERT INTO Services (category_id, service_name, description, price, " +
                "                      duration_minutes, default_total_sessions, status, " +
                "                      staff_busy_minutes, session_interval_days, requires_assessment) " +
                "VALUES (@category_id, @service_name, @description, @price, " +
                "        @duration_minutes, @default_total_sessions, @status, " +
                "        @staff_busy_minutes, @session_interval_days, @requires_assessment); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@category_id", categoryId),
                Db.P("@service_name", name),
                Db.P("@description", description),
                Db.P("@price", price),
                Db.P("@duration_minutes", durationMinutes),
                Db.P("@default_total_sessions", defaultTotalSessions),
                Db.P("@status", status),
                Db.P("@staff_busy_minutes", BusyValue(staffBusyMinutes)),
                Db.P("@session_interval_days", sessionIntervalDays),
                Db.P("@requires_assessment", requiresAssessment));

            return Convert.ToInt32(id);
        }

        /// <summary>Updates a service. The service_id is not changed.</summary>
        public static void Update(int serviceId, int categoryId, string serviceName, string description,
            decimal price, int durationMinutes, int defaultTotalSessions, string status,
            int staffBusyMinutes, int sessionIntervalDays, bool requiresAssessment)
        {
            string name = serviceName == null ? string.Empty : serviceName.Trim();

            if (ServiceNameExists(name, categoryId, serviceId))
            {
                throw new InvalidOperationException(
                    "A service with this name already exists in the selected category.");
            }

            Db.Execute(
                "UPDATE Services SET category_id = @category_id, service_name = @service_name, " +
                "       description = @description, price = @price, " +
                "       duration_minutes = @duration_minutes, " +
                "       default_total_sessions = @default_total_sessions, status = @status, " +
                "       staff_busy_minutes = @staff_busy_minutes, " +
                "       session_interval_days = @session_interval_days, " +
                "       requires_assessment = @requires_assessment " +
                "WHERE service_id = @service_id",
                Db.P("@category_id", categoryId),
                Db.P("@service_name", name),
                Db.P("@description", description),
                Db.P("@price", price),
                Db.P("@duration_minutes", durationMinutes),
                Db.P("@default_total_sessions", defaultTotalSessions),
                Db.P("@status", status),
                Db.P("@staff_busy_minutes", BusyValue(staffBusyMinutes)),
                Db.P("@session_interval_days", sessionIntervalDays),
                Db.P("@requires_assessment", requiresAssessment),
                Db.P("@service_id", serviceId));
        }

        /// <summary>0 busy minutes means "not filled in", so the column stays NULL.</summary>
        private static object BusyValue(int staffBusyMinutes)
        {
            return staffBusyMinutes > 0 ? (object)staffBusyMinutes : DBNull.Value;
        }

        /// <summary>Archives ('Archived') or restores ('Active') a service.</summary>
        public static void SetStatus(int serviceId, string status)
        {
            Db.Execute(
                "UPDATE Services SET status = @status WHERE service_id = @service_id",
                Db.P("@status", status),
                Db.P("@service_id", serviceId));
        }
    }
}
