using API.Requests.Classes;
using API.Responses.Classes;
using Application;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly XClassService _classService;

        public ClassController(XClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult?> Get([FromRoute] int id)
        {
            var entity = await _classService.Get(id);

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
                    Key = p.Key,
                    Min = p.Min,
                    Max = p.Max,
                    ClassName = p.PropertyClass.Name
                }).ToList();
                res.Ancestries = entity.XAncestries.Select(a => new AncestryDTO
                {
                    Key = a.Parent.Key,
                    Name = a.Parent.Name,
                    IsPrimitive = a.Parent.IsPrimitive,
                }).ToList();
            }

            return entity is null ? NotFound() : Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var classes = await _classService.Get();

            var res = classes.Select(x =>
            {
                return new ClassRS
                {
                    ID = x.ID,
                    Key = x.Key,
                    Name = x.Name,
                    IsPrimitive = x.IsPrimitive
                };
            }).ToList();

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClassRQ input)
        {
            var classID = await _classService.Create(new XClass
            {
                Name = input.Name,
                IsPrimitive = input.IsPrimitive,
            });

            var res = await _classService.Get(classID);

            return Ok(res);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateClassRQ input)
        {
            var c = await _classService.Get(id);
            if (c is null)
                return NotFound();

            var updateClass = new XClass
            {
                ID = id,
                Name = input.Name,
                IsPrimitive = input.IsPrimitive,
                Key = input.Key                
            };

            var res = await _classService.Update(updateClass);

            return Ok();
        }

        [HttpPost]
        [Route("{ClassID}/property")]
        public async Task<IActionResult> AddProperty([FromRoute] int ClassID, [FromBody] AddPropertyRQ input)
        {
            var c = await _classService.Get(ClassID);

            if (c is null)
                return NotFound();

            var property = new XProperty
            {
                ClassID = c.ID,
                PropertyClassID = input.PropertyClassID,
                Name = input.Name,
                Key = input.Key,
                Min = input.Min,
                Max = input.Max,
            };

            var newPropertyID = await _classService.AddProperty(c.ID, property);

            property.ID = newPropertyID;

            var res = await _classService.Get(c.ID);

            return Ok(res);
        }

        [HttpDelete]
        [Route("{ClassID}/property/{PropertyID}")]
        public async Task<IActionResult> RemoveProperty([FromRoute] int ClassID, [FromRoute] int PropertyID)
        {
            var c = await _classService.Get(ClassID);
            if (c is null)
                return NotFound();

            var property = c.XProperties.FirstOrDefault(x => x.ID == PropertyID);

            if (property is null)
                return NotFound();

            var deleted = await _classService.RemoveProperty(property.ID);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var res = await _classService.Delete(id);

            return Ok(res);
        }
    }
}
