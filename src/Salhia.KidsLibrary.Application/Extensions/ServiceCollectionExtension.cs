using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;
using Salhia.KidsLibrary.Application.Services.StatsSyncService;
using Salhia.KidsLibrary.Application.Services.StoryNotificationService;
using Salhia.KidsLibrary.Application.Services.StoryViewService;

namespace Salhia.KidsLibrary.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {

        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
           .AddFluentValidationAutoValidation();


        services.AddScoped<IMasterStoryStatsService,MasterStoryStatsService>();
        services.AddScoped<IStoryViewService, StoryViewService>();
        services.AddScoped<IStoryNotificationService, StoryNotificationService>();
        services.AddScoped<IStatsSyncService, StatsSyncService>();

    }
}
