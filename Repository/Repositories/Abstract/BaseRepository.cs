using AutoMapper;
using Domain.Interfaces.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.Contexts;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Abstract
{
    public abstract class BaseRepository<TEntity, TDbEntity, TKey> 
        where TEntity : class 
        where TDbEntity: class
        where TKey : struct
    {
        protected readonly DatabaseContextFactory _contextFactory;
        protected readonly IMapper _mapper;

        protected BaseRepository(DatabaseContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
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
            var efEntity = await context.Set<TDbEntity>().FindAsync(id);
            return efEntity != null ? _mapper.Map<TEntity>(efEntity) : null;
        }

        public virtual async Task<TEntity?> Get()
        {
            using var context = _contextFactory.CreateContext();
            var efEntity = await context.Set<TDbEntity>().FirstOrDefaultAsync();
            return efEntity != null ? _mapper.Map<TEntity>(efEntity) : null;
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

                return _mapper.Map<TEntity>(efEntity);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            using var context = _contextFactory.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var efEntity = await context.Set<TDbEntity>().FindAsync(id);
                if (efEntity == null)
                    return false;

                context.Set<TDbEntity>().Remove(efEntity);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual Task<bool> Delete(TEntity entity)
        {
            var id = GetEntityId(entity);
            return Delete(id);
        }
        protected virtual TKey GetEntityId(TEntity entity) {
            var property = entity.GetType().GetProperty("ID");
            var val = property?.GetValue(entity);
            if (val != null)
                return (TKey)val;
            throw new IOException("TKey isn't integer, you'll have to create a custom implementation of BaseRepository.ValidateEntity");
        }
    }
}
