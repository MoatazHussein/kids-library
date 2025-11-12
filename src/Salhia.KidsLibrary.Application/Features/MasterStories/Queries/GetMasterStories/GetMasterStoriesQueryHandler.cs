using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQueryHandler(
    IRepository<MasterStory> repository,
    IMasterStoryStatsService statsService,
    ILogger<GetMasterStoriesQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetMasterStoriesQuery, PagedResult<GetMasterStoriesQueryResponse>>
{
    public async Task<PagedResult<GetMasterStoriesQueryResponse>> Handle(
        GetMasterStoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Master Stories");

        var predicate = PredicateBuilder.New<MasterStory>(true);

        // Filter by story category
        if (!string.IsNullOrWhiteSpace(request.StoryCategoryId))
        {
            predicate = predicate.And(x => x.StoryCategoryId == request.StoryCategoryId);
        }

        // Filter by Media Type 
        if (request.MediaType.HasValue)
        {
            predicate = predicate.And(x => x.MediaType == request.MediaType);
        }

        // Filter by approval status
        if (request.ApprovalStatus.HasValue)
        {
            predicate = predicate.And(x => x.ApprovalStatus == request.ApprovalStatus.Value);
        }
        else
        {
            // By default, only approved stories
            predicate = predicate.And(x => x.ApprovalStatus == ApprovalStatus.Approved);
        }

        // Search in title, content, author, publish year, and media items
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            var searchPredicate = PredicateBuilder.New<MasterStory>(false);
            
            searchPredicate = searchPredicate.Or(x => x.Title.ToLower().Contains(search));
            searchPredicate = searchPredicate.Or(x => x.Content != null && x.Content.ToLower().Contains(search));
            searchPredicate = searchPredicate.Or(x => x.Author.FirstName.ToLower().Contains(search));
            searchPredicate = searchPredicate.Or(x => x.Author.LastName != null && x.Author.LastName.ToLower().Contains(search));
            searchPredicate = searchPredicate.Or(x => x.PublishYear.HasValue && x.PublishYear.ToString()!.Contains(search));
            
            predicate = predicate.And(searchPredicate);
        }

        Expression<Func<MasterStory, bool>> filter = predicate;

        // Sorting
        Expression<Func<MasterStory, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "title" => ms => ms.Title,
            "createdat" => ms => ms.CreatedAt,
            "updatedat" => ms => ms.UpdatedAt ?? ms.CreatedAt,
            _ => ms => ms.CreatedAt // Default: newest first
        };

        var parameters = new QueryParameters<MasterStory>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
            Includes = [
                ms => ms.StoryCategory!,
                ms => ms.Author!,
                ms => ms.UpdatedByUser!,
                ms => ms.Comments
            ]
        };

        var (masterStories, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var masterStoryDtos = masterStories.Select(ms => mapper.Map<GetMasterStoriesQueryResponse>(ms)).ToList();

        // Get stats for all stories in this page using service
        var storyIds = masterStoryDtos.Select(ms => ms.Id).ToList();
        var statsDict = await statsService.GetMultipleStoryRatingStatsAsync(storyIds, cancellationToken);

        // Map stats to each story
        foreach (var story in masterStoryDtos)
        {
            if (statsDict.TryGetValue(story.Id, out var stats))
            {
                story.RatingsCount = stats.RatingsCount;
                story.AverageRating = stats.AverageRating;
            }
        }

        var result = new PagedResult<GetMasterStoriesQueryResponse>(
            masterStoryDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
