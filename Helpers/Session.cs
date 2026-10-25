using System;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Remembers who is signed in for the current run of the app.
    /// This is memory only - it never reads or writes the database or any file.
    /// MainForm (and the screens that use it) read these properties instead of asking the database again.
    /// </summary>
    internal static class Session
    {
        /// <summary>The user_id of the signed-in user (0 when nobody is signed in).</summary>
        public static int UserId { get; private set; }

        /// <summary>The signed-in user's full name, for greetings and receipts.</summary>
        public static string FullName { get; private set; }

        /// <summary>The signed-in user's username, for the activity log description.</summary>
        public static string Username { get; private set; }

        /// <summary>The signed-in user's role: "Admin" or "Staff".</summary>
        public static string Role { get; private set; }

        /// <summary>True after a successful login and until End() is called.</summary>
        public static bool IsLoggedIn
        {
            get { return UserId > 0; }
        }

        /// <summary>True only for the owner account. "admin" in the database is still an Admin.</summary>
        public static bool IsAdmin
        {
            get { return string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase); }
        }

        /// <summary>Called by the login screen after the password is verified.</summary>
        public static void Start(int userId, string fullName, string username, string role)
        {
            UserId = userId;
            FullName = fullName ?? string.Empty;
            Username = username ?? string.Empty;
            Role = role ?? string.Empty;
        }

        /// <summary>Called when the user logs out or the app closes.</summary>
        public static void End()
        {
            UserId = 0;
            FullName = null;
            Username = null;
            Role = null;
        }
    }
}
