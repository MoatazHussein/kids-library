using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.CustomStories;
using Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.AddCustomStoryItem;
using Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.UpdateCustomStoryItem;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class CustomStoryItemMappingProfile : Profile
{
    public CustomStoryItemMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddCustomStoryItemCommand, CustomStoryItem>();
        CreateMap<UpdateCustomStoryItemCommand, CustomStoryItem>();

        // Queries
        CreateMap<CustomStoryItem, CustomStoryItemDto>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null));
    }
}
