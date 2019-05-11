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
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class Seancess : UserControl
    {
        ZalControl zal = new ZalControl();
        public FilmsDatesSeances filmdateseance = new FilmsDatesSeances();
        FilmsDates filmdate;
       
        public Seance seance;
        public Date date;
        public Film film;
        public MainWindow main;
        public ProfileControl profile;

        public Seancess(MainWindow main)
        {
            InitializeComponent();
            filmsGridRefresh();
            this.main = main;
            profile = new ProfileControl(main);

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
                    MessageBox.Show(date.Title.ToString());

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            main.title.Text = film.Name + "   " + date.Title.ToString("d") + "   " + seance.Title.ToString("t");
            zal.user = main.User; 
            zal.seancess = this;
            

            using (KinoContext db = new KinoContext())
            {
                 var codes = db.ReservationCodes.Where(x => x.FilmDateSeanceId == filmdateseance.Id).Count();

                if (codes != 0)
                {
                    var code = db.ReservationCodes.First(x=>x.FilmDateSeanceId == filmdateseance.Id).Id;

                    var reservedplaces = db.ReservationPlaces.Where(x => x.CodeId == code);

                    foreach (var x in reservedplaces)
                    {
                        switch (x.Range)
                        {
                            case 1: Buttons(zal.first, x.Place); break;
                            case 2: Buttons(zal.second, x.Place); break;
                            case 3: Buttons(zal.third, x.Place); break;
                            case 4: Buttons(zal.fourth, x.Place); break;
                            case 5: Buttons(zal.fifth, x.Place); break;
                            case 6: Buttons(zal.sixth, x.Place); break;
                            case 7: Buttons(zal.seventh, x.Place); break;
                            case 8: Buttons(zal.eighth, x.Place); break;
                        }
                    }
                }
            }
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(zal);
        }

        public void Buttons(StackPanel panel, int place)
        {
            
            var grid = panel.Children.OfType<Grid>();

            foreach (var x in grid)
            {
                var buttons = x.Children.OfType<Button>();

                foreach (var y in buttons)
                    if (y.Content.ToString() == place.ToString())
                    {
                        y.Style = (Style)zal.FindResource("RoundCornerReserved");
                        break;
                    }
            }

        }

        private void seancesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
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
                    MessageBox.Show(filmdateseance.Id.ToString());
                }
            }
            catch (Exception) { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Film selectedfilm = (Film)filmsGrid.SelectedItem;

            using (KinoContext db = new KinoContext())
            {
                var selectedfilmdate = db.FilmsDates.Where(x => x.FilmId == selectedfilm.Id);
                var selectedfilmdateids = new List<int>();
                foreach (var x in selectedfilmdate)
                {
                    selectedfilmdateids.Add(x.Id);
                }

                var selectedfilmdateseance = db.FilmsDatesSeances.Where(x => selectedfilmdateids.Contains((int)x.FilmsDatesId));

                db.FilmsDatesSeances.RemoveRange(selectedfilmdateseance);
                db.FilmsDates.RemoveRange(selectedfilmdate);
                db.FilmsGenres.RemoveRange(db.FilmsGenres.Where(x => x.FilmId == selectedfilm.Id));
                db.Films.Remove(db.Films.First(x=>x.Id == selectedfilm.Id));
                
                db.SaveChanges();
                filmsGridRefresh();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, film, false));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, film, true));

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
