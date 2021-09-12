using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

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
        public static (Alignment, (double left, double top, double right, double bottom)) GetTaskbarPosition()
        {
            var taskbarWindow = Native.FindWindow("Shell_TrayWnd", null);
            Matrix dpiTransform;

            using (var src = new HwndSource(new HwndSourceParameters()))
            {
                dpiTransform = src.CompositionTarget.TransformFromDevice;
            }

            Native.APPBARDATA data = new Native.APPBARDATA() { cbSize = Marshal.SizeOf(typeof(Native.APPBARDATA)), hWnd = taskbarWindow };
            Native.SHAppBarMessage(Native.ABM.GetTaskBarPos, ref data);
            return ((Alignment) data.uEdge, (data.rc.Left * dpiTransform.M11, data.rc.Top * dpiTransform.M22, data.rc.Right * dpiTransform.M11, data.rc.Bottom * dpiTransform.M22));
        }
    }
}