using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VirtualGameMode.Functions;
using VirtualGameMode.Settings;
using VirtualGameMode.ViewModels;
using Application = System.Windows.Application;
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
            var menuItemShow = new System.Windows.Forms.MenuItem()
            {
                Name = "Show/Hide",
                Text = "Minimize to tray"                
            };
            menuItemShow.Click += (sender, args) =>
            {
                if (this.IsVisible)
                {
                    WindowState = WindowState.Minimized;
                }
                else
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                }
            };
            var menuItemExit = new System.Windows.Forms.MenuItem()
            {
                Name = "Exit",
                Text = "Exit"
            };
            menuItemExit.Click += (sender, args) => Application.Current.Shutdown();
            var contextMenu = new System.Windows.Forms.ContextMenu(new []{menuItemShow});
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(menuItemExit);
            trayIcon.ContextMenu = contextMenu;
            if(SettingsCollection.Default.StartMinimized)
                WindowState = WindowState.Minimized;                        
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
            {
                this.Hide();
                trayIcon.ContextMenu.MenuItems[0].Text = "Show";
            }
            else
            {
                trayIcon.ContextMenu.MenuItems[0].Text = "Minimize to tray";
                this.Show();
            }
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
