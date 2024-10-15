using System.Numerics;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> Get(int id);
        Task<List<TEntity>> Get();
        Task<TEntity?> Create(TEntity input);
        Task<TEntity?> Update(TEntity input);
        Task Delete(int ID);
    }
}
