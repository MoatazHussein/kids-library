using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;

public class GetStoryCategoriesQueryHandler(
    IRepository<StoryCategory> repository,
    ILogger<GetStoryCategoriesQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetStoryCategoriesQuery, PagedResult<GetStoryCategoriesQueryResponse>>
{
    public async Task<PagedResult<GetStoryCategoriesQueryResponse>> Handle(
        GetStoryCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Story Categories");

        var predicate = PredicateBuilder.New<StoryCategory>(true);

        // Search in title and description
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => 
                x.Title.ToLower().Contains(search) || 
                (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        Expression<Func<StoryCategory, bool>> filter = predicate;

        // Sorting
        Expression<Func<StoryCategory, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "title" => sc => sc.Title,
            "createdat" => sc => sc.CreatedAt,
            "updatedat" => sc => sc.UpdatedAt ?? sc.CreatedAt,
            _ => sc => sc.CreatedAt // Default: newest first
        };

        var parameters = new QueryParameters<StoryCategory>
        {
            PageNumber = request.PageNumber,
            Includes = [e=> e.MasterStories],
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending
        };

        var (storyCategories, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var storyCategoryDtos = storyCategories.Select(sc => mapper.Map<GetStoryCategoriesQueryResponse>(sc)).ToList();

        var result = new PagedResult<GetStoryCategoriesQueryResponse>(
            storyCategoryDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
