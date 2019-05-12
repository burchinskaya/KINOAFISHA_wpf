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
    public partial class NewFilmControl : UserControl
    {
        byte[] poster;
        public Film film;
        List<Genre> genres;
        bool edit = false;
        public MainWindow main;
        public Seancess seancess;
        public PremiersControl premiers;
        public bool isPremier = false;

        public NewFilmControl(MainWindow main, Seancess seancess, Film film, bool isEdit)
        {
            InitializeComponent();
            this.seancess = seancess;
            
            if (isEdit)
                edit = true;

            Start(film, main);

            filmIMDb.Visibility = Visibility.Visible;
            filmKinopoisk.Visibility = Visibility.Visible;
            ratings_premier.Text = "Рейтинги: ";
            filmrating.Visibility = Visibility.Visible;
            filmpremier.Visibility = Visibility.Collapsed;
            seancesedit.IsEnabled = true;
            
        }

        public NewFilmControl(MainWindow main, PremiersControl premiers, Film film, bool isEdit)
        {
            isPremier = true;
            InitializeComponent();
            this.premiers = premiers;

            if (isEdit)
                edit = true;

            Start(film, main);
            filmIMDb.Visibility = Visibility.Collapsed;
            filmKinopoisk.Visibility = Visibility.Collapsed;
            ratings_premier.Text = "Премьера: ";
            filmpremier.Visibility = Visibility.Visible;
            filmrating.Visibility = Visibility.Collapsed;
            seancesedit.IsEnabled = false;
            
        }
        
        public void Start(Film film, MainWindow main)
        {
            genres = new List<Genre>();
            this.main = main;
            this.film = film;

            if (edit)
            {
                filmtitle.Text = film.Name;
                filmdescription.Text = film.Description;
                filmcountry.Text = film.Country;
                filmIMDb.Text = film.RatingIMDb.ToString();
                filmKinopoisk.Text = film.RatingKinopoisk.ToString();

                filmslogan.Text = film.Slogan;
                trailer.Text = film.Trailer;

                using (KinoContext db = new KinoContext())
                {
                    try
                    {
                        filmpremier.Text = db.Films.First(x => x.Id == film.Id).PremierDate.ToString("d");
                        filmposter.Source = ToImage(db.Films.First(x => x.Id == film.Id).PosterByte);
                    }
                    catch { }

                    var filmgenres = db.FilmsGenres;
                    List<int> genresids = new List<int>();

                    foreach (var x in filmgenres)
                    {
                        if (x.FilmId == film.Id)
                            genresids.Add((int)x.GenreId);
                    }

                    foreach (var x in genresids)
                        genres.Add(db.Genres.Find(x));

                    genresGrid.ItemsSource = null;
                    genresGrid.ItemsSource = genres;
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Файлы изображений (*.jpg)|*.jpg";
            if (op.ShowDialog() == true)
            {
                BitmapImage img = new BitmapImage(new Uri(op.FileName));

                poster = new byte[10000000];
                poster = ImageToByte(img);
                filmposter.Source = ToImage(poster);
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool dublicate = false;
            if (filmgenre.Text != "")
            {
                foreach (var x in genres)
                    if (filmgenre.Text == x.Name)
                        dublicate = true;

                if (!dublicate)
                { 
                    genres.Add(new Genre { Name = filmgenre.Text });
                    genresGrid.ItemsSource = null;
                    genresGrid.ItemsSource = genres;
                }
            }
            else
                MessageBox.Show("Заполните поле жанра.");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (genresGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр из таблицы для удаления");
            }
            else
            {
                Genre x = (Genre)genresGrid.SelectedItem;
                genresGrid.ItemsSource = null;
                foreach (var g in genres.ToArray())
                    if (x.Name == g.Name)
                        genres.Remove(g);
                genresGrid.ItemsSource = genres;
                filmgenre.Text = "";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SaveFilm();
            using (KinoContext db = new KinoContext())
            {
                main.allfilms = db.Films.ToList();

                foreach (var x in main.allfilms.ToList())
                {
                    var subs = db.Subscriptions.Where(s => s.FilmId == x.Id);
                    foreach (var s in subs.ToList())
                    {
                        x.RegisterObserver(db.Users.First(u => u.Id == s.UserId));
                    }
                }
            }
           
        }

        private void SaveFilm()
        {
            if (edit == true)
            {
                using (KinoContext db = new KinoContext())
                {
                    db.Films.First(x => x.Id == film.Id).Name = filmtitle.Text;
                    db.Films.First(x => x.Id == film.Id).PosterByte = ImageToByte((BitmapImage)filmposter.Source);
                    db.Films.First(x => x.Id == film.Id).RatingIMDb = float.Parse(filmIMDb.Text);
                    db.Films.First(x => x.Id == film.Id).RatingKinopoisk = float.Parse(filmKinopoisk.Text);
                    db.Films.First(x => x.Id == film.Id).Slogan = filmslogan.Text;
                    db.Films.First(x => x.Id == film.Id).Trailer = trailer.Text;
                    db.Films.First(x => x.Id == film.Id).Country = filmcountry.Text;
                    db.Films.First(x => x.Id == film.Id).Description = filmdescription.Text;

                    if (filmpremier.Visibility == Visibility.Visible)
                    {
                        db.Films.First(x => x.Id == film.Id).PremierDate = DateTime.Parse(filmpremier.Text);
                        db.SaveChanges();
                    }
                    db.SaveChanges();

                    db.FilmsGenres.RemoveRange(db.FilmsGenres.Where(x => x.FilmId == film.Id));
                    db.SaveChanges();

                    foreach (var y in genres)
                    {
                        if (db.Genres.Where(x => x.Name == y.Name).Count() == 0)
                        {
                            db.Genres.Add(y);
                            db.SaveChanges();
                        }

                        var genresid = db.Genres.First(x => x.Name == y.Name).Id;

                        db.FilmsGenres.Add(new FilmsGenres { FilmId = film.Id, GenreId = genresid });
                        db.SaveChanges();
                    }

                    main.GridPrincipal.Children.Clear();
                    try
                    {
                        main.GridPrincipal.Children.Add(seancess);
                    }
                    catch { premiers.FilmRefresh(); main.GridPrincipal.Children.Add(premiers); }
                }
            }

            else
                using (KinoContext db = new KinoContext())
                {
                    film = new Film();

                    film.Name = filmtitle.Text;
                    film.Slogan = filmslogan.Text;
                    film.PosterByte = poster;
                    try
                    {
                        film.RatingIMDb = float.Parse(filmIMDb.Text);
                        film.RatingKinopoisk = float.Parse(filmKinopoisk.Text);
                    }
                    catch { }
                    film.Description = filmdescription.Text;
                    film.Country = filmcountry.Text;
                    film.Trailer = trailer.Text;

                    try
                    {
                        film.PremierDate = DateTime.Parse(filmpremier.Text);
                    }
                    catch { film.PremierDate = DateTime.Now; }

                    if (isPremier)
                        film.IsPremiere = true;

                    db.Films.Add(film);
                    db.SaveChanges();
                    var filmid = db.Films.First(x => x.Name == film.Name).Id;

                    foreach (Genre x in genres.ToArray())
                    {
                        if (db.Genres.Where(g => g.Name == x.Name).Count() == 0)
                        {
                            db.Genres.Add(x);
                            db.SaveChanges();
                        }

                        var genreid = db.Genres.First(g => g.Name == x.Name).Id;

                        db.FilmsGenres.Add(new FilmsGenres { FilmId = filmid, GenreId = genreid });
                        db.SaveChanges();

                    }

                    main.GridPrincipal.Children.Clear();
                    try
                    {
                        main.GridPrincipal.Children.Add(seancess);
                    }
                    catch { premiers.FilmRefresh(); main.GridPrincipal.Children.Add(premiers); }
                }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SaveFilm();
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(new DateSeancesControl(main, seancess, film));
        }
    }
}
