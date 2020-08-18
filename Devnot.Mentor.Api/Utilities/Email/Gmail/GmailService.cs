using DevnotMentor.Api.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Email.Gmail
{
    public class GmailService : IMailService
    {
        public AppSettings AppSettings { get; set; }

        public GmailService(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public async Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isBodyHtml = true)
        {
            using (MailMessage mail = new MailMessage())
            {
                using (SmtpClient SmtpServer = new SmtpClient(AppSettings.Smtp))
                {
                    mail.From = new MailAddress(AppSettings.Email);

                    foreach (var email in to)
                    {
                        mail.To.Add(email);
                    }

                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isBodyHtml;

                    SmtpServer.Port = AppSettings.Port;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(AppSettings.Email, AppSettings.Password);
                    SmtpServer.EnableSsl = true;

                    await SmtpServer.SendMailAsync(mail);
                }
            }
        }
    }
}
