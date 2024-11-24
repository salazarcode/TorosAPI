namespace Repository.Models
{
    public class EFGroup
    {
        public int ID { get; set; }
        public string UniqueKey { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual EfIdentifier? Creator { get; set; }
        public virtual ICollection<EFIdentifierGroup> IdentifierGroups { get; set; } = new List<EFIdentifierGroup>();
        public virtual ICollection<EfIdentifier> PrimaryGroupIdentifiers { get; set; } = new List<EfIdentifier>();
    }
}
