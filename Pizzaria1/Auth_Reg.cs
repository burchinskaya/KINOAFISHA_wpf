using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KINOwpf
{
    class Auth_Reg
    {
        public bool SendMail(string message, string email)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("dimaalimonov1@gmail.com", "uLimona");
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
