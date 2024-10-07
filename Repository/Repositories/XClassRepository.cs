using Dapper;
using Domain.Models;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class XClassRepository : AbstractRepository<XClass>, IXClassRepository
    {
        public XClassRepository(IDbConnection dbConnection) : base(dbConnection, "Abstract.Classes")
        {
        }

        //public IEnumerable<XClass> GetEntitiesByCondition(string condition)
        //{
        //    string query = "SELECT * FROM YourTable WHERE Condition = @Condition";
        //    return _dbConnection.Query<XClass>(query, new { Condition = condition });
        //}
    }
}
