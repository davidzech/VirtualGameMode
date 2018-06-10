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
using VirtualGameMode.Models;

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
                case KeyScope.AddedApplications:
                    AddedAppWin.IsChecked = true;
                    break;
                case KeyScope.FullScreenApplications:
                    FullScreenWin.IsChecked = true;
                    break;
                case KeyScope.Global:
                    GlobalWin.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.ScopeAltTab)
            {
                case KeyScope.AddedApplications:
                    AddedAppAltTab.IsChecked = true;
                    break;
                case KeyScope.FullScreenApplications:
                    FullScreenAltTab.IsChecked = true;
                    break;
                case KeyScope.Global:
                    GlobalWinAltTab.IsChecked = true;
                    break;
            }

            switch (Properties.Settings.Default.ScopeAltF4)
            {
                case KeyScope.AddedApplications:
                    AddedAppAltF4.IsChecked = true;
                    break;
                case KeyScope.FullScreenApplications:
                    FullScreenAltF4.IsChecked = true;
                    break;
                case KeyScope.Global:
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
            Properties.Settings.Default.ScopeWin = KeyScope.AddedApplications;
        }

        private void FullScreenWin_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeWin = KeyScope.FullScreenApplications;
        }

        private void GlobalWin_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeWin = KeyScope.Global;
        }
        #endregion

        #region AltTab Scope
        private void AddedAppAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = KeyScope.AddedApplications;
        }

        private void FullScreenAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = KeyScope.FullScreenApplications;
        }

        private void GlobalWinAltTab_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltTab = KeyScope.Global;
        }
        #endregion

        #region AltF4 Scope
        private void AddedAppAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = KeyScope.AddedApplications;
        }

        private void FullScreenAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = KeyScope.FullScreenApplications;
        }

        private void GlobalAltF4_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ScopeAltF4 = KeyScope.Global;
        }
        #endregion
    }
}
