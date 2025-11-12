using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStoryWithItems;

public class AddMasterStoryWithItemsCommandHandler(
    IRepository<MasterStory> masterStoryRepository,
    IRepository<StoryCategory> storyCategoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddMasterStoryWithItemsCommand, string>
{
    public async Task<string> Handle(AddMasterStoryWithItemsCommand request, CancellationToken cancellationToken)
    {
        // Verify that the StoryCategory exists
        var categoryExists = await storyCategoryRepository.GetByIdAsync(request.StoryCategoryId, cancellationToken);
        if (categoryExists == null)
            throw new NotFoundException(nameof(StoryCategory), request.StoryCategoryId);

        // Map master story
        var masterStory = new MasterStory
        {
            StoryCategoryId = request.StoryCategoryId,
            Title = request.Title,
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            ApprovalStatus = ApprovalStatus.Pending
        };
        
        // Set audit fields
        var currentUserId = currentUserService.UserId ?? string.Empty;
        masterStory.CreatedBy = currentUserId;
        masterStory.CreatedAt = DateTime.UtcNow;

        // Map and add media items
        if (request.MediaItems != null && request.MediaItems.Any())
        {
            var mediaItems = request.MediaItems.Select(itemDto => new MediaItem
            {
                Title = itemDto.Title,
                Description = itemDto.Description,
                Url = itemDto.Url,
                MasterStoryId = masterStory.Id, // Will be set when story is saved
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            masterStory.MediaItems = mediaItems;
        }
        
        await masterStoryRepository.AddAsync(masterStory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return masterStory.Id;
    }
}
