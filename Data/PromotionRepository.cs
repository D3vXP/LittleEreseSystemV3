using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about Promotions lives here.
    /// Promotions are never deleted: an old one is set to 'Inactive' or swept to 'Expired'.
    /// </summary>
    internal static class PromotionRepository
    {
        /// <summary>
        /// Moves every promotion whose end date has passed to status 'Expired'.
        /// WHY: the status is stored, so reports and the dashboard can trust a single word.
        /// It is called once every time the Promotions screen opens.
        /// </summary>
        public static int RefreshExpired()
        {
            return Db.Execute(
                "UPDATE Promotions SET status = 'Expired' " +
                "WHERE status = 'Active' AND end_date < CAST(GETDATE() AS date)");
        }

        /// <summary>
        /// The promotions for the grid.
        ///   search - matches the promotion name (empty = no search)
        ///   status - "All" means "every status"
        /// Returns promo_id, promo_name, discount_type, discount_value, start_date, end_date,
        /// status and description (the conditions shown in the grid).
        /// </summary>
        public static DataTable GetPromotions(string search, string status)
        {
            string sql =
                "SELECT promo_id, promo_name, discount_type, discount_value, start_date, end_date, " +
                "       status, description " +
                "FROM Promotions " +
                "WHERE (@search = '' OR promo_name LIKE @like) " +
                "  AND (@status = 'All' OR status = @status) " +
                "ORDER BY start_date DESC, promo_name";

            string term = search == null ? string.Empty : search.Trim();

            return Db.Query(sql,
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@status", status ?? "All"));
        }

        /// <summary>One promotion row, or null when the id no longer exists.</summary>
        public static DataRow GetById(int promoId)
        {
            DataTable table = Db.Query(
                "SELECT promo_id, promo_name, description, discount_type, discount_value, " +
                "       start_date, end_date, status " +
                "FROM Promotions WHERE promo_id = @promo_id",
                Db.P("@promo_id", promoId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// The promotions a Staff user may see and the Billing / Appointments screens may apply:
        /// status 'Active' and today between start_date and end_date.
        /// </summary>
        public static DataTable GetActiveValid()
        {
            return Db.Query(
                "SELECT promo_id, promo_name, discount_type, discount_value, start_date, end_date, " +
                "       status, description " +
                "FROM Promotions " +
                "WHERE status = 'Active' " +
                "  AND CAST(GETDATE() AS date) BETWEEN start_date AND end_date " +
                "ORDER BY end_date, promo_name");
        }

        /// <summary>True when another promotion already uses this name (names are unique).</summary>
        public static bool PromoNameExists(string promoName, int excludePromoId)
        {
            if (string.IsNullOrWhiteSpace(promoName)) return false;

            object count = Db.Scalar(
                "SELECT COUNT(*) FROM Promotions WHERE promo_name = @promo_name AND promo_id <> @exclude_id",
                Db.P("@promo_name", promoName.Trim()),
                Db.P("@exclude_id", excludePromoId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>Inserts a promotion and returns its new promo_id.</summary>
        public static int Add(string promoName, string description, string discountType, decimal discountValue,
            DateTime startDate, DateTime endDate, string status)
        {
            string name = promoName == null ? string.Empty : promoName.Trim();

            // Kept here as well as in the form so no caller can create a duplicate.
            if (PromoNameExists(name, 0))
            {
                throw new InvalidOperationException("A promotion with this name already exists.");
            }

            object id = Db.Scalar(
                "INSERT INTO Promotions (promo_name, description, discount_type, discount_value, " +
                "                        start_date, end_date, status) " +
                "VALUES (@promo_name, @description, @discount_type, @discount_value, " +
                "        @start_date, @end_date, @status); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@promo_name", name),
                Db.P("@description", description),
                Db.P("@discount_type", discountType),
                Db.P("@discount_value", discountValue),
                Db.P("@start_date", startDate.Date),
                Db.P("@end_date", endDate.Date),
                Db.P("@status", status));

            return Convert.ToInt32(id);
        }

        /// <summary>Updates a promotion. The promo_id is not changed.</summary>
        public static void Update(int promoId, string promoName, string description, string discountType,
            decimal discountValue, DateTime startDate, DateTime endDate, string status)
        {
            string name = promoName == null ? string.Empty : promoName.Trim();

            if (PromoNameExists(name, promoId))
            {
                throw new InvalidOperationException("A promotion with this name already exists.");
            }

            Db.Execute(
                "UPDATE Promotions SET promo_name = @promo_name, description = @description, " +
                "       discount_type = @discount_type, discount_value = @discount_value, " +
                "       start_date = @start_date, end_date = @end_date, status = @status " +
                "WHERE promo_id = @promo_id",
                Db.P("@promo_name", name),
                Db.P("@description", description),
                Db.P("@discount_type", discountType),
                Db.P("@discount_value", discountValue),
                Db.P("@start_date", startDate.Date),
                Db.P("@end_date", endDate.Date),
                Db.P("@status", status),
                Db.P("@promo_id", promoId));
        }

        /// <summary>Activates ('Active') or deactivates ('Inactive') a promotion.</summary>
        public static void SetStatus(int promoId, string status)
        {
            Db.Execute(
                "UPDATE Promotions SET status = @status WHERE promo_id = @promo_id",
                Db.P("@status", status),
                Db.P("@promo_id", promoId));
        }
    }
}
