using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Trace()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Trace)
    .WriteTo.File("logs/debug-.txt", 
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/errors-.txt", 
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Error)
    .CreateLogger();

record Pedido(int Id, string Cliente, decimal Total);

foreach (var pedido in pedidos)
{
    if (pedido.Total < 0)
        Log.Error("Pedido {PedidoId} inválido", pedido.Id);
    else if (pedido.Total < 10)
        Log.Warning("Pedido {PedidoId} baixo", pedido.Id);
    else
        Log.Information("Pedido {PedidoId} OK", pedido.Id);
}