using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ControlzEx.Standard;
using MahApps.Metro.Behaviours;
using MahApps.Metro.Controls;
using VirtualGameMode.Functions;
using VirtualGameMode.Settings;
using VirtualGameMode.ViewModels;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly NotifyIcon trayIcon;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            using (var iconStream = Application
                .GetResourceStream(new Uri("pack://application:,,,/VirtualGameMode;component/Resources/icon.ico"))
                ?.Stream)
            {
                trayIcon = new NotifyIcon()
                {
                    Visible = true,
                    Icon = new System.Drawing.Icon(iconStream)
                };
            }

            trayIcon.DoubleClick += TrayIcon_DoubleClick;
            //WindowState = WindowState.Minimized;
            var mini = new MiniWindow();
            //mini.Show();

        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        private void ReorderTemplates()
        {
            // reorder templates of the root Grid so we can extend view into titlebar
            var titlebar = GetTemplateChild("PART_TitleBar") as UIElement;
            var content = GetTemplateChild("PART_Content") as UIElement;
            if (titlebar != null && titlebar.GetParentObject() is Grid parent)
            {
                var contentindex = parent.Children.IndexOf(content);
                parent.Children.Remove(titlebar);
                parent.Children.Insert(contentindex, titlebar);
            }

        }

        public override void OnApplyTemplate()
        {
            ReorderTemplates();
            base.OnApplyTemplate();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            GameModeHook.RemoveHook();
            SettingsCollection.Default.Save();           
        }


        // TODO: move this logic to MainWindowViewModel
        private void HamburgerMenu_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            this.HamburgerMenu.Content = e.InvokedItem;
        }

        private void Applications_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // not using styles here because it overrides the mahapps.metro style and thats a pain in the ass to sublcass
        private void GameModeToggle_OnChecked(object sender, RoutedEventArgs e)
        {
            GameModeToggle.Foreground = (Brush)FindResource("AccentColorBrush");
            GameModeToggle.Content = "Game Mode On";
        }

        private void GameModeToggle_OnUnchecked(object sender, RoutedEventArgs e)
        {
            GameModeToggle.Foreground = Brushes.White;
            GameModeToggle.Content = "Game Mode Off";
        }
    }
}
