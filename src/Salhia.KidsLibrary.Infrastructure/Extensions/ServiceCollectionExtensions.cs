using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.AI;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Infrastructure.Persistence;
using Salhia.KidsLibrary.Infrastructure.Repositories;
using Salhia.KidsLibrary.Infrastructure.Seeders;
using Salhia.KidsLibrary.Infrastructure.Services.AI.Configuration;
using Salhia.KidsLibrary.Infrastructure.Services.AI.OpenAI;
using Salhia.KidsLibrary.Infrastructure.Services.Email;
using Salhia.KidsLibrary.Infrastructure.Services.Pdf;
using Salhia.KidsLibrary.Infrastructure.Services.Security;
using Salhia.KidsLibrary.Infrastructure.Services.Storage;
using Salhia.KidsLibrary.Infrastructure.Services.TimeConversion;
using Salhia.KidsLibrary.Infrastructure.Services.UnitOfWork;
using Salhia.KidsLibrary.Infrastructure.Startup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Salhia.KidsLibrary.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure QuestPDF license
        QuestPDF.Settings.License = LicenseType.Community;

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
         .EnableSensitiveDataLogging());


        services.AddScoped<IMailService, MailService>();

        services.AddScoped<IJwtService, JwtService>();

        services.AddIdentityCore<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();


        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IAppSeeder, AppSeeder>();

        services.AddScoped<ITimeZoneConverter, TimeZoneConverter>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IStartupTask, EnsureStorageFoldersTask>();
        
        services.AddScoped<IPdfService, PdfService>();

        services.AddScoped<IDashboardRepository, DashboardRepository>();

        // Configure AI Services
        services.Configure<AIServiceOptions>(configuration.GetSection(AIServiceOptions.SectionName));
        
        // Register OpenAI service with HttpClient
        services.AddHttpClient<IOpenAIService, OpenAIService>();

    }

}
