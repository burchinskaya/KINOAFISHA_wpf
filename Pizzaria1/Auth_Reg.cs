using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KINOwpf
{
    public abstract class IPerson
    {
        public int Id;
        public string Login;
        public string Password;
    }

    class Auth_Reg
    {
        public int number = 0;
        User user = new User();
        public IPerson Auth(string login, string password)
        {
            using (KinoContext db = new KinoContext())
            {
                IPerson person;
                try
                {
                    person = db.Users.First(x => (x.Login == login || x.Email == login && x.Password == password));
                }
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

        public int Registration(string FirstName, string LastName, string Login, string Password, string Email)
        {
            using (KinoContext db = new KinoContext())
            {
                Random rnd = new Random();
                number = rnd.Next(1000, 9999);

                if (db.Users.Where(x => x.Email == Email).Count() != 0)
                    return 2;

                else if (db.Users.Where(x => x.Login == Login).Count() != 0 || db.Admins.Where(x => x.Login == Login).Count() != 0)
                    return 1;

                else
                {
                    user = new User { FirstName = FirstName, LastName = LastName, Login = Login, Password = Password, Email = Email };
                    SendMail("Код подтверждения регистрации: " + number, Email);
                    return number;
                }

            }
        }

        public bool FinalRegistration(int number)
        {
            if (this.number == number)
            {
                using (KinoContext db = new KinoContext())
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
                MailAddress from = new MailAddress("steam.faq.rus@gmail.com", "KINOAFISHA");
                // кому отправляем
                MailAddress to = new MailAddress(email);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Подтверждение";
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
}
