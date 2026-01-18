# Middleware

## O que é Middleware?

Middleware são componentes que formam um pipeline de processamento de requisições HTTP. Cada middleware pode:

- Processar a requisição antes de passar para o próximo
- Modificar a resposta antes de retornar ao cliente
- Encerrar o pipeline (short-circuiting)

## Pipeline de Requisições

```
Requisição → Middleware1 → Middleware2 → ... → Controller → ... → MiddlewareN → Resposta
```

## Criando Middleware Customizado

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
        // Antes da requisição
        _logger.LogInformation($"Requisição: {context.Request.Method} {context.Request.Path}");

        var stopwatch = Stopwatch.StartNew();

        // Chama o próximo middleware
        await _next(context);

        // Depois da resposta
        stopwatch.Stop();
        _logger.LogInformation($"Resposta: {context.Response.StatusCode} em {stopwatch.ElapsedMilliseconds}ms");
    }
}

// Extension method para registro
public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }
}
```

## Middleware Comum

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    var app = builder.Build();

    // Middleware de autenticação
    app.UseAuthentication();
    app.UseAuthorization();

    // Middleware de CORS
    app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    // Middleware de arquivos estáticos
    app.UseStaticFiles();

    // Middleware de roteamento
    app.UseRouting();

    // Middleware customizado
    app.UseLoggingMiddleware();

    // Middleware de endpoints
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.Run();
}
```

## Middleware de Tratamento de Erros

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Erro não tratado");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var error = new
            {
                error = "Erro interno do servidor",
                message = ex.Message,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
```

## Middleware de Rate Limiting

```csharp
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private static readonly Dictionary<string, (int Count, DateTime ResetTime)> _requests = new();

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        // Limpar requests antigos
        if (_requests.TryGetValue(clientId, out var data) && now > data.ResetTime)
        {
            _requests.Remove(clientId);
            data = (0, now.AddMinutes(1));
        }

        // Verificar limite
        if (data.Count >= 10) // 10 requests por minuto
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsJsonAsync(new { error = "Muitas requisições" });
            return;
        }

        // Incrementar contador
        _requests[clientId] = (data.Count + 1, data.ResetTime);

        await _next(context);
    }
}
```

## Ordem dos Middlewares

A ordem é importante! Middlewares são executados na ordem em que são registrados:

```csharp
app.UseExceptionHandler("/error");     // 1. Tratamento de erros
app.UseHttpsRedirection();             // 2. HTTPS
app.UseStaticFiles();                  // 3. Arquivos estáticos
app.UseRouting();                      // 4. Roteamento
app.UseAuthentication();               // 5. Autenticação
app.UseAuthorization();                // 6. Autorização
app.UseEndpoints(endpoints => ...);    // 7. Endpoints
```

## Middleware Condicional

```csharp
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    appBuilder => appBuilder.UseMiddleware<ApiLoggingMiddleware>()
);
```

## Middleware de Desenvolvimento

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
```
