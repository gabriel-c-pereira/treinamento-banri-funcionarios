using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class ExceptionBenchmarks
{
    private readonly List<int> _numeros = Enumerable.Range(1, 1000).ToList();

    [Benchmark(Baseline = true)]
    public int ComExcecoes()
    {
        var soma = 0;
        foreach (var num in _numeros)
        {
            try
            {
                if (num % 2 != 0)
                    throw new InvalidOperationException();
                soma += num;
            }
            catch
            {
                // Ignora ímpares
            }
        }
        return soma;
    }

    [Benchmark]
    public int SemExcecoes()
    {
        var soma = 0;
        foreach (var num in _numeros)
        {
            if (num % 2 == 0)
                soma += num;
        }
        return soma;
    }

    [Benchmark]
    public int ComTryParse()
    {
        var textos = _numeros.Select(n => n.ToString()).ToList();
        var soma = 0;
        foreach (var texto in textos)
        {
            if (int.TryParse(texto, out var num))
                soma += num;
        }
        return soma;
    }
}

// Executar
BenchmarkRunner.Run<ExceptionBenchmarks>();

// ═══════════════════════════════════════════════════

var policy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
    .WrapAsync(Policy.TimeoutAsync(10));

// ═══════════════════════════════════════════════════

public record Result<T>(bool IsSuccess, T? Value, string? Error)
{
    public static Result<T> Ok(T value) => new(true, value, null);
    public static Result<T> Fail(string error) => new(false, default, error);
}

// ═══════════════════════════════════════════════════

ArgumentNullException.ThrowIfNull(pedido);
ArgumentOutOfRangeException.ThrowIfNegativeOrZero(valor);