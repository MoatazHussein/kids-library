using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(dto => dto.FirstName).
        NotEmpty().WithMessage("Please provide a First Name")
        .Length(3, 50);

        RuleFor(dto => dto.LastName)
            .MaximumLength(50);

        RuleFor(dto => dto.Email)
        .EmailAddress()
        .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.Password)
           .NotEmpty().WithMessage("Password is required")
           .MinimumLength(8).WithMessage("Password must be at least 8 characters")
           .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
           .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
           .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
           .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
           .Matches(@"[!@#$%^&*(),.?"":{}|<>]").WithMessage("Password must contain at least one special character");

        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .When(dto => !string.IsNullOrEmpty(dto.PhoneNumber))
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");

    }
}