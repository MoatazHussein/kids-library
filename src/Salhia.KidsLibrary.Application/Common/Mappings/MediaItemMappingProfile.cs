using AutoMapper;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.AddMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class MediaItemMappingProfile : Profile
{
    public MediaItemMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddMediaItemCommand, MediaItem>();
        CreateMap<UpdateMediaItemCommand, MediaItem>();

        // Queries
        CreateMap<MediaItem, GetMediaItemsQueryResponse>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null));
    }
}
