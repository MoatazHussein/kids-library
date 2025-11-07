using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.DeleteStoryCategory;

public class DeleteStoryCategoryCommandHandler(
    IRepository<StoryCategory> storyCategoryRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteStoryCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteStoryCategoryCommand request, CancellationToken cancellationToken)
    {
        var storyCategory = await storyCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

        if (storyCategory == null)
            throw new NotFoundException(nameof(StoryCategory), request.Id);


        await storyCategoryRepository.DeleteAsync(storyCategory);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
