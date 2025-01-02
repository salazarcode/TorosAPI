using Infrastructure.Randomizer.Custom;
using Infrastructure.Repository.MongoDB;
using Infrastructure.Repository.MongoDB.Models;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace Tests
{
    public class ClassIntegrationTests
    {
        private readonly string _host = "mongodb://localhost:27017/";
        private readonly string _database = "toros_test";
        private readonly MongoDbContext _context;

        public ClassIntegrationTests()
        {
            _context = new MongoDbContext(_host, _database);
        }

        [Fact]
        public async Task CreateNewClass_ShouldPersistToDatabase()
        {
            var n = RandomGenerator.GenerateRandomAlphanumeric(5);

            var mClass = new ClassDocument
            {
                Id = $"SalvatoriAgencies.Customer_{n}.v1",
                TenantId = "SalvatoriAgencies",
                BaseClassId = "Customer",
                ClassVersion = 1,
                Name = "Salvatori's Client",
                IsActive = true,
                ReleaseNotes = new List<string>
                {
                    "Initial version"
                },
                CreatedAt = DateTime.Now,
                Properties = new List<ClassDocument.PropertyDocument>
                {
                    new ClassDocument.PropertyDocument
                    {
                        Id = $"SalvatoriAgencies.Customer_{n}.v1.FirstName",
                        TypeClass = "String",
                        IsReference = false,
                        Min = 1,
                        Max = 1
                    },
                    new ClassDocument.PropertyDocument
                    {
                        Id = $"SalvatoriAgencies.Customer_{n}.v1.Email",
                        TypeClass = "String",
                        IsReference = false,
                        Min = 1,
                        Max = 1
                    }
                }
            };

            await _context.Classes.InsertOneAsync(mClass);

            var classFromDb = await _context.Classes.Find(c => c.Id == $"SalvatoriAgencies.Customer_{n}.v1")
                                                    .FirstOrDefaultAsync();

            Assert.NotNull(classFromDb);
        }
    }
}
