using System;
using System.ComponentModel;
using System.Diagnostics;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Tells forms and user controls whether they are being shown inside the Visual Studio Designer.
    /// Use it to skip database and file access so the Designer never shows the yellow error screen.
    ///
    /// Example (in a UserControl or Form Load event):
    ///     if (DesignTime.IsDesignMode) return;
    /// </summary>
    internal static class DesignTime
    {
        private static readonly bool _isDesignMode = Detect();

        public static bool IsDesignMode
        {
            get { return _isDesignMode; }
        }

        private static bool Detect()
        {
            // Reliable in constructors, nested controls and inherited controls.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            // Fallback: the Designer runs inside the Visual Studio process.
            try
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    string name = process.ProcessName;
                    return name.Equals("devenv", StringComparison.OrdinalIgnoreCase)
                        || name.Equals("DesignToolsServer", StringComparison.OrdinalIgnoreCase)
                        || name.Equals("XDesProc", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
