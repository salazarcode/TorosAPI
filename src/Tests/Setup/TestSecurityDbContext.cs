using Infrastructure.Repository.EF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Tests.Setup
{
    public class TestSecurityDbContext : IDisposable
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;

        public TestSecurityDbContext()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer("Server=.;Database=eav;Trusted_Connection=True;TrustServerCertificate=True;ConnectRetryCount=3;ConnectRetryInterval=10;Connection Timeout=30;")
                .Options;

            _context = new DatabaseContext(_options);
            _context.Database.EnsureCreated();
        }

        protected DatabaseContext GetContext() => new DatabaseContext(_options);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
