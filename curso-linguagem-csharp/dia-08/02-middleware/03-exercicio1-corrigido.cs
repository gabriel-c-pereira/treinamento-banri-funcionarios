public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew();

        // Log da requisição
        _logger.LogInformation(
            "Requisição iniciada: {Method} {Path} às {StartTime}",
            context.Request.Method,
            context.Request.Path,
            startTime);

        try
        {
            // Processar requisição
            await _next(context);

            stopwatch.Stop();

            // Log da resposta
            _logger.LogInformation(
                "Requisição concluída: {Method} {Path} - Status: {StatusCode} - Tempo: {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(ex,
                "Erro na requisição: {Method} {Path} - Tempo: {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}

// Extension method
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}

// Uso no Program.cs
var app = builder.Build();
app.UseRequestLogging(); // Adicionar antes do roteamento