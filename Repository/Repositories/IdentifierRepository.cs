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
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class IdentifierRepository : BaseRepository<Identifier, EfIdentifier, int>, IIdentifierRepository
    {
        public IdentifierRepository(DatabaseContextFactory contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
        }
    }
}
