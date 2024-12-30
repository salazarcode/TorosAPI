using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.DtoDocuments
{
    /// <summary>
    /// Describe una propiedad específica dentro de la definición de Clase (en una versión).
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ClassPropertyDocument
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
