using MongoDB.Driver;
using Domain.Core.Entities;             // DomainTenant
using Domain.Core.Interfaces;           // ITenantRepository
using Infrastructure.Repository.MongoDB.Models; // TenantDocument

namespace Infrastructure.Repository.MongoDB.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<TenantDocument> _collection;

        public TenantRepository(MongoDbContext context)
        {
            _context = context;
            _collection = _context.Tenants;
        }

        public async Task<DomainTenant?> Create(DomainTenant entity)
        {
            // Mapeo de DomainTenant a TenantDocument
            var document = new TenantDocument
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            await _collection.InsertOneAsync(document);
            return entity;
        }

        public async Task<DomainTenant?> Get(string id)
        {
            var filter = Builders<TenantDocument>.Filter.Eq(x => x.Id, id);
            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return null;

            // Mapeo de TenantDocument a DomainTenant
            var domain = new DomainTenant
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description
            };

            return domain;
        }

        public async Task<IEnumerable<DomainTenant>> GetAll()
        {
            var documents = await _collection.Find(FilterDefinition<TenantDocument>.Empty).ToListAsync();

            return documents.Select(doc => new DomainTenant
            {
                Id = doc.Id,
                Name = doc.Name,
                Description = doc.Description
            });
        }

        public async Task<DomainTenant?> Update(DomainTenant entity)
        {
            var filter = Builders<TenantDocument>.Filter.Eq(x => x.Id, entity.Id);

            var update = Builders<TenantDocument>.Update
                .Set(x => x.Name, entity.Name)
                .Set(x => x.Description, entity.Description);

            var options = new FindOneAndUpdateOptions<TenantDocument>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updated = await _collection.FindOneAndUpdateAsync(filter, update, options);

            if (updated == null)
                return null;

            return new DomainTenant
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<TenantDocument>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> Delete(DomainTenant entity)
        {
            var filter = Builders<TenantDocument>.Filter.Eq(x => x.Id, entity.Id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
