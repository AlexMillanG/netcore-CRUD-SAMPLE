using System.ComponentModel.DataAnnotations;
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

    public UserController(UserService service) =>
        _service = service;

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

    [HttpGet("{email}")]
    public async Task<IActionResult> FindByEmail(string email)
    {
        var result = await _service.FindByEmail(email);
        return result.IsSuccess
            ? Ok( result.Value)
            : BadRequest(new { error = result.Error });

    }
}