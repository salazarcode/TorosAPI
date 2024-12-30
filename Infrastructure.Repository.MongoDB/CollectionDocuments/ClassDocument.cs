using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Infrastructure.Repository.MongoDB.DtoDocuments;

namespace Infrastructure.Repository.MongoDB.Models
{
    /// <summary>
    /// Representa la definición de una Clase en una versión específica,
    /// almacenada en la colección "Classes".
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ClassDocument
    {
        /// <summary>
        /// Identificador único, p.ej.: "MiEmpresa.Producto.v1".
        /// </summary>
        [BsonId]
        public string? Id { get; set; }

        /// <summary>
        /// Id base de la clase (sin versión). Ej.: "MiEmpresa.Producto".
        /// </summary>
        [BsonElement("baseClassId")]
        public string? BaseClassId { get; set; }

        /// <summary>
        /// Versión numérica (1, 2, 3...).
        /// </summary>
        [BsonElement("classVersion")]
        public int ClassVersion { get; set; }

        /// <summary>
        /// Indica si esta versión está activa para crear nuevos objetos.
        /// </summary>
        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Notas que describen los cambios o el contenido de esta versión.
        /// </summary>
        [BsonElement("releaseNotes")]
        public List<string> ReleaseNotes { get; set; } = [];

        /// <summary>
        /// Lista de propiedades definidas para esta versión.
        /// </summary>
        [BsonElement("properties")]
        public List<ClassPropertyDocument> Properties { get; set; } = new();

        /// <summary>
        /// Fecha de creación de esta versión.
        /// </summary>
        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }
}
