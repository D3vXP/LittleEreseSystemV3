using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about Promotions lives here.
    /// Promotions are never deleted: an old one is set to 'Inactive' or swept to 'Expired'.
    /// A promotion may be linked to specific services through PromotionServices; no link means
    /// the promotion applies to the whole bill.
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
        /// Returns promo_id, promo_name, description, discount_type, discount_value, promo_price,
        /// start_date, end_date and status, plus the four columns the page shows but the database
        /// does not store: services_text, regular_total, promo_price_text and saving.
        /// </summary>
        public static DataTable GetPromotions(string search, string status)
        {
            string sql =
                "SELECT promo_id, promo_name, description, discount_type, discount_value, promo_price, " +
                "       start_date, end_date, status " +
                "FROM Promotions " +
                "WHERE (@search = '' OR promo_name LIKE @like) " +
                "  AND (@status = 'All' OR status = @status) " +
                "ORDER BY start_date DESC, promo_name";

            string term = search == null ? string.Empty : search.Trim();

            DataTable table = Db.Query(sql,
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@status", status ?? "All"));

            AddListColumns(table);
            return table;
        }

        /// <summary>One promotion row, or null when the id no longer exists.</summary>
        public static DataRow GetById(int promoId)
        {
            DataTable table = Db.Query(
                "SELECT promo_id, promo_name, description, discount_type, discount_value, " +
                "       promo_price, start_date, end_date, status " +
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
            DataTable table = Db.Query(
                "SELECT promo_id, promo_name, description, discount_type, discount_value, promo_price, " +
                "       start_date, end_date, status " +
                "FROM Promotions " +
                "WHERE status = 'Active' " +
                "  AND CAST(GETDATE() AS date) BETWEEN start_date AND end_date " +
                "ORDER BY end_date, promo_name");

            AddListColumns(table);
            return table;
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

        /// <summary>
        /// The ONE place the discount formula lives.
        ///   Percentage - the eligible subtotal times the value, divided by 100, rounded to 2 decimals
        ///   Fixed      - the value itself, but never more than the eligible subtotal
        /// The result is never negative and never bigger than the eligible subtotal.
        /// </summary>
        public static decimal ComputeDiscount(string discountType, decimal value, decimal eligibleSubtotal)
        {
            if (eligibleSubtotal <= 0m) return 0m;

            decimal safeValue = value < 0m ? 0m : value;
            decimal discount;

            if (string.Equals(discountType, "Percentage", StringComparison.OrdinalIgnoreCase))
            {
                discount = eligibleSubtotal * safeValue / 100m;
            }
            else
            {
                // A fixed promo is taken off ONCE, so it can never be more than what is there to pay.
                discount = Math.Min(safeValue, eligibleSubtotal);
            }

            discount = Math.Round(discount, 2, MidpointRounding.AwayFromZero);

            if (discount < 0m) discount = 0m;
            if (discount > eligibleSubtotal) discount = eligibleSubtotal;
            return discount;
        }

        /// <summary>
        /// The service ids linked to a promotion. An empty list means "this promotion applies to
        /// every service on the bill". promoId 0 (a brand-new promotion) has no links yet.
        /// </summary>
        public static List<int> GetServiceIds(int promoId)
        {
            List<int> ids = new List<int>();
            if (promoId <= 0) return ids;

            DataTable table = Db.Query(
                "SELECT service_id FROM PromotionServices WHERE promo_id = @promo_id ORDER BY service_id",
                Db.P("@promo_id", promoId));

            foreach (DataRow row in table.Rows)
            {
                ids.Add(Db.Get<int>(row["service_id"]));
            }

            return ids;
        }

        /// <summary>
        /// Every Active service for the promotion edit dialog's pick list.
        /// is_selected is true when the service is already linked to promoId (promoId 0 = none).
        /// Returns service_id, service_name, category_name, price, is_selected,
        /// ordered by category then service name.
        /// </summary>
        public static DataTable GetPickList(int promoId)
        {
            return Db.Query(
                "SELECT s.service_id, s.service_name, c.category_name, s.price, " +
                "       CASE WHEN ps.promo_service_id IS NULL THEN CAST(0 AS bit) ELSE CAST(1 AS bit) END AS is_selected " +
                "FROM Services s " +
                "INNER JOIN ServiceCategories c ON c.category_id = s.category_id " +
                "LEFT JOIN PromotionServices ps ON ps.service_id = s.service_id AND ps.promo_id = @promo_id " +
                "WHERE s.status = 'Active' " +
                "ORDER BY c.category_name, s.service_name",
                Db.P("@promo_id", promoId));
        }

        /// <summary>
        /// Saves a promotion AND replaces its service links in ONE transaction, so a promotion can
        /// never be left half saved (for example with its old services but the new value).
        /// promoId 0 adds a new promotion and returns the new id; an existing id is updated.
        /// An empty serviceIds list means "applies to every service on the bill".
        /// The transaction is built here with its own SqlConnection, so it does not depend on any
        /// other transaction helper.
        /// </summary>
        public static int SavePromotion(int promoId, string promoName, string description, string discountType,
            decimal discountValue, decimal promoPrice, DateTime startDate, DateTime endDate, string status,
            List<int> serviceIds)
        {
            string name = promoName == null ? string.Empty : promoName.Trim();
            if (serviceIds == null) serviceIds = new List<int>();

            // A 'Promo Price' promotion stores its promotional price and no percentage / fixed value;
            // an old Percentage or Fixed promotion keeps a NULL promo price.
            bool promoPriceType = string.Equals(discountType, "Promo Price", StringComparison.OrdinalIgnoreCase);
            object promoPriceValue = promoPriceType ? (object)promoPrice : DBNull.Value;
            decimal storedValue = promoPriceType ? 0m : discountValue;

            using (SqlConnection connection = Db.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // The name must stay unique. The check runs inside the transaction, so two
                        // saves cannot slip in between the check and the insert.
                        using (SqlCommand check = new SqlCommand(
                            "SELECT COUNT(*) FROM Promotions WHERE promo_name = @promo_name AND promo_id <> @promo_id",
                            connection, transaction))
                        {
                            check.Parameters.Add(Db.P("@promo_name", name));
                            check.Parameters.Add(Db.P("@promo_id", promoId));

                            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                            {
                                throw new InvalidOperationException("A promotion with this name already exists.");
                            }
                        }

                        int savedId;

                        if (promoId > 0)
                        {
                            using (SqlCommand update = new SqlCommand(
                                "UPDATE Promotions SET promo_name = @promo_name, description = @description, " +
                                "       discount_type = @discount_type, discount_value = @discount_value, " +
                                "       promo_price = @promo_price, " +
                                "       start_date = @start_date, end_date = @end_date, status = @status " +
                                "WHERE promo_id = @promo_id",
                                connection, transaction))
                            {
                                update.Parameters.Add(Db.P("@promo_name", name));
                                update.Parameters.Add(Db.P("@description", description));
                                update.Parameters.Add(Db.P("@discount_type", discountType));
                                update.Parameters.Add(Db.P("@discount_value", storedValue));
                                update.Parameters.Add(Db.P("@promo_price", promoPriceValue));
                                update.Parameters.Add(Db.P("@start_date", startDate.Date));
                                update.Parameters.Add(Db.P("@end_date", endDate.Date));
                                update.Parameters.Add(Db.P("@status", status));
                                update.Parameters.Add(Db.P("@promo_id", promoId));

                                update.ExecuteNonQuery();
                            }

                            savedId = promoId;
                        }
                        else
                        {
                            using (SqlCommand insert = new SqlCommand(
                                "INSERT INTO Promotions (promo_name, description, discount_type, discount_value, " +
                                "                        promo_price, start_date, end_date, status) " +
                                "VALUES (@promo_name, @description, @discount_type, @discount_value, " +
                                "        @promo_price, @start_date, @end_date, @status); " +
                                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                                connection, transaction))
                            {
                                insert.Parameters.Add(Db.P("@promo_name", name));
                                insert.Parameters.Add(Db.P("@description", description));
                                insert.Parameters.Add(Db.P("@discount_type", discountType));
                                insert.Parameters.Add(Db.P("@discount_value", storedValue));
                                insert.Parameters.Add(Db.P("@promo_price", promoPriceValue));
                                insert.Parameters.Add(Db.P("@start_date", startDate.Date));
                                insert.Parameters.Add(Db.P("@end_date", endDate.Date));
                                insert.Parameters.Add(Db.P("@status", status));

                                savedId = Convert.ToInt32(insert.ExecuteScalar());
                            }
                        }

                        // The links are replaced, not merged: the dialog always sends the full tick list.
                        using (SqlCommand clear = new SqlCommand(
                            "DELETE FROM PromotionServices WHERE promo_id = @promo_id", connection, transaction))
                        {
                            clear.Parameters.Add(Db.P("@promo_id", savedId));
                            clear.ExecuteNonQuery();
                        }

                        HashSet<int> written = new HashSet<int>();

                        foreach (int serviceId in serviceIds)
                        {
                            // A caller could send the same id twice, which the UNIQUE constraint
                            // would refuse, so the duplicate is skipped here.
                            if (serviceId <= 0 || written.Contains(serviceId)) continue;
                            written.Add(serviceId);

                            using (SqlCommand link = new SqlCommand(
                                "INSERT INTO PromotionServices (promo_id, service_id) " +
                                "VALUES (@promo_id, @service_id)", connection, transaction))
                            {
                                link.Parameters.Add(Db.P("@promo_id", savedId));
                                link.Parameters.Add(Db.P("@service_id", serviceId));
                                link.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return savedId;
                    }
                    catch
                    {
                        try { transaction.Rollback(); }
                        catch { /* the original error is what the user needs to see */ }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// The discount this promotion gives to one appointment.
        ///   lines    - the appointment lines: a DataTable with service_id (int) and
        ///              amount (decimal, price x quantity)
        ///   message  - empty on success, otherwise a sentence a staff member can understand
        /// Returns 0 when the promotion is not Active, is not valid today, or when it applies only
        /// to services this appointment does not contain.
        /// WHY: this is the ONLY entry point Billing and Appointments may use, so the promo rules
        /// are enforced in exactly one place.
        /// </summary>
        public static decimal CalculateDiscount(int promoId, DataTable lines, out string message)
        {
            message = string.Empty;

            if (promoId <= 0)
            {
                message = "No promotion was selected.";
                return 0m;
            }

            DataRow promo = GetById(promoId);
            if (promo == null)
            {
                message = "This promotion no longer exists.";
                return 0m;
            }

            string status = Db.Get<string>(promo["status"]);
            if (!string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                message = "This promotion is not active.";
                return 0m;
            }

            DateTime start = Db.Get<DateTime>(promo["start_date"], DateTime.Today);
            DateTime end = Db.Get<DateTime>(promo["end_date"], start);
            DateTime today = DateTime.Today;

            if (today < start.Date || today > end.Date)
            {
                message = "This promotion is not valid today.";
                return 0m;
            }

            string type = Db.Get<string>(promo["discount_type"], "Percentage");
            decimal value = Db.Get<decimal>(promo["discount_value"]);

            List<int> linked = GetServiceIds(promoId);

            // A 'Promo Price' promotion is a fixed set of services sold for one price, so it has
            // its own rule: every linked service must be on the booking, and the discount is the
            // regular total of those services minus the promotional price.
            if (string.Equals(type, "Promo Price", StringComparison.OrdinalIgnoreCase))
            {
                return PromoPriceDiscount(promo, linked, lines, out message);
            }

            decimal eligibleSubtotal = 0m;
            int eligibleLines = 0;

            if (lines != null)
            {
                foreach (DataRow line in lines.Rows)
                {
                    int serviceId = Db.Get<int>(line["service_id"]);
                    decimal amount = Db.Get<decimal>(line["amount"]);

                    // No linked services = every line is eligible; otherwise only the linked ones.
                    if (linked.Count > 0 && !linked.Contains(serviceId)) continue;

                    eligibleSubtotal += amount;
                    eligibleLines++;
                }
            }

            if (linked.Count > 0 && eligibleLines == 0)
            {
                message = "This promotion applies only to specific services, and this appointment has none of them.";
                return 0m;
            }

            if (eligibleSubtotal <= 0m)
            {
                message = "There is nothing to discount.";
                return 0m;
            }

            return ComputeDiscount(type, value, eligibleSubtotal);
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

        /// <summary>
        /// The discount of a 'Promo Price' promotion: the regular price of its services (ONE unit
        /// each, read from the database) minus the promotional price, never below 0 and taken ONCE
        /// per appointment. Every linked service must be on the booking, otherwise the message says
        /// which ones are missing.
        /// </summary>
        private static decimal PromoPriceDiscount(DataRow promo, List<int> linked, DataTable lines, out string message)
        {
            message = string.Empty;

            if (linked.Count == 0)
            {
                message = "This promotion has no services yet.";
                return 0m;
            }

            List<int> booked = new List<int>();
            if (lines != null)
            {
                foreach (DataRow line in lines.Rows)
                {
                    booked.Add(Db.Get<int>(line["service_id"]));
                }
            }

            DataTable services = Db.Query(
                "SELECT s.service_id, s.service_name, s.price " +
                "FROM PromotionServices ps " +
                "INNER JOIN Services s ON s.service_id = ps.service_id " +
                "WHERE ps.promo_id = @promo_id " +
                "ORDER BY s.service_name",
                Db.P("@promo_id", Db.Get<int>(promo["promo_id"])));

            List<string> missing = new List<string>();
            decimal regularTotal = 0m;

            foreach (DataRow service in services.Rows)
            {
                int serviceId = Db.Get<int>(service["service_id"]);
                regularTotal += Db.Get<decimal>(service["price"]);

                if (!booked.Contains(serviceId))
                {
                    missing.Add(Db.Get<string>(service["service_name"]));
                }
            }

            if (missing.Count > 0)
            {
                message = "This promotion needs: " + string.Join(", ", missing.ToArray()) + ".";
                return 0m;
            }

            decimal promoPrice = Db.Get<decimal>(promo["promo_price"]);
            decimal discount = Math.Round(regularTotal - promoPrice, 2, MidpointRounding.AwayFromZero);

            if (discount < 0m) discount = 0m;
            return discount;
        }

        /// <summary>
        /// Adds the four display columns of the promotions page: services_text (the linked service
        /// names), regular_total (their prices), promo_price_text ("P 3,000.00" for a Promo Price
        /// promo, "15 % off" or "P 200.00 off" for an old one) and saving. The text is built in C#
        /// on purpose, so the app does not need a SQL Server version with STRING_AGG.
        /// </summary>
        private static void AddListColumns(DataTable table)
        {
            if (table == null || table.Columns.Contains("services_text")) return;

            table.Columns.Add("services_text", typeof(string));
            table.Columns.Add("regular_total", typeof(decimal));
            table.Columns.Add("promo_price_text", typeof(string));
            table.Columns.Add("saving", typeof(decimal));

            foreach (DataRow row in table.Rows)
            {
                int promoId = Db.Get<int>(row["promo_id"]);

                DataTable services = Db.Query(
                    "SELECT s.service_name, s.price " +
                    "FROM PromotionServices ps " +
                    "INNER JOIN Services s ON s.service_id = ps.service_id " +
                    "WHERE ps.promo_id = @promo_id " +
                    "ORDER BY s.service_name",
                    Db.P("@promo_id", promoId));

                List<string> names = new List<string>();
                decimal regularTotal = 0m;

                foreach (DataRow service in services.Rows)
                {
                    names.Add(Db.Get<string>(service["service_name"]));
                    regularTotal += Db.Get<decimal>(service["price"]);
                }

                bool promoPriceType = string.Equals(Db.Get<string>(row["discount_type"]),
                    "Promo Price", StringComparison.OrdinalIgnoreCase);

                decimal promoPrice = Db.Get<decimal>(row["promo_price"]);
                decimal discountValue = Db.Get<decimal>(row["discount_value"]);

                row["services_text"] = names.Count == 0 ? "All services" : string.Join(", ", names.ToArray());
                row["regular_total"] = regularTotal;
                row["saving"] = promoPriceType ? Math.Max(0m, regularTotal - promoPrice) : 0m;

                if (promoPriceType)
                {
                    row["promo_price_text"] = Money.Text(promoPrice);
                }
                else if (string.Equals(Db.Get<string>(row["discount_type"]), "Percentage",
                    StringComparison.OrdinalIgnoreCase))
                {
                    row["promo_price_text"] = discountValue.ToString("0.##") + " % off";
                }
                else
                {
                    row["promo_price_text"] = Money.Text(discountValue) + " off";
                }
            }
        }
    }
}
