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

namespace VirtualGameMode.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            switch (Properties.Settings.Default.ScopeWin)
            {
                case 0:
                    AddedAppWin.IsChecked = true;
                    break;
                case 1:
                    FullScreenWin.IsChecked = true;
                    break;
                case 2:
                    GlobalWin.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.ScopeAltTab)
            {
                case 0:
                    AddedAppAltTab.IsChecked = true;
                    break;
                case 1:
                    FullScreenAltTab.IsChecked = true;
                    break;
                case 2:
                    GlobalWinAltTab.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.ScopeAltF4)
            {
                case 0:
                    AddedAppAltF4.IsChecked = true;
                    break;
                case 1:
                    FullScreenAltF4.IsChecked = true;
                    break;
                case 2:
                    GlobalAltF4.IsChecked = true;
                    break;
            }
        }

        private void ToggleSwitch_OnChecked(object sender, RoutedEventArgs e)
        {
            StartupManager.SyncStartupKey();
        }

        #region Windows Key Scope

        private void AddedAppWin_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeWin = 0;
        }

        private void FullScreenWin_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeWin = 1;
        }

        private void GlobalWin_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeWin = 2;
        }
        #endregion

        #region AltTab Scope
        private void AddedAppAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = 0;
        }

        private void FullScreenAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = 1;
        }

        private void GlobalWinAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = 2;
        }
        #endregion

        #region AltF4 Scope
        private void AddedAppAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = 0;
        }

        private void FullScreenAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = 1;
        }

        private void GlobalAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = 2;
        }
        #endregion
    }
}
