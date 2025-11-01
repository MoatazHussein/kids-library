using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty().WithMessage("Please provide a First Name")
            .Length(3, 50);

        RuleFor(dto => dto.LastName)
            .MaximumLength(50);

        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .When(dto => !string.IsNullOrEmpty(dto.PhoneNumber))
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");

    }
}