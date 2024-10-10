using System.Numerics;

namespace Services.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> Details(int id);
        Task<List<TEntity>> All();
        Task<int> Create(TEntity input);
        Task<bool> Update(TEntity input);
        Task<bool> Delete(int ID);
    }
}
