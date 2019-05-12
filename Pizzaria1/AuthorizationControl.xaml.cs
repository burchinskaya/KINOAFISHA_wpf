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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для RegistrationControl.xaml
    /// </summary>
    public partial class AuthorizationControl : UserControl
    {
        public MainWindow main;
        public AuthorizationControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Auth_Reg a = new Auth_Reg();

            if (a.Auth(loginfield.Text, passwordfield.Password) == null)
            {
                MessageBox.Show("Что-то пошло не так.");
            }

            else
            {
                SingleWindow window = new SingleWindow();
                window.Launch(a.Auth(loginfield.Text, passwordfield.Password));
                main = window.MainWindow;
                main.Show();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.Reg();
        }
    }
}
