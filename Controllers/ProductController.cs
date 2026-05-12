using Microsoft.AspNetCore.Mvc;
using SolucionChida.Domain.DTOs;
using SolucionChida.Services;

namespace SolucionChida.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;

    public ProductController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]

    public async Task<IActionResult> FindAllProducts()
    {
        var result = await _service.FindAll();
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> FindById(int id)
    {
        var result = await _service.FindByProductId(id);

        return result.IsSuccess ? Ok(result) : NotFound(new { error = result.Error });
    }

    [HttpGet("/sku/{sku}")]
    public async Task<IActionResult> FindBySku(string sku)
    {
        var result = await _service.FindProductBySku(sku);
        return result.IsSuccess ? Ok(result) : NotFound(new { error = result.Error });
        
    }

    [HttpDelete("/{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _service.DeleteProduct(id);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpPut("/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var result = await _service.UpdateProduct(dto, id);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] CreateProductDto dto)
    {
        var result = await _service.SaveProduct(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(new { error = result.Error });
    }
}