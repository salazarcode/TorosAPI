using Domain.Models;

namespace Interfaces.Repositories
{
    public interface IXClassRepository: IRepository<XClass>
    {
        Task<int> AddProperty(XClass xclass, XProperty xproperty);
        Task<bool> RemoveProperty(XClass xclass, XProperty xproperty);
    }
}
