// Program.cs
builder.Services.AddMemoryCache();

// Models
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
}

// Repository Interface
public interface IProdutoRepository
{
    Task<List<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
}

// Service com Cache
public interface IProdutoService
{
    Task<List<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
}

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "produtos_lista";
    private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public ProdutoService(IProdutoRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out List<Produto> produtos))
        {
            produtos = await _repository.GetAllAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(CacheDuration)
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            _cache.Set(CacheKey, produtos, cacheOptions);
        }

        return produtos;
    }

    public async Task<Produto> GetByIdAsync(int id)
    {
        // Cache individual por produto
        var cacheKey = $"produto_{id}";
        if (!_cache.TryGetValue(cacheKey, out Produto produto))
        {
            produto = await _repository.GetByIdAsync(id);

            if (produto != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(CacheDuration);

                _cache.Set(cacheKey, produto, cacheOptions);
            }
        }

        return produto;
    }

    public async Task AddAsync(Produto produto)
    {
        await _repository.AddAsync(produto);
        InvalidateCache();
    }

    public async Task UpdateAsync(Produto produto)
    {
        await _repository.UpdateAsync(produto);
        InvalidateCache();
        // Invalidate specific product cache
        _cache.Remove($"produto_{produto.Id}");
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        InvalidateCache();
        _cache.Remove($"produto_{id}");
    }

    private void InvalidateCache()
    {
        _cache.Remove(CacheKey);
    }
}

// Controller
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _service.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _service.GetByIdAsync(id);
        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] Produto produto)
    {
        await _service.AddAsync(produto);
        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, [FromBody] Produto produto)
    {
        produto.Id = id;
        await _service.UpdateAsync(produto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}