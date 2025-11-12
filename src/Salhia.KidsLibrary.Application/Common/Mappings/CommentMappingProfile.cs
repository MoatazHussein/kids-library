using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        // User mapping
        CreateMap<AppUser, UserInfoDto>();
        
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddStoryCommentCommand, StoryComment>();
        CreateMap<UpdateStoryCommentCommand, StoryComment>();

        // Queries
        CreateMap<StoryComment, GetStoryCommentsQueryResponse>();
    }
}
