using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.StoryShares.Commands.ShareStory;

public class ShareStoryCommand : IRequest<Unit>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public SharePlatform Platform { get; set; }
}
