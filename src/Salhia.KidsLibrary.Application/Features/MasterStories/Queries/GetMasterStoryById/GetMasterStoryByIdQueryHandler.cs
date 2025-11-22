using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQueryHandler(
    IRepository<MasterStory> masterStoryRepository,
    IRepository<StoryComment> commentRepository,
    IRepository<StoryLike> storyLikeRepository,
    IRepository<FavoriteStory> favoriteStoryRepository,
    IMasterStoryStatsService statsService,
    ICurrentUserService currentUserService,
    ILogger<GetMasterStoryByIdQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetMasterStoryByIdQuery, GetMasterStoryByIdQueryResponse>
{
    public async Task<GetMasterStoryByIdQueryResponse> Handle(
        GetMasterStoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Master Story by Id: {Id}", request.Id);

        // Get the story with related entities
        var masterStory = await masterStoryRepository.GetByIdAsync(
            request.Id, 
            cancellationToken,
            [
                ms => ms.StoryCategory,
                ms => ms.Comments,
                ms => ms.CreatedByUser!,
                ms => ms.UpdatedByUser!
            ]);

        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.Id);

        // Map story to response
        var response = mapper.Map<GetMasterStoryByIdQueryResponse>(masterStory);

        // Get paged comments for this story
        Expression<Func<StoryComment, object>> commentsOrderBy = c => c.CreatedAt;

        var commentsParameters = new QueryParameters<StoryComment>
        {
            PageNumber = request.CommentsPageNumber,
            PageSize = request.CommentsPageSize,
            Filter = c => c.MasterStoryId == request.Id,
            OrderBy = commentsOrderBy,
            Descending = true, // Newest first
            Includes = [
                c => c.CreatedByUser!,
                c => c.UpdatedByUser!
            ]
        };

        var (comments, totalCommentsCount) = await commentRepository.GetAllMatchingAsync(
            commentsParameters, 
            cancellationToken);

        var commentDtos = comments.Select(c => mapper.Map<GetStoryCommentsQueryResponse>(c)).ToList();

        var pagedComments = new PagedResult<GetStoryCommentsQueryResponse>(
            commentDtos,
            totalCommentsCount,
            request.CommentsPageSize,
            request.CommentsPageNumber);

        response.Comments = pagedComments;

        // Get rating statistics using service
        var (ratingsCount, averageRating) = await statsService.GetStoryRatingStatsAsync(request.Id, cancellationToken);
        response.RatingsCount = ratingsCount;
        response.AverageRating = averageRating;

        // Get like statistics from stats service
        response.LikesCount = await statsService.GetLikesCountAsync(request.Id, cancellationToken);

        if (currentUserService.IsAuthenticated)
        {
            var currentUserId = currentUserService.UserId;

        // Check if current user liked this story 
            response.IsLikedByCurrentUser = await storyLikeRepository.AnyAsync(
                l => l.MasterStoryId == request.Id && l.UserId == currentUserId,
                cancellationToken);

        // Check if current user added this story to favorites 
            response.IsFavoriteByCurrentUser = await favoriteStoryRepository.AnyAsync(
                fav => fav.MasterStoryId == request.Id && fav.UserId == currentUserId,
                cancellationToken);
        }

        // Get share statistics from stats service
        response.SharesCount = await statsService.GetSharesCountAsync(request.Id, cancellationToken);

        // Get view statistics from stats service
        response.TotalViews = await statsService.GetStoryViewsCountAsync(request.Id);

        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
