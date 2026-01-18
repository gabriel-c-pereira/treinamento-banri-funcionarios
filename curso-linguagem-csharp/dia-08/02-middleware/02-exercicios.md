# 📝 Exercícios - Middleware

## 🎯 Objetivo

Praticar a criação e configuração de middleware personalizado no ASP.NET Core para logging, tratamento de erros, autenticação, rate limiting e outras funcionalidades transversais.

---

## ✏️ Exercício 1: Middleware de Logging

**Dificuldade**: ⭐ Iniciante

Crie um middleware que registre todas as requisições HTTP com informações detalhadas:

### Requisitos:
1. **Classe LoggingMiddleware**:
```csharp
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Registra início da requisição
        var startTime = DateTime.UtcNow;
        _logger.LogInformation($"Requisição iniciada: {context.Request.Method} {context.Request.Path}");

        // Chama o próximo middleware
        await _next(context);

        // Registra fim da requisição
        var duration = DateTime.UtcNow - startTime;
        _logger.LogInformation($"Requisição concluída: {context.Response.StatusCode} em {duration.TotalMilliseconds}ms");
    }
}
```

2. **Extensão para configuração**:
```csharp
public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
```

3. **Configuração no Program.cs**:
```csharp
app.UseLoggingMiddleware();
```

4. **Funcionalidades extras**:
   - Registrar headers importantes
   - Registrar corpo da requisição (opcional, com cuidado)
   - Categorizar logs por nível (Information, Warning, Error)

---

## ✏️ Exercício 2: Middleware de Tratamento de Erros

**Dificuldade**: ⭐⭐ Intermediário

Implemente um middleware global de tratamento de erros:

### Requisitos:
1. **Modelo de resposta de erro**:
```csharp
public class ErrorResponse
{
    public string Message { get; set; }
    public string Detail { get; set; }
    public int StatusCode { get; set; }
    public string TraceId { get; set; }
    public DateTime Timestamp { get; set; }

    public ErrorResponse(int statusCode, string message, string detail = null)
    {
        StatusCode = statusCode;
        Message = message;
        Detail = detail;
        Timestamp = DateTime.UtcNow;
        TraceId = Guid.NewGuid().ToString();
    }
}
```

2. **Classe ErrorHandlingMiddleware**:
```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var errorResponse = new ErrorResponse(statusCode, exception.Message);

        // Log do erro
        _logger.LogError(exception, $"Erro não tratado: {exception.Message}");

        // Configura resposta
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            KeyNotFoundException => 404,
            _ => 500
        };
    }
}
```

3. **Configuração**:
   - Deve ser o primeiro middleware (exceto DeveloperExceptionPage)
   - Capturar apenas exceções não tratadas
   - Não interferir em respostas já iniciadas

---

## ✏️ Exercício 3: Middleware de Rate Limiting

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um middleware que limite o número de requisições por IP:

### Requisitos:
1. **Classe RateLimitingMiddleware**:
```csharp
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    private const int MaxRequestsPerMinute = 10;
    private const string CacheKeyPrefix = "RateLimit_";

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _cache = cache;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = GetClientIp(context);
        var cacheKey = $"{CacheKeyPrefix}{clientIp}";

        var requestCount = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return 0;
        });

        if (requestCount >= MaxRequestsPerMinute)
        {
            _logger.LogWarning($"Rate limit excedido para IP: {clientIp}");
            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.Headers["Retry-After"] = "60";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Muitas requisições",
                message = "Limite de 10 requisições por minuto excedido",
                retryAfter = 60
            });
            return;
        }

        // Incrementa contador
        _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromMinutes(1));

        // Adiciona headers informativos
        context.Response.Headers["X-RateLimit-Limit"] = MaxRequestsPerMinute.ToString();
        context.Response.Headers["X-RateLimit-Remaining"] = (MaxRequestsPerMinute - requestCount - 1).ToString();

        await _next(context);
    }

    private string GetClientIp(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
```

2. **Funcionalidades avançadas**:
   - Configuração por endpoint (alguns endpoints podem ter limites diferentes)
   - Whitelist de IPs
   - Headers informativos sobre limite
   - Persistência em Redis (opcional)

3. **Configuração**:
   - Usar IMemoryCache para armazenamento temporário
   - Configurar expiração por minuto
   - Retornar status 429 quando exceder limite
