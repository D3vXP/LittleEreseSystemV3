using System;
using System.Data;
using System.Data.SqlClient;

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
    }
}
