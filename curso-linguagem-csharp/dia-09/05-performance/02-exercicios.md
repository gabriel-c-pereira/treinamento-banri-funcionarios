# 📝 Exercícios - Performance

## 🎯 Objetivo

Praticar técnicas de otimização de performance no ASP.NET Core, incluindo caching, response caching, otimização de queries e compressão de resposta.

---

## ✏️ Exercício 1: Caching Básico

**Dificuldade**: ⭐ Iniciante

Implemente caching in-memory para otimizar acesso a dados:

### Requisitos:
1. **Configuração de cache no Program.cs**:
```csharp
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024; // Limite de 1024 entradas
    options.CompactionPercentage = 0.25; // Remove 25% quando atingir limite
});
```

2. **Serviço com cache**:
```csharp
public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObterTodasAsync();
    Task<Categoria?> ObterPorIdAsync(int id);
    Task<Categoria> CriarAsync(Categoria categoria);
    Task<bool> AtualizarAsync(Categoria categoria);
    Task<bool> RemoverAsync(int id);
}

public class CategoriaService : ICategoriaService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CategoriaService> _logger;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    private const string CACHE_KEY_TODAS = "categorias_todas";
    private const string CACHE_KEY_PREFIX = "categoria_";

    public CategoriaService(IMemoryCache cache, ILogger<CategoriaService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<IEnumerable<Categoria>> ObterTodasAsync()
    {
        if (!_cache.TryGetValue(CACHE_KEY_TODAS, out IEnumerable<Categoria> categorias))
        {
            await _semaphore.WaitAsync();
            try
            {
                // Double-check após adquirir lock
                if (!_cache.TryGetValue(CACHE_KEY_TODAS, out categorias))
                {
                    _logger.LogInformation("Carregando categorias do banco de dados");
                    categorias = await CarregarDoBancoAsync();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetSize(1)
                        .RegisterPostEvictionCallback((key, value, reason, state) =>
                        {
                            _logger.LogInformation($"Cache {key} foi removido. Motivo: {reason}");
                        });

                    _cache.Set(CACHE_KEY_TODAS, categorias, cacheOptions);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        else
        {
            _logger.LogInformation("Categorias retornadas do cache");
        }

        return categorias;
    }

    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        var cacheKey = $"{CACHE_KEY_PREFIX}{id}";

        if (!_cache.TryGetValue(cacheKey, out Categoria categoria))
        {
            _logger.LogInformation("Carregando categoria {Id} do banco", id);
            categoria = await CarregarPorIdDoBancoAsync(id);

            if (categoria != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetSize(1);

                _cache.Set(cacheKey, categoria, cacheOptions);
            }
        }
        else
        {
            _logger.LogInformation("Categoria {Id} retornada do cache", id);
        }

        return categoria;
    }

    public async Task<Categoria> CriarAsync(Categoria categoria)
    {
        await _semaphore.WaitAsync();
        try
        {
            categoria = await SalvarNoBancoAsync(categoria);

            // Invalidar cache
            _cache.Remove(CACHE_KEY_TODAS);
            _logger.LogInformation("Cache de categorias invalidado após criação");

            return categoria;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> AtualizarAsync(Categoria categoria)
    {
        await _semaphore.WaitAsync();
        try
        {
            var sucesso = await AtualizarNoBancoAsync(categoria);

            if (sucesso)
            {
                // Invalidar caches relacionados
                _cache.Remove(CACHE_KEY_TODAS);
                _cache.Remove($"{CACHE_KEY_PREFIX}{categoria.Id}");
                _logger.LogInformation("Caches invalidados após atualização");
            }

            return sucesso;
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
            var sucesso = await RemoverDoBancoAsync(id);

            if (sucesso)
            {
                // Invalidar caches relacionados
                _cache.Remove(CACHE_KEY_TODAS);
                _cache.Remove($"{CACHE_KEY_PREFIX}{id}");
                _logger.LogInformation("Caches invalidados após remoção");
            }

            return sucesso;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    // Métodos simulados (em produção, usariam EF Core ou ADO.NET)
    private async Task<IEnumerable<Categoria>> CarregarDoBancoAsync()
    {
        await Task.Delay(100); // Simula acesso ao banco
        return new List<Categoria>
        {
            new Categoria { Id = 1, Nome = "Eletrônicos", Descricao = "Produtos eletrônicos" },
            new Categoria { Id = 2, Nome = "Roupas", Descricao = "Vestuário em geral" }
        };
    }

    private async Task<Categoria?> CarregarPorIdDoBancoAsync(int id)
    {
        await Task.Delay(50);
        return id switch
        {
            1 => new Categoria { Id = 1, Nome = "Eletrônicos", Descricao = "Produtos eletrônicos" },
            2 => new Categoria { Id = 2, Nome = "Roupas", Descricao = "Vestuário em geral" },
            _ => null
        };
    }

    private async Task<Categoria> SalvarNoBancoAsync(Categoria categoria)
    {
        await Task.Delay(200);
        categoria.Id = new Random().Next(100, 999);
        return categoria;
    }

    private async Task<bool> AtualizarNoBancoAsync(Categoria categoria) => await Task.FromResult(true);
    private async Task<bool> RemoverDoBancoAsync(int id) => await Task.FromResult(true);
}

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
```

3. **Controller usando o serviço**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _categoriaService.ObterTodasAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoria(int id)
    {
        var categoria = await _categoriaService.ObterPorIdAsync(id);
        return categoria == null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> PostCategoria([FromBody] Categoria categoria)
    {
        var novaCategoria = await _categoriaService.CriarAsync(categoria);
        return CreatedAtAction(nameof(GetCategoria), new { id = novaCategoria.Id }, novaCategoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.Id)
            return BadRequest();

        var sucesso = await _categoriaService.AtualizarAsync(categoria);
        return sucesso ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var sucesso = await _categoriaService.RemoverAsync(id);
        return sucesso ? NoContent() : NotFound();
    }
}
```

---

## ✏️ Exercício 2: Response Caching

**Dificuldade**: ⭐⭐ Intermediário

Configure response caching para melhorar performance de endpoints:

### Requisitos:
1. **Configuração no Program.cs**:
```csharp
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024; // 1MB
    options.UseCaseSensitivePaths = false;
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "application/xml",
        "text/plain"
    });
});
```

2. **Middleware no pipeline**:
```csharp
app.UseResponseCaching();
app.UseResponseCompression();
```

3. **Controller com response caching**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class PublicacoesController : ControllerBase
{
    private readonly IPublicacaoService _publicacaoService;

    public PublicacoesController(IPublicacaoService publicacaoService)
    {
        _publicacaoService = publicacaoService;
    }

    // Cache por 5 minutos, público
    [HttpGet]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "pagina", "tamanho" })]
    public async Task<IActionResult> GetPublicacoes(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanho = 10)
    {
        var publicacoes = await _publicacaoService.ObterPublicacoesAsync(pagina, tamanho);
        return Ok(publicacoes);
    }

    // Cache por 10 minutos, apenas para usuários autenticados
    [HttpGet("recomendadas")]
    [Authorize]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Client, VaryByHeader = "Authorization")]
    public async Task<IActionResult> GetPublicacoesRecomendadas()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var publicacoes = await _publicacaoService.ObterRecomendadasAsync(userId);
        return Ok(publicacoes);
    }

    // Cache condicional baseado em ETag
    [HttpGet("{id}")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetPublicacao(int id)
    {
        var publicacao = await _publicacaoService.ObterPorIdAsync(id);
        if (publicacao == null)
            return NotFound();

        // Implementar ETag para cache condicional
        var etag = $"\"{publicacao.Id}-{publicacao.DataModificacao.Ticks}\"";
        if (HttpContext.Request.Headers.IfNoneMatch.Contains(etag))
        {
            return StatusCode(304); // Not Modified
        }

        HttpContext.Response.Headers.ETag = etag;
        return Ok(publicacao);
    }

    // Sem cache - dados dinâmicos
    [HttpGet("estatisticas")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> GetEstatisticas()
    {
        var estatisticas = await _publicacaoService.ObterEstatisticasAsync();
        return Ok(estatisticas);
    }

    // Cache customizado via código
    [HttpGet("destaques")]
    public async Task<IActionResult> GetDestaques()
    {
        var destaques = await _publicacaoService.ObterDestaquesAsync();

        // Cache por 15 minutos
        HttpContext.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromMinutes(15)
            };

        HttpContext.Response.Headers.LastModified = DateTimeOffset.Now;

        return Ok(destaques);
    }
}
```

4. **Headers de cache customizados**:
```csharp
[ApiController]
[Route("api/cache")]
public class CacheController : ControllerBase
{
    [HttpGet("custom")]
    public IActionResult GetCustomCache()
    {
        // Cache-Control: private, max-age=300
        HttpContext.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Private = true,
                MaxAge = TimeSpan.FromSeconds(300)
            };

        // Vary: Accept-Encoding
        HttpContext.Response.Headers.Vary = "Accept-Encoding";

        return Ok(new { timestamp = DateTime.Now, cached = true });
    }

    [HttpGet("no-cache")]
    public IActionResult GetNoCache()
    {
        // Cache-Control: no-cache, no-store
        HttpContext.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                NoCache = true,
                NoStore = true
            };

        return Ok(new { timestamp = DateTime.Now, cached = false });
    }
}
```

5. **Testes de cache**:
```bash
# Primeira requisição (sem cache)
curl -v -H "Accept: application/json" "https://localhost:5001/api/publicacoes"

# Segunda requisição (deve usar cache)
curl -v -H "Accept: application/json" "https://localhost:5001/api/publicacoes"

# Requisição com ETag
curl -H "If-None-Match: \"123-456\"" "https://localhost:5001/api/publicacoes/1"

# Verificar headers de cache
curl -I "https://localhost:5001/api/cache/custom"
```

---

## ✏️ Exercício 3: Otimização de Queries

**Dificuldade**: ⭐⭐⭐ Avançado

Otimize queries de banco de dados com Entity Framework Core:

### Requisitos:
1. **Modelo de dados**:
```csharp
public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; }

    public Cliente Cliente { get; set; }
    public ICollection<ItemPedido> Itens { get; set; }
}

public class ItemPedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public Pedido Pedido { get; set; }
    public Produto Produto { get; set; }
}

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

    public ICollection<Pedido> Pedidos { get; set; }
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
```

2. **Repositório otimizado**:
```csharp
public interface IPedidoRepository
{
    Task<PagedResult<Pedido>> ObterPedidosAsync(PedidoFilter filter, CancellationToken cancellationToken = default);
    Task<Pedido?> ObterPedidoCompletoAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PedidoResumo>> ObterResumoPedidosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PedidoRepository> _logger;

    // Query compilada para operações frequentes
    private static readonly Func<AppDbContext, int, Task<Pedido?>> _getPedidoByIdCompiled =
        EF.CompileAsyncQuery((AppDbContext context, int id) =>
            context.Pedidos
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id));

    public PedidoRepository(AppDbContext context, ILogger<PedidoRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<Pedido>> ObterPedidosAsync(PedidoFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Pedidos.AsNoTracking();

        // Aplicar filtros
        if (filter.ClienteId.HasValue)
            query = query.Where(p => p.ClienteId == filter.ClienteId);

        if (filter.Status.HasValue)
            query = query.Where(p => p.Status == filter.Status);

        if (filter.DataInicio.HasValue)
            query = query.Where(p => p.DataPedido >= filter.DataInicio);

        if (filter.DataFim.HasValue)
            query = query.Where(p => p.DataPedido <= filter.DataFim);

        // Paginação
        var total = await query.CountAsync(cancellationToken);
        var pedidos = await query
            .OrderByDescending(p => p.DataPedido)
            .Skip((filter.Pagina - 1) * filter.Tamanho)
            .Take(filter.Tamanho)
            .Include(p => p.Cliente) // Evitar N+1
            .ToListAsync(cancellationToken);

        return new PagedResult<Pedido>
        {
            Items = pedidos,
            PaginaAtual = filter.Pagina,
            TamanhoPagina = filter.Tamanho,
            TotalItems = total,
            TotalPaginas = (int)Math.Ceiling(total / (double)filter.Tamanho)
        };
    }

    public async Task<Pedido?> ObterPedidoCompletoAsync(int id, CancellationToken cancellationToken = default)
    {
        // Usar query compilada para melhor performance
        var pedido = await _getPedidoByIdCompiled(_context, id);

        if (pedido == null)
            return null;

        // Carregar relacionamentos necessários
        await _context.Entry(pedido)
            .Reference(p => p.Cliente)
            .LoadAsync(cancellationToken);

        await _context.Entry(pedido)
            .Collection(p => p.Itens)
            .Query()
            .Include(i => i.Produto)
            .LoadAsync(cancellationToken);

        return pedido;
    }

    public async Task<IEnumerable<PedidoResumo>> ObterResumoPedidosAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        // Query otimizada para projeção (select apenas campos necessários)
        var resumos = await _context.Pedidos
            .AsNoTracking()
            .Where(p => p.DataPedido >= inicio && p.DataPedido <= fim)
            .Select(p => new PedidoResumo
            {
                Id = p.Id,
                ClienteNome = p.Cliente.Nome,
                DataPedido = p.DataPedido,
                ValorTotal = p.ValorTotal,
                Status = p.Status,
                QuantidadeItens = p.Itens.Count
            })
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Resumo de {Count} pedidos obtido", resumos.Count);
        return resumos;
    }
}

// Classes auxiliares
public class PedidoFilter
{
    public int Pagina { get; set; } = 1;
    public int Tamanho { get; set; } = 10;
    public int? ClienteId { get; set; }
    public StatusPedido? Status { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int PaginaAtual { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItems { get; set; }
    public int TotalPaginas { get; set; }
}

public class PedidoResumo
{
    public int Id { get; set; }
    public string ClienteNome { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusPedido Status { get; set; }
    public int QuantidadeItens { get; set; }
}
```

3. **Controller usando o repositório otimizado**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidosController(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPedidos([FromQuery] PedidoFilter filter, CancellationToken cancellationToken)
    {
        var result = await _pedidoRepository.ObterPedidosAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPedido(int id, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPedidoCompletoAsync(id, cancellationToken);
        return pedido == null ? NotFound() : Ok(pedido);
    }

    [HttpGet("resumo")]
    public async Task<IActionResult> GetResumoPedidos(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim,
        CancellationToken cancellationToken)
    {
        var resumos = await _pedidoRepository.ObterResumoPedidosAsync(inicio, fim, cancellationToken);
        return Ok(resumos);
    }
}
```

4. **Configuração do DbContext**:
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Índices para otimização
        modelBuilder.Entity<Pedido>()
            .HasIndex(p => p.ClienteId);

        modelBuilder.Entity<Pedido>()
            .HasIndex(p => p.DataPedido);

        modelBuilder.Entity<Pedido>()
            .HasIndex(p => new { p.Status, p.DataPedido });

        modelBuilder.Entity<ItemPedido>()
            .HasIndex(i => i.PedidoId);

        modelBuilder.Entity<ItemPedido>()
            .HasIndex(i => i.ProdutoId);
    }
}
```

5. **Testes de performance**:
```csharp
// Benchmark para comparar queries
[MemoryDiagnoser]
public class QueryBenchmark
{
    private readonly AppDbContext _context;

    [Benchmark]
    public async Task<List<Pedido>> QueryComInclude()
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .ToListAsync();
    }

    [Benchmark]
    public async Task<List<PedidoResumo>> QueryComProjecao()
    {
        return await _context.Pedidos
            .Select(p => new PedidoResumo
            {
                Id = p.Id,
                ClienteNome = p.Cliente.Nome,
                ValorTotal = p.ValorTotal
            })
            .ToListAsync();
    }
}
```
