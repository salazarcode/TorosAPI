
using MongoDB.Driver;
using Domain.Core.Entities;             // DomainClass, DomainClassProperty
using Domain.Core.Interfaces;           // IClassRepository
using Infrastructure.Repository.MongoDB.Models;
using Domain.Core.ValueObjects; // ClassDocument

namespace Infrastructure.Repository.MongoDB.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<ClassDocument> _collection;

        public ClassRepository(MongoDbContext context)
        {
            _context = context;
            _collection = _context.Classes;
        }

        public async Task<DomainClass?> Create(DomainClass entity)
        {
            var document = new ClassDocument
            {
                Id = entity.Id,
                TenantId = entity.TenantId,
                Name = entity.Name,
                BaseClassId = entity.BaseClassId,
                ClassVersion = entity.ClassVersion,
                IsActive = entity.IsActive,
                ReleaseNotes = entity.ReleaseNotes?.ToList() ?? new List<string>(),
                Properties = entity.Properties.Select(p => new ClassDocument.PropertyDocument
                {
                    Id = p.Id,
                    TypeClass = p.TypeClass,
                    IsReference = p.IsReference,
                    Min = p.Min,
                    Max = p.Max,
                    DesnormalizedFields = p.DesnormalizedFields?.ToList() ?? new List<string>()
                }).ToList(),
                CreatedAt = entity.CreatedAt
            };

            await _collection.InsertOneAsync(document);
            return entity;
        }

        public async Task<DomainClass?> Get(string id)
        {
            var filter = Builders<ClassDocument>.Filter.Eq(x => x.Id, id);
            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return null;

            var domain = new DomainClass
            {
                Id = document.Id,
                TenantId = document.TenantId,
                Name = document.Name,
                BaseClassId = document.BaseClassId,
                ClassVersion = document.ClassVersion,
                IsActive = document.IsActive,
                ReleaseNotes = document.ReleaseNotes,
                Properties = document.Properties.Select(p => new DomainClassProperty
                {
                    Id = p.Id,
                    TypeClass = p.TypeClass,
                    IsReference = p.IsReference,
                    Min = p.Min,
                    Max = p.Max,
                    DesnormalizedFields = p.DesnormalizedFields
                }).ToList(),
                CreatedAt = document.CreatedAt
            };

            return domain;
        }

        public async Task<IEnumerable<DomainClass>> GetAll()
        {
            var documents = await _collection.Find(FilterDefinition<ClassDocument>.Empty).ToListAsync();

            return documents.Select(doc => new DomainClass
            {
                Id = doc.Id,
                TenantId = doc.TenantId,
                Name = doc.Name,
                BaseClassId = doc.BaseClassId,
                ClassVersion = doc.ClassVersion,
                IsActive = doc.IsActive,
                ReleaseNotes = doc.ReleaseNotes,
                Properties = doc.Properties.Select(p => new DomainClassProperty
                {
                    Id = p.Id,
                    TypeClass = p.TypeClass,
                    IsReference = p.IsReference,
                    Min = p.Min,
                    Max = p.Max,
                    DesnormalizedFields = p.DesnormalizedFields
                }).ToList(),
                CreatedAt = doc.CreatedAt
            });
        }

        public async Task<DomainClass?> Update(DomainClass entity)
        {
            var filter = Builders<ClassDocument>.Filter.Eq(x => x.Id, entity.Id);

            var update = Builders<ClassDocument>.Update
                .Set(x => x.TenantId, entity.TenantId)
                .Set(x => x.Name, entity.Name)
                .Set(x => x.BaseClassId, entity.BaseClassId)
                .Set(x => x.ClassVersion, entity.ClassVersion)
                .Set(x => x.IsActive, entity.IsActive)
                .Set(x => x.ReleaseNotes, entity.ReleaseNotes)
                .Set(x => x.Properties, entity.Properties.Select(p => new ClassDocument.PropertyDocument
                {
                    Id = p.Id,
                    TypeClass = p.TypeClass,
                    IsReference = p.IsReference,
                    Min = p.Min,
                    Max = p.Max,
                    DesnormalizedFields = p.DesnormalizedFields?.ToList() ?? new List<string>()
                }).ToList());

            var options = new FindOneAndUpdateOptions<ClassDocument>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updated = await _collection.FindOneAndUpdateAsync(filter, update, options);
            if (updated == null)
                return null;

            return new DomainClass
            {
                Id = updated.Id,
                TenantId = updated.TenantId,
                Name = updated.Name,
                BaseClassId = updated.BaseClassId,
                ClassVersion = updated.ClassVersion,
                IsActive = updated.IsActive,
                ReleaseNotes = updated.ReleaseNotes,
                Properties = updated.Properties.Select(p => new DomainClassProperty
                {
                    Id = p.Id,
                    TypeClass = p.TypeClass,
                    IsReference = p.IsReference,
                    Min = p.Min,
                    Max = p.Max,
                    DesnormalizedFields = p.DesnormalizedFields
                }).ToList(),
                CreatedAt = updated.CreatedAt
            };
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<ClassDocument>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> Delete(DomainClass entity)
        {
            var filter = Builders<ClassDocument>.Filter.Eq(x => x.Id, entity.Id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
