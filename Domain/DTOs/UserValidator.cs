using FluentValidation;

namespace SolucionChida.Domain.DTOs;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name required")
            .MinimumLength(3).WithMessage("Minimum lenght is 3");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid Email Address");
    }
}