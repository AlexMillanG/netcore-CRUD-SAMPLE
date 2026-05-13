namespace SolucionChida.Domain.DTOs;

public record CreateCategoryDto(string Name);
public record ResponseCategoryDto(int Id, string Name);
public record UpdateCategoryDto(int Id, string Name);
