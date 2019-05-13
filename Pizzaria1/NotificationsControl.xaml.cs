using Microsoft.Win32;
using Pizzaria1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class NotificationsControl : UserControl
    {
        public int randcode;
        byte[] poster;
        public Film film;
        public MainWindow main;
        public Seancess seancess;
        public User user;

        public NotificationsControl(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            
                using (KinoContext db = new KinoContext())
                {
                    var messages = db.Notifications.Where(x => x.UserId == main.user.Id).ToList();

                    foreach (var x in messages)
                    {
                        StackPanel panel = new StackPanel();

                        Border border = new Border();
                        border.CornerRadius = new CornerRadius(15, 5, 5, 5);
                        border.BorderThickness = new Thickness(2);
                        border.BorderBrush = Brushes.DarkRed;

                        Grid grid = new Grid();
                        grid.Background = Brushes.DarkRed;

                        ColumnDefinition c1 = new ColumnDefinition();
                        c1.Width = new GridLength(200, GridUnitType.Pixel);
                        ColumnDefinition c3 = new ColumnDefinition();
                        c3.Width = new GridLength(50, GridUnitType.Pixel);
                        ColumnDefinition c2 = new ColumnDefinition();
                        c2.Width = new GridLength(400, GridUnitType.Pixel);

                        grid.ColumnDefinitions.Add(c1);
                        grid.ColumnDefinitions.Add(c3);
                        grid.ColumnDefinitions.Add(c2);

                        TextBlock text1 = new TextBlock();
                        text1.FontFamily = new FontFamily("Century Gothic");
                        text1.FontSize = 18;
                        text1.Text = x.Time;
                        text1.Foreground = Brushes.White;
                        text1.Padding = new Thickness(10);
                        text1.SetValue(Grid.ColumnProperty, 0);

                        TextBlock text2 = new TextBlock();
                        text2.FontFamily = new FontFamily("Century Gothic");
                        text2.FontSize = 18;
                        text2.Text = x.Message;
                        text2.Foreground = Brushes.White;
                        text2.Padding = new Thickness(10);
                        text2.SetValue(Grid.ColumnProperty, 2);

                        grid.Children.Add(text1);
                        grid.Children.Add(text2);

                        border.Child = grid;

                        panel.Children.Add(border);

                        ListBoxItem item = new ListBoxItem();
                        item.Content = panel;
                        messagelist.Items.Add(item);
                    }
                }
        }

        private void messagesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
