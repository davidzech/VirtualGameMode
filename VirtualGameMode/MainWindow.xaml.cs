using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlzEx.Behaviors;
using MahApps.Metro.Behaviours;
using MahApps.Metro.Controls;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private NotifyIcon trayIcon;
        public MainWindow()
        {
            InitializeComponent();
            trayIcon = new NotifyIcon()
            {
                Visible = true,
                Icon = new System.Drawing.Icon("icon.ico")

            };

            //WindowState = WindowState.Minimized;
            var mini = new MiniWindow();
            //mini.Show();

        }

        public override void OnApplyTemplate()
        {
            //this.EnableBlur();
            base.OnApplyTemplate();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private IntPtr lib32, lib64;
        private void InstallHooks()
        {
            lib32 = Native.LoadLibrary("GameModeHook32.dll");
            lib64 = Native.LoadLibrary("GameModeHook64.dll");
            var proc32 = Native.GetProcAddress(lib32, "llKeyboardHook32");
            var proc64 = Native.GetProcAddress(lib64, "llKeyboardHook64");
        }

        private void RemoveHooks()
        {

        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.HamburgerMenu.Content = e.ClickedItem;
        }
    }
}
