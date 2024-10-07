using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    [Table("Classes", Schema = "abstract")]
    public class XClass
    {
        public int ID { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public bool IsPrimitive { get; set; } = false;
        public virtual ICollection<XProperty>? XProperties { get; set; }
        public virtual ICollection<XProperty>? XOthersProperties { get; set; }
        public virtual ICollection<XAncestry>? XAncestries { get; set; }
    }
}
