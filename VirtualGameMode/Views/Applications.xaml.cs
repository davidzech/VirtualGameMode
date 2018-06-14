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
using MahApps.Metro.Controls.Dialogs;
using VirtualGameMode.ViewModels;

namespace VirtualGameMode.Views
{
    /// <summary>
    /// Interaction logic for Applications.xaml
    /// </summary>
    public partial class Applications : UserControl
    {
        public Applications()
        {
            InitializeComponent();
            // delay until loaded so we don't change stuff before the UI is loaded
            this.DataContext = new ApplicationsViewModel(DialogCoordinator.Instance);
        }
    }
}
