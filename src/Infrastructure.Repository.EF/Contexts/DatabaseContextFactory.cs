using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
    public class DatabaseContextFactory
    {
        private readonly DbContextOptions<DatabaseContext> _options;

        public DatabaseContextFactory(DbContextOptions<DatabaseContext> options)
        {
            _options = options;
        }

        public DatabaseContext CreateContext()
        {
            return new DatabaseContext(_options);
        }
    }
}
