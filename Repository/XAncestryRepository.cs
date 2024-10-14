using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infra.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories.Dapper
{
    public class XAncestryRepository : IXAncestryRepository
    {
        private readonly string _connectionString;
        public XAncestryRepository(ConnectionStrings conns)
        {
            _connectionString = conns.DevLocal;
        }
        public async Task<List<XAncestry>> GetAncestries(int ClassID)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    var sql = $@"                
                    select 
                        an.*,
                        parent.ID PClassID,
                        parent.*,
	                    child.ID CClassID,
	                    child.*
                    from Ancestries an
                    left join Classes parent on parent.ID = an.parentid
                    left join Classes child on child.ID = an.ClassID
                    where 
	                    child.id = @ClassID
                ";

                    var res = await _dbConnection.QueryAsync<XAncestry, XClass, XClass, XAncestry>(
                        sql,
                        map: (an, parent, child) =>
                        {
                            an.Parent = parent;
                            an.XClass = child;
                            return an;
                        },
                        param: new
                        {
                            ClassID
                        },
                        splitOn: "PClassID,CClassID"
                    );

                    return res.ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<XAncestry?> Get(int id)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string query = $"SELECT * FROM ancestries where id = @id";

                    var res = await _dbConnection.QueryAsync<XAncestry>(query, new
                    {
                        id
                    });

                    return res.First();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<XAncestry>> Get()
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string query = $"SELECT * FROM ancestries";

                    var res = await _dbConnection.QueryAsync<XAncestry>(query);

                    return res.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> Create(XAncestry input)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "INSERT INTO ancestries VALUES (@ClassID, @ParentID);SELECT SCOPE_IDENTITY();";

                    var ids = await _dbConnection.QueryAsync<int>(sql, new
                    {
                        input.ClassID,
                        input.ParentID
                    });

                    return ids.First();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Update(int ClassID, int ParentID, XAncestry input)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "update ancestries set ClassID = @InputClassID, ParentID = @InputParentID where ClassId = @ClassID and ParentID = @ParentID";

                    var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                    {
                        ClassID,
                        ParentID,
                        InputClassID = input.ClassID,
                        InputParentID = input.ParentID
                    });

                    return affectedRows != 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int ClassID, int ParentID)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "delete from ancestries where ClassID = @ClassID and ParentID = @ParentID";

                    var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                    {
                        ClassID,
                        ParentID
                    });

                    return affectedRows != 0;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
