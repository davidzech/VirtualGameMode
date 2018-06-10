using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Console = System.Console;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            StartupManager.SyncStartupKey();
            //EnableDebugPrivileges();
        }

        private void EnableDebugPrivileges()
        {
            IntPtr hToken;
            Native.TOKEN_PRIVILEGES priv;
            Native.LUID luidSeDebug;

            if (!Native.OpenProcessToken(Native.GetCurrentProcess(), Native.TOKEN_ADJUST_PRIVILEGES | Native.TOKEN_QUERY, out hToken))
            {
                Console.WriteLine($"OpenProcessToken() failed: {Marshal.GetLastWin32Error()}");
                throw new Exception("1");
                return;
            }

            if (!Native.LookupPrivilegeValue(null, Native.SE_DEBUG_NAME, out luidSeDebug))
            {
                Console.WriteLine($"LookupPrivilegeValue() failed: {Marshal.GetLastWin32Error()}");
                Native.CloseHandle(hToken);
                throw new Exception("2");
                return;
            }

            priv.PrivilegeCount = 1;
            priv.Privileges = new Native.LUID_AND_ATTRIBUTES[1];
            priv.Privileges[0] =
                new Native.LUID_AND_ATTRIBUTES() {Attributes = Native.SE_PRIVILEGE_ENABLED, Luid = luidSeDebug};
            if (!Native.AdjustTokenPrivileges(hToken, false, ref priv, 0))
            {
                Console.WriteLine($"AdjustTokenPrivileges() failed: {Marshal.GetLastWin32Error()}");
                throw new Exception("3");
            }
            Native.CloseHandle(hToken);
        }
    }
}
