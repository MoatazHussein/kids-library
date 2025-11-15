using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryViews.Commands;

public record RegisterStoryViewCommand(string StoryId, string VisitorKey) : IRequest<Unit>;

