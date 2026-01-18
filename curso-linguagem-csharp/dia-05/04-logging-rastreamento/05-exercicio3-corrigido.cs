using Serilog.Context;

public class OperationContext : IDisposable
{
    private readonly IDisposable _correlationScope;
    public string CorrelationId { get; }

    public OperationContext(string operationName)
    {
        CorrelationId = Guid.NewGuid().ToString("N")[..8];
        _correlationScope = LogContext.PushProperty("CorrelationId", CorrelationId);
        Log.Information("Operação iniciada: {OperationName}", operationName);
    }

    public void Dispose()
    {
        Log.Information("Operação finalizada");
        _correlationScope?.Dispose();
    }
}

// Uso
using var context = new OperationContext("ProcessarPedido");
// Todos os logs terão CorrelationId