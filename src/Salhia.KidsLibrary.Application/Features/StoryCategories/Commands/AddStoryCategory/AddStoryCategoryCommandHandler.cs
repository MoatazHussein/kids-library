using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;

public class AddStoryCategoryCommandHandler(
    IRepository<StoryCategory> storyCategoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<AddStoryCategoryCommand, string>
{
    public async Task<string> Handle(AddStoryCategoryCommand request, CancellationToken cancellationToken)
    {
        var storyCategory = mapper.Map<StoryCategory>(request);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        storyCategory.CreatedBy = currentUserId ?? string.Empty;
        storyCategory.CreatedAt = DateTime.UtcNow;
        
        await storyCategoryRepository.AddAsync(storyCategory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return storyCategory.Id;
    }
}
