using Domain.Core.Entities;

namespace Domain.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public User? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        //El grupo identificado por su campos PrimaryGroupID
        public Group? PrimaryGroup { get; set; }
        //Los grupos a los que pertenerce a través de la tabla intermedia IdentifierGroup
        public IEnumerable<Group>? OtherGroups{ get; set; }
    }
}
