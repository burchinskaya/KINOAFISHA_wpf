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
using System.Windows.Threading;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        int num = 0;
        DispatcherTimer timer;
        public Authorization()
        {
            InitializeComponent();
            intro.Play();

            timer = new DispatcherTimer();
            Start();
        }

        public void Start()
        {
            timer.Interval = TimeSpan.FromSeconds(11);

            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
                auth.Visibility = Visibility.Visible;
                reg.Visibility = Visibility.Visible;

                timer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }
        

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            auth.Visibility = Visibility.Hidden;
            reg.Visibility = Visibility.Hidden;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new AuthorizationControl(this));
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new RegistrationControl(this));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
