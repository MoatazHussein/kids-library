using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.AddCustomStory;

public class AddCustomStoryCommandHandler(
    IRepository<CustomStory> customStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<AddCustomStoryCommand, string>
{
    public async Task<string> Handle(AddCustomStoryCommand request, CancellationToken cancellationToken)
    {
        var customStory = mapper.Map<CustomStory>(request);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        customStory.CreatedBy = currentUserId ?? string.Empty;
        customStory.CreatedAt = DateTime.UtcNow;
        
        await customStoryRepository.AddAsync(customStory, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return customStory.Id;
    }
}
