using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ProjectManager.Features.SendEmail
{
    public class EmailClient
    {
        private string _smtpEmail;
        private string _smtpPassword;
        private string _receiver;
        private SmtpClient _client;

        public EmailClient()
        {
            _smtpEmail = ConfigurationManager.AppSettings["email.sender"];
            _smtpPassword = ConfigurationManager.AppSettings["email.sender.password"];
            _receiver = ConfigurationManager.AppSettings["email.receiver"];
            _client = new SmtpClient();
        }

        public void SendEmail(string subject, string body)
        {
            MailMessage msg = new MailMessage(_smtpEmail, _receiver, subject, body);
            _client.UseDefaultCredentials = false;
            _client.Host = "smtp.gmail.com";
            _client.Port = 587;
            _client.EnableSsl = true;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.Credentials = new NetworkCredential(_smtpEmail, _smtpPassword);
            _client.Send(msg);
            msg.Dispose();
        }
    }
}