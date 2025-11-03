using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;

public class AddStoryCategoryCommand : IRequest<string>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
