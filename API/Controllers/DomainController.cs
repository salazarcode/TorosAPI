using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IXClassRepository _classRepo;

        public DomainController(IXClassRepository classRepo)
        {
            _classRepo = classRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var res = _classRepo.GetAll();
            return Ok(res);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetByID(int id)
        {
            var res = _classRepo.GetByID(id);
            return Ok(res);
        }
    }
}
