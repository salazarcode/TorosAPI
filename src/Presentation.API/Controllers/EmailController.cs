using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;                   
        }

        [HttpGet]
        public async Task<IActionResult> TestAwsMail(string to, string message) {
            try
            {
                var res = await _emailService.SendEmailAsync(to: to, subject: "Email from AWS SES", htmlBody: message);
                return Ok(res);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
