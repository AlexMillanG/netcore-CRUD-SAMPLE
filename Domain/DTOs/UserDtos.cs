namespace SolucionChida.Domain.DTOs;

public record CreateUserDto(string Name, string Email);

public record UpdateUserDto(string Name, string Email);

public record UserResponseDto(int Id, string Name, string Email);