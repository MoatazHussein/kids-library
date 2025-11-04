using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.AddMediaItem;

public class AddMediaItemCommandHandler(
    IRepository<MediaItem> mediaItemRepository,
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<AddMediaItemCommand, string>
{
    public async Task<string> Handle(AddMediaItemCommand request, CancellationToken cancellationToken)
    {
        // Verify parent story exists
        var masterStory = await masterStoryRepository.GetByIdAsync(request.MasterStoryId, cancellationToken);
        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.MasterStoryId);

        // Authorization: Only the story creator or admin can add media items to the story
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (masterStory.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to add media items to this story");

        var mediaItem = mapper.Map<MediaItem>(request);
        
        // Set audit fields
        mediaItem.CreatedBy = currentUserId ?? string.Empty;
        mediaItem.CreatedAt = DateTime.UtcNow;
        
        await mediaItemRepository.AddAsync(mediaItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return mediaItem.Id;
    }
}
