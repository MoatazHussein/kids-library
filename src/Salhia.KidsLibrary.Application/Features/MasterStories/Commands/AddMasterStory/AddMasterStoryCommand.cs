using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;

public class AddMasterStoryCommand : IRequest<string>
{
    public string StoryCategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public MediaType MediaType { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public int? PublishYear { get; set; }
    public string AuthorName { get; set; } = string.Empty;
}
