namespace SchoolSystem.Services.Email
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
       Task SendAsync(string smtpSenderEmail, string smtpSenderApplicationPassword, string toEmail, string toFullName, string subject, string message);
    }
}
