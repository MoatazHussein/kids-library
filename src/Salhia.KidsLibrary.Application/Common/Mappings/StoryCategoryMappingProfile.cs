using AutoMapper;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.UpdateStoryCategory;
using Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class StoryCategoryMappingProfile : Profile
{
    public StoryCategoryMappingProfile()
    {
        // Commands - Direct mapping from Command to Entity
        CreateMap<AddStoryCategoryCommand, StoryCategory>();
        CreateMap<UpdateStoryCategoryCommand, StoryCategory>();

        // Queries
        CreateMap<StoryCategory, GetStoryCategoriesQueryResponse>();
    }
}
