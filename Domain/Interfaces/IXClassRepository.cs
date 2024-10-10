using Domain.Models;

namespace Domain.Interfaces
{
    public interface IXClassRepository : IRepository<XClass>
    {
        Task<int> AddProperty(int ClassID, XProperty xproperty);
        Task<bool> RemoveProperty(int PropertyID);
        Task<List<XProperty>> GetProperties(int ClassID);
        Task<List<XAncestry>> GetAncestries(int ClassID);
    }
}
