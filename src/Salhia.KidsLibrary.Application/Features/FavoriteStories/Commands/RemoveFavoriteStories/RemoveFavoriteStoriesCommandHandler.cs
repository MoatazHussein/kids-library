using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.RemoveFavoriteStories;

public class RemoveFavoriteStoriesCommandHandler(
    IRepository<FavoriteStory> favoriteStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<RemoveFavoriteStoriesCommandHandler> logger
    ) : IRequestHandler<RemoveFavoriteStoriesCommand, RemoveFavoriteStoriesCommandResponse>
{
    public async Task<RemoveFavoriteStoriesCommandResponse> Handle(
        RemoveFavoriteStoriesCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("User must be authenticated");

        logger.LogInformation("Removing {Count} favorite stories for user {UserId}", 
            request.MasterStoryIds.Count, currentUserId);

        var response = new RemoveFavoriteStoriesCommandResponse
        {
            TotalRequested = request.MasterStoryIds.Count
        };

        // Get distinct story IDs
        var distinctStoryIds = request.MasterStoryIds.Distinct().ToList();

        // Get existing favorites for this user
        var (existingFavoritesItems , existingFavoritesCount) = await favoriteStoryRepository.GetAllMatchingAsync(
            new QueryParameters<FavoriteStory>
            {
                Filter = fs => fs.UserId == currentUserId && distinctStoryIds.Contains(fs.MasterStoryId),
                PageSize = distinctStoryIds.Count
            },
            cancellationToken);

        // Remove favorites
        foreach (var favorite in existingFavoritesItems)
        {
            await favoriteStoryRepository.DeleteAsync(favorite);
            response.RemovedStoryIds.Add(favorite.MasterStoryId);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        response.Removed = existingFavoritesCount;
        response.NotFound = distinctStoryIds.Count - existingFavoritesCount;

        logger.LogInformation("Removed {RemovedCount} favorites, {NotFoundCount} not found",
            response.Removed, response.NotFound);

        return response;
    }
}
