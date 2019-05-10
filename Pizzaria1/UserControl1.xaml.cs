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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        FoodContext db;
        public UserControl1()
        {
            InitializeComponent();

            db = new FoodContext();
            db.Categories.Load();
            phonesGrid.ItemsSource = db.Categories.Local.ToBindingList();
            
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phonesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*  int index = phonesGrid.SelectedIndex;
              int id = 0;
              bool converted = Int32.TryParse(phonesGrid.SelectedItem.ToString(), out id);
              if (converted == false)
                  return;

              Team team = db.Teams.Find(id);
          Dish dish = db.Dishes.Find()
              listBox1.DataSource = team.Players.ToList();
              listBox1.DisplayMember = "Name";*/
            phonesGrid2.ItemsSource = null;
            phonesGrid2.Items.Clear();

            Category cat = (Category)phonesGrid.SelectedItem;
            db.Dishes.Where(x => x.CategoryId == cat.Id).Load();
            phonesGrid2.ItemsSource = db.Dishes.Local.ToBindingList();
            
        }

        private void phonesGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void updateButton_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}
