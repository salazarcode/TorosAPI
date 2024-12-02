using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.EF.Contexts
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
