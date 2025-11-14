using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.StoryShares.Commands.ShareStory;

public class ShareStoryCommandHandler(
    IRepository<StoryShare> storyShareRepository,
    IRepository<MasterStory> masterStoryRepository,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor,
    IMasterStoryStatsService statsService,
    IUnitOfWork unitOfWork,
    ILogger<ShareStoryCommandHandler> logger
    ) : IRequestHandler<ShareStoryCommand, Unit>
{
    public async Task<Unit> Handle(
        ShareStoryCommand request,
        CancellationToken cancellationToken)
    {
        // Verify story exists
        var story = await masterStoryRepository.GetByIdAsync(request.MasterStoryId, cancellationToken);
        if (story == null)
            throw new NotFoundException(nameof(MasterStory), request.MasterStoryId);

        // Get user info (if authenticated)
        string? userId = currentUserService.IsAuthenticated ? currentUserService.UserId : null;
        
        // Get IP address for anonymous tracking
        string? ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        // Create share record
        var share = new StoryShare
        {
            UserId = userId,
            MasterStoryId = request.MasterStoryId,
            Platform = request.Platform,
            IpAddress = ipAddress,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        };

        await storyShareRepository.AddAsync(share, cancellationToken);
        
        // Update stats
        await statsService.IncrementSharesCountAsync(request.MasterStoryId, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (userId != null)
        {
            logger.LogInformation("User {UserId} shared story {StoryId} via {Platform}", 
                userId, request.MasterStoryId, request.Platform);
        }
        else
        {
            logger.LogInformation("Anonymous user (IP: {IpAddress}) shared story {StoryId} via {Platform}", 
                ipAddress, request.MasterStoryId, request.Platform);
        }

        return Unit.Value;
    }
}
