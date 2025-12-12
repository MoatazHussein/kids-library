using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Features.SystemSettings.Queries.GetSystemSettings;

public class GetSystemSettingsQueryHandler(
    IRepository<SystemSetting> repository,
    ITimeZoneConverter timeZoneConverter,
    IMapper mapper
    ) : IRequestHandler<GetSystemSettingsQuery, GetSystemSettingsResponse?>
{
    public async Task<GetSystemSettingsResponse?> Handle(GetSystemSettingsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.FirstOrDefaultAsync(x=> true,cancellationToken);
        
        var response = mapper.Map<GetSystemSettingsResponse>(result);

        return timeZoneConverter.ConvertUtcToLocal(response);
    }
}
