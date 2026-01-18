public class ProcessamentoParaleloService : IProcessamentoParaleloService
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3);
    private readonly ILogger<ProcessamentoParaleloService> _logger;

    public ProcessamentoParaleloService(ILogger<ProcessamentoParaleloService> logger)
    {
        _logger = logger;
    }

    public async Task<ResultadoProcessamento> ProcessarItensAsync(
        IEnumerable<ItemProcessamento> itens,
        CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task<ResultadoItem>>();
        var resultado = new ResultadoProcessamento();

        foreach (var item in itens)
        {
            tasks.Add(ProcessarItemAsync(item, cancellationToken));
        }

        var resultados = await Task.WhenAll(tasks);

        resultado.Sucessos = resultados.Where(r => r.Sucesso).ToList();
        resultado.Erros = resultados.Where(r => !r.Sucesso).ToList();

        return resultado;
    }

    private async Task<ResultadoItem> ProcessarItemAsync(
        ItemProcessamento item,
        CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(30)); // Timeout de 30 segundos

            var resultado = await ProcessarItemCoreAsync(item, cts.Token);
            return new ResultadoItem { Item = item, Sucesso = true, Dados = resultado };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro processando item {ItemId}", item.Id);
            return new ResultadoItem { Item = item, Sucesso = false, Erro = ex.Message };
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<object> ProcessarItemCoreAsync(
        ItemProcessamento item,
        CancellationToken cancellationToken)
    {
        // Simulação de processamento
        await Task.Delay(1000, cancellationToken);
        return new { item.Id, ProcessadoEm = DateTime.UtcNow };
    }
}

public class ResultadoProcessamento
{
    public List<ResultadoItem> Sucessos { get; set; } = new();
    public List<ResultadoItem> Erros { get; set; } = new();
}

public class ResultadoItem
{
    public ItemProcessamento Item { get; set; }
    public bool Sucesso { get; set; }
    public object Dados { get; set; }
    public string Erro { get; set; }
}

public class ItemProcessamento
{
    public int Id { get; set; }
    public string Dados { get; set; }
}