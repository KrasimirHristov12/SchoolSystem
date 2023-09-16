namespace SchoolSystem.Services.Email
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        public async Task SendAsync(string smtpSenderEmail, string smtpSenderApplicationPassword, string toEmail, string toFullName, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpSenderEmail, smtpSenderApplicationPassword),
            };

            await client.SendMailAsync(new MailMessage(smtpSenderEmail, toEmail, subject, message));

        }
    }
}
