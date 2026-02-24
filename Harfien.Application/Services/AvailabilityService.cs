using Harfien.Application.DTO.CraftsmanAvailiability;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;

public class AvailabilityService : IAvailabilityService
{
    private readonly IAvailabilityRepository _availabilityRepository;
    private readonly ICraftsmanRepository _craftsmanRepository;

    public AvailabilityService(
        IAvailabilityRepository availabilityRepository,
        ICraftsmanRepository craftsmanRepository)
    {
        _availabilityRepository = availabilityRepository;
        _craftsmanRepository = craftsmanRepository;
    }

    // ==========================================
    // ✅ Add Availability
    // ==========================================
    public async Task AddAvailabilityAsync(CreateAvailabilityDto dto, string userId)
    {
        var craftsman = await _craftsmanRepository.GetByUserIdAsync(userId);
        if (craftsman == null)
            throw new Exception("Craftsman not found");

        // 🔥 Validation
        if (dto.StartTime >= dto.EndTime)
            throw new Exception("Start time must be earlier than end time");

        if (dto.EndTime == TimeSpan.Zero)
            throw new Exception("End time cannot be 00:00");

        // 🔥 Prevent duplicate day
        var existing = await _availabilityRepository
            .GetAllByCraftsmanIdAsync(craftsman.Id);

        if (existing.Any(a => a.Day == dto.Day))
            throw new Exception("Availability already exists for this day");

        var availability = new CraftsmanAvailability
        {
            CraftsmanId = craftsman.Id,
            Day = dto.Day,
            From = dto.StartTime,
            To = dto.EndTime,
            IsAvailable = dto.IsAvailable
        };

        await _availabilityRepository.AddAsync(availability);
        await _availabilityRepository.SaveAsync();
    }

    // ==========================================
    // ✅ Get Availability
    // ==========================================
    public async Task<IEnumerable<AvailabilityreadDto>>GetCraftsmanAvailabilityAsync(int craftsmanId)
    {
        var availabilities =
             await _availabilityRepository.GetAllByCraftsmanIdAsync(craftsmanId);

        return availabilities
            .Where(a => a.IsAvailable)
            .Select(a => new AvailabilityreadDto
            {
                CraftsmanId = a.CraftsmanId,
                Day = a.Day,
                StartTime = a.From,
                EndTime = a.To,
                IsAvailable = a.IsAvailable
            });
    }

    // ==========================================
    // ✅ Update My Availability
    // ==========================================
    public async Task UpdateMyAvailabilityAsync(
        List<CreateAvailabilityDto> dtos,
        string userId)
    {
        var craftsman = await _craftsmanRepository.GetByUserIdAsync(userId);
        if (craftsman == null)
            throw new Exception("Craftsman not found");

        var existingAvailabilities =
            await _availabilityRepository.GetAllByCraftsmanIdAsync(craftsman.Id);

        foreach (var dto in dtos)
        {
            if (dto.StartTime >= dto.EndTime)
                throw new Exception("Start time must be earlier than end time");

            var availability =
                existingAvailabilities.FirstOrDefault(a => a.Day == dto.Day);

            if (availability != null)
            {
                availability.From = dto.StartTime;
                availability.To = dto.EndTime;
                availability.IsAvailable = dto.IsAvailable;

                _availabilityRepository.Update(availability);
            }
            else
            {
                var newAvailability = new CraftsmanAvailability
                {
                    CraftsmanId = craftsman.Id,
                    Day = dto.Day,
                    From = dto.StartTime,
                    To = dto.EndTime,
                    IsAvailable = dto.IsAvailable
                };

                await _availabilityRepository.AddAsync(newAvailability);
            }
        }

        await _availabilityRepository.SaveAsync();
    }
}