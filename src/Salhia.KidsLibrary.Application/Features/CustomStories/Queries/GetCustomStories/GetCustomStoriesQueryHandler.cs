using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStories;

public class GetCustomStoriesQueryHandler(
    IRepository<CustomStory> repository,
    ILogger<GetCustomStoriesQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetCustomStoriesQuery, PagedResult<GetCustomStoriesQueryResponse>>
{
    public async Task<PagedResult<GetCustomStoriesQueryResponse>> Handle(
        GetCustomStoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Custom Stories");

        var predicate = PredicateBuilder.New<CustomStory>(true);

        // Filter by creator
        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            predicate = predicate.And(x => x.CreatedBy == request.CreatedBy);
        }

        // Search in title and description
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => 
                x.Title.ToLower().Contains(search) || 
                (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        Expression<Func<CustomStory, bool>> filter = predicate;

        // Sorting
        Expression<Func<CustomStory, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "title" => cs => cs.Title,
            "createdat" => cs => cs.CreatedAt,
            "updatedat" => cs => cs.UpdatedAt ?? cs.CreatedAt,
            _ => cs => cs.CreatedAt // Default: newest first
        };

        var parameters = new QueryParameters<CustomStory>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
            Includes = [
                cs => cs.CreatedByUser!,
                cs => cs.UpdatedByUser!,
                cs => cs.CustomStoryItems
            ]
        };

        var (customStories, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var customStoryDtos = customStories.Select(cs => mapper.Map<GetCustomStoriesQueryResponse>(cs)).ToList();

        var result = new PagedResult<GetCustomStoriesQueryResponse>(
            customStoryDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
