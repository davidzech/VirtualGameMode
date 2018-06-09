using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WM = VirtualGameMode.Native.WM;
using VK = VirtualGameMode.Native.VK;

namespace VirtualGameMode
{
    public static class GameModeHook
    {
        private static int _hook = 0;
        private static Native.HookProc hookfn = new Native.HookProc(llKeyboardProc);
        public static void InstallHook()
        {
            _hook = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, hookfn, IntPtr.Zero, 0);
        }

        private static bool lAltPressed = false, rAltPressed = false;
        private static int llKeyboardProc(int nCode, int wParam, IntPtr lParam)
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
                        lAltPressed = true;
                    }
                    else if (kb.vkCode == VK.RMENU)
                    {
                        rAltPressed = true;
                    }

                    break;
                case WM.KEYUP:
                case WM.SYSKEYUP:
                    if (kb.vkCode == VK.LMENU)
                    {
                        lAltPressed = false;
                    }
                    else if (kb.vkCode == VK.RMENU)
                    {
                        rAltPressed = false;
                    }

                    break;
            }

            if (kb.vkCode == VK.F4)
            {
                bool alt = lAltPressed || rAltPressed || Keyboard.GetKeyStates(Key.LeftAlt) == KeyStates.Down ||
                           (Keyboard.GetKeyStates(Key.RightAlt) == KeyStates.Down);
                if (alt)
                {
                    return 1;
                }
            }

            return Native.CallNextHookEx(0, nCode, new IntPtr(wParam), lParam);
        }

        public static void RemoveHook()
        {
            Native.UnhookWindowsHookEx(_hook);
        }
    }
}
