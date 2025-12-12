using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.SystemSettings.Commands.UpdateSystemSettings;

public class UpdateSystemSettingsCommandHandler(
    IRepository<SystemSetting> repository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSystemSettingsCommand>
{
    public async Task Handle(UpdateSystemSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await repository.FirstOrDefaultAsync(x => true, cancellationToken);

        if (settings is null)
        {
            settings = new SystemSetting
            {
                AIStoryLimitCount = request.AIStoryLimitCount,
                AIStoryLimitDays = request.AIStoryLimitDays,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId 
            };
            await repository.AddAsync(settings, cancellationToken);
        }
        else
        {
            settings.AIStoryLimitCount = request.AIStoryLimitCount;
            settings.AIStoryLimitDays = request.AIStoryLimitDays;
            settings.UpdatedAt = DateTime.UtcNow;
            settings.UpdatedBy = currentUserService.UserId;

            await repository.UpdateAsync(settings);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
