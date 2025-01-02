namespace Domain.Core.ValueObjects
{
    /// <summary>
    /// Contiene la data cuando un DomainObjectValue es referencia (Value es un DomainReferenceValue).
    /// </summary>
    public class DomainReferenceValue
    {
        /// <summary>
        /// El ID (ObjectId/string) del objeto referenciado.
        /// Equivale a ObjectDocument.ValueDocument.ReferenceValueDocument.ReferenceId.
        /// </summary>
        public string? ReferenceId { get; set; }

        /// <summary>
        /// Campos desnormalizados más fecha de última actualización.
        /// </summary>
        public DomainDesnormalizedData? Desnormalized { get; set; }
    }
}
