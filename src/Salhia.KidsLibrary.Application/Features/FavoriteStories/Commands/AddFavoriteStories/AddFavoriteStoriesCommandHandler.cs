using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.AddFavoriteStories;

public class AddFavoriteStoriesCommandHandler(
    IRepository<FavoriteStory> favoriteStoryRepository,
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<AddFavoriteStoriesCommandHandler> logger
    ) : IRequestHandler<AddFavoriteStoriesCommand, AddFavoriteStoriesCommandResponse>
{
    public async Task<AddFavoriteStoriesCommandResponse> Handle(
        AddFavoriteStoriesCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("User must be authenticated");

        logger.LogInformation("Adding {Count} favorite stories for user {UserId}", 
            request.MasterStoryIds.Count, currentUserId);

        var response = new AddFavoriteStoriesCommandResponse
        {
            TotalRequested = request.MasterStoryIds.Count
        };

        // Get distinct story IDs
        var distinctStoryIds = request.MasterStoryIds.Distinct().ToList();

        // Verify all stories exist
        var (existingStories , existingStoriesCount) = await masterStoryRepository.GetAllMatchingAsync(
            new QueryParameters<MasterStory>
            {
                Filter = ms => distinctStoryIds.Contains(ms.Id),
                PageSize = distinctStoryIds.Count
            },
            cancellationToken);

        if (existingStoriesCount != distinctStoryIds.Count)
        {
            var foundIds = existingStories.Select(s => s.Id).ToList();
            var notFoundIds = distinctStoryIds.Except(foundIds).ToList();
            throw new NotFoundException(nameof(MasterStory) , $"{string.Join(", ", notFoundIds)}");
        }

        // Get existing favorites for this user
        var (existingFavoritesItems, existingFavoritesCount) = await favoriteStoryRepository.GetAllMatchingAsync(
            new QueryParameters<FavoriteStory>
            {
                Filter = fs => fs.UserId == currentUserId && distinctStoryIds.Contains(fs.MasterStoryId),
                PageSize = distinctStoryIds.Count
            },
            cancellationToken);

        var alreadyFavoritedIds = existingFavoritesItems.Select(fs => fs.MasterStoryId).ToList();
        var newStoryIds = distinctStoryIds.Except(alreadyFavoritedIds).ToList();

        // Add new favorites
        foreach (var storyId in newStoryIds)
        {
            var favoriteStory = new FavoriteStory
            {
                UserId = currentUserId,
                MasterStoryId = storyId,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            await favoriteStoryRepository.AddAsync(favoriteStory, cancellationToken);
            response.AddedStoryIds.Add(storyId);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        response.NewlyAdded = newStoryIds.Count;
        response.AlreadyFavorited = alreadyFavoritedIds.Count;

        logger.LogInformation("Added {NewCount} new favorites, {ExistingCount} already favorited",
            response.NewlyAdded, response.AlreadyFavorited);

        return response;
    }
}
