using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.Models
{
    /// <summary>
    /// Representa una instancia de una Clase en una versión específica,
    /// almacenada en la colección "Objects".
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ObjectDocument
    {
        /// <summary>
        /// Identificador del objeto, alfanumérico o un ObjectId en string.
        /// </summary>
        [BsonId]
        public string? Id { get; set; }

        /// <summary>
        /// Apunta a la clase con su versión, p.ej.: "MiEmpresa.Producto.v1".
        /// </summary>
        [BsonElement("classId")]
        public string? ClassId { get; set; }

        /// <summary>
        /// Arreglo de pares (propertyId, value) que almacenan los datos del objeto.
        /// </summary>
        [BsonElement("values")]
        public List<ValueDocument> Values { get; set; } = [];
    }
}
