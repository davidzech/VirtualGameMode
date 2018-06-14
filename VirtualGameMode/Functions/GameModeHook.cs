using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using VirtualGameMode.Models;
using VirtualGameMode.Settings;
using VK = VirtualGameMode.Functions.Native.VK;
using WM = VirtualGameMode.Functions.Native.WM;

namespace VirtualGameMode.Functions
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

            if (SettingsCollection.Default.DisableAltF4 && IsValidScopeForSetting(SettingsCollection.Default.DisableAltF4Scope))
            {
                if (kb.vkCode == VK.F4 && alt)
                {
                    return 1;
                }
            }

            if (SettingsCollection.Default.DisableAltTab && IsValidScopeForSetting(SettingsCollection.Default.DisableAltTabScope))
            {
                if (kb.vkCode == VK.Tab && alt)
                {
                    return 1;
                }
            }

            if (SettingsCollection.Default.DisableWinKey && IsValidScopeForSetting(SettingsCollection.Default.DisableWinKeyScope))
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

            StringBuilder nameBuilder = new StringBuilder();
            Native.GetModuleFileNameEx(process, IntPtr.Zero, nameBuilder, nameBuilder.Capacity);
            var exe = nameBuilder.ToString();
            return SettingsCollection.Default.UserApplications.Exists(ua => ua.ExePath == exe); 
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
