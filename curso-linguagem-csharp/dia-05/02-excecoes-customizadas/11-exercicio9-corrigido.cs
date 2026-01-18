public class LoggableException : Exception
{
    public Dictionary<string, object> LogContext { get; }

    public LoggableException(string message, Dictionary<string, object>? context = null)
        : base(message)
    {
        LogContext = context ?? new();
    }

    public string ToStructuredLog()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {GetType().Name}");
        sb.AppendLine($"Message: {Message}");
        
        foreach (var kvp in LogContext)
        {
            sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
        }

        if (InnerException != null)
        {
            sb.AppendLine($"InnerException: {InnerException.GetType().Name}");
            sb.AppendLine($"  {InnerException.Message}");
        }

        return sb.ToString();
    }
}