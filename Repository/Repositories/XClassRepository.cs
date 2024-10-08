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

        public async Task<XClass?> Details(int id)
        {
            try
            {
                string query = $@"
                                SELECT 
                                    c.*, 
                                    p.id propertyID, 
                                    p.*, 
                                    cx.id propertyClassID, 
                                    cx.*,
									an.ParentID ParentID,
									an.*,
									cy.ID ParentClassID,
									cy.*
                                FROM 
                                    abstract.classes c 
                                    left join Abstract.Properties p on p.ClassID = c.ID
                                    left join Abstract.Classes cx on p.PropertyClassID = cx.ID
                                    left join Abstract.Ancestries an on c.ID = an.ClassID
									left join Abstract.Classes cy on an.ParentID = cy.ID
                                WHERE 
                                    c.Id = @Id
                ";

                var properties = new Dictionary<int, XProperty>();
                var ancestries = new Dictionary<string, XAncestry>();

                var res = await _dbConnection.QueryAsync<XClass, XProperty, XClass,XAncestry,XClass, XClass>(
                    sql: query, 
                    map: (c, p, cx, an, cy) =>{

                        an.Parent = cy;
                        an.XClassID = c.ID;
                        if (!ancestries.ContainsKey(an.XClassID + "-" + an.ParentID))
                            ancestries.Add(an.XClassID + "-" + an.ParentID, an);

                        p.XClass = cx;
                        if (!properties.ContainsKey(p.ID))
                            properties.Add(p.ID, p);

                        return c;
                    },
                    splitOn: "propertyID,propertyClassID,ParentID,ParentClassID",
                    param: new { 
                        Id = id 
                    }
                );

                if (res.Count() != 0)
                {
                    res.First().XProperties = properties.Values.ToList();
                    res.First().XAncestries = ancestries.Values.ToList();
                }

                return res.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
