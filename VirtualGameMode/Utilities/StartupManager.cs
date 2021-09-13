using DesktopBridge;
using Microsoft.Win32;
using Windows.ApplicationModel;
using System;

namespace VirtualGameMode.Utilities
{
    public static class StartupManager
    {
        public static void SyncStartupKey()
        {
            var helpers = new Helpers();
            if (helpers.IsRunningAsUwp())
            {
                SyncStartupUWP();
            } 
            else
            {
                SyncStartupLegacy();
            }
        }

        private static void SyncStartupLegacy()
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

        private static async void SyncStartupUWP()
        {
            StartupTask task = await StartupTask.GetAsync("VirtualGameMode");
            if (Settings.Default.LaunchOnStartup)
            {
                var state = await task.RequestEnableAsync();
                if (state != StartupTaskState.Enabled) {
                    Settings.Default.LaunchOnStartup = false;
                }
            } else
            {
                task.Disable();
            }
        }
    }
}
