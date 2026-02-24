using Harfien.Application.DTO.Profile_Craftman;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CraftsmenController : ControllerBase
    {
        private readonly ICraftsmanService _service;

        public CraftsmenController(ICraftsmanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var result = await _service.GetProfileAsync(id);

            if (result == null)
                return NotFound("Craftsman not found");

            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _service.GetMyProfileAsync(userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("me")]
        [Authorize(Roles = "Craftsman")]
        public async Task<IActionResult> UpdateMyProfile(UpdateMyProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _service.UpdateMyProfileAsync(userId, dto);

            return NoContent();
        }


    }
}
