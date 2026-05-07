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
}