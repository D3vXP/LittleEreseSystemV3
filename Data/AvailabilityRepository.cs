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
    }
}
