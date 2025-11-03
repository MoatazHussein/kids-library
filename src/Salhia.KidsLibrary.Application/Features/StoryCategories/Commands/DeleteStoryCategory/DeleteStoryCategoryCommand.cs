using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.DeleteStoryCategory;

public class DeleteStoryCategoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
