using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using DevnotMentor.Configurations.Context;

namespace DevnotMentor.Business.Utilities.Email.SmtpMail
{
    public class SmtpMailService : IMailService
    {
        private readonly IDevnotConfigurationContext devnotConfigurationContext;

        public SmtpMailService(IDevnotConfigurationContext devnotConfigurationContext)
        {
            this.devnotConfigurationContext = devnotConfigurationContext;
        }


        public async Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isBodyHtml = true)
        {
            using (MailMessage mail = new MailMessage())
            {
                using (SmtpClient SmtpServer = new SmtpClient(devnotConfigurationContext.SmtpHost))
                {
                    mail.From = new MailAddress(devnotConfigurationContext.SmtpEmail);

                    foreach (var email in to)
                    {
                        mail.To.Add(email);
                    }

                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isBodyHtml;

                    SmtpServer.Port = devnotConfigurationContext.SmtpPort;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(devnotConfigurationContext.SmtpEmail, devnotConfigurationContext.SmtpPassword);
                    SmtpServer.EnableSsl = true;

                    await SmtpServer.SendMailAsync(mail);
                }
            }
        }
    }
}
