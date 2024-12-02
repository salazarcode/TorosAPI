
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Repository.Contexts;
using Repository.Models;
using Repository.Repositories.Abstract;
using System.Linq.Expressions;

namespace repository.repositories
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
