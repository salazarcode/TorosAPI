using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstract
{
    public interface IConsistentBatchRepository<TEntity> where TEntity : class
    {
        Task<(bool Success, Exception? Error)> CreateBatch(IEnumerable<TEntity> entities);
        Task<(bool Success, Exception? Error)> UpdateBatch(IEnumerable<TEntity> entities);
        Task<(bool Success, Exception? Error)> DeleteBatch(IEnumerable<TEntity> entities);
    }
}
