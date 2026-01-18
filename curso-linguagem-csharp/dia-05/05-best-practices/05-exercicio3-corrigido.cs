// Configuração Circuit Breaker
var circuitBreakerPolicy = Policy
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30),
        onBreak: (exception, duration) =>
        {
            Log.Error("Circuit ABERTO por {Duration}s: {Exception}", 
                duration.TotalSeconds, exception.Message);
        },
        onReset: () =>
        {
            Log.Information("Circuit FECHADO - serviço recuperado");
        },
        onHalfOpen: () =>
        {
            Log.Information("Circuit HALF-OPEN - testando serviço");
        });

// Combinar Retry + Circuit Breaker
var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

try
{
    await policyWrap.ExecuteAsync(async () =>
    {
        return await ChamarServicoExterno();
    });
}
catch (BrokenCircuitException)
{
    Log.Warning("Circuit breaker aberto - usando cache");
    return ObterDadosCache();
}