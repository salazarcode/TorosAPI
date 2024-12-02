namespace Repository.Models
{
    public class EFGroup
    {
        public int ID { get; set; }
        public Guid PublicId { get; set; }
        public string UniqueKey { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual EfUser? Creator { get; set; }
        public virtual ICollection<EfUser> PrimaryGroupUsers { get; set; } = new List<EfUser>();
        public virtual ICollection<EfGroupUser> GroupUsers { get; set; } = new List<EfGroupUser>();
    }
}
