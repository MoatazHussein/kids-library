using AutoMapper;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddStoryCommentCommand, StoryComment>();
        CreateMap<UpdateStoryCommentCommand, StoryComment>();

        // Queries
        CreateMap<StoryComment, GetStoryCommentsQueryResponse>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUserName, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null));
    }
}
