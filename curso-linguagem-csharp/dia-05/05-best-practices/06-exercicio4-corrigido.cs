// Middleware customizado
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, 
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação");
            await HandleValidationException(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado");
            await HandleNotFoundException(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");
            await HandleUnexpectedException(context, ex);
        }
    }

    private static async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new
        {
            type = "ValidationError",
            title = "Erro de Validação",
            status = 400,
            errors = ex.Errors
        });
    }

    private static async Task HandleNotFoundException(HttpContext context, NotFoundException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new
        {
            type = "NotFound",
            title = "Recurso Não Encontrado",
            status = 404,
            detail = ex.Message
        });
    }

    private static async Task HandleUnexpectedException(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            type = "InternalError",
            title = "Erro Interno do Servidor",
            status = 500,
            detail = "Ocorreu um erro inesperado. Tente novamente mais tarde."
        });
    }
}

// Registrar no Program.cs
app.UseMiddleware<GlobalExceptionMiddleware>();