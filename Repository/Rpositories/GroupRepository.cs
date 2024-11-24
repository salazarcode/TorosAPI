using Domain.Interfaces.Abstract;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using Repository.Models;
using System.Collections.Concurrent;

namespace Repository.Repositories
{
    public class GroupRepository : IRepository<EFGroup>
    {
        private readonly DatabaseContextFactory _contextFactory;
        private readonly DatabaseContext _context;
        private readonly DbSet<EFGroup> _dbSet;
        private readonly int _maxConcurrency = 10;

        public GroupRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateContext();
            _dbSet = _context.Set<EFGroup>();
        }

        public async Task<EFGroup?> Create(EFGroup domainType)
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

        public async Task<EFGroup?> Update(EFGroup domainType)
        {
            ValidateEntity(domainType);

            using var context = _contextFactory.CreateContext();
            var existingEntity = await context.Groups.FindAsync(domainType.ID);

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

        public async Task<bool> Delete(EFGroup domainType)
        {
            if (domainType == null)
                return false;

            return await Delete(domainType.ID);
        }

        public async Task<EFGroup?> Get()
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.IsActive);
        }

        public async Task<EFGroup?> Get(int ID)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.ID == ID && e.IsActive);
        }

        public async Task<EFGroup?> CreateBatch(IEnumerable<EFGroup> domainTypes)
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

        public async Task<(IEnumerable<EFGroup?> successes, IEnumerable<(EFGroup failedEntity, Exception error)> failures)> UpdateBatch(IEnumerable<EFGroup> entities)
        {
            var semaphore = new SemaphoreSlim(_maxConcurrency);
            var tasks = new List<Task>();
            var successes = new ConcurrentBag<EFGroup>();
            var failures = new ConcurrentBag<(EFGroup FailedGroup, Exception Error)>();

            foreach (var domainType in entities)
            {
                ValidateEntity(domainType);

                await semaphore.WaitAsync(); // Controla la concurrencia
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using var context = _contextFactory.CreateContext();
                        using var transaction = await context.Database.BeginTransactionAsync();

                        try
                        {
                            var existingEntity = await context.Groups.FindAsync(domainType.ID);
                            if (existingEntity != null && existingEntity.IsActive)
                            {
                                // Desactivar el registro existente
                                await DeactivateEntity(context, existingEntity);

                                // Crear un nuevo registro
                                var newEntity = await CreateNewEntity(context, domainType);

                                // Actualizar relaciones
                                await UpdateRelatedEntities(context, existingEntity.ID, newEntity.ID);

                                await context.SaveChangesAsync();
                                await transaction.CommitAsync();

                                successes.Add(newEntity); // Registrar éxito
                            }
                            else
                            {
                                throw new InvalidOperationException($"Group with ID {domainType.ID} is not active or does not exist.");
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            failures.Add((domainType, ex)); // Registrar fallo
                        }
                    }
                    catch (Exception ex)
                    {
                        failures.Add((domainType, ex)); // Registrar fallo por errores externos
                    }
                    finally
                    {
                        semaphore.Release(); // Liberar el lugar en el semáforo
                    }
                }));
            }

            await Task.WhenAll(tasks); // Espera a que todas las tareas terminen
            return (successes, failures); // Devuelve los éxitos y los fallos
        }



        public async Task<EFGroup?> DeleteBatch(IEnumerable<EFGroup?> entities)
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

        private async Task DeactivateEntity(DatabaseContext context, EFGroup entity)
        {
            entity.IsActive = false;
            entity.CreatedAt = DateTime.UtcNow;
            context.Groups.Update(entity);
        }

        private async Task<EFGroup> CreateNewEntity(DatabaseContext context, EFGroup domainType)
        {
            domainType.ID = 0; // Asegura que se cree un nuevo registro
            domainType.CreatedAt = DateTime.UtcNow;
            domainType.IsActive = true;

            var newEntity = await context.Groups.AddAsync(domainType);
            return newEntity.Entity;
        }

        private async Task UpdateRelatedEntities(DatabaseContext context, int oldGroupId, int newGroupId)
        {
            // Actualizar relaciones en la tabla IdentifierGroup
            var relatedIdentifiers = await context.IdentifierGroups
                .Where(ig => ig.GroupID == oldGroupId)
                .ToListAsync();

            foreach (var identifierGroup in relatedIdentifiers)
            {
                identifierGroup.GroupID = newGroupId;
            }
        }

        private void ValidateEntity(EFGroup domainType)
        {
            if (string.IsNullOrWhiteSpace(domainType.UniqueKey))
                throw new ArgumentException("UniqueKey is required.");
        }
    }
}
