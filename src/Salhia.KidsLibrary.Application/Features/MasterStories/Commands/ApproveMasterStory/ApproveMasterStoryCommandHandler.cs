using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Services.StoryNotificationService;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.ApproveMasterStory;

public class ApproveMasterStoryCommandHandler(
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IStoryNotificationService notificationService
    ) : IRequestHandler<ApproveMasterStoryCommand, Unit>
{
    public async Task<Unit> Handle(ApproveMasterStoryCommand request, CancellationToken cancellationToken)
    {
        // Authorization: Only admin can approve/disapprove
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);
        if (!isAdmin)
            throw new UnAuthorizedAccessException("Only administrators can approve or disapprove stories");

        var masterStory = await masterStoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (masterStory is null)
            throw new NotFoundException(nameof(MasterStory), request.Id);
        
        masterStory.ApprovalStatus = request.ApprovalStatus;

        // Set audit fields
        var currentUserId = currentUserService.UserId;
        masterStory.UpdatedBy = currentUserId;
        masterStory.UpdatedAt = DateTime.UtcNow;
        
        await masterStoryRepository.UpdateAsync(masterStory);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Notify author if story is approved
        if (request.ApprovalStatus == ApprovalStatus.Approved)
        {
            await notificationService.NotifyAuthorOfApprovalAsync(masterStory, cancellationToken);
        }
        
        return Unit.Value;
    }
}
