using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.DeleteStoryComment;

public class DeleteStoryCommentCommandHandler(
    IRepository<StoryComment> commentRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteStoryCommentCommand, Unit>
{
    public async Task<Unit> Handle(DeleteStoryCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (comment == null)
            throw new NotFoundException(nameof(StoryComment), request.Id);

        // Authorization: Only creator or admin can delete
        var currentUserId = currentUserService.UserId;
        var isAdmin = currentUserService.IsInRole(UserRoles.Admin);

        if (comment.CreatedBy != currentUserId && !isAdmin)
            throw new UnAuthorizedAccessException("You don't have permission to delete this comment");
        
        await commentRepository.DeleteAsync(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
