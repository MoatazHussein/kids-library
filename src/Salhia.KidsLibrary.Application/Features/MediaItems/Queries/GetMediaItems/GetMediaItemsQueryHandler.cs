using System.Linq.Expressions;
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQueryHandler(
    IRepository<MediaItem> repository,
    ILogger<GetMediaItemsQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetMediaItemsQuery, PagedResult<GetMediaItemsQueryResponse>>
{
    public async Task<PagedResult<GetMediaItemsQueryResponse>> Handle(
        GetMediaItemsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Media Items for MasterStory: {MasterStoryId}", request.MasterStoryId);

        var predicate = PredicateBuilder.New<MediaItem>(true);

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
                x.Title.ToLower().Contains(search) || 
                (x.Description != null && x.Description.ToLower().Contains(search)));
        }

        Expression<Func<MediaItem, bool>> filter = predicate;

        // Sorting by CreatedAt (newest first by default)
        Expression<Func<MediaItem, object>> orderBy = mi => mi.CreatedAt;

        var parameters = new QueryParameters<MediaItem>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = true, // Newest first
            Includes = [
                mi => mi.CreatedByUser!,
                mi => mi.UpdatedByUser!
            ]
        };

        var (mediaItems, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var mediaItemDtos = mediaItems.Select(mi => mapper.Map<GetMediaItemsQueryResponse>(mi)).ToList();

        var result = new PagedResult<GetMediaItemsQueryResponse>(
            mediaItemDtos, 
            totalCount, 
            request.PageSize, 
            request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
