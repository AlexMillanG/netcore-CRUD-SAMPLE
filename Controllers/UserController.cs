using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucionChida.Domain.DTOs;
using SolucionChida.Services;

namespace SolucionChida.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service) =>
        _service = service;

    [Authorize(Roles = "ADMIN,USER")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _service.CreateUserAsync(dto);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        var result = await _service.FindAllUsers();
        return result.IsSuccess
            ? Ok( result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> FindByEmail(string email)
    {
        var result = await _service.FindByEmail(email);
        return result.IsSuccess
            ? Ok( result.Value)
            : BadRequest(new { error = result.Error });

    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> FindByEmail(int id)
    {
        var result = await _service.FindById(id);
        return result.IsSuccess
            ? Ok( result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await _service.UpdateUserAsync(dto, id);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }
}