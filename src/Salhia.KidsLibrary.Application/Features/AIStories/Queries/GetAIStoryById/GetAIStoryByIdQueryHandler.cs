using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Dtos.AIStories;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;

public class GetAIStoryByIdQueryHandler(
    IRepository<AIStory> aiStoryRepository,
    IRepository<AIStorySlide> aiStorySlideRepository,
    ILogger<GetAIStoryByIdQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetAIStoryByIdQuery, GetAIStoryByIdQueryResponse>
{
    public async Task<GetAIStoryByIdQueryResponse> Handle(
        GetAIStoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting AI Story by Id: {Id}", request.Id);

        // Get the story with user navigation properties
        var aiStory = await aiStoryRepository.GetByIdAsync(
            request.Id, 
            cancellationToken,
            [s => s.CreatedByUser!, s => s.UpdatedByUser!]);

        if (aiStory == null)
            throw new NotFoundException(nameof(AIStory), request.Id);

        // Map story to response
        var response = mapper.Map<GetAIStoryByIdQueryResponse>(aiStory);

        // Get paged slides for this story
        Expression<Func<AIStorySlide, object>>? slidesOrderBy = request.SlidesOrderBy?.ToLower() switch
        {
            "index" => slide => slide.Index,
            "title" => slide => slide.Title!,
            "createdat" => slide => slide.CreatedAt,
            "updatedat" => slide => slide.UpdatedAt ?? slide.CreatedAt,
            _ => slide => slide.Index // Default: Order by Index
        };

        var slidesParameters = new QueryParameters<AIStorySlide>
        {
            PageNumber = request.SlidesPageNumber,
            PageSize = request.SlidesPageSize,
            Filter = slide => slide.AIStoryId == request.Id,
            OrderBy = slidesOrderBy,
            Descending = request.SlidesDescending,
            Includes = [
                slide => slide.CreatedByUser!,
                slide => slide.UpdatedByUser!
            ]
        };

        var (slides, totalSlidesCount) = await aiStorySlideRepository.GetAllMatchingAsync(
            slidesParameters, 
            cancellationToken);

        var slideDtos = slides.Select(slide => mapper.Map<AIStorySlideDto>(slide)).ToList();

        var pagedSlides = new PagedResult<AIStorySlideDto>(
            slideDtos,
            totalSlidesCount,
            request.SlidesPageSize,
            request.SlidesPageNumber);

        response.Slides = pagedSlides;

        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
