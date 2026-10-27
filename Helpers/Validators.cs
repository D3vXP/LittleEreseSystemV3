using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// One central place for "is this input acceptable?". Forms call these methods instead of
    /// writing their own rules, so a client phone number is checked the same way on every screen.
    /// </summary>
    internal static class Validators
    {
        // POLISH: The Philippine mobile rule below follows the usual 09XXXXXXXXX and +639XXXXXXXXX shapes.
        // POLISH: Add or remove digits here if the shop later accepts landlines or foreign numbers.
        public const string PhoneRegex = @"^(09\d{9}|\+639\d{9})$";

        // POLISH: 3-20 letters, digits or underscore. Tighten it if usernames must never contain "_".
        public const string UsernameRegex = @"^[A-Za-z0-9_]{3,20}$";

        // POLISH: 2-50 characters: letters, spaces, dots, apostrophes and hyphens (for names like "Ma. Cruz-Santos").
        public const string NameRegex = @"^[A-Za-z .'\-]{2,50}$";

        private const int MinimumPasswordLength = 8;

        // A client must be at least this old on the day of saving (PROJECT_RULES.md).
        public const int MinimumClientAge = 15;

        /// <summary>The one sentence shown (and thrown) when a client is younger than MinimumClientAge.</summary>
        public const string MinimumClientAgeMessage =
            "For safety reasons, we do not accept clients under 15 years old.";

        /// <summary>True when the text has something other than spaces in it.</summary>
        public static bool IsRequired(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Age in whole years on a given day. WHY not (today - birth).Days / 365: that is wrong for
        /// every leap year, so the birthday is compared with AddYears instead.
        /// </summary>
        public static int AgeOn(DateTime birthDate, DateTime day)
        {
            int years = day.Year - birthDate.Year;

            // The birthday of this year has not happened yet, so one year must come off.
            if (birthDate.Date > day.Date.AddYears(-years)) years--;

            return years < 0 ? 0 : years;
        }

        /// <summary>True when the person born on birthDate is at least minimumAge years old on day.</summary>
        public static bool IsAtLeastAge(DateTime birthDate, int minimumAge, DateTime day)
        {
            return birthDate.Date <= day.Date.AddYears(-minimumAge);
        }

        /// <summary>True for a Philippine mobile number written as 09XXXXXXXXX or +639XXXXXXXXX.</summary>
        public static bool IsValidPhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value.Trim(), PhoneRegex);
        }

        /// <summary>True for a person's name: 2 to 50 letters, spaces, dots, apostrophes or hyphens.</summary>
        public static bool IsValidName(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            string name = value.Trim();
            if (!Regex.IsMatch(name, NameRegex)) return false;

            // A name made only of dots / spaces is not a real name.
            return Regex.IsMatch(name, @"[A-Za-z]");
        }

        /// <summary>True for 3 to 20 letters, digits or underscores (a login name).</summary>
        public static bool IsValidUsername(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value.Trim(), UsernameRegex);
        }

        /// <summary>At least 8 characters, with at least one letter and at least one digit.</summary>
        public static bool IsValidPassword(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (value.Length < MinimumPasswordLength) return false;
            if (!Regex.IsMatch(value, "[A-Za-z]")) return false;
            if (!Regex.IsMatch(value, "[0-9]")) return false;
            return true;
        }

        /// <summary>Money amounts must be a real number and strictly greater than zero.</summary>
        public static bool IsPositiveMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            decimal amount;
            // POLISH: InvariantCulture means "1234.50"; switch to TryParse(value, out amount) if staff type commas.
            if (!decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out amount)) return false;
            return amount > 0m;
        }

        /// <summary>Reads a money amount typed by the user. Returns false when it is not a number.</summary>
        public static bool TryMoney(string value, out decimal amount)
        {
            amount = 0m;
            if (string.IsNullOrWhiteSpace(value)) return false;

            // POLISH: same InvariantCulture note as IsPositiveMoney above.
            return decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out amount);
        }

        /// <summary>
        /// Checks the trimmed length of a free text field (service name, promo name, ...).
        /// WHY: names have a database column of 100 characters, and one character is not a name.
        /// </summary>
        public static bool IsLengthBetween(string value, int minimum, int maximum)
        {
            if (value == null) return false;

            int length = value.Trim().Length;
            return length >= minimum && length <= maximum;
        }

        /// <summary>True when the amount is a percentage that makes sense (greater than 0, at most 100).</summary>
        public static bool IsValidPercentage(decimal amount)
        {
            return amount > 0m && amount <= 100m;
        }

        // POLISH: widen the length range or allow "-" if a bank starts using longer or dashed
        // POLISH: reference numbers; the payment screens use this one rule.
        /// <summary>
        /// True for a GCash / bank reference: letters and digits, 6 to 30 characters
        /// (the longest bank reference seen so far is 20).
        /// </summary>
        public static bool IsValidReference(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            string text = value.Trim();
            if (text.Length < 6 || text.Length > 30) return false;

            return Regex.IsMatch(text, @"^[A-Za-z0-9]+$");
        }

        /// <summary>Attaches the red error icon and tooltip to a control.</summary>
        public static void SetError(ErrorProvider provider, Control control, string message)
        {
            if (provider == null || control == null) return;
            provider.SetError(control, message);
        }

        /// <summary>Removes the red error icon from a control.</summary>
        public static void ClearError(ErrorProvider provider, Control control)
        {
            if (provider == null || control == null) return;
            provider.SetError(control, string.Empty);
        }

        /// <summary>
        /// Marks the control when it is empty and returns whether it is filled in.
        /// Usage: if (!Validators.Require(errorProvider1, txtClientName, "Client name is required.")) return;
        /// </summary>
        public static bool Require(ErrorProvider provider, Control control, string message)
        {
            if (control == null) return false;

            string text = control.Text;
            if (!IsRequired(text))
            {
                SetError(provider, control, message);
                return false;
            }

            ClearError(provider, control);
            return true;
        }
    }
}
