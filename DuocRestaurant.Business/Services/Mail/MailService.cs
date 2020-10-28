using Domain;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }

        public void SendMail(string mailTo, string subject, string body)
        {
            try
            {
                MimeMessage mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(mailSettings.Sender, mailSettings.Email));
                mail.To.Add(MailboxAddress.Parse(mailTo));
                mail.Subject = subject;
                mail.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(mailSettings.SMTP, mailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                    client.Authenticate(mailSettings.Email, mailSettings.Password);
                    client.Send(mail);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // log error

            }
        }
    }
}
