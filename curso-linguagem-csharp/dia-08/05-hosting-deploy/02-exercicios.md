# 📝 Exercícios - Hosting e Deploy

## 🎯 Objetivo

Praticar técnicas de hospedagem e deployment de aplicações ASP.NET Core, incluindo configuração de ambientes, health checks, containerização e otimização para produção.

---

## ✏️ Exercício 1: Configuração de Ambientes

**Dificuldade**: ⭐ Iniciante

Configure diferentes ambientes com configurações específicas:

### Requisitos:
1. **Arquivos de configuração**:
   - `appsettings.json` (padrão)
   - `appsettings.Development.json`
   - `appsettings.Production.json`

2. **appsettings.Development.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MinhaAppDev;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "AppSettings": {
    "Ambiente": "Development",
    "DebugMode": true,
    "EmailService": {
      "SmtpServer": "localhost",
      "SmtpPort": 25,
      "Usuario": "",
      "Senha": ""
    }
  }
}
```

3. **appsettings.Production.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=MinhaAppProd;User Id=appuser;Password=Str0ngP@ssw0rd!;"
  },
  "AppSettings": {
    "Ambiente": "Production",
    "DebugMode": false,
    "EmailService": {
      "SmtpServer": "smtp.office365.com",
      "SmtpPort": 587,
      "Usuario": "noreply@minhaempresa.com",
      "Senha": "P@ssw0rdPr0d"
    }
  }
}
```

4. **Configuração no Program.cs**:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuração baseada no ambiente
var environment = builder.Environment;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Serviços com configuração condicional
if (environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}
else
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(60);
    });
}

// ... outros serviços
```

5. **Controller para verificar ambiente**:
```csharp
[ApiController]
[Route("api/ambiente")]
public class AmbienteController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public AmbienteController(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    [HttpGet]
    public IActionResult ObterAmbiente()
    {
        return Ok(new
        {
            Ambiente = _env.EnvironmentName,
            IsDevelopment = _env.IsDevelopment(),
            IsProduction = _env.IsProduction(),
            Configuracao = new
            {
                ConnectionString = _config.GetConnectionString("DefaultConnection"),
                EmailServer = _config["AppSettings:EmailService:SmtpServer"],
                DebugMode = _config.GetValue<bool>("AppSettings:DebugMode")
            }
        });
    }
}
```

---

## ✏️ Exercício 2: Health Checks

**Dificuldade**: ⭐⭐ Intermediário

Implemente health checks para monitorar a saúde da aplicação:

### Requisitos:
1. **Configuração básica no Program.cs**:
```csharp
builder.Services.AddHealthChecks();

// Para aplicações web
builder.Services.AddHealthChecksUI(); // Pacote separado
```

2. **Health Check personalizado**:
```csharp
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public DatabaseHealthCheck(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);

            return HealthCheckResult.Healthy("Banco de dados acessível");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Erro no banco de dados", ex);
        }
    }
}
```

3. **Registro dos health checks**:
```csharp
builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "sql-server")
    .AddUrlGroup(
        new Uri("https://api.externa.com/health"),
        name: "api-externa",
        timeout: TimeSpan.FromSeconds(10))
    .AddDiskStorageHealthCheck(
        driveName: "C",
        minimumFreeMegabytes: 1024,
        name: "disco-c");
```

4. **Endpoints no Program.cs**:
```csharp
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/detailed",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });
```

5. **Health Check UI (opcional)**:
```csharp
app.MapHealthChecksUI();
```

---

## ✏️ Exercício 3: Containerização com Docker

**Dificuldade**: ⭐⭐⭐ Avançado

Crie Dockerfile e docker-compose para containerizar a aplicação:

### Requisitos:
1. **Dockerfile**:
```dockerfile
# Usar imagem base do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["MeuProjeto.csproj", "./"]
RUN dotnet restore "MeuProjeto.csproj"

# Copiar código fonte e buildar
COPY . .
RUN dotnet build "MeuProjeto.csproj" -c Release -o /app/build

# Publicar aplicação
FROM build AS publish
RUN dotnet publish "MeuProjeto.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Usar imagem base do .NET Runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Criar usuário não-root
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

# Copiar aplicação publicada
COPY --from=publish /app/publish .

# Expor porta
EXPOSE 8080

# Definir variável de ambiente
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de execução
ENTRYPOINT ["dotnet", "MeuProjeto.dll"]
```

2. **Dockerfile otimizado para multi-stage**:
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app
COPY --from=build /src/out .

# Create non-root user
RUN useradd --create-home --shell /bin/bash appuser
RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 80
ENTRYPOINT ["dotnet", "MeuProjeto.dll"]
```

3. **docker-compose.yml**:
```yaml
version: '3.8'

services:
  webapp:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=MinhaApp;User=sa;Password=YourStrong!Passw0rd;
    depends_on:
      - db
    networks:
      - minhaapp-network

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Passw0rd
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - minhaapp-network

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    networks:
      - minhaapp-network

volumes:
  sqlserver_data:
  redis_data:

networks:
  minhaapp-network:
    driver: bridge
```

4. **docker-compose.override.yml** (para desenvolvimento):
```yaml
version: '3.8'

services:
  webapp:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:80
    volumes:
      - .:/src
      - /src/bin
      - /src/obj
    ports:
      - "5000:80"
      - "5001:443"

  db:
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Passw0rd
      - MSSQL_PID=Developer
```

5. **.dockerignore**:
```
bin/
obj/
out/
*.user
*.tmp
.vs/
.vscode/
.git/
.github/
README.md
.dockerignore
Dockerfile*
docker-compose*
```

6. **Comandos para build e execução**:
```bash
# Build da imagem
docker build -t minhaapp .

# Executar container
docker run -p 8080:80 minhaapp

# Usar docker-compose
docker-compose up -d

# Para desenvolvimento
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```
