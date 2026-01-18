// Models
public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataModificacao { get; set; }
}

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}

// Repository Otimizado
public interface IProdutoRepository
{
    Task<List<Produto>> GetAllAsync();
    Task<List<Produto>> GetProdutosAtivosAsync();
    Task<List<Produto>> GetProdutosComCategoriaAsync();
    Task<List<Produto>> GetProdutosPaginadosAsync(int pagina, int tamanhoPagina);
    Task<List<Produto>> GetProdutosPorCategoriaAsync(int categoriaId);
    Task<Produto> GetByIdAsync(int id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
}

public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _context;

    // Query compilada para operações frequentes
    private static readonly Func<ApplicationDbContext, IAsyncEnumerable<Produto>>
        GetProdutosAtivosCompiled = EF.CompileAsyncQuery(
            (ApplicationDbContext context) =>
                context.Produtos
                    .Where(p => p.Ativo)
                    .AsNoTracking());

    public ProdutoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetAllAsync()
    {
        return await _context.Produtos
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosAtivosAsync()
    {
        // Usando query compilada para melhor performance
        var produtos = new List<Produto>();
        await foreach (var produto in GetProdutosAtivosCompiled(_context))
        {
            produtos.Add(produto);
        }
        return produtos;
    }

    public async Task<List<Produto>> GetProdutosComCategoriaAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria) // Eager loading para evitar N+1
            .Where(p => p.Ativo)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosPaginadosAsync(int pagina, int tamanhoPagina)
    {
        return await _context.Produtos
            .OrderBy(p => p.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Produto>> GetProdutosPorCategoriaAsync(int categoriaId)
    {
        return await _context.Produtos
            .Where(p => p.CategoriaId == categoriaId && p.Ativo)
            .Include(p => p.Categoria)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Produto> GetByIdAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Produto produto)
    {
        produto.DataCriacao = DateTime.UtcNow;
        produto.DataModificacao = DateTime.UtcNow;

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Produto produto)
    {
        produto.DataModificacao = DateTime.UtcNow;

        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}

// Service Layer
public interface IProdutoService
{
    Task<List<Produto>> GetProdutosFiltradosAsync(string categoria, int pagina, int tamanhoPagina);
    Task<object> GetEstatisticasAsync();
    Task<List<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
}

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Produto>> GetProdutosFiltradosAsync(
        string categoria, int pagina, int tamanhoPagina)
    {
        if (!string.IsNullOrEmpty(categoria))
        {
            // Buscar por categoria (assumindo que categoria é o nome)
            var categoriaEntity = await GetCategoriaByNomeAsync(categoria);
            if (categoriaEntity != null)
            {
                return await _repository.GetProdutosPorCategoriaAsync(categoriaEntity.Id);
            }
        }

        return await _repository.GetProdutosPaginadosAsync(pagina, tamanhoPagina);
    }

    public async Task<object> GetEstatisticasAsync()
    {
        var produtos = await _repository.GetAllAsync();

        return new
        {
            TotalProdutos = produtos.Count,
            ProdutosAtivos = produtos.Count(p => p.Ativo),
            TotalCategorias = produtos.Select(p => p.CategoriaId).Distinct().Count(),
            PrecoMedio = produtos.Where(p => p.Ativo).Average(p => p.Preco),
            ProdutoMaisCaro = produtos.Where(p => p.Ativo).Max(p => p.Preco),
            ProdutoMaisBarato = produtos.Where(p => p.Ativo).Min(p => p.Preco)
        };
    }

    public async Task<List<Produto>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Produto> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);

    public async Task AddAsync(Produto produto) =>
        await _repository.AddAsync(produto);

    public async Task UpdateAsync(Produto produto) =>
        await _repository.UpdateAsync(produto);

    public async Task DeleteAsync(int id) =>
        await _repository.DeleteAsync(id);

    private async Task<Categoria> GetCategoriaByNomeAsync(string nome)
    {
        // Implementação simplificada - em produção, teria um repositório de categorias
        return new Categoria { Id = 1, Nome = nome };
    }
}