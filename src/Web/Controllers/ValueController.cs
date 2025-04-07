using Application.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("/api/values")]
public class ValueController(IValueService valueService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetValues()
    {
        var values = await valueService.GetValuesAsync();
        return Ok(values);
    }
    
    [HttpPut]
    public async Task<IActionResult> SaveValue([FromBody] ReplaceValuesRequestDto dto)
    {
        await valueService.ReplaceValuesAsync(dto);
        return Ok();
    }
}