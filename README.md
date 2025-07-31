# End-to-End Guide: Building a Unique .NET Core Proof of Concept (POC) Project

---

## 1. Project Overview

**POC Title:** Smart Task Manager API

**Purpose:**  
This Proof of Concept demonstrates building a RESTful API for managing tasks and projects, featuring a clean architecture, layered structure, Entity Framework Core integration, and a simple frontend using Razor Pages. It is designed to showcase modern .NET Core best practices, including quality checks with SonarQube.

**Functionality:**
- CRUD operations for Projects and Tasks
- Relational data modeling (each Project has multiple Tasks)
- RESTful API endpoints
- Razor Pages frontend for basic interaction
- Unit and integration testing
- Cloud deployment (Azure App Service)
- Quality analysis using SonarQube

---

## 2. Requirements

### Tools & Frameworks

| Tool/Library            | Version (as of July 2025)        |
|-------------------------|----------------------------------|
| .NET SDK/Core           | 8.0.x                            |
| Visual Studio           | 2022 17.8+ / VS Code 1.90+       |
| Entity Framework Core   | 8.0.x                            |
| SQL Server Express      | 2022                             |
| Swashbuckle (Swagger)   | 6.5.x                            |
| xUnit                   | 2.5.x                            |
| Moq                     | 5.3.x                            |
| SonarQube Scanner       | 6.0+                             |
| Azure CLI (optional)    | 2.60+                            |

---

## 3. Environment Setup

### .NET SDK
- Download and install [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

### IDE
- **Visual Studio 2022** (Community/Professional/Enterprise)
- **OR** [Visual Studio Code](https://code.visualstudio.com/) with C# & Razor extensions

### SQL Server
- Install [SQL Server Express 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).

### SonarQube (Local)
- [Download SonarQube](https://www.sonarqube.org/downloads/) and follow [installation guide](https://docs.sonarqube.org/latest/setup/get-started-2-minutes/).

### Azure CLI (for deployment)
- [Install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli).

---

## 4. Project Structure

```
SmartTaskManager/
│
├── SmartTaskManager.Api/           # Web API project
│   ├── Controllers/
│   │   ├── ProjectsController.cs
│   │   └── TasksController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── Startup.cs
│
├── SmartTaskManager.Core/          # Domain models and interfaces
│   ├── Models/
│   │   ├── Project.cs
│   │   └── TaskItem.cs
│   ├── Interfaces/
│   │   ├── IProjectRepository.cs
│   │   ├── ITaskRepository.cs
│   │   └── IService.cs
│
├── SmartTaskManager.Infrastructure/ # Data Access Layer
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Repositories/
│   │   ├── ProjectRepository.cs
│   │   └── TaskRepository.cs
│
├── SmartTaskManager.Services/      # Business logic layer
│   ├── Services/
│   │   ├── ProjectService.cs
│   │   └── TaskService.cs
│
├── SmartTaskManager.Web/           # Razor Pages frontend
│   ├── Pages/
│   │   ├── Projects.cshtml
│   │   └── Tasks.cshtml
│   └── wwwroot/
│
├── SmartTaskManager.Tests/         # Unit and integration tests
│   └── ...
│
└── sonar-project.properties        # SonarQube configuration
```

**Explanation:**
- **Api/**: Entry point for all API requests.
- **Core/**: Contains models and interfaces—pure domain logic.
- **Infrastructure/**: Implementation of data access logic.
- **Services/**: Business logic layer, manipulates data via repositories.
- **Web/**: Razor Pages frontend.
- **Tests/**: Unit/integration tests.
- **sonar-project.properties**: SonarQube config file.

---

## 6. Testing

### Unit Test Example (using xUnit & Moq)

```csharp name=SmartTaskManager.Tests/ProjectServiceTests.cs
using Xunit;
using Moq;
using SmartTaskManager.Services.Services;
using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProjectServiceTests
{
    [Fact]
    public async Task GetAllProjectsAsync_ReturnsProjects()
    {
        var mockRepo = new Mock<IProjectRepository>();
        mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Project> { new Project { Name = "Test" } });

        var service = new ProjectService(mockRepo.Object);
        var result = await service.GetAllProjectsAsync();

        Assert.Single(result);
    }
}
```

### Integration Test Example

```csharp name=SmartTaskManager.Tests/ProjectsApiTests.cs
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using SmartTaskManager.Core.Models;

public class ProjectsApiTests : IClassFixture<WebApplicationFactory<SmartTaskManager.Api.Program>>
{
    private readonly HttpClient _client;

    public ProjectsApiTests(WebApplicationFactory<SmartTaskManager.Api.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostProject_ReturnsCreated()
    {
        var project = new Project { Name = "IntegrationTest" };
        var response = await _client.PostAsJsonAsync("/api/projects", project);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Project>();
        Assert.Equal("IntegrationTest", created!.Name);
    }
}
```

---

## 7. Deployment Instructions

### Deploy to Azure App Service

1. **Publish from Visual Studio:**
   - Right-click the `SmartTaskManager.Api` project > Publish > Azure > Azure App Service (Windows/Linux) > follow the wizard.

2. **Using Azure CLI:**
   ```sh
   az login
   az group create --name TaskManagerRG --location "East US"
   az appservice plan create --name TaskManagerPlan --resource-group TaskManagerRG --sku FREE
   az webapp create --resource-group TaskManagerRG --plan TaskManagerPlan --name <your-unique-app-name> --runtime "DOTNETCORE:8.0"
   dotnet publish -c Release -o ./publish
   az webapp deploy --resource-group TaskManagerRG --name <your-unique-app-name> --src-path ./publish
   ```

3. **Configure Database:**
   - Use Azure SQL or configure `ConnectionStrings` as needed.

4. **Set Environment Variables:**
   - In the Azure Portal, go to your App Service > Configuration > New application setting for DB connection.

---

## 8. SonarQube (Quality Analysis) Configuration

### SonarQube Setup

1. **Spin up SonarQube locally:**
   ```sh
   docker run -d --name sonarqube -p 9000:9000 sonarqube:latest
   ```

2. **Create `sonar-project.properties` at root:**

```properties name=sonar-project.properties
sonar.projectKey=SmartTaskManager
sonar.organization=your-org
sonar.host.url=http://localhost:9000
sonar.login=YOUR_TOKEN
sonar.language=cs
sonar.sources=SmartTaskManager.Api,SmartTaskManager.Core,SmartTaskManager.Services,SmartTaskManager.Infrastructure
sonar.tests=SmartTaskManager.Tests
sonar.cs.opencover.reportsPaths=SmartTaskManager.Tests/TestResults/**/*.xml
```

3. **Analyze Code:**
   - Install [SonarScanner](https://docs.sonarqube.org/latest/analysis/scan/sonarscanner/)
   - Run:
     ```sh
     dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
     sonar-scanner
     ```
---

## 9. Conclusion

- **Key Takeaways:**
  - Layered architecture separates concerns and improves maintainability.
  - Entity Framework Core simplifies data access.
  - RESTful APIs make integration straightforward.
  - Unit & integration tests ensure reliability.
  - SonarQube enforces code quality and best practices.
  - Cloud deployment with Azure is streamlined.
---

HAPPY CODING

*This guide provides a robust foundation for modern .NET Core application development and can be extended for production-ready solutions.*
