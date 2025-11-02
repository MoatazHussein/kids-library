using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.UpdateCustomStoryItem;

public class UpdateCustomStoryItemCommandHandler(
    IRepository<CustomStoryItem> customStoryItemRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateCustomStoryItemCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCustomStoryItemCommand request, CancellationToken cancellationToken)
    {
        var customStoryItem = await customStoryItemRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customStoryItem == null)
            throw new NotFoundException(nameof(CustomStoryItem), request.Id);
        
        mapper.Map(request, customStoryItem);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        customStoryItem.UpdatedBy = currentUserId;
        customStoryItem.UpdatedAt = DateTime.UtcNow;
        
        await customStoryItemRepository.UpdateAsync(customStoryItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
