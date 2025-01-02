﻿using Domain.Core.Entities;
using Domain.Core.Interfaces.Abstract;

namespace Domain.Core.Interfaces
{
    public interface ITenantRepository : IRepository<DomainTenant>
    {
    }
}
