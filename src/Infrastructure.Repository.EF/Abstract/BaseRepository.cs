using AutoMapper;
using Infrastructure.Repository.EF.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository.EF.Abstract
{
    public abstract class BaseRepository<TEntity, TDbEntity, TKey>
        where TEntity : class
        where TDbEntity : class
        where TKey : struct
    {
        protected Expression<Func<TDbEntity, object>>[] _defaultIncludes { get; private set; }
        protected readonly DatabaseContextFactory _contextFactory;
        protected readonly IMapper _mapper;
        protected IQueryable<TDbEntity> DefaultQuery;

        protected BaseRepository(DatabaseContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _defaultIncludes = Array.Empty<Expression<Func<TDbEntity, object>>>();
        }

        protected void SetDefaultIncludes(Expression<Func<TDbEntity, object>>[] includes)
        {
            _defaultIncludes = includes;
        }

        protected IQueryable<TDbEntity> ApplyIncludes(DbSet<TDbEntity> dbSet)
        {
            var query = dbSet.AsQueryable();
            return _defaultIncludes.Aggregate(query, (current, include) => current.Include(include));
        }

        public virtual async Task<TEntity?> Create(TEntity entity)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var efEntity = _mapper.Map<TDbEntity>(entity);
                var result = await context.Set<TDbEntity>().AddAsync(efEntity);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return _mapper.Map<TEntity>(result.Entity);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<TEntity?> Get(TKey id)
        {
            using var context = _contextFactory.CreateContext();
            var dbSet = context.Set<TDbEntity>();
            var query = ApplyIncludes(dbSet);

            // Asumiendo que la propiedad ID es la clave primaria
            var efEntity = await query.FirstOrDefaultAsync(e =>
                Microsoft.EntityFrameworkCore.EF.Property<TKey>(e, "ID").Equals(id));

            return efEntity != null ? _mapper.Map<TEntity>(efEntity) : null;
        }

        public virtual async Task<TEntity?> Get()
        {
            using var context = _contextFactory.CreateContext();
            var dbSet = context.Set<TDbEntity>();
            var query = ApplyIncludes(dbSet);

            var efEntity = await query.FirstOrDefaultAsync();
            return efEntity != null ? _mapper.Map<TEntity>(efEntity) : null;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            var dbSet = context.Set<TDbEntity>();
            var query = ApplyIncludes(dbSet);

            var efEntities = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TEntity>>(efEntities);
        }

        public virtual async Task<TEntity?> Update(TEntity entity)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var efEntity = _mapper.Map<TDbEntity>(entity);
                context.Set<TDbEntity>().Update(efEntity);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                var res = _mapper.Map<TEntity>(efEntity);
                return res;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var efEntity = _mapper.Map<TDbEntity>(entity);
                context.Set<TDbEntity>().Remove(efEntity);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public virtual async Task<bool> Delete(int ID)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {

                var efEntity = await context.Set<TDbEntity>().FindAsync(ID);
                context.Set<TDbEntity>().Remove(efEntity);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}