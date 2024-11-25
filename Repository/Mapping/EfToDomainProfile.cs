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
            CreateMap<EfIdentifier, Identifier>()
                .ForMember(dest => dest.PrimaryGroup, opt => opt.MapFrom(src => src.PrimaryGroup))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.OtherGroups, opt => opt.MapFrom(src => src.IdentifierGroups.Select(ig => ig.Group)));

            // Mapeo inverso de EFGroup a Group
            CreateMap<EFGroup, Group>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.Identifiers, opt => opt.MapFrom(src => src.IdentifierGroups.Select(ig => ig.Identifier)));
        }
    }
}
