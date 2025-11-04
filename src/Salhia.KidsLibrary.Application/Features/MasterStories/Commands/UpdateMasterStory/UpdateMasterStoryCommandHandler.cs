using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.UpdateMasterStory;

public class UpdateMasterStoryCommandHandler(
    IRepository<MasterStory> masterStoryRepository,
    IRepository<StoryCategory> storyCategoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateMasterStoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateMasterStoryCommand request, CancellationToken cancellationToken)
    {
        var masterStory = await masterStoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.Id);

        // Authorization: Only creator or admin can update
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (masterStory.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to update this story");

        // Verify that the StoryCategory exists
        var categoryExists = await storyCategoryRepository.GetByIdAsync(request.StoryCategoryId, cancellationToken);
        if (categoryExists == null)
            throw new NotFoundException(nameof(StoryCategory), request.StoryCategoryId);
        
        mapper.Map(request, masterStory);

        // Set audit fields
        masterStory.UpdatedBy = currentUserId;
        masterStory.UpdatedAt = DateTime.UtcNow;
        
        await masterStoryRepository.UpdateAsync(masterStory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
