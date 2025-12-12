using MediatR;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.SystemSettings.Queries.GetSystemSettings;

public record GetSystemSettingsQuery : IRequest<GetSystemSettingsResponse?>;
