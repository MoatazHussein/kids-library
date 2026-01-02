using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.DeleteMasterStory;

public class DeleteMasterStoryCommandHandler(
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteMasterStoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMasterStoryCommand request, CancellationToken cancellationToken)
    {
        var masterStory = await masterStoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.Id);

        // Authorization: Only creator or admin can delete
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (masterStory.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to delete this story", "UnauthorizedStoryDeletion");
        
        await masterStoryRepository.DeleteAsync(masterStory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
