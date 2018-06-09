using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Tab = 0x09,
            F4 = 0x73,
            LWIN = 0x5B,
            RWIN = 0x5C,
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

        /// <summary>
        ///     Retrieves a handle to the foreground window (the window with which the user is currently working). The system
        ///     assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        ///     <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms633505%28v=vs.85%29.aspx for more information.</para>
        /// </summary>
        /// <returns>
        ///     C++ ( Type: Type: HWND )<br /> The return value is a handle to the foreground window. The foreground window
        ///     can be NULL in certain circumstances, such as when a window is losing activation.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int processId
        );
        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        public const int MONITOR_DEFAULTTONULL = 0;
        public const int MONITOR_DEFAULTTOPRIMARY = 1;
        public const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        // size of a device name string
        private const int CCHDEVICENAME = 32;

        /// <summary>
        /// The MONITORINFOEX structure contains information about a display monitor.
        /// The GetMonitorInfo function stores information into a MONITORINFOEX structure or a MONITORINFO structure.
        /// The MONITORINFOEX structure is a superset of the MONITORINFO structure. The MONITORINFOEX structure adds a string member to contain a name
        /// for the display monitor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MonitorInfoEx
        {
            /// <summary>
            /// The size, in bytes, of the structure. Set this member to sizeof(MONITORINFOEX) (72) before calling the GetMonitorInfo function.
            /// Doing so lets the function determine the type of structure you are passing to it.
            /// </summary>
            public int Size;

            /// <summary>
            /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates.
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public RectStruct Monitor;

            /// <summary>
            /// A RECT structure that specifies the work area rectangle of the display monitor that can be used by applications,
            /// expressed in virtual-screen coordinates. Windows uses this rectangle to maximize an application on the monitor.
            /// The rest of the area in rcMonitor contains system windows such as the task bar and side bars.
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>
            public RectStruct WorkArea;

            /// <summary>
            /// The attributes of the display monitor.
            ///
            /// This member can be the following value:
            ///   1 : MONITORINFOF_PRIMARY
            /// </summary>
            public uint Flags;

            /// <summary>
            /// A string that specifies the device name of the monitor being used. Most applications have no use for a display monitor name,
            /// and so can save some bytes by using a MONITORINFO structure.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;

            public void Init()
            {
                this.Size = 40 + 2 * CCHDEVICENAME;
                this.DeviceName = string.Empty;
            }
        }

        /// <summary>
        /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/dd162897%28VS.85%29.aspx"/>
        /// <remarks>
        /// By convention, the right and bottom edges of the rectangle are normally considered exclusive.
        /// In other words, the pixel whose coordinates are ( right, bottom ) lies immediately outside of the the rectangle.
        /// For example, when RECT is passed to the FillRect function, the rectangle is filled up to, but not including,
        /// the right column and bottom row of pixels. This structure is identical to the RECTL structure.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct RectStruct
        {
            /// <summary>
            /// The x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Left;

            /// <summary>
            /// The y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Top;

            /// <summary>
            /// The x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Right;

            /// <summary>
            /// The y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RectStruct lpRect);
    }
}
