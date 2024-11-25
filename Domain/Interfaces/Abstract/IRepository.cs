using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstract
{
    public interface IRepository<DomainType>
    {
        public Task<DomainType?> Get();
        public Task<DomainType?> Get(int ID);
        public Task<DomainType?> Create(DomainType domainType);
        public Task<DomainType?> Update(DomainType domainType);
        public Task<bool> Delete(int ID);
        public Task<bool> Delete(DomainType domainType);
        //Batch Operations
        public Task<DomainType?> CreateBatch(IEnumerable<DomainType> domainTypes);
        public Task<(IEnumerable<DomainType?> successes, IEnumerable<(DomainType failedEntity, Exception error)> failures)> UpdateBatch(IEnumerable<DomainType> entities);
        public Task<DomainType?> DeleteBatch(IEnumerable<DomainType?> entities);
    }
}
