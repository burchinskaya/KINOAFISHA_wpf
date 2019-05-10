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
using System.Windows.Shapes;

namespace Pizzaria1
{
    public partial class Window3 : Window
    {
        FoodContext db;
        public Window3()
        {
            InitializeComponent();

            db = new FoodContext();
            db.Categories.Load();
            phonesGrid.ItemsSource = db.Categories.Local.ToBindingList();
               
        }

        private void phonesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
