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
    public class AllBooking
    {
        public int Code { get; set; }
        public string Film { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public List<Place> places { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class AdminProfileControl : UserControl
    {
        public int randcode;
        byte[] poster;
        public Film film;
        public MainWindow main;
        public Seancess seancess;
        public User user;

        public AdminProfileControl(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            BookingsGridRefresh();
            
        }

        public void BookingsGridRefresh()
        {
            main.RefreshBookings();
            bookingsGrid.ItemsSource = null;
            bookingsGrid.ItemsSource = main.allbookings;
        }

        public void PlacesGridRefresh()
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
            foreach (var x in main.allbookings)
            {
                var time = x.Time.Split(':');
                var date = x.Date.Split('.');
                DateTime a = DateTime.Now;
                DateTime b = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]), int.Parse(time[0]), int.Parse(time[1]), 0);
                var diff = b.Subtract(a).TotalMinutes;

                if (diff < 15)
                {
                    DeleteOrder(x);
                }
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Booking booking = (Booking)bookingsGrid.SelectedItem;
            DeleteOrder(booking);
        }

        public void DeleteOrder(Booking booking)
        {
            using (KinoContext db = new KinoContext())
            {
                var reserv = db.ReservationCodes.First(x => x.Code == booking.Code);
                db.ReservationPlaces.RemoveRange(db.ReservationPlaces.Where(x => x.CodeId == booking.Code));
                db.ReservationCodes.Remove(db.ReservationCodes.Find(reserv.Id));
                db.SaveChanges();
            }
            main.RefreshBookings();
            BookingsGridRefresh();
        }

        private void bookingsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlacesGridRefresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Booking booking = (Booking)bookingsGrid.SelectedItem;

            using (KinoContext db = new KinoContext())
            {
                var reserv = db.ReservationCodes.First(x => x.Code == booking.Code);

                var places = db.ReservationPlaces.Where(x => x.CodeId == reserv.Id);
                
                foreach (var x in places.ToList())
                {
                    db.SoldPlaces.Add(new SoldPlace { FilmDateSeanceId = booking.FDS, Place = x.Place, Range = x.Range });
                    db.SaveChanges();
                }

                db.ReservationPlaces.RemoveRange(db.ReservationPlaces.Where(x => x.CodeId == booking.Code));
                db.ReservationCodes.Remove(db.ReservationCodes.Find(reserv.Id));
                db.SaveChanges();
            }
            main.RefreshBookings();
            BookingsGridRefresh();
        }
    }
}
