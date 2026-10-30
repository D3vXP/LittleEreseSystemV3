using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about appointments lives here: the day list, the booking, the
    /// reschedule, the cancel and the automatic start (AutoStartDue).
    ///
    /// The business rules of PROJECT_RULES.md (opening hours, daily capacity, rooms,
    /// technicians, downpayment and promos) are written down ONLY here, so a screen can never
    /// book something the shop does not allow.
    ///
    /// A booking, a reschedule and a cancel each write several tables, so each of them runs
    /// inside ONE transaction (Db.InTransaction) and can never be left half saved.
    /// </summary>
    internal static class AppointmentRepository
    {
        // ---- The booking rules, in one place ----
        public const int MaxAppointmentsPerDay = 30;         // non-cancelled appointments per day
        public const int MaxMassageAppointmentsPerDay = 10;  // of those, how many may contain a massage
        public const int NoticeDays = 3;                     // cancel / reschedule needs this much notice

        /// <summary>Massage categories are the ones whose name starts with this word.</summary>
        public const string MassageCategoryPrefix = "Massage";

        // Aesthetic services run from 10:00 AM to 8:00 PM, massage from 1:00 PM to 10:00 PM.
        public static readonly TimeSpan AestheticOpening = new TimeSpan(10, 0, 0);
        public static readonly TimeSpan AestheticClosing = new TimeSpan(20, 0, 0);
        public static readonly TimeSpan MassageOpening = new TimeSpan(13, 0, 0);
        public static readonly TimeSpan MassageClosing = new TimeSpan(22, 0, 0);

        // The columns a "lines" table must have (built by AppointmentForm or read back
        // with GetLinesForBooking). Keeping the names here stops a silent typo.
        public const string ColServiceId = "service_id";
        public const string ColServiceName = "service_name";
        public const string ColQuantity = "quantity";
        public const string ColPrice = "price";
        public const string ColDuration = "duration_minutes";
        public const string ColTechnicianId = "technician_id";
        public const string ColPackageId = "package_id";
        public const string ColSessionNumber = "session_number";
        public const string ColTotalSessions = "total_sessions";
        public const string ColRoomId = "room_id";
        public const string ColRoomName = "room_name";
        public const string ColAppointmentServiceId = "appointment_service_id";
        public const string ColStartTime = "start_time";
        public const string ColEndTime = "end_time";
        public const string ColBusyEndTime = "busy_end_time";
        public const string ColAssessedBy = "assessed_by";

        // ---------------------------------------------------------------------
        // READ
        // ---------------------------------------------------------------------

        /// <summary>
        /// The appointments of one day, ready for the grid.
        ///   status - "All" means every status, "Active" means everything except Cancelled and
        ///            Rescheduled (the working list of the day), any other value is one exact status
        ///   search - matches the client name (empty = everybody)
        /// Returns appointment_id, time_range ("10:00 AM - 11:30 AM"), client_name, services
        /// (comma separated, with " x2" for a doubled service), staff_names ("Unassigned" when
        /// nobody is assigned yet), room_names, booking_source, status and payment_status.
        /// WHY the comma lists are built in C#: the app must not depend on a SQL Server version
        /// with STRING_AGG (the same reason the promotions screen builds "Applies to" in C#).
        /// </summary>
        public static DataTable GetByDate(DateTime date, string status, string search)
        {
            string term = search == null ? string.Empty : search.Trim();

            DataTable table = Db.Query(
@"SELECT a.appointment_id,
         a.start_time, a.end_time,
         " + ClientRepository.NameSql("c.") + @" AS client_name,
         a.booking_source,
         a.status,
         ISNULL(b.payment_status, 'Unpaid') AS payment_status
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
LEFT JOIN Billing b ON b.appointment_id = a.appointment_id
WHERE a.appointment_date = @appointment_date
  AND (@status = 'All'
       OR (@status = 'Active' AND a.status NOT IN ('Cancelled', 'Rescheduled'))
       OR (@status NOT IN ('All', 'Active') AND a.status = @status))
  AND (@search = ''
       OR (" + ClientRepository.NameSql("c.") + @") LIKE @like
       OR c.first_name LIKE @like
       OR c.middle_name LIKE @like
       OR c.last_name LIKE @like)
ORDER BY a.start_time, a.appointment_id",
                Db.P("@appointment_date", date.Date),
                Db.P("@status", string.IsNullOrEmpty(status) ? "All" : status),
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"));

            // The three text columns the grid shows but the database does not store.
            table.Columns.Add("time_range", typeof(string));
            table.Columns.Add("services", typeof(string));
            table.Columns.Add("staff_names", typeof(string));
            table.Columns.Add("room_names", typeof(string));

            Dictionary<int, string> services = ChildNames(date,
@"SELECT asv.appointment_id,
         s.service_name + CASE WHEN asv.quantity > 1 THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name,
         ISNULL(asv.total_sessions, 1) AS total_sessions,
         ISNULL(asv.session_number, 0) AS session_number
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE a.appointment_date = @appointment_date AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id");

            Dictionary<int, string> staff = ChildNames(date,
@"SELECT ast.appointment_id, u.full_name AS name
FROM AppointmentStaff ast
INNER JOIN Users u ON u.user_id = ast.user_id
INNER JOIN Appointments a ON a.appointment_id = ast.appointment_id
WHERE a.appointment_date = @appointment_date AND ast.status <> 'Reassigned'
ORDER BY ast.appointment_staff_id");

            Dictionary<int, string> rooms = ChildNames(date,
@"SELECT ar.appointment_id, r.room_name AS name
FROM AppointmentRooms ar
INNER JOIN Rooms r ON r.room_id = ar.room_id
INNER JOIN Appointments a ON a.appointment_id = ar.appointment_id
WHERE a.appointment_date = @appointment_date
ORDER BY ar.appointment_room_id");

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);

                TimeSpan start = ReadTime(row["start_time"]);
                TimeSpan end = ReadTime(row["end_time"]);
                row["time_range"] = Clock(start) + " - " + Clock(end);

                row["services"] = Lookup(services, appointmentId, "-");
                row["staff_names"] = Lookup(staff, appointmentId, "Unassigned");
                row["room_names"] = Lookup(rooms, appointmentId, "-");
            }

            // The raw time columns are not shown anywhere, so they are dropped instead of
            // travelling to the grid as two invisible columns.
            table.Columns.Remove("start_time");
            table.Columns.Remove("end_time");

            return table;
        }

        /// <summary>The per-appointment values of one child query (services, technicians, rooms).</summary>
        private static Dictionary<int, string> ChildNames(DateTime date, string sql)
        {
            return CollectChildNames(Db.Query(sql, Db.P("@appointment_date", date.Date)));
        }

        /// <summary>The same child query but for a range of days (used by the month calendar).</summary>
        private static Dictionary<int, string> ChildNamesForRange(DateTime first, DateTime last, string sql)
        {
            return CollectChildNames(Db.Query(sql, Db.P("@from_date", first.Date), Db.P("@to_date", last.Date)));
        }

        /// <summary>Turns the (appointment_id, name) rows into one comma separated list per appointment.</summary>
        private static Dictionary<int, string> CollectChildNames(DataTable rows)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();

            foreach (DataRow row in rows.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);
                string name = Db.Get<string>(row["name"]);
                if (string.IsNullOrEmpty(name)) continue;

                // A package line also says which session it is ("Tirzepatide (4 sessions) -
                // Session 2 of 4"). Only the services query selects those two columns.
                if (rows.Columns.Contains("total_sessions"))
                {
                    int totalSessions = Db.Get<int>(row["total_sessions"], 1);
                    if (totalSessions > 1)
                    {
                        name = name + " (" + totalSessions + " sessions) - " +
                            SessionText(Db.Get<int>(row["session_number"], 0), totalSessions);
                    }
                }

                string existing;
                // Two services of the same appointment end up as one comma separated list.
                if (values.TryGetValue(appointmentId, out existing))
                {
                    values[appointmentId] = existing + ", " + name;
                }
                else
                {
                    values[appointmentId] = name;
                }
            }

            return values;
        }

        /// <summary>
        /// The appointments of ONE whole month, for the Dashboard calendar.
        /// Returns appointment_id, appointment_date, start_time_text ("10:00 AM"), client_name,
        /// services, staff_names, status and payment_status.
        /// WHY only the working ones: a cancelled or rescheduled visit is not work (see
        /// PROJECT_RULES.md), so the month query uses the same filter as GetByDate's "Active".
        /// The calendar and the "Selected Day" list therefore always agree on a day.
        /// WHY one query for the month: the calendar shows 42 days at once, and asking the
        /// database once per day would mean 31 round trips every time the page is refreshed.
        /// </summary>
        public static DataTable GetMonthAppointments(int year, int month)
        {
            DateTime first = new DateTime(year, month, 1);
            DateTime last = first.AddMonths(1).AddDays(-1);

            DataTable table = Db.Query(
@"SELECT a.appointment_id,
         a.appointment_date,
         a.start_time,
         " + ClientRepository.NameSql("c.") + @" AS client_name,
         a.status,
         ISNULL(b.payment_status, 'Unpaid') AS payment_status
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
LEFT JOIN Billing b ON b.appointment_id = a.appointment_id
WHERE a.appointment_date >= @from_date
  AND a.appointment_date <= @to_date
  AND a.status NOT IN ('Cancelled', 'Rescheduled')
ORDER BY a.appointment_date, a.start_time, a.appointment_id",
                Db.P("@from_date", first),
                Db.P("@to_date", last));

            table.Columns.Add("start_time_text", typeof(string));
            table.Columns.Add("services", typeof(string));
            table.Columns.Add("staff_names", typeof(string));

            Dictionary<int, string> services = ChildNamesForRange(first, last,
@"SELECT asv.appointment_id,
         s.service_name + CASE WHEN asv.quantity > 1 THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name,
         ISNULL(asv.total_sessions, 1) AS total_sessions,
         ISNULL(asv.session_number, 0) AS session_number
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE a.appointment_date >= @from_date AND a.appointment_date <= @to_date
  AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id");

            Dictionary<int, string> staff = ChildNamesForRange(first, last,
@"SELECT ast.appointment_id, u.full_name AS name
FROM AppointmentStaff ast
INNER JOIN Users u ON u.user_id = ast.user_id
INNER JOIN Appointments a ON a.appointment_id = ast.appointment_id
WHERE a.appointment_date >= @from_date AND a.appointment_date <= @to_date
  AND ast.status <> 'Reassigned'
ORDER BY ast.appointment_staff_id");

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);

                row["start_time_text"] = Clock(ReadTime(row["start_time"]));
                row["services"] = Lookup(services, appointmentId, "-");
                row["staff_names"] = Lookup(staff, appointmentId, "Unassigned");
            }

            // The raw time column is only used to build the readable one above.
            table.Columns.Remove("start_time");

            return table;
        }

        private static string Lookup(Dictionary<int, string> values, int appointmentId, string fallback)
        {
            string found;
            if (values.TryGetValue(appointmentId, out found) && !string.IsNullOrWhiteSpace(found)) return found;
            return fallback;
        }

        /// <summary>
        /// The service lines of one appointment for the details card:
        /// service_name, quantity, price, staff_name ("Unassigned"), status and duration_minutes
        /// (the extra duration is used to show the end time when an appointment is rescheduled).
        /// </summary>
        public static DataTable GetServiceLines(int appointmentId)
        {
            return Db.Query(
@"SELECT s.service_name, asv.quantity, asv.price,
         ISNULL(u.full_name, 'Unassigned') AS staff_name,
         asv.status, s.duration_minutes,
         LEFT(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.start_time, a.start_time), 100), 7)),
              LEN(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.start_time, a.start_time), 100), 7))) - 2)
             + ' ' +
         RIGHT(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.start_time, a.start_time), 100), 7)), 2)
             AS start_time,
         LEFT(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.end_time, a.end_time), 100), 7)),
              LEN(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.end_time, a.end_time), 100), 7))) - 2)
             + ' ' +
         RIGHT(LTRIM(RIGHT(CONVERT(varchar(20), ISNULL(asv.end_time, a.end_time), 100), 7)), 2)
             AS end_time,
         'Session ' + CONVERT(varchar(10), ISNULL(asv.session_number, 1)) + ' of ' +
             CONVERT(varchar(10), ISNULL(asv.total_sessions, 1)) AS session_text
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
LEFT JOIN AppointmentStaff ast ON ast.appointment_service_id = asv.appointment_service_id
                               AND ast.status <> 'Reassigned'
LEFT JOIN Users u ON u.user_id = ast.user_id
WHERE asv.appointment_id = @appointment_id
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));
        }

        /// <summary>One appointment row (date, times, source, status, notes), or null when it is gone.</summary>
        public static DataRow GetById(int appointmentId)
        {
            DataTable table = Db.Query(
@"SELECT a.appointment_id, a.client_id, a.user_id, a.appointment_date, a.start_time, a.end_time,
         a.booking_source, a.status, a.notes, a.cancelled_at, a.created_at, a.updated_at,
         " + ClientRepository.NameSql("c.") + @" AS client_name
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
WHERE a.appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// The 30-minute start times a booking may use: 10:00 AM to 9:30 PM (24 slots).
        /// WHY a fixed list: the shop only starts a service on the half hour.
        /// </summary>
        public static List<TimeSpan> GetTimeSlots()
        {
            List<TimeSpan> slots = new List<TimeSpan>();

            TimeSpan slot = AestheticOpening;
            TimeSpan lastStart = new TimeSpan(21, 30, 0);

            while (slot <= lastStart)
            {
                slots.Add(slot);
                slot = slot.Add(new TimeSpan(0, 30, 0));
            }

            return slots;
        }

        /// <summary>The rooms of the shop (room_id, room_name, capacity, status).</summary>
        public static DataTable GetRooms()
        {
            return Db.Query("SELECT room_id, room_name, capacity, status FROM Rooms ORDER BY room_name");
        }

        /// <summary>
        /// How busy one day is: the appointments that are not cancelled, and of those how many
        /// contain a massage. Used by the toolbar strip of the Appointments page
        /// ("4 appointments - 1 massage").
        /// </summary>
        public static void GetDayCounts(DateTime date, out int appointmentCount, out int massageCount)
        {
            appointmentCount = CountAppointmentsThatDay(null, null, date, 0);
            massageCount = CountMassageThatDay(null, null, date, 0);
        }

        /// <summary>
        /// The rooms that may host a service of this category (linked in RoomServiceCategories),
        /// ordered the way the automatic assignment tries them.
        /// </summary>
        public static DataTable GetRoomsForCategory(int categoryId)
        {
            return GetRoomsForCategory(null, null, categoryId);
        }

        /// <summary>
        /// The lines of an appointment in the same shape Create expects, so a reschedule can
        /// rebuild the rooms and check the rules again without asking the user to re-enter anything.
        /// </summary>
        public static DataTable GetLinesForBooking(int appointmentId)
        {
            return Db.Query(
@"SELECT asv.appointment_service_id, asv.service_id, s.service_name, asv.quantity, asv.price,
         s.duration_minutes, s.default_total_sessions, ISNULL(ast.user_id, 0) AS technician_id,
         ISNULL(asv.package_id, 0) AS package_id,
         ISNULL(asv.session_number, 0) AS session_number,
         ISNULL(asv.total_sessions, 0) AS total_sessions,
         ISNULL(asv.start_time, a.start_time) AS start_time,
         ISNULL(asv.end_time, a.end_time) AS end_time,
         ISNULL(asv.busy_end_time, ISNULL(asv.end_time, a.end_time)) AS busy_end_time,
         ISNULL(asv.assessed_by, 0) AS assessed_by,
         ISNULL(s.staff_busy_minutes, 0) AS staff_busy_minutes
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
LEFT JOIN AppointmentStaff ast ON ast.appointment_service_id = asv.appointment_service_id
                               AND ast.status <> 'Reassigned'
WHERE asv.appointment_id = @appointment_id AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));
        }

        /// <summary>
        /// The later sessions of the package the given appointment belongs to, ready for the
        /// "cancel / reschedule together" screens. The package line is the first
        /// AppointmentServices row of the appointment with package_id > 0 and total_sessions > 1;
        /// when the appointment is not a package session, an EMPTY table with the same columns
        /// comes back, so a caller can walk the rows without checking anything else.
        /// Columns: appointment_id, session_number, total_sessions, service_name, appointment_date,
        /// start_time (the earliest line start of that appointment, Appointments.start_time when
        /// the line has none). Rows: the same package_id, a higher session_number than the
        /// appointment's own line, appointment status 'Upcoming' and line status not 'Cancelled'.
        /// </summary>
        public static DataTable GetLaterSessions(int appointmentId)
        {
            DataTable result = BuildLaterSessionsTable();

            DataTable line = Db.Query(
@"SELECT TOP 1 asv.package_id, asv.session_number
FROM AppointmentServices asv
WHERE asv.appointment_id = @appointment_id
  AND ISNULL(asv.package_id, 0) > 0 AND ISNULL(asv.total_sessions, 0) > 1
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));

            if (line.Rows.Count == 0) return result;

            int packageId = Db.Get<int>(line.Rows[0]["package_id"]);
            int sessionNumber = Db.Get<int>(line.Rows[0]["session_number"]);

            DataTable rows = Db.Query(
@"SELECT a.appointment_id, asv.session_number, asv.total_sessions, s.service_name,
         a.appointment_date,
         ISNULL((SELECT MIN(ISNULL(every.start_time, a.start_time))
                 FROM AppointmentServices every
                 WHERE every.appointment_id = a.appointment_id
                   AND every.status <> 'Cancelled'), a.start_time) AS start_time
FROM AppointmentServices asv
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
INNER JOIN Services s ON s.service_id = asv.service_id
WHERE asv.package_id = @package_id
  AND asv.appointment_id <> @appointment_id
  AND ISNULL(asv.session_number, 0) > @session_number
  AND a.status = 'Upcoming'
  AND asv.status <> 'Cancelled'
ORDER BY asv.session_number",
                Db.P("@package_id", packageId),
                Db.P("@appointment_id", appointmentId),
                Db.P("@session_number", sessionNumber));

            foreach (DataRow row in rows.Rows)
            {
                result.Rows.Add(
                    Db.Get<int>(row["appointment_id"]),
                    Db.Get<int>(row["session_number"]),
                    Db.Get<int>(row["total_sessions"]),
                    Db.Get<string>(row["service_name"]),
                    Db.Get<DateTime>(row["appointment_date"]).Date,
                    ReadTime(row["start_time"]));
            }

            return result;
        }

        // ---------------------------------------------------------------------
        // PACKAGE AND MONEY HELPERS (shared by the form and by the save methods)
        // ---------------------------------------------------------------------

        /// <summary>
        /// The package this booking will use, so the form and the save agree on the price and on
        /// the "Session 2 of 3" label.
        /// Returns false when the service is not a package (default_total_sessions is 1), which
        /// means "charge the normal price and set no session numbers".
        /// </summary>
        public static bool TryGetPackagePlan(int clientId, int serviceId,
            out int packageId, out int sessionNumber, out int totalSessions, out bool isNewPackage)
        {
            packageId = 0;
            sessionNumber = 0;
            totalSessions = 0;
            isNewPackage = false;

            DataRow service = GetServiceInfo(null, null, serviceId);
            if (service == null) return false;

            int defaultSessions = Db.Get<int>(service["default_total_sessions"], 1);
            if (defaultSessions <= 1) return false;

            DataRow existing = FindActivePackage(null, null, clientId, serviceId);

            if (existing == null)
            {
                // The first session of a brand-new package carries the whole package price.
                isNewPackage = true;
                sessionNumber = 1;
                totalSessions = defaultSessions;
                return true;
            }

            packageId = Db.Get<int>(existing["package_id"]);
            totalSessions = Db.Get<int>(existing["total_sessions"], defaultSessions);
            // The number comes from the sessions that are not cancelled, not from the count of
            // completed ones, so a cancelled session can be booked again.
            sessionNumber = AvailabilityRepository.NextSessionNumber(null, null, packageId, totalSessions);
            return true;
        }

        /// <summary>The total duration of a booking: every line's duration times its quantity.</summary>
        public static TimeSpan TotalDuration(DataTable lines)
        {
            int minutes = 0;

            if (lines != null)
            {
                foreach (DataRow row in lines.Rows)
                {
                    int duration = Db.Get<int>(row[ColDuration]);
                    int quantity = Db.Get<int>(row[ColQuantity], 1);
                    if (quantity < 1) quantity = 1;
                    minutes += duration * quantity;
                }
            }

            return new TimeSpan(0, minutes, 0);
        }

        /// <summary>
        /// The label of a package session ("Session 2 of 4"). A missing number (a single service)
        /// reads "Session 1 of 1", so a label is never empty.
        /// </summary>
        public static string SessionText(int sessionNumber, int totalSessions)
        {
            if (sessionNumber <= 0 || totalSessions <= 0) return "Session 1 of 1";

            return "Session " + sessionNumber + " of " + totalSessions;
        }

        /// <summary>The subtotal of a booking: every line's price times its quantity.</summary>
        public static decimal Subtotal(DataTable lines)
        {
            decimal subtotal = 0m;

            if (lines != null)
            {
                foreach (DataRow row in lines.Rows)
                {
                    int quantity = Db.Get<int>(row[ColQuantity], 1);
                    if (quantity < 1) quantity = 1;
                    subtotal += Db.Get<decimal>(row[ColPrice]) * quantity;
                }
            }

            return subtotal;
        }

        /// <summary>
        /// The down payment the front desk takes in Billing right after a booking:
        ///   - a promotion needs the FULL total, because promos are paid in full upfront
        ///   - otherwise EVERY booking needs 50% of the total, a walk-in included
        /// The booking dialog takes no money any more, so nothing else asks for this amount.
        /// </summary>
        public static decimal DownpaymentDue(decimal subtotal, decimal discount)
        {
            decimal total = subtotal - discount;
            if (total < 0m) total = 0m;

            // WHY the promotion is checked first: a promo is never an installment, so the whole
            // total is due (PROJECT_RULES, "Promos").
            if (discount > 0m) return total;

            decimal half = Math.Round(total / 2m, 2, MidpointRounding.AwayFromZero);
            return half;
        }

        /// <summary>
        /// True when the appointment is less than 3 days away, so only an Admin may cancel or
        /// reschedule it and must type a reason. daysAway is the whole number of days left.
        /// </summary>
        public static bool IsInsideNoticeWindow(DateTime appointmentDate, out int daysAway)
        {
            TimeSpan left = appointmentDate.Date - DateTime.Today;
            daysAway = (int)left.TotalDays;
            return daysAway < NoticeDays;
        }

        // ---------------------------------------------------------------------
        // VALIDATION
        // ---------------------------------------------------------------------

        /// <summary>
        /// A COPY of the lines table where every line carries its own start, end and busy end
        /// (ColStartTime, ColEndTime, ColBusyEndTime) plus the nurse column (ColAssessedBy).
        /// WHY a copy: the caller's table stays untouched. A line with no start of its own is placed
        /// right after the previous line (the first one at firstStart), which is exactly what the
        /// old whole-appointment booking did, so an old caller keeps working.
        /// </summary>
        public static DataTable PrepareLines(SqlConnection connection, SqlTransaction transaction,
            DataTable lines, TimeSpan firstStart)
        {
            if (lines == null) return new DataTable();

            DataTable prepared = lines.Copy();

            if (!prepared.Columns.Contains(ColStartTime)) prepared.Columns.Add(ColStartTime, typeof(TimeSpan));
            if (!prepared.Columns.Contains(ColEndTime)) prepared.Columns.Add(ColEndTime, typeof(TimeSpan));
            if (!prepared.Columns.Contains(ColBusyEndTime)) prepared.Columns.Add(ColBusyEndTime, typeof(TimeSpan));
            if (!prepared.Columns.Contains(ColAssessedBy)) prepared.Columns.Add(ColAssessedBy, typeof(int));

            TimeSpan nextStart = firstStart;

            foreach (DataRow row in prepared.Rows)
            {
                // A line without a time of its own follows the line above it.
                TimeSpan lineStart = ReadTime(row[ColStartTime]);
                if (lineStart == TimeSpan.Zero) lineStart = nextStart;

                int serviceId = Db.Get<int>(row[ColServiceId]);
                int quantity = Db.Get<int>(row[ColQuantity], 1);
                if (quantity < 1) quantity = 1;

                DataRow service = GetServiceInfo(connection, transaction, serviceId);

                int duration = 0;
                int staffBusyMinutes = 0;

                if (service != null)
                {
                    duration = Db.Get<int>(service["duration_minutes"]);
                    staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);
                }
                else if (prepared.Columns.Contains(ColDuration))
                {
                    duration = Db.Get<int>(row[ColDuration]);
                }

                row[ColStartTime] = lineStart;
                row[ColEndTime] = AvailabilityRepository.LineEnd(lineStart, duration, quantity);
                row[ColBusyEndTime] = AvailabilityRepository.LineBusyEnd(lineStart, duration,
                    staffBusyMinutes, quantity);

                nextStart = ReadTime(row[ColEndTime]);
            }

            return prepared;
        }

        /// <summary>
        /// Checks a booking against the business rules and returns the problems it found
        /// (an empty list means the booking is fine).
        ///   appointmentId - 0 for a new booking, or the id being rescheduled, so the appointment
        ///                   does not count itself in the daily capacity or against the rooms
        ///   lines         - service_id, quantity, price, duration_minutes, technician_id per line
        /// The booking dialog takes no money: the 50% down payment is taken in Billing afterwards.
        /// The safety rules (a signed consent form and the nurse of an invasive service) are only
        /// checked for a NEW booking, because a reschedule changes no client and no nurse.
        /// Every message is a sentence a staff member can understand.
        /// </summary>
        public static List<string> ValidateBooking(int appointmentId, int clientId, DateTime appointmentDate,
            TimeSpan startTime, string bookingSource, DataTable lines, int promoId)
        {
            List<string> problems = new List<string>();

            if (clientId <= 0) problems.Add("Choose the client first.");

            if (lines == null || lines.Rows.Count == 0)
            {
                problems.Add("Add at least one service to the appointment.");
                return problems;   // nothing else can be checked without services
            }

            TimeSpan duration = TotalDuration(lines);

            if (duration <= TimeSpan.Zero)
            {
                problems.Add("The selected services have no duration, so the end time cannot be worked out.");
            }

            // The date may not be in the past. Today only works while the start time is ahead.
            if (appointmentDate.Date < DateTime.Today)
            {
                problems.Add("The appointment date is already in the past.");
            }
            else if (appointmentDate.Date == DateTime.Today && startTime <= DateTime.Now.TimeOfDay)
            {
                problems.Add("For today the start time must still be ahead (" +
                    DateTime.Today.Add(DateTime.Now.TimeOfDay).ToString("h:mm tt") + " now).");
            }

            // Every line is checked in ONE place: AvailabilityRepository knows the opening hours,
            // the room and the technician rules (a massage at 1:00 PM and a facial at 10:00 AM fit
            // in the same booking, because each line has its own time).
            bool hasMassage = false;

            DataTable prepared = PrepareLines(null, null, lines, startTime);

            foreach (DataRow row in prepared.Rows)
            {
                DataRow service = GetServiceInfo(null, null, Db.Get<int>(row[ColServiceId]));
                if (service != null && IsMassageCategory(Db.Get<string>(service["category_name"])))
                {
                    hasMassage = true;
                }

                // The other lines of the same booking are passed without this row, so a line is
                // never reported as overlapping itself.
                DataTable otherLines = prepared.Copy();
                otherLines.Rows.Remove(otherLines.Rows[prepared.Rows.IndexOf(row)]);

                string answer = AvailabilityRepository.WhyNotPossible(null, null, appointmentDate,
                    Db.Get<int>(row[ColServiceId]), Db.Get<int>(row[ColQuantity], 1),
                    ReadTime(row[ColStartTime]), Db.Get<int>(row[ColTechnicianId]), clientId,
                    appointmentId, otherLines);

                if (!string.IsNullOrEmpty(answer))
                {
                    string name = prepared.Columns.Contains(ColServiceName)
                        ? Db.Get<string>(row[ColServiceName])
                        : "Service #" + Db.Get<int>(row[ColServiceId]);

                    problems.Add(name + " at " + Clock(ReadTime(row[ColStartTime])) + ": " + answer);
                }
            }

            // A reschedule moves one existing appointment; the later sessions of a package already
            // exist as their own appointments, so they are not planned again.
            if (appointmentId == 0)
            {
                // A package is booked as weekly sessions, so every session AFTER the first one must
                // fit as well. When one of them cannot be placed the whole booking is refused.
                // The session number is the same one TryGetPackagePlan gives (1 for a new package).
                foreach (DataRow row in prepared.Rows)
                {
                    int serviceId = Db.Get<int>(row[ColServiceId]);

                    int packageId;
                    int sessionNumber;
                    int totalSessions;
                    bool isNewPackage;

                    if (!TryGetPackagePlan(clientId, serviceId,
                        out packageId, out sessionNumber, out totalSessions, out isNewPackage))
                    {
                        continue;   // not a package: default_total_sessions is 1
                    }

                    if (sessionNumber <= 0 || totalSessions <= 1) continue;

                    DataRow service = GetServiceInfo(null, null, serviceId);
                    string serviceName = service == null
                        ? "Service #" + serviceId
                        : Db.Get<string>(service["service_name"]);

                    DataTable plan = AvailabilityRepository.PlanSessions(null, null, clientId, serviceId,
                        appointmentDate, ReadTime(row[ColStartTime]), sessionNumber, totalSessions, appointmentId);

                    foreach (DataRow session in plan.Rows)
                    {
                        if (Db.Get<bool>(session["ok"])) continue;

                        problems.Add(serviceName + " session " + Db.Get<int>(session["session_number"]) +
                            " on " + Db.Get<DateTime>(session["session_date"]).ToString("MMM dd, yyyy") +
                            ": " + Db.Get<string>(session["problem"]));
                    }
                }
            }

            // Daily capacity: at most 30 appointments a day, of which at most 10 contain a massage.
            int bookedThatDay = CountAppointmentsThatDay(null, null, appointmentDate, appointmentId);
            if (bookedThatDay >= MaxAppointmentsPerDay)
            {
                problems.Add("This day already has " + MaxAppointmentsPerDay +
                    " appointments (the daily limit). Choose another date.");
            }

            if (hasMassage && CountMassageThatDay(null, null, appointmentDate, appointmentId) >= MaxMassageAppointmentsPerDay)
            {
                problems.Add("This day already has " + MaxMassageAppointmentsPerDay +
                    " massage appointments (the daily limit). Choose another date.");
            }

            // Promotion: it must be valid today AND apply to at least one service of this booking.
            if (promoId > 0)
            {
                string promoMessage;
                PromotionRepository.CalculateDiscount(promoId, BuildPromoLines(lines), out promoMessage);

                if (!string.IsNullOrEmpty(promoMessage))
                {
                    problems.Add(promoMessage);
                }
            }

            // Safety, checked for a NEW booking only: the signed consent form must be on file and
            // an invasive service needs the nurse who assessed the client. A reschedule changes
            // neither the client nor the nurse, so it must not ask for them again.
            if (appointmentId == 0)
            {
                if (clientId > 0)
                {
                    DataRow client = ClientRepository.GetById(clientId);
                    int consent = client == null ? 0 : Db.Get<int>(client["consent_status"]);

                    if (consent != 1)
                    {
                        problems.Add("This client has not signed the consent form yet. Open Clients, " +
                            "tick that the signed form is on file, then book again.");
                    }
                }

                DataTable nurses = UserRepository.GetActiveNurses();

                // WHY the "at least one nurse" condition: a shop with no nurse on the list cannot
                // name one, so the booking goes through instead of being blocked forever.
                if (nurses.Rows.Count > 0)
                {
                    HashSet<int> nurseIds = new HashSet<int>();
                    foreach (DataRow nurse in nurses.Rows)
                    {
                        nurseIds.Add(Db.Get<int>(nurse["user_id"]));
                    }

                    List<string> alreadyReported = new List<string>();

                    foreach (DataRow row in prepared.Rows)
                    {
                        DataRow service = GetServiceInfo(null, null, Db.Get<int>(row[ColServiceId]));
                        if (service == null || !Db.Get<bool>(service["requires_assessment"])) continue;

                        if (nurseIds.Contains(Db.Get<int>(row[ColAssessedBy]))) continue;

                        string serviceName = Db.Get<string>(service["service_name"]);
                        if (alreadyReported.Contains(serviceName)) continue;

                        alreadyReported.Add(serviceName);
                        problems.Add("Choose the nurse who assessed the client for " + serviceName + ".");
                    }
                }
            }

            return problems;
        }

        // ---------------------------------------------------------------------
        // WRITE
        // ---------------------------------------------------------------------

        /// <summary>
        /// Books an appointment and everything that belongs to it in ONE transaction:
        /// the appointment, its service lines (with the package rules applied), the technicians,
        /// the rooms, the bill, the history row and the activity log entry.
        /// The bill starts unpaid with no down payment on it: the 50% is taken in Billing.
        /// Returns the new appointment_id.
        /// The rules are checked again here (through ValidateBooking by the caller) but the prices
        /// are always read from the database, so a changed service price can never be bypassed.
        /// </summary>
        public static int Create(int clientId, int userId, DateTime appointmentDate, TimeSpan startTime,
            string bookingSource, string notes, DataTable lines, int promoId)
        {
            if (lines == null || lines.Rows.Count == 0)
            {
                throw new InvalidOperationException("An appointment needs at least one service.");
            }

            int newAppointmentId = 0;

            // How many weekly sessions of a package were created after the first one, so the
            // activity log can say it (the transaction cannot return a value).
            int extraSessions = 0;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                // 1. The prices, worked out from the database (package rule).
                DataTable resolved = ResolveLinePrices(connection, transaction, clientId, lines);

                // 2. Every line carries its own time. A line without a start follows the line above.
                DataTable prepared = PrepareLines(connection, transaction, resolved, startTime);

                // The appointment keeps the earliest line start and the latest line end.
                TimeSpan firstLineStart = startTime;
                TimeSpan lastLineEnd = startTime;
                bool firstLine = true;

                foreach (DataRow row in prepared.Rows)
                {
                    TimeSpan lineStart = ReadTime(row[ColStartTime]);
                    TimeSpan lineEnd = ReadTime(row[ColEndTime]);

                    if (firstLine || lineStart < firstLineStart) firstLineStart = lineStart;
                    if (firstLine || lineEnd > lastLineEnd) lastLineEnd = lineEnd;
                    firstLine = false;
                }

                // 3. The appointment itself.
                object id = Db.Scalar(connection, transaction,
@"INSERT INTO Appointments (client_id, user_id, appointment_date, start_time, end_time,
                           booking_source, status, notes)
VALUES (@client_id, @user_id, @appointment_date, CAST(@start_time AS time), CAST(@end_time AS time),
        @booking_source, 'Upcoming', @notes);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                    Db.P("@client_id", clientId),
                    Db.P("@user_id", userId),
                    Db.P("@appointment_date", appointmentDate.Date),
                    Db.P("@start_time", SqlTime(firstLineStart)),
                    Db.P("@end_time", SqlTime(lastLineEnd)),
                    Db.P("@booking_source", bookingSource),
                    Db.P("@notes", string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()));

                newAppointmentId = Convert.ToInt32(id);

                // 4. One row per service line, plus its technician and its room.
                decimal subtotal = 0m;

                foreach (DataRow row in prepared.Rows)
                {
                    int serviceId = Db.Get<int>(row[ColServiceId]);
                    int quantity = Db.Get<int>(row[ColQuantity], 1);
                    if (quantity < 1) quantity = 1;

                    decimal price = Db.Get<decimal>(row[ColPrice]);
                    int packageId = Db.Get<int>(row[ColPackageId]);
                    int sessionNumber = Db.Get<int>(row[ColSessionNumber]);
                    int totalSessions = Db.Get<int>(row[ColTotalSessions]);
                    int technicianId = Db.Get<int>(row[ColTechnicianId]);

                    TimeSpan lineStart = ReadTime(row[ColStartTime]);
                    TimeSpan lineEnd = ReadTime(row[ColEndTime]);
                    TimeSpan busyEnd = ReadTime(row[ColBusyEndTime]);
                    int assessedBy = Db.Get<int>(row[ColAssessedBy]);

                    // Nobody was chosen for this line, so the first free technician of the
                    // category takes it (0 leaves the line unassigned).
                    if (technicianId <= 0)
                    {
                        technicianId = AvailabilityRepository.PickTechnician(connection, transaction,
                            appointmentDate, serviceId, quantity, lineStart, 0);
                    }

                    // The room is chosen for this line's own time window. A room of 0 inserts
                    // no AppointmentRooms row, exactly as before.
                    int categoryId = 0;
                    DataRow serviceInfo = GetServiceInfo(connection, transaction, serviceId);
                    if (serviceInfo != null) categoryId = Db.Get<int>(serviceInfo["category_id"]);

                    string roomName;
                    int roomId = AvailabilityRepository.PickRoom(connection, transaction, appointmentDate,
                        categoryId, lineStart, lineEnd, 0, out roomName);

                    subtotal += price * quantity;

                    object lineId = Db.Scalar(connection, transaction,
@"INSERT INTO AppointmentServices (appointment_id, service_id, package_id, quantity, price,
                                  session_number, total_sessions, status,
                                  start_time, end_time, busy_end_time, assessed_by)
VALUES (@appointment_id, @service_id, @package_id, @quantity, @price,
        @session_number, @total_sessions, 'Pending',
        CAST(@start_time AS time), CAST(@end_time AS time), CAST(@busy_end_time AS time), @assessed_by);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                        Db.P("@appointment_id", newAppointmentId),
                        Db.P("@service_id", serviceId),
                        Db.P("@package_id", packageId > 0 ? (object)packageId : null),
                        Db.P("@quantity", quantity),
                        Db.P("@price", price),
                        Db.P("@session_number", sessionNumber > 0 ? (object)sessionNumber : null),
                        Db.P("@total_sessions", totalSessions > 0 ? (object)totalSessions : null),
                        Db.P("@start_time", SqlTime(lineStart)),
                        Db.P("@end_time", SqlTime(lineEnd)),
                        Db.P("@busy_end_time", SqlTime(busyEnd)),
                        Db.P("@assessed_by", assessedBy > 0 ? (object)assessedBy : null));

                    int appointmentServiceId = Convert.ToInt32(lineId);

                    if (technicianId > 0)
                    {
                        Db.Execute(connection, transaction,
@"INSERT INTO AppointmentStaff (appointment_id, appointment_service_id, user_id, status)
VALUES (@appointment_id, @appointment_service_id, @user_id, 'Assigned')",
                            Db.P("@appointment_id", newAppointmentId),
                            Db.P("@appointment_service_id", appointmentServiceId),
                            Db.P("@user_id", technicianId));
                    }

                    if (roomId > 0)
                    {
                        Db.Execute(connection, transaction,
@"INSERT INTO AppointmentRooms (appointment_id, appointment_service_id, room_id, status)
VALUES (@appointment_id, @appointment_service_id, @room_id, 'Reserved')",
                            Db.P("@appointment_id", newAppointmentId),
                            Db.P("@appointment_service_id", appointmentServiceId),
                            Db.P("@room_id", roomId));
                    }
                }

                // 5. The discount always comes from the one promotion formula.
                decimal discount = 0m;
                if (promoId > 0)
                {
                    string promoMessage;
                    discount = PromotionRepository.CalculateDiscount(promoId, BuildPromoLines(prepared), out promoMessage);
                    if (!string.IsNullOrEmpty(promoMessage)) discount = 0m;
                }

                decimal total = subtotal - discount;
                if (total < 0m) total = 0m;

                // 6. The bill starts unpaid with nothing on it: the 50% down payment (or the whole
                // total of a promotion) is taken in Billing right after the booking, so no
                // Payments row is written here.
                BillingRepository.CreateBilling(connection, transaction, newAppointmentId,
                    subtotal, promoId, discount, total, 0m, "None", total, "Unpaid");

                // 7. History: the appointment was created.
                Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Created', 'Upcoming', @reason)",
                    Db.P("@appointment_id", newAppointmentId),
                    Db.P("@changed_by", userId),
                    Db.P("@reason", "Booked as a " + bookingSource + " appointment."));

                // 8. A package is booked as weekly sessions: every session after the first one
                // becomes its own appointment, in the SAME transaction, so the remaining weeks
                // are already scheduled. When a session cannot be placed the whole booking rolls
                // back (the exception leaves this delegate and Db.InTransaction rolls back).
                foreach (DataRow row in prepared.Rows)
                {
                    int serviceId = Db.Get<int>(row[ColServiceId]);
                    int packageId = Db.Get<int>(row[ColPackageId]);
                    int sessionNumber = Db.Get<int>(row[ColSessionNumber]);
                    int totalSessions = Db.Get<int>(row[ColTotalSessions]);
                    int assessedBy = Db.Get<int>(row[ColAssessedBy]);

                    if (packageId <= 0 || sessionNumber <= 0 || totalSessions <= 1) continue;

                    TimeSpan lineStart = ReadTime(row[ColStartTime]);

                    DataRow service = GetServiceInfo(connection, transaction, serviceId);
                    if (service == null)
                    {
                        throw new InvalidOperationException(
                            "One of the package services no longer exists. Add it again.");
                    }

                    string serviceName = Db.Get<string>(service["service_name"]);
                    int duration = Db.Get<int>(service["duration_minutes"]);
                    int staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);
                    int categoryId = Db.Get<int>(service["category_id"]);

                    DataTable plan = AvailabilityRepository.PlanSessions(connection, transaction, clientId,
                        serviceId, appointmentDate, lineStart, sessionNumber, totalSessions, newAppointmentId);

                    foreach (DataRow session in plan.Rows)
                    {
                        if (!Db.Get<bool>(session["ok"]))
                        {
                            throw new InvalidOperationException(Db.Get<string>(session["problem"]));
                        }

                        DateTime sessionDate = Db.Get<DateTime>(session["session_date"]).Date;
                        int sessionNo = Db.Get<int>(session["session_number"]);

                        TimeSpan sessionEnd = AvailabilityRepository.LineEnd(lineStart, duration, 1);
                        TimeSpan sessionBusyEnd = AvailabilityRepository.LineBusyEnd(lineStart, duration,
                            staffBusyMinutes, 1);

                        // The technician and the room of that week are chosen for its own date.
                        int sessionTechnicianId = AvailabilityRepository.PickTechnician(connection,
                            transaction, sessionDate, serviceId, 1, lineStart, 0);

                        string sessionRoomName;
                        int sessionRoomId = AvailabilityRepository.PickRoom(connection, transaction,
                            sessionDate, categoryId, lineStart, sessionEnd, 0, out sessionRoomName);

                        object sessionAppointment = Db.Scalar(connection, transaction,
@"INSERT INTO Appointments (client_id, user_id, appointment_date, start_time, end_time,
                           booking_source, status, notes)
VALUES (@client_id, @user_id, @appointment_date, CAST(@start_time AS time), CAST(@end_time AS time),
        @booking_source, 'Upcoming', NULL);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                            Db.P("@client_id", clientId),
                            Db.P("@user_id", userId),
                            Db.P("@appointment_date", sessionDate),
                            Db.P("@start_time", SqlTime(lineStart)),
                            Db.P("@end_time", SqlTime(sessionEnd)),
                            Db.P("@booking_source", bookingSource));

                        int sessionAppointmentId = Convert.ToInt32(sessionAppointment);

                        object sessionLine = Db.Scalar(connection, transaction,
@"INSERT INTO AppointmentServices (appointment_id, service_id, package_id, quantity, price,
                                  session_number, total_sessions, status,
                                  start_time, end_time, busy_end_time, assessed_by)
VALUES (@appointment_id, @service_id, @package_id, 1, 0.00,
        @session_number, @total_sessions, 'Pending',
        CAST(@start_time AS time), CAST(@end_time AS time), CAST(@busy_end_time AS time), @assessed_by);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                            Db.P("@appointment_id", sessionAppointmentId),
                            Db.P("@service_id", serviceId),
                            Db.P("@package_id", packageId),
                            Db.P("@session_number", sessionNo),
                            Db.P("@total_sessions", totalSessions),
                            Db.P("@start_time", SqlTime(lineStart)),
                            Db.P("@end_time", SqlTime(sessionEnd)),
                            Db.P("@busy_end_time", SqlTime(sessionBusyEnd)),
                            Db.P("@assessed_by", assessedBy > 0 ? (object)assessedBy : null));

                        int sessionLineId = Convert.ToInt32(sessionLine);

                        if (sessionTechnicianId > 0)
                        {
                            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentStaff (appointment_id, appointment_service_id, user_id, status)
VALUES (@appointment_id, @appointment_service_id, @user_id, 'Assigned')",
                                Db.P("@appointment_id", sessionAppointmentId),
                                Db.P("@appointment_service_id", sessionLineId),
                                Db.P("@user_id", sessionTechnicianId));
                        }

                        if (sessionRoomId > 0)
                        {
                            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentRooms (appointment_id, appointment_service_id, room_id, status)
VALUES (@appointment_id, @appointment_service_id, @room_id, 'Reserved')",
                                Db.P("@appointment_id", sessionAppointmentId),
                                Db.P("@appointment_service_id", sessionLineId),
                                Db.P("@room_id", sessionRoomId));
                        }

                        // A later session costs nothing: the whole package price sits on session 1.
                        BillingRepository.CreateBilling(connection, transaction, sessionAppointmentId,
                            0m, 0, 0m, 0m, 0m, "None", 0m, "Unpaid");

                        Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Created', 'Upcoming', @reason)",
                            Db.P("@appointment_id", sessionAppointmentId),
                            Db.P("@changed_by", userId),
                            Db.P("@reason", "Session " + sessionNo + " of " + totalSessions + " of " +
                                serviceName + ", booked together with appointment #" + newAppointmentId + "."));

                        extraSessions++;
                    }
                }
            });

            ActivityRepository.Log(userId, "Book Appointment", "Appointments",
                "Appointment #" + newAppointmentId + " for client #" + clientId + " (" + bookingSource + ")" +
                (extraSessions > 0 ? ", plus " + extraSessions + " package session(s)." : "."));

            return newAppointmentId;
        }

        /// <summary>
        /// Moves an appointment to another date and time. The rules are checked again (the
        /// appointment does not count itself), rooms are re-assigned and a history row records the
        /// old date, the old time and the reason. The appointment stays 'Upcoming' and the
        /// downpayment stays 'Held'.
        /// Throws InvalidOperationException with a readable message when the move is not allowed.
        /// </summary>
        public static void Reschedule(int appointmentId, DateTime newDate, TimeSpan newStart, string reason)
        {
            DataRow appointment = GetById(appointmentId);
            if (appointment == null) throw new InvalidOperationException("This appointment no longer exists.");

            string currentStatus = Db.Get<string>(appointment["status"]);
            if (!string.Equals(currentStatus, "Upcoming", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only an upcoming appointment can be rescheduled.");
            }

            DateTime oldDate = Db.Get<DateTime>(appointment["appointment_date"]);
            TimeSpan oldStart = ReadTime(appointment["start_time"]);
            string oldText = oldDate.ToString("MMM dd, yyyy") + " " + Clock(oldStart);

            // The 3-day rule: inside the notice window only an Admin may move it, with a reason.
            int daysAway;
            if (IsInsideNoticeWindow(oldDate, out daysAway))
            {
                if (!Helpers.Session.IsAdmin)
                {
                    throw new InvalidOperationException(
                        "An appointment can only be rescheduled at least " + NoticeDays +
                        " days before its date. This one is in " + Math.Max(daysAway, 0) +
                        " day(s). Ask an Admin.");
                }

                if (string.IsNullOrWhiteSpace(reason))
                {
                    throw new InvalidOperationException(
                        "This appointment is less than " + NoticeDays +
                        " days away, so an Admin must type a reason for the reschedule.");
                }
            }

            DataTable lines = GetLinesForBooking(appointmentId);
            if (lines.Rows.Count == 0)
            {
                throw new InvalidOperationException("This appointment has no services, so it cannot be moved.");
            }

            // Every line moves by the SAME amount: newStart is the new start of the earliest line,
            // so the other lines keep their distance from it (a massage at 1:00 PM stays after a
            // facial at 10:00 AM). A shifted time that lands in the past is simply handed to the
            // checks, which report it like any other problem.
            TimeSpan earliestStart = TimeSpan.Zero;
            bool earliestFound = false;

            foreach (DataRow line in lines.Rows)
            {
                TimeSpan lineStart = ReadTime(line[ColStartTime]);

                // A line with no start of its own is skipped here instead of dragging the earliest
                // start to midnight; PrepareLines places it right after the line above it.
                if (lineStart == TimeSpan.Zero) continue;

                if (!earliestFound || lineStart < earliestStart) earliestStart = lineStart;
                earliestFound = true;
            }

            TimeSpan delta = newStart - earliestStart;

            DataTable shifted = lines.Copy();
            foreach (DataRow line in shifted.Rows)
            {
                TimeSpan lineStart = ReadTime(line[ColStartTime]);

                // A line with no time of its own stays empty, so PrepareLines places it right after
                // the line above it, exactly as it does for a new booking.
                if (lineStart == TimeSpan.Zero) continue;

                line[ColStartTime] = lineStart + delta;
            }

            // The end and the busy end of every line come from its service, exactly as in a booking.
            DataTable preparedLines = PrepareLines(null, null, shifted, newStart);

            // The appointment keeps the earliest line start and the latest line end.
            TimeSpan firstLineStart = newStart;
            TimeSpan lastLineEnd = newStart;
            bool firstPreparedLine = true;

            foreach (DataRow line in preparedLines.Rows)
            {
                TimeSpan lineStart = ReadTime(line[ColStartTime]);
                TimeSpan lineEnd = ReadTime(line[ColEndTime]);

                if (firstPreparedLine || lineStart < firstLineStart) firstLineStart = lineStart;
                if (firstPreparedLine || lineEnd > lastLineEnd) lastLineEnd = lineEnd;
                firstPreparedLine = false;
            }

            int clientId = Db.Get<int>(appointment["client_id"]);
            string bookingSource = Db.Get<string>(appointment["booking_source"]);

            // The promotion of the bill is passed on, so the discount rule is checked again.
            // No money is checked here: the down payment was taken in Billing, not by the booking.
            DataRow billing = BillingRepository.GetByAppointment(appointmentId);
            int promoId = billing == null ? 0 : Db.Get<int>(billing["promo_id"]);

            List<string> problems = ValidateBooking(appointmentId, clientId, newDate, newStart, bookingSource,
                preparedLines, promoId);

            if (problems.Count > 0)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, problems.ToArray()));
            }

            // A session of a package must keep its weekly place: it stays session_interval_days
            // after the session before it and before the session that follows it. The other
            // sessions already exist as their own appointments, so only their dates are read.
            List<string> packageProblems = PackageSessionProblems(appointmentId, newDate, lines);

            if (packageProblems.Count > 0)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, packageProblems.ToArray()));
            }

            using (SqlConnection connection = Db.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Db.Execute(connection, transaction,
@"UPDATE Appointments
SET appointment_date = @appointment_date, start_time = CAST(@start_time AS time),
    end_time = CAST(@end_time AS time), updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                            Db.P("@appointment_date", newDate.Date),
                            Db.P("@start_time", SqlTime(firstLineStart)),
                            Db.P("@end_time", SqlTime(lastLineEnd)),
                            Db.P("@appointment_id", appointmentId));

                        // Every line keeps its own new window, so the room and technician rules
                        // read the right times for each service.
                        foreach (DataRow line in preparedLines.Rows)
                        {
                            Db.Execute(connection, transaction,
@"UPDATE AppointmentServices
SET start_time = CAST(@start_time AS time), end_time = CAST(@end_time AS time),
    busy_end_time = CAST(@busy_end_time AS time)
WHERE appointment_service_id = @appointment_service_id",
                                Db.P("@start_time", SqlTime(ReadTime(line[ColStartTime]))),
                                Db.P("@end_time", SqlTime(ReadTime(line[ColEndTime]))),
                                Db.P("@busy_end_time", SqlTime(ReadTime(line[ColBusyEndTime]))),
                                Db.P("@appointment_service_id", Db.Get<int>(line[ColAppointmentServiceId])));
                        }

                        // The old room reservations are not valid for the new time, so they are
                        // planned again line by line, each for its own window.
                        Db.Execute(connection, transaction,
                            "DELETE FROM AppointmentRooms WHERE appointment_id = @appointment_id",
                            Db.P("@appointment_id", appointmentId));

                        foreach (DataRow line in preparedLines.Rows)
                        {
                            int serviceId = Db.Get<int>(line[ColServiceId]);

                            DataRow serviceInfo = GetServiceInfo(connection, transaction, serviceId);
                            if (serviceInfo == null) continue;

                            string roomName;
                            int roomId = AvailabilityRepository.PickRoom(connection, transaction, newDate,
                                Db.Get<int>(serviceInfo["category_id"]), ReadTime(line[ColStartTime]),
                                ReadTime(line[ColEndTime]), appointmentId, out roomName);

                            if (roomId <= 0) continue;

                            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentRooms (appointment_id, appointment_service_id, room_id, status)
VALUES (@appointment_id, @appointment_service_id, @room_id, 'Reserved')",
                                Db.P("@appointment_id", appointmentId),
                                Db.P("@appointment_service_id", Db.Get<int>(line[ColAppointmentServiceId])),
                                Db.P("@room_id", roomId));
                        }

                        Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type,
                                 old_appointment_date, old_start_time, old_status, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Rescheduled', @old_date, CAST(@old_time AS time),
        @old_status, @new_status, @reason)",
                            Db.P("@appointment_id", appointmentId),
                            Db.P("@changed_by", Helpers.Session.UserId),
                            Db.P("@old_date", oldDate.Date),
                            Db.P("@old_time", SqlTime(oldStart)),
                            Db.P("@old_status", currentStatus),
                            Db.P("@new_status", currentStatus),
                            Db.P("@reason", string.IsNullOrWhiteSpace(reason)
                                ? "Moved from " + oldText + " to " + newDate.ToString("MMM dd, yyyy") + " " + Clock(newStart) + "."
                                : reason.Trim()));

                        transaction.Commit();
                    }
                    catch
                    {
                        try { transaction.Rollback(); }
                        catch { /* the original error is the one to report */ }
                        throw;
                    }
                }
            }

            ActivityRepository.Log(Helpers.Session.UserId, "Reschedule Appointment", "Appointments",
                "Appointment #" + appointmentId + " moved from " + oldText + " to " +
                newDate.ToString("MMM dd, yyyy") + " " + Clock(newStart));
        }

        /// <summary>
        /// Moves an appointment and, when moveLaterSessions is true, keeps the weekly plan of its
        /// package by moving every later session by the same date and time delta, each through the
        /// 4-parameter Reschedule so every step is checked (and valid) on its own.
        /// WHY the order: a move that goes forward is done from the last session back, a move that
        /// goes back from the selected one forward, so no step ever breaks the weekly rule that
        /// PackageSessionProblems enforces for the step after it.
        /// </summary>
        public static void Reschedule(int appointmentId, DateTime newDate, TimeSpan newStart,
            string reason, bool moveLaterSessions)
        {
            if (!moveLaterSessions)
            {
                Reschedule(appointmentId, newDate, newStart, reason);
                return;
            }

            DataRow appointment = GetById(appointmentId);
            if (appointment == null) throw new InvalidOperationException("This appointment no longer exists.");

            // Appointments.start_time is kept as the earliest line start, so it is the anchor the
            // dialog's reschedule also uses.
            DateTime oldDate = Db.Get<DateTime>(appointment["appointment_date"]).Date;
            TimeSpan oldStart = ReadTime(appointment["start_time"]);

            TimeSpan dateDelta = newDate.Date - oldDate;
            TimeSpan timeDelta = newStart - oldStart;

            DataTable laterSessions = GetLaterSessions(appointmentId);

            DataRow selectedLine = PackageSessionLine(appointmentId);
            string selectedName = selectedLine == null
                ? "This appointment"
                : Db.Get<string>(selectedLine["service_name"]);
            int selectedNumber = selectedLine == null ? 0 : Db.Get<int>(selectedLine["session_number"]);

            // The selected appointment is a step like the others; its own row is built in the
            // shape GetLaterSessions returns so the loop below can treat every step the same way.
            DataRow selectedStep = laterSessions.NewRow();
            selectedStep["appointment_id"] = appointmentId;
            selectedStep["session_number"] = selectedNumber;
            selectedStep["service_name"] = selectedName;
            selectedStep["appointment_date"] = oldDate;
            selectedStep["start_time"] = oldStart;

            List<DataRow> order = new List<DataRow>();

            if (dateDelta + timeDelta > TimeSpan.Zero)
            {
                for (int i = laterSessions.Rows.Count - 1; i >= 0; i--) order.Add(laterSessions.Rows[i]);
                order.Add(selectedStep);
            }
            else
            {
                order.Add(selectedStep);
                foreach (DataRow row in laterSessions.Rows) order.Add(row);
            }

            int moved = 0;

            foreach (DataRow step in order)
            {
                int stepAppointmentId = Db.Get<int>(step["appointment_id"]);
                DateTime stepDate = Db.Get<DateTime>(step["appointment_date"]).Date + dateDelta;
                TimeSpan stepStart = ReadTime(step["start_time"]) + timeDelta;

                try
                {
                    Reschedule(stepAppointmentId, stepDate, stepStart, reason);
                    moved++;
                }
                catch (InvalidOperationException ex)
                {
                    // The whole set is not rolled back on purpose: the prompt wants the caller to
                    // know how many sessions already moved, so the rest can be fixed by hand.
                    throw new InvalidOperationException(
                        Db.Get<string>(step["service_name"]) + " session " + Db.Get<int>(step["session_number"]) +
                        " could not be moved: " + ex.Message + " " + moved +
                        " session(s) were already moved; the others stayed where they were.");
                }
            }

            int laterMoved = moved > 0 ? moved - 1 : 0;

            if (laterMoved > 0)
            {
                ActivityRepository.Log(Helpers.Session.UserId, "Reschedule Appointment", "Appointments",
                    "Appointment #" + appointmentId + " moved together with " + laterMoved +
                    " later package session(s).");
            }
        }

        /// <summary>
        /// Cancels ONE appointment: the statuses change, the rooms are released, the money paid
        /// upfront is forfeited, a package that was opened for this booking and never used
        /// expires, and the reason is written into the history.
        /// The 3-day rule is the same as for a reschedule (Admin override with a reason).
        /// The later sessions of the same package stay where they are (see the overload).
        /// </summary>
        public static void Cancel(int appointmentId, string reason)
        {
            Cancel(appointmentId, reason, false);
        }

        /// <summary>
        /// Cancels an appointment and, when includeLaterSessions is true, its later package
        /// sessions in the SAME transaction, right after the selected one. Session 1 cannot be
        /// cancelled alone: it carries the payment of the whole package.
        /// The 3-day notice rule is checked only for the selected appointment.
        /// </summary>
        public static void Cancel(int appointmentId, string reason, bool includeLaterSessions)
        {
            DataRow appointment = GetById(appointmentId);
            if (appointment == null) throw new InvalidOperationException("This appointment no longer exists.");

            string currentStatus = Db.Get<string>(appointment["status"]);
            if (!string.Equals(currentStatus, "Upcoming", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only an upcoming appointment can be cancelled.");
            }

            DateTime appointmentDate = Db.Get<DateTime>(appointment["appointment_date"]);

            int daysAway;
            if (IsInsideNoticeWindow(appointmentDate, out daysAway))
            {
                if (!Helpers.Session.IsAdmin)
                {
                    throw new InvalidOperationException(
                        "An appointment can only be cancelled at least " + NoticeDays +
                        " days before its date. This one is in " + Math.Max(daysAway, 0) +
                        " day(s). Ask an Admin.");
                }

                if (string.IsNullOrWhiteSpace(reason))
                {
                    throw new InvalidOperationException(
                        "This appointment is less than " + NoticeDays +
                        " days away, so an Admin must type a reason for the cancellation.");
                }
            }

            int userId = Helpers.Session.UserId;

            // The later sessions are read BEFORE the transaction: they are still 'Upcoming' now,
            // while the selected appointment is about to become 'Cancelled' inside it.
            DataTable laterSessions = GetLaterSessions(appointmentId);

            // The session number of the SELECTED appointment names it in the history of the
            // sessions it drags along (0 when the appointment is not a package session).
            DataRow selectedPackageLine = PackageSessionLine(appointmentId);
            int selectedSessionNumber = selectedPackageLine == null
                ? 0
                : Db.Get<int>(selectedPackageLine["session_number"]);

            // Session 1 carries the payment of the whole package, so it cannot be cancelled alone:
            // the client has paid for the weeks that come after it too.
            if (!includeLaterSessions && laterSessions.Rows.Count > 0 && selectedSessionNumber == 1)
            {
                throw new InvalidOperationException(
                    "Session 1 carries the payment of the package, so it can only be cancelled " +
                    "together with the later sessions.");
            }

            if (!includeLaterSessions) laterSessions = null;

            int cancelledSessions = 0;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                CancelCore(connection, transaction, appointmentId, currentStatus, userId, reason);

                // Every later session is cancelled in the SAME transaction, right after the
                // selected one, with a history row that names the session it followed.
                if (laterSessions != null)
                {
                    foreach (DataRow session in laterSessions.Rows)
                    {
                        int sessionAppointmentId = Db.Get<int>(session["appointment_id"]);

                        DataRow later = GetById(sessionAppointmentId);
                        string laterStatus = later == null ? "Upcoming" : Db.Get<string>(later["status"]);

                        CancelCore(connection, transaction, sessionAppointmentId, laterStatus, userId,
                            "Cancelled together with session " + selectedSessionNumber + " (appointment #" +
                            appointmentId + ")." +
                            (string.IsNullOrWhiteSpace(reason) ? string.Empty : " " + reason.Trim()));

                        cancelledSessions++;
                    }
                }

                // The package expires only AFTER every cancellation of this transaction, and only
                // when it was never used and no session of it is still booked. Cancelling one
                // middle session must not expire a package that still has sessions ahead.
                ExpireUnusedPackage(connection, transaction, appointmentId);
            });

            ActivityRepository.Log(userId, "Cancel Appointment", "Appointments",
                "Appointment #" + appointmentId + " cancelled" +
                (cancelledSessions > 0 ? " together with " + cancelledSessions + " later package session(s)" : "") +
                (string.IsNullOrWhiteSpace(reason) ? "." : ": " + reason.Trim()));
        }

        /// <summary>
        /// The per-appointment writes of a cancellation: statuses, rooms, forfeit and the history
        /// row. Shared by Cancel so a package session is cancelled exactly like a single one.
        /// </summary>
        private static void CancelCore(SqlConnection connection, SqlTransaction transaction,
            int appointmentId, string currentStatus, int userId, string reason)
        {
            Db.Execute(connection, transaction,
@"UPDATE Appointments
SET status = 'Cancelled', cancelled_at = GETDATE(), updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            Db.Execute(connection, transaction,
                "UPDATE AppointmentServices SET status = 'Cancelled' WHERE appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            // The rooms are free again, so release them instead of leaving them blocked.
            Db.Execute(connection, transaction,
                "UPDATE AppointmentRooms SET status = 'Released' WHERE appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            // A cancellation always forfeits the money paid upfront. A deposit becomes
            // 'Forfeited'; a promotion that was paid in full makes the bill 'Forfeited'.
            Db.Execute(connection, transaction,
@"UPDATE Billing SET
        downpayment_status = CASE WHEN downpayment_status IN ('Held', 'Applied') THEN 'Forfeited'
                                  ELSE downpayment_status END,
        payment_status = CASE WHEN payment_status = 'Paid' AND total_amount > 0 THEN 'Forfeited'
                              ELSE payment_status END,
        updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, old_status, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Cancelled', @old_status, 'Cancelled', @reason)",
                Db.P("@appointment_id", appointmentId),
                Db.P("@changed_by", userId),
                Db.P("@old_status", currentStatus),
                Db.P("@reason", string.IsNullOrWhiteSpace(reason)
                    ? "Cancelled with more than " + NoticeDays + " days notice."
                    : reason.Trim()));
        }

        /// <summary>
        /// Expires the package of the cancelled appointment, but only when it was never used
        /// (sessions_completed = 0) AND no session of it is left booked. Runs after every
        /// cancellation of the transaction, so a package with sessions still ahead stays Active.
        /// </summary>
        private static void ExpireUnusedPackage(SqlConnection connection, SqlTransaction transaction,
            int appointmentId)
        {
            Db.Execute(connection, transaction,
@"UPDATE cp SET status = 'Expired'
FROM ClientPackages cp
INNER JOIN AppointmentServices asv ON asv.package_id = cp.package_id
WHERE asv.appointment_id = @appointment_id
  AND cp.sessions_completed = 0 AND cp.status = 'Active'
  AND NOT EXISTS (SELECT 1 FROM AppointmentServices other
                  WHERE other.package_id = cp.package_id AND other.status <> 'Cancelled')",
                Db.P("@appointment_id", appointmentId));
        }

        /// <summary>
        /// The appointment's own package line (session_number and service_name), or null when the
        /// appointment is not a package session. Session 1 is the one that carries the payment.
        /// </summary>
        private static DataRow PackageSessionLine(int appointmentId)
        {
            DataTable line = Db.Query(
@"SELECT TOP 1 asv.session_number, s.service_name
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
WHERE asv.appointment_id = @appointment_id
  AND ISNULL(asv.package_id, 0) > 0 AND ISNULL(asv.total_sessions, 0) > 1
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));

            return line.Rows.Count == 0 ? null : line.Rows[0];
        }

        /// <summary>
        /// Starts every appointment whose scheduled time has arrived: 'Upcoming' becomes 'Ongoing'.
        /// WHY a past day counts too: a forgotten appointment must still be payable in Billing.
        /// Returns how many appointments were started (0 when nothing was due yet).
        /// </summary>
        public static int AutoStartDue()
        {
            int userId = Helpers.Session.UserId;
            int started = 0;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                // The whole set is read first, because each row is written one by one below.
                DataTable due = Db.Query(connection, transaction,
@"SELECT appointment_id
FROM Appointments
WHERE status = 'Upcoming'
  AND CAST(appointment_date AS datetime) + CAST(start_time AS datetime) <= GETDATE()
ORDER BY appointment_id");

                foreach (DataRow row in due.Rows)
                {
                    int appointmentId = Db.Get<int>(row["appointment_id"]);

                    Db.Execute(connection, transaction,
@"UPDATE Appointments SET status = 'Ongoing', updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                        Db.P("@appointment_id", appointmentId));

                    Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, old_status, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Status Change', 'Upcoming', 'Ongoing', @reason)",
                        Db.P("@appointment_id", appointmentId),
                        Db.P("@changed_by", userId),
                        Db.P("@reason", "Started automatically at the scheduled time."));

                    started++;
                }
            });

            // One line for the whole run, so the activity log is not flooded by the timer.
            if (started > 0)
            {
                ActivityRepository.Log(userId, "Auto-start", "Appointments",
                    "Auto-start: " + started + " appointment(s) became Ongoing");
            }

            return started;
        }

        // ---------------------------------------------------------------------
        // PRIVATE HELPERS
        // ---------------------------------------------------------------------

        /// <summary>
        /// Checks a moved package session against its neighbours: a session must stay at least
        /// Services.session_interval_days after the session before it and at least that many days
        /// before the session that follows it, because a package is booked as weekly sessions.
        /// The other sessions already exist as their own appointments, so only their numbers and
        /// dates are read. Returns one plain sentence per failing line (empty means the move fits).
        /// </summary>
        private static List<string> PackageSessionProblems(int appointmentId, DateTime newDate, DataTable lines)
        {
            List<string> problems = new List<string>();

            if (lines == null) return problems;

            foreach (DataRow line in lines.Rows)
            {
                int packageId = Db.Get<int>(line[ColPackageId]);
                int sessionNumber = Db.Get<int>(line[ColSessionNumber]);
                int totalSessions = Db.Get<int>(line[ColTotalSessions]);

                // Only a real package session has neighbours to keep a distance from.
                if (totalSessions <= 1 || packageId <= 0 || sessionNumber <= 0) continue;

                int serviceId = Db.Get<int>(line[ColServiceId]);
                DataRow service = GetServiceInfo(null, null, serviceId);

                string serviceName = service == null
                    ? "Service #" + serviceId
                    : Db.Get<string>(service["service_name"]);

                // A missing or zero interval means the weekly default of 7 days.
                int intervalDays = service == null ? 7 : Db.Get<int>(service["session_interval_days"], 7);
                if (intervalDays < 1) intervalDays = 7;

                // The other sessions of the same package that still count: neither the line nor its
                // appointment is cancelled. The moved appointment is left out through its own id.
                DataTable others = Db.Query(
@"SELECT asv.session_number, a.appointment_date
FROM AppointmentServices asv
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE asv.package_id = @package_id
  AND asv.appointment_id <> @appointment_id
  AND asv.status <> 'Cancelled'
  AND a.status <> 'Cancelled'",
                    Db.P("@package_id", packageId),
                    Db.P("@appointment_id", appointmentId));

                int previousNumber = 0;
                DateTime previousDate = DateTime.MinValue;
                int nextNumber = 0;
                DateTime nextDate = DateTime.MinValue;

                foreach (DataRow other in others.Rows)
                {
                    int number = Db.Get<int>(other["session_number"]);
                    if (number <= 0) continue;

                    // The nearest session below and the nearest one above this line decide.
                    if (number < sessionNumber && number > previousNumber)
                    {
                        previousNumber = number;
                        previousDate = Db.Get<DateTime>(other["appointment_date"]).Date;
                    }

                    if (number > sessionNumber && (nextNumber == 0 || number < nextNumber))
                    {
                        nextNumber = number;
                        nextDate = Db.Get<DateTime>(other["appointment_date"]).Date;
                    }
                }

                bool tooEarly = previousNumber > 0 && newDate.Date < previousDate.AddDays(intervalDays);
                bool tooLate = nextNumber > 0 && newDate.Date > nextDate.AddDays(-intervalDays);

                if (!tooEarly && !tooLate) continue;

                // The sentence names only the halves that have a neighbour, so it stays short.
                string sentence = serviceName + " Session " + sessionNumber + " of " + totalSessions;

                if (tooEarly)
                {
                    sentence += " must stay at least " + intervalDays + " days after Session " +
                        previousNumber + " (" + previousDate.ToString("MMM dd, yyyy") + ")";
                }

                if (tooLate)
                {
                    sentence += tooEarly ? " and at least " : " must stay at least ";
                    sentence += intervalDays + " days before Session " +
                        nextNumber + " (" + nextDate.ToString("MMM dd, yyyy") + ")";
                }

                sentence += ". Move the other sessions first.";
                problems.Add(sentence);
            }

            return problems;
        }

        /// <summary>
        /// Works out the real price of every line and which package row it uses (the package rule:
        /// the whole package price sits on the first session, every later session is 0.00).
        /// A package that does not exist yet is created here, inside the booking transaction.
        /// </summary>
        private static DataTable ResolveLinePrices(SqlConnection connection, SqlTransaction transaction,
            int clientId, DataTable lines)
        {
            DataTable resolved = BuildLinesTable();

            foreach (DataRow row in lines.Rows)
            {
                int serviceId = Db.Get<int>(row[ColServiceId]);
                int quantity = Db.Get<int>(row[ColQuantity], 1);
                if (quantity < 1) quantity = 1;

                DataRow service = GetServiceInfo(connection, transaction, serviceId);
                if (service == null)
                {
                    throw new InvalidOperationException("One of the selected services no longer exists. Add it again.");
                }

                decimal servicePrice = Db.Get<decimal>(service["price"]);
                int defaultSessions = Db.Get<int>(service["default_total_sessions"], 1);

                int packageId = 0;
                int sessionNumber = 0;
                int totalSessions = 0;
                decimal linePrice = servicePrice;

                if (defaultSessions > 1)
                {
                    DataRow existing = FindActivePackage(connection, transaction, clientId, serviceId);

                    if (existing == null)
                    {
                        // A new package: it is created with the booking that opens it.
                        object id = Db.Scalar(connection, transaction,
@"INSERT INTO ClientPackages (client_id, service_id, total_sessions, purchase_date, status)
VALUES (@client_id, @service_id, @total_sessions, CAST(GETDATE() AS date), 'Active');
SELECT CAST(SCOPE_IDENTITY() AS int);",
                            Db.P("@client_id", clientId),
                            Db.P("@service_id", serviceId),
                            Db.P("@total_sessions", defaultSessions));

                        packageId = Convert.ToInt32(id);
                        sessionNumber = 1;
                        totalSessions = defaultSessions;
                        linePrice = servicePrice;   // the whole package is paid on the first session
                    }
                    else
                    {
                        packageId = Db.Get<int>(existing["package_id"]);
                        totalSessions = Db.Get<int>(existing["total_sessions"], defaultSessions);
                        sessionNumber = AvailabilityRepository.NextSessionNumber(connection, transaction,
                            packageId, totalSessions);
                        linePrice = sessionNumber == 1 ? servicePrice : 0m;
                    }
                }

                resolved.Rows.Add(
                    serviceId,
                    Db.Get<string>(service["service_name"]),
                    quantity,
                    linePrice,
                    Db.Get<int>(service[ColDuration]),
                    Db.Get<int>(row[ColTechnicianId]),
                    packageId,
                    sessionNumber,
                    totalSessions);

                // A start the caller already set (a prepared table) is kept; otherwise the column
                // stays empty and PrepareLines places the line after the one above it.
                if (lines.Columns.Contains(ColStartTime))
                {
                    resolved.Rows[resolved.Rows.Count - 1][ColStartTime] = ReadTime(row[ColStartTime]);
                }
            }

            return resolved;
        }

        /// <summary>An empty lines table, so the resolved prices can be walked through safely.</summary>
        private static DataTable BuildLinesTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add(ColServiceId, typeof(int));
            table.Columns.Add(ColServiceName, typeof(string));
            table.Columns.Add(ColQuantity, typeof(int));
            table.Columns.Add(ColPrice, typeof(decimal));
            table.Columns.Add(ColDuration, typeof(int));
            table.Columns.Add(ColTechnicianId, typeof(int));
            table.Columns.Add(ColPackageId, typeof(int));
            table.Columns.Add(ColSessionNumber, typeof(int));
            table.Columns.Add(ColTotalSessions, typeof(int));
            // The line's own start travels with the resolved row, so Create keeps the times the
            // validator already approved. A line without one is placed after the line above.
            table.Columns.Add(ColStartTime, typeof(TimeSpan));
            return table;
        }

        /// <summary>
        /// An empty table with the columns GetLaterSessions promises, so an appointment without a
        /// package returns the same shape as one with later sessions (and a caller can read it
        /// without checking for null or for missing columns).
        /// </summary>
        private static DataTable BuildLaterSessionsTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("appointment_id", typeof(int));
            table.Columns.Add("session_number", typeof(int));
            table.Columns.Add("total_sessions", typeof(int));
            table.Columns.Add("service_name", typeof(string));
            table.Columns.Add("appointment_date", typeof(DateTime));
            table.Columns.Add("start_time", typeof(TimeSpan));
            return table;
        }

        /// <summary>The lines of a booking as the discount formula wants them (service_id + amount).</summary>
        private static DataTable BuildPromoLines(DataTable lines)
        {
            DataTable table = new DataTable();
            table.Columns.Add("service_id", typeof(int));
            table.Columns.Add("amount", typeof(decimal));

            if (lines != null)
            {
                foreach (DataRow row in lines.Rows)
                {
                    int quantity = Db.Get<int>(row[ColQuantity], 1);
                    if (quantity < 1) quantity = 1;

                    table.Rows.Add(Db.Get<int>(row[ColServiceId]), Db.Get<decimal>(row[ColPrice]) * quantity);
                }
            }

            return table;
        }

        /// <summary>The service with its category (price, duration, package size, category name).</summary>
        internal static DataRow GetServiceInfo(SqlConnection connection, SqlTransaction transaction, int serviceId)
        {
            DataTable table = Db.Query(connection, transaction,
@"SELECT s.service_id, s.service_name, s.category_id, c.category_name, s.price,
         s.duration_minutes, s.default_total_sessions,
         s.staff_busy_minutes, s.session_interval_days, s.requires_assessment
FROM Services s
INNER JOIN ServiceCategories c ON c.category_id = s.category_id
WHERE s.service_id = @service_id",
                Db.P("@service_id", serviceId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        internal static DataTable GetRoomsForCategory(SqlConnection connection, SqlTransaction transaction, int categoryId)
        {
            return Db.Query(connection, transaction,
@"SELECT r.room_id, r.room_name, r.capacity
FROM RoomServiceCategories rsc
INNER JOIN Rooms r ON r.room_id = rsc.room_id
WHERE rsc.category_id = @category_id AND r.status = 'Available'
ORDER BY r.capacity DESC, r.room_name",
                Db.P("@category_id", categoryId));
        }

        /// <summary>
        /// The first Active package of a client for one service that still has a free session
        /// number, or null when there is none (the caller then opens a new package).
        /// </summary>
        private static DataRow FindActivePackage(SqlConnection connection, SqlTransaction transaction,
            int clientId, int serviceId)
        {
            // The first Active package that still has a free session number, in purchase order.
            // A package whose sessions are all booked has nothing left, so the next booking
            // starts a new package instead of reusing a finished one.
            DataTable table = Db.Query(connection, transaction,
@"SELECT package_id, total_sessions, sessions_completed
FROM ClientPackages
WHERE client_id = @client_id AND service_id = @service_id AND status = 'Active'
ORDER BY purchase_date, package_id",
                Db.P("@client_id", clientId),
                Db.P("@service_id", serviceId));

            foreach (DataRow row in table.Rows)
            {
                int totalSessions = Db.Get<int>(row["total_sessions"]);

                if (AvailabilityRepository.NextSessionNumber(connection, transaction,
                    Db.Get<int>(row["package_id"]), totalSessions) > 0)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>How many appointments (not cancelled) a day already has.</summary>
        internal static int CountAppointmentsThatDay(SqlConnection connection, SqlTransaction transaction,
            DateTime appointmentDate, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(*) FROM Appointments
WHERE appointment_date = @appointment_date AND status <> 'Cancelled'
  AND appointment_id <> @exclude_id",
                Db.P("@appointment_date", appointmentDate.Date),
                Db.P("@exclude_id", excludeAppointmentId));

            return Convert.ToInt32(count);
        }

        /// <summary>How many appointments of that day already contain a massage service.</summary>
        internal static int CountMassageThatDay(SqlConnection connection, SqlTransaction transaction,
            DateTime appointmentDate, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(DISTINCT a.appointment_id)
FROM Appointments a
INNER JOIN AppointmentServices asv ON asv.appointment_id = a.appointment_id
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN ServiceCategories c ON c.category_id = s.category_id
WHERE a.appointment_date = @appointment_date
  AND a.status <> 'Cancelled'
  AND a.appointment_id <> @exclude_id
  AND c.category_name LIKE @massage_like",
                Db.P("@appointment_date", appointmentDate.Date),
                Db.P("@exclude_id", excludeAppointmentId),
                Db.P("@massage_like", MassageCategoryPrefix + "%"));

            return Convert.ToInt32(count);
        }

        /// <summary>The full name of a user, for a readable message.</summary>
        internal static string UserName(SqlConnection connection, SqlTransaction transaction, int userId)
        {
            DataTable table = Db.Query(connection, transaction,
                "SELECT full_name FROM Users WHERE user_id = @user_id",
                Db.P("@user_id", userId));

            return table.Rows.Count == 0 ? "The selected technician" : Db.Get<string>(table.Rows[0]["full_name"]);
        }

        /// <summary>True when the category name means "massage" (they open later and close later).</summary>
        internal static bool IsMassageCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return false;
            return categoryName.Trim().StartsWith(MassageCategoryPrefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// A time value written the way SQL Server reads it as a time, which keeps the parameters
        /// independent of the database provider's type mapping.
        /// </summary>
        internal static string SqlTime(TimeSpan value)
        {
            return value.ToString(@"hh\:mm\:ss");
        }

        /// <summary>A time column read back safely (SqlDataReader and DataAdapter differ here).</summary>
        internal static TimeSpan ReadTime(object value)
        {
            if (value == null || value == DBNull.Value) return TimeSpan.Zero;
            if (value is TimeSpan) return (TimeSpan)value;
            if (value is DateTime) return ((DateTime)value).TimeOfDay;

            TimeSpan parsed;
            if (TimeSpan.TryParse(value.ToString(), out parsed)) return parsed;
            return TimeSpan.Zero;
        }

        /// <summary>"10:00 AM" for a time value, so every message reads the same way.</summary>
        internal static string Clock(TimeSpan value)
        {
            return DateTime.Today.Add(value).ToString("h:mm tt");
        }

    }
}
