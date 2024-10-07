namespace Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();
        TEntity GetByID(int id);
    }
}
