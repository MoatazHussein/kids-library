using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;

public class AddMasterStoryCommandHandler(
    IRepository<MasterStory> masterStoryRepository,
    IRepository<StoryCategory> storyCategoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IStoryNotificationService notificationService
    ) : IRequestHandler<AddMasterStoryCommand, string>
{
    public async Task<string> Handle(AddMasterStoryCommand request, CancellationToken cancellationToken)
    {
        // Verify that the StoryCategory exists
        var categoryExists = await storyCategoryRepository.GetByIdAsync(request.StoryCategoryId, cancellationToken);
        if (categoryExists == null)
            throw new NotFoundException(nameof(StoryCategory), request.StoryCategoryId);

        var masterStory = mapper.Map<MasterStory>(request);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        masterStory.CreatedBy = currentUserId ?? string.Empty;
        masterStory.CreatedAt = DateTime.UtcNow;
        
        await masterStoryRepository.AddAsync(masterStory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Notify all admins about the new story
        await notificationService.NotifyAdminsOfNewStoryAsync(
            masterStory, 
            currentUserId ?? string.Empty, 
            categoryExists.Title, 
            cancellationToken);
        
        return masterStory.Id;
    }
}
