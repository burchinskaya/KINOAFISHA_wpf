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
        public ZalControl()
        {
            InitializeComponent();
            panels = main.Children.OfType<StackPanel>();

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

            if (button.Style == (Style)FindResource("RoundCorner"))
                button.Style = (Style)FindResource("RoundCornerChosed");
            else if (button.Style == (Style)FindResource("RoundCornerChosed"))
                button.Style = (Style)FindResource("RoundCorner");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var num = 1;

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
                MessageBox.Show(db.Users.First(x => x.Id == user.Id).PathForTickets);
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

                List<ReservationPlace> places = new List<ReservationPlace>();
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
                                places.Add(new ReservationPlace { CodeId = codeid, Range = i + 1, Place = int.Parse(z.Content.ToString()) });
                                Ticket t = new Ticket(user);
                                t.path = folder;
                                t.filmname.Text = seancess.film.Name;
                                t.filmcode.Text = db.ReservationCodes.First(x => x.UserId == user.Id && x.FilmDateSeanceId == seancess.filmdateseance.Id).Code.ToString();
                                t.filmdate.Text = seancess.date.Title.ToString("d");
                                t.filmplace.Text = z.Content.ToString();
                                t.filmrange.Text = (i + 1).ToString();
                                t.filmseance.Text = seancess.seance.Title.ToString("t");

                                switch (i)
                                {
                                    case 0: t.filmprice.Text = firstf.Text; break;
                                    case 1: t.filmprice.Text = secondf.Text; break;
                                    case 2: t.filmprice.Text = thirdf.Text; break;
                                    case 3: t.filmprice.Text = fourthf.Text; break;
                                    case 4: t.filmprice.Text = fifthf.Text; break;
                                    case 5: t.filmprice.Text = sixf.Text; break;
                                    case 6: t.filmprice.Text = seventhf.Text; break;
                                    case 7: t.filmprice.Text = eighthf.Text; break;

                                }
                                t.dir = dir;
                                t.number = num.ToString();
                                t.Show();
                                num++;
                            }
                        }
                    }
                }


                db.ReservationPlaces.AddRange(places);
                db.SaveChanges();
            }
        
            
        }
    }
}
