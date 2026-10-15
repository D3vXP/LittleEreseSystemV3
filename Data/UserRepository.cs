using System;
using System.Data;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3.Data
{
    /// <summary>
    /// Every database rule about Users (login, unique usernames, creating accounts) lives here.
    /// Forms only call these methods - they never write SQL themselves.
    /// </summary>
    internal static class UserRepository
    {
        /// <summary>
        /// Checks a username and password pair.
        /// Returns the user row (user_id, full_name, username, password_hash, role, status) when the
        /// account is Active and the password matches, otherwise null. Never throws for a wrong login.
        /// </summary>
        public static DataRow Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            DataTable table = Db.Query(
                "SELECT user_id, full_name, username, password_hash, role, status " +
                "FROM Users WHERE username = @username",
                Db.P("@username", username.Trim()));

            if (table.Rows.Count == 0) return null;

            DataRow row = table.Rows[0];

            // A disabled account must not be able to sign in, even with the right password.
            string status = Db.Get<string>(row["status"]);
            if (!string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase)) return null;

            string storedHash = Db.Get<string>(row["password_hash"]);
            if (!PasswordHasher.Verify(password, storedHash)) return null;

            return row;
        }

        /// <summary>True when a user with this username already exists (usernames are unique in the table).</summary>
        public static bool UsernameExists(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            object count = Db.Scalar(
                "SELECT COUNT(*) FROM Users WHERE username = @username",
                Db.P("@username", username.Trim()));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Creates a new account with status 'Active' and returns the new user_id.
        /// The plain password is hashed here so it is never stored in the database.
        /// </summary>
        public static int Create(string fullName, string username, string password, string role, string contactNumber)
        {
            string hash = PasswordHasher.Hash(password);

            object id = Db.Scalar(
                "INSERT INTO Users (full_name, username, password_hash, role, contact_number, status) " +
                "VALUES (@full_name, @username, @password_hash, @role, @contact_number, 'Active'); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@full_name", fullName),
                Db.P("@username", username),
                Db.P("@password_hash", hash),
                Db.P("@role", role),
                Db.P("@contact_number", contactNumber));

            return Convert.ToInt32(id);
        }

        /// <summary>Replaces the password of an existing account (also used to upgrade an old hash).</summary>
        public static void SetPassword(int userId, string newPassword)
        {
            Db.Execute(
                "UPDATE Users SET password_hash = @password_hash WHERE user_id = @user_id",
                Db.P("@password_hash", PasswordHasher.Hash(newPassword)),
                Db.P("@user_id", userId));
        }
    }
}
