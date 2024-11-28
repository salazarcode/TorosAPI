using AutoMapper;
using Repository.Contexts;

namespace Repository.Repositories.Abstract
{
    public abstract class ConsistentBatchRepository<TEntity, TDbEntity, TKey>
        where TEntity : class
        where TDbEntity : class
        where TKey : struct
    {
        protected readonly DatabaseContextFactory _contextFactory;
        protected readonly IMapper _mapper;

        protected ConsistentBatchRepository(DatabaseContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public virtual async Task<(bool Success, Exception? Error)> CreateBatch(IEnumerable<TEntity> entities)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                foreach (var entity in entities)
                {
                    var efEntity = _mapper.Map<TDbEntity>(entity);
                    await context.Set<TDbEntity>().AddAsync(efEntity);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex);
            }
        }

        public virtual async Task<(bool Success, Exception? Error)> UpdateBatch(IEnumerable<TEntity> entities)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                foreach (var entity in entities)
                {
                    var efEntity = _mapper.Map<TDbEntity>(entity);
                    context.Set<TDbEntity>().Update(efEntity);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex);
            }
        }

        public virtual async Task<(bool Success, Exception? Error)> DeleteBatch(IEnumerable<TEntity> entities)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                foreach (var entity in entities)
                {
                    var efEntity = _mapper.Map<TDbEntity>(entity);
                    context.Set<TDbEntity>().Remove(efEntity);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex);
            }
        }
    }
}
