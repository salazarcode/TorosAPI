using AutoMapper;
using Domain.Core.Interfaces;
using Domain.Entities;
using Infrastructure.Repository.EF.Abstract;
using Infrastructure.Repository.EF.Contexts;
using Infrastructure.Repository.EF.Models;
using System.Linq.Expressions;

namespace Infrastructure.Repository.EF
{
    public class UsersRepository : BaseRepository<User, EfUser, int>, IUsersRepository
    {
        public UsersRepository(DatabaseContextFactory contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
            SetDefaultIncludes(new Expression<Func<EfUser, object>>[]
            {
                x => x.PrimaryGroup,
                x => x.GroupUsers
            });
        }
    }
}
