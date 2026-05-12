using FluentValidation;

namespace SolucionChida.Domain.DTOs;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The name is required").MinimumLength(3).WithMessage("The name must have at least 3 characters");
        RuleFor(x => x.Sku).NotEmpty().WithMessage("The Sku is required").MinimumLength(3).WithMessage("The sku must have at least 3 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The Description is required").MinimumLength(3).WithMessage("The Description must have at least 3 characters");
    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The name is required").MinimumLength(3).WithMessage("The name must have at least 3 characters");
        RuleFor(x => x.Sku).NotEmpty().WithMessage("The Sku is required").MinimumLength(3).WithMessage("The sku must have at least 3 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The Description is required").MinimumLength(3).WithMessage("The Description must have at least 3 characters");
    }
}