namespace Domain.Core.ValueObjects
{
    /// <summary>
    /// Representa cada (propertyId, value) en la definición de un objeto.
    /// </summary>
    public class DomainObjectValue
    {
        /// <summary>
        /// Identificador único de la propiedad (incluye versión),
        /// p.ej.: "MiEmpresa.Producto.v1.Nombre".
        /// Equivale a ObjectDocument.ValueDocument.PropertyId.
        /// </summary>
        public string? PropertyId { get; set; }

        /// <summary>
        /// Indica si esta "Value" es una referencia a otro objeto.
        /// </summary>
        public bool IsReference { get; set; }

        /// <summary>
        /// Puede ser un valor primitivo (string, int, double, DateTime, etc.)
        /// o un DomainReferenceValue si es referencia.
        /// </summary>
        public object? Value { get; set; }
    }
}
