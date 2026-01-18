static async Task Main()
{
    var processor = new DataProcessor();
    var resultado = await processor.ProcessarDadosAsync(...);  // ⬤ Breakpoint 1
}

public async Task<int> ProcessarDadosAsync(string url)
{
    var dados = await _client.BuscarDadosAsync(url);  // ⬤ Breakpoint 2
    var linhas = dados.Split('\n');
    return linhas.Length;
}

public async Task<string> BuscarDadosAsync(string url)
{
    await Task.Delay(1000);  // ⬤ Breakpoint 3
    
    var response = await _http.GetStringAsync(url);  // ⬤ Breakpoint 4
    return response;
}