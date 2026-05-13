using FluentValidation;

namespace SolucionChida.Domain.DTOs;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The category must have a name");
        RuleFor(x => x.Name).MinimumLength(3).WithMessage("The minimum length of the category must be 3");
    }
}