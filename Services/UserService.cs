using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class UserService
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserDto> _validator;
    
    //constructor de toda la vida
    public UserService(IUserRepository repo, IUnitOfWork unitOfWork, IValidator<CreateUserDto> validator)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            return Result<UserResponseDto>.Failure(validation.Errors.First().ErrorMessage);
        }

        if (await _repo.GetByEmailAsync(dto.Email) != null)
        {
            return Result<UserResponseDto>.Failure("this email is already taken");
        }

        var user = new User
        {
            email = dto.Email,
            name = dto.Name,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<UserResponseDto>.Success(new UserResponseDto(user.Id,user.name,user.email));
    }

    public async Task<Result<List<UserResponseDto>>>FindAllUsers()
    {
        var users = await _repo.FindAll();

        var dtos = users.Select(u => new UserResponseDto(
            u.Id,
            u.name,
            u.email
        )).ToList();
        
        return Result<List<UserResponseDto>>.Success(dtos);
    }

    public async Task<Result<UserResponseDto>> FindByEmail(string email)
    {
        var user = await _repo.GetByEmailAsync(email);

        if (user == null)
        {
            return Result<UserResponseDto>.Failure("El usuario no existe.");
        }
        
        var dto = new UserResponseDto(
                user.Id,
                user.name,
                user.email
            );
        
        return Result<UserResponseDto>.Success(dto);
    }
    
}