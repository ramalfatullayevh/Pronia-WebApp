using ProniaMVC.Abstractions;
using System.Net;
using System.Net.Mail;
using ProniaMVC.Abstractions;

namespace ProniaMVC.Services
{
    public class EmailService:IEmailService
    {
        IConfiguration _configuration { get; }   
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string mailTo, string subject, string body , bool IsBodyHtml=false)
        {
            SmtpClient smtpClient = new SmtpClient(_configuration["Email:Host"], Convert.ToInt32(_configuration["Email:Port"]));
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_configuration["Email:Login"], _configuration["Email:Password"]);
            MailAddress from = new MailAddress(_configuration["Email:Login"], "Pronia");
            MailAddress to = new MailAddress(mailTo);

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = IsBodyHtml;
            smtpClient.Send(message);

          
        }
    }
}
