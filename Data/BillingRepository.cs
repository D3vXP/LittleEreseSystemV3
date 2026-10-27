using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about the bill of one appointment and the payments made on it.
    /// An appointment has exactly ONE bill, so the billing row is created together with the
    /// appointment (the same transaction).
    ///
    /// The methods that take a SqlConnection + SqlTransaction are used from inside a booking /
    /// cancel transaction (see Db.InTransaction), so the appointment, its bill and its payment
    /// are saved as one unit of work.
    /// </summary>
    internal static class BillingRepository
    {
        /// <summary>
        /// Creates the bill of an appointment and returns the new billing_id.
        /// Runs inside the caller's transaction, so the bill can never exist without its appointment.
        /// </summary>
        public static int CreateBilling(SqlConnection connection, SqlTransaction transaction, int appointmentId,
            decimal subtotal, int promoId, decimal discountAmount, decimal totalAmount,
            decimal downpayment, string downpaymentStatus, decimal balanceDue, string paymentStatus)
        {
            object id = Db.Scalar(connection, transaction,
                "INSERT INTO Billing (appointment_id, promo_id, subtotal, discount_amount, downpayment, " +
                "                     downpayment_status, total_amount, balance_due, payment_status) " +
                "VALUES (@appointment_id, @promo_id, @subtotal, @discount_amount, @downpayment, " +
                "        @downpayment_status, @total_amount, @balance_due, @payment_status); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@appointment_id", appointmentId),
                Db.P("@promo_id", promoId > 0 ? (object)promoId : null),
                Db.P("@subtotal", subtotal),
                Db.P("@discount_amount", discountAmount),
                Db.P("@downpayment", downpayment),
                Db.P("@downpayment_status", downpaymentStatus),
                Db.P("@total_amount", totalAmount),
                Db.P("@balance_due", balanceDue),
                Db.P("@payment_status", paymentStatus));

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Records one payment of a bill and refreshes the balance / payment status of that bill,
        /// so those two columns always agree with the Payments table.
        /// paymentType is 'Downpayment' (a deposit) or 'Final' (a promo needs full payment upfront).
        /// </summary>
        public static int AddPayment(SqlConnection connection, SqlTransaction transaction, int billingId,
            int receivedBy, string paymentType, string paymentMethod, string referenceNumber,
            decimal amountPaid, string verificationStatus)
        {
            object id = Db.Scalar(connection, transaction,
                "INSERT INTO Payments (billing_id, received_by, payment_type, payment_method, " +
                "                      reference_number, amount_paid, verification_status) " +
                "VALUES (@billing_id, @received_by, @payment_type, @payment_method, " +
                "        @reference_number, @amount_paid, @verification_status); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@billing_id", billingId),
                Db.P("@received_by", receivedBy),
                Db.P("@payment_type", paymentType),
                Db.P("@payment_method", paymentMethod),
                Db.P("@reference_number", string.IsNullOrWhiteSpace(referenceNumber) ? null : referenceNumber.Trim()),
                Db.P("@amount_paid", amountPaid),
                Db.P("@verification_status", verificationStatus));

            RefreshBalance(connection, transaction, billingId);

            return Convert.ToInt32(id);
        }

        /// <summary>One bill row with its paid total, or null when the appointment has no bill yet.</summary>
        public static DataRow GetByAppointment(int appointmentId)
        {
            DataTable table = Db.Query(
                "SELECT b.billing_id, b.appointment_id, b.promo_id, b.subtotal, b.discount_amount, " +
                "       b.downpayment, b.downpayment_status, b.total_amount, b.balance_due, " +
                "       b.payment_status, p.promo_name, " +
                "       ISNULL(paid.paid_amount, 0) AS paid_amount " +
                "FROM Billing b " +
                "LEFT JOIN Promotions p ON p.promo_id = b.promo_id " +
                "LEFT JOIN (SELECT billing_id, SUM(amount_paid) AS paid_amount FROM Payments " +
                "            WHERE verification_status <> 'Rejected' GROUP BY billing_id) paid " +
                "       ON paid.billing_id = b.billing_id " +
                "WHERE b.appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// Re-computes balance_due and payment_status of one bill from its payments.
        /// WHY it lives here: the rule "balance = total minus every payment that was not rejected"
        /// must be written down exactly once.
        /// </summary>
        public static void RefreshBalance(SqlConnection connection, SqlTransaction transaction, int billingId)
        {
            Db.Execute(connection, transaction,
                "UPDATE Billing SET " +
                "       balance_due = CASE WHEN total_amount - ISNULL(paid.paid_amount, 0) < 0 " +
                "                           THEN 0 " +
                "                           ELSE total_amount - ISNULL(paid.paid_amount, 0) END, " +
                "       payment_status = CASE WHEN ISNULL(paid.paid_amount, 0) <= 0 THEN 'Unpaid' " +
                "                             WHEN total_amount - ISNULL(paid.paid_amount, 0) <= 0 THEN 'Paid' " +
                "                             ELSE 'Partial' END, " +
                "       updated_at = GETDATE() " +
                "FROM Billing b " +
                "LEFT JOIN (SELECT billing_id, SUM(amount_paid) AS paid_amount FROM Payments " +
                "            WHERE verification_status <> 'Rejected' GROUP BY billing_id) paid " +
                "       ON paid.billing_id = b.billing_id " +
                "WHERE b.billing_id = @billing_id",
                Db.P("@billing_id", billingId));
        }

        // ---------------------------------------------------------------------
        // READ: the checkout queue, one bill, the transactions list and the receipt
        // ---------------------------------------------------------------------

        /// <summary>
        /// The queue of one day: every appointment that is not cancelled, ready for the grid.
        /// Returns appointment_id, time_range ("10:00 AM - 11:30 AM"), client_name, services
        /// (comma separated, with " x2" for a doubled service), status, payment_status and
        /// balance_due.
        /// A past day is allowed on purpose: a forgotten appointment must still be paid out.
        /// The comma list is built in C#, so the app needs no STRING_AGG (same reason as
        /// AppointmentRepository.GetByDate).
        /// </summary>
        public static DataTable GetQueue(DateTime date)
        {
            DataTable table = Db.Query(
@"SELECT a.appointment_id, a.start_time, a.end_time,
         c.last_name + ', ' + c.first_name AS client_name,
         a.status,
         ISNULL(b.payment_status, 'Unpaid') AS payment_status,
         ISNULL(b.balance_due, 0) AS balance_due
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
LEFT JOIN Billing b ON b.appointment_id = a.appointment_id
WHERE a.appointment_date = @appointment_date AND a.status <> 'Cancelled'
ORDER BY a.start_time, a.appointment_id",
                Db.P("@appointment_date", date.Date));

            Dictionary<int, string> services = NamesForDate(date);

            table.Columns.Add("time_range", typeof(string));
            table.Columns.Add("services", typeof(string));

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);

                TimeSpan start = ReadTime(row["start_time"]);
                TimeSpan end = ReadTime(row["end_time"]);
                row["time_range"] = Clock(start) + " - " + Clock(end);

                string names;
                row["services"] = services.TryGetValue(appointmentId, out names) ? names : "-";
            }

            // The raw time columns are not shown anywhere, so they do not travel to the grid.
            table.Columns.Remove("start_time");
            table.Columns.Remove("end_time");

            return table;
        }

        /// <summary>
        /// The bill of one appointment for the bill card: billing_id, client_name,
        /// appointment_date, start_time, end_time, appointment_status, promo_id, promo_name,
        /// subtotal, discount_amount, total_amount, paid_so_far (every payment that was not
        /// rejected), balance_due and payment_status.
        /// Returns null when the appointment has no bill (or no longer exists).
        /// </summary>
        public static DataRow GetBillHeader(int appointmentId)
        {
            DataTable table = Db.Query(
@"SELECT b.billing_id, b.appointment_id, b.promo_id, b.subtotal, b.discount_amount,
         b.total_amount, b.balance_due, b.payment_status,
         ISNULL(paid.paid_amount, 0) AS paid_so_far,
         a.appointment_date, a.start_time, a.end_time,
         a.status AS appointment_status,
         c.last_name + ', ' + c.first_name AS client_name,
         ISNULL(p.promo_name, '') AS promo_name
FROM Appointments a
INNER JOIN Clients c ON c.client_id = a.client_id
INNER JOIN Billing b ON b.appointment_id = a.appointment_id
LEFT JOIN Promotions p ON p.promo_id = b.promo_id
LEFT JOIN (SELECT billing_id, SUM(amount_paid) AS paid_amount FROM Payments
           WHERE verification_status <> 'Rejected' GROUP BY billing_id) paid
       ON paid.billing_id = b.billing_id
WHERE a.appointment_id = @appointment_id",
                Db.P("@appointment_id", appointmentId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// The service lines of one appointment for the bill card: appointment_service_id,
        /// service_name, quantity, price, session_label ("2 / 8" or empty),
        /// technician_id (0 when no technician is requested) and line_status.
        /// Cancelled lines are left out, because nobody pays for them.
        /// </summary>
        public static DataTable GetBillLines(int appointmentId)
        {
            return Db.Query(
@"SELECT asv.appointment_service_id, s.service_name, asv.quantity, asv.price,
         CASE WHEN asv.session_number IS NOT NULL AND asv.total_sessions IS NOT NULL
              THEN CAST(asv.session_number AS varchar(10)) + ' / ' + CAST(asv.total_sessions AS varchar(10))
              ELSE '' END AS session_label,
         ISNULL(ast.user_id, 0) AS technician_id,
         asv.status AS line_status
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
LEFT JOIN AppointmentStaff ast ON ast.appointment_service_id = asv.appointment_service_id
                               AND ast.status <> 'Reassigned'
WHERE asv.appointment_id = @appointment_id AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_id", appointmentId));
        }

        /// <summary>
        /// The payments list of the Transactions tab: one row per bill that has at least one
        /// payment that was not rejected.
        /// Returns billing_id, payment_date (the latest payment), client_name, services,
        /// total_amount, amount_paid (the sum of those payments), payment_method (of the latest
        /// payment), reference_number, staff_names and payment_status.
        /// method "All" means every method; the dates are compared against the latest payment.
        /// </summary>
        public static DataTable GetTransactions(DateTime from, DateTime to, string search, string method)
        {
            string term = search == null ? string.Empty : search.Trim();
            string selectedMethod = string.IsNullOrEmpty(method) ? "All" : method;

            DataTable table = Db.Query(
@"SELECT b.billing_id, b.total_amount, b.payment_status,
         c.last_name + ', ' + c.first_name AS client_name,
         paid.paid_amount AS amount_paid,
         last_pay.payment_date, last_pay.payment_method, last_pay.reference_number,
         a.appointment_id
FROM Billing b
INNER JOIN Appointments a ON a.appointment_id = b.appointment_id
INNER JOIN Clients c ON c.client_id = a.client_id
INNER JOIN (SELECT billing_id, SUM(amount_paid) AS paid_amount FROM Payments
            WHERE verification_status <> 'Rejected' GROUP BY billing_id) paid
       ON paid.billing_id = b.billing_id
OUTER APPLY (SELECT TOP 1 p.payment_date, p.payment_method, p.reference_number
             FROM Payments p
             WHERE p.billing_id = b.billing_id AND p.verification_status <> 'Rejected'
             ORDER BY p.payment_date DESC, p.payment_id DESC) last_pay
WHERE last_pay.payment_date >= @from_date
  AND last_pay.payment_date < @to_date
  AND (@search = ''
       OR (c.last_name + ', ' + c.first_name) LIKE @like
       OR c.first_name LIKE @like
       OR c.last_name LIKE @like)
  AND (@method = 'All' OR last_pay.payment_method = @method)
ORDER BY last_pay.payment_date DESC, b.billing_id DESC",
                Db.P("@from_date", from.Date),
                // The whole "to" day belongs to the range, so the next day is the boundary.
                Db.P("@to_date", to.Date.AddDays(1)),
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@method", selectedMethod));

            Dictionary<int, string> services = NamesForRows(table, "appointment_id", false);
            Dictionary<int, string> staff = NamesForRows(table, "appointment_id", true);

            table.Columns.Add("services", typeof(string));
            table.Columns.Add("staff_names", typeof(string));

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);

                string names;
                row["services"] = services.TryGetValue(appointmentId, out names) ? names : "-";

                string people;
                row["staff_names"] = staff.TryGetValue(appointmentId, out people) ? people : "Unassigned";
            }

            table.Columns.Remove("appointment_id");
            return table;
        }

        /// <summary>
        /// Everything the printed receipt needs, one row per service line (so the header fields
        /// repeat on every row): billing_id, client_name, appointment_date, payment_date,
        /// payment_method, reference_number, staff_names, subtotal, discount_amount,
        /// total_amount, amount_paid, balance_due, service_name, quantity and price.
        /// A bill without lines returns an empty table.
        /// </summary>
        public static DataTable GetReceiptData(int billingId)
        {
            DataTable table = Db.Query(
@"SELECT b.billing_id, b.subtotal, b.discount_amount, b.total_amount, b.balance_due,
         ISNULL(paid.paid_amount, 0) AS amount_paid,
         c.last_name + ', ' + c.first_name AS client_name,
         a.appointment_id, a.appointment_date,
         last_pay.payment_date, last_pay.payment_method, last_pay.reference_number,
         asv.quantity, asv.price, s.service_name
FROM Billing b
INNER JOIN Appointments a ON a.appointment_id = b.appointment_id
INNER JOIN Clients c ON c.client_id = a.client_id
INNER JOIN AppointmentServices asv ON asv.appointment_id = a.appointment_id
                                  AND asv.status <> 'Cancelled'
INNER JOIN Services s ON s.service_id = asv.service_id
LEFT JOIN (SELECT billing_id, SUM(amount_paid) AS paid_amount FROM Payments
           WHERE verification_status <> 'Rejected' GROUP BY billing_id) paid
       ON paid.billing_id = b.billing_id
OUTER APPLY (SELECT TOP 1 p.payment_date, p.payment_method, p.reference_number
             FROM Payments p
             WHERE p.billing_id = b.billing_id AND p.verification_status <> 'Rejected'
             ORDER BY p.payment_date DESC, p.payment_id DESC) last_pay
WHERE b.billing_id = @billing_id
ORDER BY asv.appointment_service_id",
                Db.P("@billing_id", billingId));

            Dictionary<int, string> staff = NamesForRows(table, "appointment_id", true);

            table.Columns.Add("staff_names", typeof(string));

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);

                string people;
                row["staff_names"] = staff.TryGetValue(appointmentId, out people) ? people : "-";
            }

            table.Columns.Remove("appointment_id");
            return table;
        }

        // ---------------------------------------------------------------------
        // WRITE: the promotion, then the checkout
        // ---------------------------------------------------------------------

        /// <summary>
        /// Applies a promotion to the bill of an appointment and returns an empty text on success,
        /// otherwise a sentence a staff member can understand.
        /// Allowed only when the promotion is valid today, the bill has no promotion yet and
        /// nothing has been paid (a promotion is never an installment). The discount, the total
        /// and the balance are recalculated here.
        /// The discount ALWAYS comes from PromotionRepository.CalculateDiscount.
        /// </summary>
        public static string ApplyPromo(int appointmentId, int promoId)
        {
            if (promoId <= 0) return "Choose a promotion first.";

            DataRow bill = GetBillHeader(appointmentId);
            if (bill == null) return "This appointment has no bill yet.";

            if (Db.Get<int>(bill["promo_id"]) > 0) return "This bill already has a promotion.";

            if (Db.Get<decimal>(bill["paid_so_far"]) > 0m)
            {
                return "A promotion can only be applied before any payment is made.";
            }

            string status = Db.Get<string>(bill["appointment_status"]);
            if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                return "This appointment is already completed.";
            }

            string message;
            decimal discount = PromotionRepository.CalculateDiscount(promoId, PromoLines(appointmentId), out message);
            if (!string.IsNullOrEmpty(message)) return message;
            if (discount <= 0m) return "This promotion gives no discount on this bill.";

            decimal subtotal = Db.Get<decimal>(bill["subtotal"]);
            decimal total = subtotal - discount;
            if (total < 0m) total = 0m;

            int billingId = Db.Get<int>(bill["billing_id"]);

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                Db.Execute(connection, transaction,
@"UPDATE Billing
SET promo_id = @promo_id, discount_amount = @discount_amount, total_amount = @total_amount,
    balance_due = @total_amount, updated_at = GETDATE()
WHERE billing_id = @billing_id",
                    Db.P("@promo_id", promoId),
                    Db.P("@discount_amount", discount),
                    Db.P("@total_amount", total),
                    Db.P("@billing_id", billingId));
            });

            ActivityRepository.Log(Helpers.Session.UserId, "Apply Promo", "Billing",
                "Promotion #" + promoId + " applied to bill #" + billingId + " (-" + Money.Text(discount) + ")");

            return string.Empty;
        }

        /// <summary>
        /// Pays out and closes one appointment, all in ONE transaction, and returns the billing_id.
        ///   technicianByLine - appointment_service_id -> user_id of the attending staff
        ///   method           - Cash / GCash / Bank Transfer (only needed while money is due)
        ///   reference        - the GCash / bank reference (needed for those two methods)
        ///   verified         - the "I checked it in the app" tick (same two methods)
        ///   tendered         - the cash handed over (must reach the balance for Cash)
        /// Besides the payment it completes the staff rows, the service lines, the appointment,
        /// the package sessions, the commissions and the client's treatment history.
        /// Throws InvalidOperationException with a readable message when the checkout is not allowed.
        /// </summary>
        public static int Checkout(int appointmentId, Dictionary<int, int> technicianByLine,
            string method, string reference, bool verified, decimal tendered)
        {
            DataRow appointment = AppointmentRepository.GetById(appointmentId);
            if (appointment == null) throw new InvalidOperationException("This appointment no longer exists.");

            string oldStatus = Db.Get<string>(appointment["status"]);
            if (string.Equals(oldStatus, "Cancelled", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(oldStatus, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This appointment is already " + oldStatus.ToLowerInvariant() +
                    ", so it cannot be checked out.");
            }

            DateTime appointmentDate = Db.Get<DateTime>(appointment["appointment_date"]);
            if (appointmentDate.Date > DateTime.Today)
            {
                throw new InvalidOperationException(
                    "This appointment is on a future date, so it cannot be checked out yet.");
            }

            DataRow bill = GetBillHeader(appointmentId);
            if (bill == null) throw new InvalidOperationException("This appointment has no bill yet.");

            int billingId = Db.Get<int>(bill["billing_id"]);
            int clientId = Db.Get<int>(appointment["client_id"]);
            decimal subtotal = Db.Get<decimal>(bill["subtotal"]);
            decimal discount = Db.Get<decimal>(bill["discount_amount"]);
            decimal balance = Db.Get<decimal>(bill["balance_due"]);

            DataTable lines = GetBillLines(appointmentId);
            if (lines.Rows.Count == 0)
            {
                throw new InvalidOperationException("This appointment has no services to complete.");
            }

            if (technicianByLine == null) technicianByLine = new Dictionary<int, int>();

            // Every service needs the staff member who performed it, because the commission and
            // the treatment history are based on that person.
            foreach (DataRow row in lines.Rows)
            {
                int lineId = Db.Get<int>(row["appointment_service_id"]);

                int technicianId;
                if (!technicianByLine.TryGetValue(lineId, out technicianId) || technicianId <= 0)
                {
                    throw new InvalidOperationException(
                        "Give every service line an attending staff member first.");
                }
            }

            string paymentMethod = method == null ? string.Empty : method.Trim();
            string paymentReference = reference == null ? string.Empty : reference.Trim();

            if (balance > 0m)
            {
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    throw new InvalidOperationException("Choose a payment method for the balance of " +
                        Money.Text(balance) + ".");
                }

                if (NeedsReference(paymentMethod))
                {
                    if (string.IsNullOrWhiteSpace(paymentReference))
                    {
                        throw new InvalidOperationException("Type the " + paymentMethod + " reference number.");
                    }

                    if (!verified)
                    {
                        throw new InvalidOperationException(
                            "Tick 'I verified this payment in the GCash / bank app' before saving a " +
                            paymentMethod + " payment.");
                    }
                }
                else if (tendered < balance)
                {
                    throw new InvalidOperationException("The amount tendered does not cover the balance of " +
                        Money.Text(balance) + ".");
                }
            }

            int userId = Helpers.Session.UserId;
            int paymentId = 0;

            Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
            {
                // (a) The money that is still due. A 'Final' payment closes the bill.
                if (balance > 0m)
                {
                    object id = Db.Scalar(connection, transaction,
@"INSERT INTO Payments (billing_id, received_by, payment_type, payment_method, reference_number,
                       amount_paid, verification_status)
VALUES (@billing_id, @received_by, 'Final', @payment_method, @reference_number,
        @amount_paid, 'Verified');
SELECT CAST(SCOPE_IDENTITY() AS int);",
                        Db.P("@billing_id", billingId),
                        Db.P("@received_by", userId),
                        Db.P("@payment_method", paymentMethod),
                        Db.P("@reference_number", string.IsNullOrWhiteSpace(paymentReference)
                            ? null
                            : paymentReference),
                        Db.P("@amount_paid", balance));

                    paymentId = Convert.ToInt32(id);
                }

                // (b) The bill is settled: nothing is due and the deposit (if any) was used.
                Db.Execute(connection, transaction,
@"UPDATE Billing
SET balance_due = 0, payment_status = 'Paid',
    downpayment_status = CASE WHEN downpayment_status = 'Held' THEN 'Applied' ELSE downpayment_status END,
    updated_at = GETDATE()
WHERE billing_id = @billing_id",
                    Db.P("@billing_id", billingId));

                // (c) The attending staff of every line. The requested technician is replaced by
                // the person who really did the service, and the row is marked 'Completed'.
                foreach (DataRow row in lines.Rows)
                {
                    int lineId = Db.Get<int>(row["appointment_service_id"]);
                    int technicianId = technicianByLine[lineId];

                    int affected = Db.Execute(connection, transaction,
@"UPDATE AppointmentStaff SET user_id = @user_id, status = 'Completed'
WHERE appointment_service_id = @appointment_service_id AND status <> 'Reassigned'",
                        Db.P("@user_id", technicianId),
                        Db.P("@appointment_service_id", lineId));

                    if (affected == 0)
                    {
                        Db.Execute(connection, transaction,
@"INSERT INTO AppointmentStaff (appointment_id, appointment_service_id, user_id, status)
VALUES (@appointment_id, @appointment_service_id, @user_id, 'Completed')",
                            Db.P("@appointment_id", appointmentId),
                            Db.P("@appointment_service_id", lineId),
                            Db.P("@user_id", technicianId));
                    }
                }

                // (d) Every service of the appointment is done.
                Db.Execute(connection, transaction,
                    "UPDATE AppointmentServices SET status = 'Completed' " +
                    "WHERE appointment_id = @appointment_id AND status <> 'Cancelled'",
                    Db.P("@appointment_id", appointmentId));

                // (e) The appointment itself.
                Db.Execute(connection, transaction,
@"UPDATE Appointments SET status = 'Completed', updated_at = GETDATE()
WHERE appointment_id = @appointment_id",
                    Db.P("@appointment_id", appointmentId));

                // The line details the commissions and the treatment history need. The package
                // size and the whole package price live in Services.
                DataTable lineInfo = Db.Query(connection, transaction,
@"SELECT asv.appointment_service_id, asv.quantity, asv.price,
         ISNULL(asv.package_id, 0) AS package_id,
         ISNULL(asv.total_sessions, 0) AS total_sessions,
         s.price AS service_price, s.service_name
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
WHERE asv.appointment_id = @appointment_id AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id",
                    Db.P("@appointment_id", appointmentId));

                List<string> treatmentLines = new List<string>();

                foreach (DataRow info in lineInfo.Rows)
                {
                    int lineId = Db.Get<int>(info["appointment_service_id"]);
                    int technicianId = technicianByLine.ContainsKey(lineId) ? technicianByLine[lineId] : 0;

                    // (g) Commission: 10 % of the line's net amount, so the proportional share of
                    // the discount is taken off first. A package line earns 10 % of the whole
                    // package price divided by its number of sessions, because later sessions
                    // of that package cost 0.00.
                    decimal lineAmount = Db.Get<decimal>(info["price"]) * Db.Get<int>(info["quantity"], 1);
                    decimal baseAmount;

                    int packageId = Db.Get<int>(info["package_id"]);
                    int totalSessions = Db.Get<int>(info["total_sessions"]);

                    if (packageId > 0 && totalSessions > 0)
                    {
                        baseAmount = Db.Get<decimal>(info["service_price"]) / totalSessions;
                    }
                    else
                    {
                        baseAmount = lineAmount;
                        if (subtotal > 0m && discount > 0m)
                        {
                            baseAmount -= discount * (lineAmount / subtotal);
                        }
                    }

                    if (baseAmount < 0m) baseAmount = 0m;

                    decimal commission = Math.Round(baseAmount * 0.10m, 2, MidpointRounding.AwayFromZero);

                    Db.Execute(connection, transaction,
@"INSERT INTO Commissions (user_id, appointment_service_id, payment_id, commission_rate,
                          commission_amount, payout_status)
VALUES (@user_id, @appointment_service_id, @payment_id, 10.00, @commission_amount, 'Unpaid')",
                        Db.P("@user_id", technicianId),
                        Db.P("@appointment_service_id", lineId),
                        Db.P("@payment_id", paymentId > 0 ? (object)paymentId : null),
                        Db.P("@commission_amount", commission));

                    // (f) A package session was used, so one more session is done.
                    if (packageId > 0)
                    {
                        Db.Execute(connection, transaction,
@"UPDATE ClientPackages
SET sessions_completed = sessions_completed + 1
WHERE package_id = @package_id AND sessions_completed < total_sessions",
                            Db.P("@package_id", packageId));

                        Db.Execute(connection, transaction,
@"UPDATE ClientPackages SET status = 'Completed'
WHERE package_id = @package_id AND sessions_completed >= total_sessions",
                            Db.P("@package_id", packageId));
                    }

                    // (h) The treatment history entry:
                    // "MMM dd, yyyy - Service name (Staff full name)".
                    string staffName = Db.Get<string>(Db.Scalar(connection, transaction,
                        "SELECT full_name FROM Users WHERE user_id = @user_id",
                        Db.P("@user_id", technicianId)), "-");

                    treatmentLines.Add(appointmentDate.ToString("MMM dd, yyyy") + " - " +
                        Db.Get<string>(info["service_name"]) + " (" + staffName + ")");
                }

                if (treatmentLines.Count > 0)
                {
                    string existing = Db.Get<string>(Db.Scalar(connection, transaction,
                        "SELECT treatment_history FROM Clients WHERE client_id = @client_id",
                        Db.P("@client_id", clientId)));

                    string addition = string.Join(Environment.NewLine, treatmentLines.ToArray());
                    string combined = string.IsNullOrWhiteSpace(existing)
                        ? addition
                        : existing.TrimEnd() + Environment.NewLine + addition;

                    Db.Execute(connection, transaction,
                        "UPDATE Clients SET treatment_history = @treatment_history, updated_at = GETDATE() " +
                        "WHERE client_id = @client_id",
                        Db.P("@treatment_history", combined),
                        Db.P("@client_id", clientId));
                }

                // (i) The audit row of the status change.
                Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, old_status, new_status, reason)
VALUES (@appointment_id, @changed_by, 'Status Change', @old_status, 'Completed', @reason)",
                    Db.P("@appointment_id", appointmentId),
                    Db.P("@changed_by", userId),
                    Db.P("@old_status", oldStatus),
                    Db.P("@reason", "Paid out at the counter (" +
                        (balance > 0m ? paymentMethod + ", " + Money.Text(balance) : "nothing left to pay") + ")."));
            });

            // (j) The activity log. It never throws, so a failure here cannot undo the checkout.
            ActivityRepository.Log(userId, "Checkout", "Billing",
                "Appointment #" + appointmentId + " completed, bill #" + billingId +
                (balance > 0m ? ", " + paymentMethod + " " + Money.Text(balance) : ", nothing left to pay"));

            return billingId;
        }

        // ---------------------------------------------------------------------
        // PRIVATE HELPERS
        // ---------------------------------------------------------------------

        /// <summary>The service names of every appointment of one day (for the queue grid).</summary>
        private static Dictionary<int, string> NamesForDate(DateTime date)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();

            DataTable table = Db.Query(
@"SELECT asv.appointment_id,
         s.service_name + CASE WHEN asv.quantity > 1
                               THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name
FROM AppointmentServices asv
INNER JOIN Services s ON s.service_id = asv.service_id
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
WHERE a.appointment_date = @appointment_date AND asv.status <> 'Cancelled'
ORDER BY asv.appointment_service_id",
                Db.P("@appointment_date", date.Date));

            Merge(values, table);
            return values;
        }

        /// <summary>
        /// The service names (staff = false) or technician names (staff = true) of the appointments
        /// listed in <paramref name="rows"/>. One query for the whole list instead of one per row.
        /// </summary>
        private static Dictionary<int, string> NamesForRows(DataTable rows, string idColumn, bool staff)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            if (rows == null || rows.Rows.Count == 0) return values;

            List<int> ids = new List<int>();
            foreach (DataRow row in rows.Rows)
            {
                int id = Db.Get<int>(row[idColumn]);
                if (id > 0 && !ids.Contains(id)) ids.Add(id);
            }

            if (ids.Count == 0) return values;

            List<SqlParameter> parameters = new List<SqlParameter>();
            string[] placeholders = new string[ids.Count];

            for (int i = 0; i < ids.Count; i++)
            {
                placeholders[i] = "@id" + i;
                parameters.Add(Db.P(placeholders[i], ids[i]));
            }

            string inList = string.Join(", ", placeholders);

            string sql = staff
                ? "SELECT ast.appointment_id, u.full_name AS name " +
                  "FROM AppointmentStaff ast " +
                  "INNER JOIN Users u ON u.user_id = ast.user_id " +
                  "WHERE ast.status <> 'Reassigned' AND ast.appointment_id IN (" + inList + ") " +
                  "ORDER BY ast.appointment_staff_id"
                : "SELECT asv.appointment_id, " +
                  "       s.service_name + CASE WHEN asv.quantity > 1 " +
                  "                             THEN ' x' + CAST(asv.quantity AS varchar(10)) ELSE '' END AS name " +
                  "FROM AppointmentServices asv " +
                  "INNER JOIN Services s ON s.service_id = asv.service_id " +
                  "WHERE asv.status <> 'Cancelled' AND asv.appointment_id IN (" + inList + ") " +
                  "ORDER BY asv.appointment_service_id";

            Merge(values, Db.Query(sql, parameters.ToArray()));
            return values;
        }

        /// <summary>Turns "appointment_id + name" rows into one comma separated text per appointment.</summary>
        private static void Merge(Dictionary<int, string> values, DataTable table)
        {
            if (table == null) return;

            foreach (DataRow row in table.Rows)
            {
                int appointmentId = Db.Get<int>(row["appointment_id"]);
                string name = Db.Get<string>(row["name"]);
                if (string.IsNullOrEmpty(name)) continue;

                string existing;
                if (values.TryGetValue(appointmentId, out existing))
                {
                    values[appointmentId] = existing + ", " + name;
                }
                else
                {
                    values[appointmentId] = name;
                }
            }
        }

        /// <summary>
        /// The lines of an appointment in the shape the discount formula wants
        /// (service_id + amount = price x quantity). Cancelled lines are not paid for.
        /// </summary>
        private static DataTable PromoLines(int appointmentId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("service_id", typeof(int));
            table.Columns.Add("amount", typeof(decimal));

            DataTable lines = Db.Query(
                "SELECT service_id, price, quantity FROM AppointmentServices " +
                "WHERE appointment_id = @appointment_id AND status <> 'Cancelled' " +
                "ORDER BY appointment_service_id",
                Db.P("@appointment_id", appointmentId));

            foreach (DataRow row in lines.Rows)
            {
                int quantity = Db.Get<int>(row["quantity"], 1);
                if (quantity < 1) quantity = 1;

                table.Rows.Add(Db.Get<int>(row["service_id"]), Db.Get<decimal>(row["price"]) * quantity);
            }

            return table;
        }

        /// <summary>True when the payment method needs a reference number and a verified tick.</summary>
        private static bool NeedsReference(string paymentMethod)
        {
            return string.Equals(paymentMethod, "GCash", StringComparison.OrdinalIgnoreCase)
                || string.Equals(paymentMethod, "Bank Transfer", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>A time column read back safely (the provider may hand over a TimeSpan).</summary>
        private static TimeSpan ReadTime(object value)
        {
            if (value == null || value == DBNull.Value) return TimeSpan.Zero;
            if (value is TimeSpan) return (TimeSpan)value;
            if (value is DateTime) return ((DateTime)value).TimeOfDay;

            TimeSpan parsed;
            if (TimeSpan.TryParse(value.ToString(), out parsed)) return parsed;
            return TimeSpan.Zero;
        }

        /// <summary>"10:00 AM" for a time value, so every line reads the same way.</summary>
        private static string Clock(TimeSpan value)
        {
            return DateTime.Today.Add(value).ToString("h:mm tt");
        }

    }
}
