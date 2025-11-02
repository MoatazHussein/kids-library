using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.DeleteCustomStory;

public class DeleteCustomStoryCommandHandler(
    IRepository<CustomStory> customStoryRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCustomStoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCustomStoryCommand request, CancellationToken cancellationToken)
    {
        var customStory = await customStoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customStory == null)
            throw new NotFoundException(nameof(CustomStory), request.Id);
        
        await customStoryRepository.DeleteAsync(customStory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
