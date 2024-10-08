namespace Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> Details(int id);
        Task<List<TEntity>> All();
    }
}
