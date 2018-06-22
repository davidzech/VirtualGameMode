using System.Drawing;
using System.Runtime.InteropServices;

namespace VirtualGameMode.Utilities
{
    public enum Alignment
    {
        LEFT = 0,
        TOP = 1,
        RIGHT = 2,
        BOTTOM = 3
    }
    public static class TaskbarPosition
    {
        public static (Alignment, Rectangle) GetTaskbarPosition()
        {
            var taskbarWindow = Native.FindWindow("Shell_TrayWnd", null);
            Native.APPBARDATA data = new Native.APPBARDATA() {cbSize = Marshal.SizeOf(typeof(Native.APPBARDATA)), hWnd = taskbarWindow};
            Native.SHAppBarMessage(Native.ABM.GetTaskBarPos, ref data);
            return ((Alignment) data.uEdge, Rectangle.FromLTRB(data.rc.Left, data.rc.Top, data.rc.Right, data.rc.Bottom));
        }
    }
}