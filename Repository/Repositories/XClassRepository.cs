using Dapper;
using Domain.Models;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Repository.Repositories
{
    public class XClassRepository : IXClassRepository
    {
        private readonly IDbConnection _dbConnection;
        public XClassRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<XClass>> All()
        {
            try
            {
                string query = $"SELECT * FROM abstract.classes";

                var res = await _dbConnection.QueryAsync<XClass>(query);

                return res.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> Create(XClass input)
        {
            try
            {
                string sql = "INSERT INTO abstract.classes VALUES (@Name, @IsPrimitive);SELECT SCOPE_IDENTITY();";

                var ids = await _dbConnection.QueryAsync<int>(sql, new { 
                    Name = input.Name, 
                    IsPrimitive = input.IsPrimitive ? 1 : 0 
                });

                return ids.First();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> Delete(int ID)
        {
            try
            {
                string sql = "delete from abstract.classes where ID = @id";

                var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                {
                    id = ID
                });

                return affectedRows != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<XClass?> Details(int id)
        {
            try
            {
                // Abrir manualmente la conexión si no está abierta
                if (_dbConnection.State == ConnectionState.Closed)
                {
                    _dbConnection.Open();
                }

                var classTask = _dbConnection.QueryAsync<XClass>("select * from abstract.classes where id = @id", new { id = id });
                var ancestriesTask = GetAncestries(id);
                var propertiesTask = GetProperties(id);

                await Task.WhenAll(classTask, ancestriesTask, propertiesTask);

                var classRes = await classTask;
                var xAncestries = await ancestriesTask;
                var xProperties = await propertiesTask;

                // Verificar si se encontró la clase
                if (!classRes.Any())
                    throw new Exception("Class not found");

                // Poblar la entidad con los resultados obtenidos
                XClass xclass = classRes.First();
                xclass.XProperties = xProperties;
                xclass.XAncestries = xAncestries;

                return xclass;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> Update(XClass input)
        {
            try
            {
                string sql = "update abstract.class set name = @name, isprimitive = @isprimitive where id = @id";

                var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                {
                    name = input.Name,
                    isprimitive = input.IsPrimitive,
                    id = input.ID
                });

                return affectedRows != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> AddProperty(int ClassID, XProperty xproperty)
        {
            try
            {
                string sql = "INSERT INTO abstract.properties VALUES (@ClassID, @PropertyClassID, @Name, @Min, @Max);SELECT SCOPE_IDENTITY();";

                var ids = await _dbConnection.QueryAsync<int>(sql, new
                {
                    ClassID = ClassID,
                    PropertyClassID = xproperty.PropertyClassID,
                    Name = xproperty.Name,
                    Min = xproperty.Min,
                    Max = xproperty.Max
                });

                return ids.First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveProperty(int PropertyID)
        {
            try
            {
                string sql = "delete from abstract.properties where id = @id";

                var affectedRows = await _dbConnection.ExecuteAsync(sql, new
                {
                    id = PropertyID
                });

                return affectedRows != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<XProperty>> GetProperties(int ClassID)
        {
            try
            {
                var sql = $@"                
                    select 
                        p.*,
                        c.id PClassID,
                        c.*
                    from Abstract.Properties p
                    inner join abstract.classes c on p.PropertyClassID = c.ID
                    where p.ClassID in(
	                    select 
		                    id
	                    from 
		                    Abstract.Classes c
	                    where 
		                    c.ID = @ClassID
	                    union
	                    select 
		                    parentid 
	                    from 
		                    Abstract.Ancestries an
	                    where
		                    an.ClassID = @ClassID
                    )
                ";

                var res = await _dbConnection.QueryAsync<XProperty, XClass, XProperty>(
                    sql, 
                    map: (p, c) => {
                        p.XClass = c;
                        return p;
                    },
                    param: new { 
                        ClassID = ClassID
                    },
                    splitOn: "PClassID"
                );

                return res.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<XAncestry>> GetAncestries(int ClassID)
        {
            try
            {
                var sql = $@"                
                    select 
                        an.*,
                        parent.ID PClassID,
                        parent.*,
	                    child.ID CClassID,
	                    child.*
                    from Abstract.Ancestries an
                    left join Abstract.Classes parent on parent.ID = an.parentid
                    left join Abstract.Classes child on child.ID = an.ClassID
                    where 
	                    child.id = @ClassID
                ";

                var res = await _dbConnection.QueryAsync<XAncestry, XClass,XClass, XAncestry>(
                    sql,
                    map: (an, parent, child) => {
                        an.Parent = parent;
                        an.XClass = child;
                        return an;
                    },
                    param: new
                    {
                        ClassID = ClassID
                    },
                    splitOn: "PClassID,CClassID"
                );

                return res.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
