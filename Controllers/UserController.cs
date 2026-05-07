using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SolucionChida.Domain.DTOs;
using SolucionChida.Services;

namespace SolucionChida.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _service.CreateUserAsync(dto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), result.Value)
            : BadRequest(new { error = result.Error });
    }
}