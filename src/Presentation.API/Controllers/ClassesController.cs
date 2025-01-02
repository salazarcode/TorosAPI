using Microsoft.AspNetCore.Mvc;
using Domain.Core.Entities;
using Domain.Core.Interfaces;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepository;

        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        // GET: api/classes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classRepository.GetAll();
            return Ok(classes);
        }

        // GET: api/classes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var domainClass = await _classRepository.Get(id);
            if (domainClass == null)
                return NotFound();

            return Ok(domainClass);
        }

        // POST: api/classes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DomainClass domainClass)
        {
            if (domainClass == null)
                return BadRequest("Class data is required.");

            var createdClass = await _classRepository.Create(domainClass);
            return CreatedAtAction(nameof(GetById), new { id = createdClass?.Id }, createdClass);
        }

        // PUT: api/classes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] DomainClass domainClass)
        {
            if (domainClass == null)
                return BadRequest("Class data is required.");

            domainClass.Id = id;

            var updatedClass = await _classRepository.Update(domainClass);
            if (updatedClass == null)
                return NotFound(); // No se encontró la clase para actualizar

            return Ok(updatedClass);
        }

        // DELETE: api/classes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _classRepository.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
