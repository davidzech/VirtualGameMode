﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VirtualGameMode.Models;

namespace VirtualGameMode.Functions
{
    public static class WindowEnumerator
    {
        private static readonly string[] blacklist = new[] {"explorer.exe", "microsoftedgecp.exe", "pplicationFrameHost.exe" };

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
                    string error = $"OpenProcess() failed {Marshal.GetLastWin32Error()}";
                    Console.WriteLine(error);
                    return null;
                }            
                Native.GetModuleFileNameEx(process, IntPtr.Zero, exeBuilder, exeBuilder.Capacity);
                var exe = exeBuilder.ToString();
                return exe;
            }).Where(x => x != null).Distinct();
            return exePaths.Select(exe =>
            {
                var fileName = Path.GetFileName(exe)?.ToLower();
                if (File.Exists(exe) && fileName != null && !blacklist.Contains(fileName))
                {
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(Path.GetFullPath(exe));
                    return new UserApplication() { Name = info.ProductName, ExePath = exe };
                }
                else
                {
                    return null;
                }
            }).Where(x => x != null);        
        }
    }
}
