using System.Net.Mail;
using System.Net;
using BackEnd.Services.Interfaces;
using BackEnd.Infrastructure;

namespace BackEnd.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly AuthOptions _authOptions;

        public MailService(AuthOptions authOptions)
        {
            _authOptions = authOptions;
        }

        public void SendEmail(string toEmail, string subject, string content)
        {          

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_authOptions.SenderAddress);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = true;               

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(_authOptions.SenderAddress, _authOptions.SenderPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

      
        }
    }
}
