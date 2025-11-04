using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQueryHandler(
    IRepository<MasterStory> masterStoryRepository,
    IRepository<MediaItem> mediaItemRepository,
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
                ms => ms.Author!,
                ms => ms.UpdatedByUser!
            ]);

        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.Id);

        // Map story to response
        var response = mapper.Map<GetMasterStoryByIdQueryResponse>(masterStory);

        // Get paged media items for this story
        Expression<Func<MediaItem, object>> mediaItemsOrderBy = mi => mi.CreatedAt;

        var mediaItemsParameters = new QueryParameters<MediaItem>
        {
            PageNumber = request.MediaItemsPageNumber,
            PageSize = request.MediaItemsPageSize,
            Filter = mi => mi.MasterStoryId == request.Id,
            OrderBy = mediaItemsOrderBy,
            Descending = true, // Newest first
            Includes = [
                mi => mi.CreatedByUser!,
                mi => mi.UpdatedByUser!
            ]
        };

        var (mediaItems, totalMediaItemsCount) = await mediaItemRepository.GetAllMatchingAsync(
            mediaItemsParameters, 
            cancellationToken);

        var mediaItemDtos = mediaItems.Select(mi => mapper.Map<GetMediaItemsQueryResponse>(mi)).ToList();

        var pagedMediaItems = new PagedResult<GetMediaItemsQueryResponse>(
            mediaItemDtos,
            totalMediaItemsCount,
            request.MediaItemsPageSize,
            request.MediaItemsPageNumber);

        response.MediaItems = pagedMediaItems;

        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
