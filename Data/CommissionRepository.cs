using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Reads the staff commissions and marks them as paid.
    /// The money rule itself lives in BillingRepository.Checkout (10 % of the line's net amount);
    /// this class only groups and updates what was already written there.
    /// </summary>
    internal class CommissionRepository
    {
        /// <summary>
        /// One row per staff member: user_id, full_name, services_count, total_commission,
        /// unpaid_amount and paid_amount.
        /// userId 0 means every staff member. payout is All / Unpaid / Paid.
        /// The date filter uses the appointment date of the commission's service line, because a
        /// commission belongs to the day the service was performed, never to the payout day.
        /// </summary>
        public static DataTable GetSummary(DateTime from, DateTime to, int userId, string payout)
        {
            string payoutFilter = NormalizePayout(payout);

            return Db.Query(
@"SELECT u.user_id, u.full_name,
         COUNT(*) AS services_count,
         SUM(c.commission_amount) AS total_commission,
         SUM(CASE WHEN c.payout_status = 'Paid' THEN 0 ELSE c.commission_amount END) AS unpaid_amount,
         SUM(CASE WHEN c.payout_status = 'Paid' THEN c.commission_amount ELSE 0 END) AS paid_amount
FROM Commissions c
INNER JOIN Users u ON u.user_id = c.user_id
INNER JOIN AppointmentServices asv ON asv.appointment_service_id = c.appointment_service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE a.appointment_date >= @from_date
  AND a.appointment_date <= @to_date
  AND (@user_id = 0 OR c.user_id = @user_id)
  AND (@payout = 'All' OR c.payout_status = @payout)
GROUP BY u.user_id, u.full_name
ORDER BY u.full_name",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date),
                Db.P("@user_id", userId),
                Db.P("@payout", payoutFilter));
        }

        /// <summary>
        /// The single commission rows behind the summary: commission_id, service_date, staff_name,
        /// client_name, service_name, base_amount, commission_rate, commission_amount and
        /// payout_status.
        /// base_amount is read back from the stored amount and rate (amount * 100 / rate) so the
        /// screen shows exactly what was paid out, not a re-calculation of today's prices.
        /// </summary>
        public static DataTable GetDetails(DateTime from, DateTime to, int userId, string payout)
        {
            string payoutFilter = NormalizePayout(payout);

            return Db.Query(
@"SELECT c.commission_id, a.appointment_date AS service_date,
         u.full_name AS staff_name,
         cl.last_name + ', ' + cl.first_name AS client_name,
         s.service_name,
         CASE WHEN c.commission_rate > 0
              THEN ROUND(c.commission_amount * 100.0 / c.commission_rate, 2)
              ELSE c.commission_amount END AS base_amount,
         c.commission_rate, c.commission_amount, c.payout_status
FROM Commissions c
INNER JOIN Users u ON u.user_id = c.user_id
INNER JOIN AppointmentServices asv ON asv.appointment_service_id = c.appointment_service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
INNER JOIN Clients cl ON cl.client_id = a.client_id
INNER JOIN Services s ON s.service_id = asv.service_id
WHERE a.appointment_date >= @from_date
  AND a.appointment_date <= @to_date
  AND (@user_id = 0 OR c.user_id = @user_id)
  AND (@payout = 'All' OR c.payout_status = @payout)
ORDER BY a.appointment_date DESC, c.commission_id DESC",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date),
                Db.P("@user_id", userId),
                Db.P("@payout", payoutFilter));
        }

        /// <summary>
        /// Marks the chosen commissions as paid and stamps the payout date. Rows that are already
        /// Paid are left alone, so a double click can never overwrite an earlier payout date.
        /// Returns how many rows really changed.
        /// </summary>
        public static int MarkPaid(List<int> commissionIds)
        {
            if (commissionIds == null || commissionIds.Count == 0) return 0;

            List<SqlParameter> parameters = new List<SqlParameter>();
            string[] placeholders = new string[commissionIds.Count];

            for (int i = 0; i < commissionIds.Count; i++)
            {
                placeholders[i] = "@id" + i;
                parameters.Add(Db.P(placeholders[i], commissionIds[i]));
            }

            string inList = string.Join(", ", placeholders);

            return Db.Execute(
                "UPDATE Commissions SET payout_status = 'Paid', payout_date = GETDATE() " +
                "WHERE payout_status = 'Unpaid' AND commission_id IN (" + inList + ")",
                parameters.ToArray());
        }

        /// <summary>Only the three words the screen offers are passed on to SQL.</summary>
        private static string NormalizePayout(string payout)
        {
            if (string.Equals(payout, "Unpaid", StringComparison.OrdinalIgnoreCase)) return "Unpaid";
            if (string.Equals(payout, "Paid", StringComparison.OrdinalIgnoreCase)) return "Paid";
            return "All";
        }
    }
}
