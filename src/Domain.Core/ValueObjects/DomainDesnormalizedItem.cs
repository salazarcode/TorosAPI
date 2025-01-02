namespace Domain.Core.ValueObjects
{
    /// <summary>
    /// Par clave-valor de un campo desnormalizado.
    /// </summary>
    public class DomainDesnormalizedItem
    {
        /// <summary>
        /// Nombre del campo desnormalizado.
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Valor almacenado para la clave desnormalizada.
        /// </summary>
        public object? Value { get; set; }
    }
}
