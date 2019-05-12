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
    public class DateMy
    {
        public string Title { get; set; }
        public DateTime InFormat { get; set; }
    }

    public class SeanceMy
    {
        public DateTime InFormat { get; set; }
        public string Title { get; set; }
        public DateTime SeanceInFormat { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для Seancess.xaml
    /// </summary>
    public partial class DateSeancesControl : UserControl
    {
        byte[] poster;
        public Film film;
        List<Genre> genres;
        public MainWindow main;
        Seancess seancess;
        List<DateTime> dates;
        List<DateTime> seances;
        List<DateMy> datesname;
        List<SeanceMy> seancesname;
        List<SeanceMy> currentdate;
        DateMy currdate;

        public DateSeancesControl(MainWindow main, Seancess seancess, Film film)
        {
            InitializeComponent();
            genres = new List<Genre>();
            this.main = main;
            this.seancess = seancess;
            this.film = film;
            MessageBox.Show(film.Name);
            datesname = new List<DateMy>();
            seancesname = new List<SeanceMy>();
            currentdate = new List<SeanceMy>();

            using (KinoContext db = new KinoContext())
            {
                List<int> datesids = new List<int>();
                var filmdates = db.FilmsDates.Where(x => x.FilmId == film.Id);

                foreach (var x in filmdates.ToList())
                {
                    datesids.Add((int)x.DateId);
                }

                foreach (var x in datesids.ToList())
                {
                    var newdate = db.Dates.First(d => d.Id == x).Title;
                    datesname.Add(new DateMy { Title = newdate.ToString("d"), InFormat = newdate });
                    datesGrid.ItemsSource = null;
                    datesGrid.ItemsSource = datesname;
                    
                }


                foreach (var x in filmdates.ToList())
                {
                    int dateid = (int)x.DateId;
                    var currdate = (DateTime)(db.Dates.Find(dateid).Title);

                    List<int> seancesids = new List<int>();
                    var filmdatesseances = db.FilmsDatesSeances.Where(f => f.FilmsDatesId == x.Id);

                    foreach (var f in filmdatesseances)
                        seancesids.Add((int)f.SeanceId);

                    foreach (var s in seancesids.ToList())
                    {
                        var newseance = db.Seances.First(d => d.Id == s).Title;
                        seancesname.Add(new SeanceMy { Title = newseance.ToString("t"), InFormat = currdate, SeanceInFormat = newseance });

                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.GridPrincipal.Children.Clear();
            main.GridPrincipal.Children.Add(seancess);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool dublicate = false;
            DateTime newdate = DateTime.Parse(date.Text);

            foreach (var x in datesname)
                if (x.InFormat == newdate)
                {
                    dublicate = true;
                    break;
                }

            if (!dublicate)
            {
                datesname.Add(new DateMy { Title = newdate.ToString("d"), InFormat = newdate });
                datesGrid.ItemsSource = null;
                datesGrid.ItemsSource = datesname;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool dublicate = false;
            DateTime newseance = DateTime.Parse(seance.Text);

            foreach (var x in seancesname)
            {
                if (x.Title.ToString() == seance.Text.ToString() && x.InFormat == currdate.InFormat)
                {
                    dublicate = true;
                    break;
                }
            }


            
            if (!dublicate)
            {
                seancesname.Add(new SeanceMy { SeanceInFormat = DateTime.Parse(seance.Text), InFormat = currdate.InFormat, Title = DateTime.Parse(seance.Text).ToString("t") });

                SeanceMy temp = seancesname.ElementAt(seancesname.Count - 1);
                

                currentdate = new List<SeanceMy>();

                foreach (var x in seancesname)
                    if (x.InFormat == currdate.InFormat)
                        currentdate.Add(x);

                seancesGrid.ItemsSource = null;
                seancesGrid.ItemsSource = currentdate;

            }
        }

        private void datesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            currdate = (DateMy)datesGrid.SelectedItem;

            try
            {
                currentdate = new List<SeanceMy>();

                foreach (var x in seancesname)
                    if (x.InFormat == currdate.InFormat)
                        currentdate.Add(x);
            }
            catch { }
            seancesGrid.ItemsSource = null;
            seancesGrid.ItemsSource = currentdate;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(film.Name);

            using (KinoContext db = new KinoContext())
            {
                try
                {
                    var filmdate = db.FilmsDates.Where(x => x.FilmId == film.Id);
                    var selectedfilmdate = db.FilmsDates.Where(x => x.FilmId == film.Id);
                    var selectedfilmdateids = new List<int>();
                    foreach (var x in filmdate)
                    {
                        selectedfilmdateids.Add(x.Id);
                    }

                    var selectedfilmdateseance = db.FilmsDatesSeances.Where(x => selectedfilmdateids.Contains((int)x.FilmsDatesId));

                    db.FilmsDatesSeances.RemoveRange(selectedfilmdateseance);
                    db.FilmsDates.RemoveRange(selectedfilmdate);

                    db.SaveChanges();



                    foreach (var x in datesname)
                    {
                        if (db.Dates.Where(d => d.Title == x.InFormat).Count() == 0)
                        {
                            db.Dates.Add(new Date { Title = x.InFormat });
                            db.SaveChanges();
                        }

                        var dateid = db.Dates.First(d => d.Title == x.InFormat).Id;

                        MessageBox.Show(dateid.ToString());
                        if (db.FilmsDates.Where(fd => fd.DateId == dateid && fd.FilmId == film.Id).Count() == 0)
                        {
                            db.FilmsDates.Add(new FilmsDates { DateId = dateid, FilmId = film.Id });
                            db.SaveChanges();
                            MessageBox.Show(dateid.ToString() + "        " + film.Id);
                        }

                        foreach (var y in seancesname)
                        {
                            int seanceid = -1;
                            var dub = false;
                            var seancesdb = db.Seances;
                            if (y.InFormat == x.InFormat)
                            {
                                foreach (var ss in seancesdb)
                                {
                                    if (ss.Title.ToString().Contains(y.Title))
                                    {
                                        dub = true;
                                        seanceid = ss.Id;
                                        break;
                                    }
                                }
                                if (!dub)
                                {
                                    db.Seances.Add(new Seance { Title = y.SeanceInFormat });
                                    db.SaveChanges();
                                }

                                var filmdateid = db.FilmsDates.First(fd => fd.FilmId == film.Id && fd.DateId == dateid).Id;

                                if (seanceid == -1)
                                    seanceid = db.Seances.First(s => s.Title == y.SeanceInFormat).Id;

                                if (db.FilmsDatesSeances.Where(fds => fds.FilmsDatesId == filmdateid && fds.SeanceId == seanceid).Count() == 0)
                                {
                                    db.FilmsDatesSeances.Add(new FilmsDatesSeances { FilmsDatesId = filmdateid, SeanceId = seanceid });
                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                }
                catch { }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DateMy datefordelete = (DateMy)datesGrid.SelectedItem;
            datesname.Remove(datefordelete);

            foreach (var x in seancesname)
                if (x.InFormat == datefordelete.InFormat)
                    currentdate.Remove(x);


            datesGrid.ItemsSource = null;
            datesGrid.ItemsSource = datesname;

            try
            {
                datesGrid.SelectedItem = datesGrid.Items[0];
                currdate = (DateMy)datesGrid.Items[0];
            }
            catch (Exception) { }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            SeanceMy seancefordelete = (SeanceMy)seancesGrid.SelectedItem;
            currentdate.Remove(seancefordelete);

            seancesGrid.ItemsSource = null;
            seancesGrid.ItemsSource = currentdate;

            try
            {
                seancesGrid.SelectedItem = seancesGrid.Items[0];
            }
            catch (Exception) { }
        }
    }
}
