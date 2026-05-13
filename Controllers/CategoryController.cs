using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolucionChida.Domain.DTOs;
using SolucionChida.Services;

namespace SolucionChida.Controllers;

[Authorize]
[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        var result = await _service.FindAll();
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById(int id)
    {
        var result = await _service.FindById(id);
        return result.IsSuccess ? Ok(result) : NotFound(new { error = result.Error });
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> FindByName(string name)
    {
        var result = await _service.FindByName(name);
        return result.IsSuccess ? Ok(result) : NotFound(new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] CreateCategoryDto dto)
    {
        var result = await _service.Save(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto, int id)
    {
        var result = await _service.Update(dto, id);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
        
    }
    
    
}