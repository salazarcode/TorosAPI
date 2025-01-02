using Infrastructure.Repository.MongoDB.Models;
using MongoDB.Driver;

namespace Infrastructure.Repository.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<TenantDocument> Tenants => _database.GetCollection<TenantDocument>("tenants");
        public IMongoCollection<ClassDocument> Classes => _database.GetCollection<ClassDocument>("classes");
        public IMongoCollection<ObjectDocument> Objects => _database.GetCollection<ObjectDocument>("objects");
    }
}
