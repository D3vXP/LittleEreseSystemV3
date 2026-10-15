using System;
using System.IO;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;
using LittleEreseSystemV3.Forms;
using LittleEreseSystemV3.Helpers;

namespace LittleEreseSystemV3
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Catch every exception that would otherwise close the app with the ugly
            // "unhandled exception" dialog, and show a friendly message instead.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(OnThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainException);

            Application.Run(new LoginForm());
        }

        /// <summary>Exceptions raised on the UI thread (button clicks, timers, Load events).</summary>
        private static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Report(e == null ? null : e.Exception);
        }

        /// <summary>
        /// Exceptions raised on other threads. The app cannot be kept alive here, so we only log it.
        /// </summary>
        private static void OnDomainException(object sender, UnhandledExceptionEventArgs e)
        {
            Report(e == null ? null : e.ExceptionObject as Exception);
        }

        /// <summary>Shows the plain-English message and writes the technical details to error.log.</summary>
        private static void Report(Exception exception)
        {
            if (exception == null) return;

            LogError(exception);

            try
            {
                Messages.Error(Db.Describe(exception));
            }
            catch
            {
                // Showing the message must never cause a second failure.
            }
        }

        /// <summary>
        /// Appends the full exception (with a timestamp) to error.log next to the exe.
        /// This runs while the app is already broken, so it swallows every problem of its own.
        /// </summary>
        private static void LogError(Exception exception)
        {
            if (exception == null) return;

            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                string entry =
                    "==============================================================" + Environment.NewLine +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine +
                    exception.ToString() + Environment.NewLine + Environment.NewLine;

                File.AppendAllText(path, entry);
            }
            catch
            {
                // No log file (read-only folder, disk full): the message box is still shown.
            }
        }
    }
}
