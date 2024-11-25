//using AutoMapper;
//using Domain.Interfaces;
//using Domain.Entities; // Asegúrate de que este espacio de nombres sea correcto
//using Microsoft.EntityFrameworkCore;
//using Repository.Contexts;
//using Repository.Models;
//using System.Collections.Concurrent;

//namespace Repository.Repositories
//{
//    public class GroupRepository : IGroupRepository
//    {
//        private readonly DatabaseContextFactory _contextFactory;
//        private readonly IMapper _mapper;
//        private readonly int _maxConcurrency;

//        public GroupRepository(DatabaseContextFactory contextFactory, IMapper mapper, int maxConcurrency = 10)
//        {
//            _contextFactory = contextFactory;
//            _mapper = mapper;
//            _maxConcurrency = maxConcurrency;
//        }

//        public async Task<Group?> Create(Group domainGroup)
//        {
//            ValidateEntity(domainGroup);

//            var efGroup = _mapper.Map<EFGroup>(domainGroup);
//            efGroup.CreatedAt = DateTime.UtcNow;
//            efGroup.IsActive = true;

//            using var context = _contextFactory.CreateContext();
//            using var transaction = await context.Database.BeginTransactionAsync();

//            try
//            {
//                var entity = await context.Groups.AddAsync(efGroup);
//                await context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                return _mapper.Map<Group>(entity.Entity);
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<(IEnumerable<Group> Successes, IEnumerable<(Group FailedGroup, Exception Error)> Failures)> CreateBatch(IEnumerable<Group> domainGroups)
//        {
//            var semaphore = new SemaphoreSlim(_maxConcurrency);
//            var tasks = new List<Task>();
//            var successes = new ConcurrentBag<Group>();
//            var failures = new ConcurrentBag<(Group FailedGroup, Exception Error)>();

//            foreach (var domainGroup in domainGroups)
//            {
//                try
//                {
//                    ValidateEntity(domainGroup);
//                }
//                catch (Exception ex)
//                {
//                    failures.Add((domainGroup, ex));
//                    continue; // Salta la creación de este grupo
//                }

//                var efGroup = _mapper.Map<EFGroup>(domainGroup);
//                efGroup.CreatedAt = DateTime.UtcNow;
//                efGroup.IsActive = true;

//                await semaphore.WaitAsync();
//                tasks.Add(Task.Run(async () =>
//                {
//                    try
//                    {
//                        using var context = _contextFactory.CreateContext();
//                        using var transaction = await context.Database.BeginTransactionAsync();

//                        try
//                        {
//                            var entity = await context.Groups.AddAsync(efGroup);
//                            await context.SaveChangesAsync();
//                            await transaction.CommitAsync();

//                            var createdGroup = _mapper.Map<Group>(entity.Entity);
//                            successes.Add(createdGroup);
//                        }
//                        catch (Exception ex)
//                        {
//                            await transaction.RollbackAsync();
//                            var failedGroup = _mapper.Map<Group>(efGroup);
//                            failures.Add((failedGroup, ex));
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        var failedGroup = _mapper.Map<Group>(efGroup);
//                        failures.Add((failedGroup, ex));
//                    }
//                    finally
//                    {
//                        semaphore.Release();
//                    }
//                }));
//            }

//            await Task.WhenAll(tasks);
//            return (successes, failures);
//        }

//        public async Task<Group?> Get()
//        {
//            using var context = _contextFactory.CreateContext();
//            var efGroup = await context.Groups.FirstOrDefaultAsync(e => e.IsActive);
//            return efGroup != null ? _mapper.Map<Group>(efGroup) : null;
//        }

//        public async Task<Group?> Get(int ID)
//        {
//            using var context = _contextFactory.CreateContext();
//            var efGroup = await context.Groups.FirstOrDefaultAsync(e => e.ID == ID && e.IsActive);
//            return efGroup != null ? _mapper.Map<Group>(efGroup) : null;
//        }

//        public async Task<Group?> Update(Group domainGroup)
//        {
//            ValidateEntity(domainGroup);

//            using var context = _contextFactory.CreateContext();
//            var existingEfGroup = await context.Groups.FindAsync(domainGroup.ID);

//            if (existingEfGroup == null || !existingEfGroup.IsActive)
//                return null;

//            using var transaction = await context.Database.BeginTransactionAsync();
//            try
//            {
//                // Desactivar el grupo existente
//                existingEfGroup.IsActive = false;
//                existingEfGroup.CreatedAt = DateTime.UtcNow;
//                context.Groups.Update(existingEfGroup);

//                // Crear un nuevo grupo con los datos actualizados
//                var newEfGroup = _mapper.Map<EFGroup>(domainGroup);
//                newEfGroup.ID = 0; // Asegura que se cree un nuevo registro
//                newEfGroup.CreatedAt = DateTime.UtcNow;
//                newEfGroup.IsActive = true;

//                var addedGroup = await context.Groups.AddAsync(newEfGroup);
//                await context.SaveChangesAsync();

//                // Actualizar relaciones si es necesario
//                await UpdateRelatedEntities(context, existingEfGroup.ID, addedGroup.Entity.ID);

//                await transaction.CommitAsync();

//                return _mapper.Map<Group>(addedGroup.Entity);
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<(IEnumerable<Group> Successes, IEnumerable<(Group FailedGroup, Exception Error)> Failures)> UpdateBatch(IEnumerable<Group> domainGroups)
//        {
//            var semaphore = new SemaphoreSlim(_maxConcurrency);
//            var tasks = new List<Task>();
//            var successes = new ConcurrentBag<Group>();
//            var failures = new ConcurrentBag<(Group FailedGroup, Exception Error)>();

//            foreach (var domainGroup in domainGroups)
//            {
//                try
//                {
//                    ValidateEntity(domainGroup);
//                }
//                catch (Exception ex)
//                {
//                    failures.Add((domainGroup, ex));
//                    continue; // Salta la actualización de este grupo
//                }

//                await semaphore.WaitAsync();
//                tasks.Add(Task.Run(async () =>
//                {
//                    try
//                    {
//                        using var context = _contextFactory.CreateContext();
//                        using var transaction = await context.Database.BeginTransactionAsync();

//                        try
//                        {
//                            var existingEfGroup = await context.Groups.FindAsync(domainGroup.ID);
//                            if (existingEfGroup != null && existingEfGroup.IsActive)
//                            {
//                                // Desactivar el grupo existente
//                                existingEfGroup.IsActive = false;
//                                existingEfGroup.CreatedAt = DateTime.UtcNow;
//                                context.Groups.Update(existingEfGroup);

//                                // Crear un nuevo grupo con los datos actualizados
//                                var newEfGroup = _mapper.Map<EFGroup>(domainGroup);
//                                newEfGroup.ID = 0; // Asegura que se cree un nuevo registro
//                                newEfGroup.CreatedAt = DateTime.UtcNow;
//                                newEfGroup.IsActive = true;

//                                var addedGroup = await context.Groups.AddAsync(newEfGroup);
//                                await context.SaveChangesAsync();

//                                // Actualizar relaciones si es necesario
//                                await UpdateRelatedEntities(context, existingEfGroup.ID, addedGroup.Entity.ID);

//                                await transaction.CommitAsync();

//                                var createdGroup = _mapper.Map<Group>(addedGroup.Entity);
//                                successes.Add(createdGroup);
//                            }
//                            else
//                            {
//                                throw new InvalidOperationException($"Group with ID {domainGroup.ID} is not active or does not exist.");
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            await transaction.RollbackAsync();
//                            failures.Add((domainGroup, ex));
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        failures.Add((domainGroup, ex));
//                    }
//                    finally
//                    {
//                        semaphore.Release();
//                    }
//                }));
//            }

//            await Task.WhenAll(tasks);
//            return (successes, failures);
//        }

//        public async Task<(IEnumerable<Group> Successes, IEnumerable<(Group FailedGroup, Exception Error)> Failures)> DeleteBatch(IEnumerable<Group?> domainGroups)
//        {
//            var semaphore = new SemaphoreSlim(_maxConcurrency);
//            var tasks = new List<Task>();
//            var successes = new ConcurrentBag<Group>();
//            var failures = new ConcurrentBag<(Group FailedGroup, Exception Error)>();

//            foreach (var domainGroup in domainGroups)
//            {
//                if (domainGroup == null)
//                {
//                    continue; // Salta grupos nulos
//                }

//                await semaphore.WaitAsync();
//                tasks.Add(Task.Run(async () =>
//                {
//                    try
//                    {
//                        using var context = _contextFactory.CreateContext();
//                        using var transaction = await context.Database.BeginTransactionAsync();

//                        try
//                        {
//                            var existingEfGroup = await context.Groups.FindAsync(domainGroup.ID);
//                            if (existingEfGroup != null && existingEfGroup.IsActive)
//                            {
//                                // Desactivar el grupo existente
//                                existingEfGroup.IsActive = false;
//                                existingEfGroup.CreatedAt = DateTime.UtcNow;
//                                context.Groups.Update(existingEfGroup);

//                                await context.SaveChangesAsync();
//                                await transaction.CommitAsync();

//                                var updatedGroup = _mapper.Map<Group>(existingEfGroup);
//                                successes.Add(updatedGroup);
//                            }
//                            else
//                            {
//                                throw new InvalidOperationException($"Group with ID {domainGroup.ID} is not active or does not exist.");
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            await transaction.RollbackAsync();
//                            failures.Add((domainGroup, ex));
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        failures.Add((domainGroup, ex));
//                    }
//                    finally
//                    {
//                        semaphore.Release();
//                    }
//                }));
//            }

//            await Task.WhenAll(tasks);
//            return (successes, failures);
//        }

//        public async Task<bool> Delete(int ID)
//        {
//            using var context = _contextFactory.CreateContext();
//            using var transaction = await context.Database.BeginTransactionAsync();

//            try
//            {
//                var efGroup = await context.Groups.FindAsync(ID);
//                if (efGroup == null || !efGroup.IsActive)
//                {
//                    return false;
//                }

//                efGroup.IsActive = false;
//                efGroup.CreatedAt = DateTime.UtcNow;
//                context.Groups.Update(efGroup);

//                await context.SaveChangesAsync();
//                await transaction.CommitAsync();
//                return true;
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<bool> Delete(Group domainGroup)
//        {
//            if (domainGroup == null)
//                return false;

//            return await Delete(domainGroup.ID);
//        }

//        public async Task<Group?> Update(Group domainGroup)
//        {
//            ValidateEntity(domainGroup);

//            using var context = _contextFactory.CreateContext();
//            var existingEfGroup = await context.Groups.FindAsync(domainGroup.ID);

//            if (existingEfGroup == null || !existingEfGroup.IsActive)
//                return null;

//            using var transaction = await context.Database.BeginTransactionAsync();
//            try
//            {
//                // Desactivar el grupo existente
//                existingEfGroup.IsActive = false;
//                existingEfGroup.CreatedAt = DateTime.UtcNow;
//                context.Groups.Update(existingEfGroup);

//                // Crear un nuevo grupo con los datos actualizados
//                var newEfGroup = _mapper.Map<EFGroup>(domainGroup);
//                newEfGroup.ID = 0; // Asegura que se cree un nuevo registro
//                newEfGroup.CreatedAt = DateTime.UtcNow;
//                newEfGroup.IsActive = true;

//                var addedGroup = await context.Groups.AddAsync(newEfGroup);
//                await context.SaveChangesAsync();

//                // Actualizar relaciones si es necesario
//                await UpdateRelatedEntities(context, existingEfGroup.ID, addedGroup.Entity.ID);

//                await transaction.CommitAsync();

//                return _mapper.Map<Group>(addedGroup.Entity);
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<Group?> Get()
//        {
//            using var context = _contextFactory.CreateContext();
//            var efGroup = await context.Groups.FirstOrDefaultAsync(e => e.IsActive);
//            return efGroup != null ? _mapper.Map<Group>(efGroup) : null;
//        }

//        public async Task<Group?> Get(int ID)
//        {
//            using var context = _contextFactory.CreateContext();
//            var efGroup = await context.Groups.FirstOrDefaultAsync(e => e.ID == ID && e.IsActive);
//            return efGroup != null ? _mapper.Map<Group>(efGroup) : null;
//        }

//        // Métodos auxiliares

//        private async Task DeactivateEntity(DatabaseContext context, EFGroup efGroup)
//        {
//            efGroup.IsActive = false;
//            efGroup.CreatedAt = DateTime.UtcNow;
//            context.Groups.Update(efGroup);
//            await Task.CompletedTask; // Placeholder para compatibilidad async
//        }

//        private async Task<EFGroup> CreateNewEntity(DatabaseContext context, Group domainGroup)
//        {
//            var efGroup = _mapper.Map<EFGroup>(domainGroup);
//            efGroup.ID = 0; // Asegura que se cree un nuevo registro
//            efGroup.CreatedAt = DateTime.UtcNow;
//            efGroup.IsActive = true;

//            var newEntity = await context.Groups.AddAsync(efGroup);
//            return newEntity.Entity;
//        }

//        private async Task UpdateRelatedEntities(DatabaseContext context, int oldGroupId, int newGroupId)
//        {
//            var relatedIdentifiers = await context.IdentifierGroups
//                .Where(ig => ig.GroupID == oldGroupId)
//                .ToListAsync();

//            foreach (var identifierGroup in relatedIdentifiers)
//            {
//                identifierGroup.GroupID = newGroupId;
//            }

//            await context.SaveChangesAsync();
//        }

//        private void ValidateEntity(Group domainGroup)
//        {
//            if (string.IsNullOrWhiteSpace(domainGroup.UniqueKey))
//                throw new ArgumentException("UniqueKey is required.");
//        }
//    }
//}
