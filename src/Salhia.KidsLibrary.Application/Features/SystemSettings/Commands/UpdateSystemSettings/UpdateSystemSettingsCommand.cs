using MediatR;

namespace Salhia.KidsLibrary.Application.Features.SystemSettings.Commands.UpdateSystemSettings;

public record UpdateSystemSettingsCommand(int AIStoryLimitCount, int AIStoryLimitDays) : IRequest;
