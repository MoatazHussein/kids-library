using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.UpdateStoryCategory;

public class UpdateStoryCategoryCommandHandler(
    IRepository<StoryCategory> storyCategoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateStoryCategoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateStoryCategoryCommand request, CancellationToken cancellationToken)
    {
        var storyCategory = await storyCategoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (storyCategory == null)
            throw new NotFoundException(nameof(StoryCategory), request.Id);
        
        mapper.Map(request, storyCategory);

        // Set audit fields
        var currentUserId = currentUserService.UserId;
        storyCategory.UpdatedBy = currentUserId;
        storyCategory.UpdatedAt = DateTime.UtcNow;
        
        await storyCategoryRepository.UpdateAsync(storyCategory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
