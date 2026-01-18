// Pages/Produtos/Create.cshtml.cs
public class CreateModel : PageModel
{
    private readonly IProdutoService _service;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IProdutoService service, ILogger<CreateModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    [BindProperty]
    public CreateProdutoViewModel Produto { get; set; }

    public IActionResult OnGet()
    {
        Produto = new CreateProdutoViewModel();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Modelo inválido: {@ModelState}", ModelState);
            return Page();
        }

        try
        {
            var produto = new Produto
            {
                Nome = Produto.Nome,
                Descricao = Produto.Descricao,
                Preco = Produto.Preco,
                CategoriaId = Produto.CategoriaId
            };

            await _service.CreateAsync(produto);

            _logger.LogInformation("Produto criado: {Nome}", produto.Nome);

            TempData["SuccessMessage"] = "Produto criado com sucesso!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            ModelState.AddModelError("", "Erro ao criar produto. Tente novamente.");
            return Page();
        }
    }
}

// ViewModel
public class CreateProdutoViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public int CategoriaId { get; set; }

    public IEnumerable<SelectListItem> Categorias { get; set; }
}