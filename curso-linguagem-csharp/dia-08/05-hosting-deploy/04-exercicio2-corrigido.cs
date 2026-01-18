// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Adicionar health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "Database",
        tags: new[] { "db", "sql" })
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis"),
        name: "Redis Cache",
        tags: new[] { "cache", "redis" });

// Health checks customizados
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Custom Database Check");

var app = builder.Build();

// Mapear endpoints de health check
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.Run();

// Custom health check
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly EcommerceDbContext _context;

    public DatabaseHealthCheck(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Database.CanConnectAsync(cancellationToken);
            var count = await _context.Produtos.CountAsync(cancellationToken);

            return HealthCheckResult.Healthy($"Database healthy. Products: {count}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database unhealthy", ex);
        }
    }
}