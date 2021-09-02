using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevnotMentor.Utilities.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isBodyHtml = true);
    }
}
