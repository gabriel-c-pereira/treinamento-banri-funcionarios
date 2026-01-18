# Dependency Injection

## O que é DI?

Dependency Injection (DI) é um padrão de design que permite injetar dependências em uma classe, ao invés de criá-las internamente.

## Lifetimes

- **Transient**: Nova instância a cada injeção
- **Scoped**: Uma instância por requisição
- **Singleton**: Uma instância para toda aplicação

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Transient - sempre nova instância
    services.AddTransient<ITransientService, TransientService>();

    // Scoped - uma por requisição
    services.AddScoped<IScopedService, ScopedService>();

    // Singleton - uma para toda aplicação
    services.AddSingleton<ISingletonService, SingletonService>();
}
```

## Injeção por Construtor

```csharp
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;
    private readonly ILogger<ProdutoService> _logger;

    public ProdutoService(
        IProdutoRepository repository,
        ILogger<ProdutoService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
```

## Injeção por Método/Action

```csharp
[ApiController]
public class ProdutosController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromServices] IProdutoService service, [FromBody] ProdutoDto produto)
    {
        // service é injetado automaticamente
        return Ok();
    }
}
```

## Factory Pattern com DI

```csharp
public interface IServiceFactory<T>
{
    T Create();
}

public class ProdutoServiceFactory : IServiceFactory<IProdutoService>
{
    private readonly IServiceProvider _serviceProvider;

    public ProdutoServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IProdutoService Create()
    {
        return _serviceProvider.GetRequiredService<IProdutoService>();
    }
}
```

## Options Pattern

```csharp
public class AppSettings
{
    public string ConnectionString { get; set; }
    public int MaxItems { get; set; }
}

public void ConfigureServices(IServiceCollection services)
{
    services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
}

public class ProdutoService
{
    private readonly AppSettings _settings;

    public ProdutoService(IOptions<AppSettings> options)
    {
        _settings = options.Value;
    }
}
```
