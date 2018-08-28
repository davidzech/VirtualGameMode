using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DesktopBridge;
using Squirrel;
using VirtualGameMode.Models;
using VirtualGameMode.Utilities;
using Console = System.Console;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _mutex;
        public App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            _mutex = new Mutex(true, "VirtualGameModeMutex", out bool newlyCreated);
            if (!newlyCreated)
            {
                MessageBox.Show("Another instance of VirtualGameMode is already running, terminating this one.");
                Application.Current.Shutdown();
            }

            StartupManager.SyncStartupKey();         
            DesktopBridge.Helpers _bridge = new Helpers();
            if (!_bridge.IsRunningAsUwp())
            {
#if DEBUG
#else
                CheckForUpdates();
#endif
            }
        }

        private async void CheckForUpdates()
        {
#if DEBUG
            string path = "C:\\Users\\david\\source\\repos\\VirtualGameMode\\VirtualGameMode\\bin\\Release";
#else
            string path = "https://zech.io";
#endif
            using (var um = new UpdateManager(path))
            {
                await um.UpdateApp();
            }
        }

        private void EnableDebugPrivileges()
        {
            Native.TOKEN_PRIVILEGES priv;

            if (!Native.OpenProcessToken(Native.GetCurrentProcess(), Native.TOKEN_ADJUST_PRIVILEGES | Native.TOKEN_QUERY, out var hToken))
            {
                Console.Error.WriteLine($"OpenProcessToken() failed: {Marshal.GetLastWin32Error()}");
                return;
            }

            if (!Native.LookupPrivilegeValue(null, Native.SE_DEBUG_NAME, out var luidSeDebug))
            {
                Console.Error.WriteLine($"LookupPrivilegeValue() failed: {Marshal.GetLastWin32Error()}");
                Native.CloseHandle(hToken);
                return;
            }

            priv.PrivilegeCount = 1;
            priv.Privileges = new Native.LUID_AND_ATTRIBUTES[1];
            priv.Privileges[0] =
                new Native.LUID_AND_ATTRIBUTES() {Attributes = Native.SE_PRIVILEGE_ENABLED, Luid = luidSeDebug};
            if (!Native.AdjustTokenPrivileges(hToken, false, ref priv, 0))
            {
                Console.Error.WriteLine($"AdjustTokenPrivileges() failed: {Marshal.GetLastWin32Error()}");
            }
            Native.CloseHandle(hToken);
        }
    }
}
