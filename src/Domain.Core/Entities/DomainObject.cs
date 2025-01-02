
using Domain.Core.ValueObjects;

namespace Domain.Core.Entities
{
    /// <summary>
    /// Representa una instancia de una Clase en una versión específica (un "objeto") a nivel de dominio.
    /// </summary>
    public class DomainObject
    {
        /// <summary>
        /// Identificador del objeto (string u ObjectId en Mongo).
        /// Equivale a ObjectDocument.Id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Apunta a la clase con su versión, p.ej.: "MiEmpresa.Producto.v1".
        /// Equivale a ObjectDocument.ClassId.
        /// </summary>
        public string? ClassId { get; set; }

        /// <summary>
        /// Arreglo de pares (propertyId, value) que almacenan los datos del objeto.
        /// Equivale a ObjectDocument.Values -> DomainObjectValue.
        /// </summary>
        public List<DomainObjectValue> Values { get; set; } = new();

    }
}
