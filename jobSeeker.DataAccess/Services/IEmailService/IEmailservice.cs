using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IEmailService
{
    public interface IEmailservice
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
