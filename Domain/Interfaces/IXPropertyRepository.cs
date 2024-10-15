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
        public Task<List<XProperty>> GetFromClassID(int ClassID);
        Task<int> AddProperty(XProperty property);
        Task<bool> RemoveProperty(XProperty propert);
    }
}
