using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.Models
{
    /// <summary>
    /// Modela cada elemento de "values" en ObjectDocument.
    /// - propertyId: la propiedad que se está almacenando
    /// - value: valor real (puede ser primitivo o un ReferenceValueDocument)
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ValueDocument
    {
        /// <summary>
        /// Identificador único de la propiedad (incluyendo versión),
        /// p.ej.: "MiEmpresa.Producto.v1.Nombre".
        /// </summary>
        [BsonElement("propertyId")]
        public string? PropertyId { get; set; }

        /// <summary>
        /// Puede ser un valor primitivo (string, int, double, DateTime, etc.) 
        /// o un objeto con la estructura de ReferenceValueDocument si es referencia.
        /// </summary>
        [BsonElement("value")]
        public object? Value { get; set; }
    }
}
