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
        private readonly IMapper _mapper;
        public GroupsController(IGroupsRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;             
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            var res = await _groupRepository.GetAll();
            var mapped = _mapper.Map<Group>(res);
            return res;
        }
    }
}
