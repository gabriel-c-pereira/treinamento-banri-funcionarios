public interface IDynamicConfigurationBuilder
{
    dynamic Build();
    IDynamicConfigurationBuilder AddProperty(string name, object value);
    IDynamicConfigurationBuilder AddMethod(string name, Delegate method);
    IDynamicConfigurationBuilder AddValidator(string propertyName, Func<object, bool> validator);
    bool Validate();
}

public class DynamicConfigurationBuilder : IDynamicConfigurationBuilder
{
    private readonly ExpandoObject _configuration = new ExpandoObject();
    private readonly Dictionary<string, Func<object, bool>> _validators = new();

    public dynamic Build()
    {
        return _configuration;
    }

    public IDynamicConfigurationBuilder AddProperty(string name, object value)
    {
        var dict = (IDictionary<string, object>)_configuration;
        dict[name] = value;
        return this;
    }

    public IDynamicConfigurationBuilder AddMethod(string name, Delegate method)
    {
        var dict = (IDictionary<string, object>)_configuration;
        dict[name] = method;
        return this;
    }

    public IDynamicConfigurationBuilder AddValidator(string propertyName, Func<object, bool> validator)
    {
        _validators[propertyName] = validator;
        return this;
    }

    public bool Validate()
    {
        var dict = (IDictionary<string, object>)_configuration;

        foreach (var validator in _validators)
        {
            if (dict.TryGetValue(validator.Key, out var value))
            {
                if (!validator.Value(value))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

public class ConfigurationService
{
    private readonly IDynamicConfigurationBuilder _builder;

    public ConfigurationService(IDynamicConfigurationBuilder builder)
    {
        _builder = builder;
    }

    public dynamic CreateDatabaseConfiguration()
    {
        return _builder
            .AddProperty("ConnectionString", "Server=localhost;Database=TestDB;")
            .AddProperty("Timeout", 30)
            .AddProperty("MaxPoolSize", 100)
            .AddValidator("Timeout", value => (int)value > 0)
            .AddValidator("MaxPoolSize", value => (int)value > 0 && (int)value <= 1000)
            .AddMethod("TestConnection", (Func<Task<bool>>)(async () =>
            {
                // Simulação de teste de conexão
                await Task.Delay(100);
                return true;
            }))
            .Build();
    }

    public dynamic CreateEmailConfiguration()
    {
        return _builder
            .AddProperty("SmtpServer", "smtp.gmail.com")
            .AddProperty("Port", 587)
            .AddProperty("UseSsl", true)
            .AddProperty("Username", "user@example.com")
            .AddProperty("Password", "")
            .AddValidator("Port", value => (int)value > 0 && (int)value <= 65535)
            .AddValidator("Username", value => !string.IsNullOrEmpty((string)value))
            .AddMethod("SendTestEmail", (Func<string, Task<bool>>)(async (to) =>
            {
                // Simulação de envio de email
                Console.WriteLine($"Enviando email de teste para: {to}");
                await Task.Delay(200);
                return true;
            }))
            .Build();
    }

    public dynamic CreateApiConfiguration()
    {
        return _builder
            .AddProperty("BaseUrl", "https://api.example.com")
            .AddProperty("Timeout", 60)
            .AddProperty("Retries", 3)
            .AddProperty("ApiKey", "")
            .AddValidator("BaseUrl", value => Uri.IsWellFormedUriString((string)value, UriKind.Absolute))
            .AddValidator("Timeout", value => (int)value > 0)
            .AddValidator("Retries", value => (int)value >= 0 && (int)value <= 10)
            .AddMethod("HealthCheck", (Func<Task<bool>>)(async () =>
            {
                // Simulação de health check
                await Task.Delay(150);
                return true;
            }))
            .Build();
    }
}

public class ConfigurationManager
{
    private readonly ConfigurationService _configService;

    public ConfigurationManager(ConfigurationService configService)
    {
        _configService = configService;
    }

    public async Task InitializeConfigurationsAsync()
    {
        // Configuração de banco de dados
        dynamic dbConfig = _configService.CreateDatabaseConfiguration();
        if (!((IDynamicConfigurationBuilder)_configService).Validate())
        {
            throw new InvalidOperationException("Configuração de banco inválida");
        }

        Console.WriteLine($"DB Connection: {dbConfig.ConnectionString}");
        Console.WriteLine($"DB Timeout: {dbConfig.Timeout}");

        var connectionOk = await dbConfig.TestConnection();
        Console.WriteLine($"Teste de conexão: {(connectionOk ? "OK" : "Falhou")}");

        // Configuração de email
        dynamic emailConfig = _configService.CreateEmailConfiguration();
        Console.WriteLine($"SMTP Server: {emailConfig.SmtpServer}");
        Console.WriteLine($"Port: {emailConfig.Port}");

        var emailSent = await emailConfig.SendTestEmail("test@example.com");
        Console.WriteLine($"Email de teste: {(emailSent ? "Enviado" : "Falhou")}");

        // Configuração de API
        dynamic apiConfig = _configService.CreateApiConfiguration();
        Console.WriteLine($"API Base URL: {apiConfig.BaseUrl}");
        Console.WriteLine($"API Timeout: {apiConfig.Timeout}");

        var healthOk = await apiConfig.HealthCheck();
        Console.WriteLine($"Health Check: {(healthOk ? "OK" : "Falhou")}");
    }
}

// Exemplo de uso avançado com configuração fluente
public static class DynamicConfigurationExtensions
{
    public static IDynamicConfigurationBuilder WithDatabase(
        this IDynamicConfigurationBuilder builder,
        string connectionString,
        int timeout = 30)
    {
        return builder
            .AddProperty("ConnectionString", connectionString)
            .AddProperty("Timeout", timeout)
            .AddValidator("Timeout", value => (int)value > 0);
    }

    public static IDynamicConfigurationBuilder WithEmail(
        this IDynamicConfigurationBuilder builder,
        string smtpServer,
        int port = 587,
        bool useSsl = true)
    {
        return builder
            .AddProperty("SmtpServer", smtpServer)
            .AddProperty("Port", port)
            .AddProperty("UseSsl", useSsl)
            .AddValidator("Port", value => (int)value > 0 && (int)value <= 65535);
    }

    public static IDynamicConfigurationBuilder WithValidation(
        this IDynamicConfigurationBuilder builder,
        string propertyName,
        Func<object, bool> validator)
    {
        return builder.AddValidator(propertyName, validator);
    }
}