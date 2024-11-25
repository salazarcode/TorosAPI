using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Group
    {
        public int ID { get; set; }
        public string UniqueKey { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Identifier? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        //Usuarios que tienen este grupo en IdentifierGroup o que su grupo primario es este.
        public virtual IEnumerable<Identifier>? Identifiers { get; set; }
    }
}
