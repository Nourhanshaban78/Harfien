using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetAllByCityId(int cityId)
        {
            var areas = await _areaService.GetAllByCityIdAsync(cityId);
            return Ok(areas);
        }
    }
}
