using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required")
            .Length(26).WithMessage("User ID must be 26 characters (ULID format)");
    }
}
