namespace Domain.Core.ValueObjects
{
    /// <summary>
    /// Modela la parte desnormalizada de un DomainReferenceValue (campos replicados + fecha).
    /// </summary>
    public class DomainDesnormalizedData
    {
        /// <summary>
        /// Fecha de la última sincronización de la data desnormalizada.
        /// Equivale a ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument.LastUpdateDate.
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Lista de pares (key, value) con los campos replicados.
        /// </summary>
        public List<DomainDesnormalizedItem> Fields { get; set; } = new();
    }
}
