using Dapper;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class AbstractRepository<TEntity> where TEntity : class
    {
        protected readonly IDbConnection _dbConnection;
        private readonly string _tableName;
        public AbstractRepository(IDbConnection dbConnection, string tableName)
        {
            _dbConnection = dbConnection;
            _tableName = tableName;
        }

        public virtual List<TEntity> GetAll()
        {
            try
            {
                string query = $"SELECT * FROM {_tableName}";
                return _dbConnection.Query<TEntity>(query).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual TEntity GetByID(int id)
        {
            try
            {
                string query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
                return _dbConnection.QueryFirstOrDefault<TEntity>(query, new { Id = id });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
