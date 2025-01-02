
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Domain.Core.Entities;             // DomainObject, DomainObjectValue, DomainReferenceValue...
using Domain.Core.Interfaces;           // IObjectRepository
using Infrastructure.Repository.MongoDB.Models;
using Domain.Core.ValueObjects; // ObjectDocument

namespace Infrastructure.Repository.MongoDB.Repositories
{
    public class ObjectRepository : IObjectRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<ObjectDocument> _collection;

        public ObjectRepository(MongoDbContext context)
        {
            _context = context;
            _collection = _context.Objects;
        }

        public async Task<DomainObject?> Create(DomainObject entity)
        {
            var document = new ObjectDocument
            {
                Id = entity.Id,
                ClassId = entity.ClassId,
                Values = entity.Values.Select(v =>
                {
                    if (v.IsReference && v.Value is DomainReferenceValue refValue)
                    {
                        return new ObjectDocument.ValueDocument
                        {
                            PropertyId = v.PropertyId,
                            Value = new ObjectDocument.ValueDocument.ReferenceValueDocument
                            {
                                ReferenceId = refValue.ReferenceId,
                                Desnormalized = new ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument
                                {
                                    LastUpdateDate = refValue.Desnormalized?.LastUpdateDate ?? default,
                                    Fields = refValue.Desnormalized?.Fields.Select(f =>
                                        new ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument.DesnormalizedItemDocument
                                        {
                                            Key = f.Key,
                                            Value = f.Value
                                        }).ToList() ?? new List<ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument.DesnormalizedItemDocument>()
                                }
                            }
                        };
                    }
                    else
                    {
                        // Valor primitivo
                        return new ObjectDocument.ValueDocument
                        {
                            PropertyId = v.PropertyId,
                            Value = v.Value
                        };
                    }
                }).ToList()
            };

            await _collection.InsertOneAsync(document);
            return entity;
        }

        public async Task<DomainObject?> Get(string id)
        {
            var filter = Builders<ObjectDocument>.Filter.Eq(x => x.Id, id);
            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return null;

            // Mapeo ObjectDocument -> DomainObject
            var domainObject = new DomainObject
            {
                Id = document.Id,
                ClassId = document.ClassId,
                Values = document.Values.Select(docVal =>
                {
                    // Si docVal.Value es un BsonDocument, podría ser la referencia
                    if (docVal.Value is BsonDocument bsonRef)
                    {
                        var refValue = BsonSerializer.Deserialize<ObjectDocument.ValueDocument.ReferenceValueDocument>(bsonRef);
                        return new DomainObjectValue
                        {
                            PropertyId = docVal.PropertyId,
                            IsReference = true,
                            Value = new DomainReferenceValue
                            {
                                ReferenceId = refValue.ReferenceId,
                                Desnormalized = new DomainDesnormalizedData
                                {
                                    LastUpdateDate = refValue.Desnormalized?.LastUpdateDate ?? default,
                                    Fields = refValue.Desnormalized?.Fields.Select(f => new DomainDesnormalizedItem
                                    {
                                        Key = f.Key,
                                        Value = f.Value
                                    }).ToList() ?? new List<DomainDesnormalizedItem>()
                                }
                            }
                        };
                    }
                    else
                    {
                        // Valor primitivo
                        return new DomainObjectValue
                        {
                            PropertyId = docVal.PropertyId,
                            IsReference = false,
                            Value = docVal.Value
                        };
                    }
                }).ToList()
            };

            return domainObject;
        }

        public async Task<IEnumerable<DomainObject>> GetAll()
        {
            var documents = await _collection.Find(FilterDefinition<ObjectDocument>.Empty).ToListAsync();

            return documents.Select(doc =>
            {
                var domainObject = new DomainObject
                {
                    Id = doc.Id,
                    ClassId = doc.ClassId,
                    Values = doc.Values.Select(docVal =>
                    {
                        if (docVal.Value is BsonDocument bsonRef)
                        {
                            var refValue = BsonSerializer.Deserialize<ObjectDocument.ValueDocument.ReferenceValueDocument>(bsonRef);
                            return new DomainObjectValue
                            {
                                PropertyId = docVal.PropertyId,
                                IsReference = true,
                                Value = new DomainReferenceValue
                                {
                                    ReferenceId = refValue.ReferenceId,
                                    Desnormalized = new DomainDesnormalizedData
                                    {
                                        LastUpdateDate = refValue.Desnormalized?.LastUpdateDate ?? default,
                                        Fields = refValue.Desnormalized?.Fields.Select(f => new DomainDesnormalizedItem
                                        {
                                            Key = f.Key,
                                            Value = f.Value
                                        }).ToList() ?? new List<DomainDesnormalizedItem>()
                                    }
                                }
                            };
                        }
                        else
                        {
                            return new DomainObjectValue
                            {
                                PropertyId = docVal.PropertyId,
                                IsReference = false,
                                Value = docVal.Value
                            };
                        }
                    }).ToList()
                };

                return domainObject;
            });
        }

        public async Task<DomainObject?> Update(DomainObject entity)
        {
            var filter = Builders<ObjectDocument>.Filter.Eq(x => x.Id, entity.Id);

            // Se reconstruye el documento (ReplaceOne) o se hace un update parcial, según prefieras
            var document = new ObjectDocument
            {
                Id = entity.Id,
                ClassId = entity.ClassId,
                Values = entity.Values.Select(v =>
                {
                    if (v.IsReference && v.Value is DomainReferenceValue refValue)
                    {
                        return new ObjectDocument.ValueDocument
                        {
                            PropertyId = v.PropertyId,
                            Value = new ObjectDocument.ValueDocument.ReferenceValueDocument
                            {
                                ReferenceId = refValue.ReferenceId,
                                Desnormalized = new ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument
                                {
                                    LastUpdateDate = refValue.Desnormalized?.LastUpdateDate ?? default,
                                    Fields = refValue.Desnormalized?.Fields.Select(f =>
                                        new ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument.DesnormalizedItemDocument
                                        {
                                            Key = f.Key,
                                            Value = f.Value
                                        }).ToList() ?? new List<ObjectDocument.ValueDocument.ReferenceValueDocument.DesnormalizedDataDocument.DesnormalizedItemDocument>()
                                }
                            }
                        };
                    }
                    else
                    {
                        return new ObjectDocument.ValueDocument
                        {
                            PropertyId = v.PropertyId,
                            Value = v.Value
                        };
                    }
                }).ToList()
            };

            // Usamos ReplaceOne para sustituir el documento por completo
            var result = await _collection.ReplaceOneAsync(filter, document);
            if (result.ModifiedCount > 0)
            {
                return entity;
            }
            return null;
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<ObjectDocument>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public async Task<bool> Delete(DomainObject entity)
        {
            var filter = Builders<ObjectDocument>.Filter.Eq(x => x.Id, entity.Id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
