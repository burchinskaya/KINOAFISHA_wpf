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
using Microsoft.VisualBasic;

namespace Pizzaria1
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        Authorization a = new Authorization();
        public int num = 0;
        public Window2()
        {
            InitializeComponent();
        }
        

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            num = a.Registration(firstname.Text, lastname.Text, login.Text, password.Text, email.Text, address.Text);

            if (num == 1)
                MessageBox.Show("Пользователь с таким логином уже существует.");
            else if (num == 2)
                MessageBox.Show("Пользователь с такой электронной почтой уже существует.");
            else if (num == 0)
                MessageBox.Show("Что-то пошло не так.");
            else
            {
                MessageBox.Show("Письмо с кодом для регистрации отправлено на введенный email.");
                check.Visibility = Visibility.Visible;
            }

            }

        private void check_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (check.Visibility == Visibility.Visible && a.FinalRegistration(int.Parse(check.Text)))
            {
                MessageBox.Show("Вы успешно прошли регистрацию.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
