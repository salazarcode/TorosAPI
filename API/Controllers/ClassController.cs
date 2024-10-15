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

        private ClassDetailRS GetMappedClass(XClass entity) {
            ClassDetailRS res = new ClassDetailRS();

            res = new ClassDetailRS();
            res.ID = entity.Id;
            res.Name = entity.Name ?? "";
            res.Key = entity.Key;
            res.Properties = entity.PropertyClasses.Select(p => new PropertyDTO
            {
                ID = p.Id,
                Name = p.Name ?? "",
                Key = p.Key,
                ClassName = p.PropertyClass?.Name ?? ""
            }).ToList();
            res.Ancestries = entity.Parents.Select(a => new AncestryDTO
            {
                Key = a.Key,
                Name = a.Name ?? "",
                IsPrimitive = a.IsPrimitive,
            }).ToList();

            return res;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult?> Get([FromRoute] int id)
        {
            var entity = await _classService.Get(id);

            ClassDetailRS? res = null;

            if (entity is not null)
            {
                res = GetMappedClass(entity);
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
                    ID = x.Id,
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
                Id = id,
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
                ClassId = c.Id,
                PropertyClassId = input.PropertyClassID,
                Name = input.Name,
                Key = input.Key
            };

            var newPropertyID = await _classService.AddProperty(c.Id, property);

            property.Id = newPropertyID;

            var res = await _classService.Get(c.Id);
            var x = GetMappedClass(res);
            return Ok(x);
        }

        [HttpDelete]
        [Route("{ClassID}/property/{PropertyID}")]
        public async Task<IActionResult> RemoveProperty([FromRoute] int ClassID, [FromRoute] int PropertyID)
        {
            var c = await _classService.Get(ClassID);
            if (c is null)
                return NotFound();

            var property = c.PropertyClasses.FirstOrDefault(x => x.Id == PropertyID);

            if (property is null)
                return NotFound();

            var deleted = await _classService.RemoveProperty(property.Id);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _classService.Delete(id);

            return Ok();
        }
    }
}
