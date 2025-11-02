using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.DeleteCustomStoryItem;

public class DeleteCustomStoryItemCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
