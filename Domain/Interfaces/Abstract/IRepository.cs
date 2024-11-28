using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> Create(TEntity entity);
        Task<TEntity?> Get(int id);
        Task<TEntity?> Get();
        Task<TEntity?> Update(TEntity entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(TEntity entity);
    }
}
