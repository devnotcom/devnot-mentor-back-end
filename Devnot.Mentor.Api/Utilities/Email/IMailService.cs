using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isBodyHtml = true);
    }
}
