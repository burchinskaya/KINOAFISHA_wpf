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
    public partial class RegistrationControl : UserControl
    {
        public int num = 0;
        Auth_Reg a;
        public Authorization auth;
        public RegistrationControl(Authorization auth)
        {
            InitializeComponent();
            this.auth = auth;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            a = new Auth_Reg();
            num = a.Registration(namefield.Text, lastnamefield.Text, loginfield.Text, passwordfield.Password, emailfield.Text);

            if (num == 1)
                MessageBox.Show("Пользователь с таким логином уже существует.");
            else if (num == 2)
                MessageBox.Show("Пользователь с такой электронной почтой уже существует.");
            else
            {
                MessageBox.Show("Письмо с кодом для регистрации отправлено на введенный email.");
                codefield.Visibility = Visibility.Visible;
                codelabel.Visibility = Visibility.Visible;
            }
        }

        private void codefield_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (a.FinalRegistration(int.Parse(codefield.Text)))
            {
                MessageBox.Show("Вы успешно прошли регистрацию.");
                auth.GridPrincipal.Children.Clear();
            }
        }
    }
}
