using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGameMode
{
    public class Native
    {
        public const int HC_ACTION = 0;
        public const int WH_KEYBOARD_LL = 13;

        public enum VK : Int32
        {
            LMENU = 0xA4,
            RMENU = 0xA5,
            F4 = 0x73,
        }

        public enum WM : Int32
        {
            KEYDOWN = 0x100,
            KEYUP = 0x101,
            SYSKEYDOWN = 0x104,
            SYSKEYUP = 0x105
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardLowLevelHookStruct
        {
            public VK vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public UInt32 dwExtraInfo;
        }

        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        //This is the Import for the SetWindowsHookEx function.
        //Use this function to install a thread-specific hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);

        //This is the Import for the UnhookWindowsHookEx function.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int hhk, int nCode, IntPtr wParam, IntPtr lParam);

    }
}
