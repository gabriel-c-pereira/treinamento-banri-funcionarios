// Interfaces
public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
}

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public interface ICacheService
{
    Task SetAsync(string key, object value);
    Task<T> GetAsync<T>(string key);
}

// Serviço com múltiplas dependências
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ICacheService _cache;
    private readonly ILogger<ProdutoService> _logger;

    public ProdutoService(
        IProdutoRepository repository,
        IEmailService emailService,
        ICacheService cache,
        ILogger<ProdutoService> logger)
    {
        _repository = repository;
        _emailService = emailService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Produto> GetProdutoAsync(int id)
    {
        var cacheKey = $"produto_{id}";

        // Tentar cache primeiro
        var produto = await _cache.GetAsync<Produto>(cacheKey);
        if (produto != null)
        {
            _logger.LogInformation("Produto {Id} encontrado no cache", id);
            return produto;
        }

        // Buscar no repositório
        produto = await _repository.GetByIdAsync(id);
        if (produto != null)
        {
            await _cache.SetAsync(cacheKey, produto);
            _logger.LogInformation("Produto {Id} encontrado no repositório", id);
        }

        return produto;
    }

    public async Task NotifyProdutoCriadoAsync(Produto produto)
    {
        var subject = $"Novo produto criado: {produto.Nome}";
        var body = $"O produto {produto.Nome} foi criado com sucesso.";

        await _emailService.SendEmailAsync("admin@empresa.com", subject, body);
        _logger.LogInformation("Notificação enviada para produto {Id}", produto.Id);
    }
}