using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.DeleteCustomStoryItem;

public class DeleteCustomStoryItemCommandValidator : AbstractValidator<DeleteCustomStoryItemCommand>
{
    public DeleteCustomStoryItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Custom Story Item ID is required")
            .Length(26).WithMessage("Custom Story Item ID must be 26 characters (ULID format)");
    }
}
