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

namespace Pizzaria1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Admin admin;
        User user;
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Authorization a = new Authorization();

            if (a.Auth(login.Text, password.Text) == null)
            {
                MessageBox.Show("Что-то пошло не так.");
            }

            else if (a.Auth(login.Text, password.Text) is User)
            {
                user = a.Auth(login.Text, password.Text) as User;
                MainWindow m = new MainWindow();
                m.user = user;
                m.Show();
                MessageBox.Show(m.user.Login);
            }

            else if (a.Auth(login.Text, password.Text) is Admin)
            {
                admin = a.Auth(login.Text, password.Text) as Admin;
                MainWindow m = new MainWindow();
                m.admin = admin;
                m.Show();
                MessageBox.Show(admin.Login);
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window2 w = new Window2();
            w.Show();
        }
    }
}
