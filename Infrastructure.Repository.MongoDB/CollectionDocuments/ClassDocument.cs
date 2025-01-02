using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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
        /// Id del tenant al que corresponde esta clase. Ej.: "MiEmpresa".
        /// </summary>
        [BsonElement("tenantId")]
        public string? TenantId { get; set; }

        /// <summary>
        /// Display name de la clase, p.ej.: "Producto".
        /// </summary>
        [BsonElement("name")]
        public string? Name { get; set; }

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
        public List<PropertyDocument> Properties { get; set; } = new();

        /// <summary>
        /// Fecha de creación de esta versión.
        /// </summary>
        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Describe una propiedad específica dentro de la definición de Clase (en una versión).
        /// </summary>
        [BsonIgnoreExtraElements]
        public class PropertyDocument
        {
            /// <summary>
            /// Identificador de la propiedad, p.ej.: "MiEmpresa.Producto.v1.Nombre".
            /// </summary>
            [BsonId]
            public string? Id { get; set; }

            /// <summary>
            /// Tipo de la propiedad (String, Int, Double, DateTime, etc.) 
            /// o referencia a otra clase (p.ej. "MiEmpresa.Categoria.v1").
            /// </summary>
            [BsonElement("typeClass")]
            public string? TypeClass { get; set; }

            /// <summary>
            /// Indica si la propiedad es una referencia (a otro documento en la colección "Objects").
            /// </summary>
            [BsonElement("isReference")]
            public bool IsReference { get; set; }

            /// <summary>
            /// Mínimo permitido (opcional), puede referirse a valores numéricos
            /// o longitud para strings.
            /// </summary>
            [BsonElement("min")]
            public int? Min { get; set; }

            /// <summary>
            /// Máximo permitido (opcional), puede referirse a valores numéricos
            /// o longitud para strings.
            /// </summary>
            [BsonElement("max")]
            public int? Max { get; set; }

            /// <summary>
            /// Campos que deben desnormalizarse si la propiedad es una referencia.
            /// </summary>
            [BsonElement("desnormalizedFields")]
            public List<string>? DesnormalizedFields { get; set; } = [];
        }
    }
}
