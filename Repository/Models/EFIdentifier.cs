using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class EfIdentifier
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
        public virtual EFGroup? PrimaryGroup { get; set; }
        public virtual EfIdentifier? Creator { get; set; }
        public virtual ICollection<EFIdentifierGroup> IdentifierGroups { get; set; } = new List<EFIdentifierGroup>();
        public virtual ICollection<EfIdentifier> CreatedIdentifiers { get; set; } = new List<EfIdentifier>();
        public virtual ICollection<EFGroup> CreatedGroups { get; set; } = new List<EFGroup>();
    }
}
