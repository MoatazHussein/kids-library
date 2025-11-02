using AutoMapper;
using Salhia.KidsLibrary.Application.Features.CustomStories.Commands.AddCustomStory;
using Salhia.KidsLibrary.Application.Features.CustomStories.Commands.UpdateCustomStory;
using Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStories;
using Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class CustomStoryMappingProfile : Profile
{
    public CustomStoryMappingProfile()
    {
        // Commands
        CreateMap<AddCustomStoryCommand, CustomStory>();
        CreateMap<UpdateCustomStoryCommand, CustomStory>();

        // Queries
        CreateMap<CustomStory, GetCustomStoriesQueryResponse>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null))
            .ForMember(dest => dest.CustomStoryItemsCount, opt => opt.MapFrom(src => src.CustomStoryItems.Count));

        CreateMap<CustomStory, GetCustomStoryByIdQueryResponse>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null))
            .ForMember(dest => dest.Items, opt => opt.Ignore()); // Items populated separately in handler
    }
}
