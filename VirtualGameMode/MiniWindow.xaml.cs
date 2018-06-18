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
using System.Windows.Shapes;
using System.Windows.Shell;

namespace VirtualGameMode
{
    /// <summary>
    /// Interaction logic for MiniWindow.xaml
    /// </summary>
    public partial class MiniWindow
    {

        public MiniWindow()
        {
            InitializeComponent();
            Loaded += MiniWindow_Loaded;
        }

        public override void OnApplyTemplate()
        {
            this.EnableBlur();
            base.OnApplyTemplate();
        }

        private void MiniWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
