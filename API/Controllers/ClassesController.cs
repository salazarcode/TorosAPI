using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IXClassRepository _classRepo;

        public ClassesController(IXClassRepository classRepo)
        {
            _classRepo = classRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var res = await _classRepo.Details(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await _classRepo.All();
            return Ok(res);
        }
    }
}
