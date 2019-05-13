using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KINOwpf
{
    public class KinoContext : DbContext
    {
        public KinoContext()
        : base("DBConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Seance> Seances { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SoldPlace> SoldPlaces { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<FilmsGenres> FilmsGenres { get; set; }
        public DbSet<FilmsDates> FilmsDates { get; set; }
        public DbSet<FilmsDatesSeances> FilmsDatesSeances { get; set; }
        public DbSet<ReservationCode> ReservationCodes { get; set; }
        public DbSet<ReservationPlace> ReservationPlaces { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Price> Prices { get; set; } 
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public string Time { get; set; }

        public virtual User User { get; set; }
    }

    public interface IObserver
    {
        void Update(Object ob);
    }

    public interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }

    public class Film : IObservable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public byte[] PosterByte { get; set; }
        public string Trailer { get; set; }
        public string Country { get; set; }
        public float RatingIMDb { get; set; }
        public float RatingKinopoisk { get; set; }
        public string Slogan { get; set; }
        public bool IsPremiere { get; set; }
        public DateTime PremierDate { get; set; }
        public virtual ICollection<IObserver> Subscribers { get; set; }
        public virtual ICollection<FilmsGenres> FilmsGenres { get; set; }
        public virtual ICollection<FilmsDates> FilmsDates { get; set; }
        public Film()
        {
            FilmsDates = new List<FilmsDates>();
            FilmsGenres = new List<FilmsGenres>();
            Subscribers = new List<IObserver>();
        }

        public void RegisterObserver(IObserver o)
        {
            Subscribers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            Subscribers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (IObserver o in Subscribers)
            {
                o.Update(this);
            }
        }

        public void ChangeState()
        {
            IsPremiere = false;
            NotifyObservers();
        }
    }

    public class Price
    {
        public int Id { get; set; }
        public int Range { get; set; }
        public int Cost { get; set; }
    }

    public class Subscription
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? FilmId { get; set; }
        public virtual User User { get; set; }
        public virtual Film Film { get; set; }
    }

    public class ReservationPlace
    {
        public int Id { get; set; }
        public int Range { get; set; }
        public int Place { get; set; }
        public int Price { get; set; }
        public bool Student { get; set; }
        public bool Retiree { get; set; }
        public int? CodeId { get; set; }
        public virtual ReservationCode ReservationCode { get; set; }
    }

    public class SoldPlace
    {
        public int Id { get; set; }
        public int Range { get; set; }
        public int Place { get; set; }
        
        public int? FilmDateSeanceId { get; set; }
        public virtual FilmsDatesSeances FilmDateSeance { get; set; }
    }

    public class ReservationCode
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int TotalPrice { get; set; }
        public int? UserId { get; set; }
        public int? FilmDateSeanceId { get; set; }
        public virtual User User { get; set; }
        public virtual FilmsDatesSeances FilmDateSeance { get; set; }

    }


    public class User : IPerson, IObserver
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PathForTickets { get; set; }

        public void Update(object ob)
        {
            Film film = (Film)ob;
            using (KinoContext db = new KinoContext())
            {
                db.Notifications.Add(new Notification { Message = $"Привет, {this.FirstName}!\nФильм {film.Name} уже в прокате, проверьте вкладку \"Сеансы\".\nУспейте забронировать билет!", Time = $"{DateTime.Now.Day.ToString("00")}.{DateTime.Now.Month.ToString("00")}\n{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}", UserId = this.Id });
                db.SaveChanges();
            }
        }
    }

    public class Admin : IPerson
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

   

    public class Date
    {
        public int Id { get; set; }
        public DateTime Title { get; set; }

        public virtual ICollection<FilmsDates> FilmsDates { get; set; }
        public Date()
        {
            FilmsDates = new List<FilmsDates>();
        }
    }

    public class Seance
    {
        public int Id { get; set; }
        public DateTime Title { get; set; }

        public virtual ICollection<FilmsDatesSeances> FilmsDatesSeances { get; set; }
        public Seance()
        {
            FilmsDatesSeances = new List<FilmsDatesSeances>();
        }
    }

    public class FilmsDates
    {
        public int Id { get; set; }
        public int? FilmId { get; set; }
        public int? DateId { get; set; }
        public virtual Film Film { get; set; }
        public virtual Date Date { get; set; }

        public virtual ICollection<FilmsDatesSeances> FilmsDatesSeances { get; set; }
        public FilmsDates()
        {
            FilmsDatesSeances = new List<FilmsDatesSeances>();
        }
    }

    public class FilmsDatesSeances
    {
        public int Id { get; set; }
        public int? FilmsDatesId { get; set; }
        public int? SeanceId { get; set; }
        public virtual FilmsDates FilmsDates { get; set; }
        public virtual Seance Seance { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<FilmsGenres> FilmsGenres { get; set; }
        public Genre()
        {
            FilmsGenres = new List<FilmsGenres>();
        }
    }

    public class FilmsGenres
    {
        public int Id { get; set; }
        public int? FilmId { get; set; }
        public int? GenreId { get; set; }
        public virtual Film Film { get; set; }
        public virtual Genre Genre { get; set; }
    }
    
}
