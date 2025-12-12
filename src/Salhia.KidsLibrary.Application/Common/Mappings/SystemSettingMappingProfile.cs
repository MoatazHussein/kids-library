using AutoMapper;
using Salhia.KidsLibrary.Application.Features.SystemSettings.Queries.GetSystemSettings;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Mappings;

public class SystemSettingMappingProfile : Profile
{
    public SystemSettingMappingProfile()
    {
        CreateMap<SystemSetting, GetSystemSettingsResponse>();
    }
}
