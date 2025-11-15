using MediatR;
using Salhia.KidsLibrary.Application.Services.StoryViewService;

namespace Salhia.KidsLibrary.Application.Features.StoryViews.Commands;

public class RegisterStoryViewCommandHandler(IStoryViewService storyViewService)
              : IRequestHandler<RegisterStoryViewCommand, Unit>
{
    public async Task<Unit> Handle(RegisterStoryViewCommand request, CancellationToken cancellationToken)
    {
        await storyViewService.RegisterViewAsync(
            request.StoryId,
            request.VisitorKey,
            cancellationToken);

        return Unit.Value;
    }
}