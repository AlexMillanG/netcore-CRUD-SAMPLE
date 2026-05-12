namespace SolucionChida.Domain.DTOs;

public record CreateProductDto(string Name, string Description, string Sku);

public record UpdateProductDto(string Name, string Description, string Sku);

public record ProductResponseDto(int Id, string Name, string Description, string Sku);
