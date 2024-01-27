using System.Net;
using System.Net.Mail;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["SmtpSettings:From"]),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        mailMessage.To.Add(new MailAddress(email));

        using var client = new SmtpClient(_configuration["SmtpSettings:Host"], 
                                          int.Parse(_configuration["SmtpSettings:Port"]))
        {
            Credentials = new NetworkCredential(_configuration["SmtpSettings:Username"], 
                                                _configuration["SmtpSettings:Password"]),
            EnableSsl = true,
        };

        await client.SendMailAsync(mailMessage);
    }
}
