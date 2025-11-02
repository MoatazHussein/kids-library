using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.UpdateCustomStory;

public class UpdateCustomStoryCommandHandler(
    IRepository<CustomStory> customStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateCustomStoryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCustomStoryCommand request, CancellationToken cancellationToken)
    {
        var customStory = await customStoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (customStory == null)
            throw new NotFoundException(nameof(CustomStory), request.Id);
        
        mapper.Map(request, customStory);

        // Set audit fields
        var currentUserId = currentUserService.UserId;
        customStory.UpdatedBy = currentUserId;
        customStory.UpdatedAt = DateTime.UtcNow;
        
        await customStoryRepository.UpdateAsync(customStory);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
