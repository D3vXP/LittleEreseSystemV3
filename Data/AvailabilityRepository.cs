using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Answers "can THIS service be done at THIS time?" for ONE service line.
    /// Every method takes (SqlConnection connection, SqlTransaction transaction) first;
    /// null is allowed.
    ///
    /// WHY: Create runs inside a transaction and a query on a second connection would wait
    /// for the rows that transaction just inserted.
    /// </summary>
    internal static class AvailabilityRepository
    {
        /// <summary>Every start time is on a 30-minute mark.</summary>
        public const int SlotMinutes = 30;

        /// <summary>
        /// When the service line stops using its room: the start plus the duration of every unit.
        /// </summary>
        public static TimeSpan LineEnd(TimeSpan start, int durationMinutes, int quantity)
        {
            return start.Add(new TimeSpan(0, durationMinutes * Math.Max(quantity, 1), 0));
        }

        /// <summary>
        /// When the technician is free again. A service may need the room longer than the person
        /// (gluta drips: 15 busy minutes, 60 room minutes), so the busy time wins when it is set
        /// and shorter than the duration.
        /// </summary>
        public static TimeSpan LineBusyEnd(TimeSpan start, int durationMinutes, int staffBusyMinutes,
            int quantity)
        {
            int perUnit = durationMinutes;

            if (staffBusyMinutes > 0 && staffBusyMinutes <= durationMinutes)
            {
                perUnit = staffBusyMinutes;
            }

            return start.Add(new TimeSpan(0, perUnit * Math.Max(quantity, 1), 0));
        }

        /// <summary>
        /// How many other service lines already hold this room while this one would run.
        /// Old rows have no line times, so ISNULL falls back to the appointment's own times.
        /// </summary>
        public static int RoomUsage(SqlConnection connection, SqlTransaction transaction, int roomId,
            DateTime date, TimeSpan start, TimeSpan end, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(DISTINCT ar.appointment_service_id)
FROM AppointmentRooms ar
INNER JOIN AppointmentServices asv ON asv.appointment_service_id = ar.appointment_service_id
INNER JOIN Appointments a ON a.appointment_id = ar.appointment_id
WHERE ar.room_id = @room_id
  AND ar.status <> 'Released'
  AND asv.status <> 'Cancelled'
  AND a.status NOT IN ('Cancelled', 'Rescheduled')
  AND a.appointment_date = @date
  AND ISNULL(asv.start_time, a.start_time) < CAST(@end AS time)
  AND ISNULL(asv.end_time, a.end_time) > CAST(@start AS time)
  AND a.appointment_id <> @exclude",
                Db.P("@room_id", roomId),
                Db.P("@date", date.Date),
                Db.P("@start", AppointmentRepository.SqlTime(start)),
                Db.P("@end", AppointmentRepository.SqlTime(end)),
                Db.P("@exclude", excludeAppointmentId));

            return Convert.ToInt32(count);
        }

        /// <summary>
        /// True when this technician is already busy with another service in that time window.
        /// The window ends at the BUSY end, because the person is free earlier than the room.
        /// </summary>
        public static bool TechnicianBusy(SqlConnection connection, SqlTransaction transaction, int userId,
            DateTime date, TimeSpan start, TimeSpan busyEnd, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(*)
FROM AppointmentStaff ast
INNER JOIN Appointments a ON a.appointment_id = ast.appointment_id
LEFT JOIN AppointmentServices asv ON asv.appointment_service_id = ast.appointment_service_id
WHERE ast.user_id = @user_id
  AND ast.status = 'Assigned'
  AND (asv.status IS NULL OR asv.status <> 'Cancelled')
  AND a.status IN ('Upcoming', 'Ongoing')
  AND a.appointment_date = @date
  AND ISNULL(asv.start_time, a.start_time) < CAST(@busy_end AS time)
  AND ISNULL(asv.busy_end_time, ISNULL(asv.end_time, a.end_time)) > CAST(@start AS time)
  AND a.appointment_id <> @exclude",
                Db.P("@user_id", userId),
                Db.P("@date", date.Date),
                Db.P("@start", AppointmentRepository.SqlTime(start)),
                Db.P("@busy_end", AppointmentRepository.SqlTime(busyEnd)),
                Db.P("@exclude", excludeAppointmentId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// True when this client already has another active service line in that time window,
        /// because one person cannot be in two treatments at once.
        /// </summary>
        public static bool ClientBusy(SqlConnection connection, SqlTransaction transaction, int clientId,
            DateTime date, TimeSpan start, TimeSpan end, int excludeAppointmentId)
        {
            object count = Db.Scalar(connection, transaction,
@"SELECT COUNT(*)
FROM AppointmentServices asv
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE a.client_id = @client_id
  AND a.status IN ('Upcoming', 'Ongoing')
  AND asv.status <> 'Cancelled'
  AND a.appointment_date = @date
  AND ISNULL(asv.start_time, a.start_time) < CAST(@end AS time)
  AND ISNULL(asv.end_time, a.end_time) > CAST(@start AS time)
  AND a.appointment_id <> @exclude",
                Db.P("@client_id", clientId),
                Db.P("@date", date.Date),
                Db.P("@start", AppointmentRepository.SqlTime(start)),
                Db.P("@end", AppointmentRepository.SqlTime(end)),
                Db.P("@exclude", excludeAppointmentId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// The reason this ONE service line cannot start at that time, or "" when it can.
        /// The first failing rule wins, so the user reads the most useful sentence first.
        /// A rule problem never throws; only a database error does.
        /// otherLines are the other lines of the booking being built (may be null).
        /// </summary>
        public static string WhyNotPossible(SqlConnection connection, SqlTransaction transaction,
            DateTime date, int serviceId, int quantity, TimeSpan start, int technicianId, int clientId,
            int excludeAppointmentId, DataTable otherLines)
        {
            // a) A past date, or today at a time that has already gone by.
            if (date.Date < DateTime.Today ||
                (date.Date == DateTime.Today && start <= DateTime.Now.TimeOfDay))
            {
                return "That time has already passed.";
            }

            DataRow service = AppointmentRepository.GetServiceInfo(connection, transaction, serviceId);
            if (service == null) return "This service no longer exists.";

            string serviceName = Db.Get<string>(service["service_name"]);
            string categoryName = Db.Get<string>(service["category_name"]);
            int categoryId = Db.Get<int>(service["category_id"]);
            int duration = Db.Get<int>(service["duration_minutes"]);
            bool massage = AppointmentRepository.IsMassageCategory(categoryName);

            if (quantity < 1) quantity = 1;

            TimeSpan end = LineEnd(start, duration, quantity);

            // b) Opening hours: massage opens later and closes later than an aesthetic service.
            TimeSpan opens = massage
                ? AppointmentRepository.MassageOpening
                : AppointmentRepository.AestheticOpening;
            TimeSpan closes = massage
                ? AppointmentRepository.MassageClosing
                : AppointmentRepository.AestheticClosing;

            if (start < opens || end > closes)
            {
                return serviceName + " can only be done between " +
                    AppointmentRepository.Clock(opens) + " and " + AppointmentRepository.Clock(closes) +
                    "; starting at " + AppointmentRepository.Clock(start) + " it would end at " +
                    AppointmentRepository.Clock(end) + ".";
            }

            // c) The shop only starts a service on the half hour.
            if (start.TotalMinutes % SlotMinutes != 0) return "Start times are on the half hour.";

            // d) The client cannot be in two places at once, neither on another appointment
            //    nor on another line of the booking that is being built right now.
            if (clientId > 0 &&
                ClientBusy(connection, transaction, clientId, date, start, end, excludeAppointmentId))
            {
                return "The client already has another service at that time.";
            }

            if (otherLines != null)
            {
                foreach (DataRow line in otherLines.Rows)
                {
                    TimeSpan lineStart = AppointmentRepository.ReadTime(line[AppointmentRepository.ColStartTime]);

                    // A line that has no time yet is not placed on the day, so it cannot overlap.
                    if (lineStart == TimeSpan.Zero) continue;

                    int lineDuration = Db.Get<int>(line[AppointmentRepository.ColDuration]);
                    int lineQuantity = Db.Get<int>(line[AppointmentRepository.ColQuantity], 1);
                    TimeSpan lineEnd = LineEnd(lineStart, lineDuration, lineQuantity);

                    if (lineStart < end && lineEnd > start)
                    {
                        return "This overlaps another service of the same appointment.";
                    }
                }
            }

            // e) At least one room of the category must still have room for one more line.
            // POLISH: when a category has no room linked in RoomServiceCategories there is no
            // room rule, otherwise no booking would be possible until the owner links the rooms.
            DataTable rooms = AppointmentRepository.GetRoomsForCategory(connection, transaction, categoryId);

            if (rooms.Rows.Count > 0)
            {
                bool roomFree = false;

                foreach (DataRow room in rooms.Rows)
                {
                    int used = RoomUsage(connection, transaction, Db.Get<int>(room["room_id"]), date,
                        start, end, excludeAppointmentId);

                    if (used < Db.Get<int>(room["capacity"], 1))
                    {
                        roomFree = true;
                        break;
                    }
                }

                if (!roomFree)
                {
                    return "No " + categoryName + " room is free from " +
                        AppointmentRepository.Clock(start) + " to " + AppointmentRepository.Clock(end) + ".";
                }
            }

            // f) The technician: a chosen one must be free, and when nobody is chosen at least
            //    one technician of the category must be free. The person is free at the BUSY end,
            //    which is shorter than the room time for a gluta drip.
            int staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);
            TimeSpan busyEnd = LineBusyEnd(start, duration, staffBusyMinutes, quantity);

            if (technicianId > 0)
            {
                if (TechnicianBusy(connection, transaction, technicianId, date, start, busyEnd,
                    excludeAppointmentId))
                {
                    return AppointmentRepository.UserName(connection, transaction, technicianId) +
                        " is busy at that time.";
                }
            }
            else
            {
                DataTable eligible = UserRepository.GetActiveStaffFor(categoryName);

                // No technician of that type exists yet, so the rule cannot be applied.
                if (eligible.Rows.Count > 0)
                {
                    bool someoneFree = false;

                    foreach (DataRow person in eligible.Rows)
                    {
                        if (!TechnicianBusy(connection, transaction, Db.Get<int>(person["user_id"]),
                            date, start, busyEnd, excludeAppointmentId))
                        {
                            someoneFree = true;
                            break;
                        }
                    }

                    if (!someoneFree)
                    {
                        return "No " + (massage ? "Massage Therapist" : "Aesthetician") +
                            " is free at that time.";
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The start times on that day where this line really fits: every 30 minutes from the
        /// category's opening to the last start that still ends before closing.
        /// A past date has no start times at all; today loses its past times through rule (a)
        /// of WhyNotPossible, so the list is only ever in the future.
        /// </summary>
        public static List<TimeSpan> GetFreeStarts(SqlConnection connection, SqlTransaction transaction,
            DateTime date, int serviceId, int quantity, int technicianId, int clientId,
            int excludeAppointmentId, DataTable otherLines)
        {
            List<TimeSpan> free = new List<TimeSpan>();

            if (date.Date < DateTime.Today) return free;

            DataRow service = AppointmentRepository.GetServiceInfo(connection, transaction, serviceId);
            if (service == null) return free;

            string categoryName = Db.Get<string>(service["category_name"]);
            int duration = Db.Get<int>(service["duration_minutes"]);
            if (duration <= 0) return free;

            bool massage = AppointmentRepository.IsMassageCategory(categoryName);
            TimeSpan opens = massage
                ? AppointmentRepository.MassageOpening
                : AppointmentRepository.AestheticOpening;
            TimeSpan closes = massage
                ? AppointmentRepository.MassageClosing
                : AppointmentRepository.AestheticClosing;

            for (TimeSpan slot = opens; slot <= closes; slot = slot.Add(new TimeSpan(0, SlotMinutes, 0)))
            {
                // The last start of the day is the one whose end is still inside the opening hours.
                if (LineEnd(slot, duration, quantity) > closes) break;

                if (WhyNotPossible(connection, transaction, date, serviceId, quantity, slot,
                    technicianId, clientId, excludeAppointmentId, otherLines) == string.Empty)
                {
                    free.Add(slot);
                }
            }

            return free;
        }

        /// <summary>
        /// The first technician of the category who is free for the busy window of this line,
        /// or 0 when the category has no eligible technician or nobody is free.
        /// </summary>
        public static int PickTechnician(SqlConnection connection, SqlTransaction transaction,
            DateTime date, int serviceId, int quantity, TimeSpan start, int excludeAppointmentId)
        {
            DataRow service = AppointmentRepository.GetServiceInfo(connection, transaction, serviceId);
            if (service == null) return 0;

            string categoryName = Db.Get<string>(service["category_name"]);
            int duration = Db.Get<int>(service["duration_minutes"]);
            int staffBusyMinutes = Db.Get<int>(service["staff_busy_minutes"], 0);

            // The person is free at the BUSY end, which is shorter than the room time for a drip.
            TimeSpan busyEnd = LineBusyEnd(start, duration, staffBusyMinutes, quantity);

            DataTable eligible = UserRepository.GetActiveStaffFor(categoryName);

            foreach (DataRow person in eligible.Rows)
            {
                int userId = Db.Get<int>(person["user_id"]);

                if (!TechnicianBusy(connection, transaction, userId, date, start, busyEnd,
                    excludeAppointmentId))
                {
                    return userId;
                }
            }

            return 0;
        }

        /// <summary>
        /// The first room of the category that still has a free seat, in the same order as
        /// AppointmentRepository.BuildRoomPlan (GetRoomsForCategory). Returns 0 and an empty
        /// name when no room is linked to the category or none is free.
        /// </summary>
        public static int PickRoom(SqlConnection connection, SqlTransaction transaction, DateTime date,
            int categoryId, TimeSpan start, TimeSpan end, int excludeAppointmentId, out string roomName)
        {
            roomName = string.Empty;

            DataTable rooms = AppointmentRepository.GetRoomsForCategory(connection, transaction, categoryId);

            foreach (DataRow room in rooms.Rows)
            {
                int roomId = Db.Get<int>(room["room_id"]);
                int capacity = Db.Get<int>(room["capacity"], 1);

                int used = RoomUsage(connection, transaction, roomId, date, start, end,
                    excludeAppointmentId);

                if (used < capacity)
                {
                    roomName = Db.Get<string>(room["room_name"]);
                    return roomId;
                }
            }

            return 0;
        }
    }
}
