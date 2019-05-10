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
    /// Логика взаимодействия для MenuControl_2.xaml
    /// </summary>
    public partial class MenuControl_2 : UserControl
    {
        public MenuControl_2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search("Десерты");
            uLimona.Text = "Десерты   уЛимона";
            ingredientsGrid.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        public void Search(string DishName)
        {
            using (FoodContext db = new FoodContext())
            {
                db.Dishes.Where(x => x.Category.Name == DishName).Load();
                dishesGrid.ItemsSource = db.Dishes.Local.ToBindingList();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Search("Закуски");
            uLimona.Text = "Закуски   уЛимона";
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Search("Напитки");
            uLimona.Text = "Напитки   уЛимона";
            dishname.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Search("Паста");
            uLimona.Text = "Паста   уЛимона";
            dishname.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Search("Пицца");
            uLimona.Text = "Пицца   уЛимона";
            dishname.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Search("Салаты");
            uLimona.Text = "Салаты   уЛимона";
            dishname.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Search("Супы");
            uLimona.Text = "Супы   уЛимона";
           dishname.Visibility = Visibility.Visible;
            ingredientsGrid.Visibility = Visibility.Visible;
            dishesGrid.Visibility = Visibility.Visible;
        }

        private void dishesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Dish dish = (Dish)dishesGrid.SelectedItem;

                dishimage.Source = new BitmapImage(new Uri("Images/" + dish.Photo + ".png", UriKind.Relative));


            

            using (FoodContext db = new FoodContext())
            {
                    var ids = db.DishIngredients.Where(x => x.DishId == dish.Id).Select(x => x.IngredientId).ToArray();

                    db.Ingredients.Where(x => ids.Contains(x.Id)).Load();

                    ingredientsGrid.ItemsSource = db.Ingredients.Local.ToBindingList();
            }

            dishname.Text = dish.Name;
            }
            catch (Exception) { }
        }
    }
}
