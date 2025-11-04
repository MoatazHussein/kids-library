using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;

public class UpdateMediaItemCommandHandler(
    IRepository<MediaItem> mediaItemRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateMediaItemCommand, Unit>
{
    public async Task<Unit> Handle(UpdateMediaItemCommand request, CancellationToken cancellationToken)
    {
        var mediaItem = await mediaItemRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (mediaItem == null)
            throw new NotFoundException(nameof(MediaItem), request.Id);

        // Authorization: Only creator or admin can update
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (mediaItem.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to update this media item");
        
        mapper.Map(request, mediaItem);
        
        // Set audit fields
        mediaItem.UpdatedBy = currentUserId;
        mediaItem.UpdatedAt = DateTime.UtcNow;
        
        await mediaItemRepository.UpdateAsync(mediaItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
