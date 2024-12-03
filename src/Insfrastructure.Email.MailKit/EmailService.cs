using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlStringifiedMessageBody)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(emailSettings["Email"]);
        email.From.Add(MailboxAddress.Parse(emailSettings["Email"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlStringifiedMessageBody
        };
        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(emailSettings["Email"], emailSettings["Password"]);

            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Manejo de excepciones y logs
            Console.WriteLine($"Error al enviar el correo: {ex.Message}");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}
