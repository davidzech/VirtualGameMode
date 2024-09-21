﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using VirtualGameMode.Models;
using VK = VirtualGameMode.Utilities.Native.VK;
using WM = VirtualGameMode.Utilities.Native.WM;

namespace VirtualGameMode.Utilities
{
    public static class GameModeHook
    {
        private static int _hook;
        private static readonly Native.HookProc Hookfn = KeyboardProc;
        public static void InstallHook()
        {
            _hook = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, Hookfn, IntPtr.Zero, 0);
        }

        private static bool _lAltPressed, _rAltPressed;

        private static long _previousAltPress;

        private static int KeyboardProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0 || nCode != Native.HC_ACTION)
                return Native.CallNextHookEx(0, nCode, new IntPtr(wParam), lParam);

            bool sys = false;
            Native.KeyboardLowLevelHookStruct kb = Marshal.PtrToStructure<Native.KeyboardLowLevelHookStruct>(lParam);
            switch ((WM)wParam)
            {
                case WM.SYSKEYDOWN:
                    sys = true;
                    goto case WM.KEYDOWN;

                case WM.KEYDOWN:
                    if (kb.vkCode == VK.LMENU)
                    {                        
                        _lAltPressed = true;
                    }
                    else if (kb.vkCode == VK.RMENU)
                    {
                        _rAltPressed = true;

                    }
                    _previousAltPress = DateTime.Now.Ticks;
                    break;

                case WM.SYSKEYUP:
                    sys = true;
                    goto case WM.KEYUP;

                case WM.KEYUP:
                    if (kb.vkCode == VK.LMENU)
                    {
                        _lAltPressed = false;                        
                    }
                    else if (kb.vkCode == VK.RMENU)
                    {
                        _rAltPressed = false;                        
                    }
                    break;
            }
            bool alt = _lAltPressed || _rAltPressed || sys;
            long ticksSinceLastAlt = DateTime.Now.Ticks - _previousAltPress;
            TimeSpan span = new TimeSpan(ticksSinceLastAlt);

            if (Settings.Default.DisableAltF4 && IsValidScopeForSetting(Settings.Default.DisableAltF4Scope))
            { 
                if (kb.vkCode == VK.F4 && (alt || (span.TotalSeconds < 2.0)))
                {
                    Console.WriteLine("Alt-F4 caught");
                    return 1;
                }
            }

            if (Settings.Default.DisableAltTab && IsValidScopeForSetting(Settings.Default.DisableAltTabScope))
            {
                if (kb.vkCode == VK.Tab && alt)
                {
                    Console.WriteLine("Alt-Tab caught");
                    return 1;
                }
            }

            if (Settings.Default.DisableAltSpace && IsValidScopeForSetting(Settings.Default.DisableAltSpaceScope))
            {
                if (kb.vkCode == VK.Space && alt)
                {
                    Console.WriteLine("Alt-Space caught");
                    return 1;
                }
            }

            if (Settings.Default.DisableWinKey && IsValidScopeForSetting(Settings.Default.DisableWinKeyScope))
            {
                if (kb.vkCode == VK.LWIN || kb.vkCode == VK.RWIN)
                {
                    Console.WriteLine("Windows key caught");
                    return 1;
                }
            }
           
            return Native.CallNextHookEx(0, nCode, new IntPtr(wParam), lParam);
        }

        private static bool IsValidScopeForSetting(KeyScope scope)
        {
            switch (scope)
            {
                case KeyScope.AddedApplications:
                    // check app
                    return IsForegroundWindowInAppList();
                case KeyScope.FullScreenApplications:
                    return IsForegroundWindowFullScreen();
                case KeyScope.Global:
                    return true;
                default:
                    Console.Error.WriteLine($"Unknown KeyScope {scope}. Your settings.json may be corrupted.");
                    return false;
            }
        }

        private static bool IsForegroundWindowInAppList()
        {
            var hwnd = Native.GetForegroundWindow();
            Native.GetWindowThreadProcessId(hwnd, out var processId);
            var process = Native.OpenProcess(Native.ProcessAccessFlags.QueryInformation | Native.ProcessAccessFlags.VirtualMemoryRead, false, processId);
            if (process == IntPtr.Zero)
            { 
                Console.Error.WriteLine($"OpenProcess() failed {Marshal.GetLastWin32Error()}");
                Console.Error.WriteLine("IsForegroundWindow() failed because OpenProcess() returned 0.");
                return false;
            }

            StringBuilder nameBuilder = new StringBuilder(1024);
            Native.GetModuleFileNameEx(process, IntPtr.Zero, nameBuilder, nameBuilder.Capacity);
            var exe = nameBuilder.ToString();
            return Settings.Default.UserApplications.Count(ua => ua.ExePath == exe) != 0; 
        }

        private static bool IsForegroundWindowFullScreen()
        {
            var hwnd = Native.GetForegroundWindow();
            Native.GetWindowRect(hwnd, out var windowRect);
            var monitor = Native.MonitorFromWindow(hwnd, Native.MONITOR_DEFAULTTONEAREST);
            var monitorInfo = new Native.MonitorInfoEx();
            monitorInfo.Init();
            Native.GetMonitorInfo(monitor, ref monitorInfo);
            return (windowRect.Bottom == monitorInfo.Monitor.Bottom &&
                    windowRect.Left == monitorInfo.Monitor.Left &&
                    windowRect.Right == monitorInfo.Monitor.Right &&
                    windowRect.Top == monitorInfo.Monitor.Top
                );
        }
        public static void RemoveHook()

        {
            Native.UnhookWindowsHookEx(_hook);
        }
    }
}
