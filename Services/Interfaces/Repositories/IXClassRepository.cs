using Domain.Models;

namespace Services.Interfaces.Repositories
{
    public interface IXClassRepository : IRepository<XClass>
    {
        Task<int> AddProperty(int ClassID, XProperty xproperty);
        Task<bool> RemoveProperty(int PropertyID);
        Task<List<XProperty>> GetProperties(int ClassID);
        Task<List<XAncestry>> GetAncestries(int ClassID);
    }
}
