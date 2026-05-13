using FluentValidation;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProductDto> _validator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductService(
        IProductRepository repo,
        ICategoryRepository categoryRepo,
        IUnitOfWork unitOfWork,
        IValidator<CreateProductDto> validator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<List<ProductResponseDto>>> FindAll()
    {
        var products = await _repo.GetAllProductAsync();

        List<ProductResponseDto> dtos = new List<ProductResponseDto>();
        
        foreach (var product in products)
        {
            var dto = new ProductResponseDto(product.Id, product.Name, product.Description, product.Sku,
                product.CategoryId, product.Category.Name);
            
            dtos.Add(dto);
        }
        
        return Result<List<ProductResponseDto>>.Success(dtos);
    }

    public async Task<Result<ProductResponseDto>> SaveProduct(CreateProductDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductResponseDto>.Failure(validation.Errors.First().ErrorMessage);

        var skuExists = await _repo.GeProductBySkuAsync(dto.Sku);
        if (skuExists != null)
            return Result<ProductResponseDto>.Failure("The Sku must be unique");

        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Result<ProductResponseDto>.Failure("Category not found");

        var product = new Product
        {
            Name = dto.Name,
            Sku = dto.Sku,
            Description = dto.Description,
            CategoryId = dto.CategoryId
        };

        await _repo.SaveProductAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(
            new ProductResponseDto(product.Id, product.Name, product.Description, product.Sku, category.Id, category.Name));
    }

    public async Task<Result<ProductResponseDto>> FindByProductId(int id)
    {
        var found = await _repo.GetProductByIdAsync(id);

        if (found == null)
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");

        return Result<ProductResponseDto>.Success(
            new ProductResponseDto(found.Id, found.Name, found.Description, found.Sku, found.CategoryId, found.Category.Name));
    }

    public async Task<Result<ProductResponseDto>> FindProductBySku(string sku)
    {
        var found = await _repo.GeProductBySkuAsync(sku);

        if (found == null)
            return Result<ProductResponseDto>.Failure($"product with sku {sku} not found");

        return Result<ProductResponseDto>.Success(
            new ProductResponseDto(found.Id, found.Name, found.Description, found.Sku, found.CategoryId, found.Category.Name));
    }

    public async Task<Result<ProductResponseDto>> DeleteProduct(int id)
    {
        var found = await _repo.GetProductByIdAsync(id);

        if (found == null)
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");

        var response = new ProductResponseDto(found.Id, found.Name, found.Description, found.Sku, found.CategoryId, found.Category.Name);

        await _repo.DeleteProductAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(response);
    }

    public async Task<Result<ProductResponseDto>> UpdateProduct(UpdateProductDto dto, int id)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductResponseDto>.Failure(validation.Errors.First().ErrorMessage);

        var found = await _repo.GetProductByIdAsync(id);
        if (found == null)
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");

        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Result<ProductResponseDto>.Failure("Category not found");

        found.Name = dto.Name;
        found.Sku = dto.Sku;
        found.Description = dto.Description;
        found.CategoryId = dto.CategoryId;

        await _repo.UpdateProductAsync(found, id);
        await _unitOfWork.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(
            new ProductResponseDto(found.Id, found.Name, found.Description, found.Sku, category.Id, category.Name));
    }

    public async Task<Result<List<ProductResponseDto>>> GetByCategory(int categoryId)
    {
        var foundCategory = await _categoryRepo.GetByIdAsync(categoryId);
        if (foundCategory == null)
        {
            return Result<List<ProductResponseDto>>.Failure($"category with {categoryId} not found");
        }

        var products = await _repo.GetByCategoryId(categoryId);

        List<ProductResponseDto> dtos = new List<ProductResponseDto>();
        
        foreach (var product in products)
        {
            var dto = new ProductResponseDto(product.Id, product.Name, product.Description, product.Sku,
                product.CategoryId, product.Category.Name);
            dtos.Add(dto);
        }
        
        return Result<List<ProductResponseDto>>.Success(dtos);
    }
}
