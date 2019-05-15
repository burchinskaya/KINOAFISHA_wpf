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
    public class Place
    {
        public int Range { get; set; }
        public int Seat { get; set; }
        public bool Student { get; set; }
        public bool Retiree { get; set; }
        public string Price { get; set; }
    }

    public class Booking
    {
        public int Code { get; set; }
        public string Film { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public int TotalCost { get; set; }
        public int FDS { get; set; }
        public List<Place> places { get; set; }
    }
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Booking> allbookings;
        public User user;
        public Admin admin;
        public List<Film> allfilms;

        private static MainWindow instance;

        public MainWindow()
        {

        }

        public void RefreshBookings()
        {
            using (KinoContext db = new KinoContext())
            {
                allbookings = new List<Booking>();

                var bookingsdb = db.ReservationCodes;
                var bookingsids = new List<int>();
                foreach (var x in bookingsdb)
                    bookingsids.Add(x.Id);

                foreach (var x in bookingsids)
                {
                    List<Place> currplaces = new List<Place>();
                    ReservationCode curr = db.ReservationCodes.First(r => r.Id == x);

                    FilmsDatesSeances fds = db.FilmsDatesSeances.First(f => f.Id == curr.FilmDateSeanceId);
                    FilmsDates fd = db.FilmsDates.First(f => f.Id == fds.FilmsDatesId);
                    Seance s = db.Seances.First(f => f.Id == fds.SeanceId);
                    Film film = db.Films.First(fi => fi.Id == fd.FilmId);
                    Date date = db.Dates.First(da => da.Id == fd.DateId);

                    var placesdb = db.ReservationPlaces.Where(pl => pl.CodeId == x);

                    foreach (var place in placesdb)
                    {
                        Place temp = new Place { Range = place.Range, Seat = place.Place, Retiree = place.Retiree, Student = place.Student };

                        if (place.Student)
                            temp.Price = place.Price + " (студент)";
                        else if (place.Retiree)
                            temp.Price = place.Price + " (пенсионер)";
                        else temp.Price = place.Price.ToString();

                        currplaces.Add(temp);
                    }

                    allbookings.Add(new Booking { UserId = (int)curr.UserId, Code = curr.Code, User = db.Users.First(u => u.Id == curr.UserId).FirstName + " " + db.Users.First(u => u.Id == curr.UserId).LastName, Date = date.Title.ToString("d"), Film = film.Name, Time = s.Title.ToString("t"), places = currplaces, TotalCost = curr.TotalPrice, FDS = fds.Id });
                }
            }

            if (user == null)
                notifications.Visibility = Visibility.Collapsed;
        }

        protected MainWindow(IPerson person, List<Film> allfilms)
        {
            InitializeComponent();

            if (person is Admin)
                admin = (Admin)person;
            else if (person is User)
                user = (User)person;
            
            this.allfilms = allfilms;
            GridPrincipal.Children.Clear();
            RefreshBookings();
        }

        public static MainWindow getInstance(IPerson person, List<Film> films)
        {
            if (instance == null)
                instance = new MainWindow(person, films);
            return instance;
        }

        public static MainWindow getInstance()
        {
            if (instance == null)
                instance = new MainWindow();
            return instance;
        }

        public void RefreshUserInfo()
        {
            try
            {
                using (KinoContext db = new KinoContext())
                {
                    user = db.Users.First(x => x.Id == user.Id);
                }
            }
            catch { }
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.Subscriptions.RemoveRange(db.Subscriptions);
                db.SaveChanges();

                foreach (var x in allfilms.ToList())
                {
                    var subs = x.Subscribers;
                    foreach (User s in subs.ToList())
                    {
                        db.Subscriptions.Add(new Subscription { UserId = s.Id, FilmId = x.Id });
                        db.SaveChanges();
                    }
                }
            }
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
                    GridPrincipal.Children.Clear();

                    if (user != null)
                    {
                        RefreshUserInfo();
                        GridPrincipal.Children.Add(new ProfileControl(this));
                    }
                    else
                    {
                        RefreshUserInfo();
                        GridPrincipal.Children.Add(new AdminProfileControl(this));
                    }
                    break;
                case 1:
                    title.Text = "Сеансы";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Seancess(this));
                    break;
                case 2:
                    title.Text = "PopCorn";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new GameControl());
                    break;
                case 3:
                    title.Text = "Скоро на экранах";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new PremiersControl(this));
                    break;
                case 4:
                    title.Text = "Оповещения";
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new NotificationsControl(this));
                    break;
                default:
                    break;
            }
        }
        

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
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

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
