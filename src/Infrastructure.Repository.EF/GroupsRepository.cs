
using AutoMapper;
using Domain.Core.Entities;
using Domain.Core.Interfaces;
using Infrastructure.Repository.EF.Abstract;
using Infrastructure.Repository.EF.Contexts;
using Infrastructure.Repository.EF.Models;
using System.Linq.Expressions;

namespace Infrastructure.Repository.EF
{
    public class GroupsRepository : BaseRepository<Group, EFGroup, int>, IGroupsRepository
    {
        public GroupsRepository(DatabaseContextFactory contextFactory, IMapper mapper, int maxConcurrency = 10) : base(contextFactory, mapper)
        {
            SetDefaultIncludes(new Expression<Func<EFGroup, object>>[]
            {
                x => x.PrimaryGroupUsers,
                x => x.GroupUsers
            });
        }
    }
}
