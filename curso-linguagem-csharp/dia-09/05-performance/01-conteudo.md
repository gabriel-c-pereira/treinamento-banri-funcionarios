# Performance e Escalabilidade

## Caching

### In-Memory Cache
```csharp
// Program.cs
builder.Services.AddMemoryCache();

// Service
public class ProdutoService : IProdutoService
{
    private readonly IMemoryCache _cache;
    private readonly IProdutoRepository _repository;
    private const string CacheKey = "produtos";

    public ProdutoService(IMemoryCache cache, IProdutoRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out List<Produto> produtos))
        {
            produtos = await _repository.GetAllAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(CacheKey, produtos, cacheOptions);
        }

        return produtos;
    }

    public async Task InvalidateCacheAsync()
    {
        _cache.Remove(CacheKey);
    }
}
```

### Distributed Cache (Redis)
```csharp
// Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Service
public class CacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions();

        if (expiry.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiry;

        await _cache.SetStringAsync(key, json, options);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);
        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }
}
```

## Response Caching

### Controller Level
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    [HttpGet]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _service.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _service.GetByIdAsync(id);
        return Ok(produto);
    }
}
```

### Middleware Configuration
```csharp
// Program.cs
builder.Services.AddResponseCaching();

app.UseResponseCaching();
```

## Output Caching (ASP.NET Core 7+)

```csharp
// Program.cs
builder.Services.AddOutputCache();

// Controller
[ApiController]
[Route("api/[controller]")]
[OutputCache]
public class RelatoriosController : ControllerBase
{
    [HttpGet("vendas")]
    [OutputCache(Duration = 300)]
    public async Task<IActionResult> GetRelatorioVendas()
    {
        var relatorio = await _service.GerarRelatorioVendasAsync();
        return Ok(relatorio);
    }

    [HttpGet("vendas/{ano}")]
    [OutputCache(Duration = 600, VaryByQueryKeys = new[] { "ano" })]
    public async Task<IActionResult> GetRelatorioVendasPorAno(int ano)
    {
        var relatorio = await _service.GerarRelatorioVendasPorAnoAsync(ano);
        return Ok(relatorio);
    }
}
```

## Database Optimization

### Connection Pooling
```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MinhaDB;Trusted_Connection=True;Max Pool Size=100;Min Pool Size=5;"
  }
}
```

### Query Optimization
```csharp
public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _context;

    public ProdutoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetProdutosAtivosAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo)
            .AsNoTracking() // Não track mudanças
            .ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosComCategoriaAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria) // Eager loading
            .Where(p => p.Ativo)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosPaginadosAsync(int page, int pageSize)
    {
        return await _context.Produtos
            .OrderBy(p => p.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }
}
```

## Compression

```csharp
// Program.cs
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/json", "application/xml" });
});

app.UseResponseCompression();
```

## Rate Limiting

```csharp
// Program.cs
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter("uploads", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
        opt.SegmentsPerWindow = 6;
    });
});

// Controller
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("api")]
public class ProdutosController : ControllerBase
{
    [HttpPost("upload")]
    [EnableRateLimiting("uploads")]
    public async Task<IActionResult> UploadArquivo(IFormFile file)
    {
        // Processamento do upload
        return Ok();
    }
}
```

## Monitoring e Diagnostics

### Health Checks
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddRedis(builder.Configuration.GetConnectionString("Redis"));

// Controller
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        return report.Status == HealthStatus.Healthy
            ? Ok(report)
            : StatusCode(503, report);
    }
}
```

### Performance Monitoring
```csharp
public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;

    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> _logger)
    {
        _next = next;
        this._logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var start = Stopwatch.GetTimestamp();
        var path = context.Request.Path;
        var method = context.Request.Method;

        try
        {
            await _next(context);
        }
        finally
        {
            var elapsed = Stopwatch.GetElapsedTime(start);
            _logger.LogInformation(
                "Request {Method} {Path} completed in {Elapsed}ms with status {Status}",
                method, path, elapsed.TotalMilliseconds, context.Response.StatusCode);

            if (elapsed.TotalMilliseconds > 1000) // Log slow requests
            {
                _logger.LogWarning(
                    "Slow request detected: {Method} {Path} took {Elapsed}ms",
                    method, path, elapsed.TotalMilliseconds);
            }
        }
    }
}
```
