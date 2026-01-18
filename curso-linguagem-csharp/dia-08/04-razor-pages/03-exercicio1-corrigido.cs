// Pages/Produtos/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly IProdutoService _service;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IProdutoService service, ILogger<IndexModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    public IList<Produto> Produtos { get; set; }
    public string Mensagem { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            Produtos = await _service.GetAllAsync();
            _logger.LogInformation("Carregados {Count} produtos", Produtos.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produtos");
            Mensagem = "Erro ao carregar produtos";
            Produtos = new List<Produto>();
        }
    }
}