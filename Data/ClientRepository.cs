using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using LittleEreseSystemV3.Helpers;

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
        /// The checkboxes of the paper health form, in the exact order they are printed.
        /// WHY one list: every screen builds its checkboxes from here, so a new condition is added
        /// in ONE place and the stored text always uses the same wording.
        /// </summary>
        public static readonly string[] HealthConditionOptions = new string[]
        {
            "High blood pressure",
            "Heart condition",
            "Diabetes",
            "Asthma",
            "Epilepsy or seizures",
            "Bleeding or clotting problem",
            "Taking blood thinners",
            "Keloid or easy scarring",
            "Skin condition (eczema, psoriasis)",
            "Thyroid condition",
            "Kidney or liver condition",
            "Autoimmune condition",
            "Cancer (past or present)",
            "Pregnant or breastfeeding",
            "Recent surgery"
        };

        /// <summary>
        /// Turns the ticked health boxes into the single text the Clients column stores.
        /// An empty selection becomes an empty string, which the caller stores as NULL.
        /// </summary>
        public static string JoinHealthConditions(IEnumerable<string> conditions)
        {
            if (conditions == null) return string.Empty;

            List<string> kept = new List<string>();
            foreach (string condition in conditions)
            {
                if (!string.IsNullOrWhiteSpace(condition)) kept.Add(condition.Trim());
            }

            return string.Join(", ", kept.ToArray());
        }

        /// <summary>
        /// The opposite of JoinHealthConditions: splits the stored text into single conditions.
        /// Blank entries are dropped, so an empty or NULL column simply gives no ticks.
        /// </summary>
        public static List<string> SplitHealthConditions(string stored)
        {
            List<string> conditions = new List<string>();
            if (string.IsNullOrWhiteSpace(stored)) return conditions;

            foreach (string part in stored.Split(','))
            {
                if (!string.IsNullOrWhiteSpace(part)) conditions.Add(part.Trim());
            }

            return conditions;
        }

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
                "       address, allergies, medical_history, health_conditions, treatment_history, " +
                "       consent_status, " +
                "       created_at, updated_at " +
                "FROM Clients WHERE client_id = @client_id",
                Db.P("@client_id", clientId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// The treatments this client really received at Little Erese's: one row per COMPLETED
        /// service line, newest first. Columns: visit_date, service_name, session_text, staff_name.
        /// WHY it is read here and not stored in Clients.treatment_history: the shop's own work is
        /// always the truth of the completed visits, so a text column could only go out of step.
        /// An empty table is returned when the client has no completed visit yet.
        /// </summary>
        public static DataTable GetClinicHistory(int clientId)
        {
            DataTable table = Db.Query(
                "SELECT a.appointment_date AS visit_date, s.service_name, " +
                "       ISNULL(asv.session_number, 0) AS session_number, " +
                "       ISNULL(asv.total_sessions, 0) AS total_sessions, " +
                "       ISNULL(us.full_name, '-') AS staff_name " +
                "FROM AppointmentServices asv " +
                "INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id " +
                "INNER JOIN Services s ON s.service_id = asv.service_id " +
                "OUTER APPLY (SELECT TOP 1 u.full_name FROM AppointmentStaff ast " +
                "             INNER JOIN Users u ON u.user_id = ast.user_id " +
                "             WHERE ast.appointment_service_id = asv.appointment_service_id " +
                "             ORDER BY ast.appointment_staff_id) us " +
                "WHERE a.client_id = @client_id AND asv.status = 'Completed' " +
                "ORDER BY a.appointment_date DESC, asv.appointment_service_id DESC",
                Db.P("@client_id", clientId));

            // The "Session 2 of 4" label is built in C# so the wording stays in one place.
            table.Columns.Add("session_text", typeof(string));
            foreach (DataRow row in table.Rows)
            {
                int totalSessions = Db.Get<int>(row["total_sessions"], 0);
                row["session_text"] = totalSessions > 1
                    ? AppointmentRepository.SessionText(Db.Get<int>(row["session_number"], 0), totalSessions)
                    : string.Empty;
            }

            return table;
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
            string healthConditions, string treatmentHistory, bool consentSigned)
        {
            EnsureClientIsOldEnough(birthDate);

            object id = Db.Scalar(
                "INSERT INTO Clients (first_name, last_name, birth_date, gender, contact_number, " +
                "                     address, allergies, medical_history, health_conditions, " +
                "                     treatment_history, consent_status) " +
                "VALUES (@first_name, @last_name, @birth_date, @gender, @contact_number, " +
                "        @address, @allergies, @medical_history, @health_conditions, " +
                "        @treatment_history, @consent_status); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@first_name", Trim(firstName)),
                Db.P("@last_name", Trim(lastName)),
                Db.P("@birth_date", birthDate.HasValue ? (object)birthDate.Value.Date : null),
                Db.P("@gender", Trim(gender)),
                Db.P("@contact_number", Trim(contact)),
                Db.P("@address", Trim(address)),
                Db.P("@allergies", Trim(allergies)),
                Db.P("@medical_history", Trim(medicalHistory)),
                Db.P("@health_conditions", Trim(healthConditions)),
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
            string healthConditions, string treatmentHistory, bool consentSigned)
        {
            EnsureClientIsOldEnough(birthDate);

            Db.Execute(
                "UPDATE Clients SET first_name = @first_name, last_name = @last_name, " +
                "       birth_date = @birth_date, gender = @gender, contact_number = @contact_number, " +
                "       address = @address, allergies = @allergies, medical_history = @medical_history, " +
                "       health_conditions = @health_conditions, " +
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
                Db.P("@health_conditions", Trim(healthConditions)),
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
            // Only a service sold as several sessions can be a package (PROJECT_RULES.md).
            // The rule lives here as well, so no screen can save a one-session "package".
            object sessions = Db.Scalar(
                "SELECT default_total_sessions FROM Services WHERE service_id = @service_id",
                Db.P("@service_id", serviceId));

            if (Convert.ToInt32(sessions == null || sessions == DBNull.Value ? 1 : sessions) <= 1)
            {
                throw new InvalidOperationException("Only a service with more than one session can be a package.");
            }

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

        /// <summary>
        /// The one-time clean-up of the treatment_history lines the app itself used to append
        /// ("MMM dd, yyyy - Service name (Staff name)"). Those lines were the shop's own work, which
        /// is now read from the completed visits, so they are removed from the text column.
        /// Every other line the staff typed is KEPT; when nothing is left the column becomes NULL.
        /// Returns how many clients were changed. Safe to run twice: a clean database changes nothing.
        /// </summary>
        public static int RemoveGeneratedHistoryLines()
        {
            Regex generated = new Regex(@"^[A-Z][a-z]{2} \d{2}, \d{4} - .+ \(.+\)$");
            int changed = 0;

            DataTable clients = Db.Query(
                "SELECT client_id, treatment_history FROM Clients " +
                "WHERE treatment_history IS NOT NULL AND LTRIM(RTRIM(treatment_history)) <> ''");

            foreach (DataRow row in clients.Rows)
            {
                string[] lines = Db.Get<string>(row["treatment_history"])
                    .Replace("\r\n", "\n").Split('\n');

                List<string> kept = new List<string>();
                int before = 0;
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (trimmed.Length == 0) continue;
                    before++;
                    if (!generated.IsMatch(trimmed)) kept.Add(trimmed);
                }

                if (kept.Count == before) continue;   // nothing generated in this row

                Db.Execute(
                    "UPDATE Clients SET treatment_history = @treatment_history, updated_at = GETDATE() " +
                    "WHERE client_id = @client_id",
                    Db.P("@treatment_history", kept.Count == 0
                        ? (object)null
                        : string.Join(Environment.NewLine, kept.ToArray())),
                    Db.P("@client_id", Db.Get<int>(row["client_id"])));

                changed++;
            }

            return changed;
        }

        /// <summary>Trimmed text, or null when the box was left empty (so the column stays NULL).</summary>
        private static string Trim(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return value.Trim();
        }

        /// <summary>
        /// The minimum-age rule of PROJECT_RULES.md, in the one place every save passes through, so
        /// no other screen can write a client the shop does not accept. Throws with the sentence the
        /// user must read; the form also checks the same rule with Validators before it gets here.
        /// </summary>
        internal static void EnsureClientIsOldEnough(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
            {
                throw new InvalidOperationException("Birth date is required.");
            }

            if (!Validators.IsAtLeastAge(birthDate.Value, Validators.MinimumClientAge, DateTime.Today))
            {
                throw new InvalidOperationException(Validators.MinimumClientAgeMessage);
            }
        }
    }
}
