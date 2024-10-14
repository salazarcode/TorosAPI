using Domain.Interfaces;
using Domain.Models;

namespace Application
{
    public class XClassService
    {
        private readonly IXClassRepository _classRepository;
        private readonly IXPropertyRepository _propertyRepository;
        private readonly IXAncestryRepository _ancestryRepository;
        public XClassService(IXClassRepository classRepository, IXPropertyRepository propertyRepository, IXAncestryRepository ancestryRepository)
        {
            _classRepository = classRepository;
            _propertyRepository = propertyRepository;
            _ancestryRepository = ancestryRepository;
        }

        public async Task<XClass?> Get(int ClassID)
        {
            try
            {
                var classTask = _classRepository.Get(ClassID);
                var ancestriesTask = _ancestryRepository.GetAncestries(ClassID);
                var propertiesTask = _propertyRepository.GetProperties(ClassID);

                await Task.WhenAll(classTask, ancestriesTask, propertiesTask);

                var classResult = classTask.Result;
                if (classResult != null)
                {
                    classResult.XAncestries = ancestriesTask.Result;
                    classResult.XProperties = propertiesTask.Result;
                }

                return classResult;
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
                var res = await _classRepository.Update(input);
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
                var res = await _propertyRepository.AddProperty(ClassID, property);
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
                var res = await _propertyRepository.RemoveProperty(PropertyID);
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
                var res = await _classRepository.Delete(id);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
