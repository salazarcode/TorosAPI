using AutoMapper;
using Domain.Core.Entities;
using Domain.Entities;
using Infrastructure.Repository.EF.Models;

namespace Infrastructure.Mapper.AutoMapper.Maps
{
    public class EfToDomainProfile : Profile
    {
        public EfToDomainProfile()
        {
            // Mapeo de EfUser a User
            CreateMap<EfUser, User>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.PrimaryGroup, opt => opt.MapFrom(src => src.PrimaryGroup))
                .ForMember(dest => dest.OtherGroups, opt => opt.MapFrom(src => src.GroupUsers.Select(gu => gu.Group)));

            // Mapeo de User a EfUser
            CreateMap<User, EfUser>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PublicId, opt => opt.Ignore()) // Se generará en el repositorio
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => src.PasswordSalt))
                .ForMember(dest => dest.PrimaryGroupID, opt => opt.MapFrom(src => src.PrimaryGroup != null ? src.PrimaryGroup.ID : (int?)null))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.ID : (int?)null))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryGroup, opt => opt.Ignore())
                .ForMember(dest => dest.GroupUsers, opt => opt.Ignore());


            // Mapeo de EFGroup a Group
            CreateMap<EFGroup, Group>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.PrimaryGroupUsers, opt => opt.MapFrom(src => src.PrimaryGroupUsers))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.GroupUsers.Select(gu => gu.User)));

            // Mapeo de Group a EFGroup
            CreateMap<Group, EFGroup>()
                .ForMember(dest => dest.PublicId, opt => opt.Ignore()) // Se generará en el repositorio
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.ID : (int?)null))
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryGroupUsers, opt => opt.Ignore())
                .ForMember(dest => dest.GroupUsers, opt => opt.Ignore());
        }
    }
}
