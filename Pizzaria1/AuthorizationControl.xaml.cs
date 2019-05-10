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
            main = new MainWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                var count = db.Users.Where(x => x.Login == loginfield.Text && x.Password == passwordfield.Password).Count();

                if (count == 1)
                {
                    main.User = db.Users.First(x => x.Login == loginfield.Text && x.Password == passwordfield.Password);
                    main.Show();
                }
                else
                {
                    User newuser = new User { Login = loginfield.Text, Password = passwordfield.Password };
                    db.Users.Add(newuser);
                    db.SaveChanges();
                    MessageBox.Show("Пользователь успешно добавлен." + db.Users.Count());

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.Reg();
        }
    }
}
