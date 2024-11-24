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

        /// <summary>
        /// Updates a batch of entities and returns the successful and failed updates.
        /// </summary>
        /// <param name="domainTypes">The batch of entities to update.</param>
        /// <returns>
        /// A tuple containing:
        /// - Successes: The list of successfully updated entities.
        /// - Failures: The list of entities that failed to update with their respective exceptions.
        /// </returns>
        public Task<(IEnumerable<DomainType?> successes, IEnumerable<(DomainType failedEntity, Exception error)> failures)> UpdateBatch(IEnumerable<DomainType> entities);
        public Task<DomainType?> DeleteBatch(IEnumerable<DomainType?> entities);
    }
}
