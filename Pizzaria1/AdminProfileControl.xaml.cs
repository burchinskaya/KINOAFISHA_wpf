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
    public partial class AdminProfileControl : UserControl
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

        public AdminProfileControl(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
           
        }

       
    }
}
