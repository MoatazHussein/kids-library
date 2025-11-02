using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.AddCustomStoryItem;

public class AddCustomStoryItemCommandHandler(
    IRepository<CustomStoryItem> customStoryItemRepository,
    IRepository<CustomStory> customStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<AddCustomStoryItemCommand, string>
{
    public async Task<string> Handle(AddCustomStoryItemCommand request, CancellationToken cancellationToken)
    {
        // Verify parent story exists
        var storyExists = await customStoryRepository.GetByIdAsync(request.CustomStoryId, cancellationToken);
        if (storyExists == null)
            throw new NotFoundException(nameof(CustomStory), request.CustomStoryId);

        var customStoryItem = mapper.Map<CustomStoryItem>(request);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        customStoryItem.CreatedBy = currentUserId ?? string.Empty;
        customStoryItem.CreatedAt = DateTime.UtcNow;
        
        await customStoryItemRepository.AddAsync(customStoryItem, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return customStoryItem.Id;
    }
}
