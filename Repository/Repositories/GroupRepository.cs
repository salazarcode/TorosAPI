
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Repository.Contexts;
using Repository.Models;
using Repository.Repositories.Abstract;

namespace repository.repositories
{
    public class GroupRepository : BaseRepository<Group, EFGroup, int>, IGroupRepository
    {
        public GroupRepository(DatabaseContextFactory contextFactory, IMapper mapper, int maxConcurrency = 10) : base(contextFactory, mapper)
        {
        }
    }
}
