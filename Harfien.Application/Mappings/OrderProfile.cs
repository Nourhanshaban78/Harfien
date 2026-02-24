using AutoMapper;
using Harfien.Application.DTO.Order;
using Harfien.Domain.Entities;

namespace Harfien.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            // باقي الحقول زي ClientId و Status handled بعد المابّينج

            CreateMap<Order, OrderResponseDto>()
                .ForMember(d => d.ServiceName,
                    o => o.MapFrom(s => s.Service != null ? s.Service.Name : "N/A"))
                .ForMember(d => d.ClientName,
                    o => o.MapFrom(c => c.Client != null && c.Client.User != null
                        ? c.Client.User.FullName
                        : "N/A"))
                .ForMember(d => d.CraftsmanName,
                    o => o.MapFrom(c => c.Craftsman != null && c.Craftsman.User != null
                        ? c.Craftsman.User.FullName
                        : "N/A"));
            CreateMap<Order, OrderInfoDto>()
              .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client != null && s.Client.User != null
               ? s.Client.User.FullName
                 : "N/A"))
               .ForMember(d => d.CraftsmanName, o => o.MapFrom(s => s.Craftsman != null && s.Craftsman.User != null
                 ? s.Craftsman.User.FullName
                : "N/A"))
              .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service != null
                ? s.Service.Name
               : "N/A"));
        }
    }
}