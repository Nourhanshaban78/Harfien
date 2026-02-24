using Harfien.Domain.Enums;

namespace Harfien.Application.DTO.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string CraftsmanName { get; set; } = null!;
        public decimal Amount { get; set; }

        public int? OrderId { get; set; }      

        public OrderStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int CraftsmanId { get; set; }
        public int ClientId { get; set; }
    }
}