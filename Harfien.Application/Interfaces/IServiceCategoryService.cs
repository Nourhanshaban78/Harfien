using Harfien.Application.DTO.ServiceCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface IServiceCategoryService
    {
        Task<IEnumerable<ServiceCategoryDto>> GetAllAsync();
        Task<ServiceCategoryDto> GetByIdAsync(int id);
        Task AddAsync(AddServiceCategoryDto dto);
        Task UpdateAsync(ServiceCategoryDto entity);
        Task DeleteAsync(int id);
    }

}
