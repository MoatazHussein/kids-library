using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;

public class AddStoryCommentCommandHandler(
    IRepository<StoryComment> commentRepository,
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<AddStoryCommentCommand, string>
{
    public async Task<string> Handle(AddStoryCommentCommand request, CancellationToken cancellationToken)
    {
        // Verify master story exists
        var masterStory = await masterStoryRepository.GetByIdAsync(request.MasterStoryId, cancellationToken);
        if (masterStory == null)
            throw new NotFoundException(nameof(MasterStory), request.MasterStoryId);

        var comment = mapper.Map<StoryComment>(request);
        
        // Set audit fields
        var currentUserId = currentUserService.UserId;
        comment.CreatedBy = currentUserId ?? string.Empty;
        comment.CreatedAt = DateTime.UtcNow;
        
        await commentRepository.AddAsync(comment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return comment.Id;
    }
}
