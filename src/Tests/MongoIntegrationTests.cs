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
    public class MongoIntegrationTests
    {
        private readonly string _host = "mongodb://localhost:27017/";
        private readonly string _database = "toros_test";
        private readonly MongoDbContext _context;

        public MongoIntegrationTests()
        {
            _context = new MongoDbContext(_host, _database);
            _context.CreateIndexesAsync().GetAwaiter();
        }

        //[Fact]
        //public async Task CreateNewTenant_ShouldPersistToDatabase()
        //{
        //    var tenant = new TenantDocument
        //    {
        //        Id = "SalvatoriAgencies",
        //        Name = "Salvatori Agencies"
        //    };

        //    await _context.Tenants.InsertOneAsync(tenant);

        //    var tenantFromDb = await _context.Tenants.Find(t => t.Id == "SalvatoriAgencies")
        //                                            .FirstOrDefaultAsync();

        //    Assert.NotNull(tenantFromDb);
        //}

        //[Fact]
        //public async Task CreateNewClass_ShouldPersistToDatabase()
        //{
        //    var mClass = new ClassDocument
        //    {
        //        Id = "Customer",
        //        TenantId = "SalvatoriAgencies",
        //        Name = "SalvatoriAgencies Customer",
        //        Properties = new List<ClassDocument.MProperty>
        //        {
        //            new ClassDocument.MProperty
        //            {
        //                Key = "name",
        //                Min = 1,
        //                Max = 1
        //            },
        //            new ClassDocument.MProperty
        //            {
        //                Key = "email",
        //                Min = 1,
        //                Max = 1
        //            }
        //        },
        //        IsShared = false,
        //        IsActive = true
        //    };

        //    await _context.Classes.InsertOneAsync(mClass);

        //    var classFromDb = await _context.Classes.Find(c => c.Id == "1")
        //                                            .FirstOrDefaultAsync();

        //    Assert.NotNull(classFromDb);
        //}
    }
}
