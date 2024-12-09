using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Application.Services.Interfaces;
using Application.Services.DTOs;
using Amazon;

namespace Infrastructure.Email.AwsSES
{
    public class AwsSesEmailService : IEmailService
    {
        private readonly IAmazonSimpleEmailService _sesClient;
        private readonly AwsSesConfiguration _config;
        private readonly ILogger<AwsSesEmailService> _logger;

        public AwsSesEmailService(
            IConfiguration configuration,
            ILogger<AwsSesEmailService> logger)
        {
            _logger = logger;

            // Cargar configuración
            _config = new AwsSesConfiguration();
            configuration.GetSection("AWS:SES").Bind(_config);

            // Validar configuración mínima
            if (string.IsNullOrEmpty(_config.AccessKey) ||
                string.IsNullOrEmpty(_config.SecretKey) ||
                string.IsNullOrEmpty(_config.Region))
            {
                throw new ArgumentException("AWS SES configuration is incomplete");
            }

            // Inicializar cliente SES
            var sesConfig = new AmazonSimpleEmailServiceConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_config.Region)
            };

            _sesClient = new AmazonSimpleEmailServiceClient(
                _config.AccessKey,
                _config.SecretKey,
                sesConfig
            );
        }

        public async Task<bool> SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            string textBody = "",
            List<EmailAttachment>? attachments = null)
        {
            try
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = string.IsNullOrEmpty(_config.FromName)
                        ? _config.FromEmail
                        : $"{_config.FromName} <{_config.FromEmail}>",

                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { to }
                    },

                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            }
                        }
                    }
                };

                // Agregar cuerpo texto plano si se proporciona
                if (!string.IsNullOrEmpty(textBody))
                {
                    sendRequest.Message.Body.Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = textBody
                    };
                }

                // Agregar attachments si existen
                if (attachments?.Any() == true)
                {
                    var rawMessage = await CreateRawEmailWithAttachments(
                        to,
                        subject,
                        htmlBody,
                        textBody,
                        attachments
                    );

                    var rawEmailRequest = new SendRawEmailRequest { RawMessage = rawMessage };
                    await _sesClient.SendRawEmailAsync(rawEmailRequest);
                }
                else
                {
                    await _sesClient.SendEmailAsync(sendRequest);
                }

                _logger.LogInformation("Email sent successfully to {Email}", to);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", to);
                return false;
            }
        }

        public async Task<bool> SendBulkEmailAsync(
            List<string> to,
            string subject,
            string htmlBody,
            string textBody = "",
            List<EmailAttachment>? attachments = null)
        {
            try
            {
                var tasks = to.Select(email => SendEmailAsync(
                    email,
                    subject,
                    htmlBody,
                    textBody,
                    attachments
                ));

                var results = await Task.WhenAll(tasks);

                var allSucceeded = results.All(x => x);

                if (!allSucceeded)
                {
                    _logger.LogWarning("Some emails in bulk send operation failed");
                }

                return allSucceeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk email send operation");
                return false;
            }
        }

        private async Task<RawMessage> CreateRawEmailWithAttachments(
            string to,
            string subject,
            string htmlBody,
            string textBody,
            List<EmailAttachment> attachments)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);

            // Headers básicos
            await writer.WriteLineAsync($"From: {_config.FromEmail}");
            await writer.WriteLineAsync($"To: {to}");
            await writer.WriteLineAsync("Subject: =?UTF-8?B?" +
                Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(subject)) + "?=");

            // Generar boundary único
            var boundary = $"==Multipart_Boundary_{DateTime.UtcNow.Ticks}==";
            await writer.WriteLineAsync($"Content-Type: multipart/mixed; boundary=\"{boundary}\"");
            await writer.WriteLineAsync("MIME-Version: 1.0");
            await writer.WriteLineAsync();

            // Contenido del correo
            await writer.WriteLineAsync($"--{boundary}");
            await writer.WriteLineAsync("Content-Type: multipart/alternative; boundary=\"alt-{boundary}\"");
            await writer.WriteLineAsync();

            if (!string.IsNullOrEmpty(textBody))
            {
                await writer.WriteLineAsync($"--alt-{boundary}");
                await writer.WriteLineAsync("Content-Type: text/plain; charset=UTF-8");
                await writer.WriteLineAsync("Content-Transfer-Encoding: base64");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync(Convert.ToBase64String(
                    System.Text.Encoding.UTF8.GetBytes(textBody)));
                await writer.WriteLineAsync();
            }

            await writer.WriteLineAsync($"--alt-{boundary}");
            await writer.WriteLineAsync("Content-Type: text/html; charset=UTF-8");
            await writer.WriteLineAsync("Content-Transfer-Encoding: base64");
            await writer.WriteLineAsync();
            await writer.WriteLineAsync(Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(htmlBody)));
            await writer.WriteLineAsync();
            await writer.WriteLineAsync($"--alt-{boundary}--");

            // Adjuntos
            foreach (var attachment in attachments)
            {
                await writer.WriteLineAsync($"--{boundary}");
                await writer.WriteLineAsync($"Content-Type: {attachment.ContentType}; name=\"{attachment.FileName}\"");
                await writer.WriteLineAsync("Content-Transfer-Encoding: base64");
                await writer.WriteLineAsync($"Content-Disposition: attachment; filename=\"{attachment.FileName}\"");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync(Convert.ToBase64String(attachment.Content));
                await writer.WriteLineAsync();
            }

            await writer.WriteLineAsync($"--{boundary}--");
            await writer.FlushAsync();

            return new RawMessage { Data = memoryStream };
        }
    }
}
