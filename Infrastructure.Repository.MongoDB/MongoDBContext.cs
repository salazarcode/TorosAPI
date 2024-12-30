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

        public async Task CreateIndexesAsync()
        {
            var classIndexes = await Classes.Indexes.List().ToListAsync();
            if (!classIndexes.Any(idx => idx["name"].AsString == "properties.id_1"))
            {
                await Classes.Indexes.CreateOneAsync(
                    new CreateIndexModel<ClassDocument>(
                        Builders<ClassDocument>.IndexKeys.Ascending("properties.id"),
                        new CreateIndexOptions { Unique = true }
                    )
                );
            }
        }
    }
}
