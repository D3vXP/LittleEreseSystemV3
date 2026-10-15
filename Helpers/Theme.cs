using System;
using System.Drawing;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// The single place that holds the spa's colors, fonts and the shared control styles.
    /// It touches no files and no database, so it is safe to call from anywhere.
    ///
    /// Note: .Designer.cs files must NOT call these methods (the Designer cannot run them),
    /// they copy the same Color.FromArgb literals instead. These methods are for the
    /// Load events (runtime styling).
    /// </summary>
    internal static class Theme
    {
        // ---- Brand palette ----
        public static readonly Color Primary = Color.FromArgb(0xA4, 0x7B, 0x2B);
        public static readonly Color PrimaryLight = Color.FromArgb(0xC3, 0x9D, 0x4A);
        public static readonly Color PrimaryDark = Color.FromArgb(0x7A, 0x59, 0x1C);
        public static readonly Color Accent = Color.FromArgb(0xE6, 0xA3, 0x0F);
        public static readonly Color SidebarBack = Color.FromArgb(0x2A, 0x24, 0x20);
        public static readonly Color SidebarText = Color.FromArgb(0xF5, 0xEF, 0xE0);
        public static readonly Color SidebarHover = Color.FromArgb(0x3A, 0x32, 0x2B);
        public static readonly Color AppBack = Color.FromArgb(0xF7, 0xF3, 0xEA);
        public static readonly Color Surface = Color.FromArgb(0xFF, 0xFF, 0xFF);
        public static readonly Color Border = Color.FromArgb(0xE3, 0xDA, 0xC6);
        public static readonly Color TextDark = Color.FromArgb(0x2B, 0x26, 0x22);
        public static readonly Color TextMuted = Color.FromArgb(0x7A, 0x72, 0x66);
        public static readonly Color Success = Color.FromArgb(0x3E, 0x8E, 0x5A);
        public static readonly Color Warning = Color.FromArgb(0xD9, 0x8E, 0x04);
        public static readonly Color Danger = Color.FromArgb(0xC0, 0x39, 0x2B);
        public static readonly Color Info = Color.FromArgb(0x3B, 0x7D, 0xA8);

        // The zebra stripe used by every grid (kept here so the Designer can copy it).
        public static readonly Color AlternateRow = Color.FromArgb(0xFB, 0xF8, 0xF1);

        // ---- Status colors (appointment status, payment status, promotion status) ----
        public static readonly Color StatusUpcoming = PrimaryLight;
        public static readonly Color StatusOngoing = Info;
        public static readonly Color StatusCompleted = Success;
        public static readonly Color StatusCancelled = Danger;
        public static readonly Color StatusRescheduled = Warning;

        // ---- Fonts ----
        public static readonly Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font BoldFont = new Font("Segoe UI Semibold", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font HeadingFont = new Font("Segoe UI Semibold", 16F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 22F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font SmallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font IconFont = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular, GraphicsUnit.Point);

        /// <summary>Gold button with white text - used for the main action on a screen.</summary>
        public static void StylePrimaryButton(Button button)
        {
            if (button == null) return;
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;
            button.Font = BodyFont;
            button.BackColor = Primary;
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = PrimaryDark;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = PrimaryLight;
            button.FlatAppearance.MouseDownBackColor = PrimaryDark;
        }

        /// <summary>White button with a gold border - used for Cancel, Close and other light actions.</summary>
        public static void StyleSecondaryButton(Button button)
        {
            if (button == null) return;
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;
            button.Font = BodyFont;
            button.BackColor = Color.White;
            button.ForeColor = Primary;
            button.FlatAppearance.BorderColor = Primary;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = AppBack;
            button.FlatAppearance.MouseDownBackColor = Border;
        }

        /// <summary>Red button - used for Delete and other actions that cannot be undone.</summary>
        public static void StyleDangerButton(Button button)
        {
            if (button == null) return;
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;
            button.Font = BodyFont;
            button.BackColor = Danger;
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Danger;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0xD0, 0x50, 0x40);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0xA5, 0x2F, 0x22);
        }

        /// <summary>Gives every DataGridView in the app the same clean, readable look.</summary>
        public static void StyleGrid(DataGridView grid)
        {
            if (grid == null) return;

            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.GridColor = Border;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.RowTemplate.Height = 30;
            grid.ColumnHeadersHeight = 34;
            grid.Font = BodyFont;

            grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryDark;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = BoldFont;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = PrimaryDark;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 4, 0);

            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = TextDark;
            grid.DefaultCellStyle.SelectionBackColor = PrimaryLight;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = AlternateRow;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = PrimaryLight;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }

        /// <summary>
        /// Maps a status word (from the database) to the color used in grids and labels.
        /// Anything unknown becomes the muted gray so a new status never crashes the screen.
        /// </summary>
        public static Color StatusColor(string status)
        {
            if (string.IsNullOrEmpty(status)) return TextMuted;

            string value = status.Trim().ToLowerInvariant();
            switch (value)
            {
                case "upcoming":
                    return StatusUpcoming;
                case "ongoing":
                    return StatusOngoing;
                case "completed":
                    return StatusCompleted;
                case "cancelled":
                case "canceled":
                    return StatusCancelled;
                case "rescheduled":
                    return StatusRescheduled;

                // Payment / promotion / account words that mean "good".
                case "paid":
                case "verified":
                case "active":
                case "applied":
                    return Success;

                // Words that mean "something is wrong or finished".
                case "unpaid":
                case "rejected":
                case "inactive":
                case "forfeited":
                case "expired":
                    return Danger;

                // Words that need someone to look at them.
                case "partial":
                case "pending":
                case "held":
                    return Warning;

                default:
                    return TextMuted;
            }
        }
    }
}
