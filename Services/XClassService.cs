using Services.Interfaces;
using Services.Interfaces.Repositories;

namespace Services
{
    public class XClassService
    {
        private readonly IXClassRepository repository;
        public XClassService(IXClassRepository repo)
        {
            repository = repo;
        }
    }
}
