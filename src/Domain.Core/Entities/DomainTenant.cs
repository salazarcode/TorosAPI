namespace Domain.Core.Entities
{
    /// <summary>
    /// Representa el Tenant a nivel de dominio.
    /// </summary>
    public class DomainTenant
    {
        /// <summary>
        /// Identificador del Tenant (string en lugar de int).
        /// Equivale a TenantDocument.Id en MongoDB.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del Tenant.
        /// Equivale a TenantDocument.Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descripción o información adicional.
        /// Equivale a TenantDocument.Description.
        /// </summary>
        public string? Description { get; set; }
    }
}
