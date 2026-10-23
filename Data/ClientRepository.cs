using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about Clients and their packages lives here.
    /// Forms call these methods only - they never write SQL themselves.
    /// Clients are NEVER deleted: the shop keeps the treatment history of every guest.
    /// </summary>
    internal static class ClientRepository
    {
        /// <summary>
        /// The clients for the grid.
        ///   text    - matches the name (first or last), the "Last, First" form or the contact number
        ///   consent - "All" | "Signed" | "Not signed"
        /// Returns client_id, full_name ("Last, First"), contact_number, birth_date, gender,
        /// consent_text ('Signed' or 'Pending') and last_visit (the date of the latest COMPLETED
        /// appointment, or NULL when the guest has never been in).
        /// </summary>
        public static DataTable Search(string text, string consent)
        {
            string sql =
                "SELECT c.client_id, " +
                "       c.last_name + ', ' + c.first_name AS full_name, " +
                "       c.contact_number, c.birth_date, c.gender, " +
                "       CASE WHEN c.consent_status = 1 THEN 'Signed' ELSE 'Pending' END AS consent_text, " +
                "       (SELECT MAX(a.appointment_date) FROM Appointments a " +
                "         WHERE a.client_id = c.client_id AND a.status = 'Completed') AS last_visit " +
                "FROM Clients c " +
                "WHERE (@search = '' " +
                "       OR c.first_name LIKE @like " +
                "       OR c.last_name LIKE @like " +
                "       OR (c.last_name + ', ' + c.first_name) LIKE @like " +
                "       OR c.contact_number LIKE @like) " +
                "  AND (@consent = 'All' " +
                "       OR (@consent = 'Signed' AND c.consent_status = 1) " +
                "       OR (@consent = 'Not signed' AND c.consent_status = 0)) " +
                "ORDER BY c.last_name, c.first_name";

            string term = text == null ? string.Empty : text.Trim();

            return Db.Query(sql,
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@consent", consent ?? "All"));
        }

        /// <summary>One full client row (for the details card), or null when the id no longer exists.</summary>
        public static DataRow GetById(int clientId)
        {
            DataTable table = Db.Query(
                "SELECT client_id, first_name, last_name, birth_date, gender, contact_number, " +
                "       address, allergies, medical_history, treatment_history, consent_status, " +
                "       created_at, updated_at " +
                "FROM Clients WHERE client_id = @client_id",
                Db.P("@client_id", clientId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// Every client for a drop-down list, as client_id + display_name ("Last, First - contact").
        /// The booking screen later uses this to pick the guest.
        /// </summary>
        public static DataTable GetForCombo()
        {
            return Db.Query(
                "SELECT client_id, " +
                "       last_name + ', ' + first_name + ' - ' + contact_number AS display_name " +
                "FROM Clients ORDER BY last_name, first_name");
        }

        /// <summary>Inserts a client and returns the new client_id. An empty optional box is stored as NULL.</summary>
        public static int Add(string firstName, string lastName, DateTime? birthDate, string gender,
            string contact, string address, string allergies, string medicalHistory,
            string treatmentHistory, bool consentSigned)
        {
            object id = Db.Scalar(
                "INSERT INTO Clients (first_name, last_name, birth_date, gender, contact_number, " +
                "                     address, allergies, medical_history, treatment_history, " +
                "                     consent_status) " +
                "VALUES (@first_name, @last_name, @birth_date, @gender, @contact_number, " +
                "        @address, @allergies, @medical_history, @treatment_history, @consent_status); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@first_name", Trim(firstName)),
                Db.P("@last_name", Trim(lastName)),
                Db.P("@birth_date", birthDate.HasValue ? (object)birthDate.Value.Date : null),
                Db.P("@gender", Trim(gender)),
                Db.P("@contact_number", Trim(contact)),
                Db.P("@address", Trim(address)),
                Db.P("@allergies", Trim(allergies)),
                Db.P("@medical_history", Trim(medicalHistory)),
                Db.P("@treatment_history", Trim(treatmentHistory)),
                Db.P("@consent_status", consentSigned));

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// Updates a client and stamps updated_at.
        /// An empty optional text box is stored as NULL, not as "" (named parameters, never SQL text).
        /// </summary>
        public static void Update(int clientId, string firstName, string lastName, DateTime? birthDate,
            string gender, string contact, string address, string allergies, string medicalHistory,
            string treatmentHistory, bool consentSigned)
        {
            Db.Execute(
                "UPDATE Clients SET first_name = @first_name, last_name = @last_name, " +
                "       birth_date = @birth_date, gender = @gender, contact_number = @contact_number, " +
                "       address = @address, allergies = @allergies, medical_history = @medical_history, " +
                "       treatment_history = @treatment_history, consent_status = @consent_status, " +
                "       updated_at = GETDATE() " +
                "WHERE client_id = @client_id",
                Db.P("@first_name", Trim(firstName)),
                Db.P("@last_name", Trim(lastName)),
                Db.P("@birth_date", birthDate.HasValue ? (object)birthDate.Value.Date : null),
                Db.P("@gender", Trim(gender)),
                Db.P("@contact_number", Trim(contact)),
                Db.P("@address", Trim(address)),
                Db.P("@allergies", Trim(allergies)),
                Db.P("@medical_history", Trim(medicalHistory)),
                Db.P("@treatment_history", Trim(treatmentHistory)),
                Db.P("@consent_status", consentSigned),
                Db.P("@client_id", clientId));
        }

        /// <summary>
        /// True when ANOTHER client already uses this contact number.
        /// The number is how the shop recognises a returning guest, so the form only WARNS about it
        /// (two family members may share one number). excludeClientId lets an update keep its own.
        /// </summary>
        public static bool ContactExists(string contact, int excludeClientId)
        {
            if (string.IsNullOrWhiteSpace(contact)) return false;

            object count = Db.Scalar(
                "SELECT COUNT(*) FROM Clients " +
                "WHERE contact_number = @contact_number AND client_id <> @exclude_id",
                Db.P("@contact_number", contact.Trim()),
                Db.P("@exclude_id", excludeClientId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// The packages of one client, for the details card.
        /// Returns package_id, service_name, sessions_label ("3 / 8"), expiry_date and status.
        /// Client id 0 simply returns an empty table (no client selected yet).
        /// </summary>
        public static DataTable GetPackages(int clientId)
        {
            return Db.Query(
                "SELECT cp.package_id, s.service_name, " +
                "       CAST(cp.sessions_completed AS varchar(10)) + ' / ' + " +
                "       CAST(cp.total_sessions AS varchar(10)) AS sessions_label, " +
                "       cp.expiry_date, cp.status " +
                "FROM ClientPackages cp " +
                "INNER JOIN Services s ON s.service_id = cp.service_id " +
                "WHERE cp.client_id = @client_id " +
                "ORDER BY cp.status, cp.purchase_date DESC",
                Db.P("@client_id", clientId));
        }

        /// <summary>Adds a package bought by a client and returns the new package_id.</summary>
        public static int AddPackage(int clientId, int serviceId, int totalSessions,
            DateTime purchaseDate, DateTime? expiryDate)
        {
            object id = Db.Scalar(
                "INSERT INTO ClientPackages (client_id, service_id, total_sessions, purchase_date, " +
                "                            expiry_date, status) " +
                "VALUES (@client_id, @service_id, @total_sessions, @purchase_date, @expiry_date, 'Active'); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@client_id", clientId),
                Db.P("@service_id", serviceId),
                Db.P("@total_sessions", totalSessions),
                Db.P("@purchase_date", purchaseDate.Date),
                Db.P("@expiry_date", expiryDate.HasValue ? (object)expiryDate.Value.Date : null));

            return Convert.ToInt32(id);
        }

        /// <summary>
        /// True when the client still has an Active package for this service.
        /// Billing uses this to charge the package price on the FIRST session only.
        /// </summary>
        public static bool HasActivePackage(int clientId, int serviceId)
        {
            object count = Db.Scalar(
                "SELECT COUNT(*) FROM ClientPackages " +
                "WHERE client_id = @client_id AND service_id = @service_id AND status = 'Active'",
                Db.P("@client_id", clientId),
                Db.P("@service_id", serviceId));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Marks every Active package whose expiry date is already in the past as 'Expired'.
        /// WHY it runs when the Clients page loads: nobody has to remember to clean up, and an
        /// expired package can never be used for a booking. Returns how many rows changed.
        /// </summary>
        public static int ExpireOldPackages()
        {
            return Db.Execute(
                "UPDATE ClientPackages SET status = 'Expired' " +
                "WHERE status = 'Active' " +
                "  AND expiry_date IS NOT NULL " +
                "  AND expiry_date < CAST(GETDATE() AS DATE)");
        }

        /// <summary>Trimmed text, or null when the box was left empty (so the column stays NULL).</summary>
        private static string Trim(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return value.Trim();
        }
    }
}
