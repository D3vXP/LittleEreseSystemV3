using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Writes the visible part of a grid to a CSV file the owner can open in Excel.
    /// Hidden columns (the id columns) are left out on purpose: they mean nothing outside the app.
    /// It never throws - a failure is shown with Messages.Error instead.
    /// </summary>
    internal static class CsvExporter
    {
        /// <summary>
        /// Asks for a file name, then writes every visible column and row of the grid.
        /// The file is UTF-8 WITH a byte order mark, so Excel reads the peso sign and the
        /// accented names correctly on a Filipino Windows.
        /// </summary>
        public static void Export(DataGridView grid, string suggestedFileName)
        {
            try
            {
                if (grid == null || grid.Rows.Count == 0)
                {
                    Messages.Warn("There is nothing to export yet.");
                    return;
                }

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "Export to CSV";
                    dialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
                    dialog.FileName = SafeFileName(suggestedFileName);
                    dialog.AddExtension = true;
                    dialog.OverwritePrompt = true;

                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    int written = Write(grid, dialog.FileName);
                    Messages.Info(written + " row(s) exported to:" + Environment.NewLine + dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Messages.Error("The file could not be saved." + Environment.NewLine + DbSafe(ex));
            }
        }

        /// <summary>Writes the grid and returns how many data rows were written.</summary>
        private static int Write(DataGridView grid, string path)
        {
            StringBuilder text = new StringBuilder();

            // Header line: the caption the user sees, so the file reads like the screen.
            bool first = true;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (!column.Visible) continue;

                if (!first) text.Append(',');
                text.Append(Quote(column.HeaderText));
                first = false;
            }
            text.Append("\r\n");

            int rows = 0;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;

                first = true;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    if (!column.Visible) continue;

                    if (!first) text.Append(',');
                    text.Append(Quote(CellText(row, column)));
                    first = false;
                }
                text.Append("\r\n");
                rows++;
            }

            // A BOM makes Excel read the file as UTF-8 instead of the local code page.
            File.WriteAllText(path, text.ToString(), new UTF8Encoding(true));
            return rows;
        }

        /// <summary>
        /// What the user sees in the cell. FormattedCellValue is the text after the column format
        /// (the peso sign, the date pattern) has been applied, which is exactly what the export
        /// should contain.
        /// </summary>
        private static string CellText(DataGridViewRow row, DataGridViewColumn column)
        {
            DataGridViewCell cell = row.Cells[column.Index];
            if (cell == null || cell.Value == null || cell.Value == DBNull.Value) return string.Empty;

            string formatted = cell.FormattedValue as string;
            if (formatted != null) return formatted;

            object value = cell.FormattedValue;
            return value == null ? string.Empty : value.ToString();
        }

        /// <summary>
        /// Puts the text in double quotes and doubles every quote inside it, which is the CSV rule
        /// for text that holds commas, quotes or line breaks.
        /// </summary>
        private static string Quote(string value)
        {
            string text = value == null ? string.Empty : value;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>Removes the characters Windows does not allow in a file name.</summary>
        private static string SafeFileName(string suggestedFileName)
        {
            string name = string.IsNullOrWhiteSpace(suggestedFileName) ? "export" : suggestedFileName.Trim();

            char[] invalid = Path.GetInvalidFileNameChars();
            for (int i = 0; i < invalid.Length; i++)
            {
                name = name.Replace(invalid[i], '-');
            }

            return name;
        }

        /// <summary>A short, safe message for the user (Db.Describe would talk about SQL).</summary>
        private static string DbSafe(Exception ex)
        {
            return ex == null ? string.Empty : ex.Message;
        }
    }
}
