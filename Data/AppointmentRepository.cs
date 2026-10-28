using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about appointments lives here: the day list, the booking, the
    /// reschedule, the cancel and the "start service" step.
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
         c.last_name + ', ' + c.first_name AS client_name,
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
       OR (c.last_name + ', ' + c.first_name) LIKE @like
       OR c.first_name LIKE @like
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
         s.service_name + CASE WHEN asv.quantity > 1 THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name
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
         c.last_name + ', ' + c.first_name AS client_name,
         a.status,
         ISNULL(b.payment_status, 'Unpaid') AS payment_status
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
LEFT JOIN Billing b ON b.appointment_id = a.appointment_id
WHERE a.appointment_date >= @from_date
  AND a.appointment_date <= @to_date
ORDER BY a.appointment_date, a.start_time, a.appointment_id",
                Db.P("@from_date", first),
                Db.P("@to_date", last));

            table.Columns.Add("start_time_text", typeof(string));
            table.Columns.Add("services", typeof(string));
            table.Columns.Add("staff_names", typeof(string));

            Dictionary<int, string> services = ChildNamesForRange(first, last,
@"SELECT asv.appointment_id,
         s.service_name + CASE WHEN asv.quantity > 1 THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name
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
         asv.status, s.duration_minutes
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
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
         c.last_name + ', ' + c.first_name AS client_name
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
@"SELECT asv.service_id, s.service_name, asv.quantity, asv.price, s.duration_minutes,
         s.default_total_sessions, ISNULL(ast.user_id, 0) AS technician_id,
         ISNULL(asv.package_id, 0) AS package_id,
         ISNULL(asv.session_number, 0) AS session_number,
         ISNULL(asv.total_sessions, 0) AS total_sessions
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
LEFT JOIN AppointmentStaff ast ON ast.appointment_service_id = asv.appointment_service_id
                               AND ast.status <> 'Reassigned'
WHERE asv.appointment_id = @appointment_id AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));
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
        /// How much money the shop asks for right now:
        ///   - a promotion needs the FULL total, because promos are paid in full upfront
        ///   - otherwise a booking from Messenger / Text / Phone needs 50% of the subtotal
        ///   - a walk-in without a promotion pays at the counter, so nothing is required now
        /// The form and the validator both call this, so the amount can never differ between them.
        /// </summary>
        public static decimal RequiredNow(string bookingSource, decimal subtotal, decimal discount)
        {
            decimal total = subtotal - discount;
            if (total < 0m) total = 0m;

            // WHY the promotion is checked first: a promo is never an installment, so the whole
            // total is due now whichever way the client booked (PROJECT_RULES, "Promos").
            if (discount > 0m) return total;

            bool walkIn = string.Equals(bookingSource, "Walk-in", StringComparison.OrdinalIgnoreCase);
            if (walkIn) return 0m;

            decimal half = Math.Round(subtotal / 2m, 2, MidpointRounding.AwayFromZero);
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
        /// Checks a booking against the business rules and returns the problems it found
        /// (an empty list means the booking is fine).
        ///   appointmentId - 0 for a new booking, or the id being rescheduled, so the appointment
        ///                   does not count itself in the daily capacity or against the rooms
        ///   lines         - service_id, quantity, price, duration_minutes, technician_id per line
        ///   amountPaid    - the money handed over now (0 for a walk-in without promotion)
        /// Every message is a sentence a staff member can understand.
        /// </summary>
        public static List<string> ValidateBooking(int appointmentId, int clientId, DateTime appointmentDate,
            TimeSpan startTime, string bookingSource, DataTable lines, int promoId,
            decimal amountPaid, string paymentMethod, string paymentReference, bool paymentVerified)
        {
            List<string> problems = new List<string>();

            if (clientId <= 0) problems.Add("Choose the client first.");

            if (lines == null || lines.Rows.Count == 0)
            {
                problems.Add("Add at least one service to the appointment.");
                return problems;   // nothing else can be checked without services
            }

            TimeSpan duration = TotalDuration(lines);
            TimeSpan endTime = startTime + duration;

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

            // Opening hours: every service must start after it opens and finish before it closes.
            bool hasMassage = false;

            foreach (DataRow row in lines.Rows)
            {
                DataRow service = GetServiceInfo(null, null, Db.Get<int>(row[ColServiceId]));
                if (service == null) continue;

                string categoryName = Db.Get<string>(service["category_name"]);
                bool massage = IsMassageCategory(categoryName);
                if (massage) hasMassage = true;

                TimeSpan opens = massage ? MassageOpening : AestheticOpening;
                TimeSpan closes = massage ? MassageClosing : AestheticClosing;

                if (startTime < opens || endTime > closes)
                {
                    problems.Add("'" + Db.Get<string>(service["service_name"]) + "' can only be done between " +
                        Clock(opens) + " and " + Clock(closes) + ", so this appointment does not fit.");
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

            // Rooms: every line needs a room that is linked to its category and is still free.
            // BuildRoomPlan puts the chosen room in a copy of the lines table (see Create).
            BuildRoomPlan(null, null, appointmentDate, startTime, endTime, lines, appointmentId, problems);

            // A requested technician may not already have an overlapping service.
            foreach (DataRow row in lines.Rows)
            {
                int technicianId = Db.Get<int>(row[ColTechnicianId]);
                if (technicianId <= 0) continue;

                if (HasTechnicianOverlap(null, null, technicianId, appointmentDate, startTime, endTime, appointmentId))
                {
                    string name = UserName(null, null, technicianId);
                    problems.Add(name + " is already booked for another service at this time. " +
                        "Choose another technician or another time.");
                }
            }

            // Promotion: it must be valid today AND apply to at least one service of this booking.
            decimal discount = 0m;
            if (promoId > 0)
            {
                string promoMessage;
                discount = PromotionRepository.CalculateDiscount(promoId, BuildPromoLines(lines), out promoMessage);

                if (!string.IsNullOrEmpty(promoMessage))
                {
                    problems.Add(promoMessage);
                }
            }

            // Money: the upfront payment must reach what the shop asks for and must be verifiable.
            decimal subtotal = Subtotal(lines);
            decimal required = RequiredNow(bookingSource, subtotal, discount);

            if (required > 0m && amountPaid < required)
            {
                problems.Add("This " + bookingSource + " booking needs " + Money.Text(required) +
                    " paid now" + (discount > 0m
                        ? " (a promotion is always paid in full)."
                        : " (50% of the " + Money.Text(subtotal) + " subtotal)."));
            }

            if (amountPaid > 0m && NeedsReference(paymentMethod))
            {
                if (string.IsNullOrWhiteSpace(paymentReference))
                {
                    problems.Add("Type the " + paymentMethod + " reference number.");
                }

                if (!paymentVerified)
                {
                    problems.Add("Tick 'I verified this payment in the GCash / bank app' before saving a " +
                        paymentMethod + " payment.");
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
        /// the rooms, the bill, the upfront payment, the history row and the activity log entry.
        /// Returns the new appointment_id.
        /// The rules are checked again here (through ValidateBooking by the caller) but the prices
        /// are always read from the database, so a changed service price can never be bypassed.
        /// </summary>
        public static int Create(int clientId, int userId, DateTime appointmentDate, TimeSpan startTime,
            string bookingSource, string notes, DataTable lines, int promoId,
            decimal amountPaid, string paymentMethod, string paymentReference, bool paymentVerified)
        {
            if (lines == null || lines.Rows.Count == 0)
            {
                throw new InvalidOperationException("An appointment needs at least one service.");
            }

            TimeSpan endTime = startTime + TotalDuration(lines);
            int newAppointmentId = 0;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                // 1. The appointment itself.
                object id = Db.Scalar(connection, transaction,
@"INSERT INTO Appointments (client_id, user_id, appointment_date, start_time, end_time,
                           booking_source, status, notes)
VALUES (@client_id, @user_id, @appointment_date, CAST(@start_time AS time), CAST(@end_time AS time),
        @booking_source, 'Upcoming', @notes);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                    Db.P("@client_id", clientId),
                    Db.P("@user_id", userId),
                    Db.P("@appointment_date", appointmentDate.Date),
                    Db.P("@start_time", SqlTime(startTime)),
                    Db.P("@end_time", SqlTime(endTime)),
                    Db.P("@booking_source", bookingSource),
                    Db.P("@notes", string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()));

                newAppointmentId = Convert.ToInt32(id);

                // 2. The prices, worked out from the database (package rule).
                DataTable resolved = ResolveLinePrices(connection, transaction, clientId, lines);

                // 3. The rooms are chosen with the same helper the validator uses.
                List<string> roomProblems = new List<string>();
                DataTable plan = BuildRoomPlan(connection, transaction, appointmentDate, startTime, endTime,
                    resolved, 0, roomProblems);

                // 4. One row per service line, plus its technician and its room.
                decimal subtotal = 0m;

                foreach (DataRow row in plan.Rows)
                {
                    int serviceId = Db.Get<int>(row[ColServiceId]);
                    int quantity = Db.Get<int>(row[ColQuantity], 1);
                    if (quantity < 1) quantity = 1;

                    decimal price = Db.Get<decimal>(row[ColPrice]);
                    int packageId = Db.Get<int>(row[ColPackageId]);
                    int sessionNumber = Db.Get<int>(row[ColSessionNumber]);
                    int totalSessions = Db.Get<int>(row[ColTotalSessions]);
                    int technicianId = Db.Get<int>(row[ColTechnicianId]);
                    int roomId = Db.Get<int>(row[ColRoomId]);

                    subtotal += price * quantity;

                    object lineId = Db.Scalar(connection, transaction,
@"INSERT INTO AppointmentServices (appointment_id, service_id, package_id, quantity, price,
                                  session_number, total_sessions, status)
VALUES (@appointment_id, @service_id, @package_id, @quantity, @price,
        @session_number, @total_sessions, 'Pending');
SELECT CAST(SCOPE_IDENTITY() AS int);",
                        Db.P("@appointment_id", newAppointmentId),
                        Db.P("@service_id", serviceId),
                        Db.P("@package_id", packageId > 0 ? (object)packageId : null),
                        Db.P("@quantity", quantity),
                        Db.P("@price", price),
                        Db.P("@session_number", sessionNumber > 0 ? (object)sessionNumber : null),
                        Db.P("@total_sessions", totalSessions > 0 ? (object)totalSessions : null));

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
                    discount = PromotionRepository.CalculateDiscount(promoId, BuildPromoLines(plan), out promoMessage);
                    if (!string.IsNullOrEmpty(promoMessage)) discount = 0m;
                }

                decimal total = subtotal - discount;
                if (total < 0m) total = 0m;

                // 6. The bill. A deposit is 'Held'; a promotion is paid in full, so it is 'Applied'.
                string downpaymentStatus = amountPaid > 0m ? (promoId > 0 ? "Applied" : "Held") : "None";

                int billingId = BillingRepository.CreateBilling(connection, transaction, newAppointmentId,
                    subtotal, promoId, discount, total, amountPaid, downpaymentStatus, total, "Unpaid");

                // 7. The money paid now. Promos are a Final payment, deposits a Downpayment.
                if (amountPaid > 0m)
                {
                    BillingRepository.AddPayment(connection, transaction, billingId, userId,
                        promoId > 0 ? "Final" : "Downpayment", paymentMethod, paymentReference,
                        amountPaid, paymentVerified ? "Verified" : "Pending");
                }

                // 8. History: the appointment was created.
                Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Created', 'Upcoming', @reason)",
                    Db.P("@appointment_id", newAppointmentId),
                    Db.P("@changed_by", userId),
                    Db.P("@reason", "Booked as a " + bookingSource + " appointment."));
            });

            ActivityRepository.Log(userId, "Book Appointment", "Appointments",
                "Appointment #" + newAppointmentId + " for client #" + clientId + " (" + bookingSource + ")");

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

            TimeSpan endTime = newStart + TotalDuration(lines);
            int clientId = Db.Get<int>(appointment["client_id"]);
            string bookingSource = Db.Get<string>(appointment["booking_source"]);

            // A reschedule keeps the money that is already held, so the payment rule is checked
            // against what was really received (and normally passes without a word).
            DataRow billing = BillingRepository.GetByAppointment(appointmentId);
            decimal paid = billing == null ? 0m : Db.Get<decimal>(billing["paid_amount"]);
            int promoId = billing == null ? 0 : Db.Get<int>(billing["promo_id"]);

            // A reschedule keeps the money that is already held, so the payment rule is checked
            // against what was really received (that is why it normally passes without a word).
            string method;
            string reference;
            bool verified;
            ReadPaymentInfo(appointmentId, out method, out reference, out verified);

            List<string> problems = ValidateBooking(appointmentId, clientId, newDate, newStart, bookingSource,
                lines, promoId, paid, method, reference, verified);

            if (problems.Count > 0)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, problems.ToArray()));
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
                            Db.P("@start_time", SqlTime(newStart)),
                            Db.P("@end_time", SqlTime(endTime)),
                            Db.P("@appointment_id", appointmentId));

                        // The old room reservations are not valid for the new time, so they are
                        // replaced by a fresh plan (the same helper the booking uses).
                        Db.Execute(connection, transaction,
                            "DELETE FROM AppointmentRooms WHERE appointment_id = @appointment_id",
                            Db.P("@appointment_id", appointmentId));

                        List<string> roomProblems = new List<string>();
                        DataTable plan = BuildRoomPlan(connection, transaction, newDate, newStart, endTime,
                            lines, appointmentId, roomProblems);

                        foreach (DataRow row in plan.Rows)
                        {
                            int roomId = Db.Get<int>(row[ColRoomId]);
                            if (roomId <= 0) continue;

                            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentRooms (appointment_id, appointment_service_id, room_id, status)
SELECT @appointment_id, asv.appointment_service_id, @room_id, 'Reserved'
FROM AppointmentServices asv
WHERE asv.appointment_id = @appointment_id AND asv.service_id = @service_id
  AND asv.status <> 'Cancelled'
  AND NOT EXISTS (SELECT 1 FROM AppointmentRooms ar
                  WHERE ar.appointment_service_id = asv.appointment_service_id)",
                                Db.P("@appointment_id", appointmentId),
                                Db.P("@service_id", Db.Get<int>(row[ColServiceId])),
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
        /// Cancels an appointment: the statuses change, the rooms are released, the money paid
        /// upfront is forfeited, a package that was opened for this booking and never used
        /// expires, and the reason is written into the history.
        /// The 3-day rule is the same as for a reschedule (Admin override with a reason).
        /// </summary>
        public static void Cancel(int appointmentId, string reason)
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

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
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
        payment_status = CASE WHEN payment_status = 'Paid' THEN 'Forfeited' ELSE payment_status END,
        updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                    Db.P("@appointment_id", appointmentId));

                // A package opened for this booking that has not been used yet is expired,
                // so the client is not charged for a treatment that never happened.
                Db.Execute(connection, transaction,
@"UPDATE cp SET status = 'Expired'
FROM ClientPackages cp
INNER JOIN AppointmentServices asv ON asv.package_id = cp.package_id
WHERE asv.appointment_id = @appointment_id AND cp.sessions_completed = 0 AND cp.status = 'Active'",
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
            });

            ActivityRepository.Log(userId, "Cancel Appointment", "Appointments",
                "Appointment #" + appointmentId + " cancelled" +
                (string.IsNullOrWhiteSpace(reason) ? "." : ": " + reason.Trim()));
        }

        /// <summary>
        /// Moves an appointment from 'Upcoming' to 'Ongoing' ("Start Service").
        /// WHY the date check: a service can only start on the day of the appointment itself.
        /// </summary>
        public static void SetStatus(int appointmentId, string newStatus)
        {
            DataRow appointment = GetById(appointmentId);
            if (appointment == null) throw new InvalidOperationException("This appointment no longer exists.");

            string currentStatus = Db.Get<string>(appointment["status"]);
            if (!string.Equals(currentStatus, "Upcoming", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only an upcoming appointment can be started.");
            }

            if (Db.Get<DateTime>(appointment["appointment_date"]).Date != DateTime.Today)
            {
                throw new InvalidOperationException("An appointment can only be started on its own day.");
            }

            int userId = Helpers.Session.UserId;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                Db.Execute(connection, transaction,
@"UPDATE Appointments SET status = @status, updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                    Db.P("@status", newStatus),
                    Db.P("@appointment_id", appointmentId));

                Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, old_status, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Status Change', @old_status, @new_status, @reason)",
                    Db.P("@appointment_id", appointmentId),
                    Db.P("@changed_by", userId),
                    Db.P("@old_status", currentStatus),
                    Db.P("@new_status", newStatus),
                    Db.P("@reason", "Service started."));
            });

            ActivityRepository.Log(userId, "Start Service", "Appointments",
                "Appointment #" + appointmentId + " started (" + currentStatus + " -> " + newStatus + ")");
        }

        // ---------------------------------------------------------------------
        // PRIVATE HELPERS
        // ---------------------------------------------------------------------

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
            return table;
        }

        /// <summary>
        /// Chooses a room for every line: the first room linked to the service category whose
        /// overlapping reservations are still below its capacity. The result is a COPY of the
        /// lines table with room_id / room_name filled in, so the caller can insert them.
        /// Every problem found is added to <paramref name="problems"/>.
        /// POLISH: when a category has no room linked in RoomServiceCategories, nothing is
        /// assigned and no problem is raised - otherwise no booking would be possible until the
        /// owner links the rooms of the shop.
        /// </summary>
        private static DataTable BuildRoomPlan(SqlConnection connection, SqlTransaction transaction,
            DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, DataTable lines,
            int excludeAppointmentId, List<string> problems)
        {
            DataTable plan = lines.Copy();

            if (!plan.Columns.Contains(ColRoomId)) plan.Columns.Add(ColRoomId, typeof(int));
            if (!plan.Columns.Contains(ColRoomName)) plan.Columns.Add(ColRoomName, typeof(string));

            foreach (DataRow row in plan.Rows)
            {
                row[ColRoomId] = 0;
                row[ColRoomName] = string.Empty;

                DataRow service = GetServiceInfo(connection, transaction, Db.Get<int>(row[ColServiceId]));
                if (service == null) continue;

                int categoryId = Db.Get<int>(service["category_id"]);
                DataTable rooms = GetRoomsForCategory(connection, transaction, categoryId);

                // No room is linked to this category yet, so the room rule cannot be applied.
                if (rooms.Rows.Count == 0) continue;

                bool placed = false;

                foreach (DataRow room in rooms.Rows)
                {
                    int roomId = Db.Get<int>(room["room_id"]);
                    int capacity = Db.Get<int>(room["capacity"], 1);

                    int used = CountRoomUsage(connection, transaction, roomId, appointmentDate,
                        startTime, endTime, excludeAppointmentId);

                    if (used < capacity)
                    {
                        row[ColRoomId] = roomId;
                        row[ColRoomName] = Db.Get<string>(room["room_name"]);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    problems.Add("No '" + Db.Get<string>(service["category_name"]) + "' room is free from " +
                        Clock(startTime) + " to " + Clock(endTime) + ". Choose another time or another date.");
                }
            }

            return plan;
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

        /// <summary>
        /// How many OTHER appointments already hold this room during the time slot.
        /// Two services of the SAME appointment share the room, so they count once.
        /// </summary>
        private static int CountRoomUsage(SqlConnection connection, SqlTransaction transaction, int roomId,
            DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(DISTINCT ar.appointment_id)
FROM AppointmentRooms ar
INNER JOIN Appointments a ON a.appointment_id = ar.appointment_id
WHERE ar.room_id = @room_id
  AND ar.status <> 'Released'
  AND a.status <> 'Cancelled'
  AND a.appointment_date = @appointment_date
  AND a.start_time < CAST(@end_time AS time)
  AND a.end_time > CAST(@start_time AS time)
  AND a.appointment_id <> @exclude_id",
                Db.P("@room_id", roomId),
                Db.P("@appointment_date", appointmentDate.Date),
                Db.P("@start_time", SqlTime(startTime)),
                Db.P("@end_time", SqlTime(endTime)),
                Db.P("@exclude_id", excludeAppointmentId));

            return Convert.ToInt32(count);
        }

        /// <summary>True when this technician is already assigned to an overlapping service.</summary>
        private static bool HasTechnicianOverlap(SqlConnection connection, SqlTransaction transaction,
            int technicianId, DateTime appointmentDate, TimeSpan startTime, TimeSpan endTime, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(*)
FROM AppointmentStaff ast
INNER JOIN Appointments a ON a.appointment_id = ast.appointment_id
WHERE ast.user_id = @user_id
  AND ast.status = 'Assigned'
  AND a.status IN ('Upcoming', 'Ongoing')
  AND a.appointment_date = @appointment_date
  AND a.start_time < CAST(@end_time AS time)
  AND a.end_time > CAST(@start_time AS time)
  AND a.appointment_id <> @exclude_id",
                Db.P("@user_id", technicianId),
                Db.P("@appointment_date", appointmentDate.Date),
                Db.P("@start_time", SqlTime(startTime)),
                Db.P("@end_time", SqlTime(endTime)),
                Db.P("@exclude_id", excludeAppointmentId));

            return Convert.ToInt32(count) > 0;
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

        /// <summary>
        /// The method and the reference of the last payment of an appointment.
        /// WHY verified is always true here: the money is already recorded on this booking, so a
        /// reschedule only re-checks the amount and the reference and must not ask for the
        /// "verified" tick all over again.
        /// </summary>
        private static void ReadPaymentInfo(int appointmentId, out string method, out string reference, out bool verified)
        {
            method = "Cash";
            reference = string.Empty;
            verified = true;

            DataTable table = Db.Query(
@"SELECT TOP 1 p.payment_method, p.reference_number
FROM Payments p
INNER JOIN Billing b ON b.billing_id = p.billing_id
WHERE b.appointment_id = @appointment_id AND p.verification_status <> 'Rejected'
ORDER BY p.payment_id DESC",
                Db.P("@appointment_id", appointmentId));

            if (table.Rows.Count == 0) return;

            DataRow row = table.Rows[0];
            method = Db.Get<string>(row["payment_method"], "Cash");
            reference = Db.Get<string>(row["reference_number"]);
        }

        /// <summary>True when the payment method needs a reference number and a verified tick.</summary>
        private static bool NeedsReference(string paymentMethod)
        {
            return string.Equals(paymentMethod, "GCash", StringComparison.OrdinalIgnoreCase)
                || string.Equals(paymentMethod, "Bank Transfer", StringComparison.OrdinalIgnoreCase);
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
