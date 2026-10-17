using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
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
    /// saves a PNG of it into the previews folder next to the exe and closes again.
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

        // The hosting form used for a UserControl (the page size the rules ask for).
        private const int PageHostWidth = 1180;
        private const int PageHostHeight = 720;

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
            Control screen = null;

            try
            {
                window = CreateWindow(type, className, role, isPage, out screen);

                window.ShowInTaskbar = false;
                window.StartPosition = FormStartPosition.CenterScreen;
                window.Show();

                // Layout and any runtime work in the Load event need the message loop to run,
                // so keep the window responsive for a moment before taking the picture.
                Pump(SettleMilliseconds);

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
                else if (screen != null)
                {
                    try { screen.Dispose(); }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Builds the window to photograph: the form itself, or a host form with the page docked in it.
        /// </summary>
        private static Form CreateWindow(Type type, string className, string role, bool isPage, out Control screen)
        {
            screen = null;

            object instance = Activator.CreateInstance(type);

            if (!isPage)
            {
                Form form = instance as Form;
                if (form == null) throw new InvalidOperationException("'" + className + "' could not be created as a Form.");
                screen = form;
                return form;
            }

            UserControl page = instance as UserControl;
            if (page == null) throw new InvalidOperationException("'" + className + "' could not be created as a UserControl.");

            Form host = new Form();
            host.Text = className + " (preview)";
            host.BackColor = Color.FromArgb(0xF7, 0xF3, 0xEA);
            host.ClientSize = new Size(PageHostWidth, PageHostHeight);
            host.MinimumSize = new Size(400, 300);

            page.Dock = DockStyle.Fill;
            host.Controls.Add(page);
            screen = page;

            return host;
        }

        /// <summary>Saves the window's client area as previews\&lt;ClassName&gt;.png next to the exe.</summary>
        private static void Save(Form window, string className)
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "previews");
            Directory.CreateDirectory(folder);

            // DrawToBitmap paints the whole window, title bar and border included, so the
            // picture is taken at the window size and then cut down to the working area.
            // Without the cut the bottom of the screen would be missing from the file.
            Size windowSize = window.Size;
            if (windowSize.Width < 1 || windowSize.Height < 1) windowSize = window.ClientSize;

            using (Bitmap wholeWindow = new Bitmap(windowSize.Width, windowSize.Height, PixelFormat.Format32bppArgb))
            {
                window.DrawToBitmap(wholeWindow, new Rectangle(0, 0, windowSize.Width, windowSize.Height));

                Rectangle area = ClientArea(window, windowSize);
                using (Bitmap picture = wholeWindow.Clone(area, PixelFormat.Format32bppArgb))
                {
                    picture.Save(Path.Combine(folder, className + ".png"), ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// Where the working area of the window sits inside the whole window (skipping the
        /// title bar and the border). Falls back to the entire picture if it cannot be measured.
        /// </summary>
        private static Rectangle ClientArea(Form window, Size windowSize)
        {
            int left = 0;
            int top = 0;

            try
            {
                Rectangle clientOnScreen = window.RectangleToScreen(window.ClientRectangle);
                Rectangle windowOnScreen = window.Bounds;
                left = clientOnScreen.X - windowOnScreen.X;
                top = clientOnScreen.Y - windowOnScreen.Y;
            }
            catch
            {
                left = 0;
                top = 0;
            }

            if (left < 0) left = 0;
            if (top < 0) top = 0;

            int width = window.ClientSize.Width;
            int height = window.ClientSize.Height;

            if (width < 1 || height < 1) return new Rectangle(0, 0, windowSize.Width, windowSize.Height);
            if (left + width > windowSize.Width) width = windowSize.Width - left;
            if (top + height > windowSize.Height) height = windowSize.Height - top;
            if (width < 1 || height < 1) return new Rectangle(0, 0, windowSize.Width, windowSize.Height);

            return new Rectangle(left, top, width, height);
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
                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "previews");
                Directory.CreateDirectory(folder);

                File.WriteAllText(Path.Combine(folder, name + ".error.txt"), text);
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
