using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Infra.Repositories.EF.Models;
using Infra.Repositories.EF.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.EF.Repositories
{
    public class XClassRepository : BaseRepository<Class>, IXClassRepository
    {
        private readonly IMapper _mapper;
        public XClassRepository(EavContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<XClass?> Create(XClass input)
        {
            var c = await this.Create(input);
            return _mapper.Map<XClass>(c);
        }

        public async Task<XClass?> Update(XClass input)
        {
            var c = await this.Update(input);
            return _mapper.Map<XClass>(c);
        }

        async Task<XClass?> IRepository<XClass>.Get(int id)
        {
            var c = await _dbSet.Where(x => x.Id == id)
                                .Include(y => y.Parents)
                                .Include(z => z.PropertyClasses)
                                    .ThenInclude(pd => pd.PropertyClass)
                                .Include(w => w.Classes).FirstOrDefaultAsync();

            return _mapper.Map<XClass>(c);
        }

        async Task<List<XClass>> IRepository<XClass>.Get()
        {
            var c = await this.Get();
            return _mapper.Map<List<XClass>>(c);
        }
    }
}
