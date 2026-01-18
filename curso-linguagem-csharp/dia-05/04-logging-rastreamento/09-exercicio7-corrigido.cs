Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Destructure.With(new SensitiveDataDestructuringPolicy())
    
    // Console: Info+
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
    
    // Debug com sampling 10%
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => 
            evt.Level >= LogEventLevel.Debug && Random.Shared.Next(100) < 10)
        .WriteTo.File("logs/debug-.txt", rollingInterval: RollingInterval.Day))
    
    // Errors: JSON
    .WriteTo.File(new JsonFormatter(), "logs/errors-.json",
        restrictedToMinimumLevel: LogEventLevel.Error)
    
    .CreateLogger();

// Mascaramento
public class SensitiveDataDestructuringPolicy : IDestructuringPolicy
{
    public bool TryDestructure(object value, ILogEventPropertyValueFactory factory, 
        out LogEventPropertyValue? result)
    {
        if (value is string texto && Regex.IsMatch(texto, @"\d{3}\.\d{3}\.\d{3}-\d{2}"))
        {
            result = new ScalarValue("***.***.***.***"); // Mascarar CPF
            return true;
        }
        result = null;
        return false;
    }
}