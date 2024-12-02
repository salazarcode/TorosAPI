namespace Domain.Core.Interfaces.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> Create(TEntity entity);
        Task<TEntity?> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Update(TEntity entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(TEntity entity);
    }
}
