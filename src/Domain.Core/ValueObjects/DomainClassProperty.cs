namespace Domain.Core.ValueObjects
{
    /// <summary>
    /// Representa una propiedad específica en la definición de Clase (y su versión).
    /// </summary>
    public class DomainClassProperty
    {
        /// <summary>
        /// Identificador de la propiedad, p.ej. "MiEmpresa.Producto.v1.Nombre".
        /// Equivale a ClassDocument.PropertyDocument.Id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Tipo de la propiedad (String, Int, Double, DateTime, etc.) 
        /// o referencia a otra clase, p.ej. "MiEmpresa.Categoria.v1".
        /// Equivale a ClassDocument.PropertyDocument.TypeClass.
        /// </summary>
        public string? TypeClass { get; set; }

        /// <summary>
        /// Indica si la propiedad es una referencia a otro documento.
        /// Equivale a ClassDocument.PropertyDocument.IsReference.
        /// </summary>
        public bool IsReference { get; set; }

        /// <summary>
        /// Mínimo permitido (numérico o longitud).
        /// Equivale a ClassDocument.PropertyDocument.Min.
        /// </summary>
        public int? Min { get; set; }

        /// <summary>
        /// Máximo permitido (numérico o longitud).
        /// Equivale a ClassDocument.PropertyDocument.Max.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Campos que se desnormalizan si la propiedad es referencia.
        /// Equivale a ClassDocument.PropertyDocument.DesnormalizedFields.
        /// </summary>
        public List<string>? DesnormalizedFields { get; set; } = new();
    }
}
