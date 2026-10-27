using System.Globalization;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// The ONE money formatter of the project: Philippine peso, en-PH culture, two decimals and
    /// a thousands separator (the peso sign, then 1,234.50).
    /// WHY: each screen used to carry its own format constant, so the same amount could appear as
    /// 5,000.00 on one grid and with a peso sign on another. Every grid column, label, message and
    /// the receipt ask this class instead, so an amount can never look different in two places.
    /// </summary>
    internal static class Money
    {
        /// <summary>The en-PH culture, which is what supplies the peso sign for "C2".</summary>
        public static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("en-PH");

        /// <summary>The format string of every money grid column ("C2" = currency, two decimals).</summary>
        public const string Format = "C2";

        /// <summary>
        /// Formats an amount for a label, a message or the receipt (peso sign, two decimals).
        /// </summary>
        public static string Text(decimal amount)
        {
            return amount.ToString(Format, Culture);
        }

        /// <summary>
        /// Sends one grid column through this formatter. A column whose Format stays "N2" shows
        /// 5,000.00 and no peso sign, so every money column of every grid is styled with this.
        /// The culture also decides how the cell's own Format string is applied.
        /// </summary>
        public static void ApplyTo(System.Windows.Forms.DataGridViewColumn column)
        {
            if (column == null) return;

            column.DefaultCellStyle.Format = Format;
            column.DefaultCellStyle.FormatProvider = Culture;
        }
    }
}
