using FluentValidation;
using Microsoft.OpenApi.Any;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class UserService
{
    private readonly IUserRepository _repo;
    private readonly IRoleRepository _roleRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateUserDto> _validator;
    private readonly IValidator<UpdateUserDto> _updateValidator;

    public UserService(
        IRoleRepository roleRepo,
        IUserRepository repo,
        IUnitOfWork unitOfWork,
        IValidator<CreateUserDto> validator,
        IValidator<UpdateUserDto> updateValidator)
    {
        _roleRepo = roleRepo;
        _repo = repo;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _updateValidator = updateValidator;
    }
    
    public async Task<Result<List<UserResponseDto>>>FindAllUsers()
    {
        var users = await _repo.FindAll();

        var dtos = users.Select(u => new UserResponseDto(
            u.Id,
            u.name,
            u.email,
            u.Roles
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
                user.email,
                user.Roles
            );
        
        return Result<UserResponseDto>.Success(dto);
    }
    
    public async Task<Result<UserResponseDto>> FindById(int id)
    {
        var user = await _repo.GetByIdAsync(id);

        if (user == null)
        {
            return Result<UserResponseDto>.Failure("El usuario no existe.");
        }
        
        var dto = new UserResponseDto(
            user.Id,
            user.name,
            user.email,
            user.Roles
        );
        
        return Result<UserResponseDto>.Success(dto);
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

        var userRoles = await _roleRepo.GetByIdsAsync(dto.RolesId);
        if (userRoles.Count != dto.RolesId.Count)
            return Result<UserResponseDto>.Failure("One or more roles are invalid.");

        var user = new User
        {
            email = dto.Email,
            name = dto.Name,
            CreatedAt = DateTime.UtcNow,
            Roles = userRoles,
            PasswordHashed = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _repo.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<UserResponseDto>.Success(new UserResponseDto(user.Id,user.name,user.email,user.Roles));
    }

    public async Task<Result<UserResponseDto>> UpdateUserAsync(UpdateUserDto dto, int id)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<UserResponseDto>.Failure(validation.Errors.First().ErrorMessage);

        var user = await _repo.GetByIdAsync(id);
        if (user == null)
            return Result<UserResponseDto>.Failure("User not found.");
        
        var emailOwner = await _repo.GetByEmailAsync(dto.Email);
        if (emailOwner != null && emailOwner.Id != id)
            return Result<UserResponseDto>.Failure("This email is already taken.");

        user.name = dto.Name;
        user.email = dto.Email;

        await _repo.UpdateAsync(user, id);
        await _unitOfWork.SaveChangesAsync();

        return Result<UserResponseDto>.Success(new UserResponseDto(user.Id, user.name, user.email,user.Roles));
    }

    public async Task<Result<UserResponseDto>> DeleteAsync(int id)
    {
        var found = await _repo.GetByIdAsync(id);

        if (found == null)
        {
            return Result<UserResponseDto>.Failure("El usuario no existe.");
        }

        var response = new UserResponseDto (found.Id, found.email, found.name, found.Roles);
        

        await _repo.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Result<UserResponseDto>.Success(response);
    }
    
}