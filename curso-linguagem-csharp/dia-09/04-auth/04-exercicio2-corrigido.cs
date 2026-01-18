// Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserPremium", policy =>
        policy.RequireClaim("subscription", "premium"));

    options.AddPolicy("DepartmentManager", policy =>
        policy.RequireRole("Manager")
              .RequireClaim("department", "IT", "HR", "Finance"));
});

// Controllers com diferentes níveis de autorização
[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    [HttpGet("usuarios")]
    public IActionResult GetAllUsers()
    {
        return Ok(new[] { "User1", "User2", "User3" });
    }

    [HttpDelete("usuarios/{id}")]
    public IActionResult DeleteUser(int id)
    {
        return Ok(new { message = $"Usuário {id} deletado" });
    }
}

[ApiController]
[Route("api/premium")]
[Authorize(Policy = "UserPremium")]
public class PremiumController : ControllerBase
{
    [HttpGet("relatorios")]
    public IActionResult GetRelatorios()
    {
        return Ok(new[] { "Relatório 1", "Relatório 2" });
    }

    [HttpPost("exportar")]
    public IActionResult ExportarDados()
    {
        return Ok(new { message = "Dados exportados" });
    }
}

[ApiController]
[Route("api/departamento")]
[Authorize(Policy = "DepartmentManager")]
public class DepartamentoController : ControllerBase
{
    [HttpGet("funcionarios")]
    public IActionResult GetFuncionarios()
    {
        var department = User.FindFirst("department")?.Value;
        return Ok(new { department, funcionarios = new[] { "Func1", "Func2" } });
    }
}

// Middleware customizado para logging
public class AuthorizationLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationLoggingMiddleware> _logger;

    public AuthorizationLoggingMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationLoggingMiddleware> _logger)
    {
        _next = next;
        this._logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

        if (authorizeAttribute != null)
        {
            var user = context.User;
            var userName = user.Identity?.Name ?? "Anônimo";
            var path = context.Request.Path;
            var method = context.Request.Method;

            _logger.LogInformation(
                "Acesso autorizado - Usuário: {User}, Método: {Method}, Path: {Path}",
                userName, method, path);
        }

        await _next(context);
    }
}

// Program.cs (registro do middleware)
app.UseMiddleware<AuthorizationLoggingMiddleware>();