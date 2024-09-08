using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IEmailService
{
    public class Emailservice : IEmailservice
    {
        private readonly IConfiguration _configuration;

        public Emailservice(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Get email settings from configuration
                string fromMail = _configuration["EmailSettings1:From"];
                string fromPassword = _configuration["EmailSettings1:Password"];

                // Create the MailMessage
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(new MailAddress(toEmail));

                // Configure the SMTP client
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(mailMessage); // Send email asynchronously
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new InvalidOperationException("Error sending email", ex);
            }
        }
    }
}
