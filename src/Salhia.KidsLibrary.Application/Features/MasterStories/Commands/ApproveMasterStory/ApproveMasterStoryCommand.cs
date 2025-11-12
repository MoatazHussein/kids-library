using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.ApproveMasterStory;

public class ApproveMasterStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
}
