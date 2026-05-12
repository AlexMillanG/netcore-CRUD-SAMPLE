namespace SolucionChida.Domain.DTOs;

public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string Email);