using AutoMapper;
using Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class StoryRatingMappingProfile : Profile
{
    public StoryRatingMappingProfile()
    {
        // Queries
        CreateMap<StoryRating, GetRatingQueryResponse>()
            .ForMember(dest => dest.RatingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
