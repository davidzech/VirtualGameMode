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
            switch (Properties.Settings.Default.Scope)
            {
                case 0:
                    AddedApp.IsChecked = true;
                    break;
                case 1:
                    FullScreen.IsChecked = true;
                    break;
                case 2:
                    Global.IsChecked = true;
                    break;
            }
        }

        private void AddedApp_OnChecked(object sender, RoutedEventArgs e)
        {
                Properties.Settings.Default.Scope = 0;
        }

        private void FullScreen_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Scope = 1;
        }

        private void Global_OnChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Scope = 2;
        }
    }
}
