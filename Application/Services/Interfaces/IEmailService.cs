using Application.Services.DTOs;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="textBody"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task<bool> SendEmailAsync(
            string to,
            string subject,
            string htmlBody,
            string textBody = "",
            List<EmailAttachment>? attachments = null
        );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="textBody"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task<bool> SendBulkEmailAsync(
            List<string> to,
            string subject,
            string htmlBody,
            string textBody = "",
            List<EmailAttachment>? attachments = null
        );
    }
}
