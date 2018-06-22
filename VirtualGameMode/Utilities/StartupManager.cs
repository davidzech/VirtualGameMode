using Microsoft.Win32;

namespace VirtualGameMode.Utilities
{
    public static class StartupManager
    {
        public static void SyncStartupKey()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            const string appName = "VirtualGameMode";
            if (Settings.Default.LaunchOnStartup)
            {
                key?.SetValue(appName, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                key?.DeleteValue(appName, false);
            }
        }
    }
}
