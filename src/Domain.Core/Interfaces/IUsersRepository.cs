using Domain.Core.Interfaces.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
    }
}
