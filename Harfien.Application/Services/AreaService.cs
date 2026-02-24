using Harfien.Application.DTO.Area;
using Harfien.Application.DTO.City;
using Harfien.Application.Interfaces;
using Harfien.Domain.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        public async Task<IEnumerable<GetAreaDto>> GetAllByCityIdAsync(int cityId)
        {
            var areas = await _areaRepository.GetAllByCityIdAsync(cityId);
            return areas.Select(c => new GetAreaDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }
    }
}
