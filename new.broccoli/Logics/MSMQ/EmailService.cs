using System;
using System.Net;
using System.Net.Mail;
using BroccoliTrade.Domain.Models;

namespace BroccoliTrade.Logics.MSMQ
{
    public class EmailService
    {
        public void SendMessage(EmailMessage message, string username, string password, string host, int port, bool enableSsl)
        {
            var from = new MailAddress(message.From, message.DisplayNameFrom);
			var to = new MailAddress(message.To);

            var mm = new MailMessage(from, to)
                {
                    Subject = message.Subject,
                    Body = message.Message,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true
                };

            var credentials = new NetworkCredential(username, password);
            var sc = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl, 
                    Credentials = credentials
                };

            sc.Send(mm);
        }
    }
}
