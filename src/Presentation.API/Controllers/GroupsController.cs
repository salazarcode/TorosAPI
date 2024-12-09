using Application.Services.Interfaces;
using AutoMapper;
using Domain.Core.Entities;
using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsRepository _groupRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public GroupsController(IGroupsRepository groupRepository, IMapper mapper, IEmailService emailService)
        {
            _groupRepository = groupRepository;             
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            var res = await _groupRepository.GetAll();
            var mapped = _mapper.Map<Group>(res);
            return res;
        }

        public record EmailRequest(string emailTo, string subject, string message);

        [HttpPost]
        [Route("send-mail")]
        public async Task<IActionResult> SendEmail(EmailRequest request)
        {
            await _emailService.SendEmailAsync(request.emailTo, request.subject, request.message, "");
            return Ok();
        }
    }
}
