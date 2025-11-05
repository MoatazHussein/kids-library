using AutoMapper;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class FavoriteStoryMappingProfile : Profile
{
    public FavoriteStoryMappingProfile()
    {
        // Queries
        CreateMap<FavoriteStory, GetFavoriteStoriesQueryResponse>()
            .ForMember(dest => dest.FavoriteId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FavoritedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.MasterStoryId, opt => opt.MapFrom(src => src.MasterStory.Id))
            .ForMember(dest => dest.StoryCategoryId, opt => opt.MapFrom(src => src.MasterStory.StoryCategoryId))
            .ForMember(dest => dest.StoryCategoryTitle, opt => opt.MapFrom(src => src.MasterStory.StoryCategory != null ? src.MasterStory.StoryCategory.Title : null))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.MasterStory.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.MasterStory.Content))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.MasterStory.ImageUrl))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.MasterStory.IsApproved))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.MasterStory.CreatedBy))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.MasterStory.Author != null ? src.MasterStory.Author.UserName : null))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.MasterStory.CreatedAt))
            .ForMember(dest => dest.MediaItemsCount, opt => opt.MapFrom(src => src.MasterStory.MediaItems.Count))
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.MasterStory.Comments.Count));
    }
}
