using Domain.Interfaces;
using Domain.Models;

namespace Application
{
    public class XClassService
    {
        private readonly IXClassRepository repository;
        public XClassService(IXClassRepository repo)
        {
            repository = repo;
        }

        public async Task<XClass?> Get(int id, bool WithRelations = false)
        {
            try
            {
                var entity = await repository.Get(id, WithRelations);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<XClass>> Get()
        {
            try
            {
                var list = await repository.Get();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> Create(XClass input)
        {
            try
            {
                var res = await repository.Create(input);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Update(XClass input)
        {
            try
            {
                var res = await repository.Update(input);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> AddProperty(int ClassID, XProperty property)
        {
            try
            {
                var res = await repository.AddProperty(ClassID, property);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveProperty(int PropertyID)
        {
            try
            {
                var res = await repository.RemoveProperty(PropertyID);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var res = await repository.Delete(id);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
