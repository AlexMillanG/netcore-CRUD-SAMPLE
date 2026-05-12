using Microsoft.AspNetCore.Mvc;
using SolucionChida.Domain.DTOs;
using SolucionChida.Services;

namespace SolucionChida.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service) =>
    _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await _service.Login(req);
        return result.IsSuccess ? Ok(result) : Unauthorized(result.Error);
    }
    
    
}