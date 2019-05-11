using KINOwpf;
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

namespace Pizzaria1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public User User;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void RefreshUserInfo()
        {
            using (KinoContext db = new KinoContext())
            {
                User = db.Users.First(x => x.Id == User.Id);
            }
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    title.Text = "Профиль";
                    RefreshUserInfo();
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new ProfileControl(this));
                    break;
                case 1:
                    title.Text = "Сеансы";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Seancess(this));
                    break;
                case 3:
                    title.Text = "Скоро на экранах";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new PremiersControl(this));
                    break;
                default:
                    break;
            }
        }

        public void Reg()
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new RegistrationControl());
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;

        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_Selected_2(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_Selected_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
