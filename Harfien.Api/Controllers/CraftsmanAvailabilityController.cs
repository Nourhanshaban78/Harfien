using Harfien.Application.DTO.CraftsmanAvailiability;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CraftsmanAvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;

    public CraftsmanAvailabilityController(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }
    /*
    [HttpGet]
    [Authorize(Roles = "Craftsman")]
    public async Task<IActionResult> GetMyAvailability()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var availability = await _availabilityService.GetMyAvailabilityAsync(userId);
        return Ok(availability);
    }*/
    [HttpGet("{craftsmanId}")]
    [Authorize(Roles = "Client,Craftsman")]
    public async Task<IActionResult> GetCraftsmanAvailability(int craftsmanId)
    {
        var availability = await _availabilityService.GetCraftsmanAvailabilityAsync(craftsmanId);
        return Ok(availability);
    }

    [HttpPost]
    [Authorize(Roles = "Craftsman")]
    public async Task<IActionResult> AddAvailability([FromBody] CreateAvailabilityDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
         await _availabilityService.AddAvailabilityAsync(dto, userId);

        return Ok("Availability added successfully");
    
       
    }

    [HttpPut]
   // [Authorize(Roles = "Craftsman")]
    public async Task<IActionResult> UpdateAvailability([FromBody] List<CreateAvailabilityDto> dtos)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        await _availabilityService.UpdateMyAvailabilityAsync(dtos, userId);
        return Ok("Availability Update successfully");
    }
}

