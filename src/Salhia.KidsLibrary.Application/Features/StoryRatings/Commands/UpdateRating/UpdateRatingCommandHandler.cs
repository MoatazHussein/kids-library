using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.UpdateRating;

public class UpdateRatingCommandHandler(
    IRepository<StoryRating> storyRatingRepository,
    IMasterStoryStatsService statsService,
    ICurrentUserService currentUserService,
    ILogger<UpdateRatingCommandHandler> logger,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateRatingCommand, UpdateRatingCommandResponse>
{
    public async Task<UpdateRatingCommandResponse> Handle(
        UpdateRatingCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        logger.LogInformation("User {UserId} is updating rating for story {StoryId}", 
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
                $"No rating found for story {request.MasterStoryId}. Use add instead.");
        }

        var previousRating = existingRating.Rating;
        existingRating.Rating = request.Rating;
        existingRating.UpdatedAt = DateTime.UtcNow;
        existingRating.UpdatedBy = currentUserId;

        await storyRatingRepository.UpdateAsync(existingRating);
        
        // Update stats using service
        await statsService.UpdateRatingAsync(request.MasterStoryId, previousRating, request.Rating, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Rating updated from {PreviousRating} to {NewRating}", 
            previousRating, request.Rating);

        return new UpdateRatingCommandResponse
        {
            RatingId = existingRating.Id,
            MasterStoryId = request.MasterStoryId,
            NewRating = request.Rating,
            PreviousRating = previousRating,
        };
    }
}
