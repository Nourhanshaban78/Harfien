using Harfien.Application.DTO.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<GetCityDto>> GetAllCitiesAsync();
    }
}
