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
    public partial class ProfileControl : UserControl
    {
        public int randcode;
        byte[] poster;
        public Film film;
        public MainWindow main;
        public Seancess seancess;
        public User user;
        List<Booking> bookings = new List<Booking>();
        List<Subscription> subscriptions = new List<Subscription>();
        List<Film> subsource = new List<Film>();

        public ProfileControl(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            user = this.main.user;
            firstname.Text = user.FirstName;
            lastname.Text = user.LastName;
            email.Text = user.Email;
            login.Text = user.Login;
            path.Text = user.PathForTickets;
            
            SubscriptionsRefresh();
            main.RefreshBookings();
            BookingsGridRefresh();
        }



        public void BookingsGridRefresh()
        {
            main.RefreshBookings();
            bookingsGrid.ItemsSource = null;
            bookingsGrid.ItemsSource = main.allbookings.Where(x=>x.UserId == user.Id);
        }

       /* public void BookingsRefresh()
        {
            bookings = new List<Booking>();
            using (KinoContext db = new KinoContext())
            {
                var bookingsdb = db.ReservationCodes.Where(x => x.UserId == user.Id);
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
                        currplaces.Add(new Place { Range = place.Range, Seat = place.Place });
                    }

                    bookings.Add(new Booking { Code = curr.Code, Date = date.Title.ToString("d"), Film = film.Name, Time = s.Title.ToString("t"), places = currplaces });

                }

               
            }
        }*/

        public void SubscriptionsRefresh()
        {
            var subs = new List<Film>();
                var films = main.allfilms;

            foreach (var x in films)
            {
                var subscr = x.Subscribers;

                foreach (User s in subscr)
                {
                    if (s.Id == user.Id)
                    subs.Add(x);
                }
            }

            subscriptionsGrid.ItemsSource = null;
            subscriptionsGrid.ItemsSource = subs;


            /*List<int> filmsids = new List<int>();
            subscriptions = new List<Subscription>();
            subsource = new List<Film>();
            using (KinoContext db = new KinoContext())
            {
                subscriptions = db.Subscriptions.Where(x => x.UserId == user.Id).ToList();

                foreach (var x in subscriptions)
                    filmsids.Add((int)x.FilmId);

                subsource.AddRange(db.Films.Where(x => filmsids.Contains(x.Id)));

                subscriptionsGrid.ItemsSource = null;
                subscriptionsGrid.ItemsSource = subsource;
            }*/
        }

        private void bookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Booking currbooking = (Booking)bookingsGrid.SelectedItem;
                placesGrid.ItemsSource = null;
                placesGrid.ItemsSource = currbooking.places;

                var filmname = currbooking.Film;
                Film film;

                using (KinoContext db = new KinoContext())
                {
                    film = db.Films.First(x => x.Name == filmname);
                }

                filmposter.Source = ToImage(film.PosterByte);
            }
            catch { }
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.Users.First(x => x.Id == user.Id).FirstName = firstname.Text;
                db.SaveChanges();
                MessageBox.Show("Имя успешно изменено.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.Users.First(x => x.Id == user.Id).LastName = lastname.Text;
                db.SaveChanges();
                MessageBox.Show("Фамилия успешно изменена.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.Users.First(x => x.Id == user.Id).Login = login.Text;
                db.SaveChanges();
                MessageBox.Show("Логин успешно изменен.");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            var newemail = email.Text;
            randcode = rnd.Next(1000, 9999);
            Auth_Reg a = new Auth_Reg();
            bool mail = a.SendMail($"Код подтверждения смены email: {randcode}", email.Text);

            if (mail)
            {
                code.Visibility = Visibility.Visible;
                submit.Visibility = Visibility.Visible;
                MessageBox.Show("Письмо с кодом подтверждения было отправлено на введенную почту");
            }

            else MessageBox.Show("Что-то пошло не так.");
        }
    
        
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                if (db.Users.First(x => x.Id == user.Id).Password == oldpass.Password && newpass.Password == newpass2.Password)
                {
                    db.Users.First(x => x.Id == user.Id).Password = newpass.Password;
                    db.SaveChanges();
                    MessageBox.Show("Пароль успешно изменен.");
                }
                else MessageBox.Show("Что-то пошло не так.");
            }
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (code.Text == randcode.ToString())
            {
                using (KinoContext db = new KinoContext())
                {
                    db.Users.First(x => x.Id == user.Id).Email = email.Text;
                    db.SaveChanges();
                }
                MessageBox.Show("E-mail успешно изменен.");
                code.Visibility = Visibility.Collapsed;
                submit.Visibility = Visibility.Collapsed;
            }

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Booking selectedbooking = (Booking)bookingsGrid.SelectedItem;

            using (KinoContext db = new KinoContext())
            {
                var film = db.Films.First(x => x.Name == selectedbooking.Film);
                db.ReservationPlaces.RemoveRange(db.ReservationPlaces.Where(x => x.CodeId == selectedbooking.Code));
                db.ReservationCodes.Remove(db.ReservationCodes.First(x => x.Code == selectedbooking.Code));
                db.Notifications.Add(new Notification { Message = $"Привет, {user.FirstName}!\nВы отменили бронирование №{selectedbooking.Code} на сумму {selectedbooking.TotalCost} грн.\n\nФильм: {film.Name}\nДата: {selectedbooking.Date}\nСеанс:  {selectedbooking.Time}", Time = $"{DateTime.Now.Day.ToString("00")}.{DateTime.Now.Month.ToString("00")}\n{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}", UserId = user.Id });

                db.SaveChanges();
            }

            try
            {
                bookingsGrid.SelectedItem = bookings.ElementAt(0);
            }
            catch { }

            BookingsGridRefresh();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                var image = ImageToByte((BitmapImage)filmposter.Source);
                var filmid = db.Films.First(x => x.PosterByte == image).Id;
                
                main.GridPrincipal.Children.Clear();
                var filmmenu = new Seancess(main);
                filmmenu.profile = this;
                filmmenu.back.Visibility = Visibility.Visible;
                foreach (var x in filmmenu.filmsGrid.Items)
                {
                    filmmenu.filmsGrid.SelectedItem = x;
                    Film f = (Film)filmmenu.filmsGrid.SelectedItem;
                    if (f.Id == filmid)
                        break;
                }

                main.GridPrincipal.Children.Add(filmmenu);
            }
        }

        public byte[] ImageToByte(BitmapImage imagesource)
        {
            byte[] data;
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imagesource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            folderBrowser.FileName = "Folder Selection.";

            if (folderBrowser.ShowDialog() == true)
            {
                string folderPath = System.IO.Path.GetDirectoryName(folderBrowser.FileName);
                path.Text = folderPath;

                using (KinoContext db = new KinoContext())
                {
                    db.Users.First(x => x.Id == user.Id).PathForTickets = path.Text;
                    db.SaveChanges();
                }
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Film selected = (Film)subscriptionsGrid.SelectedItem;

            var films = main.allfilms.First(x => x.Id == selected.Id);
            foreach (User x in films.Subscribers.ToList())
            {
                if (x.Id == user.Id)
                    films.RemoveObserver(x);
            }
            SubscriptionsRefresh();
        }
    }
}
