using System.Numerics;

namespace Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> Details(int id);
        Task<List<TEntity>> All();
        Task<int> Create(TEntity input);
        Task<TEntity> Update(int ID);
        Task<bool> Delete(int ID);
    }
}
