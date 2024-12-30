using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Repository.MongoDB.DtoDocuments
{
    /// <summary>
    /// Almacena la fecha de última actualización y los campos desnormalizados
    /// de una referencia.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DesnormalizedDataDocument
    {
        /// <summary>
        /// Fecha de la última sincronización de los campos desnormalizados.
        /// </summary>
        [BsonElement("lastUpdateDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Lista de pares (key, value) con los campos replicados del objeto referenciado.
        /// </summary>
        [BsonElement("fields")]
        public List<DesnormalizedDataRowDocument> Fields { get; set; } = [];
    }
}
