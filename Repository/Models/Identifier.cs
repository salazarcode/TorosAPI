using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Identifier
    {
        public int ID { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public int? PrimaryGroupID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Group? PrimaryGroup { get; set; }
        public virtual Identifier? Creator { get; set; }
        public virtual ICollection<IdentifierGroup> IdentifierGroups { get; set; } = new List<IdentifierGroup>();
        public virtual ICollection<Identifier> CreatedIdentifiers { get; set; } = new List<Identifier>();
        public virtual ICollection<Group> CreatedGroups { get; set; } = new List<Group>();
    }
}
