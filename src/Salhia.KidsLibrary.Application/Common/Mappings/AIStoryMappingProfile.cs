using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.AIStories;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class AIStoryMappingProfile : Profile
{
    public AIStoryMappingProfile()
    {
        // AIStory mappings
        CreateMap<AIStory, AIStoryDto>()
            .ForMember(dest => dest.Slides, opt => opt.MapFrom(src => src.AIStorySlides));

        CreateMap<AIStorySlide, AIStorySlideDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<AIStory, GetAIStoryByIdQueryResponse>()
            .ForMember(dest => dest.Slides, opt => opt.Ignore()); // Slides populated separately in handler
    }
}
