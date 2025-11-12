using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.AddMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;
using Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class MediaItemMappingProfile : Profile
{
    public MediaItemMappingProfile()
    {
        // User mapping
        CreateMap<AppUser, UserInfoDto>();
        
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddMediaItemCommand, MediaItem>();
        CreateMap<UpdateMediaItemCommand, MediaItem>();

        // Queries
        CreateMap<MediaItem, GetMediaItemsQueryResponse>();
    }
}
