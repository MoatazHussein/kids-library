using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class FavoriteStoryMappingProfile : Profile
{
    public FavoriteStoryMappingProfile()
    {
        // User mapping
        CreateMap<AppUser, UserInfoDto>();
        
        // Queries
        CreateMap<FavoriteStory, GetFavoriteStoriesQueryResponse>()
            .ForMember(dest => dest.FavoriteId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FavoritedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.MasterStoryId, opt => opt.MapFrom(src => src.MasterStory.Id))
            .ForMember(dest => dest.StoryCategoryId, opt => opt.MapFrom(src => src.MasterStory.StoryCategoryId))
            .ForMember(dest => dest.StoryCategoryTitle, opt => opt.MapFrom(src => src.MasterStory.StoryCategory != null ? src.MasterStory.StoryCategory.Title : null))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.MasterStory.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.MasterStory.Content))
            .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.MasterStory.CoverImageUrl))
            .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => src.MasterStory.MediaType))
            .ForMember(dest => dest.MediaUrl, opt => opt.MapFrom(src => src.MasterStory.MediaUrl))
            .ForMember(dest => dest.PublishYear, opt => opt.MapFrom(src => src.MasterStory.PublishYear))
            .ForMember(dest => dest.ApprovalStatus, opt => opt.MapFrom(src => src.MasterStory.ApprovalStatus))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.MasterStory.CreatedAt))
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.MasterStory.Comments.Count));
    }
}
