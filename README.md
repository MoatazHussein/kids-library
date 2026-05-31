# Salhia Kids Library

Salhia Kids Library is a .NET 8 ASP.NET Core Web API for a children's digital library platform. It supports curated stories, user-created stories, AI-generated stories, engagement features, dashboards, file uploads, PDF generation, email workflows, and administrative content management.

The solution follows a Clean Architecture style with separate API, Application, Domain, and Infrastructure projects.

## Solution Structure

```text
Salhia.KidsLibrary.sln
src/
  Salhia.KidsLibrary.API/             ASP.NET Core Web API, middleware, controllers, Swagger
  Salhia.KidsLibrary.Application/     CQRS features, DTOs, validators, services, interfaces
  Salhia.KidsLibrary.Domain/          Entities, enums, constants, domain exceptions
  Salhia.KidsLibrary.Infrastructure/  EF Core, SQL Server, Identity, AI, email, storage, Quartz, PDF
```

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8 with SQL Server
- ASP.NET Core Identity with JWT authentication
- MediatR for CQRS request handling
- FluentValidation for validation
- AutoMapper for mapping
- Serilog for logging
- Quartz.NET for scheduled jobs
- QuestPDF for PDF generation
- OpenAI and FalAI integrations for AI story generation

## Main Features

- User registration, login, email confirmation, password reset, and role management
- Master story catalog with categories, approval workflow, ratings, likes, comments, favorites, and shares
- Custom stories with story items and PDF export
- AI story generation with story text, generated images, retry support, and PDF export
- Dashboard and analytics endpoints for story engagement and user activity
- File and image uploads through local storage
- Landing page statistics
- Scheduled story statistics synchronization

## Prerequisites

- .NET SDK 8.0 or later
- SQL Server or SQL Server Express
- Visual Studio 2022, Rider, or VS Code
- Optional: API keys for OpenAI and FalAI if AI story generation is used
- Optional: SMTP credentials if email workflows are used

## Configuration

The API reads configuration from `src/Salhia.KidsLibrary.API/appsettings.json`, environment-specific appsettings files, and environment variables.

Important configuration sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "App": {
    "BaseUrl": "...",
    "ConfirmEmailApiUrl": "...",
    "ResetPasswordUrl": "...",
    "FrontendBaseUrl": "...",
    "DefaultTimeZone": "Arab Standard Time"
  },
  "Jwt": {
    "Key": "...",
    "Issuer": "...",
    "Audience": "..."
  },
  "AIServices": {
    "OpenAI": {
      "ApiKey": "...",
      "BaseUrl": "...",
      "Model": "gpt-4o-mini"
    },
    "FalAI": {
      "ApiKey": "...",
      "BaseUrl": "..."
    }
  },
  "Smtp": {
    "Host": "...",
    "Port": 587,
    "Username": "...",
    "Password": "..."
  }
}
```

For local development, prefer user secrets or environment variables for sensitive values instead of committing secrets to source control. Environment variable names can use double underscores, for example:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=.\SQLExpress;Database=Salhia-KidsLibraryDb;Trusted_Connection=True;TrustServerCertificate=True"
$env:Jwt__Key = "replace-with-a-long-local-development-secret"
$env:Jwt__Issuer = "https://localhost:7072"
$env:Jwt__Audience = "https://localhost:7072"
```

## Getting Started

Restore packages:

```powershell
dotnet restore Salhia.KidsLibrary.sln
```

Update the database connection string in development configuration or environment variables.

Apply Entity Framework migrations:

```powershell
dotnet ef database update `
  --project src/Salhia.KidsLibrary.Infrastructure `
  --startup-project src/Salhia.KidsLibrary.API
```

Run the API:

```powershell
dotnet run --project src/Salhia.KidsLibrary.API
```

Default launch URLs:

- HTTPS: `https://localhost:7072`
- HTTP: `http://localhost:5241`
- Swagger: `https://localhost:7072/swagger`

## Database

The Infrastructure project contains the EF Core `AppDbContext`, entity configurations, and migrations.

Common migration commands:

```powershell
dotnet ef migrations add MigrationName `
  --project src/Salhia.KidsLibrary.Infrastructure `
  --startup-project src/Salhia.KidsLibrary.API

dotnet ef database update `
  --project src/Salhia.KidsLibrary.Infrastructure `
  --startup-project src/Salhia.KidsLibrary.API
```

Startup seeders run when the API starts. Storage folders are also created during startup through the configured startup task.

## API Documentation

Swagger is enabled for development and production in `Program.cs`.

After running the API, open:

```text
https://localhost:7072/swagger
```

JWT bearer authentication is configured in Swagger. Authenticate through the identity endpoints, then provide the bearer token in Swagger to test secured endpoints.

## Storage and Static Files

The API serves static files from:

- `wwwroot`
- `Storage`, exposed at `/Storage`

Uploaded files and generated assets are stored locally by the infrastructure storage service.

## Background Jobs

Quartz.NET is configured in the Infrastructure project. The `StatsSyncJob` synchronizes story statistics on a daily schedule.

## CORS

The API currently allows requests from:

- `http://localhost:3000`
- `https://salhiakids.vercel.app`

Update the CORS policy in `src/Salhia.KidsLibrary.API/Program.cs` when adding new frontend origins.

## Build

```powershell
dotnet build Salhia.KidsLibrary.sln
```

## Notes for Contributors

- Keep business rules in the Application and Domain layers where possible.
- Keep infrastructure details such as EF Core, email, AI providers, storage, and PDF generation in the Infrastructure layer.
- Add new API endpoints through controllers in the API project and route work through Application commands or queries.
- Do not commit production secrets, API keys, SMTP passwords, or database credentials.
