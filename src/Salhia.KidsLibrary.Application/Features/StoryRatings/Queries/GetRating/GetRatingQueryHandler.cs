using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;

public class GetRatingQueryHandler(
    IRepository<StoryRating> storyRatingRepository,
    ICurrentUserService currentUserService,
    ILogger<GetRatingQueryHandler> logger,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetRatingQuery, GetRatingQueryResponse?>
{
    public async Task<GetRatingQueryResponse?> Handle(
        GetRatingQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        logger.LogInformation("Getting rating for user {UserId} and story {StoryId}", 
            currentUserId, request.MasterStoryId);

        var predicate = PredicateBuilder.New<StoryRating>(true)
            .And(sr => sr.UserId == currentUserId)
            .And(sr => sr.MasterStoryId == request.MasterStoryId);

        var parameters = new QueryParameters<StoryRating>
        {
            Filter = predicate
        };

        var (ratings, _) = await storyRatingRepository.GetAllMatchingAsync(
            parameters, 
            cancellationToken);

        var rating = ratings.FirstOrDefault();

        if (rating == null)
            return null;

        var response = mapper.Map<GetRatingQueryResponse>(rating);
        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
