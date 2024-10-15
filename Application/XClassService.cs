using Domain.Interfaces;
using Domain.Models;

namespace Application
{
    public class XClassService
    {
        private readonly IXClassRepository _classRepository;
        private readonly IXPropertyRepository _propertyRepository;
        public XClassService(IXClassRepository classRepository, IXPropertyRepository propertyRepository)
        {
            _classRepository = classRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<XClass?> Get(int ClassID)
        {
            try
            {
                var res = await _classRepository.Get(ClassID);
                return res;
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
                var list = await _classRepository.Get();
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
                var res = await _classRepository.Create(input);
                return res.Id;
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
                var res = await _classRepository.Update(input);
                return res is not null;
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
                property.ClassId = ClassID;
                var res = await _propertyRepository.AddProperty(property);
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
                var p = await _propertyRepository.Get(PropertyID);

                if (p is null)
                    return false;

                var res = await _propertyRepository.RemoveProperty(p);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await _classRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
