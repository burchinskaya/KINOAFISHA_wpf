using Pizzaria1;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
    public class SeanceFDS
    {
        public DateTime date { get; set; }
        public DateTime time { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class Seancess : UserControl
    {
        
        public FilmsDatesSeances filmdateseance = new FilmsDatesSeances();
        FilmsDates filmdate;

        public ZalControl zal;
        public Seance seance;
        public Date date;
        public Film film;
        public MainWindow main;
        public ProfileControl profile;

        public Seancess(MainWindow main)
        {
            InitializeComponent();
            if (main.user != null)
            {
                plus.Visibility = Visibility.Collapsed;
                minus.Visibility = Visibility.Collapsed;
                edit.Visibility = Visibility.Collapsed;
                profile = new ProfileControl(main);
            }
            
            filmsGridRefresh();
            this.main = main;

            zal = new ZalControl(this);
        }

        public void filmsGridRefresh()
        {
            using (KinoContext db = new KinoContext())
            {
                db.Films.Where(x=>x.IsPremiere == false).Load();
                filmsGrid.ItemsSource = db.Films.Local.ToBindingList();
            }
            back.Visibility = Visibility.Collapsed;
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

        private void filmsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datesList.SelectedItem = null;
            datesList.ItemsSource = null;
            seancesList.SelectedItem = null;
            seancesList.ItemsSource = null;
            film = (Film)filmsGrid.SelectedItem;

            try
            { 
                using (KinoContext db = new KinoContext())
                {
                    film = (Film)filmsGrid.SelectedItem;
                    filmslogan.Text = film.Slogan;
                    poster.Source = ToImage(db.Films.First(x => x.Id == film.Id).PosterByte);
                    filmname.Text = film.Name;
                    filmcountry.Text = film.Country;
                    filmrating.Text = $"IMDb: {film.RatingIMDb}   Кинопоиск: {film.RatingKinopoisk}";
                    filmdescription.Text = film.Description; 

                    var genres = db.FilmsGenres.Where(x => x.FilmId == film.Id).Select(x=>x.GenreId).ToArray();
                    var genress = db.Genres.Where(x => genres.Contains(x.Id)).Select(x=>x.Name);
                    StringBuilder s = new StringBuilder();

                    foreach (var x in genress)
                        s.Append(x + ", ");
                    s.Remove(s.Length - 2, 2);

                    
                    filmgenres.Text = s.ToString();

                    var dates = db.FilmsDates.Where(x => x.FilmId == film.Id).Select(x=>x.DateId).ToArray();
                    var datess = db.Dates.Where(x => dates.Contains(x.Id)).Select(x=>x.Title).ToList();
                    var datesss = new List<string>();

                    foreach (var x in datess)
                    {
                        var temp =  ((DateTime)x).ToString("d");
                        datesss.Add(temp);
                    }

                    datesList.ItemsSource = datesss;
                    
                }
           }
           catch (Exception) { }
        }

        
        private void datesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (KinoContext db = new KinoContext())
                {
                    var seldate = DateTime.Parse(datesList.SelectedValue.ToString());
                    date = db.Dates.First(x => x.Title == seldate);
                    filmdate = db.FilmsDates.First(x => x.FilmId == film.Id && x.DateId == date.Id);

                    var seances = db.FilmsDatesSeances.Where(x => x.FilmsDatesId == filmdate.Id).Select(x => x.SeanceId).ToArray();
                    var seancess = db.Seances.Where(x => seances.Contains(x.Id)).Select(x => x.Title).ToList();
                    var seancesss = new List<string>();

                    foreach (var x in seancess)
                    {
                        var temp = ((DateTime)x).ToString("t");
                        seancesss.Add(temp);
                    }

                    seancesList.ItemsSource = seancesss;
                }
            }
            catch (Exception) { }
        }

        private void FreeTickets(object sender, RoutedEventArgs e)
        {

            main.title.Text = film.Name + "   " + date.Title.ToString("d") + "   " + seance.Title.ToString("t");
            zal.user = main.user; 
            zal.seancess = this;
            

            using (KinoContext db = new KinoContext())
            {
                try
                {
                    var codeList = db.ReservationCodes.Where(x => x.FilmDateSeanceId == filmdateseance.Id);

                    foreach (var code in codeList.ToList())
                    {
                        var reservedplaces = db.ReservationPlaces.Where(x => x.CodeId == code.Id);
                       
                        foreach (var x in reservedplaces.ToList())
                        {
                            switch (x.Range)
                            {
                                case 1: Buttons(zal.first, x.Place, "RoundCornerReserved"); break;
                                case 2: Buttons(zal.second, x.Place, "RoundCornerReserved"); break;
                                case 3: Buttons(zal.third, x.Place, "RoundCornerReserved"); break;
                                case 4: Buttons(zal.fourth, x.Place, "RoundCornerReserved"); break;
                                case 5: Buttons(zal.fifth, x.Place, "RoundCornerReserved"); break;
                                case 6: Buttons(zal.sixth, x.Place, "RoundCornerReserved"); break;
                                case 7: Buttons(zal.seventh, x.Place, "RoundCornerReserved"); break;
                                case 8: Buttons(zal.eighth, x.Place, "RoundCornerReserved"); break;
                            }
                        }

                        
                    }

                    var soldplaces = db.SoldPlaces.Where(x => x.FilmDateSeanceId == filmdateseance.Id);
                    foreach (var x in soldplaces.ToList())
                    {
                        switch (x.Range)
                        {
                            case 1: Buttons(zal.first, x.Place, "RoundCornerSold"); break;
                            case 2: Buttons(zal.second, x.Place, "RoundCornerSold"); break;
                            case 3: Buttons(zal.third, x.Place, "RoundCornerSold"); break;
                            case 4: Buttons(zal.fourth, x.Place, "RoundCornerSold"); break;
                            case 5: Buttons(zal.fifth, x.Place, "RoundCornerSold"); break;
                            case 6: Buttons(zal.sixth, x.Place, "RoundCornerSold"); break;
                            case 7: Buttons(zal.seventh, x.Place, "RoundCornerSold"); break;
                            case 8: Buttons(zal.eighth, x.Place, "RoundCornerSold"); break;
                        }
                    }
                }
                catch { }
            }
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(zal);
        }

        public void Buttons(StackPanel panel, int place, string Style)
        {
            
            var grid = panel.Children.OfType<Grid>();

            foreach (var x in grid)
            {
                var buttons = x.Children.OfType<Button>();

                foreach (var y in buttons)
                    if (y.Content.ToString() == place.ToString())
                    {
                        y.Style = (Style)zal.FindResource(Style);
                        break;
                    }
            }

        }

       
        private void seancesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // try
            //{
                using (KinoContext db = new KinoContext())
                {
                    seance = new Seance();
                    foreach (var x in db.Seances)
                    {
                        if (x.Title.ToString().Contains(seancesList.SelectedValue.ToString()) == true)
                        {
                            seance = x;
                            break;
                        }
                    }
                    filmdateseance = db.FilmsDatesSeances.First(x => x.FilmsDatesId == filmdate.Id && x.SeanceId == seance.Id);
                }
            //}
            //catch (Exception) { }
        }

        private void DeleteFilm(object sender, RoutedEventArgs e)
        {
                bool delete = true;
                Film selectedfilm = (Film)filmsGrid.SelectedItem;

                using (KinoContext db = new KinoContext())
                {
                    List<SeanceFDS> seanceFDs = new List<SeanceFDS>();
                    var selectedfilmdate = db.FilmsDates.Where(x => x.FilmId == selectedfilm.Id);
                    
                    foreach (var x in selectedfilmdate.ToList())
                    {
                        var selecteddate = db.Dates.First(d => d.Id == x.DateId);

                        List<FilmsDatesSeances> filmdateseances = new List<FilmsDatesSeances>();
                        filmdateseances = db.FilmsDatesSeances.Where(fds => fds.FilmsDatesId == x.Id).ToList();

                        foreach (var fds in filmdateseances.ToList())
                        {
                            var selectedseance = db.Seances.First(s => s.Id == fds.SeanceId);
                            seanceFDs.Add(new SeanceFDS { date = selecteddate.Title, time = selectedseance.Title });
                        }
                    }

                    foreach (var x in seanceFDs.ToList())
                    {
                        var date = x.time.ToString("t").Split(':');
                        var time = x.date.ToString("d").Split('.');
                        DateTime a = DateTime.Now;
                        DateTime b = new DateTime(int.Parse(time[2]), int.Parse(time[1]), int.Parse(time[0]), int.Parse(date[0]), int.Parse(date[1]), 0);
                        var diff = b.Subtract(a).TotalMinutes;
                    
                        if (diff > 0)
                        {
                            delete = false;
                            break;
                        }
                    }

                    if (delete)
                    {
                        var selectedfilmdateids = new List<int>();
                        foreach (var x in selectedfilmdate.ToList())
                        {
                            List<FilmsDatesSeances> filmdateseances = new List<FilmsDatesSeances>();
                            filmdateseances = db.FilmsDatesSeances.Where(fds => fds.FilmsDatesId == x.Id).ToList();

                            foreach (var fds in filmdateseances)
                            {
                                var reservations = db.ReservationCodes.Where(r => r.FilmDateSeanceId == fds.Id);
                                var reservcodes = new List<int>();
                                foreach (var r in reservations)
                                {
                                    reservcodes.Add(r.Id);
                                }

                                foreach (var rc in reservcodes)
                                {
                                    db.ReservationPlaces.RemoveRange(db.ReservationPlaces.Where(rp => rp.CodeId == rc));
                                    db.SaveChanges();
                                }
                                db.ReservationCodes.RemoveRange(db.ReservationCodes.Where(rc => rc.FilmDateSeanceId == fds.Id));
                                db.SaveChanges();

                                db.SoldPlaces.RemoveRange(db.SoldPlaces.Where(s => s.FilmDateSeanceId == fds.Id));
                            }

                            db.FilmsDatesSeances.RemoveRange(filmdateseances);

                        }
                        db.FilmsDates.RemoveRange(selectedfilmdate);
                        db.FilmsGenres.RemoveRange(db.FilmsGenres.Where(g => g.FilmId == selectedfilm.Id));
                        db.Films.Remove(db.Films.First(g => g.Id == selectedfilm.Id));
                        db.Subscriptions.RemoveRange(db.Subscriptions.Where(x => x.FilmId == selectedfilm.Id));
                        db.SaveChanges();
                        filmsGridRefresh();
                    }
                    else MessageBox.Show("Нельзя удалить выбранный фильм, так как сеансы еще не прошли.");
                        
                }
            
        }

        private void AddFilm(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, film, false));
        }

        private void EditFilm(object sender, RoutedEventArgs e)
        {

            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, film, true));

        }

        private void WatchTrailer(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                Process.Start(db.Films.First(x=>x.Id == film.Id).Trailer);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(profile);
        }
    }
}
