using AutoMapper;
using Harfien.Application.DTO.Service;
using Harfien.Domain.Entities;

namespace Harfien.Application.Mappings
{
    public class ServiceProfile:Profile
    {
      public  ServiceProfile() {
            CreateMap<ServiceCreateDto, Service>();

            CreateMap<Service,ServiceUpdateDto>().ReverseMap();
            CreateMap<Service, ServiceReadDto>()
           .ForMember(dest => dest.CraftsmanName, opt => opt.MapFrom(src => src.Craftsman.User.FullName))
           .ForMember(dest => dest.ServiceCategoryName, opt => opt.MapFrom(src => src.ServiceCategory.Name))
           .ForMember(d => d.CraftsmanCity, o => o.MapFrom(s => s.Craftsman.User.Area.Name));


        }
    }
}
