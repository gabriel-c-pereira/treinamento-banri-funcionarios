# 📝 Exercícios - Dependency Injection

## 🎯 Objetivo

Praticar a configuração e uso do container de DI (Dependency Injection) no ASP.NET Core, incluindo diferentes lifetimes, injeção de dependências e padrões como Options Pattern.

---

## ✏️ Exercício 1: Configuração Básica de Serviços

**Dificuldade**: ⭐ Iniciante

Configure serviços com diferentes lifetimes no `Program.cs`:

### Requisitos:
1. **Serviços a implementar**:
```csharp
// Transient - nova instância a cada injeção
public interface IOperacaoRapida
{
    Guid Id { get; }
    string Executar();
}

public class OperacaoRapida : IOperacaoRapida
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Executar() => $"Operação rápida: {Id}";
}

// Scoped - mesma instância durante a requisição
public interface IRepositorio
{
    Guid Id { get; }
    Task<List<string>> ObterDadosAsync();
}

public class Repositorio : IRepositorio
{
    public Guid Id { get; } = Guid.NewGuid();
    public async Task<List<string>> ObterDadosAsync()
    {
        await Task.Delay(100); // Simula acesso a dados
        return new List<string> { "Dado1", "Dado2", "Dado3" };
    }
}

// Singleton - mesma instância para toda aplicação
public interface IConfiguracaoGlobal
{
    Guid Id { get; }
    string Ambiente { get; }
    DateTime InicioAplicacao { get; }
}

public class ConfiguracaoGlobal : IConfiguracaoGlobal
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Ambiente { get; }
    public DateTime InicioAplicacao { get; } = DateTime.UtcNow;

    public ConfiguracaoGlobal()
    {
        Ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    }
}
```

2. **Configuração no Program.cs**:
```csharp
builder.Services.AddTransient<IOperacaoRapida, OperacaoRapida>();
builder.Services.AddScoped<IRepositorio, Repositorio>();
builder.Services.AddSingleton<IConfiguracaoGlobal, ConfiguracaoGlobal>();
```

3. **Controller para testar**:
```csharp
[ApiController]
[Route("api/teste-di")]
public class TesteDIController : ControllerBase
{
    private readonly IOperacaoRapida _operacao1;
    private readonly IOperacaoRapida _operacao2;
    private readonly IRepositorio _repositorio;
    private readonly IConfiguracaoGlobal _config;

    public TesteDIController(
        IOperacaoRapida operacao1,
        IOperacaoRapida operacao2,
        IRepositorio repositorio,
        IConfiguracaoGlobal config)
    {
        _operacao1 = operacao1;
        _operacao2 = operacao2;
        _repositorio = repositorio;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> TestarLifetimes()
    {
        return Ok(new
        {
            OperacaoRapida1 = new { Id = _operacao1.Id, Resultado = _operacao1.Executar() },
            OperacaoRapida2 = new { Id = _operacao2.Id, Resultado = _operacao2.Executar() },
            Repositorio = new { Id = _repositorio.Id },
            Configuracao = new { Id = _config.Id, Ambiente = _config.Ambiente, Inicio = _config.InicioAplicacao }
        });
    }
}
```

---

## ✏️ Exercício 2: Injeção por Construtor e Múltiplas Dependências

**Dificuldade**: ⭐⭐ Intermediário

Implemente um serviço complexo que recebe múltiplas dependências:

### Requisitos:
1. **Interfaces e implementações**:
```csharp
public interface IEmailService
{
    Task EnviarEmailAsync(string destinatario, string assunto, string mensagem);
}

public interface ISmsService
{
    Task EnviarSmsAsync(string numero, string mensagem);
}

public interface ILogService
{
    Task LogAsync(string mensagem, LogLevel nivel = LogLevel.Information);
}

public interface INotificacaoService
{
    Task EnviarNotificacaoAsync(string usuarioId, string titulo, string mensagem);
}

// Implementações...
public class EmailService : IEmailService
{
    private readonly ILogService _logger;
    public EmailService(ILogService logger) => _logger = logger;

    public async Task EnviarEmailAsync(string destinatario, string assunto, string mensagem)
    {
        await _logger.LogAsync($"Enviando email para {destinatario}");
        // Lógica de envio...
    }
}

public class SmsService : ISmsService
{
    private readonly ILogService _logger;
    public SmsService(ILogService logger) => _logger = logger;

    public async Task EnviarSmsAsync(string numero, string mensagem)
    {
        await _logger.LogAsync($"Enviando SMS para {numero}");
        // Lógica de envio...
    }
}

public class LogService : ILogService
{
    public async Task LogAsync(string mensagem, LogLevel nivel = LogLevel.Information)
    {
        await Task.Run(() => Console.WriteLine($"[{nivel}] {DateTime.UtcNow}: {mensagem}"));
    }
}
```

2. **Serviço de Notificação (alta dependência)**:
```csharp
public class NotificacaoService : INotificacaoService
{
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly ILogService _logService;

    public NotificacaoService(
        IEmailService emailService,
        ISmsService smsService,
        ILogService logService)
    {
        _emailService = emailService;
        _smsService = smsService;
        _logService = logService;
    }

    public async Task EnviarNotificacaoAsync(string usuarioId, string titulo, string mensagem)
    {
        await _logService.LogAsync($"Enviando notificação para usuário {usuarioId}");

        // Envia email
        await _emailService.EnviarEmailAsync($"{usuarioId}@dominio.com", titulo, mensagem);

        // Envia SMS (se número disponível)
        await _smsService.EnviarSmsAsync("+5511999999999", $"{titulo}: {mensagem}");

        await _logService.LogAsync("Notificação enviada com sucesso");
    }
}
```

3. **Configuração no DI Container**:
```csharp
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
```

---

## ✏️ Exercício 3: Options Pattern

**Dificuldade**: ⭐⭐⭐ Avançado

Configure e use o Options Pattern para configurações da aplicação:

### Requisitos:
1. **Classes de configuração**:
```csharp
public class AppSettings
{
    public string NomeAplicacao { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public EmailConfig Email { get; set; }
    public CacheConfig Cache { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
    public string RedisConnection { get; set; }
}

public class EmailConfig
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }
    public bool UsarSsl { get; set; }
}

public class CacheConfig
{
    public int DuracaoMinutos { get; set; }
    public bool Habilitado { get; set; }
    public string Provider { get; set; }
}
```

2. **Arquivo appsettings.json**:
```json
{
  "AppSettings": {
    "NomeAplicacao": "MinhaAplicacao",
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=MinhaDB;Trusted_Connection=True;",
      "RedisConnection": "localhost:6379"
    },
    "Email": {
      "SmtpServer": "smtp.gmail.com",
      "SmtpPort": 587,
      "Usuario": "meuemail@gmail.com",
      "Senha": "minhasenha",
      "UsarSsl": true
    },
    "Cache": {
      "DuracaoMinutos": 30,
      "Habilitado": true,
      "Provider": "Redis"
    }
  }
}
```

3. **Configuração no Program.cs**:
```csharp
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
```

4. **Serviço que usa as configurações**:
```csharp
public class ConfiguracaoService
{
    private readonly AppSettings _config;
    private readonly IConfiguration _configuration;

    public ConfiguracaoService(IOptions<AppSettings> options, IConfiguration configuration)
    {
        _config = options.Value;
        _configuration = configuration;
    }

    public void ExibirConfiguracoes()
    {
        Console.WriteLine($"Aplicação: {_config.NomeAplicacao}");
        Console.WriteLine($"DB Connection: {_config.ConnectionStrings.DefaultConnection}");
        Console.WriteLine($"Email Server: {_config.Email.SmtpServer}:{_config.Email.SmtpPort}");
        Console.WriteLine($"Cache: {_config.Cache.Habilitado} ({_config.Cache.Provider})");
    }

    public string ObterConfiguracaoDireta(string chave)
    {
        return _configuration[chave];
    }
}
```

5. **Uso em Controller**:
```csharp
[ApiController]
[Route("api/config")]
public class ConfigController : ControllerBase
{
    private readonly AppSettings _config;
    private readonly ConfiguracaoService _service;

    public ConfigController(IOptions<AppSettings> options, ConfiguracaoService service)
    {
        _config = options.Value;
        _service = service;
    }

    [HttpGet]
    public IActionResult ObterConfig() => Ok(_config);

    [HttpGet("email")]
    public IActionResult ObterEmailConfig() => Ok(_config.Email);
}
```
