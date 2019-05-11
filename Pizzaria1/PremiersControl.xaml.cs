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
using System.Windows.Threading;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class PremiersControl : UserControl
    {
        DispatcherTimer timer;
        public List<Film> films;
        public Film selectedfilm;
        public int index = 0;
        public int total=0, days = 0, hours = 0, minutes = 0, seconds = 0;
        DateTime time1 = DateTime.Now;
        DateTime time2 = new DateTime(2019, 05, 13);
        public MainWindow main;
        public ProfileControl profile;

        public PremiersControl(MainWindow main)
        {
            this.main = main;
            timer = new DispatcherTimer();
            InitializeComponent();
            films = new List<Film>();

            using (KinoContext db = new KinoContext())
            {
                films = db.Films.Where(x => x.IsPremiere == true).ToList();
            }
            selectedfilm = films.ElementAt(0);
            FilmRefresh();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, selectedfilm, false));
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            if (index == 0)
            {
                index = films.Count() - 1;
                selectedfilm = films[index];
            }
            else
            {
                selectedfilm = films[index - 1];
                index--;
            }

            FilmRefresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, this, selectedfilm, true));
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            if (index == films.Count() - 1)
            {
                index = 0;
                selectedfilm = films[index];
            }
            else
            {
                selectedfilm = films[index + 1];
                index++;
            }

            FilmRefresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.FilmsGenres.RemoveRange(db.FilmsGenres.Where(x => x.FilmId == selectedfilm.Id));
                db.Films.Remove(db.Films.First(x => x.Id == selectedfilm.Id));
                db.SaveChanges();
                films = db.Films.Where(x => x.IsPremiere == true).ToList();
                selectedfilm = films.ElementAt(0);
            }

            FilmRefresh();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
                Process.Start(db.Films.First(x => x.Id == selectedfilm.Id).Trailer);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (KinoContext db = new KinoContext())
            {
                db.Films.First(x => x.Id == selectedfilm.Id).IsPremiere = false;
                db.SaveChanges();
            }

                main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new NewFilmControl(main, new Seancess(main), selectedfilm, true));
            main.title.Text = "Добавление фильма в сеансы";
        }

        public void FilmRefresh()
        {
            using (KinoContext db = new KinoContext())
            {
                films = db.Films.Where(x => x.IsPremiere == true).ToList();
                time2 = db.Films.First(x => x.Id == selectedfilm.Id).PremierDate;
            }
            timer.Stop();
            timer = new DispatcherTimer();
            StartClock();
            
            try
            {
                using (KinoContext db = new KinoContext())
                {
                    filmname.Text = db.Films.First(x => x.Id == selectedfilm.Id).Name;
                    filmcountry.Text = db.Films.First(x => x.Id == selectedfilm.Id).Country;
                    filmdescription.Text = db.Films.First(x => x.Id == selectedfilm.Id).Description;
                    filmslogan.Text = db.Films.First(x => x.Id == selectedfilm.Id).Slogan;
                    filmcountry.Text = db.Films.First(x => x.Id == selectedfilm.Id).Country;
                    filmcountry.Text = db.Films.First(x => x.Id == selectedfilm.Id).Country;
                    poster.Source = ToImage(db.Films.First(x => x.Id == selectedfilm.Id).PosterByte);
                    var genres = db.FilmsGenres.Where(x => x.FilmId == selectedfilm.Id).Select(x => x.GenreId).ToArray();
                    var genress = db.Genres.Where(x => genres.Contains(x.Id)).Select(x => x.Name);
                    StringBuilder s = new StringBuilder();

                    foreach (var x in genress)
                        s.Append(x + ", ");
                    s.Remove(s.Length - 2, 2);

                    filmgenres.Text = s.ToString();
                }
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

        private void StartClock()
        {
            total = 0;
            days = 0;
            hours = 0;
            minutes = 0;
            seconds = 0;
            total = (int)(time2 - time1).TotalSeconds;

            while (total > 0)
            {
                if (total - 86400 > 0)
                {
                    days++;
                    total -= 86400;
                }

                else if (total - 3600 > 0)
                {
                    hours++;
                    total -= 3600;
                }

                else if (total - 60 > 0)
                {
                    minutes++;
                    total -= 60;
                }

                else
                {
                    seconds++;
                    total--;
                }
            }
            
            timer.Interval = TimeSpan.FromSeconds(1);
            
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            using (KinoContext db = new KinoContext())
            filmrating.Text = ($"{db.Films.First(x=>x.Id == selectedfilm.Id).PremierDate.ToString("d")}  ({days} : {hours.ToString("00")} : {minutes.ToString("00")} : {seconds.ToString("00")})");

            if (seconds - 1 >= 0)
                seconds--;
            else if (minutes - 1 >= 0)
            {
                seconds = 59;
                minutes--;
            }
            else if (hours - 1 >= 0)
            {
                minutes = 59;
                seconds = 59;
                hours--;
            }
            else
            {
                hours = 23;
                days--;
            }
        
        }

     
    }
}
