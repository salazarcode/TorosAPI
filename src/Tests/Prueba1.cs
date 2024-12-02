using Infrastructure.Hasher;
using Infrastructure.Repository.EF.Models;
using Microsoft.EntityFrameworkCore;
using Tests.Setup;

namespace Tests
{
    public class Prueba1 : TestSecurityDbContext
    {
        [Fact]
        public async Task CanCreateIdentifier()
        {
            using var context = GetContext();

            var identifier = new EfUser();
            identifier.Username = "asalazar";
            identifier.Email = "andresalteclado@hotmail.com";
            identifier.PasswordSalt = PasswordHasher.GenerateSalt();
            identifier.PasswordHash = PasswordHasher.GenerateHash(password: "123456", salt: identifier.PasswordSalt);
            identifier.CreatedAt = DateTime.UtcNow;
            identifier.CreatedBy = context.Set<EfUser>().FirstOrDefault(x => x.Username == "root")?.ID;
            identifier.PrimaryGroupID = context.Set<EFGroup>().FirstOrDefault(x => x.UniqueKey == "sudo")?.ID;
            identifier.IsActive = true;

            await context.Set<EfUser>().AddAsync(identifier);
            var result = await context.SaveChangesAsync();

            EfUser? createdUser = await context.Set<EfUser>().FirstOrDefaultAsync(x => x.Username == identifier.Username);

            Assert.NotNull(createdUser);
        }
    }
}
