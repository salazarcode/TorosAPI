using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Repository;

namespace Tests
{
    public class Prueba1 : TestSecurityDbContext
    {
        [Fact]
        public async Task CanCreateIdentifier()
        {
            using var context = GetContext();

            var identifier = new EfIdentifier();
            identifier.Username = "asalazar";
            identifier.Email = "andresalteclado@hotmail.com";
            identifier.PasswordSalt = PasswordHasher.GenerateSalt();
            identifier.PasswordHash = PasswordHasher.GenerateHash(password: "123456", salt: identifier.PasswordSalt);
            identifier.CreatedAt = DateTime.UtcNow;
            identifier.CreatedBy = context.Set<EfIdentifier>().FirstOrDefault(x => x.Username == "root")?.ID;
            identifier.PrimaryGroupID = context.Set<EFGroup>().FirstOrDefault(x => x.UniqueKey == "sudo")?.ID;
            identifier.IsActive = true;

            await context.Set<EfIdentifier>().AddAsync(identifier);
            var result = await context.SaveChangesAsync();

            EfIdentifier? createdUser = await context.Set<EfIdentifier>().FirstOrDefaultAsync(x => x.Username == identifier.Username);

            Assert.NotNull(createdUser);
        }
    }
}
