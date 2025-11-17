using AutoMapper;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.UpdateStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class StoryCategoryMappingProfile : Profile
{
    public StoryCategoryMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddStoryCategoryCommand, StoryCategory>();
        CreateMap<UpdateStoryCategoryCommand, StoryCategory>();

        // Queries
        CreateMap<StoryCategory, GetStoryCategoriesQueryResponse>()
            .ForMember(dest => dest.MasterStoriesCount,
                opt => opt.MapFrom(src => src.MasterStories.Count(e => e.ApprovalStatus == ApprovalStatus.Approved)));


    }
}
