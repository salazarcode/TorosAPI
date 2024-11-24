using Domain.Interfaces.Abstract;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class IdentifierRepository : IRepository<EfIdentifier>
    {
        private readonly DatabaseContextFactory _contextFactory;
        private readonly DatabaseContext _context;
        private readonly DbSet<EfIdentifier> _dbSet;
        private readonly int _maxConcurrency = 10;

        public IdentifierRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateContext();
            _dbSet = _context.Set<EfIdentifier>();
        }

        public async Task<EfIdentifier?> Create(EfIdentifier domainType)
        {
            ValidateEntity(domainType);

            domainType.CreatedAt = DateTime.UtcNow;
            domainType.IsActive = true;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _dbSet.AddAsync(domainType);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return entity.Entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EfIdentifier?> Update(EfIdentifier domainType)
        {
            ValidateEntity(domainType);

            using var context = _contextFactory.CreateContext();
            var existingEntity = await context.Identifiers.FindAsync(domainType.ID);

            if (existingEntity == null || !existingEntity.IsActive)
                return null;

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Desactivar el registro anterior
                await DeactivateEntity(context, existingEntity);

                // Crear un nuevo registro
                var newEntity = await CreateNewEntity(context, domainType);

                // Actualizar relaciones
                await UpdateRelatedEntities(context, existingEntity.ID, newEntity.ID);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return newEntity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(int ID)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _dbSet.FindAsync(ID);
                if (entity == null || !entity.IsActive)
                {
                    return false; // No se realizó la operación
                }

                // Desactivar el registro
                await DeactivateEntity(_context, entity);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true; // Operación exitosa
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> Delete(EfIdentifier domainType)
        {
            if (domainType == null)
                return false;

            return await Delete(domainType.ID);
        }

        public async Task<EfIdentifier?> Get()
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.IsActive);
        }

        public async Task<EfIdentifier?> Get(int ID)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.ID == ID && e.IsActive);
        }

        public async Task<EfIdentifier?> CreateBatch(IEnumerable<EfIdentifier> domainTypes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var domainType in domainTypes)
                {
                    ValidateEntity(domainType);
                    domainType.CreatedAt = DateTime.UtcNow;
                    domainType.IsActive = true;
                }

                await _dbSet.AddRangeAsync(domainTypes);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return domainTypes.FirstOrDefault();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EfIdentifier?> UpdateBatch(IEnumerable<EfIdentifier> domainTypes)
        {
            var semaphore = new SemaphoreSlim(_maxConcurrency);
            var tasks = new List<Task>();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var domainType in domainTypes)
                {
                    ValidateEntity(domainType);

                    await semaphore.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            using var context = _contextFactory.CreateContext();
                            var existingEntity = await context.Identifiers.FindAsync(domainType.ID);

                            if (existingEntity != null && existingEntity.IsActive)
                            {
                                // Desactivar el registro anterior
                                await DeactivateEntity(context, existingEntity);

                                // Crear un nuevo registro
                                var newEntity = await CreateNewEntity(context, domainType);

                                // Actualizar relaciones
                                await UpdateRelatedEntities(context, existingEntity.ID, newEntity.ID);

                                await context.SaveChangesAsync();
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }

                await Task.WhenAll(tasks);
                await transaction.CommitAsync();
                return domainTypes.FirstOrDefault();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EfIdentifier?> DeleteBatch(IEnumerable<EfIdentifier?> entities)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var entity in entities)
                {
                    if (entity != null)
                    {
                        await Delete(entity.ID);
                    }
                }

                await transaction.CommitAsync();
                return entities.FirstOrDefault();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task DeactivateEntity(DatabaseContext context, EfIdentifier entity)
        {
            entity.IsActive = false;
            entity.CreatedAt = DateTime.UtcNow;
            context.Identifiers.Update(entity);
        }

        private async Task<EfIdentifier> CreateNewEntity(DatabaseContext context, EfIdentifier domainType)
        {
            domainType.ID = 0; // Asegura que se cree un nuevo registro
            domainType.CreatedAt = DateTime.UtcNow;
            domainType.IsActive = true;

            var newEntity = await context.Identifiers.AddAsync(domainType);
            return newEntity.Entity;
        }

        private async Task UpdateRelatedEntities(DatabaseContext context, int oldIdentifierId, int newIdentifierId)
        {
            // Actualizar relaciones en la tabla IdentifierGroup
            var relatedGroups = await context.IdentifierGroups
                .Where(ig => ig.IdentifierID == oldIdentifierId)
                .ToListAsync();

            foreach (var group in relatedGroups)
            {
                group.IdentifierID = newIdentifierId;
            }
        }

        private void ValidateEntity(EfIdentifier domainType)
        {
            if (string.IsNullOrWhiteSpace(domainType.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(domainType.Email))
                throw new ArgumentException("Email is required.");
        }
    }
}
