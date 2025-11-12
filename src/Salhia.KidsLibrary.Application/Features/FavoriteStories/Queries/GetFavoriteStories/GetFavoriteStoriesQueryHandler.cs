using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

public class GetFavoriteStoriesQueryHandler(
    IRepository<FavoriteStory> favoriteStoryRepository,
    ICurrentUserService currentUserService,
    ILogger<GetFavoriteStoriesQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetFavoriteStoriesQuery, PagedResult<GetFavoriteStoriesQueryResponse>>
{
    public async Task<PagedResult<GetFavoriteStoriesQueryResponse>> Handle(
        GetFavoriteStoriesQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        if (string.IsNullOrEmpty(currentUserId))
            throw new UnauthorizedAccessException("User must be authenticated");

        logger.LogInformation("Getting favorite stories for user {UserId}", currentUserId);

        var predicate = PredicateBuilder.New<FavoriteStory>(true);

        // Filter by current user
        predicate = predicate.And(fs => fs.UserId == currentUserId);

        // Search in story title and content
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(fs => 
                fs.MasterStory.Title.ToLower().Contains(search) || 
                (fs.MasterStory.Content != null && fs.MasterStory.Content.ToLower().Contains(search)));
        }

        Expression<Func<FavoriteStory, bool>> filter = predicate;

        // Sorting
        Expression<Func<FavoriteStory, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "title" => fs => fs.MasterStory.Title,
            "favoritedat" => fs => fs.CreatedAt,
            "createdat" => fs => fs.MasterStory.CreatedAt,
            _ => fs => fs.CreatedAt // Default: newest favorites first
        };

        var parameters = new QueryParameters<FavoriteStory>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
            Includes = [
                fs => fs.MasterStory,
                fs => fs.MasterStory.StoryCategory,
                fs => fs.MasterStory.Author!,
                fs => fs.MasterStory.Comments
            ]
        };

        var (favoriteStories, totalCount) = await favoriteStoryRepository.GetAllMatchingAsync(
            parameters, 
            cancellationToken);

        var favoriteDtos = favoriteStories.Select(fs => mapper.Map<GetFavoriteStoriesQueryResponse>(fs)).ToList();

        var result = new PagedResult<GetFavoriteStoriesQueryResponse>(
            favoriteDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
