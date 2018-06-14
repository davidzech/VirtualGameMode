using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using VirtualGameMode.Settings;

namespace VirtualGameMode.Functions
{
    public static class StartupManager
    {
        public static void SyncStartupKey()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            const string appName = "VirtualGameMode";
            if (SettingsCollection.Default.LaunchOnStartup)
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
