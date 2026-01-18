# 04 - Logging e Rastreamento

## 🎯 Objetivos

Ao final deste módulo, você será capaz de:
- Entender e aplicar níveis de log corretamente
- Configurar Microsoft.Extensions.Logging (ILogger)
- Usar Serilog para logs estruturados
- Implementar correlation IDs e contexto distribuído
- Configurar múltiplos sinks (Console, File, Database, Cloud)
- Integrar OpenTelemetry para observabilidade
- Aplicar melhores práticas de logging em produção

---

## 📑 Índice

1. [Fundamentos de Logging](#1-fundamentos-de-logging)
2. [Microsoft.Extensions.Logging](#2-microsoftextensionslogging)
3. [Serilog - Logging Estruturado](#3-serilog---logging-estruturado)
4. [Correlation ID e Contexto](#4-correlation-id-e-contexto)
5. [Sinks e Destinos](#5-sinks-e-destinos)
6. [OpenTelemetry e Traces](#6-opentelemetry-e-traces)
7. [Logging em Produção](#7-logging-em-produção)
8. [Melhores Práticas](#8-melhores-práticas)

---

## 1. Fundamentos de Logging

### 1.1 Por Que Fazer Logging?

```csharp
// ❌ Debugging com Console.WriteLine
public void ProcessarPedido(Pedido pedido)
{
    Console.WriteLine($"Processando pedido {pedido.Id}");  // Não persistente
    // Se der erro, como saber o que aconteceu?
}

// ✅ Logging estruturado
public void ProcessarPedido(Pedido pedido)
{
    _logger.LogInformation("Processando pedido {PedidoId} do cliente {ClienteId}", 
        pedido.Id, pedido.ClienteId);
    
    try
    {
        // lógica
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao processar pedido {PedidoId}", pedido.Id);
        throw;
    }
}
```

**Benefícios:**
- ✅ **Persistência:** Logs salvos para análise posterior
- ✅ **Estruturação:** Dados estruturados, não apenas texto
- ✅ **Níveis:** Filtragem por severidade
- ✅ **Contexto:** Informações adicionais (timestamp, thread, etc.)
- ✅ **Correlação:** Rastrear operações distribuídas

### 1.2 Níveis de Log

```csharp
public enum LogLevel
{
    Trace = 0,        // Detalhes extremos (raramente usado)
    Debug = 1,        // Informações de debug (desenvolvimento)
    Information = 2,  // Fluxo geral da aplicação
    Warning = 3,      // Comportamentos inesperados (não são erros)
    Error = 4,        // Erros e exceções
    Critical = 5,     // Falhas críticas do sistema
    None = 6          // Desabilita logging
}
```

**Quando usar cada nível:**

```csharp
// Trace: Detalhes granulares
_logger.LogTrace("Entrando no método ProcessarPedido com pedido {PedidoId}", id);

// Debug: Informações para desenvolvimento
_logger.LogDebug("Cache hit para produto {ProdutoId}: {Nome}", id, nome);

// Information: Eventos importantes do negócio
_logger.LogInformation("Pedido {PedidoId} criado com sucesso. Total: {Total:C}", id, total);

// Warning: Situação não ideal mas recuperável
_logger.LogWarning("API externa lenta: {Endpoint} demorou {Tempo}ms", endpoint, tempo);

// Error: Erro que impede uma operação
_logger.LogError(ex, "Falha ao processar pagamento para pedido {PedidoId}", id);

// Critical: Sistema em estado crítico
_logger.LogCritical("Banco de dados inacessível. Sistema entrando em modo readonly");
```

### 1.3 Logging vs. Exceções

```csharp
// ❌ NÃO use exceções para controle de fluxo
try
{
    var produto = produtos.Single(p => p.Id == id);
}
catch (InvalidOperationException)
{
    // Exceção esperada - má prática!
}

// ✅ Use null check + log
var produto = produtos.FirstOrDefault(p => p.Id == id);
if (produto == null)
{
    _logger.LogWarning("Produto {ProdutoId} não encontrado", id);
    return null;
}

// ✅ Log exceções não tratadas
try
{
    await ProcessarPagamentoAsync(pedido);
}
catch (PaymentException ex)
{
    _logger.LogError(ex, "Falha no pagamento do pedido {PedidoId}", pedido.Id);
    throw;  // Re-lança para camadas superiores tratarem
}
```

---

## 2. Microsoft.Extensions.Logging

### 2.1 Configuração Básica

```bash
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.Console
```

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        // Configurar DI container
        var serviceProvider = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);
            })
            .AddTransient<PedidoService>()
            .BuildServiceProvider();
        
        // Obter serviço com logger injetado
        var pedidoService = serviceProvider.GetRequiredService<PedidoService>();
        pedidoService.ProcessarPedido(123);
    }
}

public class PedidoService
{
    private readonly ILogger<PedidoService> _logger;
    
    public PedidoService(ILogger<PedidoService> logger)
    {
        _logger = logger;
    }
    
    public void ProcessarPedido(int pedidoId)
    {
        _logger.LogInformation("Iniciando processamento do pedido {PedidoId}", pedidoId);
        
        try
        {
            // Lógica de negócio
            ValidarPedido(pedidoId);
            CalcularTotal(pedidoId);
            SalvarPedido(pedidoId);
            
            _logger.LogInformation("Pedido {PedidoId} processado com sucesso", pedidoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar pedido {PedidoId}", pedidoId);
            throw;
        }
    }
    
    private void ValidarPedido(int pedidoId)
    {
        _logger.LogDebug("Validando pedido {PedidoId}", pedidoId);
        // validação
    }
    
    private decimal CalcularTotal(int pedidoId)
    {
        _logger.LogDebug("Calculando total do pedido {PedidoId}", pedidoId);
        return 150.00m;
    }
    
    private void SalvarPedido(int pedidoId)
    {
        _logger.LogDebug("Salvando pedido {PedidoId} no banco de dados", pedidoId);
        // salvar
    }
}
```

**Saída:**
```
info: PedidoService[0]
      Iniciando processamento do pedido 123
dbug: PedidoService[0]
      Validando pedido 123
dbug: PedidoService[0]
      Calculando total do pedido 123
dbug: PedidoService[0]
      Salvando pedido 123 no banco de dados
info: PedidoService[0]
      Pedido 123 processado com sucesso
```

### 2.2 Configuração via appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "MeuNamespace.PedidoService": "Debug"
    }
  }
}
```

```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder
            .AddConsole()
            .AddConfiguration(configuration.GetSection("Logging"));
    })
    .BuildServiceProvider();
```

### 2.3 Scopes (Contexto)

```csharp
public void ProcessarLote(List<Pedido> pedidos)
{
    using (_logger.BeginScope("Lote de {QuantidadePedidos} pedidos", pedidos.Count))
    {
        _logger.LogInformation("Iniciando processamento do lote");
        
        foreach (var pedido in pedidos)
        {
            using (_logger.BeginScope("PedidoId:{PedidoId}", pedido.Id))
            {
                _logger.LogInformation("Processando pedido");
                ProcessarPedido(pedido);
            }
        }
        
        _logger.LogInformation("Lote processado com sucesso");
    }
}
```

**Saída:**
```
info: [Lote de 3 pedidos] Iniciando processamento do lote
info: [Lote de 3 pedidos] [PedidoId:101] Processando pedido
info: [Lote de 3 pedidos] [PedidoId:102] Processando pedido
info: [Lote de 3 pedidos] [PedidoId:103] Processando pedido
info: [Lote de 3 pedidos] Lote processado com sucesso
```

---

## 3. Serilog - Logging Estruturado

### 3.1 Instalação e Configuração

```bash
dotnet add package Serilog
dotnet add package Serilog.Extensions.Logging
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Settings.Configuration
```

### 3.2 Configuração Básica

```csharp
using Serilog;

class Program
{
    static void Main()
    {
        // Configurar Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        try
        {
            Log.Information("Aplicação iniciada");
            
            ProcessarPedidos();
            
            Log.Information("Aplicação finalizada");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Aplicação encerrada inesperadamente");
        }
        finally
        {
            Log.CloseAndFlush();  // Importante!
        }
    }
    
    static void ProcessarPedidos()
    {
        var pedidos = new[] { 101, 102, 103 };
        
        foreach (var pedidoId in pedidos)
        {
            Log.Information("Processando pedido {PedidoId}", pedidoId);
            
            // Simulação
            Thread.Sleep(100);
        }
    }
}
```

### 3.3 Logs Estruturados (JSON)

```bash
dotnet add package Serilog.Formatting.Compact
```

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        new CompactJsonFormatter(),
        "logs/log-.json",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Log estruturado
Log.Information("Pedido criado {@Pedido}", new
{
    PedidoId = 123,
    ClienteId = 456,
    Total = 150.00m,
    Itens = new[]
    {
        new { Produto = "Mouse", Quantidade = 2 },
        new { Produto = "Teclado", Quantidade = 1 }
    }
});
```

**Saída JSON:**
```json
{
  "@t": "2025-10-27T14:32:15.1234567Z",
  "@l": "Information",
  "@m": "Pedido criado {\"PedidoId\":123,\"ClienteId\":456,\"Total\":150.00,...}",
  "Pedido": {
    "PedidoId": 123,
    "ClienteId": 456,
    "Total": 150.00,
    "Itens": [
      { "Produto": "Mouse", "Quantidade": 2 },
      { "Produto": "Teclado", "Quantidade": 1 }
    ]
  }
}
```

### 3.4 Integração com Microsoft.Extensions.Logging

```csharp
using Serilog;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.ClearProviders();  // Remove providers padrão
        builder.AddSerilog();      // Adiciona Serilog
    })
    .AddTransient<PedidoService>()
    .BuildServiceProvider();

// PedidoService usa ILogger<PedidoService> normalmente
var service = serviceProvider.GetRequiredService<PedidoService>();
```

### 3.5 Enrichers (Enriquecedores)

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()          // Propriedades do LogContext
    .Enrich.WithMachineName()         // Nome da máquina
    .Enrich.WithThreadId()            // ID da thread
    .Enrich.WithEnvironmentUserName() // Usuário do SO
    .Enrich.WithProperty("Application", "MeuApp")
    .Enrich.WithProperty("Version", "1.0.0")
    .CreateLogger();

// Uso com LogContext
using (LogContext.PushProperty("UserId", 12345))
using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
{
    Log.Information("Processando requisição");
    // Todos os logs dentro deste bloco terão UserId e RequestId
}
```

**Saída:**
```json
{
  "@t": "2025-10-27T14:32:15.1234567Z",
  "@l": "Information",
  "@m": "Processando requisição",
  "MachineName": "SERVER-01",
  "ThreadId": 12,
  "Application": "MeuApp",
  "Version": "1.0.0",
  "UserId": 12345,
  "RequestId": "550e8400-e29b-41d4-a716-446655440000"
}
```

---

## 4. Correlation ID e Contexto

### 4.1 O que é Correlation ID?

Identificador único que rastreia uma operação através de múltiplos serviços/camadas.

```csharp
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;
    
    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Obter ou gerar Correlation ID
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
            ?? Guid.NewGuid().ToString();
        
        // Adicionar ao response
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
        
        // Adicionar ao LogContext
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogInformation("Requisição iniciada: {Method} {Path}", 
                context.Request.Method, context.Request.Path);
            
            await _next(context);
            
            _logger.LogInformation("Requisição finalizada: {StatusCode}", 
                context.Response.StatusCode);
        }
    }
}

// Registro no Program.cs
app.UseMiddleware<CorrelationIdMiddleware>();
```

### 4.2 Implementação Console App

```csharp
public class OperationContext : IDisposable
{
    private readonly IDisposable _correlationIdScope;
    private readonly IDisposable _operationIdScope;
    
    public string CorrelationId { get; }
    public string OperationId { get; }
    
    public OperationContext(string? correlationId = null)
    {
        CorrelationId = correlationId ?? Guid.NewGuid().ToString();
        OperationId = Guid.NewGuid().ToString();
        
        _correlationIdScope = LogContext.PushProperty("CorrelationId", CorrelationId);
        _operationIdScope = LogContext.PushProperty("OperationId", OperationId);
    }
    
    public void Dispose()
    {
        _correlationIdScope?.Dispose();
        _operationIdScope?.Dispose();
    }
}

// Uso
using (var context = new OperationContext())
{
    Log.Information("Processando pedido");
    
    // Chamada para outro serviço (passa o CorrelationId)
    await _apiClient.ProcessarAsync(context.CorrelationId);
}
```

### 4.3 Contexto Distribuído (AsyncLocal)

```csharp
public static class CorrelationContext
{
    private static readonly AsyncLocal<string?> _correlationId = new();
    
    public static string? CorrelationId
    {
        get => _correlationId.Value;
        set => _correlationId.Value = value;
    }
    
    public static IDisposable BeginScope(string? correlationId = null)
    {
        var previousValue = CorrelationId;
        CorrelationId = correlationId ?? Guid.NewGuid().ToString();
        
        return new CorrelationScope(previousValue);
    }
    
    private class CorrelationScope : IDisposable
    {
        private readonly string? _previousValue;
        
        public CorrelationScope(string? previousValue)
        {
            _previousValue = previousValue;
        }
        
        public void Dispose()
        {
            CorrelationId = _previousValue;
        }
    }
}

// Uso
using (CorrelationContext.BeginScope())
{
    Log.Information("CorrelationId: {CorrelationId}", CorrelationContext.CorrelationId);
    
    await ProcessarAsync();  // Mantém o mesmo CorrelationId
}
```

---

## 5. Sinks e Destinos

### 5.1 Console Sink

```csharp
.WriteTo.Console(
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
```

### 5.2 File Sink

```csharp
.WriteTo.File(
    path: "logs/log-.txt",
    rollingInterval: RollingInterval.Day,
    retainedFileCountLimit: 30,  // Manter últimos 30 dias
    fileSizeLimitBytes: 10_000_000,  // 10 MB
    rollOnFileSizeLimit: true)
```

### 5.3 SQL Server Sink

```bash
dotnet add package Serilog.Sinks.MSSqlServer
```

```csharp
.WriteTo.MSSqlServer(
    connectionString: "Server=...;Database=Logs;",
    sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "Logs",
        SchemaName = "dbo",
        AutoCreateSqlTable = true
    })
```

### 5.4 Azure Application Insights

```bash
dotnet add package Serilog.Sinks.ApplicationInsights
```

```csharp
.WriteTo.ApplicationInsights(
    telemetryConfiguration: TelemetryConfiguration.CreateDefault(),
    telemetryConverter: TelemetryConverter.Traces)
```

### 5.5 Elasticsearch / Seq

```bash
dotnet add package Serilog.Sinks.Elasticsearch
dotnet add package Serilog.Sinks.Seq
```

```csharp
// Elasticsearch
.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
{
    IndexFormat = "logs-{0:yyyy.MM.dd}",
    AutoRegisterTemplate = true
})

// Seq (ferramenta local de análise de logs)
.WriteTo.Seq("http://localhost:5341")
```

### 5.6 Múltiplos Sinks com Níveis Diferentes

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    
    // Console: apenas Information+
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Information)
        .WriteTo.Console())
    
    // Arquivo Debug: tudo
    .WriteTo.Logger(lc => lc
        .WriteTo.File("logs/debug-.txt", rollingInterval: RollingInterval.Day))
    
    // Arquivo Errors: apenas Error+
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Error)
        .WriteTo.File("logs/errors-.txt", rollingInterval: RollingInterval.Day))
    
    .CreateLogger();
```

---

## 6. OpenTelemetry e Traces

### 6.1 Instalação

```bash
dotnet add package OpenTelemetry
dotnet add package OpenTelemetry.Exporter.Console
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.AspNetCore
```

### 6.2 Configuração Básica

```csharp
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "MeuServico";
var serviceVersion = "1.0.0";

var serviceProvider = new ServiceCollection()
    .AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .AddSource(serviceName)
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName, serviceVersion))
            .AddConsoleExporter();
    })
    .BuildServiceProvider();
```

### 6.3 Criar Traces e Spans

```csharp
using System.Diagnostics;

public class PedidoService
{
    private static readonly ActivitySource Activity Source = new("MeuServico");
    
    public async Task ProcessarPedidoAsync(int pedidoId)
    {
        using var activity = ActivitySource.StartActivity("ProcessarPedido");
        activity?.SetTag("pedido.id", pedidoId);
        
        try
        {
            await ValidarPedidoAsync(pedidoId);
            await CalcularTotalAsync(pedidoId);
            await SalvarPedidoAsync(pedidoId);
            
            activity?.SetStatus(ActivityStatusCode.Ok);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }
    
    private async Task ValidarPedidoAsync(int pedidoId)
    {
        using var activity = ActivitySource.StartActivity("ValidarPedido");
        activity?.SetTag("pedido.id", pedidoId);
        
        await Task.Delay(50);  // Simula validação
    }
    
    private async Task CalcularTotalAsync(int pedidoId)
    {
        using var activity = ActivitySource.StartActivity("CalcularTotal");
        activity?.SetTag("pedido.id", pedidoId);
        
        await Task.Delay(100);  // Simula cálculo
        
        activity?.SetTag("total", 150.00m);
    }
    
    private async Task SalvarPedidoAsync(int pedidoId)
    {
        using var activity = ActivitySource.StartActivity("SalvarPedido");
        activity?.SetTag("pedido.id", pedidoId);
        activity?.SetTag("database", "SqlServer");
        
        await Task.Delay(75);  // Simula I/O
    }
}
```

**Trace Output:**
```
Activity.Id:          00-abc123-def456-01
Activity.DisplayName: ProcessarPedido
Activity.Kind:        Internal
Activity.StartTime:   2025-10-27T14:32:15.0000000Z
Activity.Duration:    00:00:00.2250000
Activity.Tags:
    pedido.id: 123
Status.Code:          Ok

  Activity.Id:          00-abc123-ghi789-01
  Activity.ParentId:    00-abc123-def456-01
  Activity.DisplayName: ValidarPedido
  Activity.Duration:    00:00:00.0500000
  Activity.Tags:
      pedido.id: 123
  
  Activity.Id:          00-abc123-jkl012-01
  Activity.ParentId:    00-abc123-def456-01
  Activity.DisplayName: CalcularTotal
  Activity.Duration:    00:00:00.1000000
  Activity.Tags:
      pedido.id: 123
      total: 150.00
```

---

## 7. Logging em Produção

### 7.1 Configuração Completa

```csharp
Log.Logger = new LoggerConfiguration()
    // Nível mínimo
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    
    // Enrichers
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithProperty("Application", "MeuApp")
    .Enrich.WithProperty("Version", Assembly.GetExecutingAssembly().GetName().Version?.ToString())
    
    // Console (desenvolvimento)
    .WriteTo.Console()
    
    // Arquivo local (backup)
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        fileSizeLimitBytes: 50_000_000,
        rollOnFileSizeLimit: true)
    
    // Application Insights (produção)
    .WriteTo.ApplicationInsights(
        telemetryConfiguration,
        TelemetryConverter.Traces)
    
    // Seq (análise local/staging)
    .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
    
    .CreateLogger();
```

### 7.2 Filtros e Sampling

```csharp
// Filtrar logs específicos
.Filter.ByExcluding(e => e.MessageTemplate.Text.Contains("HealthCheck"))

// Sampling: logar apenas 10% dos logs Debug
.Filter.ByIncludingOnly(e => 
    e.Level > LogEventLevel.Debug || 
    Random.Shared.Next(100) < 10)

// Excluir propriedades sensíveis
.Destructure.ByTransforming<Usuario>(u => new
{
    u.Id,
    u.Nome,
    Email = MaskEmail(u.Email),
    Senha = "***"  // Nunca logar senhas!
})
```

### 7.3 Performance

```csharp
// ❌ Ruim: concatenação de strings
_logger.LogInformation("Pedido " + pedido.Id + " processado");

// ✅ Bom: message templates
_logger.LogInformation("Pedido {PedidoId} processado", pedido.Id);

// ❌ Ruim: serialização desnecessária
_logger.LogDebug("Pedido: {Pedido}", JsonSerializer.Serialize(pedido));

// ✅ Bom: deixa Serilog fazer
_logger.LogDebug("Pedido: {@Pedido}", pedido);

// ✅ Verificar nível antes de operações caras
if (_logger.IsEnabled(LogLevel.Debug))
{
    var detalhes = GerarRelatorioComplexo();  // Operação cara
    _logger.LogDebug("Detalhes: {@Detalhes}", detalhes);
}
```

---

## 8. Melhores Práticas

### ✅ DO: Use Logging Estruturado

```csharp
// ❌ String interpolation
_logger.LogInformation($"Pedido {pedido.Id} criado");

// ✅ Message templates
_logger.LogInformation("Pedido {PedidoId} criado", pedido.Id);
```

### ✅ DO: Log Exceções Completas

```csharp
try
{
    ProcessarPagamento(pedido);
}
catch (Exception ex)
{
    // ✅ Passa exceção como primeiro parâmetro
    _logger.LogError(ex, "Erro ao processar pagamento do pedido {PedidoId}", pedido.Id);
    throw;
}
```

### ✅ DO: Use Níveis Apropriados

```csharp
// Information: Eventos de negócio importantes
_logger.LogInformation("Pedido {PedidoId} aprovado", pedido.Id);

// Warning: Situação não ideal
_logger.LogWarning("Estoque baixo do produto {ProdutoId}: {Quantidade} unidades", id, qtd);

// Error: Falhas recuperáveis
_logger.LogError("Falha ao enviar email para {Email}", email);

// Critical: Sistema comprometido
_logger.LogCritical("Banco de dados inacessível há {Minutos} minutos", minutos);
```

### ✅ DO: Adicione Contexto

```csharp
using (_logger.BeginScope(new Dictionary<string, object>
{
    ["UserId"] = userId,
    ["TenantId"] = tenantId,
    ["Operation"] = "ProcessarPedido"
}))
{
    // Todos os logs terão essas propriedades
    _logger.LogInformation("Iniciando processamento");
}
```

### ⚠️ DON'T: Log Dados Sensíveis

```csharp
// ❌ NUNCA logue senhas, tokens, cartões de crédito
_logger.LogInformation("Login: {Email} {Senha}", email, senha);

// ✅ Logue apenas dados não sensíveis
_logger.LogInformation("Tentativa de login: {Email}", email);

// ✅ Mascare dados sensíveis
_logger.LogInformation("Cartão: ****{UltimosDigitos}", cartao.Substring(cartao.Length - 4));
```

### ⚠️ DON'T: Log em Excesso

```csharp
// ❌ Log desnecessário em loop
foreach (var item in lista)
{
    _logger.LogDebug("Processando item {ItemId}", item.Id);  // 10.000 logs!
}

// ✅ Log resumido
_logger.LogInformation("Processando {Quantidade} itens", lista.Count);
// ... processar ...
_logger.LogInformation("Itens processados: {Sucesso}/{Total}", sucesso, lista.Count);
```

---

## 🎓 Resumo

Você aprendeu:

1. **Fundamentos:** Níveis de log, quando usar cada um
2. **ILogger:** Microsoft.Extensions.Logging, DI, scopes
3. **Serilog:** Logging estruturado, enrichers, JSON
4. **Correlação:** Correlation IDs, contexto distribuído
5. **Sinks:** Console, File, Database, Cloud
6. **OpenTelemetry:** Traces distribuídos, spans, tags
7. **Produção:** Configuração completa, performance, segurança
8. **Boas Práticas:** O que fazer e evitar

**Próximo tópico:** Best Practices - Padrões de Resiliência e Anti-patterns
