using Salhia.KidsLibrary.API.Extensions;
using Salhia.KidsLibrary.API.Middlewares;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Extensions;
using Salhia.KidsLibrary.Infrastructure.Extensions;
using Salhia.KidsLibrary.Infrastructure.Seeders;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
        .WithOrigins("http://localhost:3000", "https://salhiakids.vercel.app") 
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

// Execute all startup tasks
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IAppSeeder>();
    await seeder.Seed();

    var startupTasks = scope.ServiceProvider.GetServices<IStartupTask>();

    foreach (var task in startupTasks)
    {
        await task.ExecuteAsync();
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseMiddleware<RequestTimeLoggingMiddleware>();

app.UseSerilogRequestLogging();

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "API V1.0");
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();      // serves /wwwroot/** (UI build is in /wwwroot/wwwroot)

// Serve the external "Storage" folder 
app.UseStaticFiles(new StaticFileOptions()

{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Storage")),
    RequestPath = new PathString("/Storage")
});

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();



