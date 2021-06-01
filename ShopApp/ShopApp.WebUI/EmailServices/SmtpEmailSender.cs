using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        string host;
        int port;
        bool enableSSL;
        string username;
        string password;
        public SmtpEmailSender(string host, int port, bool enableSSL, string username, string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.username = username;
            this.password = password;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSSL
            };

            return client.SendMailAsync(
                new MailMessage(this.username, email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                }
            );
        }
    }
}
