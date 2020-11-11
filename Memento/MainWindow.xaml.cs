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

using Memento.UserControls;
using Memento.BLL;
using Memento.DAL;
using System.Diagnostics;

namespace Memento
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Content = new MainPageUserControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }
    }
}
