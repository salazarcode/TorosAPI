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
            /// o un objeto con la estructura de ObjectReferenceValueDocument si es referencia.
            /// </summary>
            [BsonElement("value")]
            public object? Value { get; set; }

            /// <summary>
            /// Representa el contenido de "value" cuando isReference = true.
            /// Contiene un "referenceId" y la parte desnormalizada.
            /// </summary>
            [BsonIgnoreExtraElements]
            public class ReferenceValueDocument
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
                    public List<DesnormalizedItemDocument> Fields { get; set; } = [];

                    /// <summary>
                    /// Par clave-valor dentro de la información desnormalizada de un objeto referenciado.
                    /// "key" es el nombre del campo; "value" es su contenido.
                    /// </summary>
                    [BsonIgnoreExtraElements]
                    public class DesnormalizedItemDocument
                    {
                        [BsonElement("key")]
                        public string? Key { get; set; }

                        [BsonElement("value")]
                        public object? Value { get; set; }
                    }
                }
            }
        }
    }
}
