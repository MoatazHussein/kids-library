using AutoMapper;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;
using Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class StoryCommentMappingProfile : Profile
{
    public StoryCommentMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddStoryCommentCommand, StoryComment>();

        CreateMap<AddStoryCommentCommand, StoryComment>();
        CreateMap<UpdateStoryCommentCommand, StoryComment>();
    }
}
