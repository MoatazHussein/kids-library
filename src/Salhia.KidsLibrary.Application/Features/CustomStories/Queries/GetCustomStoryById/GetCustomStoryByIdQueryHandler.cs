using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Dtos.CustomStories;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;

public class GetCustomStoryByIdQueryHandler(
    IRepository<CustomStory> customStoryRepository,
    IRepository<CustomStoryItem> customStoryItemRepository,
    ILogger<GetCustomStoryByIdQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetCustomStoryByIdQuery, GetCustomStoryByIdQueryResponse>
{
    public async Task<GetCustomStoryByIdQueryResponse> Handle(
        GetCustomStoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Custom Story by Id: {Id}", request.Id);

        // Get the story with user navigation properties
        var customStory = await customStoryRepository.GetByIdAsync(
            request.Id, 
            cancellationToken,
            [cs => cs.CreatedByUser!, cs => cs.UpdatedByUser!]);

        if (customStory == null)
            throw new NotFoundException(nameof(CustomStory), request.Id);

        // Map story to response
        var response = mapper.Map<GetCustomStoryByIdQueryResponse>(customStory);

        // Get paged items for this story
        Expression<Func<CustomStoryItem, object>>? itemsOrderBy = request.ItemsOrderBy?.ToLower() switch
        {
            "title" => item => item.Title,
            "createdat" => item => item.CreatedAt,
            "updatedat" => item => item.UpdatedAt ?? item.CreatedAt,
            _ => item => item.CreatedAt // Default: chronological order (oldest first)
        };

        var itemsParameters = new QueryParameters<CustomStoryItem>
        {
            PageNumber = request.ItemsPageNumber,
            PageSize = request.ItemsPageSize,
            Filter = item => item.CustomStoryId == request.Id,
            OrderBy = itemsOrderBy,
            Descending = request.ItemsDescending,
            Includes = [
                item => item.CreatedByUser!,
                item => item.UpdatedByUser!
            ]
        };

        var (items, totalItemsCount) = await customStoryItemRepository.GetAllMatchingAsync(
            itemsParameters, 
            cancellationToken);

        var itemDtos = items.Select(item => mapper.Map<CustomStoryItemDto>(item)).ToList();

        var pagedItems = new PagedResult<CustomStoryItemDto>(
            itemDtos,
            totalItemsCount,
            request.ItemsPageSize,
            request.ItemsPageNumber);

        response.Items = pagedItems;

        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
