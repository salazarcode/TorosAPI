using AutoMapper;
using Infrastructure.Repository.EF.Contexts;
using System.Collections.Concurrent;

namespace Infrastructure.Repository.EF.Abstract
{
    public abstract class ParallelBatchRepository<TEntity, TDbEntity, TKey>
        where TEntity : class
        where TDbEntity : class
        where TKey : struct
    {
        protected readonly DatabaseContextFactory _contextFactory;
        protected readonly IMapper _mapper;
        protected readonly int _maxConcurrency = 10;

        protected ParallelBatchRepository(
            DatabaseContextFactory contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public virtual async Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            CreateBatch(IEnumerable<TEntity> entities)
        {
            var semaphore = new SemaphoreSlim(_maxConcurrency);
            var tasks = new List<Task>();
            var successes = new ConcurrentBag<TEntity>();
            var failures = new ConcurrentBag<(TEntity, Exception)>();

            foreach (var entity in entities)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using var context = _contextFactory.CreateContext();
                        using var transaction = await context.Database.BeginTransactionAsync();

                        try
                        {
                            var efEntity = _mapper.Map<TDbEntity>(entity);
                            var result = await context.Set<TDbEntity>().AddAsync(efEntity);
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            var createdEntity = _mapper.Map<TEntity>(result.Entity);
                            successes.Add(createdEntity);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            failures.Add((entity, ex));
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            return (successes, failures);
        }

        public virtual async Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            UpdateBatch(IEnumerable<TEntity> entities)
        {
            var semaphore = new SemaphoreSlim(_maxConcurrency);
            var tasks = new List<Task>();
            var successes = new ConcurrentBag<TEntity>();
            var failures = new ConcurrentBag<(TEntity, Exception)>();

            foreach (var entity in entities)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using var context = _contextFactory.CreateContext();
                        using var transaction = await context.Database.BeginTransactionAsync();

                        try
                        {
                            var efEntity = _mapper.Map<TDbEntity>(entity);
                            context.Set<TDbEntity>().Update(efEntity);
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            successes.Add(entity);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            failures.Add((entity, ex));
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            return (successes, failures);
        }

        public virtual async Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            DeleteBatch(IEnumerable<TEntity> entities)
        {
            var semaphore = new SemaphoreSlim(_maxConcurrency);
            var tasks = new List<Task>();
            var successes = new ConcurrentBag<TEntity>();
            var failures = new ConcurrentBag<(TEntity, Exception)>();

            foreach (var entity in entities)
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using var context = _contextFactory.CreateContext();
                        using var transaction = await context.Database.BeginTransactionAsync();

                        try
                        {
                            var efEntity = _mapper.Map<TDbEntity>(entity);
                            context.Set<TDbEntity>().Remove(efEntity);
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            successes.Add(entity);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            failures.Add((entity, ex));
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
            return (successes, failures);
        }
    }
}
