using Harfien.Application.DTO.Area;

namespace Harfien.Application.Interfaces
{
    public interface IAreaService
    {
        Task<IEnumerable<GetAreaDto>> GetAllByCityIdAsync(int cityId);
    }
}
