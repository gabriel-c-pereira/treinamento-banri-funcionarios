using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class ExceptionPerformanceBenchmark
{
    [Benchmark(Baseline = true)]
    public int LancarExcecaoSimples()
    {
        var count = 0;
        for (int i = 0; i < 1000; i++)
        {
            try
            {
                throw new Exception("Erro simples");
            }
            catch
            {
                count++;
            }
        }
        return count;
    }

    [Benchmark]
    public int LancarExcecaoComplexa()
    {
        var count = 0;
        for (int i = 0; i < 1000; i++)
        {
            try
            {
                throw new PagamentoException.Builder()
                    .ComCartao("1234")
                    .ComValor(100)
                    .ComMetadata("Id", i)
                    .Build("Erro complexo");
            }
            catch
            {
                count++;
            }
        }
        return count;
    }

    [Benchmark]
    public int UsarReturnCode()
    {
        var count = 0;
        for (int i = 0; i < 1000; i++)
        {
            var resultado = ProcessarComReturnCode();
            if (!resultado.Success)
                count++;
        }
        return count;
    }

    private (bool Success, string Error) ProcessarComReturnCode()
    {
        return (false, "Erro");
    }
}

// Executar: BenchmarkRunner.Run<ExceptionPerformanceBenchmark>();