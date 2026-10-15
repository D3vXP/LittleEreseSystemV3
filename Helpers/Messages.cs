using System;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Every message box in the app goes through here, so all of them look the same
    /// and always carry the shop's name in the title bar.
    /// </summary>
    internal static class Messages
    {
        public const string Title = "Little Erese's";

        public static void Info(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message)
        {
            MessageBox.Show(message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Yes/No question. The default button is No, so pressing Enter by accident
        /// never deletes or changes anything.
        /// </summary>
        public static bool Confirm(string message)
        {
            return MessageBox.Show(message, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>Shows a database error in plain English instead of a raw SQL Server message.</summary>
        public static void DbError(Exception ex)
        {
            if (ex == null) return;
            Error(Db.Describe(ex));
        }
    }
}
