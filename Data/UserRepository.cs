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
                // UPPER on both sides so "Admin" and "admin" are treated as the same username,
                // whatever collation the database happens to use.
                "SELECT COUNT(*) FROM Users WHERE UPPER(username) = UPPER(@username)",
                Db.P("@username", username.Trim()));

            return Convert.ToInt32(count) > 0;
        }

        /// <summary>
        /// Creates a new account with status 'Active' and returns the new user_id.
        /// The plain password is hashed here so it is never stored in the database.
        /// staffType is the job type of a Staff account and is always null for an Admin.
        /// </summary>
        public static int Create(string fullName, string username, string password, string role, string contactNumber,
            string staffType)
        {
            string hash = PasswordHasher.Hash(password);
            string type = NormalizeStaffType(role, staffType);

            object id = Db.Scalar(
                "INSERT INTO Users (full_name, username, password_hash, role, contact_number, status, staff_type) " +
                "VALUES (@full_name, @username, @password_hash, @role, @contact_number, 'Active', @staff_type); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);",
                Db.P("@full_name", fullName),
                Db.P("@username", username),
                Db.P("@password_hash", hash),
                Db.P("@role", role),
                Db.P("@contact_number", contactNumber),
                Db.P("@staff_type", type));

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

        // ---------------------------------------------------------------------
        // Staff Accounts (the Admin-only screen)
        // ---------------------------------------------------------------------

        /// <summary>
        /// The accounts for the Staff Accounts grid.
        ///   search - matches the full name OR the username (empty = everything)
        ///   status - "All" means every status
        /// Returns user_id, full_name, username, role, staff_type, contact_number, status, created_at.
        /// </summary>
        public static DataTable GetUsers(string search, string status)
        {
            string sql =
                "SELECT user_id, full_name, username, role, staff_type, contact_number, status, created_at " +
                "FROM Users " +
                "WHERE (@search = '' OR full_name LIKE @like OR username LIKE @like) " +
                "  AND (@status = 'All' OR status = @status) " +
                "ORDER BY full_name";

            string term = search == null ? string.Empty : search.Trim();

            return Db.Query(sql,
                Db.P("@search", term),
                Db.P("@like", "%" + term + "%"),
                Db.P("@status", status ?? "All"));
        }

        /// <summary>One account row, or null when the id no longer exists.</summary>
        public static DataRow GetById(int userId)
        {
            DataTable table = Db.Query(
                "SELECT user_id, full_name, username, role, staff_type, contact_number, status, created_at " +
                "FROM Users WHERE user_id = @user_id",
                Db.P("@user_id", userId));

            return table.Rows.Count == 0 ? null : table.Rows[0];
        }

        /// <summary>
        /// Updates the editable parts of an account (the username never changes).
        /// Throws InvalidOperationException with a clear message when the change would
        /// lock the shop out: nobody may demote or deactivate themselves, and the last
        /// Active Admin may not be demoted or deactivated either.
        /// staffType is the job type of a Staff account and is always null for an Admin.
        /// </summary>
        public static void Update(int userId, string fullName, string role, string contact, string status,
            string staffType)
        {
            DataRow current = GetById(userId);
            if (current == null)
            {
                throw new InvalidOperationException("This account no longer exists.");
            }

            EnsureRoleAndStatusChangeAllowed(
                userId,
                Db.Get<string>(current["role"]),
                Db.Get<string>(current["status"]),
                role,
                status);

            string type = NormalizeStaffType(role, staffType);

            Db.Execute(
                "UPDATE Users SET full_name = @full_name, role = @role, " +
                "       contact_number = @contact_number, status = @status, staff_type = @staff_type " +
                "WHERE user_id = @user_id",
                Db.P("@full_name", fullName),
                Db.P("@role", role),
                Db.P("@contact_number", contact),
                Db.P("@status", status),
                Db.P("@staff_type", type),
                Db.P("@user_id", userId));
        }

        /// <summary>
        /// Activates ('Active') or deactivates ('Inactive') an account.
        /// The same lock-out rules as Update apply.
        /// </summary>
        public static void SetStatus(int userId, string status)
        {
            DataRow current = GetById(userId);
            if (current == null)
            {
                throw new InvalidOperationException("This account no longer exists.");
            }

            EnsureRoleAndStatusChangeAllowed(
                userId,
                Db.Get<string>(current["role"]),
                Db.Get<string>(current["status"]),
                Db.Get<string>(current["role"]),
                status);

            Db.Execute(
                "UPDATE Users SET status = @status WHERE user_id = @user_id",
                Db.P("@status", status),
                Db.P("@user_id", userId));
        }

        /// <summary>How many accounts are Admin AND Active. Used to protect the last one.</summary>
        public static int CountActiveAdmins()
        {
            object count = Db.Scalar(
                "SELECT COUNT(*) FROM Users WHERE role = 'Admin' AND status = 'Active'");

            return Convert.ToInt32(count);
        }

        /// <summary>
        /// The signed-in user changes their own password. The current password must be right.
        /// Returns false when it is wrong (the caller shows "Current password is incorrect.").
        /// </summary>
        public static bool ChangeOwnPassword(int userId, string currentPassword, string newPassword)
        {
            DataTable table = Db.Query(
                "SELECT password_hash FROM Users WHERE user_id = @user_id",
                Db.P("@user_id", userId));

            if (table.Rows.Count == 0) return false;

            string storedHash = Db.Get<string>(table.Rows[0]["password_hash"]);
            if (!PasswordHasher.Verify(currentPassword, storedHash)) return false;

            SetPassword(userId, newPassword);
            return true;
        }

        /// <summary>
        /// The technicians a booking may be assigned to: role Staff and status Active.
        /// Returns user_id, full_name - used by the technician drop-downs later.
        /// </summary>
        public static DataTable GetActiveStaff()
        {
            return Db.Query(
                "SELECT user_id, full_name FROM Users " +
                "WHERE role = 'Staff' AND status = 'Active' " +
                "ORDER BY full_name");
        }

        /// <summary>
        /// The technicians of one service category: Active Staff whose staff_type matches the
        /// category ("Massage..." categories need a Massage Therapist, every other category an
        /// Aesthetician). A staff member typed 'Other' or not typed at all appears in every list.
        /// Returns user_id, full_name.
        /// POLISH: nurses never appear here - an assessment dropdown uses GetActiveNurses instead.
        /// </summary>
        public static DataTable GetActiveStaffFor(string categoryName)
        {
            string wantedType = IsMassageCategoryName(categoryName) ? "Massage Therapist" : "Aesthetician";

            return Db.Query(
                "SELECT user_id, full_name FROM Users " +
                "WHERE role = 'Staff' AND status = 'Active' " +
                "  AND (staff_type = @staff_type OR staff_type = 'Other' OR staff_type IS NULL) " +
                "ORDER BY full_name",
                Db.P("@staff_type", wantedType));
        }

        /// <summary>
        /// The nurses a service line may be assessed by: Active Staff with staff_type 'Nurse'.
        /// Returns user_id, full_name.
        /// </summary>
        public static DataTable GetActiveNurses()
        {
            return Db.Query(
                "SELECT user_id, full_name FROM Users " +
                "WHERE role = 'Staff' AND status = 'Active' AND staff_type = 'Nurse' " +
                "ORDER BY full_name");
        }

        /// <summary>
        /// An Admin has no job type (the column stays NULL), a Staff account must have one
        /// (the form asks for it, this is the second check the database rules need).
        /// </summary>
        private static string NormalizeStaffType(string role, string staffType)
        {
            if (IsAdminRole(role)) return null;

            if (string.IsNullOrWhiteSpace(staffType))
            {
                throw new InvalidOperationException(Validators.StaffTypeRequiredMessage);
            }

            return staffType.Trim();
        }

        /// <summary>
        /// True for a massage category. The category name is the only signal the schema has,
        /// so this reads the same prefix AppointmentRepository uses for the opening hours.
        /// </summary>
        private static bool IsMassageCategoryName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return false;

            return categoryName.Trim().StartsWith(AppointmentRepository.MassageCategoryPrefix,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// One place for the "do not lock yourself out" rules, shared by Update and SetStatus.
        /// WHY: without this the owner could deactivate the only Admin account and nobody
        /// could ever sign in to fix it.
        /// </summary>
        private static void EnsureRoleAndStatusChangeAllowed(
            int userId, string currentRole, string currentStatus, string newRole, string newStatus)
        {
            bool isSelf = userId == Session.UserId;
            bool losesAdminRole = IsAdminRole(currentRole) && !IsAdminRole(newRole);
            bool becomesInactive = IsActiveStatus(currentStatus) && !IsActiveStatus(newStatus);

            if (isSelf && losesAdminRole)
            {
                throw new InvalidOperationException("You cannot change your own role.");
            }

            if (isSelf && becomesInactive)
            {
                throw new InvalidOperationException("You cannot deactivate your own account.");
            }

            // Only the LAST Active Admin is protected - a second Admin may be changed freely.
            if (IsAdminRole(currentRole) && IsActiveStatus(currentStatus) &&
                (losesAdminRole || becomesInactive) && CountActiveAdmins() <= 1)
            {
                throw new InvalidOperationException(
                    "This is the last active Admin. Promote or activate another Admin first.");
            }
        }

        private static bool IsAdminRole(string role)
        {
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsActiveStatus(string status)
        {
            return string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase);
        }
    }
}
