using AutoMapper;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Commands.UpdateMasterStory;
using Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;
using Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class MasterStoryMappingProfile : Profile
{
    public MasterStoryMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddMasterStoryCommand, MasterStory>();
        CreateMap<UpdateMasterStoryCommand, MasterStory>();

        // Queries
        CreateMap<MasterStory, GetMasterStoriesQueryResponse>()
            .ForMember(dest => dest.StoryCategoryTitle, opt => opt.MapFrom(src => src.StoryCategory != null ? src.StoryCategory.Title : null))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserName : null))
            .ForMember(dest => dest.MediaItemsCount, opt => opt.MapFrom(src => src.MediaItems.Count))
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count));

        CreateMap<MasterStory, GetMasterStoryByIdQueryResponse>()
            .ForMember(dest => dest.StoryCategoryTitle, opt => opt.MapFrom(src => src.StoryCategory != null ? src.StoryCategory.Title : null))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null))
            .ForMember(dest => dest.MediaItems, opt => opt.Ignore()) // Handled in handler
            .ForMember(dest => dest.Comments, opt => opt.Ignore()); // Handled in handler
    }
}
