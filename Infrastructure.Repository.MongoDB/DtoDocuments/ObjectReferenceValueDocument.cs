using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Infrastructure.Repository.MongoDB.DtoDocuments;

namespace Infrastructure.Repository.MongoDB.Models
{
    /// <summary>
    /// Representa el contenido de "value" cuando isReference = true.
    /// Contiene un "referenceId" y la parte desnormalizada.
    /// </summary>
    [BsonIgnoreExtraElements]
    public partial class ReferenceValueDocument
    {
        /// <summary>
        /// El ID (generalmente ObjectId en string) del objeto referenciado.
        /// </summary>
        [BsonElement("referenceId")]
        public string? ReferenceId { get; set; }

        /// <summary>
        /// Datos desnormalizados y fecha de la última actualización.
        /// </summary>
        [BsonElement("desnormalized")]
        public DesnormalizedDataDocument? Desnormalized { get; set; }
    }
}
