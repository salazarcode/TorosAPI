using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsRepository _groupRepository;
        public GroupsController(IGroupsRepository groupRepository)
        {
            _groupRepository = groupRepository;                
        }

        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            return await _groupRepository.GetAll();
        }
    }
}
