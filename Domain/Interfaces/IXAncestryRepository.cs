using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IXAncestryRepository
    {
        Task<List<XAncestry>> GetAncestries(int ClassID);
        Task<bool> Update(int ClassID, int ParentID, XAncestry input);
        Task<bool> Delete(int ClassID, int ParentID);
    }
}
