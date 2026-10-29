using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Creates the demo accounts AND the demo business data, so every screen has something to show
    /// during the defense.
    ///
    /// SAFE ON EVERY START:
    ///   (a) EnsureDemoUsers() only adds the accounts that are missing.
    ///   (b) EnsureDemoData() writes the demo records ONLY when the Clients table is still empty,
    ///       and it writes them in ONE transaction, so a stopped run leaves nothing half saved.
    /// Nothing is ever deleted or overwritten, and every value obeys the business rules of
    /// PROJECT_RULES.md (opening hours, 30-minute starts, daily capacity, rooms, downpayment,
    /// promotion, package and commission rules).
    /// </summary>
    internal static class DemoDataSeeder
    {
        // POLISH: set to false before the shop uses the system for real.
        public const bool EnableDemoSeed = true;

        // POLISH: the owner can change these demo passwords here (or delete the accounts in Staff Accounts).
        private const string AdminPassword = "Admin@123";
        private const string StaffPassword = "Staff@123";

        // A FIXED seed: every machine and every run produces exactly the same demo data.
        private const int RandomSeed = 20260415;

        // The demo leaves a 30-minute clean-up break between two visits of the same day, which also
        // stops two visits from overlapping in one room or with one technician.
        private const int GapMinutes = 30;

        // How many finished visits the past two weeks hold (the smoke test asks for at least 25).
        private const int PastCompletedVisits = 25;

        // The shop pays 10% commission on the net amount of a service line.
        private const decimal CommissionPercent = 10.00m;

        // The seven technicians the demo books with, by username (the accounts EnsureDemoUsers makes).
        private static readonly string[] DemoStaffUsernames = new string[]
        {
            "aesthetician1", "aesthetician2", "aesthetician3", "aesthetician4",
            "therapist1", "therapist2", "nurse1"
        };

        // The services the demo uses for its finished visits. Only real catalogue rows are named
        // here, so the price and the duration always come from the Services table.
        private static readonly string[,] DemoAestheticServices = new string[,]
        {
            { "Botox", "Traptox" },
            { "Glutathione", "Ultimate Whitening Gluta Drip" },
            { "Facial", "Diamond Peel" },
            { "Facial", "Hydrafacial" },
            { "Facial", "Rejuvenating Facial" },
            { "Slimming", "Mesolipo" },
            { "Hair Removal and Whitening", "Underarm Whitening" },
            { "Face Beautification", "Brow Tint" },
            { "Face Beautification", "Lash Lift w/ Tint" },
            { "Nails", "Gel Manicure (Classic)" },
            { "Nails", "Classic Pedicure" }
        };

        private static readonly string[,] DemoMassageServices = new string[,]
        {
            { "Massage - Full Body", "Swedish Massage (1 hour)" },
            { "Massage - Full Body", "Thai Herbal Massage (1.5 hours)" },
            { "Massage - Feet", "Foot Spa w/ Massage" },
            { "Massage - Hand", "Hand Massage" }
        };

        // ---------------------------------------------------------------------
        // PUBLIC ENTRY POINTS
        // ---------------------------------------------------------------------

        /// <summary>
        /// (a) Gives the placeholder "admin" a real password, (b) creates the demo staff accounts
        /// if their usernames are still free.
        /// </summary>
        public static void EnsureDemoUsers()
        {
            if (EnableDemoSeed)
            {
                EnsureAdminPassword();

                // The job type of each demo account matches its name (see EnsureStaff).
                // POLISH: rename these demo staff accounts to the real employees in Staff Accounts.
                EnsureStaff("Aesthetician 1", "aesthetician1", "Aesthetician");
                EnsureStaff("Aesthetician 2", "aesthetician2", "Aesthetician");
                EnsureStaff("Aesthetician 3", "aesthetician3", "Aesthetician");
                EnsureStaff("Aesthetician 4", "aesthetician4", "Aesthetician");
                EnsureStaff("Massage Therapist 1", "therapist1", "Massage Therapist");
                EnsureStaff("Massage Therapist 2", "therapist2", "Massage Therapist");
                EnsureStaff("Nurse 1", "nurse1", "Nurse");
            }
        }

        /// <summary>
        /// Creates the demo business data ONCE, when the shop has no clients yet.
        /// The whole set is written in ONE transaction (Db.InTransaction), so a stopped run leaves
        /// nothing behind and the next start simply tries again.
        /// </summary>
        public static void EnsureDemoData()
        {
            if (EnableDemoSeed)
            {
                // The shop already has its own records: never touch them.
                if (CountClients() > 0) return;

                Db.InTransaction(delegate (SqlConnection connection, SqlTransaction transaction)
                {
                    SeedAll(connection, transaction);
                });
            }
        }

        private static int CountClients()
        {
            return Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Clients"));
        }

        /// <summary>The whole demo set, in the order the foreign keys need.</summary>
        private static void SeedAll(SqlConnection connection, SqlTransaction transaction)
        {
            Random random = new Random(RandomSeed);

            int adminId = UserId(connection, transaction, "admin");
            List<int> staffIds = StaffIds(connection, transaction);

            // Without the demo accounts there is nobody to book or to pay the commission to.
            if (adminId <= 0 || staffIds.Count == 0) return;

            DataTable services = LoadServices(connection, transaction);
            DataTable rooms = LoadRooms(connection, transaction);

            int[] clientIds = SeedClients(connection, transaction);
            int glowPromoId = SeedPromotions(connection, transaction);

            SeedPastVisits(connection, transaction, random, clientIds, adminId, staffIds,
                services, rooms, glowPromoId);

            SeedTodayVisits(connection, transaction, random, clientIds, adminId, staffIds,
                services, rooms, glowPromoId);

            SeedUpcomingVisits(connection, transaction, random, clientIds, adminId, staffIds, services, rooms);

            SeedCancelledVisits(connection, transaction, random, clientIds, adminId, staffIds, services, rooms);

            SeedActivityLog(connection, transaction, adminId, staffIds);

            // LAST, after every section: the lines written above carry no times of their own yet.
            BackfillLineTimes(connection, transaction);
        }

        /// <summary>
        /// Gives every service line the times of its appointment when it has none.
        /// The line inherits the appointment's start and end; the technician is busy for
        /// s.staff_busy_minutes only (a gluta drip), never past the line end.
        /// ONE statement for the whole set, no loop.
        /// </summary>
        private static void BackfillLineTimes(SqlConnection connection, SqlTransaction transaction)
        {
            Db.Execute(connection, transaction,
@"UPDATE asv
SET asv.start_time = a.start_time,
    asv.end_time = a.end_time,
    asv.busy_end_time = CASE
        WHEN s.staff_busy_minutes > 0
            THEN CAST(CASE WHEN DATEADD(MINUTE, s.staff_busy_minutes, a.start_time) > a.end_time
                           THEN a.end_time
                           ELSE DATEADD(MINUTE, s.staff_busy_minutes, a.start_time) END AS time)
        ELSE a.end_time
    END
FROM AppointmentServices asv
INNER JOIN Appointments a ON a.appointment_id = asv.appointment_id
INNER JOIN Services s ON s.service_id = asv.service_id
WHERE asv.start_time IS NULL");
        }

        // ---------------------------------------------------------------------
        // SECTION 1 - CLIENTS (12 guests, 9 with a signed consent form)
        // ---------------------------------------------------------------------

        private static int[] SeedClients(SqlConnection connection, SqlTransaction transaction)
        {
            List<int> clients = new List<int>();

            // POLISH: replace these placeholder guests with the real client list before the defense.
            clients.Add(InsertClient(connection, transaction, "Maria Clara", "Santos",
                new DateTime(1994, 3, 12), "Female", "09171234501",
                "San Nicolas Poblacion, Concepcion, Tarlac", null, null, null, null, true, 120));

            clients.Add(InsertClient(connection, transaction, "Jhon Rey", "Cunanan",
                new DateTime(1990, 7, 5), "Male", "09171234502",
                "Poblacion 3, Concepcion, Tarlac", null, null, null, null, true, 110));

            clients.Add(InsertClient(connection, transaction, "Aileen", "Dela Cruz",
                new DateTime(1996, 11, 20), "Female", "09171234503",
                "Sto. Cristo, Concepcion, Tarlac", "Penicillin",
                "Hypertension - maintenance medicine",
                ClientRepository.JoinHealthConditions(new string[] { "High blood pressure", "Taking blood thinners" }),
                null, true, 100));

            clients.Add(InsertClient(connection, transaction, "Mark Anthony", "Reyes",
                new DateTime(1988, 2, 14), "Male", "09171234504",
                "Alfonso, Concepcion, Tarlac", "Shellfish", null,
                ClientRepository.JoinHealthConditions(new string[] { "Diabetes" }),
                null, false, 95));

            clients.Add(InsertClient(connection, transaction, "Jenny Lyn", "Bautista",
                new DateTime(1999, 6, 30), "Female", "09171234505",
                "Minane, Concepcion, Tarlac", null, "Mild asthma - no maintenance medicine",
                null, null, true, 90));

            clients.Add(InsertClient(connection, transaction, "Carlo Miguel", "Aquino",
                new DateTime(1993, 9, 8), "Male", "09171234506",
                "Balutu, Concepcion, Tarlac", null, "Sensitive skin - reacts to strong peeling acids",
                ClientRepository.JoinHealthConditions(new string[] { "Asthma", "Skin condition (eczema, psoriasis)" }),
                "Chemical peel at another clinic (3 months ago)", true, 80));

            clients.Add(InsertClient(connection, transaction, "Rosalie", "Manalo",
                new DateTime(1985, 12, 1), "Female", "09171234507",
                "Sta. Cruz, Concepcion, Tarlac", null, "Occasional migraine",
                ClientRepository.JoinHealthConditions(new string[] { "High blood pressure", "Thyroid condition", "Kidney or liver condition" }),
                "Chemical peel at another clinic (3 months ago)", true, 70));

            clients.Add(InsertClient(connection, transaction, "Kevin Paul", "Garcia",
                new DateTime(1997, 4, 17), "Male", "09171234508",
                "Tinang, Concepcion, Tarlac", null, null, null, null, false, 60));

            clients.Add(InsertClient(connection, transaction, "Divina", "Mercado",
                new DateTime(1991, 8, 23), "Female", "09171234509",
                "San Jose, Concepcion, Tarlac", "Latex", "Occasional lower back pain",
                ClientRepository.JoinHealthConditions(new string[] { "Keloid or easy scarring" }),
                null, true, 50));

            clients.Add(InsertClient(connection, transaction, "Paolo Vincent", "Laxamana",
                new DateTime(1992, 1, 5), "Male", "09171234510",
                "Marimla, Concepcion, Tarlac", null, "Mild gastritis",
                null, null, true, 40));

            clients.Add(InsertClient(connection, transaction, "Cristina", "Villanueva",
                new DateTime(2000, 5, 19), "Female", "09171234511",
                "Taldiapan, Concepcion, Tarlac", null,
                "Pregnant (2nd trimester) - no Botox or injectables",
                ClientRepository.JoinHealthConditions(new string[] { "Pregnant or breastfeeding" }),
                null, false, 30));

            clients.Add(InsertClient(connection, transaction, "Jomar", "Tiongson",
                new DateTime(1989, 10, 27), "Male", "09171234512",
                "San Martin, Concepcion, Tarlac", null, "Seasonal allergic rhinitis",
                null, null, true, 20));
            return clients.ToArray();
        }

        /// <summary>Writes one client and returns the new client_id (created_at is back-dated).</summary>
        private static int InsertClient(SqlConnection connection, SqlTransaction transaction,
            string firstName, string lastName, DateTime birthDate, string gender, string contact,
            string address, string allergies, string medicalHistory, string healthConditions,
            string treatmentHistory, bool consentSigned, int daysAgo)
        {
            DateTime registered = DateTime.Now.AddDays(-daysAgo);

            object id = Db.Scalar(connection, transaction,
@"INSERT INTO Clients (first_name, last_name, birth_date, gender, contact_number, address,
                     allergies, medical_history, health_conditions, treatment_history,
                     consent_status, created_at, updated_at)
VALUES (@first_name, @last_name, @birth_date, @gender, @contact_number, @address,
        @allergies, @medical_history, @health_conditions, @treatment_history,
        @consent_status, @created_at, @created_at);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@first_name", firstName),
                Db.P("@last_name", lastName),
                Db.P("@birth_date", birthDate.Date),
                Db.P("@gender", gender),
                Db.P("@contact_number", contact),
                Db.P("@address", address),
                Db.P("@allergies", allergies),
                Db.P("@medical_history", medicalHistory),
                // An empty list must land as NULL, so "clients with health_conditions" counts real entries.
                Db.P("@health_conditions", string.IsNullOrEmpty(healthConditions) ? null : (object)healthConditions),
                Db.P("@treatment_history", string.IsNullOrEmpty(treatmentHistory) ? null : (object)treatmentHistory),
                Db.P("@consent_status", consentSigned),
                Db.P("@created_at", registered));

            return Convert.ToInt32(id);
        }

        // ---------------------------------------------------------------------
        // SECTION 2 - PROMOTIONS (one valid today, one fixed, one expired last month)
        // ---------------------------------------------------------------------

        /// <summary>Creates the three promotions and returns the id of the one the demo books with.</summary>
        private static int SeedPromotions(SqlConnection connection, SqlTransaction transaction)
        {
            DateTime today = DateTime.Today;
            DateTime firstOfThisMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastMonthStart = firstOfThisMonth.AddMonths(-1);
            DateTime lastMonthEnd = firstOfThisMonth.AddDays(-1);

            // (a) The promo the shop runs right now: 15% off, valid for a week either side of today.
            int glowId = InsertPromotion(connection, transaction, "Monthly Glow Promo",
                "15% off any aesthetic or massage service. Paid in full when the booking is made.",
                "Percentage", 15m, today.AddDays(-7), today.AddDays(7), "Active");

            // (b) A fixed amount promo that is also valid today, so Billing offers two real choices.
            InsertPromotion(connection, transaction, "Beauty Bucket 500",
                "PHP 500 off the whole bill. Paid in full when the booking is made.",
                "Fixed", 500m, today.AddDays(-30), today.AddDays(30), "Active");

            // (c) Last month's promo. It ended last month, so it can never be applied again.
            InsertPromotion(connection, transaction, "Summer Radiance Promo",
                "10% off every glutathione drip during the summer month.",
                "Percentage", 10m, lastMonthStart, lastMonthEnd, "Expired");

            return glowId;
        }

        private static int InsertPromotion(SqlConnection connection, SqlTransaction transaction,
            string promoName, string description, string discountType, decimal discountValue,
            DateTime startDate, DateTime endDate, string status)
        {
            object id = Db.Scalar(connection, transaction,
@"INSERT INTO Promotions (promo_name, description, discount_type, discount_value, start_date, end_date, status)
VALUES (@promo_name, @description, @discount_type, @discount_value, @start_date, @end_date, @status);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@promo_name", promoName),
                Db.P("@description", description),
                Db.P("@discount_type", discountType),
                Db.P("@discount_value", discountValue),
                Db.P("@start_date", startDate.Date),
                Db.P("@end_date", endDate.Date),
                Db.P("@status", status));

            return Convert.ToInt32(id);
        }

        // ---------------------------------------------------------------------
        // SECTION 3 - THE PAST TWO WEEKS (packages + finished, fully paid visits)
        // ---------------------------------------------------------------------

        /// <summary>
        /// Seeds the three packages, the visits that use them, and enough further finished visits
        /// to reach PastCompletedVisits. Every visit is planned first (date, 30-minute start time,
        /// technician, promo) and inserted afterwards.
        /// </summary>
        private static void SeedPastVisits(SqlConnection connection, SqlTransaction transaction, Random random,
            int[] clientIds, int adminId, List<int> staffIds,
            DataTable services, DataTable rooms, int glowPromoId)
        {
            DateTime today = DateTime.Today;

            // The start time every day remembers, so two visits of one day can never overlap.
            Dictionary<DateTime, TimeSpan> cursors = new Dictionary<DateTime, TimeSpan>();
            DataTable plan = CreateVisitPlan();

            // ---- The three packages, each with the visits that belong to it ----
            DataRow prp = FindService(services, "Facial", "PRP/Vampire Facial");
            if (prp != null)
            {
                int prpServiceId = Db.Get<int>(prp["service_id"]);

                // (a) One session of three done, so the Clients screen shows "1 / 3".
                int packageA = InsertPackage(connection, transaction, clientIds[0], prpServiceId, 3, today.AddDays(-15));
                AddVisit(plan, cursors, random, today.AddDays(-10), clientIds[0], prp, packageA, 1, 3,
                    staffIds[0], 0, "Walk-in");

                // (b) A second client finished the whole 3-session package, so one row reads "Completed".
                int packageB = InsertPackage(connection, transaction, clientIds[2], prpServiceId, 3, today.AddDays(-16));
                AddVisit(plan, cursors, random, today.AddDays(-13), clientIds[2], prp, packageB, 1, 3,
                    staffIds[1], 0, "Messenger");
                AddVisit(plan, cursors, random, today.AddDays(-9), clientIds[2], prp, packageB, 2, 3,
                    staffIds[2], 0, "Messenger");
                AddVisit(plan, cursors, random, today.AddDays(-5), clientIds[2], prp, packageB, 3, 3,
                    staffIds[3], 0, "Messenger");

                // (c) A third package halfway through, so the Clients screen shows three different
                //     progress labels (1 / 3, 2 / 3 and Completed).
                int packageC = InsertPackage(connection, transaction, clientIds[5], prpServiceId, 3, today.AddDays(-14));
                AddVisit(plan, cursors, random, today.AddDays(-11), clientIds[5], prp, packageC, 1, 3,
                    staffIds[6], 0, "Text");
                AddVisit(plan, cursors, random, today.AddDays(-7), clientIds[5], prp, packageC, 2, 3,
                    staffIds[0], 0, "Text");
            }

            // POLISH: the catalogue has no multi-session laser service yet, so the demo uses the
            // other multi-session service it finds (today: "Facial test", 8 sessions).
            DataRow multi = FindMultiSessionService(services, prp == null ? 0 : Db.Get<int>(prp["service_id"]));
            if (multi != null)
            {
                int multiServiceId = Db.Get<int>(multi["service_id"]);
                int totalSessions = Db.Get<int>(multi["default_total_sessions"], 1);

                int packageC = InsertPackage(connection, transaction, clientIds[4], multiServiceId,
                    totalSessions, today.AddDays(-20));

                AddVisit(plan, cursors, random, today.AddDays(-8), clientIds[4], multi, packageC, 1,
                    totalSessions, staffIds[4], 0, "Phone");
                AddVisit(plan, cursors, random, today.AddDays(-6), clientIds[4], multi, packageC, 2,
                    totalSessions, staffIds[5], 0, "Phone");
            }

            // ---- The rest of the finished visits ----
            // 3 quiet days with one visit and 11 days with two = 25 finished visits.
            for (int dayOffset = -14; dayOffset <= -1 && plan.Rows.Count < PastCompletedVisits; dayOffset++)
            {
                DateTime date = today.AddDays(dayOffset);
                int visitsThatDay = dayOffset <= -13 ? 1 : 2;

                while (CountVisitsOn(plan, date) < visitsThatDay && plan.Rows.Count < PastCompletedVisits)
                {
                    int clientId = clientIds[random.Next(clientIds.Length)];
                    int technicianId = staffIds[plan.Rows.Count % staffIds.Count];
                    string source = PickBookingSource(random);

                    // The promo is only used while it is valid (it covers the last seven days).
                    int promoId = glowPromoId > 0 && date >= today.AddDays(-7) && random.Next(0, 4) == 0
                        ? glowPromoId
                        : 0;

                    if (!AddRandomVisit(plan, cursors, random, date, clientId, technicianId, promoId, source, services))
                    {
                        // The opening hours of that day are full: move on to the next day.
                        break;
                    }
                }
            }

            InsertFinishedVisits(connection, transaction, random, plan, services, rooms, adminId);
        }

        /// <summary>The empty plan table of the finished visits (one row = one visit).</summary>
        private static DataTable CreateVisitPlan()
        {
            DataTable table = new DataTable();
            table.Columns.Add("visit_date", typeof(DateTime));
            table.Columns.Add("start_time", typeof(TimeSpan));
            table.Columns.Add("client_id", typeof(int));
            table.Columns.Add("service_id", typeof(int));
            table.Columns.Add("package_id", typeof(int));
            table.Columns.Add("session_number", typeof(int));
            table.Columns.Add("total_sessions", typeof(int));
            table.Columns.Add("staff_user_id", typeof(int));
            table.Columns.Add("promo_id", typeof(int));
            table.Columns.Add("booking_source", typeof(string));
            return table;
        }

        /// <summary>
        /// Adds one visit to the plan. The start time comes from the day's cursor, so the visits of
        /// one day follow each other (with the clean-up break) and never overlap.
        /// Returns false when the service no longer fits inside the opening hours of that day.
        /// </summary>
        private static bool AddVisit(DataTable plan, Dictionary<DateTime, TimeSpan> cursors, Random random,
            DateTime date, int clientId, DataRow service, int packageId, int sessionNumber, int totalSessions,
            int staffId, int promoId, string bookingSource)
        {
            if (service == null) return false;

            TimeSpan start;
            if (!TryStartTime(cursors, random, date, service, out start)) return false;

            plan.Rows.Add(date.Date, start, clientId, Db.Get<int>(service["service_id"]), packageId,
                sessionNumber, totalSessions, staffId, promoId, bookingSource);

            return true;
        }

        /// <summary>Adds one visit with a service picked from the demo catalogue.</summary>
        private static bool AddRandomVisit(DataTable plan, Dictionary<DateTime, TimeSpan> cursors, Random random,
            DateTime date, int clientId, int staffId, int promoId, string bookingSource, DataTable services)
        {
            // Three tries: a long service late in the day may not fit, a shorter one will.
            for (int attempt = 0; attempt < 3; attempt++)
            {
                DataRow service = PickService(random, services);
                if (service == null) return false;

                if (AddVisit(plan, cursors, random, date, clientId, service, 0, 0, 0, staffId, promoId, bookingSource))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The start time of the next visit of one day: always on a 30-minute mark, inside the
        /// opening hours of the service (massage 1:00 PM - 10:00 PM, the rest 10:00 AM - 8:00 PM),
        /// and after the previous visit plus the clean-up break. The cursor of the day is moved on.
        /// </summary>
        private static bool TryStartTime(Dictionary<DateTime, TimeSpan> cursors, Random random,
            DateTime date, DataRow service, out TimeSpan start)
        {
            string categoryName = Db.Get<string>(service["category_name"]);
            bool massage = IsMassageCategory(categoryName);
            int minutes = Db.Get<int>(service["duration_minutes"], 60);

            TimeSpan cursor;
            if (!cursors.TryGetValue(date.Date, out cursor))
            {
                // A fresh day starts between 10:00 AM and 12:00 PM, so the days do not all look alike.
                cursor = AppointmentRepository.AestheticOpening.Add(new TimeSpan(0, 30 * random.Next(0, 5), 0));
            }

            start = cursor;

            if (massage && start < AppointmentRepository.MassageOpening)
            {
                start = AppointmentRepository.MassageOpening;
            }

            // The shop only starts a service on the half hour.
            TimeSpan half = new TimeSpan(0, 30, 0);
            long rest = start.Ticks % half.Ticks;
            if (rest != 0) start = start.Add(new TimeSpan(half.Ticks - rest));

            TimeSpan closing = massage ? AppointmentRepository.MassageClosing : AppointmentRepository.AestheticClosing;
            if (start.Add(new TimeSpan(0, minutes, 0)) > closing) return false;

            cursors[date.Date] = start.Add(new TimeSpan(0, minutes + GapMinutes, 0));
            return true;
        }

        private static int CountVisitsOn(DataTable plan, DateTime date)
        {
            int count = 0;

            foreach (DataRow row in plan.Rows)
            {
                if (Db.Get<DateTime>(row["visit_date"]).Date == date.Date) count++;
            }

            return count;
        }

        /// <summary>A random service of the demo catalogue (one visit in three is a massage).</summary>
        private static DataRow PickService(Random random, DataTable services)
        {
            bool massage = random.Next(0, 3) == 0;
            return PickFrom(massage ? DemoMassageServices : DemoAestheticServices, random, services);
        }

        private static DataRow PickFrom(string[,] catalogue, Random random, DataTable services)
        {
            int index = random.Next(0, catalogue.GetLength(0));
            return FindService(services, catalogue[index, 0], catalogue[index, 1]);
        }

        /// <summary>
        /// Inserts every planned visit with everything that belongs to it: the appointment, its
        /// line, the technician, the room, the bill, the payments, the commission, the history row
        /// and the package session. The shop's own work is read from these completed lines, so
        /// nothing is written into Clients.treatment_history (that column holds only what the guest
        /// was treated with at other clinics).
        /// </summary>
        private static void InsertFinishedVisits(SqlConnection connection, SqlTransaction transaction,
            Random random, DataTable plan, DataTable services, DataTable rooms, int receivedBy)
        {
            foreach (DataRow visit in plan.Rows)
            {
                DateTime date = Db.Get<DateTime>(visit["visit_date"]);
                TimeSpan start = (TimeSpan)visit["start_time"];
                int clientId = Db.Get<int>(visit["client_id"]);
                int serviceId = Db.Get<int>(visit["service_id"]);
                int packageId = Db.Get<int>(visit["package_id"]);
                int sessionNumber = Db.Get<int>(visit["session_number"]);
                int totalSessions = Db.Get<int>(visit["total_sessions"]);
                int technicianId = Db.Get<int>(visit["staff_user_id"]);
                int promoId = Db.Get<int>(visit["promo_id"]);
                string bookingSource = Db.Get<string>(visit["booking_source"], "Walk-in");

                DataRow service = FindServiceById(services, serviceId);
                if (service == null) continue;

                string categoryName = Db.Get<string>(service["category_name"]);
                int categoryId = Db.Get<int>(service["category_id"]);
                decimal servicePrice = Db.Get<decimal>(service["price"]);
                TimeSpan end = start.Add(new TimeSpan(0, Db.Get<int>(service["duration_minutes"], 60), 0));

                // (1) The appointment and its single service line. The package rule says the whole
                //     package price sits on the first session and every later session is 0.00.
                DateTime bookedAt = date.AddDays(-5);
                int appointmentId = InsertAppointment(connection, transaction, clientId, receivedBy, date,
                    start, end, bookingSource, "Completed", bookedAt, null);

                decimal price = packageId > 0 && sessionNumber > 1 ? 0m : servicePrice;

                int lineId = InsertLine(connection, transaction, appointmentId, serviceId, 1, price,
                    packageId, sessionNumber, totalSessions, "Completed");

                InsertStaff(connection, transaction, appointmentId, lineId, technicianId, "Completed");

                int roomId = RoomForCategory(connection, transaction, rooms, categoryId, categoryName);
                if (roomId > 0)
                {
                    InsertRoom(connection, transaction, appointmentId, lineId, roomId, "Released");
                }

                // (2) The money. A promotion is paid in full when the booking is made, any other
                //     booking that did not come from the shop door left a 50% deposit, and a
                //     walk-in pays the whole bill at the counter.
                decimal subtotal = price;
                decimal discount = DiscountFor(connection, transaction, promoId, subtotal);
                decimal total = subtotal - discount;
                if (total < 0m) total = 0m;

                decimal upfront = promoId > 0 ? total : (IsWalkIn(bookingSource) ? 0m : HalfOf(subtotal));
                string downpaymentStatus = upfront > 0m ? "Applied" : "None";

                // A line of 0.00 (a later session of a package) leaves nothing to collect, so the
                // bill is born 'Paid' - the same rule migration 005 applies to older rows.
                string paymentStatus = total == 0m ? "Paid" : "Unpaid";

                int billingId = BillingRepository.CreateBilling(connection, transaction, appointmentId,
                    subtotal, promoId, discount, total, upfront, downpaymentStatus, total, paymentStatus);

                int finalPaymentId = 0;

                if (promoId > 0)
                {
                    // A promotion is never an installment: the whole total was paid at the booking.
                    string promoMethod = PickPaymentMethod(random);
                    finalPaymentId = BillingRepository.AddPayment(connection, transaction, billingId, receivedBy,
                        "Final", promoMethod, ReferenceFor(promoMethod, random, date), total, "Verified");
                }
                else
                {
                    if (upfront > 0m)
                    {
                        string depositMethod = PickPaymentMethod(random);
                        BillingRepository.AddPayment(connection, transaction, billingId, receivedBy,
                            "Downpayment", depositMethod, ReferenceFor(depositMethod, random, date),
                            upfront, "Verified");
                    }

                    decimal settlement = total - upfront;
                    if (settlement > 0m)
                    {
                        string method = PickPaymentMethod(random);
                        finalPaymentId = BillingRepository.AddPayment(connection, transaction, billingId,
                            receivedBy, "Final", method, ReferenceFor(method, random, date), settlement, "Verified");
                    }
                }

                // (3) The commission: 10% of the net line amount. A package line earns 10% of the
                //     package price spread over its sessions, exactly like BillingRepository.Checkout.
                decimal baseAmount;
                if (packageId > 0 && totalSessions > 0)
                {
                    baseAmount = servicePrice / totalSessions;
                }
                else
                {
                    baseAmount = price;
                    if (subtotal > 0m && discount > 0m) baseAmount -= discount * (price / subtotal);
                }

                if (baseAmount < 0m) baseAmount = 0m;

                decimal commission = Math.Round(baseAmount * CommissionPercent / 100m, 2, MidpointRounding.AwayFromZero);

                // About 7 out of 10 commissions are still waiting for the payout.
                bool paidOut = random.Next(0, 10) < 3;
                DateTime? payoutDate = null;

                if (paidOut)
                {
                    DateTime payout = date.AddDays(random.Next(1, 6));
                    if (payout > DateTime.Today) payout = DateTime.Today;
                    payoutDate = payout;
                }

                InsertCommission(connection, transaction, technicianId, lineId, finalPaymentId, commission,
                    paidOut ? "Paid" : "Unpaid", payoutDate);

                // (4) The audit row of the booking itself.
                InsertHistory(connection, transaction, appointmentId, receivedBy, "Created", null, null,
                    null, "Upcoming", "Booked as a " + bookingSource + " appointment.", bookedAt);

                // (5) A package session was used, so the package moves one step forward.
                if (packageId > 0) CompletePackageSession(connection, transaction, packageId);
            }
        }

        // ---------------------------------------------------------------------
        // SECTION 4 - TODAY (2 finished, 1 in progress, 3 still to come)
        // ---------------------------------------------------------------------

        private static void SeedTodayVisits(SqlConnection connection, SqlTransaction transaction, Random random,
            int[] clientIds, int adminId, List<int> staffIds,
            DataTable services, DataTable rooms, int glowPromoId)
        {
            DateTime today = DateTime.Today;

            // (a) Two visits that are already paid out: 10:00 AM, then the next one after the break.
            DataTable plan = CreateVisitPlan();
            Dictionary<DateTime, TimeSpan> cursors = new Dictionary<DateTime, TimeSpan>();
            cursors[today] = AppointmentRepository.AestheticOpening;

            AddVisit(plan, cursors, random, today, clientIds[0],
                FindService(services, "Facial", "Hydrafacial"), 0, 0, 0, staffIds[0], glowPromoId, "Messenger");

            AddVisit(plan, cursors, random, today, clientIds[1],
                FindService(services, "Botox", "Traptox"), 0, 0, 0, staffIds[1], 0, "Walk-in");

            InsertFinishedVisits(connection, transaction, random, plan, services, rooms, adminId);

            // (b) One service in progress and three still to come, all at different times, so no
            //     room and no technician is used twice.
            InsertScheduledVisit(connection, transaction, random, clientIds[2], adminId, staffIds[2], today,
                new TimeSpan(13, 0, 0), FindService(services, "Facial", "Rejuvenating Facial"),
                "Phone", "Ongoing", rooms);

            InsertScheduledVisit(connection, transaction, random, clientIds[3], adminId, staffIds[3], today,
                new TimeSpan(15, 0, 0), FindService(services, "Facial", "Korean BB Glow + Blush Facial"),
                "Messenger", "Upcoming", rooms);

            InsertScheduledVisit(connection, transaction, random, clientIds[4], adminId, staffIds[4], today,
                new TimeSpan(16, 30, 0), FindService(services, "Nails", "Gel Manicure (Classic)"),
                "Text", "Upcoming", rooms);

            InsertScheduledVisit(connection, transaction, random, clientIds[5], adminId, staffIds[5], today,
                new TimeSpan(18, 0, 0), FindService(services, "Facial", "Diamond Peel"),
                "Walk-in", "Upcoming", rooms);
        }

        // ---------------------------------------------------------------------
        // SECTION 5 - THE NEXT TWO WEEKS (one booking a day, deposits held)
        // ---------------------------------------------------------------------

        private static void SeedUpcomingVisits(SqlConnection connection, SqlTransaction transaction, Random random,
            int[] clientIds, int adminId, List<int> staffIds, DataTable services, DataTable rooms)
        {
            for (int day = 1; day <= 14; day++)
            {
                DateTime date = DateTime.Today.AddDays(day);

                bool massage = day % 4 == 0;

                DataRow service = PickFrom(massage ? DemoMassageServices : DemoAestheticServices, random, services);
                if (service == null) continue;

                // A massage may only start at 1:00 PM or later; everything else opens at 10:00 AM.
                TimeSpan start = massage ? AppointmentRepository.MassageOpening : new TimeSpan(10, 0, 0);

                int clientId = clientIds[random.Next(clientIds.Length)];
                int technicianId = staffIds[day % staffIds.Count];

                // Every fifth booking walked in from the door, so it carries no deposit.
                string source = day % 5 == 0 ? "Walk-in" : PickBookingSource(random);

                int appointmentId = InsertScheduledVisit(connection, transaction, random, clientId, adminId,
                    technicianId, date, start, service, source, "Upcoming", rooms);

                // The third booking was moved by the client with more than three days of notice,
                // so the History screen and the Rescheduled counter have a real row to show.
                if (day == 3)
                {
                    InsertHistory(connection, transaction, appointmentId, adminId, "Rescheduled",
                        date.AddDays(-5), new TimeSpan(14, 0, 0), "Upcoming", "Upcoming",
                        "Client asked to move the appointment a week later.", DateTime.Today.AddDays(-4));
                }
            }
        }

        /// <summary>
        /// Writes one appointment that has not happened yet (Upcoming or Ongoing): the line, the
        /// technician, the room, the bill and - for any booking that did not come from the shop
        /// door - the 50% downpayment saved as a 'Downpayment' payment with 'Held' status.
        /// </summary>
        private static int InsertScheduledVisit(SqlConnection connection, SqlTransaction transaction,
            Random random, int clientId, int bookedBy, int technicianId, DateTime date, TimeSpan start,
            DataRow service, string bookingSource, string status, DataTable rooms)
        {
            if (service == null) return 0;

            int minutes = Db.Get<int>(service["duration_minutes"], 60);
            TimeSpan end = start.Add(new TimeSpan(0, minutes, 0));
            decimal subtotal = Db.Get<decimal>(service["price"]);
            DateTime bookedAt = date.AddDays(-6);

            int appointmentId = InsertAppointment(connection, transaction, clientId, bookedBy, date, start, end,
                bookingSource, status, bookedAt, null);

            int lineId = InsertLine(connection, transaction, appointmentId, Db.Get<int>(service["service_id"]),
                1, subtotal, 0, 0, 0, "Pending");

            InsertStaff(connection, transaction, appointmentId, lineId, technicianId, "Assigned");

            int roomId = RoomForCategory(connection, transaction, rooms, Db.Get<int>(service["category_id"]),
                Db.Get<string>(service["category_name"]));
            if (roomId > 0)
            {
                InsertRoom(connection, transaction, appointmentId, lineId, roomId, "Reserved");
            }

            // Booking rule: a Messenger / Text / Phone booking needs 50% of the subtotal paid now,
            // saved as a 'Downpayment' payment, and the bill keeps it as 'Held'.
            decimal deposit = IsWalkIn(bookingSource) ? 0m : HalfOf(subtotal);
            decimal balance = subtotal - deposit;

            string downpaymentStatus = deposit > 0m ? "Held" : "None";
            string paymentStatus = deposit > 0m ? "Partial" : "Unpaid";

            int billingId = BillingRepository.CreateBilling(connection, transaction, appointmentId,
                subtotal, 0, 0m, subtotal, deposit, downpaymentStatus, balance, paymentStatus);

            if (deposit > 0m)
            {
                string method = PickPaymentMethod(random);
                BillingRepository.AddPayment(connection, transaction, billingId, bookedBy, "Downpayment",
                    method, ReferenceFor(method, random, bookedAt), deposit, "Verified");
            }

            InsertHistory(connection, transaction, appointmentId, bookedBy, "Created", null, null, null,
                "Upcoming", "Booked as a " + bookingSource + " appointment.", bookedAt);

            return appointmentId;
        }

        // ---------------------------------------------------------------------
        // SECTION 6 - THE TWO CANCELLED BOOKINGS (deposit forfeited)
        // ---------------------------------------------------------------------

        private static void SeedCancelledVisits(SqlConnection connection, SqlTransaction transaction, Random random,
            int[] clientIds, int adminId, List<int> staffIds, DataTable services, DataTable rooms)
        {
            // Both were dropped with more than three days of notice, so the deposit is forfeited.
            InsertCancelledVisit(connection, transaction, random, clientIds[6], adminId, staffIds[5],
                DateTime.Today.AddDays(-20), new TimeSpan(14, 0, 0),
                FindService(services, "Massage - Full Body", "Swedish Massage (1 hour)"), "Messenger",
                "Client had to travel to Manila and asked to cancel.", rooms);

            InsertCancelledVisit(connection, transaction, random, clientIds[7], adminId, staffIds[0],
                DateTime.Today.AddDays(-12), new TimeSpan(15, 0, 0),
                FindService(services, "Hair Removal and Whitening", "Underarm Whitening"), "Text",
                "Client cancelled because of a skin allergy (see the medical note).", rooms);
        }

        private static void InsertCancelledVisit(SqlConnection connection, SqlTransaction transaction, Random random,
            int clientId, int bookedBy, int technicianId, DateTime date, TimeSpan start, DataRow service,
            string bookingSource, string reason, DataTable rooms)
        {
            if (service == null) return;

            int minutes = Db.Get<int>(service["duration_minutes"], 60);
            TimeSpan end = start.Add(new TimeSpan(0, minutes, 0));
            decimal subtotal = Db.Get<decimal>(service["price"]);
            DateTime bookedAt = date.AddDays(-7);
            DateTime cancelledAt = date.AddDays(-5);

            int appointmentId = InsertAppointment(connection, transaction, clientId, bookedBy, date, start, end,
                bookingSource, "Cancelled", bookedAt, cancelledAt);

            int lineId = InsertLine(connection, transaction, appointmentId, Db.Get<int>(service["service_id"]),
                1, subtotal, 0, 0, 0, "Cancelled");

            InsertStaff(connection, transaction, appointmentId, lineId, technicianId, "Assigned");

            int roomId = RoomForCategory(connection, transaction, rooms, Db.Get<int>(service["category_id"]),
                Db.Get<string>(service["category_name"]));
            if (roomId > 0)
            {
                InsertRoom(connection, transaction, appointmentId, lineId, roomId, "Released");
            }

            decimal deposit = HalfOf(subtotal);
            decimal balance = subtotal - deposit;

            int billingId = BillingRepository.CreateBilling(connection, transaction, appointmentId,
                subtotal, 0, 0m, subtotal, deposit, "Held", balance, "Unpaid");

            string method = PickPaymentMethod(random);
            BillingRepository.AddPayment(connection, transaction, billingId, bookedBy, "Downpayment",
                method, ReferenceFor(method, random, bookedAt), deposit, "Verified");

            // A cancellation always forfeits the money paid upfront (PROJECT_RULES, "Cancel").
            // The status is written AFTER the payment, because AddPayment re-balances the bill.
            Db.Execute(connection, transaction,
                "UPDATE Billing SET downpayment_status = 'Forfeited', payment_status = 'Forfeited', " +
                "       updated_at = GETDATE() " +
                "WHERE billing_id = @billing_id",
                Db.P("@billing_id", billingId));

            InsertHistory(connection, transaction, appointmentId, bookedBy, "Created", null, null, null,
                "Upcoming", "Booked as a " + bookingSource + " appointment.", bookedAt);

            InsertHistory(connection, transaction, appointmentId, bookedBy, "Cancelled", null, null,
                "Upcoming", "Cancelled", reason, cancelledAt);
        }

        // ---------------------------------------------------------------------
        // SECTION 7 - ACTIVITY LOG
        // ---------------------------------------------------------------------

        /// <summary>A handful of log entries spread over the past week, so the page is not empty.</summary>
        private static void SeedActivityLog(SqlConnection connection, SqlTransaction transaction,
            int adminId, List<int> staffIds)
        {
            DateTime today = DateTime.Today;

            InsertActivity(connection, transaction, adminId, "Login", "Authentication",
                "User logged in", today.AddDays(-7).AddHours(8).AddMinutes(5));

            InsertActivity(connection, transaction, adminId, "Add Client", "Clients",
                "Added client Jomar Tiongson", today.AddDays(-6).AddHours(9).AddMinutes(40));

            InsertActivity(connection, transaction, staffIds[0], "Book Appointment", "Appointments",
                "Booked a Messenger appointment with a 50% downpayment", today.AddDays(-5).AddHours(11).AddMinutes(15));

            InsertActivity(connection, transaction, adminId, "Apply Promo", "Billing",
                "Applied the Monthly Glow Promo (15% off)", today.AddDays(-4).AddHours(14).AddMinutes(2));

            InsertActivity(connection, transaction, staffIds[1], "Start Service", "Appointments",
                "Started a scheduled service", today.AddDays(-3).AddHours(13).AddMinutes(30));

            InsertActivity(connection, transaction, adminId, "Checkout", "Billing",
                "Collected the payment and completed an appointment", today.AddDays(-2).AddHours(17).AddMinutes(20));

            InsertActivity(connection, transaction, adminId, "Commission payout", "Commissions",
                "Marked the weekly commissions as paid", today.AddDays(-1).AddHours(18).AddMinutes(10));

            InsertActivity(connection, transaction, adminId, "Backup", "File Maintenance",
                "Backup LittleEreseBackup.bak written and verified", today.AddHours(8).AddMinutes(30));
        }

        // ---------------------------------------------------------------------
        // SMALL INSERT HELPERS (each one writes ONE row and returns its id where useful)
        // ---------------------------------------------------------------------

        private static int InsertAppointment(SqlConnection connection, SqlTransaction transaction,
            int clientId, int userId, DateTime date, TimeSpan start, TimeSpan end, string bookingSource,
            string status, DateTime createdAt, DateTime? cancelledAt)
        {
            object id = Db.Scalar(connection, transaction,
@"INSERT INTO Appointments (client_id, user_id, appointment_date, start_time, end_time, booking_source,
                           status, notes, cancelled_at, created_at, updated_at)
VALUES (@client_id, @user_id, @appointment_date, CAST(@start_time AS time), CAST(@end_time AS time),
        @booking_source, @status, NULL, @cancelled_at, @created_at, @created_at);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@client_id", clientId),
                Db.P("@user_id", userId),
                Db.P("@appointment_date", date.Date),
                Db.P("@start_time", SqlTime(start)),
                Db.P("@end_time", SqlTime(end)),
                Db.P("@booking_source", bookingSource),
                Db.P("@status", status),
                Db.P("@cancelled_at", cancelledAt),
                Db.P("@created_at", createdAt));

            return Convert.ToInt32(id);
        }

        private static int InsertLine(SqlConnection connection, SqlTransaction transaction, int appointmentId,
            int serviceId, int quantity, decimal price, int packageId, int sessionNumber, int totalSessions,
            string status)
        {
            object id = Db.Scalar(connection, transaction,
@"INSERT INTO AppointmentServices (appointment_id, service_id, package_id, quantity, price,
                                  session_number, total_sessions, status)
VALUES (@appointment_id, @service_id, @package_id, @quantity, @price,
        @session_number, @total_sessions, @status);
SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@appointment_id", appointmentId),
                Db.P("@service_id", serviceId),
                Db.P("@package_id", packageId > 0 ? (object)packageId : null),
                Db.P("@quantity", quantity),
                Db.P("@price", price),
                Db.P("@session_number", sessionNumber > 0 ? (object)sessionNumber : null),
                Db.P("@total_sessions", totalSessions > 0 ? (object)totalSessions : null),
                Db.P("@status", status));

            return Convert.ToInt32(id);
        }

        private static void InsertStaff(SqlConnection connection, SqlTransaction transaction, int appointmentId,
            int lineId, int userId, string status)
        {
            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentStaff (appointment_id, appointment_service_id, user_id, status)
VALUES (@appointment_id, @appointment_service_id, @user_id, @status)",
                Db.P("@appointment_id", appointmentId),
                Db.P("@appointment_service_id", lineId),
                Db.P("@user_id", userId),
                Db.P("@status", status));
        }

        private static void InsertRoom(SqlConnection connection, SqlTransaction transaction, int appointmentId,
            int lineId, int roomId, string status)
        {
            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentRooms (appointment_id, appointment_service_id, room_id, status)
VALUES (@appointment_id, @appointment_service_id, @room_id, @status)",
                Db.P("@appointment_id", appointmentId),
                Db.P("@appointment_service_id", lineId),
                Db.P("@room_id", roomId),
                Db.P("@status", status));
        }

        private static void InsertCommission(SqlConnection connection, SqlTransaction transaction, int userId,
            int lineId, int paymentId, decimal amount, string payoutStatus, DateTime? payoutDate)
        {
            Db.Execute(connection, transaction,
@"INSERT INTO Commissions (user_id, appointment_service_id, payment_id, commission_rate,
                          commission_amount, payout_status, payout_date)
VALUES (@user_id, @appointment_service_id, @payment_id, @commission_rate,
        @commission_amount, @payout_status, @payout_date)",
                Db.P("@user_id", userId),
                Db.P("@appointment_service_id", lineId),
                Db.P("@payment_id", paymentId > 0 ? (object)paymentId : null),
                Db.P("@commission_rate", CommissionPercent),
                Db.P("@commission_amount", amount),
                Db.P("@payout_status", payoutStatus),
                Db.P("@payout_date", payoutDate));
        }

        private static void InsertHistory(SqlConnection connection, SqlTransaction transaction, int appointmentId,
            int changedBy, string changeType, DateTime? oldDate, TimeSpan? oldStart, string oldStatus,
            string newStatus, string reason, DateTime changedAt)
        {
            Db.Execute(connection, transaction,
@"INSERT INTO AppointmentHistory (appointment_id, changed_by, change_type, old_appointment_date,
                                 old_start_time, old_status, new_status, reason, changed_at)
VALUES (@appointment_id, @changed_by, @change_type, @old_appointment_date, CAST(@old_start_time AS time),
        @old_status, @new_status, @reason, @changed_at)",
                Db.P("@appointment_id", appointmentId),
                Db.P("@changed_by", changedBy),
                Db.P("@change_type", changeType),
                Db.P("@old_appointment_date", oldDate),
                Db.P("@old_start_time", oldStart.HasValue ? (object)SqlTime(oldStart.Value) : null),
                Db.P("@old_status", oldStatus),
                Db.P("@new_status", newStatus),
                Db.P("@reason", reason),
                Db.P("@changed_at", changedAt));
        }

        private static void InsertActivity(SqlConnection connection, SqlTransaction transaction, int userId,
            string action, string module, string description, DateTime loggedAt)
        {
            Db.Execute(connection, transaction,
@"INSERT INTO ActivityLog (user_id, action, module, description, logged_at)
VALUES (@user_id, @action, @module, @description, @logged_at)",
                Db.P("@user_id", userId),
                Db.P("@action", action),
                Db.P("@module", module),
                Db.P("@description", description),
                Db.P("@logged_at", loggedAt));
        }

        /// <summary>One more package session is done; at the last session the package is 'Completed'.</summary>
        private static void CompletePackageSession(SqlConnection connection, SqlTransaction transaction, int packageId)
        {
            Db.Execute(connection, transaction,
                "UPDATE ClientPackages SET sessions_completed = sessions_completed + 1 " +
                "WHERE package_id = @package_id AND sessions_completed < total_sessions",
                Db.P("@package_id", packageId));

            Db.Execute(connection, transaction,
                "UPDATE ClientPackages SET status = 'Completed' " +
                "WHERE package_id = @package_id AND sessions_completed >= total_sessions",
                Db.P("@package_id", packageId));
        }

        private static int InsertPackage(SqlConnection connection, SqlTransaction transaction, int clientId,
            int serviceId, int totalSessions, DateTime purchaseDate)
        {
            object id = Db.Scalar(connection, transaction,
@"INSERT INTO ClientPackages (client_id, service_id, total_sessions, sessions_completed,
                             purchase_date, expiry_date, status)
VALUES (@client_id, @service_id, @total_sessions, 0, @purchase_date, @expiry_date, 'Active');
SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@client_id", clientId),
                Db.P("@service_id", serviceId),
                Db.P("@total_sessions", totalSessions),
                Db.P("@purchase_date", purchaseDate.Date),
                Db.P("@expiry_date", purchaseDate.Date.AddMonths(6)));

            return Convert.ToInt32(id);
        }

        // ---------------------------------------------------------------------
        // LOOKUPS (every id is found by NAME, never hard-coded)
        // ---------------------------------------------------------------------

        private static int UserId(SqlConnection connection, SqlTransaction transaction, string username)
        {
            object id = Db.Scalar(connection, transaction,
                "SELECT user_id FROM Users WHERE username = @username",
                Db.P("@username", username));

            return id == null || id == DBNull.Value ? 0 : Convert.ToInt32(id);
        }

        private static List<int> StaffIds(SqlConnection connection, SqlTransaction transaction)
        {
            List<int> ids = new List<int>();

            foreach (string username in DemoStaffUsernames)
            {
                int id = UserId(connection, transaction, username);
                if (id > 0) ids.Add(id);
            }

            return ids;
        }

        private static DataTable LoadServices(SqlConnection connection, SqlTransaction transaction)
        {
            return Db.Query(connection, transaction,
@"SELECT s.service_id, s.category_id, c.category_name, s.service_name, s.price,
         s.duration_minutes, s.default_total_sessions
FROM Services s
INNER JOIN ServiceCategories c ON c.category_id = s.category_id
ORDER BY c.category_name, s.service_name");
        }

        private static DataTable LoadRooms(SqlConnection connection, SqlTransaction transaction)
        {
            return Db.Query(connection, transaction,
                "SELECT room_id, room_name, capacity FROM Rooms ORDER BY room_name");
        }

        /// <summary>The service row of one category + name pair, or null when the catalogue has none.</summary>
        private static DataRow FindService(DataTable services, string categoryName, string serviceName)
        {
            foreach (DataRow row in services.Rows)
            {
                if (string.Equals(Db.Get<string>(row["category_name"]), categoryName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(Db.Get<string>(row["service_name"]), serviceName, StringComparison.OrdinalIgnoreCase))
                {
                    return row;
                }
            }

            return null;
        }

        private static DataRow FindServiceById(DataTable services, int serviceId)
        {
            foreach (DataRow row in services.Rows)
            {
                if (Db.Get<int>(row["service_id"]) == serviceId) return row;
            }

            return null;
        }

        /// <summary>
        /// The first service that is sold as a multi-session package (default_total_sessions > 1),
        /// ignoring the one the caller already used. Null when the catalogue has no such service.
        /// </summary>
        private static DataRow FindMultiSessionService(DataTable services, int excludeServiceId)
        {
            foreach (DataRow row in services.Rows)
            {
                int sessions = Db.Get<int>(row["default_total_sessions"], 1);
                if (sessions > 1 && Db.Get<int>(row["service_id"]) != excludeServiceId) return row;
            }

            return null;
        }

        private static int RoomId(DataTable rooms, string roomName)
        {
            foreach (DataRow row in rooms.Rows)
            {
                if (string.Equals(Db.Get<string>(row["room_name"]), roomName, StringComparison.OrdinalIgnoreCase))
                {
                    return Db.Get<int>(row["room_id"]);
                }
            }

            return 0;
        }

        /// <summary>
        /// The room an appointment of this category is held in. The owner's own links in
        /// RoomServiceCategories win; when a category is not linked yet (the demo database has no
        /// links at all) the shop's usual room is used by category name.
        /// POLISH: link the real rooms on the Rooms screen and this fallback is never used again.
        /// </summary>
        private static int RoomForCategory(SqlConnection connection, SqlTransaction transaction,
            DataTable rooms, int categoryId, string categoryName)
        {
            DataTable linked = Db.Query(connection, transaction,
@"SELECT TOP 1 r.room_id
FROM RoomServiceCategories rsc
INNER JOIN Rooms r ON r.room_id = rsc.room_id
WHERE rsc.category_id = @category_id AND r.status = 'Available'
ORDER BY r.capacity DESC, r.room_name",
                Db.P("@category_id", categoryId));

            if (linked.Rows.Count > 0) return Db.Get<int>(linked.Rows[0]["room_id"]);

            if (IsMassageCategory(categoryName)) return RoomId(rooms, "Massage Room");
            if (string.Equals(categoryName, "Facial", StringComparison.OrdinalIgnoreCase))
                return RoomId(rooms, "Facial Room");
            if (string.Equals(categoryName, "Slimming", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(categoryName, "Hair Removal and Whitening", StringComparison.OrdinalIgnoreCase))
                return RoomId(rooms, "Diode Machine Lobby");

            return RoomId(rooms, "Aesthetic Lobby");
        }

        // ---------------------------------------------------------------------
        // SMALL VALUE HELPERS (one place each, so the demo stays consistent)
        // ---------------------------------------------------------------------

        /// <summary>
        /// The discount of one bill, using the ONE formula (PromotionRepository.ComputeDiscount).
        /// WHY CalculateDiscount is not used here: it opens its OWN connection and would wait for
        /// this transaction to commit (a deadlock). The demo promotions are not linked to any
        /// service, so the whole subtotal is eligible and both calls return the same amount; a
        /// promotion that does have service links is simply not applied by the demo.
        /// </summary>
        private static decimal DiscountFor(SqlConnection connection, SqlTransaction transaction,
            int promoId, decimal subtotal)
        {
            if (promoId <= 0 || subtotal <= 0m) return 0m;

            DataTable promo = Db.Query(connection, transaction,
                "SELECT p.discount_type, p.discount_value, " +
                "       (SELECT COUNT(*) FROM PromotionServices ps WHERE ps.promo_id = p.promo_id) AS links " +
                "FROM Promotions p WHERE p.promo_id = @promo_id",
                Db.P("@promo_id", promoId));

            if (promo.Rows.Count == 0) return 0m;

            DataRow row = promo.Rows[0];
            if (Db.Get<int>(row["links"]) > 0) return 0m;

            return PromotionRepository.ComputeDiscount(Db.Get<string>(row["discount_type"], "Percentage"),
                Db.Get<decimal>(row["discount_value"]), subtotal);
        }

        private static bool IsMassageCategory(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return false;

            return categoryName.Trim().StartsWith(AppointmentRepository.MassageCategoryPrefix,
                StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsWalkIn(string bookingSource)
        {
            return string.Equals(bookingSource, "Walk-in", StringComparison.OrdinalIgnoreCase);
        }

        private static decimal HalfOf(decimal amount)
        {
            return Math.Round(amount / 2m, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>The sources a booking can come from, without the walk-in (see IsWalkIn).</summary>
        private static string PickBookingSource(Random random)
        {
            string[] sources = new string[] { "Messenger", "Text", "Phone" };
            return sources[random.Next(0, sources.Length)];
        }

        private static string PickPaymentMethod(Random random)
        {
            int roll = random.Next(0, 10);
            if (roll < 5) return "Cash";
            if (roll < 8) return "GCash";
            return "Bank Transfer";
        }

        /// <summary>A reference number for GCash / Bank Transfer; empty for cash.</summary>
        private static string ReferenceFor(string paymentMethod, Random random, DateTime date)
        {
            if (string.Equals(paymentMethod, "GCash", StringComparison.OrdinalIgnoreCase))
            {
                return "GC" + date.ToString("yyMMdd") + random.Next(100, 999).ToString();
            }

            if (string.Equals(paymentMethod, "Bank Transfer", StringComparison.OrdinalIgnoreCase))
            {
                return "BT" + date.ToString("yyMMdd") + random.Next(1000, 9999).ToString();
            }

            return null;
        }

        /// <summary>A time value written the way SQL Server reads it (see AppointmentRepository).</summary>
        private static string SqlTime(TimeSpan value)
        {
            return value.ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// The database script inserts the admin with the literal text REPLACE_WITH_HASH, which no
        /// password can ever match. Replace it once with a real hash.
        /// </summary>
        private static void EnsureAdminPassword()
        {
            DataTable table = Db.Query(
                "SELECT user_id, password_hash FROM Users WHERE username = @username",
                Db.P("@username", "admin"));

            if (table.Rows.Count == 0) return;

            DataRow row = table.Rows[0];
            string hash = Db.Get<string>(row["password_hash"]);

            if (string.IsNullOrEmpty(hash) || hash.Trim().Equals("REPLACE_WITH_HASH", StringComparison.OrdinalIgnoreCase))
            {
                UserRepository.SetPassword(Db.Get<int>(row["user_id"]), AdminPassword);
            }
        }

        /// <summary>
        /// Creates one staff account, but only when the username is not taken yet.
        /// An account that already exists without a job type gets it here, because on a fresh
        /// database the migrations run BEFORE this seeder and fill the column from the full name
        /// only (an account created before 002 would otherwise stay NULL forever).
        /// </summary>
        private static void EnsureStaff(string fullName, string username, string staffType)
        {
            if (UserRepository.UsernameExists(username))
            {
                Db.Execute(
                    "UPDATE Users SET staff_type = @staff_type " +
                    "WHERE username = @username AND role = 'Staff' AND staff_type IS NULL",
                    Db.P("@staff_type", staffType),
                    Db.P("@username", username));

                return;
            }

            UserRepository.Create(fullName, username, StaffPassword, "Staff", null, staffType);
        }
    }
}
