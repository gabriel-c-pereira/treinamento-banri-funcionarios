// Classe de configurações
public class AppSettings
{
    public DatabaseSettings Database { get; set; }
    public EmailSettings Email { get; set; }
    public CacheSettings Cache { get; set; }
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public int CommandTimeout { get; set; } = 30;
    public bool EnableRetryOnFailure { get; set; } = true;
    public int MaxRetryCount { get; set; } = 3;
}

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string Username { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
}

public class CacheSettings
{
    public string RedisConnection { get; set; }
    public int DefaultExpirationMinutes { get; set; } = 60;
    public bool EnableCompression { get; set; } = false;
}

// Configuração no Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configurar options
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

builder.Services.AddControllers();

// Serviço usando DatabaseSettings
public class ProdutoRepository : IProdutoRepository
{
    private readonly string _connectionString;
    private readonly int _commandTimeout;

    public ProdutoRepository(IOptions<DatabaseSettings> dbOptions)
    {
        _connectionString = dbOptions.Value.ConnectionString;
        _commandTimeout = dbOptions.Value.CommandTimeout;
    }

    public async Task<Produto> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT * FROM Produtos WHERE Id = @Id", connection);
        command.CommandTimeout = _commandTimeout;
        command.Parameters.AddWithValue("@Id", id);

        // ... executar query
    }
}