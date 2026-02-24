using AutoMapper;
using Harfien.Application.DTO.Order;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;

namespace Harfien.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICraftsmanRepository _craftsmanRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            ICraftsmanRepository craftsmanRepository,
            IClientRepository clientRepository,
            IAvailabilityRepository availabilityRepository,
            IServiceRepository serviceRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _craftsmanRepository = craftsmanRepository;
            _clientRepository = clientRepository;
            _availabilityRepository = availabilityRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        // ===========================
        // إنشاء أوردر جديد
        // ===========================
        public async Task<string> CreateAsync(CreateOrderDto dto, int clientId)
        {
            var client = await _clientRepository.GetByIdAsync(clientId)
        ?? throw new KeyNotFoundException("Client not found");

            var craftsman = await _craftsmanRepository.GetByIdAsync(dto.CraftsmanId)
                ?? throw new KeyNotFoundException("Craftsman not found");

            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId)
                ?? throw new KeyNotFoundException("Service not found");


            var scheduledLocal = dto.ScheduledAt;

            var isAvailable = await _availabilityRepository
                .IsAvailableAsync(craftsman.Id, scheduledLocal);

            if (!isAvailable)
                return "Craftsman not available at this time";

            var isBooked = await _orderRepository.ExistsAsync(craftsman.Id, scheduledLocal);
            if (isBooked)
                return "This slot is already booked";

            var order = new Order
            {
                ClientId = clientId,
                CraftsmanId = dto.CraftsmanId,
                ServiceId = dto.ServiceId,
                Description = dto.Description,
                ScheduledAt = scheduledLocal.ToUniversalTime(), // حفظ UTC
                Status = OrderStatus.Pending,
                Amount = service.Price
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync();

            return "Order created successfully";
        }

        // ===========================
        // جلب أوردر حسب Id
        // ===========================
        public async Task<OrderResponseDto> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(id)
                ?? throw new KeyNotFoundException("Order not found");

            return _mapper.Map<OrderResponseDto>(order);
        }

        // ===========================
        // جلب كل أوردرز العميل
        // ===========================
        public async Task<IEnumerable<OrderInfoDto>> GetClientOrdersAsync(int clientId)
        {
            var orders = await _orderRepository.GetByClientIdAsync(clientId);
            return _mapper.Map<IEnumerable<OrderInfoDto>>(orders);
        }

        // ===========================
        // جلب كل أوردرز الحرفي
        // ===========================
        public async Task<IEnumerable<OrderInfoDto>> GetCraftsmanOrdersAsync(int craftsmanId)
        {
            var orders = await _orderRepository.GetByCraftsmanIdAsync(craftsmanId);
            return _mapper.Map<IEnumerable<OrderInfoDto>>(orders);
        }

        // ===========================
        // جلب كل الأوردرز
        // ===========================
        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        // ===========================
        // جلب الأوردرز حسب الحالة
        // ===========================
        public async Task<IEnumerable<OrderResponseDto>> GetByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        // ===========================
        // تحديث حالات الأوردر
        // ===========================
        private async Task UpdateStatusAsync(int orderId, int userId, bool isClient, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId)
                ?? throw new KeyNotFoundException("Order not found");

            if (isClient && order.ClientId != userId)
                throw new UnauthorizedAccessException("Unauthorized");
            if (!isClient && order.CraftsmanId != userId)
                throw new UnauthorizedAccessException("Unauthorized");

            if (order.Status == newStatus)
                throw new InvalidOperationException("Order already in this status");

            // 🔴 Validate Transition
            if (!IsValidTransition(order.Status, newStatus))
                throw new InvalidOperationException("Invalid status transition");

            order.Status = newStatus;

            _orderRepository.Update(order);
            await _orderRepository.SaveAsync();
        }

        private bool IsValidTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                // Pending
                (OrderStatus.Pending, OrderStatus.Accepted) => true,
                (OrderStatus.Pending, OrderStatus.Rejected) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,

                // Accepted
                (OrderStatus.Accepted, OrderStatus.Running) => true,
                (OrderStatus.Accepted, OrderStatus.Cancelled) => true,

                // Running
                (OrderStatus.Running, OrderStatus.Completed) => true,

                _ => false
            };
        }



        public Task AcceptAsync(int orderId, int craftsmanId) =>
            UpdateStatusAsync(orderId, craftsmanId, false, OrderStatus.Accepted);

        public Task RejectAsync(int orderId, int craftsmanId) =>
            UpdateStatusAsync(orderId, craftsmanId, false, OrderStatus.Rejected);

        public Task StartAsync(int orderId, int craftsmanId) =>
            UpdateStatusAsync(orderId, craftsmanId, false, OrderStatus.Running);

        public Task CompleteAsync(int orderId, int craftsmanId) =>
            UpdateStatusAsync(orderId, craftsmanId, false, OrderStatus.Completed);

        public Task CancelAsync(int orderId, int clientId) =>
            UpdateStatusAsync(orderId, clientId, true, OrderStatus.Cancelled);
    }
}
