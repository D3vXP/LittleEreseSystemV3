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
    }
}
