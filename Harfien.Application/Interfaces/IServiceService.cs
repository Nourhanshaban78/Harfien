using Harfien.Application.DTO.Service;
using Harfien.Domain.Shared;

namespace Harfien.Application.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceReadDto> CreateServiceAsync(ServiceCreateDto dto);
        Task<ServiceReadDto> UpdateServiceAsync(int id,ServiceUpdateDto dto);
        Task<bool> DeleteServiceAsync(int id);

        Task<ServiceReadDto> GetServiceByIdAsync(int id);
        Task<IEnumerable<ServiceReadDto>> GetAllServicesAsync();
        Task<IEnumerable<ServiceReadDto>> GetServicesByCategoryAsync(int categoryId);

        Task<PagedResult<ServiceReadDto>> GetFilteredAsync(ServiceQueryDto query);
        Task<PagedResult<ServiceReadDto>> GetServicesByCraftsmanIdAsync(int craftsmanId,
    int pageNumber,
    int pageSize);

    }
}
