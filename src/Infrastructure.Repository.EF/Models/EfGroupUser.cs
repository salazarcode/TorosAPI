using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class EfGroupUser
    {
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual EfUser User { get; set; } = null!;
        public virtual EFGroup Group { get; set; } = null!;
        public virtual EfUser Creator { get; set; } = null!;
    }
}
