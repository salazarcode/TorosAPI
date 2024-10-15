using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infra.Repositories.EF.Models;
using Infra.Repositories.EF.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.EF.Repositories
{
    public class XPropertyRepository : BaseRepository<Property>, IXPropertyRepository
    {
        private readonly IMapper _mapper;
        public XPropertyRepository(EavContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<int> AddProperty(XProperty property)
        {
            var p = _mapper.Map<Property>(property);
            await _dbSet.AddAsync(p);
            return await _context.SaveChangesAsync();
        }

        public async Task<XProperty?> Create(XProperty input)
        {
            var r = await Create(input);
            return _mapper.Map<XProperty>(r);
        }

        public async Task<List<XProperty>> GetFromClassID(int ClassID)
        {
            var r = await _dbSet.Where(x => x.ClassId == ClassID).FirstOrDefaultAsync();
            return _mapper.Map<List<XProperty>>(r);
        }

        public async Task<bool> RemoveProperty(XProperty property)
        {
            var p = await _dbSet.FindAsync(property.Id);

            if (p == null)
                throw new Exception("Property not found");

            _dbSet.Remove(p);
            var res = await _context.SaveChangesAsync();
            return res != 0;
        }

        public async Task<XProperty?> Update(XProperty input)
        {
            var r = await Update(input);
            return _mapper.Map<XProperty>(r);
        }

        async Task<XProperty?> IRepository<XProperty>.Get(int id)
        {
            var r = await Get(id);
            return _mapper.Map<XProperty>(r);
        }

        async Task<List<XProperty>> IRepository<XProperty>.Get()
        {
            var r = await Get();
            return _mapper.Map<List<XProperty>>(r);
        }
    }
}
