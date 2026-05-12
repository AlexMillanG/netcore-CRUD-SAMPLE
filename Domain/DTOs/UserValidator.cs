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
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password required").MinimumLength(6).WithMessage("minimum length is 6");
    }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Minimum length is 3");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
    }
}