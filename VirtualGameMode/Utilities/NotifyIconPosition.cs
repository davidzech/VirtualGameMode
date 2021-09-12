using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VirtualGameMode.Utilities
{
    public static class NotifyIconPosition
    {
        public static Rectangle GetNotifyIconPosition(NotifyIcon icon)
        {
            // obtain private field "id"
            // ReSharper disable once PossibleNullReferenceException
            var id = (uint)icon.GetType().GetField("id", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(icon);
            // ReSharper disable once PossibleNullReferenceException
            var hwnd = (NativeWindow) icon.GetType()
                .GetField("window", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(icon);

            Native.NOTIFYICONIDENTIFIER identifier = new Native.NOTIFYICONIDENTIFIER() {cbSize = Marshal.SizeOf(typeof(Native.NOTIFYICONIDENTIFIER)),hWnd = hwnd.Handle, uID = (int)id};

            Native.Shell_NotifyIconGetRect(ref identifier, out var rect);
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static Rectangle GetRect(this NotifyIcon icon)
        {
            return GetNotifyIconPosition(icon);
        }
    }
}
