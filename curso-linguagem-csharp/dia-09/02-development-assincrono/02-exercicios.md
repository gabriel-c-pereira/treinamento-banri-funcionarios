# 📝 Exercícios - Desenvolvimento Assíncrono

## 🎯 Objetivo

Praticar programação assíncrona no ASP.NET Core, incluindo controllers assíncronos, cancellation tokens, paralelismo controlado e tratamento adequado de operações I/O-bound e CPU-bound.

---

## ✏️ Exercício 1: Controller Assíncrono Básico

**Dificuldade**: ⭐ Iniciante

Crie um controller `ProdutosController` com operações assíncronas usando injeção de dependência:

### Requisitos:
1. **Interface do serviço**:
```csharp
public interface IProdutoService
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task<Produto> CriarAsync(Produto produto);
    Task<bool> AtualizarAsync(Produto produto);
    Task<bool> RemoverAsync(int id);
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

2. **Implementação do serviço**:
```csharp
public class ProdutoService : IProdutoService
{
    private static readonly List<Produto> _produtos = new();
    private static int _proximoId = 1;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        await Task.Delay(100); // Simula acesso a banco
        return _produtos.ToList();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        await Task.Delay(50); // Simula consulta
        return _produtos.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Produto> CriarAsync(Produto produto)
    {
        await _semaphore.WaitAsync();
        try
        {
            await Task.Delay(200); // Simula inserção

            produto.Id = _proximoId++;
            produto.DataCadastro = DateTime.Now;
            _produtos.Add(produto);

            return produto;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> AtualizarAsync(Produto produto)
    {
        await _semaphore.WaitAsync();
        try
        {
            await Task.Delay(150); // Simula atualização

            var existente = _produtos.FirstOrDefault(p => p.Id == produto.Id);
            if (existente == null)
                return false;

            existente.Nome = produto.Nome;
            existente.Descricao = produto.Descricao;
            existente.Preco = produto.Preco;
            existente.Estoque = produto.Estoque;

            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> RemoverAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            await Task.Delay(100); // Simula exclusão

            var produto = _produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return false;

            _produtos.Remove(produto);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```

3. **Controller assíncrono**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IProdutoService produtoService, ILogger<ProdutosController> logger)
    {
        _produtoService = produtoService;
        _logger = logger;
    }

    // GET api/produtos
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
    {
        _logger.LogInformation("Obtendo todos os produtos");
        var produtos = await _produtoService.ObterTodosAsync();
        return Ok(produtos);
    }

    // GET api/produtos/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        _logger.LogInformation("Obtendo produto {Id}", id);
        var produto = await _produtoService.ObterPorIdAsync(id);

        if (produto == null)
        {
            _logger.LogWarning("Produto {Id} não encontrado", id);
            return NotFound();
        }

        return Ok(produto);
    }

    // POST api/produtos
    [HttpPost]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Criando novo produto: {Nome}", produto.Nome);
        var novoProduto = await _produtoService.CriarAsync(produto);

        return CreatedAtAction(nameof(GetProduto), new { id = novoProduto.Id }, novoProduto);
    }

    // PUT api/produtos/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produto)
    {
        if (id != produto.Id)
            return BadRequest("ID da URL não corresponde ao ID do produto");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Atualizando produto {Id}", id);
        var sucesso = await _produtoService.AtualizarAsync(produto);

        if (!sucesso)
        {
            _logger.LogWarning("Produto {Id} não encontrado para atualização", id);
            return NotFound();
        }

        return NoContent();
    }

    // DELETE api/produtos/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        _logger.LogInformation("Removendo produto {Id}", id);
        var sucesso = await _produtoService.RemoverAsync(id);

        if (!sucesso)
        {
            _logger.LogWarning("Produto {Id} não encontrado para remoção", id);
            return NotFound();
        }

        return NoContent();
    }
}
```

4. **Registro no DI**:
```csharp
builder.Services.AddScoped<IProdutoService, ProdutoService>();
```

---

## ✏️ Exercício 2: Cancellation Token

**Dificuldade**: ⭐⭐ Intermediário

Implemente um endpoint que execute operação demorada com suporte a cancelamento:

### Requisitos:
1. **Serviço de processamento**:
```csharp
public interface IProcessamentoService
{
    Task<StatusProcessamento> IniciarProcessamentoAsync(int id, CancellationToken cancellationToken = default);
    Task<StatusProcessamento?> ObterStatusAsync(int id);
    Task CancelarProcessamentoAsync(int id);
}

public class StatusProcessamento
{
    public int Id { get; set; }
    public StatusProcessamentoEnum Status { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int Progresso { get; set; } // 0-100
    public string? Mensagem { get; set; }
    public string? Erro { get; set; }
}

public enum StatusProcessamentoEnum
{
    Pendente,
    Executando,
    Concluido,
    Cancelado,
    Erro
}
```

2. **Implementação do serviço**:
```csharp
public class ProcessamentoService : IProcessamentoService
{
    private readonly ConcurrentDictionary<int, StatusProcessamento> _processamentos = new();
    private readonly ConcurrentDictionary<int, CancellationTokenSource> _cancellationTokens = new();

    public async Task<StatusProcessamento> IniciarProcessamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        var status = new StatusProcessamento
        {
            Id = id,
            Status = StatusProcessamentoEnum.Executando,
            DataInicio = DateTime.Now,
            Progresso = 0
        };

        _processamentos[id] = status;

        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _cancellationTokens[id] = cts;

        // Executa processamento em background
        _ = Task.Run(() => ExecutarProcessamentoAsync(id, cts.Token));

        return status;
    }

    private async Task ExecutarProcessamentoAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var status = _processamentos[id];

            // Simula processamento em etapas
            for (int i = 0; i <= 100; i += 10)
            {
                cancellationToken.ThrowIfCancellationRequested();

                status.Progresso = i;
                status.Mensagem = $"Processando... {i}%";

                await Task.Delay(500, cancellationToken); // Simula trabalho
            }

            status.Status = StatusProcessamentoEnum.Concluido;
            status.DataFim = DateTime.Now;
            status.Mensagem = "Processamento concluído com sucesso";
        }
        catch (OperationCanceledException)
        {
            var status = _processamentos[id];
            status.Status = StatusProcessamentoEnum.Cancelado;
            status.DataFim = DateTime.Now;
            status.Mensagem = "Processamento cancelado pelo usuário";
        }
        catch (Exception ex)
        {
            var status = _processamentos[id];
            status.Status = StatusProcessamentoEnum.Erro;
            status.DataFim = DateTime.Now;
            status.Erro = ex.Message;
        }
        finally
        {
            _cancellationTokens.TryRemove(id, out _);
        }
    }

    public Task<StatusProcessamento?> ObterStatusAsync(int id)
    {
        _processamentos.TryGetValue(id, out var status);
        return Task.FromResult(status);
    }

    public Task CancelarProcessamentoAsync(int id)
    {
        if (_cancellationTokens.TryGetValue(id, out var cts))
        {
            cts.Cancel();
        }
        return Task.CompletedTask;
    }
}
```

3. **Controller com cancellation token**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProcessamentoController : ControllerBase
{
    private readonly IProcessamentoService _processamentoService;

    public ProcessamentoController(IProcessamentoService processamentoService)
    {
        _processamentoService = processamentoService;
    }

    // POST api/processamento/5/iniciar
    [HttpPost("{id}/iniciar")]
    [ProducesResponseType(typeof(StatusProcessamento), StatusCodes.Status200OK)]
    public async Task<ActionResult<StatusProcessamento>> IniciarProcessamento(int id)
    {
        var status = await _processamentoService.IniciarProcessamentoAsync(id, HttpContext.RequestAborted);
        return Ok(status);
    }

    // GET api/processamento/5/status
    [HttpGet("{id}/status")]
    [ProducesResponseType(typeof(StatusProcessamento), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatusProcessamento>> ObterStatus(int id)
    {
        var status = await _processamentoService.ObterStatusAsync(id);
        if (status == null)
            return NotFound();

        return Ok(status);
    }

    // POST api/processamento/5/cancelar
    [HttpPost("{id}/cancelar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelarProcessamento(int id)
    {
        var status = await _processamentoService.ObterStatusAsync(id);
        if (status == null)
            return NotFound();

        if (status.Status != StatusProcessamentoEnum.Executando)
            return BadRequest("Processamento não está em execução");

        await _processamentoService.CancelarProcessamentoAsync(id);
        return Ok();
    }
}
```

---

## ✏️ Exercício 3: Paralelismo Controlado

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um serviço que processe múltiplas operações em paralelo com controle de concorrência:

### Requisitos:
1. **Serviço de processamento paralelo**:
```csharp
public interface IProcessamentoParaleloService
{
    Task<ResultadoProcessamento> ProcessarVariosAsync(IEnumerable<DadosProcessamento> dados);
}

public class DadosProcessamento
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public int DuracaoSimuladaMs { get; set; }
}

public class ResultadoItem
{
    public int Id { get; set; }
    public bool Sucesso { get; set; }
    public string? Resultado { get; set; }
    public string? Erro { get; set; }
    public TimeSpan Duracao { get; set; }
}

public class ResultadoProcessamento
{
    public List<ResultadoItem> Resultados { get; set; } = new();
    public int TotalProcessado => Resultados.Count;
    public int Sucessos => Resultados.Count(r => r.Sucesso);
    public int Erros => Resultados.Count(r => !r.Sucesso);
    public TimeSpan DuracaoTotal { get; set; }
}
```

2. **Implementação com controle de concorrência**:
```csharp
public class ProcessamentoParaleloService : IProcessamentoParaleloService
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<ProcessamentoParaleloService> _logger;

    public ProcessamentoParaleloService(ILogger<ProcessamentoParaleloService> logger)
    {
        _semaphore = new SemaphoreSlim(3); // Máximo 3 operações simultâneas
        _logger = logger;
    }

    public async Task<ResultadoProcessamento> ProcessarVariosAsync(IEnumerable<DadosProcessamento> dados)
    {
        var inicio = DateTime.Now;
        var resultado = new ResultadoProcessamento();

        _logger.LogInformation("Iniciando processamento paralelo de {Count} itens", dados.Count());

        // Processa em paralelo com controle de concorrência
        var tarefas = dados.Select(dado => ProcessarItemAsync(dado, resultado));
        await Task.WhenAll(tarefas);

        resultado.DuracaoTotal = DateTime.Now - inicio;
        _logger.LogInformation("Processamento concluído. Sucessos: {Sucessos}, Erros: {Erros}, Tempo: {Duracao}",
            resultado.Sucessos, resultado.Erros, resultado.DuracaoTotal);

        return resultado;
    }

    private async Task ProcessarItemAsync(DadosProcessamento dado, ResultadoProcessamento resultado)
    {
        await _semaphore.WaitAsync();

        var inicioItem = DateTime.Now;
        var resultadoItem = new ResultadoItem { Id = dado.Id };

        try
        {
            _logger.LogDebug("Iniciando processamento do item {Id}: {Descricao}", dado.Id, dado.Descricao);

            // Simula processamento com timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await Task.Delay(dado.DuracaoSimuladaMs, cts.Token);

            // Simula chance de erro (10%)
            if (Random.Shared.Next(100) < 10)
                throw new Exception("Erro simulado no processamento");

            resultadoItem.Sucesso = true;
            resultadoItem.Resultado = $"Processado: {dado.Descricao}";
            _logger.LogDebug("Item {Id} processado com sucesso", dado.Id);
        }
        catch (Exception ex)
        {
            resultadoItem.Sucesso = false;
            resultadoItem.Erro = ex.Message;
            _logger.LogWarning("Erro no processamento do item {Id}: {Error}", dado.Id, ex.Message);
        }
        finally
        {
            resultadoItem.Duracao = DateTime.Now - inicioItem;
            resultado.Resultados.Add(resultadoItem);
            _semaphore.Release();
        }
    }
}
```

3. **Controller para testar**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ParaleloController : ControllerBase
{
    private readonly IProcessamentoParaleloService _processamentoService;

    public ParaleloController(IProcessamentoParaleloService processamentoService)
    {
        _processamentoService = processamentoService;
    }

    // POST api/paralelo/processar
    [HttpPost("processar")]
    [ProducesResponseType(typeof(ResultadoProcessamento), StatusCodes.Status200OK)]
    public async Task<ActionResult<ResultadoProcessamento>> ProcessarVarios([FromBody] List<DadosProcessamento> dados)
    {
        if (dados == null || !dados.Any())
            return BadRequest("Dados para processamento são obrigatórios");

        var resultado = await _processamentoService.ProcessarVariosAsync(dados);
        return Ok(resultado);
    }

    // GET api/paralelo/teste
    [HttpGet("teste")]
    public ActionResult<List<DadosProcessamento>> ObterDadosTeste()
    {
        var dados = Enumerable.Range(1, 10).Select(i => new DadosProcessamento
        {
            Id = i,
            Descricao = $"Tarefa {i}",
            DuracaoSimuladaMs = Random.Shared.Next(1000, 5000) // 1-5 segundos
        }).ToList();

        return Ok(dados);
    }
}
```

4. **Configuração no Program.cs**:
```csharp
builder.Services.AddScoped<IProcessamentoParaleloService, ProcessamentoParaleloService>();
```

5. **Teste do paralelismo**:
```bash
# Obter dados de teste
curl -X GET "https://localhost:5001/api/paralelo/teste"

# Processar em paralelo
curl -X POST "https://localhost:5001/api/paralelo/processar" \
  -H "Content-Type: application/json" \
  -d '[
    {"id": 1, "descricao": "Tarefa 1", "duracaoSimuladaMs": 2000},
    {"id": 2, "descricao": "Tarefa 2", "duracaoSimuladaMs": 1500},
    {"id": 3, "descricao": "Tarefa 3", "duracaoSimuladaMs": 3000},
    {"id": 4, "descricao": "Tarefa 4", "duracaoSimuladaMs": 1000},
    {"id": 5, "descricao": "Tarefa 5", "duracaoSimuladaMs": 2500}
  ]'
```
