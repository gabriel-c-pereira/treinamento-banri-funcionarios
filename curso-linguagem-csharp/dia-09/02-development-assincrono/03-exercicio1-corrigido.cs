[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos()
    {
        var produtos = await _produtoService.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _produtoService.GetByIdAsync(id);
        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var produto = await _produtoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }
}

public interface IProdutoService
{
    Task<List<Produto>> GetAllAsync();
    Task<Produto> GetByIdAsync(int id);
    Task<Produto> CreateAsync(CreateProdutoRequest request);
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}

public class CreateProdutoRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }
}