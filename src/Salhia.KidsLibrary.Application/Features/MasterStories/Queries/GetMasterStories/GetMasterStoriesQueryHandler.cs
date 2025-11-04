using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQueryHandler(
    IRepository<MasterStory> repository,
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

        // Filter by creator/author
        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            predicate = predicate.And(x => x.CreatedBy == request.CreatedBy);
        }

        // Filter by approval status
        if (request.IsApproved.HasValue)
        {
            predicate = predicate.And(x => x.IsApproved == request.IsApproved.Value);
        }

        // Search in title and content
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => 
                x.Title.ToLower().Contains(search) || 
                (x.Content != null && x.Content.ToLower().Contains(search)));
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
                ms => ms.Author!
            ]
        };

        var (masterStories, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var masterStoryDtos = masterStories.Select(ms => mapper.Map<GetMasterStoriesQueryResponse>(ms)).ToList();

        var result = new PagedResult<GetMasterStoriesQueryResponse>(
            masterStoryDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
