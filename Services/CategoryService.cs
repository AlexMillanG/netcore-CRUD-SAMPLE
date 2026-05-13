using FluentValidation;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryDto> _validator;

    public CategoryService(ICategoryRepository repo, IUnitOfWork unitOfWork, IValidator<CreateCategoryDto> validator)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<List<Category>>> FindAll()
    { 
        return Result<List<Category>>.Success(await _repo.FindAllAsync());
    }

    public async Task<Result<ResponseCategoryDto>> Save(CreateCategoryDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);

        if (!validation.IsValid)
        {
            return Result<ResponseCategoryDto>.Failure(validation.Errors.First().ErrorMessage);
        }

        var found = await _repo.GetByNameAsync(dto.Name);

        if (found != null)
        {
            return Result<ResponseCategoryDto>.Failure("there's a existing category with that name");
        }

        var category = new Category{
            Name = dto.Name
        };

        await _repo.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var response = new ResponseCategoryDto
        (
            category.Id,
            category.Name
        );
        
        return Result<ResponseCategoryDto>.Success(response);

    }

    public async Task<Result<ResponseCategoryDto>> FindById(int id)
    {
        var found = await _repo.GetByIdAsync(id);
        if (found == null)
        {
            return Result<ResponseCategoryDto>.Failure($"category with id {id} not found");
        }

        var response = new ResponseCategoryDto(
            found.Id,
            found.Name
        );
        
        return Result<ResponseCategoryDto>.Success(response);
    }

    public async Task<Result<ResponseCategoryDto>> FindByName(string name)
    {
        var found = await _repo.GetByNameAsync(name);
        if (found == null)
        {
            return Result<ResponseCategoryDto>.Failure($"category with name {name} not found");
        }

        var response = new ResponseCategoryDto(
            found.Id,
            found.Name
        );
        
        return Result<ResponseCategoryDto>.Success(response);
    }

    public async Task<Result<ResponseCategoryDto>> Update(UpdateCategoryDto dto, int id)
    {
        var found = await _repo.GetByIdAsync(id);

        if (found == null)
        {
            return Result<ResponseCategoryDto>.Failure($"category with id {id} not found");
        }

        found.Name = dto.Name;

        await _unitOfWork.SaveChangesAsync();

        return Result<ResponseCategoryDto>.Success(
            new ResponseCategoryDto(found.Id, found.Name)
        );
    }

    public async Task<Result<ResponseCategoryDto>> Delete(int id)
    {
        var found = await _repo.GetByIdAsync(id);
        if (found == null)
        {
            return Result<ResponseCategoryDto>.Failure($"category with id {id} not found");
        }

        var response = new ResponseCategoryDto(found.Id, found.Name);

        await _repo.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<ResponseCategoryDto>.Success(response);
    }
    
    

}