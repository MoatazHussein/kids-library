using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.DeleteMediaItem;

public class DeleteMediaItemCommandHandler(
    IRepository<MediaItem> mediaItemRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteMediaItemCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMediaItemCommand request, CancellationToken cancellationToken)
    {
        var mediaItem = await mediaItemRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (mediaItem == null)
            throw new NotFoundException(nameof(MediaItem), request.Id);

        // Authorization: Only creator or admin can delete
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (mediaItem.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to delete this media item");
        
        await mediaItemRepository.DeleteAsync(mediaItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
