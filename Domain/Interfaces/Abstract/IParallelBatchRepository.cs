using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstract
{
    public interface IParallelBatchRepository<TEntity> where TEntity : class
    {
        Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            CreateBatch(IEnumerable<TEntity> entities);

        Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            UpdateBatch(IEnumerable<TEntity> entities);

        Task<(IEnumerable<TEntity> Successes, IEnumerable<(TEntity Entity, Exception Error)> Failures)>
            DeleteBatch(IEnumerable<TEntity> entities);
    }
}
