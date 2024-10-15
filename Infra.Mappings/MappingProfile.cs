using AutoMapper;
using Domain.Models;
using Infra.DTOs.Responses.Classes;
using Infra.Repositories.EF.Models;

namespace Infra.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<XClass, Class>();
            CreateMap<Class, XClass>();

            CreateMap<XProperty, Property>();
            CreateMap<Property, XProperty>();

            CreateMap<XObject, Repositories.EF.Models.Object>();
            CreateMap<Repositories.EF.Models.Object, XObject>();

            CreateMap<XStringValue, StringValue>();
            CreateMap<StringValue, XStringValue>();

            CreateMap<XAbstractPropertyDetail, AbstractPropertyDetail>();
            CreateMap<AbstractPropertyDetail, XAbstractPropertyDetail>();

            CreateMap<XClass, ClassDetailRS>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.PropertyClasses))
                .ForMember(dest => dest.Parents, opt => opt.MapFrom(src => src.Parents));


            CreateMap<XProperty, PropertyDTO>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassKey, opt => opt.MapFrom(src => src.PropertyClass.Key));


            CreateMap<ClassDetailRS, XClass>();
        }
    }
}
