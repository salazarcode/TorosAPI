using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Group
    {
        public int ID { get; set; }
        public string UniqueKey { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Identifier? Creator { get; set; }
        public virtual ICollection<IdentifierGroup> IdentifierGroups { get; set; } = new List<IdentifierGroup>();
        public virtual ICollection<Identifier> PrimaryGroupIdentifiers { get; set; } = new List<Identifier>();
    }
}
