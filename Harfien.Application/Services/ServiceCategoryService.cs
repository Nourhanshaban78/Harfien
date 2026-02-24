using Harfien.Application.DTO.ServiceCategory;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class ServiceCategoryService : IServiceCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServiceCategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.ServiceCategories.GetAllAsync();

            return categories.Select(c => new ServiceCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                Description = c.Description
            });
        }

        public async Task<ServiceCategoryDto> GetByIdAsync(int id)
        {
            var c = await _unitOfWork.ServiceCategories.GetByIdAsync(id);

            if (c == null) return null;

            return new ServiceCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type,
                Description = c.Description
            };
        }

        public async Task AddAsync(AddServiceCategoryDto dto)
        {
            var entity = new ServiceCategory
            {
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ServiceCategories.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }
       public async Task UpdateAsync(ServiceCategoryDto entity)
       {
            var serviceCategory = await _unitOfWork.ServiceCategories.GetByIdAsync(entity.Id);
            if (serviceCategory == null)
                throw new Exception("Service Category Not Found");

            serviceCategory.Name = entity.Name;
            serviceCategory.Type = entity.Type;
            serviceCategory.Description = entity.Description;

           _unitOfWork.ServiceCategories.Update(serviceCategory);
           await _unitOfWork.SaveAsync();   

        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.ServiceCategories.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Service Category Not Found");

           _unitOfWork.ServiceCategories.Delete(entity);     
           
           await _unitOfWork.SaveAsync();   


        }
    }

}
