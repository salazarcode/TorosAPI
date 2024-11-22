﻿using Infrastructure;
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

            var identifier = new Identifier();
            identifier.Email = "andresalteclado@hotmail.com";
            identifier.Username = "asalazar";
            identifier.PasswordSalt = PasswordHasher.GenerateSalt();
            identifier.PasswordHash = PasswordHasher.GenerateHash(password: "123456", salt: identifier.PasswordSalt);
            identifier.CreatedAt = DateTime.UtcNow;
            identifier.CreatedBy = context.Set<Identifier>().FirstOrDefault(x => x.Username == "root")?.ID;
            identifier.IsActive = true;

            var result = await context.SaveChangesAsync();

            Identifier? createdUser = await context.Set<Identifier>().FirstOrDefaultAsync(x => x.Username == identifier.Username);

            Assert.NotNull(createdUser);
        }
    }
}
