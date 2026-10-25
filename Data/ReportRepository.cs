using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// The read-only queries behind the Reports page. Every method returns a DataTable whose
    /// column names match the DataPropertyName values of the Reports grids.
    ///
    /// POLISH: sales are counted when the service is completed (not when money is received), so a
    /// period can show sales that were only paid for later. Ask the owner if the cash-basis view
    /// is the one she wants.
    /// </summary>
    internal class ReportRepository
    {
        /// <summary>
        /// The money of one service line, already reduced by the line's proportional share of the
        /// bill discount. Shared by the sales, the top services and the totals, so a category, a
        /// service and the grand total always add up to the same number.
        /// WHY proportional: a discount belongs to the whole bill, so it is spread over the lines by
        /// their share of the subtotal.
        /// </summary>
        private const string LineCte =
@"WITH SalesLine AS (
    SELECT a.appointment_date, s.service_id, s.service_name, sc.category_name,
           asv.quantity * asv.price AS gross_amount,
           CASE WHEN ISNULL(b.subtotal, 0) > 0
                THEN ISNULL(b.discount_amount, 0) * (asv.quantity * asv.price) / b.subtotal
                ELSE 0 END AS discount_share
    FROM AppointmentServices asv
    INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
    INNER JOIN Services s ON s.service_id = asv.service_id
    INNER JOIN ServiceCategories sc ON sc.category_id = s.category_id
    LEFT JOIN Billing b ON b.appointment_id = a.appointment_id
    WHERE a.status = 'Completed'
      AND asv.status <> 'Cancelled'
      AND a.appointment_date >= @from_date
      AND a.appointment_date <= @to_date
)";

        /// <summary>
        /// Sales per service category: category_name, services_count, gross_sales,
        /// discount_total and net_sales.
        /// </summary>
        public static DataTable GetSalesByCategory(DateTime from, DateTime to)
        {
            return Db.Query(
LineCte +
@"
SELECT category_name,
       COUNT(*) AS services_count,
       SUM(gross_amount) AS gross_sales,
       SUM(discount_share) AS discount_total,
       SUM(gross_amount - discount_share) AS net_sales
FROM SalesLine
GROUP BY category_name
ORDER BY SUM(gross_amount - discount_share) DESC, category_name",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date));
        }

        /// <summary>
        /// The money really collected: payment_method, payments_count and amount, from the payments
        /// that were not rejected, dated by payment_date (when the money arrived).
        /// </summary>
        public static DataTable GetPaymentsByMethod(DateTime from, DateTime to)
        {
            return Db.Query(
@"SELECT p.payment_method,
         COUNT(*) AS payments_count,
         SUM(p.amount_paid) AS amount
FROM Payments p
WHERE p.verification_status <> 'Rejected'
  AND p.payment_date >= @from_date
  AND p.payment_date < @to_date_exclusive
GROUP BY p.payment_method
ORDER BY SUM(p.amount_paid) DESC, p.payment_method",
                Db.P("@from_date", from.Date),
                // The whole "to" day belongs to the range, so the next day is the boundary.
                Db.P("@to_date_exclusive", to.Date.AddDays(1)));
        }

        /// <summary>
        /// The best selling services: rank_no, service_name, category_name, times_availed and
        /// revenue (net of discounts). top = how many rows the owner wants to see.
        /// </summary>
        public static DataTable GetTopServices(DateTime from, DateTime to, int top)
        {
            int limit = top < 1 ? 10 : top;

            return Db.Query(
LineCte +
@",
Ranked AS (
    SELECT service_name, category_name,
           COUNT(*) AS times_availed,
           SUM(gross_amount - discount_share) AS revenue,
           ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC, SUM(gross_amount - discount_share) DESC, service_name) AS rank_no
    FROM SalesLine
    GROUP BY service_name, category_name
)
SELECT TOP (@top) rank_no, service_name, category_name, times_availed, revenue
FROM Ranked
ORDER BY rank_no",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date),
                Db.P("@top", limit));
        }

        /// <summary>
        /// The commission summary of every staff member for the period: full_name, services_count,
        /// total_commission, unpaid_amount and paid_amount.
        /// The same numbers as CommissionRepository.GetSummary (all staff, all payouts), so the
        /// Commissions report and the Commissions page can never disagree.
        /// </summary>
        public static DataTable GetCommissionReport(DateTime from, DateTime to)
        {
            return CommissionRepository.GetSummary(from, to, 0, "All");
        }

        /// <summary>
        /// The five counting rows of the Appointments tab: Upcoming, Ongoing, Completed, Cancelled
        /// and Rescheduled. The first four come from the appointments of the period; the last one
        /// comes from the 'Rescheduled' history rows written in the period.
        /// The rows always come back (zeros included), so the grid never looks broken.
        /// </summary>
        public static DataTable GetAppointmentStatusCounts(DateTime from, DateTime to)
        {
            DataTable counts = Db.Query(
@"SELECT a.status AS status_label, COUNT(*) AS total
FROM Appointments a
WHERE a.appointment_date >= @from_date AND a.appointment_date <= @to_date
GROUP BY a.status",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date));

            // POLISH: "Rescheduled" counts the moves MADE in the period (changed_at), not the
            // appointments that sit on a rescheduled date. Confirm which one the owner reads.
            object rescheduled = Db.Scalar(
@"SELECT COUNT(*) FROM AppointmentHistory
WHERE change_type = 'Rescheduled'
  AND changed_at >= @from_date AND changed_at < @to_date_exclusive",
                Db.P("@from_date", from.Date),
                Db.P("@to_date_exclusive", to.Date.AddDays(1)));

            DataTable table = new DataTable();
            table.Columns.Add("status_label", typeof(string));
            table.Columns.Add("total", typeof(int));

            string[] labels = { "Upcoming", "Ongoing", "Completed", "Cancelled" };
            for (int i = 0; i < labels.Length; i++)
            {
                table.Rows.Add(labels[i], CountFor(counts, labels[i]));
            }

            table.Rows.Add("Rescheduled", Db.Get<int>(rescheduled));
            return table;
        }

        /// <summary>
        /// How the bookings of the period came in: booking_source and total, cancelled bookings
        /// left out because they never happened.
        /// </summary>
        public static DataTable GetAppointmentSourceCounts(DateTime from, DateTime to)
        {
            return Db.Query(
@"SELECT a.booking_source, COUNT(*) AS total
FROM Appointments a
WHERE a.appointment_date >= @from_date
  AND a.appointment_date <= @to_date
  AND a.status <> 'Cancelled'
GROUP BY a.booking_source
ORDER BY COUNT(*) DESC, a.booking_source",
                Db.P("@from_date", from.Date),
                Db.P("@to_date", to.Date));
        }

        /// <summary>One of the counted statuses, or 0 when the period has none.</summary>
        private static int CountFor(DataTable counts, string status)
        {
            foreach (DataRow row in counts.Rows)
            {
                if (string.Equals(Db.Get<string>(row["status_label"]), status, StringComparison.OrdinalIgnoreCase))
                {
                    return Db.Get<int>(row["total"]);
                }
            }

            return 0;
        }

        /// <summary>
        /// The five numbers of the Dashboard cards for one day:
        /// today_total (every appointment that is not cancelled), remaining (Upcoming or Ongoing),
        /// completed, today_sales (the money that arrived that day) and active_packages.
        /// includeSales = false keeps sales out of the query, because Staff must not see amounts
        /// (the caller simply does not show the value then).
        /// Always returns one row with zeros on a quiet day, so the cards never show a blank.
        /// </summary>
        public static DataRow GetDashboardStats(DateTime date, bool includeSales)
        {
            string salesExpression = includeSales
                ? @"(SELECT ISNULL(SUM(p.amount_paid), 0)
       FROM Payments p
       WHERE p.verification_status <> 'Rejected'
         AND p.payment_date >= @from_date
         AND p.payment_date < @to_date)"
                : "0";

            // One SELECT with sub-queries: a single round trip gives all five numbers, and the day
            // and the sales range are separate parameters so the boundaries are never guessed.
            DataTable stats = Db.Query(
@"SELECT
    (SELECT COUNT(*)
       FROM Appointments a
      WHERE a.appointment_date = @day_date
        AND a.status <> 'Cancelled') AS today_total,
    (SELECT COUNT(*)
       FROM Appointments a
      WHERE a.appointment_date = @day_date
        AND a.status IN ('Upcoming', 'Ongoing')) AS remaining,
    (SELECT COUNT(*)
       FROM Appointments a
      WHERE a.appointment_date = @day_date
        AND a.status = 'Completed') AS completed,
    " + salesExpression + @" AS today_sales,
    (SELECT COUNT(*)
       FROM ClientPackages cp
      WHERE cp.status = 'Active') AS active_packages",
                Db.P("@day_date", date.Date),
                Db.P("@from_date", date.Date),
                Db.P("@to_date", date.Date.AddDays(1)));

            if (stats.Rows.Count == 0) stats.Rows.Add(stats.NewRow());
            return stats.Rows[0];
        }
    }
}
