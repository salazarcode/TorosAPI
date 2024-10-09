using API.Requests.Classes;
using API.Responses.Classes;
using Domain.Models;
using Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IXClassRepository _classRepo;

        public ClassController(IXClassRepository classRepo)
        {
            _classRepo = classRepo;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult?> Get([FromRoute] int id)
        {
            var entity = await _classRepo.Details(id);
            ClassDetailRS? res = null;

            if (entity is not null)
            {
                res = new ClassDetailRS();
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

            return entity is null ? NotFound() : Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var classes = await _classRepo.All();

            var res = classes.Select(x => {
                return new ClassRS
                {
                    ID = x.ID,
                    Name = x.Name,
                    IsPrimitive = x.IsPrimitive
                };
            }).ToList();

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassRQ input)
        {
            var classID = await _classRepo.Create(new XClass
            {
                Name = input.Name,
                IsPrimitive = input.IsPrimitive,
            });

            var res = await _classRepo.Details(classID);

            return Ok(res);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateClassRQ input)
        {
            var c = await _classRepo.Details(id);
            if (c is null)
                return NotFound();

            var updateClass = new XClass
            {
                Name = input.Name,
                IsPrimitive = input.IsPrimitive,
                ID = id
            };

            var res = await _classRepo.Update(updateClass);

            return Ok();
        }

        [HttpPost]
        [Route("{ClassID}/property")]
        public async Task<IActionResult> AddProperty([FromRoute] int ClassID, [FromBody] AddPropertyRQ input)
        {
            var c = await _classRepo.Details(ClassID);
            if (c is null)
                return NotFound();

            var property = new XProperty {
                ClassID = c.ID,
                PropertyClassID = input.PropertyClassID,
                Name = input.Name,
                Min = input.Min,
                Max = input.Max,
            };

            var newPropertyID = await _classRepo.AddProperty(c.ID, property);

            property.ID = newPropertyID;

            var res = await _classRepo.Details(c.ID);

            return Ok(res);
        }

        [HttpDelete]
        [Route("{ClassID}/property/{PropertyID}")]
        public async Task<IActionResult> RemoveProperty([FromRoute] int ClassID, [FromRoute] int PropertyID)
        {
            var c = await _classRepo.Details(ClassID);
            if (c is null)
                return NotFound();

            var property = c.XProperties.FirstOrDefault(x => x.ID == PropertyID);

            if (property is null)
                return NotFound();

            var deleted = await _classRepo.RemoveProperty(property.ID);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var res = await _classRepo.Delete(id);

            return Ok(res);
        }
    }
}
