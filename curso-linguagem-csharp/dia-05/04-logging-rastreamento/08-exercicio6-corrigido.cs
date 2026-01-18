using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;

private static readonly ActivitySource ActivitySource = new("MeuServico");

using var activity = ActivitySource.StartActivity("ProcessarPedido");
activity?.SetTag("pedido.id", pedidoId);
activity?.SetTag("cliente.id", clienteId);

// Configuração
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("MeuServico")
    .AddConsoleExporter()
    .Build();