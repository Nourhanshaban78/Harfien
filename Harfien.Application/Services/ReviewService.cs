using Harfien.Application.DTO.Review;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared;
using Harfien.Domain.Shared.Repositories;

namespace Harfien.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICraftsmanRepository _craftsmanRepository;
        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository, ICraftsmanRepository craftsmanRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _craftsmanRepository = craftsmanRepository;
        }
        public async Task<ReviewDto> AddReviewAsync(CreateReviewDto dto, String currentUserId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(dto.OrderId);

            if (order == null)
                throw new Exception("Order not found.");

            if (order.Client?.User?.Id != currentUserId)
                throw new Exception("You are not authorized to review this order.");

            if (await _reviewRepository.HasReviewForOrderAsync(order.Id))
                throw new Exception("You already reviewed this order.");

            if (order.Status != OrderStatus.Completed)
                throw new Exception("You can only review completed orders.");

            if (order.Craftsman == null)
                throw new Exception("Craftsman information is missing for this order.");

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var review = new Review
            {
                OrderId = order.Id,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ClientId = order.ClientId,
                CraftsmanId = order.CraftsmanId,
                CreatedAt = DateTime.UtcNow
            };
            //var craftsman = await _craftsmanRepository.GetByIdAsync(order.CraftsmanId);
            //if (craftsman == null)
            //    throw new Exception("Craftsman not found.");
            order.Craftsman.Rating = await CalculateNewCraftsmanRating(oldAvg: order.Craftsman.Rating, newRating: dto.Rating, order);
            _craftsmanRepository.Update(order.Craftsman);
            await _reviewRepository.AddAsync(review);
            // could use unit of work for avoding confusion
            // This saves the Review AND the Craftsman update in a single transaction.
            await _reviewRepository.SaveAsync();

            var createdReview = await _reviewRepository.GetByIdAsync(review.Id);
            if (createdReview == null) throw new Exception("Failed to create review.");

            return new ReviewDto
            {
                Id = createdReview.Id,
                Rating = createdReview.Rating,
                Comment = createdReview.Comment,
                ClientName = createdReview.Order?.Client?.User?.FullName ?? "Unknown User",
                CreatedAt = DateOnly.FromDateTime(createdReview.CreatedAt)
            };
        }
        private async Task<double> CalculateNewCraftsmanRating(double oldAvg, double newRating, Order order)
        {
            var existingReviews = await _reviewRepository.GetAllByCraftsmanIdAsync(order.CraftsmanId);
            int oldCount = existingReviews.ToList().Count;
            double newAverage = ((oldAvg * oldCount) + newRating) / (oldCount + 1);
            return Math.Round(newAverage, 2);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByCraftsmanIdAsync(int craftsmanId)
        {
            var reviews = await _reviewRepository.GetAllByCraftsmanIdAsync(craftsmanId);

            // Map Entity to DTO
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ClientName = r.Order?.Client?.User?.FullName ?? "Unknown User",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = DateOnly.FromDateTime(r.CreatedAt)
            }).ToList();
        }
        public async Task<PagedResult<ReviewDto>> GetPagedReviewsByCraftsmanIdAsync(int craftsmanId, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var (reviews, totalCount) = await _reviewRepository.GetPagedByCraftsmanIdAsync(craftsmanId, pageNumber, pageSize);

            var dtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ClientName = r.Order?.Client?.User?.FullName ?? "Unknown User",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = DateOnly.FromDateTime(r.CreatedAt)
            }).ToList();

            return new PagedResult<ReviewDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
