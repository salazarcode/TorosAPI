using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email.SendGrid
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridEmailService(IConfiguration configuration)
        {
            var apiKey = configuration["SendGrid:ApiKey"]
                ?? throw new ArgumentNullException("SendGrid:ApiKey no está configurado");
            _sendGridClient = new SendGridClient(apiKey);
            _fromEmail = configuration["SendGrid:FromEmail"]
                ?? throw new ArgumentNullException("SendGrid:FromEmail no está configurado");
            _fromName = configuration["SendGrid:FromName"] ?? "Tu MVP";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent, string plainTextContent)
        {
            var message = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent
            };

            message.AddTo(new EmailAddress(to));

            var response = await _sendGridClient.SendEmailAsync(message);
            return response.IsSuccessStatusCode;
        }
    }
}
