using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Prints a simple 80 mm style receipt for one bill, or shows it in a preview window.
    /// WHY a PrintDocument drawn line by line: the shop only needs a narrow counter receipt and
    /// the project must not depend on Crystal Reports or a report designer.
    /// Every failure is shown with Messages.Error instead of reaching the user as a raw exception.
    /// </summary>
    internal static class ReceiptPrinter
    {
        // POLISH: change the receipt header text here if the shop's address changes.
        private const string BusinessName = "Little Erese's Aesthetics and Spa";
        private const string BusinessAddress = "San Nicolas Poblacion, Concepcion, Tarlac";

        // The narrow roll: 80 mm is about 302 printer units; the text fits inside the margins.
        private const int ReceiptWidth = 280;

        private const string MonoFontName = "Consolas";

        /// <summary>
        /// Prints the receipt of one bill. preview = true shows it in a PrintPreviewDialog,
        /// otherwise it goes to the default printer. Never throws.
        /// </summary>
        public static void Print(int billingId, bool preview)
        {
            try
            {
                if (billingId <= 0)
                {
                    Messages.Warn("There is no receipt to print yet.");
                    return;
                }

                DataTable data = BillingRepository.GetReceiptData(billingId);
                if (data == null || data.Rows.Count == 0)
                {
                    Messages.Warn("This bill has nothing to print.");
                    return;
                }

                using (PrintDocument document = new PrintDocument())
                {
                    document.DocumentName = ReceiptNumber(billingId);

                    // A narrow roll keeps the printer from scaling the text down.
                    try
                    {
                        document.DefaultPageSettings.PaperSize = new PaperSize("Receipt80", ReceiptWidth, 900);
                        document.DefaultPageSettings.Margins = new Margins(14, 14, 14, 14);
                    }
                    catch (InvalidOperationException)
                    {
                        // A printer that refuses a custom paper size simply keeps its own.
                    }

                    ReceiptBody body = new ReceiptBody(data);
                    document.PrintPage += new PrintPageEventHandler(body.Draw);

                    if (preview)
                    {
                        using (PrintPreviewDialog dialog = new PrintPreviewDialog())
                        {
                            dialog.Document = document;
                            dialog.Width = 520;
                            dialog.Height = 700;
                            dialog.ShowDialog();
                        }
                    }
                    else
                    {
                        document.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.DbError(ex);
            }
        }

        /// <summary>OR-yyyyMMdd-&lt;billing_id&gt;: the number the counter quotes when a client calls back.</summary>
        private static string ReceiptNumber(int billingId)
        {
            return "OR-" + DateTime.Now.ToString("yyyyMMdd") + "-" + billingId;
        }

        /// <summary>
        /// Draws the whole receipt. It is a class (not a lambda) so the page handler can keep track
        /// of how far it has printed between pages.
        /// </summary>
        private sealed class ReceiptBody
        {
            private readonly DataTable _data;
            private int _rowIndex;

            public ReceiptBody(DataTable data)
            {
                _data = data;
            }

            public void Draw(object sender, PrintPageEventArgs e)
            {
                if (_data == null || _data.Rows.Count == 0)
                {
                    e.HasMorePages = false;
                    return;
                }

                const int left = 0;
                int y = 0;

                using (Font normal = new Font(MonoFontName, 8.5F, FontStyle.Regular, GraphicsUnit.Point))
                using (Font bold = new Font(MonoFontName, 9F, FontStyle.Bold, GraphicsUnit.Point))
                using (Font heading = new Font(MonoFontName, 10F, FontStyle.Bold, GraphicsUnit.Point))
                using (Brush ink = new SolidBrush(Color.Black))
                {
                    int width = e.MarginBounds.Width;
                    int line = normal.Height + 3;

                    DataRow first = _data.Rows[0];

                    // Header: only on the first page, so a long receipt does not repeat the shop name.
                    if (_rowIndex == 0)
                    {
                        DrawCentered(e, ink, heading, BusinessName, left, width, ref y);
                        DrawCentered(e, ink, normal, BusinessAddress, left, width, ref y);
                        y += 4;
                        DrawCentered(e, ink, bold, "OFFICIAL RECEIPT", left, width, ref y);
                        DrawCentered(e, ink, normal, ReceiptNumber(Db.Get<int>(first["billing_id"])), left, width, ref y);
                        y += 2;
                        DrawRule(e, ink, left, width, ref y);

                        DrawPair(e, ink, normal, "Date", DateTime.Now.ToString("MMM dd, yyyy h:mm tt"), left, width, ref y);
                        DrawPair(e, ink, normal, "Client", Db.Get<string>(first["client_name"], "-"), left, width, ref y);

                        DateTime visit = Db.Get<DateTime>(first["appointment_date"], DateTime.Today);
                        DrawPair(e, ink, normal, "Visit", visit.ToString("MMM dd, yyyy"), left, width, ref y);
                        y += 2;
                        DrawRule(e, ink, left, width, ref y);
                    }

                    // The service lines.
                    while (_rowIndex < _data.Rows.Count)
                    {
                        DataRow row = _data.Rows[_rowIndex];

                        // A new page is started when the paper runs out mid receipt.
                        if (y + (line * 2) > e.MarginBounds.Height)
                        {
                            e.HasMorePages = true;
                            return;
                        }

                        int quantity = Db.Get<int>(row["quantity"], 1);
                        if (quantity < 1) quantity = 1;

                        decimal price = Db.Get<decimal>(row["price"]);
                        decimal amount = price * quantity;

                        e.Graphics.DrawString(Db.Get<string>(row["service_name"], "-"), normal, ink, left, y);
                        y += line;

                        // "  2 x 500.00" on the left, the line amount on the right.
                        string details = "  " + quantity + " x " + Money.Text(price);
                        string lineAmount = Money.Text(amount);
                        e.Graphics.DrawString(details, normal, ink, left, y);
                        e.Graphics.DrawString(lineAmount, normal, ink,
                            left + width - Measure(e, normal, lineAmount), y);
                        y += line;

                        _rowIndex++;
                    }

                    // Only the totals, the payment line and the footer fit on the last page.
                    DataRow totals = _data.Rows[_data.Rows.Count - 1];

                    y += 2;
                    DrawRule(e, ink, left, width, ref y);

                    DrawPair(e, ink, normal, "Subtotal", Money.Text(Db.Get<decimal>(totals["subtotal"])),
                        left, width, ref y);
                    DrawPair(e, ink, normal, "Discount", Money.Text(Db.Get<decimal>(totals["discount_amount"])),
                        left, width, ref y);
                    DrawPair(e, ink, normal, "Paid upfront", Money.Text(Db.Get<decimal>(totals["amount_paid"])),
                        left, width, ref y);

                    e.Graphics.DrawString("Total", bold, ink, left, y);
                    string grandTotal = Money.Text(Db.Get<decimal>(totals["total_amount"]));
                    e.Graphics.DrawString(grandTotal, bold, ink,
                        left + width - Measure(e, bold, grandTotal), y);
                    y += line;

                    string method = Db.Get<string>(totals["payment_method"], "-");
                    string reference = Db.Get<string>(totals["reference_number"]);
                    DrawPair(e, ink, normal, "Payment", method, left, width, ref y);

                    if (!string.IsNullOrWhiteSpace(reference))
                    {
                        DrawPair(e, ink, normal, "Reference", reference, left, width, ref y);
                    }

                    string staff = Db.Get<string>(totals["staff_names"]);
                    if (!string.IsNullOrWhiteSpace(staff))
                    {
                        DrawPair(e, ink, normal, "Attending", staff, left, width, ref y);
                    }

                    y += 2;
                    DrawRule(e, ink, left, width, ref y);
                    DrawCentered(e, ink, normal, "Thank you for trusting Little Erese's.", left, width, ref y);

                    e.HasMorePages = false;
                }
            }

            /// <summary>"Label   value" on one line, with the value pushed to the right edge.</summary>
            private static void DrawPair(PrintPageEventArgs e, Brush ink, Font font, string label, string value,
                int left, int width, ref int y)
            {
                if (string.IsNullOrEmpty(value)) value = "-";

                e.Graphics.DrawString(label, font, ink, left, y);

                int valueWidth = Measure(e, font, value);
                e.Graphics.DrawString(value, font, ink, left + width - valueWidth, y);

                y += font.Height + 3;
            }

            private static void DrawCentered(PrintPageEventArgs e, Brush ink, Font font, string text,
                int left, int width, ref int y)
            {
                int textWidth = Measure(e, font, text);
                e.Graphics.DrawString(text, font, ink, left + ((width - textWidth) / 2), y);
                y += font.Height + 3;
            }

            private static void DrawRule(PrintPageEventArgs e, Brush ink, int left, int width, ref int y)
            {
                e.Graphics.DrawLine(Pens.Black, left, y, left + width, y);
                y += 6;
            }

            private static int Measure(PrintPageEventArgs e, Font font, string text)
            {
                if (string.IsNullOrEmpty(text)) return 0;
                return (int)Math.Ceiling(e.Graphics.MeasureString(text, font).Width);
            }
        }
    }
}
