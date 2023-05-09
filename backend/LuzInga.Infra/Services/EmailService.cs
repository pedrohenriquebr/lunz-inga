using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LuzInga.Application.Configuration;
using LuzInga.Application.Services;
using Microsoft.Extensions.Options;

namespace LuzInga.Infra.Services
{
    public class EmailProvider : IEmailProvider
    {
        private readonly IOptions<EmailProviderConfig> config;

        public EmailProvider(IOptions<EmailProviderConfig> config)
        {
            this.config = config;
        }
        public void SendEmail(string to, string subject, string body)
        {
            using SmtpClient smtp = new SmtpClient();
            using MailMessage message = new MailMessage();
            message.From = new MailAddress(config.Value.Email);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            smtp.Host = config.Value.Host;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(config.Value.Email, config.Value.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 10000; // 10 seconds
            smtp.Send(message);
        }
    }
}