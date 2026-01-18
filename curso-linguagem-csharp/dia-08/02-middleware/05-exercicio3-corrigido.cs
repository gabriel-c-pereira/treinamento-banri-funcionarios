public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly IMemoryCache _cache;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IMemoryCache cache)
    {
        _next = next;
        _logger = logger;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = GetClientIdentifier(context);
        var cacheKey = $"rate_limit_{clientId}";

        // Verificar se o cliente está bloqueado
        if (_cache.TryGetValue($"blocked_{clientId}", out _))
        {
            context.Response.StatusCode = 429;
            context.Response.Headers["Retry-After"] = "60";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Muitas requisições. Tente novamente em 1 minuto.",
                retryAfter = 60
            });
            return;
        }

        // Obter ou criar contador de requisições
        var requestCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return Task.FromResult(0);
        });

        // Verificar limite
        if (requestCount >= 10) // 10 requisições por minuto
        {
            // Bloquear por 1 minuto
            _cache.Set($"blocked_{clientId}", true, TimeSpan.FromMinutes(1));

            _logger.LogWarning("Rate limit excedido para cliente: {ClientId}", clientId);

            context.Response.StatusCode = 429;
            context.Response.Headers["Retry-After"] = "60";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Limite de requisições excedido. Tente novamente em 1 minuto.",
                retryAfter = 60,
                limit = 10,
                window = "1 minuto"
            });
            return;
        }

        // Incrementar contador
        _cache.Set(cacheKey, requestCount + 1, TimeSpan.FromMinutes(1));

        // Adicionar headers informativos
        context.Response.Headers["X-RateLimit-Limit"] = "10";
        context.Response.Headers["X-RateLimit-Remaining"] = (9 - requestCount).ToString();
        context.Response.Headers["X-RateLimit-Reset"] = DateTime.UtcNow.AddMinutes(1).ToString("O");

        await _next(context);
    }

    private string GetClientIdentifier(HttpContext context)
    {
        // Priorizar header X-Forwarded-For (proxy/load balancer)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        // Fallback para IP remoto
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}

// Extension method
public static class RateLimitingMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}

// Uso no Program.cs
var app = builder.Build();

// Adicionar após autenticação mas antes de endpoints
app.UseAuthentication();
app.UseRateLimiting();
app.UseAuthorization();

// ═══════════════════════════════════════════════════

public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    // Adicionar serviços
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();

    var app = builder.Build();

    // 1. Tratamento de erros (primeiro)
    app.UseGlobalExceptionHandler();

    // 2. HTTPS
    app.UseHttpsRedirection();

    // 3. Arquivos estáticos
    app.UseStaticFiles();

    // 4. CORS
    app.UseCors();

    // 5. Rate limiting
    app.UseRateLimiting();

    // 6. Roteamento
    app.UseRouting();

    // 7. Autenticação e autorização
    app.UseAuthentication();
    app.UseAuthorization();

    // 8. Logging customizado
    app.UseRequestLogging();

    // 9. Endpoints
    app.MapControllers();

    app.Run();
}