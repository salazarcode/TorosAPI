using Microsoft.AspNetCore.Mvc;
using Domain.Core.Entities;
using Domain.Core.Interfaces;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectsController : ControllerBase
    {
        private readonly IObjectRepository _objectRepository;

        public ObjectsController(IObjectRepository objectRepository)
        {
            _objectRepository = objectRepository;
        }

        // GET: api/objects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var objects = await _objectRepository.GetAll();
            return Ok(objects);
        }

        // GET: api/objects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var domainObject = await _objectRepository.Get(id);
            if (domainObject == null)
                return NotFound();

            return Ok(domainObject);
        }

        // POST: api/objects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DomainObject domainObject)
        {
            if (domainObject == null)
                return BadRequest("Object data is required.");

            var createdObject = await _objectRepository.Create(domainObject);
            return CreatedAtAction(nameof(GetById), new { id = createdObject?.Id }, createdObject);
        }

        // PUT: api/objects/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] DomainObject domainObject)
        {
            if (domainObject == null)
                return BadRequest("Object data is required.");

            domainObject.Id = id;

            var updated = await _objectRepository.Update(domainObject);
            if (updated == null)
                return NotFound(); // No se encontró el objeto para actualizar

            return Ok(updated);
        }

        // DELETE: api/objects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _objectRepository.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
