using Harfien.Domain.Entities;

namespace Harfien.Domain.Shared.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<City?> GetByIdAsync(int id);
    }
}