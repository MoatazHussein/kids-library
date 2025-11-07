using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.AddRating;

public class AddRatingCommandHandler(
    IRepository<StoryRating> storyRatingRepository,
    IRepository<MasterStory> masterStoryRepository,
    IMasterStoryStatsService statsService,
    ICurrentUserService currentUserService,
    ILogger<AddRatingCommandHandler> logger,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddRatingCommand, AddRatingCommandResponse>
{
    public async Task<AddRatingCommandResponse> Handle(
        AddRatingCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        logger.LogInformation("User {UserId} is adding rating for story {StoryId}", 
            currentUserId, request.MasterStoryId);

        // Verify the story exists
        var storyExists = await masterStoryRepository.GetByIdAsync(request.MasterStoryId, cancellationToken);
        if (storyExists == null)
            throw new NotFoundException(nameof(MasterStory), request.MasterStoryId);

        // Check if user already rated this story
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

        if (existingRatings.Any())
        {
            throw new AppException("You have already rated this story. Use update instead.");
        }

        // Create new rating
        var storyRating = new StoryRating
        {
            UserId = currentUserId,
            MasterStoryId = request.MasterStoryId,
            Rating = request.Rating,
            CreatedBy = currentUserId,
            CreatedAt =DateTime.UtcNow
        };

        await storyRatingRepository.AddAsync(storyRating, cancellationToken);
        
        // Update stats using service
        await statsService.IncrementRatingAsync(request.MasterStoryId, request.Rating, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Rating added successfully with value {Rating}", request.Rating);

        return new AddRatingCommandResponse
        {
            RatingId = storyRating.Id,
            MasterStoryId = request.MasterStoryId,
            Rating = request.Rating,
        };
    }
}
