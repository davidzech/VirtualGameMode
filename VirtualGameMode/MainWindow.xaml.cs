using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using VirtualGameMode.Utilities;
using VirtualGameMode.ViewModels;
using Application = System.Windows.Application;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using static VirtualGameMode.Utilities.Native;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Control = System.Windows.Controls.Control;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly NotifyIcon _trayIcon;
        private readonly MiniWindow _mini;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            using (var iconStream = Application
                .GetResourceStream(new Uri("pack://application:,,,/VirtualGameMode;component/Resources/icon.ico"))
                ?.Stream)
            {
                _trayIcon = new NotifyIcon()
                {
                    Visible = true,
                    Icon = new System.Drawing.Icon(iconStream)
                };
            }
            _trayIcon.DoubleClick += TrayIcon_DoubleClick;
            var menuItemShow = new System.Windows.Forms.MenuItem()
            {
                Name =  "Show/Hide",
                Text = Settings.Default.StartMinimized ? "Show" : "Minimize to tray"                
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
            _trayIcon.ContextMenu = contextMenu;
            if (Settings.Default.StartMinimized)
            {
                Hide();
                WindowState = WindowState.Minimized;
            }             
            _mini = new MiniWindow();
            _mini.Deactivated += MiniOnDeactivated;
            _trayIcon.Click += TrayIconOnClick;
        }

        private void TrayIconOnClick(object sender, EventArgs e)
        {
            MouseEventArgs mouseArgs = (MouseEventArgs)e;
            if (mouseArgs.Button == MouseButtons.Left)
            {
                if (!_iconDeactivatedWithClick)
                {
                    // calculate where to put
                    (Alignment align, Rectangle rect) = TaskbarPosition.GetTaskbarPosition();
                    switch (align)
                    {
                        case Alignment.BOTTOM:
                            // place on bottom right
                            _mini.Top = rect.Bottom - _mini.Height - rect.Height;
                            _mini.Left = rect.Right - _mini.Width;
                            break;
                        case Alignment.LEFT:
                            // place on bottom left
                            _mini.Top = rect.Bottom - _mini.Height;
                            _mini.Left = rect.Left + rect.Width;
                            break;
                        case Alignment.RIGHT:
                            // place on bottom right, but down more
                            _mini.Top = rect.Bottom - _mini.Height;
                            _mini.Left = rect.Right - _mini.Width - rect.Width;
                            break;
                        case Alignment.TOP:
                            // place on top right
                            _mini.Top = rect.Bottom;
                            _mini.Left = rect.Right - _mini.Width;
                            break;
                    }
                    _mini.Show();
                    _mini.Activate();
                }
                _iconDeactivatedWithClick = false;
            }
        }

        private bool _iconDeactivatedWithClick;
        private void MiniOnDeactivated(object sender, EventArgs e)
        {
            Rectangle rect = NotifyIconPosition.GetNotifyIconPosition(_trayIcon);
            if (rect.Contains(System.Windows.Forms.Cursor.Position))
                _iconDeactivatedWithClick = true;
            _mini.Hide();
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            if (Visibility != Visibility.Visible)
            {
                Show();
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
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
                _trayIcon.ContextMenu.MenuItems[0].Text = "Show";
            }
            else
            {
                _trayIcon.ContextMenu.MenuItems[0].Text = "Minimize to tray";
                this.Show();
            }
            base.OnStateChanged(e);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            GameModeHook.RemoveHook();
            Settings.Default.Save();           
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
