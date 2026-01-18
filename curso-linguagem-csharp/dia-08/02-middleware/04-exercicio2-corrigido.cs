public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, ex, 400, "Dados inválidos");
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, ex, 401, "Acesso não autorizado");
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, 404, "Recurso não encontrado");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, 500, "Erro interno do servidor");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode, string message)
    {
        _logger.LogError(ex, "Erro: {Message}", ex.Message);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            error = message,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path.ToString(),
            method = context.Request.Method
        };

        // Em desenvolvimento, incluir detalhes do erro
        if (_environment.IsDevelopment())
        {
            errorResponse = new
            {
                error = message,
                timestamp = DateTime.UtcNow,
                path = context.Request.Path.ToString(),
                method = context.Request.Method,
                details = ex.Message,
                stackTrace = ex.StackTrace
            };
        }

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}

// Extension method
public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionMiddleware>();
    }
}

// Uso no Program.cs
var app = builder.Build();
app.UseGlobalExceptionHandler(); // Deve ser o primeiro middleware