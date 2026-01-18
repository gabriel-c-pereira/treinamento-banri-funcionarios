using System.Text.Json;
using System.Text.Json.Serialization;

public class PedidoException : Exception
{
    public int PedidoId { get; }
    public string CodigoErro { get; }
    
    [JsonConstructor]
    public PedidoException(string message, int pedidoId, string codigoErro, Exception? innerException = null)
        : base(message, innerException)
    {
        PedidoId = pedidoId;
        CodigoErro = codigoErro;
    }

    // Para serialização
    public string ToJson()
    {
        return JsonSerializer.Serialize(new
        {
            Type = GetType().Name,
            Message,
            PedidoId,
            CodigoErro,
            StackTrace,
            InnerException = InnerException?.Message
        }, new JsonSerializerOptions { WriteIndented = true });
    }

    // Para desserialização
    public static PedidoException? FromJson(string json)
    {
        var data = JsonSerializer.Deserialize<ExceptionData>(json);
        if (data == null) return null;

        return new PedidoException(data.Message, data.PedidoId, data.CodigoErro);
    }

    private class ExceptionData
    {
        public string Message { get; set; } = string.Empty;
        public int PedidoId { get; set; }
        public string CodigoErro { get; set; } = string.Empty;
    }
}

// Teste
try
{
    throw new PedidoException("Pagamento recusado", 1001, "PAY_001");
}
catch (PedidoException ex)
{
    var json = ex.ToJson();
    Console.WriteLine(json);
    
    var restored = PedidoException.FromJson(json);
    Console.WriteLine($"Restaurado: {restored?.Message}");
}