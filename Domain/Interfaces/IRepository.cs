using System.Numerics;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> Get(int id);
        Task<List<TEntity>> Get();
        Task<int> Create(TEntity input);
        Task<bool> Update(TEntity input);
        Task<bool> Delete(int ID);
    }
}
