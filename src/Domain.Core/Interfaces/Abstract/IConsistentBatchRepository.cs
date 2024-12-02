namespace Domain.Core.Interfaces.Abstract
{
    public interface IConsistentBatchRepository<TEntity> where TEntity : class
    {
        Task<(bool Success, Exception? Error)> CreateBatch(IEnumerable<TEntity> entities);
        Task<(bool Success, Exception? Error)> UpdateBatch(IEnumerable<TEntity> entities);
        Task<(bool Success, Exception? Error)> DeleteBatch(IEnumerable<TEntity> entities);
    }
}
