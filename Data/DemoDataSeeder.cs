using System;
using System.Data;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Creates the demo accounts so the owner can try the system right after installing it.
    /// Safe to run on every start: it only adds what is missing and never deletes or overwrites data.
    /// </summary>
    internal static class DemoDataSeeder
    {
        public const bool EnableDemoSeed = true; // POLISH: set to false before real deployment

        // POLISH: the owner can change these demo passwords here (or delete the accounts in Staff Accounts).
        private const string AdminPassword = "Admin@123";
        private const string StaffPassword = "Staff@123";

        /// <summary>
        /// (a) Gives the placeholder "admin" a real password, (b) creates the demo staff accounts
        /// if their usernames are still free.
        /// </summary>
        public static void EnsureDemoUsers()
        {
            if (EnableDemoSeed)
            {
                EnsureAdminPassword();

            // The schema has no job-title column, so the staff role title is part of the full name.
            // POLISH: rename these demo staff accounts to the real employees in Staff Accounts.
            EnsureStaff("Aesthetician 1", "aesthetician1");
            EnsureStaff("Aesthetician 2", "aesthetician2");
            EnsureStaff("Aesthetician 3", "aesthetician3");
            EnsureStaff("Aesthetician 4", "aesthetician4");
            EnsureStaff("Massage Therapist 1", "therapist1");
            EnsureStaff("Massage Therapist 2", "therapist2");
            EnsureStaff("Nurse 1", "nurse1");
            }
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

        /// <summary>Creates one staff account, but only when the username is not taken yet.</summary>
        private static void EnsureStaff(string fullName, string username)
        {
            if (UserRepository.UsernameExists(username)) return;

            UserRepository.Create(fullName, username, StaffPassword, "Staff", null);
        }
    }
}
