using Harfien.Application.DTO.Order;
using Harfien.Domain.Enums;

namespace Harfien.Application.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateAsync(CreateOrderDto dto, int clientId);
        Task<OrderResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<OrderInfoDto>> GetClientOrdersAsync(int clientId);
        Task<IEnumerable<OrderInfoDto>> GetCraftsmanOrdersAsync(int craftsmanId);
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<IEnumerable<OrderResponseDto>> GetByStatusAsync(OrderStatus status);
        Task AcceptAsync(int orderId, int craftsmanId);
        Task RejectAsync(int orderId, int craftsmanId);
        Task StartAsync(int orderId, int craftsmanId);
        Task CompleteAsync(int orderId, int craftsmanId);
        Task CancelAsync(int orderId, int clientId);
    }
}