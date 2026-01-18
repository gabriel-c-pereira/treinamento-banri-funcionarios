public class ImportacaoService
{
    public async Task ImportarEmLote(List<string> arquivos)
    {
        var tasks = arquivos.Select(ImportarArquivo).ToList();
        
        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception)
        {
            var exceptions = tasks
                .Where(t => t.IsFaulted)
                .SelectMany(t => t.Exception!.InnerExceptions)
                .ToList();

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Falha em {exceptions.Count} de {arquivos.Count} importações",
                    exceptions);
            }
        }
    }

    private async Task ImportarArquivo(string arquivo)
    {
        await Task.Delay(100); // Simula processamento

        if (arquivo.Contains("invalido"))
            throw new InvalidDataException($"Arquivo {arquivo} é inválido");
        
        if (arquivo.Contains("corrupto"))
            throw new IOException($"Arquivo {arquivo} está corrupto");
    }
}

// Uso com tratamento
try
{
    var service = new ImportacaoService();
    await service.ImportarEmLote(new List<string>
    {
        "pedidos.csv",
        "clientes_invalido.csv",
        "produtos_corrupto.csv",
        "estoque.csv"
    });
}
catch (AggregateException aggEx)
{
    Console.WriteLine($"❌ {aggEx.InnerExceptions.Count} erros:");
    
    foreach (var ex in aggEx.InnerExceptions)
    {
        Console.WriteLine($"  - {ex.GetType().Name}: {ex.Message}");
    }

    // Tratamento específico por tipo
    aggEx.Handle(ex =>
    {
        if (ex is InvalidDataException)
        {
            Console.WriteLine($"⚠️  Dados inválidos (ignorado): {ex.Message}");
            return true; // Handled
        }
        return false; // Re-throw
    });
}