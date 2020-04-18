using AppWeather.Core.Domain.UserSearchModel;
using AppWeather.Core.Infrastructure;
using AutoMapper;

namespace AppWeather.Persistence.Model
{
    public class DomainToDataMappingProfile : Profile, IMapperProfile
    {
        public int Order => 1;

        public DomainToDataMappingProfile()
        {
            CreateMap<UserSearch, UserSearchData>();
        }
    }
}
