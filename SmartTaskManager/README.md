# Smart Task Manager API

This project is a Proof of Concept for a layered .NET Core 8.0 application featuring:
- Entity Framework Core
- RESTful APIs
- Razor Pages frontend
- Unit and integration tests
- SonarQube quality analysis
- Azure deployment

## Folder Structure

- `SmartTaskManager.Api` - API project (controllers, entry point)
- `SmartTaskManager.Core` - Domain models and repository interfaces
- `SmartTaskManager.Infrastructure` - EF Core DbContext and repository implementations
- `SmartTaskManager.Services` - Business logic services
- `SmartTaskManager.Web` - Razor Pages frontend
- `SmartTaskManager.Tests` - Unit and integration tests

## Run Locally

1. Set up the database (SQL Server)
2. Update `appsettings.json` with your connection string
3. Run migrations: