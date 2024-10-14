using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IXPropertyRepository : IRepository<XProperty>
    {
        Task<int> AddProperty(int ClassID, XProperty xproperty);
        Task<bool> RemoveProperty(int PropertyID);
        Task<List<XProperty>> GetProperties(int ClassID);
    }
}
