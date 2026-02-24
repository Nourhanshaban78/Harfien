using AutoMapper;
using Harfien.Application.DTO.Service;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Harfien.Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;

        public  ServiceService(IMapper mapper,IServiceRepository serviceRepository) {
            _mapper = mapper;
           _serviceRepository = serviceRepository;
        }
        #region crud op
        public async Task<ServiceReadDto> CreateServiceAsync(ServiceCreateDto dto)
        {
             
            var service = _mapper.Map<Service>(dto);

             await _serviceRepository.AddAsync(service);

             await _serviceRepository.SaveAsync();
            var serviceWithRelations = await _serviceRepository.GetServiceByIdWithCraftData(service.Id);

            return _mapper.Map<ServiceReadDto>(serviceWithRelations);

            


        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service == null)
                return false;  

            _serviceRepository.Delete(service);
            await _serviceRepository.SaveAsync();
            return true;  
        }


       
        public async Task<ServiceReadDto> UpdateServiceAsync(int id, ServiceUpdateDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            _mapper.Map(dto, service);
            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();
            var serviceWithRelations = await _serviceRepository.GetServiceByIdWithCraftData(service.Id);

            return _mapper.Map<ServiceReadDto>(serviceWithRelations);




        }
        #endregion

        public async Task<IEnumerable<ServiceReadDto>> GetAllServicesAsync()
        {
           var services= await _serviceRepository.GetAllServicesWithCraftData();
            return _mapper.Map<List<ServiceReadDto>>(services);
        }

        public async Task<ServiceReadDto> GetServiceByIdAsync(int id)
        {
            var service = await _serviceRepository.GetServiceByIdWithCraftData(id);
            return _mapper.Map<ServiceReadDto>(service);
        }

        public async Task<IEnumerable<ServiceReadDto>> GetServicesByCategoryAsync(int categoryId)
        {
            var services = await _serviceRepository.GetServicesByCategory(categoryId);
            return _mapper.Map<List<ServiceReadDto>>(services);
        }

        public async Task<PagedResult<ServiceReadDto>> GetFilteredAsync(ServiceQueryDto query)
        {
            var result = await _serviceRepository.GetFilteredAsync(query);

            return new PagedResult<ServiceReadDto>
            {
                Items = _mapper.Map<IEnumerable<ServiceReadDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public async Task<PagedResult<ServiceReadDto>> GetServicesByCraftsmanIdAsync(int craftsmanId, int pageNumber, int pageSize)
        {
            var result = await _serviceRepository.GetServicesByCraftsmanIdAsync( craftsmanId,   pageNumber,   pageSize);

            return new PagedResult<ServiceReadDto>
            {
                Items = _mapper.Map<IEnumerable<ServiceReadDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }
    }
}
