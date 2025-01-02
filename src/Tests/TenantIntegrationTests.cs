using Infrastructure.Repository.MongoDB;
using Infrastructure.Repository.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TenantIntegrationTests
    {
        private readonly string _host = "mongodb://localhost:27017/";
        private readonly string _database = "toros_test";
        private readonly MongoDbContext _context;

        public TenantIntegrationTests()
        {
            _context = new MongoDbContext(_host, _database);
        }

        [Fact]
        public async Task CreateNewTenant_ShouldPersistToDatabase()
        {
            var tenant = new TenantDocument
            {
                Id = "SalvatoriAgencies",
                Name = "Salvatori Agencies"
            };

            await _context.Tenants.InsertOneAsync(tenant);

            var tenantFromDb = await _context.Tenants.Find(t => t.Id == "SalvatoriAgencies")
                                                    .FirstOrDefaultAsync();

            Assert.NotNull(tenantFromDb);
        }
    }
}
