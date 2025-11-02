using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.DeleteCustomStoryItem;

public class DeleteCustomStoryItemCommandHandler(
    IRepository<CustomStoryItem> customStoryItemRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCustomStoryItemCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCustomStoryItemCommand request, CancellationToken cancellationToken)
    {
        var customStoryItem = await customStoryItemRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customStoryItem == null)
            throw new NotFoundException(nameof(CustomStoryItem), request.Id);
        
        await customStoryItemRepository.DeleteAsync(customStoryItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
