using Harfien.Application.DTO;
using Harfien.Application.DTO.Profile_Craftman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface ICraftsmanService
    {
        Task<List<CraftsmanDto>> GetAllAsync();
        Task<CraftsmanProfileDto?> GetProfileAsync(int id);
        Task<GetMyProfileDto?> GetMyProfileAsync(string userId);
        Task UpdateMyProfileAsync(string userId, UpdateMyProfileDto dto);
    }
}
