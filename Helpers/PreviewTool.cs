using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// A small photo booth for the screens, started with:
    ///     LittleEreseSystemV3.exe --preview LoginForm
    ///     LittleEreseSystemV3.exe --preview ucDashboard --role Staff
    ///
    /// It shows one form (or one user control inside a hosting form) for under a second,
    /// draws the WHOLE window into a PNG in the previews folder next to the exe and closes again.
    /// Nothing is copied off the screen in the normal case, so the picture always shows the full
    /// size of the window and the monitor it was taken on cannot cut anything off.
    /// This exists so layout problems - overlaps, cut off text, controls in the wrong order -
    /// can be seen without clicking through the whole app by hand.
    ///
    /// It never throws: any problem is written to previews\&lt;ClassName&gt;.error.txt.
    /// It always ends the process, so the caller only has to look at the exit code.
    /// </summary>
    internal static class PreviewTool
    {
        private const string FormsNamespace = "LittleEreseSystemV3.Forms";
        private const string ControlsNamespace = "LittleEreseSystemV3.Controls";

        // How long the screen is allowed to settle before the picture is taken.
        private const int SettleMilliseconds = 800;
        private const int PumpStepMilliseconds = 20;

        // The hosting form used for a UserControl: the content area a maximized MainForm can
        // leave free on the smallest supported monitor (see PROJECT_RULES, "Screen size").
        private const int PageHostWidth = 1240;
        private const int PageHostHeight = 600;

        // The window is parked out of sight while it is prepared. The picture is drawn, not
        // photographed, so the position has no effect on the result.
        private const int OffScreenX = -32000;
        private const int OffScreenY = -32000;

        // A monitor with 150% scaling would make every screen lay out half again as big, which
        // would make the picture depend on the monitor it was taken on. For this one run the
        // thread tells Windows to treat it as unscaled, so the PNG always shows the sizes the
        // Designer shows.
        private const int DpiAwarenessUnaware = -1;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetThreadDpiAwarenessContext(IntPtr value);

        /// <summary>
        /// Entry point for the --preview command line switch.
        /// Reads the class name and the optional role, takes the picture, then exits.
        /// </summary>
        public static void Run(string[] args)
        {
            string className = null;
            string role = "Admin";
            string error = null;

            try
            {
                className = ValueAfter(args, "--preview");
                string requestedRole = ValueAfter(args, "--role");
                if (!string.IsNullOrEmpty(requestedRole)) role = requestedRole.Trim();

                UseLogicalPixels();

                if (string.IsNullOrEmpty(className))
                {
                    error = "No class name given. Usage: --preview <ClassName> [--role Admin|Staff]";
                }
                else
                {
                    // Forms read Session in their Load events, so a preview needs a signed in user.
                    Session.Start(1, "Preview User", "preview", role);
                    error = Capture(className, role);
                }
            }
            catch (Exception ex)
            {
                error = Describe(ex);
            }

            if (error == null)
            {
                Environment.Exit(0);
            }
            else
            {
                WriteErrorFile(className, error);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Asks Windows to treat this thread as if the screen had no scaling, so the layout keeps
        /// the size the Designer shows no matter which monitor the app runs on.
        /// Older Windows versions do not have this call; there the picture is simply scaled up.
        /// </summary>
        private static void UseLogicalPixels()
        {
            try
            {
                SetThreadDpiAwarenessContext(new IntPtr(DpiAwarenessUnaware));
            }
            catch
            {
                // Not supported here, so the picture is taken at the monitor's scaling.
            }
        }

        /// <summary>
        /// Shows the requested screen once and saves the PNG.
        /// Returns null when the picture was written, or the problem text when it was not.
        /// </summary>
        private static string Capture(string className, string role)
        {
            Type type = FindClass(className);
            if (type == null)
            {
                return "Class '" + className + "' was not found in " + FormsNamespace + " or " + ControlsNamespace + ".";
            }

            // A form is shown as it is; a page is put inside a plain form so it has a parent
            // and a real size, exactly like MainForm does at runtime.
            bool isPage = typeof(UserControl).IsAssignableFrom(type);
            if (!isPage && !typeof(Form).IsAssignableFrom(type))
            {
                return "Class '" + className + "' is neither a Form nor a UserControl.";
            }

            Form window = null;

            try
            {
                window = CreateWindow(type, className, isPage);

                // Off screen and out of the task bar, so taking a preview does not disturb
                // whatever the user is doing at that moment.
                window.ShowInTaskbar = false;
                window.StartPosition = FormStartPosition.Manual;
                window.Location = new Point(OffScreenX, OffScreenY);
                window.Show();

                // Layout and any runtime work in the Load event need the message loop to run,
                // so keep the window responsive for a moment before taking the picture.
                Pump(SettleMilliseconds);

                PrepareForPicture(window);

                window.Refresh();
                Pump(100);

                Save(window, className);

                return null;
            }
            catch (Exception ex)
            {
                return Describe(ex);
            }
            finally
            {
                if (window != null)
                {
                    try { window.Close(); }
                    catch { /* closing a broken window must not hide the result */ }

                    try { window.Dispose(); }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Builds the window to photograph: the form itself, or a host form with the page docked in it.
        /// </summary>
        private static Form CreateWindow(Type type, string className, bool isPage)
        {
            object instance = Activator.CreateInstance(type);

            if (!isPage)
            {
                Form form = instance as Form;
                if (form == null) throw new InvalidOperationException("'" + className + "' could not be created as a Form.");
                return form;
            }

            UserControl page = instance as UserControl;
            if (page == null) throw new InvalidOperationException("'" + className + "' could not be created as a UserControl.");

            Form host = new Form();
            host.Text = className + " (preview)";
            host.BackColor = Color.FromArgb(0xF7, 0xF3, 0xEA);
            host.ClientSize = new Size(PageHostWidth, PageHostHeight);
            host.MinimumSize = new Size(400, 300);
            host.StartPosition = FormStartPosition.Manual;
            host.Location = new Point(OffScreenX, OffScreenY);

            page.Dock = DockStyle.Fill;
            host.Controls.Add(page);

            return host;
        }

        /// <summary>
        /// Puts the window back to the size it has when it is not filling the screen. A form whose
        /// Load event maximizes it (MainForm) would otherwise be as tall as the monitor, and the
        /// picture is meant to show the whole window whatever the monitor is.
        /// </summary>
        private static void PrepareForPicture(Form window)
        {
            Size normal = window.WindowState == FormWindowState.Normal ? window.Size : window.RestoreBounds.Size;
            if (normal.Width < 1 || normal.Height < 1) return;

            window.WindowState = FormWindowState.Normal;
            window.Size = normal;
            window.Location = new Point(OffScreenX, OffScreenY);
            window.PerformLayout();

            Pump(80);
        }

        /// <summary>
        /// Draws the whole window (title bar and border included) into previews\&lt;ClassName&gt;.png
        /// next to the exe, at its full size.
        /// </summary>
        private static void Save(Form window, string className)
        {
            string path = Path.Combine(PreviewFolder(), className + ".png");

            // Drawing a window can come out blank on some systems (a window that never painted
            // itself). That is checked, and only then is the screen photographed - the single case
            // where a preview can be smaller than the window.
            if (DrawWindowToFile(window, path)) return;

            CopyScreenToFile(window, path);
        }

        /// <summary>
        /// Draws the window with Control.DrawToBitmap. Returns false when the result looks empty,
        /// so the caller knows to fall back to copying the screen.
        /// </summary>
        private static bool DrawWindowToFile(Form window, string path)
        {
            try
            {
                Size size = window.Size;
                if (size.Width < 1 || size.Height < 1) size = window.ClientSize;
                if (size.Width < 1 || size.Height < 1) return false;

                using (Bitmap picture = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb))
                {
                    window.DrawToBitmap(picture, new Rectangle(0, 0, size.Width, size.Height));

                    if (IsBlank(picture)) return false;

                    picture.Save(path, ImageFormat.Png);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the part of the screen the window occupies. Fallback for windows DrawToBitmap
        /// cannot handle; the window is shrunk to the working area first, because anything outside
        /// the screen would be missing from the copy anyway.
        /// </summary>
        private static void CopyScreenToFile(Form window, string path)
        {
            Rectangle workingArea = Screen.FromControl(window).WorkingArea;

            if (window.Width > workingArea.Width || window.Height > workingArea.Height)
            {
                window.Size = new Size(
                    Math.Min(window.Width, workingArea.Width),
                    Math.Min(window.Height, workingArea.Height));
            }

            window.Location = new Point(workingArea.Left, workingArea.Top);
            window.Refresh();
            Pump(120);

            using (Bitmap picture = new Bitmap(window.Width, window.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics canvas = Graphics.FromImage(picture))
                {
                    canvas.CopyFromScreen(window.Location, Point.Empty, window.Size);
                }

                picture.Save(path, ImageFormat.Png);
            }
        }

        /// <summary>
        /// True when the picture holds no drawing at all, which is what a failed DrawToBitmap
        /// looks like: one flat color or a fully transparent rectangle. The window frame alone
        /// already gives a drawn screen several colors, so a real screen never counts as blank.
        /// </summary>
        private static bool IsBlank(Bitmap picture)
        {
            HashSet<int> colors = new HashSet<int>();

            // A coarse grid is enough: a drawn screen changes color almost everywhere it matters.
            int stepX = Math.Max(1, picture.Width / 20);
            int stepY = Math.Max(1, picture.Height / 20);

            for (int y = 0; y < picture.Height; y += stepY)
            {
                for (int x = 0; x < picture.Width; x += stepX)
                {
                    colors.Add(picture.GetPixel(x, y).ToArgb());
                    if (colors.Count > 2) return false;
                }
            }

            return true;
        }

        private static string PreviewFolder()
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "previews");
            Directory.CreateDirectory(folder);
            return folder;
        }

        /// <summary>Looks the class up by simple name in the two UI namespaces.</summary>
        private static Type FindClass(string className)
        {
            Type[] types = typeof(PreviewTool).Assembly.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.Name != className) continue;

                string space = type.Namespace ?? "";
                if (space == FormsNamespace || space == ControlsNamespace) return type;
            }

            return null;
        }

        /// <summary>Returns the argument that follows the given switch, or null.</summary>
        private static string ValueAfter(string[] args, string switchName)
        {
            if (args == null) return null;

            for (int i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], switchName, StringComparison.OrdinalIgnoreCase)) return args[i + 1];
            }

            return null;
        }

        /// <summary>
        /// Runs the message loop by hand for the requested time, so Load events, layout and
        /// painting all happen while nobody is watching.
        /// </summary>
        private static void Pump(int milliseconds)
        {
            int waited = 0;
            while (waited < milliseconds)
            {
                Application.DoEvents();
                Thread.Sleep(PumpStepMilliseconds);
                waited += PumpStepMilliseconds;
            }
        }

        /// <summary>Writes the problem next to the (missing) picture. Never throws.</summary>
        private static void WriteErrorFile(string className, string text)
        {
            string name = string.IsNullOrEmpty(className) ? "preview" : className;

            try
            {
                File.WriteAllText(Path.Combine(PreviewFolder(), name + ".error.txt"), text);
            }
            catch
            {
                // The exit code still tells the caller that the preview failed.
            }
        }

        private static string Describe(Exception ex)
        {
            if (ex == null) return "Unknown preview error.";

            List<string> lines = new List<string>();
            Exception current = ex;
            while (current != null)
            {
                lines.Add(current.GetType().Name + ": " + current.Message);
                current = current.InnerException;
            }
            lines.Add(ex.StackTrace ?? "");
            return string.Join(Environment.NewLine, lines.ToArray());
        }
    }
}
