# 03 - CI/CD e Automação

## Continuous Integration (CI)

### GitHub Actions
```yaml
# .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore

    - name: Test
      run: dotnet test --no-build --verbosity normal
```

### Azure DevOps Pipelines
```yaml
# azure-pipelines.yml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: UseDotNet@2
  inputs:
    packageType: 'sdk'
    version: '8.0.x'

- task: DotNetCoreCLI@2
  inputs:
    command: 'restore'

- task: DotNetCoreCLI@2
  inputs:
    command: 'build'

- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
```

## Continuous Deployment (CD)

### Deploy para Azure App Service
```yaml
# Deploy job
- name: Deploy to Azure
  uses: azure/webapps-deploy@v2
  with:
    app-name: 'my-app-name'
    slot-name: 'production'
    publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE }}
    package: './publish'
```

### Docker + Kubernetes
```yaml
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyApp.csproj", "."]
RUN dotnet restore "MyApp.csproj"
COPY . .
RUN dotnet build "MyApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

## Automação de Testes

### Testes Unitários
```csharp
[Fact]
public async Task CreateUser_ShouldReturnCreatedUser()
{
    // Arrange
    var user = new User { Name = "John Doe", Email = "john@example.com" };

    // Act
    var result = await _userService.CreateUserAsync(user);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("John Doe", result.Name);
}
```

### Testes de Integração
```csharp
[Fact]
public async Task GetUsers_ShouldReturnUsersFromDatabase()
{
    // Arrange
    await using var context = new TestDbContext(_options);
    context.Users.Add(new User { Name = "Test User" });
    await context.SaveChangesAsync();

    // Act
    var users = await _userRepository.GetAllAsync();

    // Assert
    Assert.Single(users);
    Assert.Equal("Test User", users.First().Name);
}
```

## Code Quality Gates

### Code Coverage
```xml
<!-- Directory.Build.props -->
<Project>
  <PropertyGroup>
    <CollectCoverage>true</CollectCoverage>
    <CoverletOutputFormat>opencover</CoverletOutputFormat>
    <Exclude>[*.Tests]*</Exclude>
  </PropertyGroup>
</Project>
```

### SonarQube Integration
```yaml
- name: SonarQube Scan
  uses: sonarsource/sonarqube-scan-action@v1
  with:
    projectBaseDir: .
    sonarProjectKey: my-project-key
```

## Blue-Green Deployment

### Estratégia de Deploy
```csharp
public class DeploymentService
{
    public async Task DeployToStagingAsync()
    {
        // Deploy para staging
        await _deploymentClient.DeployAsync("staging", _packagePath);

        // Executar testes de smoke
        var healthCheck = await _healthCheckService.CheckHealthAsync("staging");
        if (!healthCheck.IsHealthy)
            throw new DeploymentException("Staging health check failed");

        // Swap para production
        await _deploymentClient.SwapSlotsAsync("staging", "production");
    }
}
```
