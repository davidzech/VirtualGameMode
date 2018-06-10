using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VirtualGameMode.Models;
using WM = VirtualGameMode.Native.WM;
using VK = VirtualGameMode.Native.VK;

namespace VirtualGameMode
{
    public static class GameModeHook
    {
        private static int _hook = 0;
        private static Native.HookProc hookfn = new Native.HookProc(KeyboardProc);
        public static void InstallHook()
        {
            _hook = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, hookfn, IntPtr.Zero, 0);
        }

        private static bool _lAltPressed = false, _rAltPressed = false;
        private static int KeyboardProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0 || nCode != Native.HC_ACTION)
                return Native.CallNextHookEx(0, nCode, new IntPtr(wParam), lParam);

            Native.KeyboardLowLevelHookStruct kb = Marshal.PtrToStructure<Native.KeyboardLowLevelHookStruct>(lParam);
            switch ((WM)wParam)
            {
                case WM.KEYDOWN:
                case WM.SYSKEYDOWN:
                    if (kb.vkCode == VK.LMENU)
                    {
                        _lAltPressed = true;
                    }
                    else if (kb.vkCode == VK.RMENU)
                    {
                        _rAltPressed = true;
                    }

                    break;
                case WM.KEYUP:
                case WM.SYSKEYUP:
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
            bool alt = _lAltPressed || _rAltPressed || Keyboard.GetKeyStates(Key.LeftAlt) == KeyStates.Down ||
                       (Keyboard.GetKeyStates(Key.RightAlt) == KeyStates.Down);            

            if (Properties.Settings.Default.DisableAltF4 && IsValidScopeForSetting(Properties.Settings.Default.ScopeAltF4))
            {
                if (kb.vkCode == VK.F4 && alt)
                {
                    return 1;
                }
            }

            if (Properties.Settings.Default.DisableAltTab && IsValidScopeForSetting(Properties.Settings.Default.ScopeAltTab))
            {
                if (kb.vkCode == VK.Tab && alt)
                {
                    return 1;
                }
            }

            if (Properties.Settings.Default.DisableWinKey && IsValidScopeForSetting(Properties.Settings.Default.ScopeWin))
            {
                if (kb.vkCode == VK.LWIN || kb.vkCode == VK.RWIN)
                {
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
                    // Log this
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
                string error = $"OpenProcess() failed {Marshal.GetLastWin32Error()}";
                Console.WriteLine(error);
                return false;
            }

            StringBuilder nameBuilder = new StringBuilder();
            Native.GetModuleFileNameEx(process, IntPtr.Zero, nameBuilder, nameBuilder.Capacity);
            return Properties.Settings.Default.Applications.Contains(nameBuilder.ToString());
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
