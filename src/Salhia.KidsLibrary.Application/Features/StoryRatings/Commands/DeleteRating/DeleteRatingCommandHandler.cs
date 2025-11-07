using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.DeleteRating;

public class DeleteRatingCommandHandler(
    IRepository<StoryRating> storyRatingRepository,
    IMasterStoryStatsService statsService,
    ICurrentUserService currentUserService,
    ILogger<DeleteRatingCommandHandler> logger,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteRatingCommand>
{
    public async Task Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        logger.LogInformation("User {UserId} is deleting rating for story {StoryId}", 
            currentUserId, request.MasterStoryId);

        // Find the user's rating for this story
        var predicate = PredicateBuilder.New<StoryRating>(true)
            .And(sr => sr.UserId == currentUserId)
            .And(sr => sr.MasterStoryId == request.MasterStoryId);

        var parameters = new QueryParameters<StoryRating>
        {
            Filter = predicate
        };

        var (existingRatings, _) = await storyRatingRepository.GetAllMatchingAsync(
            parameters, 
            cancellationToken);

        var existingRating = existingRatings.FirstOrDefault();

        if (existingRating == null)
        {
            throw new NotFoundException(nameof(StoryRating), 
                $"No rating found for user {currentUserId} and story {request.MasterStoryId}");
        }

        var deletedRatingValue = existingRating.Rating;
        var storyId = existingRating.MasterStoryId;

        await storyRatingRepository.DeleteAsync(existingRating);
        
        // Update stats using service
        await statsService.DecrementRatingAsync(storyId, deletedRatingValue, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Rating deleted successfully");
    }
}
