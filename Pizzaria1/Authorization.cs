using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace Pizzaria1
{
    public abstract class IPerson
    {
        public int Id;
        public string Login;
        public string Password;
    }

    class Authorization
    {
        public int number = 0;
        User user = new User();
        public IPerson Auth(string login, string password)
        {
            using (FoodContext db = new FoodContext())
            {
                IPerson person;
                try {
                    person = db.Users.First(x => (x.Login == login || x.Email == login && x.Password == password)); }
                catch (Exception) { person = null; };

                if (person == null)
                {
                    try
                    {
                        person = db.Admins.First(x => (x.Login == login && x.Password == password));
                    }
                    catch (Exception) { person = null; }
                }

                return person;
            }
        }

        public int Registration(string FirstName, string LastName, string Login, string Password, string Email, string Address)
        {
            using (FoodContext db = new FoodContext())
            {
                bool log = false;
                bool em = false;
                Random rnd = new Random();
                number = rnd.Next(1000, 9999);

                try { db.Users.First(x => (x.Login == Login)); }
                catch (Exception) { log = true; }

                if (log == false)
                    return 1;
              
                try { db.Users.First(x => x.Email == Email); }
                catch (Exception) { em = true; }

                if (em == false)
                    return 2;
                
                    user = new User { FirstName = FirstName, LastName = LastName, Login = Login, Password = Password, Email = Email, Address = Address };
                    SendMail("Код подтверждения регистрации: " + number, Email);
                    return number;
                
                
            }
        }

        public bool FinalRegistration (int number)
        {
            if (this.number == number)
            {
                using (FoodContext db = new FoodContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            else return false;
        }

        public bool SendMail(string message, string email)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("dimaalimonov1@gmail.com", "KINOAFISHA");
                // кому отправляем
                MailAddress to = new MailAddress(email);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Подтверждение регистрации";
                // текст письма
                m.Body = ($"<h2> {message} </h2>");
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("sburchinskaya2109@gmail.com", "Sashulia2109!");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

    class FoodContext : DbContext
    {
        public FoodContext() : base("DbConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
    }

    public class User : IPerson
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public User()
        {
            Purchases = new List<Purchase>();
        }

    }

    public class Admin : IPerson
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }

        public Category()
        {
            Dishes = new List<Dish>();
        }
    }

    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Photo { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<DishIngredient> DishIngredients { get; set; }
        public Dish()
        {
            DishIngredients = new List<DishIngredient>();
        }
    }

    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DishIngredient> DishIngredients { get; set; }
        public Ingredient()
        {
            DishIngredients = new List<DishIngredient>();
        }
    }

    public class DishIngredient
    {
        public int Id { get; set; }
        public int? DishId { get; set; }
        public int? IngredientId { get; set; }

        public Dish Dish { get; set; }
        public Ingredient Ingredient { get; set; }
    }

    public class Purchase
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public bool Confirmation { get; set; }
        public int? UserId { get; set; }

        public User User { get; set; }
    }
}
    