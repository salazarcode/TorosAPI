using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class EFIdentifierGroup
    {
        public int IdentifierID { get; set; }
        public int GroupID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual EfIdentifier Identifier { get; set; } = null!;
        public virtual EFGroup Group { get; set; } = null!;
        public virtual EfIdentifier Creator { get; set; } = null!;
    }
}
