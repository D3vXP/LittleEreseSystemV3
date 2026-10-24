using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Prints (or previews) whatever a grid currently shows, in landscape, with the shop name and
    /// the date on top. WHY this exists: a report is often needed on paper and the project must
    /// not depend on Crystal Reports or a report designer.
    /// Hidden columns (the id columns) are never printed, and it never throws.
    /// </summary>
    internal static class GridPrinter
    {
        // POLISH: the paper header can be changed here.
        private const string BusinessName = "Little Erese's Aesthetics and Spa";

        /// <summary>
        /// Shows a print preview of the grid with the given title.
        /// A preview is used instead of printing straight away, so paper is never wasted by a
        /// wrong click and the user can pick another printer or save a PDF.
        /// </summary>
        public static void Print(DataGridView grid, string title)
        {
            try
            {
                if (grid == null || grid.Rows.Count == 0)
                {
                    Messages.Warn("There is nothing to print yet.");
                    return;
                }

                using (PrintDocument document = new PrintDocument())
                {
                    document.DocumentName = string.IsNullOrWhiteSpace(title) ? "Report" : title;
                    document.DefaultPageSettings.Landscape = true;
                    document.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

                    GridBody body = new GridBody(grid, document.DocumentName);
                    document.PrintPage += new PrintPageEventHandler(body.Draw);

                    using (PrintPreviewDialog dialog = new PrintPreviewDialog())
                    {
                        dialog.Document = document;
                        dialog.Width = 1000;
                        dialog.Height = 700;
                        dialog.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.Error("The report could not be printed." + Environment.NewLine +
                    (ex == null ? string.Empty : ex.Message));
            }
        }

        /// <summary>
        /// Draws the grid. It is a class (not a lambda) so the handler can remember which row it
        /// reached between pages.
        /// </summary>
        private sealed class GridBody
        {
            private readonly DataGridView _grid;
            private readonly string _title;
            private int _rowIndex;

            public GridBody(DataGridView grid, string title)
            {
                _grid = grid;
                _title = title;
            }

            public void Draw(object sender, PrintPageEventArgs e)
            {
                if (_grid == null || e == null || e.Graphics == null)
                {
                    if (e != null) e.HasMorePages = false;
                    return;
                }

                using (Font caption = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font heading = new Font("Segoe UI Semibold", 13F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font header = new Font("Segoe UI Semibold", 8.5F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font cell = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point))
                using (Brush ink = new SolidBrush(Color.Black))
                {
                    Rectangle bounds = e.MarginBounds;
                    int y = bounds.Top;

                    // The page header repeats on every page, so a loose sheet can still be filed.
                    e.Graphics.DrawString(_title ?? "Report", heading, ink, bounds.Left, y);
                    y += (int)heading.GetHeight(e.Graphics) + 2;

                    e.Graphics.DrawString(BusinessName + "   -   printed " +
                        DateTime.Now.ToString("MMM dd, yyyy h:mm tt"), caption, ink, bounds.Left, y);
                    y += (int)caption.GetHeight(e.Graphics) + 8;

                    int[] widths = ColumnWidths(bounds.Width);
                    int rowHeight = (int)cell.GetHeight(e.Graphics) + 6;

                    // The column captions.
                    int x = bounds.Left;
                    int visible = 0;
                    foreach (DataGridViewColumn column in _grid.Columns)
                    {
                        if (!column.Visible) continue;
                        if (visible >= widths.Length) break;

                        e.Graphics.DrawString(column.HeaderText, header, ink,
                            new RectangleF(x, y, widths[visible], rowHeight));
                        x += widths[visible];
                        visible++;
                    }

                    y += rowHeight;
                    e.Graphics.DrawLine(Pens.Black, bounds.Left, y, bounds.Right, y);
                    y += 4;

                    // The rows, until the paper runs out.
                    while (_rowIndex < _grid.Rows.Count)
                    {
                        DataGridViewRow row = _grid.Rows[_rowIndex];
                        if (row.IsNewRow) { _rowIndex++; continue; }

                        if (y + rowHeight > bounds.Bottom)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        x = bounds.Left;
                        visible = 0;
                        foreach (DataGridViewColumn column in _grid.Columns)
                        {
                            if (!column.Visible) continue;
                            if (visible >= widths.Length) break;

                            e.Graphics.DrawString(Text(row, column), cell, ink,
                                new RectangleF(x, y, widths[visible], rowHeight));
                            x += widths[visible];
                            visible++;
                        }

                        y += rowHeight;
                        _rowIndex++;
                    }

                    e.HasMorePages = false;
                }
            }

            /// <summary>
            /// Spreads the paper width over the visible columns, in proportion to the width the
            /// grid gives them on screen. A very narrow column still gets enough room for a few
            /// characters, so nothing becomes an unreadable sliver.
            /// </summary>
            private int[] ColumnWidths(int pageWidth)
            {
                int count = 0;
                int total = 0;

                foreach (DataGridViewColumn column in _grid.Columns)
                {
                    if (!column.Visible) continue;
                    count++;
                    total += Math.Max(20, column.Width);
                }

                if (count == 0) return new int[0];

                int[] widths = new int[count];
                int index = 0;
                int used = 0;

                foreach (DataGridViewColumn column in _grid.Columns)
                {
                    if (!column.Visible) continue;

                    int share = (int)((long)pageWidth * Math.Max(20, column.Width) / Math.Max(1, total));
                    widths[index] = share;
                    used += share;
                    index++;
                }

                // Rounding leaves a few pixels over; they go to the last column.
                if (widths.Length > 0) widths[widths.Length - 1] += pageWidth - used;
                return widths;
            }

            /// <summary>What the user sees in the cell (after the column format).</summary>
            private static string Text(DataGridViewRow row, DataGridViewColumn column)
            {
                DataGridViewCell cell = row.Cells[column.Index];
                if (cell == null) return string.Empty;

                object value = cell.FormattedValue;
                if (value == null || value == DBNull.Value) return string.Empty;

                string text = value as string;
                return text ?? value.ToString();
            }
        }
    }
}
