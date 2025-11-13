using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryLikes.Commands.ToggleStoryLike;

public class ToggleStoryLikeCommandHandler(
    IRepository<StoryLike> storyLikeRepository,
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IMasterStoryStatsService statsService,
    IUnitOfWork unitOfWork,
    ILogger<ToggleStoryLikeCommandHandler> logger
    ) : IRequestHandler<ToggleStoryLikeCommand, Unit>
{
    public async Task<Unit> Handle(
        ToggleStoryLikeCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("User must be authenticated");

        // Verify story exists
        var story = await masterStoryRepository.GetByIdAsync(request.MasterStoryId, cancellationToken);
        if (story == null)
            throw new NotFoundException(nameof(MasterStory), request.MasterStoryId);

        // Check if user already liked this story
        var existingLike = await storyLikeRepository.FirstOrDefaultAsync(
            x => x.UserId == currentUserId && x.MasterStoryId == request.MasterStoryId,
            cancellationToken);

        int likesIncrement;

        if (existingLike != null)
        {
            // Already liked → Remove (unlike)
            await storyLikeRepository.DeleteAsync(existingLike);
            likesIncrement = -1; // Decrement
            logger.LogInformation("User {UserId} unliked story {StoryId}", currentUserId, request.MasterStoryId);
        }
        else
        {
            // Not liked → Add like
            var newLike = new StoryLike
            {
                UserId = currentUserId,
                MasterStoryId = request.MasterStoryId,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            await storyLikeRepository.AddAsync(newLike, cancellationToken);
            likesIncrement = 1; // Increment
            logger.LogInformation("User {UserId} liked story {StoryId}", currentUserId, request.MasterStoryId);
        }

        // Update stats
        await statsService.UpdateLikesCountAsync(request.MasterStoryId, likesIncrement, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
