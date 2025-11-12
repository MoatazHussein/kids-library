using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;

public class GetStoryCommentsQueryHandler(
    IRepository<StoryComment> repository,
    ILogger<GetStoryCommentsQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetStoryCommentsQuery, PagedResult<GetStoryCommentsQueryResponse>>
{
    public async Task<PagedResult<GetStoryCommentsQueryResponse>> Handle(
        GetStoryCommentsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Story Comments");

        var predicate = PredicateBuilder.New<StoryComment>(true);

        // Filter by MasterStoryId
        if (!string.IsNullOrWhiteSpace(request.MasterStoryId))
        {
            predicate = predicate.And(x => x.MasterStoryId == request.MasterStoryId);
        }

        // Search in title and description
        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => 
                x.Content.ToLower().Contains(search));
        }

        Expression<Func<StoryComment, bool>> filter = predicate;

        // Sorting
        Expression<Func<StoryComment, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "content" => sc => sc.Content,
            "createdat" => sc => sc.CreatedAt,
            "updatedat" => sc => sc.UpdatedAt ?? sc.CreatedAt,
            _ => sc => sc.CreatedAt // Default: newest first
        };

        var parameters = new QueryParameters<StoryComment>
        {
            PageNumber = request.PageNumber,
            Includes = [sc=>sc.CreatedByUser , sc => sc.UpdatedByUser!],
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending
        };

        var (storyComments, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var storyCommentDtos = storyComments.Select(sc => mapper.Map<GetStoryCommentsQueryResponse>(sc)).ToList();

        var result = new PagedResult<GetStoryCommentsQueryResponse>(
            storyCommentDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
