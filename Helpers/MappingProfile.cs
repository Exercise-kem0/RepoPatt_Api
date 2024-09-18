using AutoMapper;
using RepoPatt_Core.Models;

namespace RepoPatt_Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, Author>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
