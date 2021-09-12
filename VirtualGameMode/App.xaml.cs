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

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            StartupManager.SyncStartupKey();         
            DesktopBridge.Helpers _bridge = new Helpers();
            if (!_bridge.IsRunningAsUwp())
            {

            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ToString());
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
