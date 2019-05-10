using Pizzaria1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для MenuControl.xaml
    /// </summary>
    public partial class MenuControl : Page
    {
        FoodContext db;
        public MenuControl()
        {
            InitializeComponent();
            db = new FoodContext();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search("Десерты");
        }

        public void Search(string DishName)
        {
            using (FoodContext db = new FoodContext())
            {
                db.Dishes.Where(x=>x.Category.Name == DishName).Load();
                dishesGrid.ItemsSource = db.Dishes.Local.ToBindingList();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Search("Закуски");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Search("Напитки");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Search("Паста");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Search("Пицца");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Search("Салаты");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Search("Супы");
        }
    }
}
