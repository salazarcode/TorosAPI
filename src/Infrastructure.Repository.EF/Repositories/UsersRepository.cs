using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Abstract;
using Microsoft.EntityFrameworkCore;
using Repository.Contexts;
using Repository.Models;
using Repository.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repositories
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
