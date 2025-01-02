using Domain.Core.ValueObjects;

namespace Domain.Core.Entities
{
    /// <summary>
    /// Representa la definición de una Clase (con su versión) a nivel de dominio.
    /// </summary>
    public class DomainClass
    {
        /// <summary>
        /// Identificador único, por ejemplo: "MiEmpresa.Producto.v1".
        /// Equivale a ClassDocument.Id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Id del tenant al que corresponde esta clase. Ej.: "MiEmpresa".
        /// Equivale a ClassDocument.TenantId.
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Display name de la clase, p.ej.: "Producto".
        /// Equivale a ClassDocument.Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Id base de la clase (sin versión). Ej.: "MiEmpresa.Producto".
        /// Equivale a ClassDocument.BaseClassId.
        /// </summary>
        public string? BaseClassId { get; set; }

        /// <summary>
        /// Versión numérica (1, 2, 3...).
        /// Equivale a ClassDocument.ClassVersion.
        /// </summary>
        public int ClassVersion { get; set; }

        /// <summary>
        /// Indica si esta versión está activa para crear nuevos objetos.
        /// Equivale a ClassDocument.IsActive.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Notas que describen los cambios o el contenido de esta versión.
        /// Equivale a ClassDocument.ReleaseNotes.
        /// </summary>
        public List<string>? ReleaseNotes { get; set; } = new();

        /// <summary>
        /// Lista de propiedades definidas para esta versión.
        /// Equivale a ClassDocument.Properties -> DomainClassProperty.
        /// </summary>
        public List<DomainClassProperty> Properties { get; set; } = new();

        /// <summary>
        /// Fecha de creación de esta versión.
        /// Equivale a ClassDocument.CreatedAt.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
