using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.ExternalIntegrations
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendMail(string reciepientAddress, string message,
            string subject)
        {
            bool result = false;
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(reciepientAddress));
                email.Subject = subject;
                email.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);

                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

                    await client.SendAsync(email);

                    client.Disconnect(true);

                    result = true;
                }

                return result;
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }
    }
}
