using AutoMapper;
using Domain.Models;
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
        }
    }
}
