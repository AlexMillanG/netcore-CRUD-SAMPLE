using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _configuration;


    public AuthService(IUserRepository repo, IConfiguration configuration)
    {
        _repo = repo;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _repo.GetByEmailAsync(request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHashed))
        {
            return Result<AuthResponse>.Failure("invalid credentials");
        }

        var token = GenerateJwt(user);
        
        return Result<AuthResponse>.Success(new AuthResponse(token,user.email));
    }
    private string GenerateJwt(User user){
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );
            
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}