using Microsoft.AspNetCore.Mvc;
using Domain.Core.Entities;
using Domain.Core.Interfaces;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantsController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        // GET: api/tenants
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantRepository.GetAll();
            return Ok(tenants);
        }

        // GET: api/tenants/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tenant = await _tenantRepository.Get(id);
            if (tenant == null)
                return NotFound();

            return Ok(tenant);
        }

        // POST: api/tenants
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DomainTenant tenant)
        {
            if (tenant == null)
                return BadRequest("Tenant data is required.");

            var createdTenant = await _tenantRepository.Create(tenant);

            // Si deseas devolver 201 Created con la ubicación:
            return CreatedAtAction(nameof(GetById), new { id = createdTenant?.Id }, createdTenant);
        }

        // PUT: api/tenants/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] DomainTenant tenant)
        {
            if (tenant == null)
                return BadRequest("Tenant data is required.");

            // Aseguramos que el ID en la URL y el del objeto coincidan
            tenant.Id = id;

            var updatedTenant = await _tenantRepository.Update(tenant);
            if (updatedTenant == null)
                return NotFound(); // No se encontró el tenant para actualizar

            return Ok(updatedTenant);
        }

        // DELETE: api/tenants/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _tenantRepository.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
