using AutoMapper;
using Domain.Entities;
using Repository.Models;

namespace Repository.Mappings
{
    public class EfToDomainProfile : Profile
    {
        public EfToDomainProfile()
        {
            // Mapeo inverso de EfIdentifier a Identifier
            CreateMap<EfUser, User>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator));

            // Mapeo EfIdentifier a Identifier
            CreateMap<User, EfUser>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.CreatedBy));


            // Mapeo inverso de EFGroup a Group
            CreateMap<EFGroup, Group>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator));

            // Mapeo inverso de EFGroup a Group
            CreateMap<Group, EFGroup>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}
