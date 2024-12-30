using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.DtoDocuments
{
    /// <summary>
    /// Par clave-valor dentro de la información desnormalizada de un objeto referenciado.
    /// "key" es el nombre del campo; "value" es su contenido.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DesnormalizedDataRowDocument
    {
        [BsonElement("key")]
        public string? Key { get; set; }

        [BsonElement("value")]
        public object? Value { get; set; }
    }
}
