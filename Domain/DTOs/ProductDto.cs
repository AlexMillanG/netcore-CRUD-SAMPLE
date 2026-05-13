namespace SolucionChida.Domain.DTOs;

public record CreateProductDto(string Name, string Description, string Sku, int CategoryId);

public record UpdateProductDto(string Name, string Description, string Sku, int CategoryId);

public record ProductResponseDto(int Id, string Name, string Description, string Sku, int CategoryId, string CategoryName);
