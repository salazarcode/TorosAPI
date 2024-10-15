using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infra.DTOs;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using static Dapper.SqlMapper;

namespace Infra.Repositories.Dapper
{
    public class XClassRepository : IXClassRepository
    {
        private readonly string _connectionString;
        public XClassRepository(ConnectionStrings conns)
        {
            _connectionString = conns.DevLocal;
        }

        public async Task<List<XClass>> Get()
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string query = $"SELECT * FROM classes";

                    var res = await _dbConnection.QueryAsync<XClass>(query);

                    return res.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<XClass?> Get(int id)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string query = $"SELECT * FROM classes where id = @id";

                    var res = await _dbConnection.QueryAsync<XClass>(query, new
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

        public async Task<XClass> Create(XClass input)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "INSERT INTO classes VALUES (@Key, @Name, @IsPrimitive);SELECT SCOPE_IDENTITY();";

                    var ids = await _dbConnection.QueryAsync<int>(sql, new
                    {
                        input.Name,
                        IsPrimitive = input.IsPrimitive ? 1 : 0,
                        input.Key
                    });

                    input.ID = ids.First();

                    return input;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task Delete(int ID)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "delete from classes where ID = @id";

                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        id = ID
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<XClass> Update(XClass input)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_connectionString))
                {
                    _dbConnection.Open();

                    string sql = "update classes set name = @name, [key] = @key, isprimitive = @isprimitive where id = @id";

                    var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                    {
                        name = input.Name,
                        isprimitive = input.IsPrimitive,
                        id = input.ID,
                        key = input.Key,
                    });

                    return input;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
