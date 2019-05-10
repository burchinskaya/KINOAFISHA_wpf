using Pizzaria1;
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

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new AuthorizationControl());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new AuthorizationControl());
        }

        private void Label_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new RegistrationControl());
        }
    }
}
