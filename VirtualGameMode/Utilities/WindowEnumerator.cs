using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VirtualGameMode.Models;

namespace VirtualGameMode.Utilities
{
    public static class WindowEnumerator
    {
        private static readonly string[] blacklist = new[] {"explorer.exe", "microsoftedgecp.exe", "pplicationFrameHost.exe" };

        private static bool AlreadyAdded(string exePath)
        {
            return Settings.Default.UserApplications.Count(a => a.ExePath == exePath) != 0;
        }

        public static IEnumerable<UserApplication> GetAllWindows()
        {
            List<UserApplication> windows = new List<UserApplication>();
            List<IntPtr> handles = new List<IntPtr>();
            Native.EnumWindowsProc enumWindowsProc = new Native.EnumWindowsProc((hwnd, lparam) =>
            {
                //StringBuilder exeBuilder = new StringBuilder();
                //StringBuilder nameBuilder = new StringBuilder();
                //Native.GetWindowModuleFileName(hwnd, exeBuilder, exeBuilder.MaxCapacity);
                //Native.GetWindowText(hwnd, nameBuilder, nameBuilder.MaxCapacity);
                //var name = CleanUpWindowName(nameBuilder.ToString());
                //var exe = exeBuilder.ToString();
                //windows.Add(new UserApplication() {Name = name, ExePath = exe});
                if(Native.IsWindowVisible(hwnd))
                    handles.Add(hwnd);
                return true;
            });
            Native.EnumWindows(enumWindowsProc, IntPtr.Zero);
            var exePaths = handles.Select(hwnd =>
            {
                StringBuilder exeBuilder = new StringBuilder(1024);
                Native.GetWindowThreadProcessId(hwnd, out var processId);
                var process = Native.OpenProcess(Native.ProcessAccessFlags.QueryInformation | Native.ProcessAccessFlags.VirtualMemoryRead, false, processId);
                if (process == IntPtr.Zero)
                {                   
                    Console.Error.WriteLine($"OpenProcess() failed {Marshal.GetLastWin32Error()}");
                    Console.Error.WriteLine("EnumerateWindow() failed because OpenProcess() returned 0.");
                    return null;
                }            
                Native.GetModuleFileNameEx(process, IntPtr.Zero, exeBuilder, exeBuilder.Capacity);
                var exe = exeBuilder.ToString();
                return exe;
            }).Where(x => x != null).Distinct();
            return exePaths.Select(exe =>
            {
                var fileName = Path.GetFileName(exe)?.ToLower();
                if (File.Exists(exe) && fileName != null && !blacklist.Contains(fileName) && !AlreadyAdded(exe))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(Path.GetFullPath(exe));
                    return new UserApplication() { Name = GetNameFromInfo(info), ExePath = exe };
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null);        
        }

        private static string GetNameFromInfo(FileVersionInfo info)
        {
            string potential;
            if (!String.IsNullOrEmpty(info.FileDescription))
            {
                potential = info.FileDescription;
            } else if (!String.IsNullOrEmpty(info.ProductName))
            {
                potential = info.ProductName;
            }
            else
            {
                potential = Path.GetFileNameWithoutExtension(info.FileName);
            }

            if (potential.EndsWith(".exe"))
            {
                return potential.Substring(0, potential.Length - 4);
            }

            return potential;
        }
    }
}
