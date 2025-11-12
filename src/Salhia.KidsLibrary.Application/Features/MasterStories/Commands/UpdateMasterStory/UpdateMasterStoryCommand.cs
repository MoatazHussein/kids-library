using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.UpdateMasterStory;

public class UpdateMasterStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string StoryCategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public MediaType MediaType { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public int? PublishYear { get; set; }
}
