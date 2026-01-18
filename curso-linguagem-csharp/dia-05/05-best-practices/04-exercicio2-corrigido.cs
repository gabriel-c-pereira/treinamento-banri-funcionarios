using Polly;
using Polly.Retry;

// Configuração Retry
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .Or<TimeoutException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (exception, timeSpan, retryCount, context) =>
        {
            Log.Warning("Tentativa {RetryCount} após {Delay}s: {Exception}", 
                retryCount, timeSpan.TotalSeconds, exception.Message);
        });

// Uso
var resultado = await retryPolicy.ExecuteAsync(async () =>
{
    Log.Information("Chamando API externa...");
    return await httpClient.GetStringAsync("https://api.exemplo.com/data");
});