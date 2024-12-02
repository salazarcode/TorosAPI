using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Repository.EF.Models
{
    public class EfUser
    {
        public int ID { get; set; }
        public Guid PublicId { get; set; }
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
        public virtual EfUser? Creator { get; set; }
        public virtual ICollection<EfGroupUser> GroupUsers { get; set; } = new List<EfGroupUser>();

    }
}
