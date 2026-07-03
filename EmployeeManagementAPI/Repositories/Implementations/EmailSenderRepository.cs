using EmployeeManagementAPI.Repositories.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class SmtpEmailSenderRepository : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;

    public SmtpEmailSenderRepository(string host, int port, string fromEmail, string password)
    {
        _fromEmail = fromEmail;
        _smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, string bccEmail = null)
    {
        var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
        {
            IsBodyHtml = false
        };

        if (!string.IsNullOrEmpty(bccEmail))
        {
            mailMessage.Bcc.Add(bccEmail);
        }

        await _smtpClient.SendMailAsync(mailMessage);
    }
}
