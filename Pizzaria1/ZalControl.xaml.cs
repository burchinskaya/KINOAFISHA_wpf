using Microsoft.Win32;
using Pizzaria1;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ZalControl.xaml
    /// </summary>
    public partial class ZalControl : UserControl
    {
        public string folder;
        public User user;
        public Seancess seancess;
        IEnumerable<StackPanel> panels;
       
        public ZalControl(Seancess seancess)
        {
            InitializeComponent();

            panels = main.Children.OfType<StackPanel>();
            this.seancess = seancess;

            if (seancess.main.user == null)
                order.Content = "Сохранить";

                using (KinoContext db = new KinoContext())
                {
                    if (seancess.main.user != null)
                    {
                        firstf.IsReadOnly = true;
                        secondf.IsReadOnly = true;
                        thirdf.IsReadOnly = true;
                        fourthf.IsReadOnly = true;
                        fifthf.IsReadOnly = true;
                        sixf.IsReadOnly = true;
                        seventhf.IsReadOnly = true;
                        eighthf.IsReadOnly = true;
                       
                    }

                    firstf.Text = db.Prices.First(x => x.Range == 1).Cost.ToString();
                    secondf.Text = db.Prices.First(x => x.Range == 2).Cost.ToString();
                    thirdf.Text = db.Prices.First(x => x.Range == 3).Cost.ToString();
                    fourthf.Text = db.Prices.First(x => x.Range == 4).Cost.ToString();
                    fifthf.Text = db.Prices.First(x => x.Range == 5).Cost.ToString();
                    sixf.Text = db.Prices.First(x => x.Range == 6).Cost.ToString();
                    seventhf.Text = db.Prices.First(x => x.Range == 7).Cost.ToString();
                    eighthf.Text = db.Prices.First(x => x.Range == 8).Cost.ToString();
                }
            

            foreach (var x in panels)
            {
                var grids = x.Children.OfType<Grid>();

                foreach (var y in grids)
                {
                    var buttons = y.Children.OfType<Button>();

                    foreach (var z in buttons)
                        z.Style = (Style)FindResource("RoundCorner");
                }
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (seancess.main.user != null)
            {
                if (button.Style == (Style)FindResource("RoundCorner"))
                    button.Style = (Style)FindResource("RoundCornerChosed");
                else if (button.Style == (Style)FindResource("RoundCornerChosed"))
                    button.Style = (Style)FindResource("RoundCorner");
                else MessageBox.Show("Выберите доступные места.");
            }

            else
            {
                if (button.Style == (Style)FindResource("RoundCorner"))
                    button.Style = (Style)FindResource("RoundCornerSold");
                else if (button.Style == (Style)FindResource("RoundCornerSold"))
                    button.Style = (Style)FindResource("RoundCorner");
            }
        }

        public string SetPath()
        {
            string folderPath = "";
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Выберите папку для сохранения электронных билетов.";

            if (folderBrowser.ShowDialog() == true)
            {
                folderPath = System.IO.Path.GetDirectoryName(folderBrowser.FileName);
            }

            return folderPath;
        }

        private void Book(object sender, RoutedEventArgs e)
        {
            var num = 1;
            var date = seancess.seance.Title.ToString("t").Split(':');
            var time = seancess.date.Title.ToString("d").Split('.');
            DateTime a = DateTime.Now;
            DateTime b = new DateTime(int.Parse(time[2]), int.Parse(time[1]), int.Parse(time[0]), int.Parse(date[0]), int.Parse(date[1]), 0);
            var diff = b.Subtract(a).TotalMinutes;

            if (seancess.main.user == null)
            {
                using (KinoContext db = new KinoContext())
                {
                    for (int i = 0; i < panels.Count(); i++)
                    {
                        var grids = panels.ElementAt(i).Children.OfType<Grid>();

                        foreach (var y in grids)
                        {
                            var buttons = y.Children.OfType<Button>();

                            foreach (var z in buttons)
                            {
                                if (z.Style == (Style)FindResource("RoundCornerSold"))
                                {
                                    db.SoldPlaces.Add(new SoldPlace { Range = i + 1, Place = int.Parse(z.Content.ToString()), FilmDateSeanceId = seancess.filmdateseance.Id });
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    db.Prices.First(x => x.Range == 1).Cost = int.Parse(firstf.Text);
                    db.Prices.First(x => x.Range == 2).Cost = int.Parse(secondf.Text);
                    db.Prices.First(x => x.Range == 3).Cost = int.Parse(thirdf.Text);
                    db.Prices.First(x => x.Range == 4).Cost = int.Parse(fourthf.Text);
                    db.Prices.First(x => x.Range == 5).Cost = int.Parse(fifthf.Text);
                    db.Prices.First(x => x.Range == 6).Cost = int.Parse(sixf.Text);
                    db.Prices.First(x => x.Range == 7).Cost = int.Parse(seventhf.Text);
                    db.Prices.First(x => x.Range == 8).Cost = int.Parse(eighthf.Text);
                    db.SaveChanges();

                }
            }



            else if (diff > 15)
            {

                var dir = "KINOAFISHA " + DateTime.Now.ToString("dd-mm-yy hh-mm");
                using (KinoContext db = new KinoContext())
                {
                    MessageBox.Show(user.Id.ToString());
                    if (db.Users.First(x => x.Id == user.Id).PathForTickets == null)
                    {
                        folder = SetPath();
                        MessageBox.Show(db.Users.First(x => x.Id == user.Id).PathForTickets);
                        db.Users.First(x => x.Id == user.Id).PathForTickets = folder;
                        db.SaveChanges();
                        MessageBox.Show(db.Users.First(x => x.Id == user.Id).PathForTickets);
                    }
                    else folder = db.Users.First(x => x.Id == user.Id).PathForTickets;
                }

                using (KinoContext db = new KinoContext())
                {
                    Directory.CreateDirectory(db.Users.First(x => x.Id == user.Id).PathForTickets + "\\" + dir);

                    var number = 0;

                    if (db.ReservationCodes.Where(x => x.UserId == user.Id && x.FilmDateSeanceId == seancess.filmdateseance.Id).Count() == 0)
                    {
                        Random rnd = new Random();
                        number = rnd.Next(100000, 999999);
                        ReservationCode code = new ReservationCode { Code = number, UserId = user.Id, FilmDateSeanceId = seancess.filmdateseance.Id };
                        db.ReservationCodes.Add(code);
                        db.SaveChanges();

                    }
                    else number = db.ReservationCodes.First(x => x.UserId == user.Id && x.FilmDateSeanceId == seancess.filmdateseance.Id).Code;

                    var codeid = db.ReservationCodes.First(x => x.UserId == user.Id && x.FilmDateSeanceId == seancess.filmdateseance.Id).Id;

                    List<Order> orders = new List<Order>();
                    for (int i = 0; i < panels.Count(); i++)
                    {
                        var grids = panels.ElementAt(i).Children.OfType<Grid>();

                        foreach (var y in grids)
                        {
                            var buttons = y.Children.OfType<Button>();

                            foreach (var z in buttons)
                            {
                                if (z.Style == (Style)FindResource("RoundCornerChosed"))
                                {
                                    int price = 0;
                                    switch (i)
                                    {
                                        case 0: price = int.Parse(firstf.Text); break;
                                        case 1: price = int.Parse(secondf.Text); break;
                                        case 2: price = int.Parse(thirdf.Text); break;
                                        case 3: price = int.Parse(fourthf.Text); break;
                                        case 4: price = int.Parse(fifthf.Text); break;
                                        case 5: price = int.Parse(sixf.Text); break;
                                        case 6: price = int.Parse(seventhf.Text); break;
                                        case 7: price = int.Parse(eighthf.Text); break;

                                    }

                                    if (student.IsChecked == true)
                                    {
                                        orders.Add(new StudentOrder(i + 1, int.Parse(z.Content.ToString()), codeid, price));
                                    }

                                    else if (retiree.IsChecked == true)
                                    {
                                        orders.Add(new RetireeOrder(i + 1, int.Parse(z.Content.ToString()), codeid, price));
                                    }

                                    else
                                    {
                                        orders.Add(new SimpleOrder(i + 1, int.Parse(z.Content.ToString()), codeid, price));
                                    }
                                    num++;
                                }
                            }
                        }
                    }

                    if (orders.Count() >= 5)
                    {
                        if (orders.Count() >= 10)
                        {
                            for (int i=0; i<orders.Count(); i++)
                                orders[i] = new MoreThan10Decorator(orders[i]);
                        }
                        else
                        {
                            for (int i = 0; i < orders.Count(); i++)
                                orders[i] = new MoreThan5Decorator(orders[i]);
                        }
                    }

                    num = 1;
                    var totalprice = db.ReservationCodes.First(r => r.Id == codeid).TotalPrice;
                    foreach (var x in orders)
                    {
                        db.ReservationPlaces.Add(new ReservationPlace { CodeId = x.Code, Place = x.Place, Range = x.Range, Price = x.Price, Retiree = x.Retiree, Student = x.Student});
                        db.SaveChanges();

                        Ticket t = new Ticket(user);
                        t.path = folder;
                        t.filmname.Text = seancess.film.Name;
                        t.filmcode.Text = db.ReservationCodes.First(r => r.UserId == user.Id && r.FilmDateSeanceId == seancess.filmdateseance.Id).Code.ToString();
                        t.filmdate.Text = seancess.date.Title.ToString("d");
                        t.filmplace.Text = x.Place.ToString();
                        t.filmrange.Text = x.Range.ToString();
                        t.filmseance.Text = seancess.seance.Title.ToString("t");
                        t.filmprice.Text = x.GetCost().ToString();
                        t.dir = dir;
                        t.number = num.ToString();
                        t.Show();
                        num++;
                        totalprice += x.Price;
                    }
                    db.ReservationCodes.First(r => r.Id == codeid).TotalPrice = totalprice;

                    Auth_Reg auth = new Auth_Reg();
                    auth.SendMail($"Привет, {user.FirstName}! Ваше бронирование №{db.ReservationCodes.First(r => r.Id == codeid).Code} на сумму {totalprice} было успешно оформлено. Основная информация про заказ: фильм - \"{seancess.film.Name}\", дата - {seancess.date.Title.ToString("d")}, сеанс - {seancess.seance.Title.ToString("t")}. Для более детальной информации просмотрите вкладку \"Профиль\" в нашем приложении KINOAFISHA. Спасибо за бронирование!", user.Email);
                    MessageBox.Show($"Письмо о бронировании было отправлено на Вашу электронную почту, в папке {folder} были сохранены электронные билеты. Спасибо за бронирование!");
                    
                        db.Notifications.Add(new Notification { Message = $"Привет, {user.FirstName}!\nВы успешно совершили бронирование №{db.ReservationCodes.First(r => r.Id == codeid).Code} на сумму {totalprice} грн.\n\nФильм: \"{seancess.film.Name}\"\nДата: {seancess.date.Title.ToString("d")}\nСеанс:  {seancess.seance.Title.ToString("t")}", Time = $"{DateTime.Now.Day.ToString("00")}.{DateTime.Now.Month.ToString("00")}\n{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}", UserId = user.Id });
 
                    db.SaveChanges();
                    seancess.main.GridPrincipal.Children.Clear();
                    seancess.main.GridPrincipal.Children.Add(seancess);
                }

            }
            else MessageBox.Show("Бронировать билеты можно не позже, чем за 15 минут до начала сеанса.");
        }

        private void student_Checked(object sender, RoutedEventArgs e)
        {
            retiree.IsChecked = false;
        }

        private void retiree_Checked(object sender, RoutedEventArgs e)
        {
            student.IsChecked = false;
        }

        private void Return (object sender, RoutedEventArgs e)
        {
            seancess.main.GridPrincipal.Children.Clear();
            seancess.main.GridPrincipal.Children.Add(seancess);
        }
    }
}
