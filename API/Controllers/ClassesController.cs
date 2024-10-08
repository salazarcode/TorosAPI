using API.Responses.Domain;
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
        public async Task<IActionResult?> Get([FromRoute] int id)
        {
            var entity = await _classRepo.Details(id);
            ClassDetailDTO? res = null;

            if (entity is not null)
            {
                res = new ClassDetailDTO();
                res.ID = entity.ID;
                res.Name = entity.Name;
                res.Properties = entity.XProperties.Select(p => new PropertyDTO
                {
                    ID = p.ID,
                    Name = p.Name,
                    Min = p.Min,
                    Max = p.Max,
                    ClassName = p.XClass.Name
                }).ToList();
                res.Ancestries = entity.XAncestries.Select(a => new AncestryDTO
                {
                    Name = a.Parent.Name,
                    IsPrimitive = a.Parent.IsPrimitive,
                }).ToList();                
            }

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var classes = await _classRepo.All();

            var res = classes.Select(x => {
                return new ClassDTO
                {
                    ID = x.ID,
                    Name = x.Name,
                    IsPrimitive = x.IsPrimitive
                };
            }).ToList(); 

            return Ok(res);
        }
    }
}
