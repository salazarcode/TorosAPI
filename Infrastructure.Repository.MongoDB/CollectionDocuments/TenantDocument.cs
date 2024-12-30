using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.Models
{
    /// <summary>
    /// Representa un Tenant (cliente/entidad) en el sistema.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class TenantDocument
    {
        /// <summary>
        /// Identificador del Tenant. 
        /// Ej.: "MiEmpresa".
        /// </summary>
        [BsonId]
        public string? Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del Tenant.
        /// </summary>
        [BsonElement("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Descripción o información adicional.
        /// </summary>
        [BsonElement("description")]
        public string? Description { get; set; }
    }
}
