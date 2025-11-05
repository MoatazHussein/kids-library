using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;

public class UpdateStoryCommentCommandHandler(
    IRepository<StoryComment> commentRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateStoryCommentCommand, Unit>
{
    public async Task<Unit> Handle(UpdateStoryCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (comment == null)
            throw new NotFoundException(nameof(StoryComment), request.Id);

        // Authorization: Only creator 
        var currentUserId = currentUserService.UserId;

        if (comment.CreatedBy != currentUserId)
            throw new UnAuthorizedAccessException("You don't have permission to update this comment");
        
        mapper.Map(request, comment);
        
        // Set audit fields
        comment.UpdatedBy = currentUserId;
        comment.UpdatedAt = DateTime.UtcNow;
        
        await commentRepository.UpdateAsync(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
