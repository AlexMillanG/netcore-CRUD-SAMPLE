using FluentValidation;
using SolucionChida.Common;
using SolucionChida.Domain.DTOs;
using SolucionChida.Domain.Entities;
using SolucionChida.Domain.Interfaces;

namespace SolucionChida.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProductDto> _validator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductService(IProductRepository repo, IUnitOfWork unitOfWork, IValidator<CreateProductDto> validator, IValidator<UpdateProductDto> updateValidator)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<List<Product>>> FindAll()
    {
        var products = await _repo.GetAllProductAsync();
        return Result<List<Product>>.Success(products);
    }

    public async Task<Result<ProductResponseDto>> SaveProduct(CreateProductDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductResponseDto>.Failure(validation.Errors.First().ErrorMessage);

        var found = await _repo.GeProductBySkuAsync(dto.Sku);

        if (found != null)
        {
            return Result<ProductResponseDto>.Failure("The Sku must be unique");
        }

        var product = new Product
        {
            Name  = dto.Name,
            Sku = dto.Sku,
            Description = dto.Description
        };

        await _repo.SaveProductAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return Result<ProductResponseDto>.Success(new ProductResponseDto(product.Id,product.Name, product.Description,product.Sku));
    }

    public async Task<Result<ProductResponseDto>> FindByProductId(int id)
    {
        var found = await _repo.GetProductByIdAsync(id);

        if (found == null)
        {
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");
        }

        var product = new ProductResponseDto(
            found.Id,
            found.Name,
            found.Description,
            found.Sku
        );
        
        return Result<ProductResponseDto>.Success(product);
    }
    
    public async Task<Result<ProductResponseDto>> FindProductBySku(string sku)
    {
        var found = await _repo.GeProductBySkuAsync(sku);

        if (found == null)
        {
            return Result<ProductResponseDto>.Failure($"product with sku {sku} not found");
        }

        var product = new ProductResponseDto(
            found.Id,
            found.Name,
            found.Description,
            found.Sku
        );
        
        return Result<ProductResponseDto>.Success(product);
    }

    public async Task<Result<ProductResponseDto>> DeleteProduct(int id)
    {
        var found = await _repo.GetProductByIdAsync(id);
        
        if (found == null)
        {
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");
        }

        var product = new ProductResponseDto(
            found.Id,
            found.Name,
            found.Description,
            found.Sku
            );
        
        await _repo.DeleteProductAsync(id);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<ProductResponseDto>.Success(product);
    }

    public async Task<Result<ProductResponseDto>> UpdateProduct(UpdateProductDto dto, int id)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<ProductResponseDto>.Failure(validation.Errors.First().ErrorMessage);

        var found = await _repo.GetProductByIdAsync(id);

        if (found == null)
        {
            return Result<ProductResponseDto>.Failure($"product with id {id} not found");
        }

        found.Name = dto.Name;
        found.Sku = dto.Sku;
        found.Description = dto.Description;

        await _repo.UpdateProductAsync(found, id);
        await _unitOfWork.SaveChangesAsync();

        var response = new ProductResponseDto(found.Id, found.Name, found.Description, found.Sku);

        return Result<ProductResponseDto>.Success(response);
    }
    
}