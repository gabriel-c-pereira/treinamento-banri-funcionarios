[ApiController]
[Route("api/produtos")]
[Authorize]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProdutos([FromQuery] ProdutoFilter filter)
    {
        try
        {
            var produtos = await _service.GetFilteredAsync(filter);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProduto(int id)
    {
        try
        {
            var produto = await _service.GetByIdAsync(id);
            if (produto == null)
                return NotFound(new { error = "Produto não encontrado" });

            return Ok(produto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var produto = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, request);
            if (!result)
                return NotFound(new { error = "Produto não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { error = "Produto não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }

    [HttpGet("categoria/{categoriaId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProdutosByCategoria(int categoriaId)
    {
        try
        {
            var produtos = await _service.GetByCategoriaAsync(categoriaId);
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
}

// Filtro customizado
public class LogActionFilter : IActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"Executando {context.ActionDescriptor.DisplayName}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Executado {context.ActionDescriptor.DisplayName} - Status: {context.HttpContext.Response.StatusCode}");
    }
}

// DTOs
public class ProdutoFilter
{
    public string Nome { get; set; }
    public int? CategoriaId { get; set; }
    public decimal? PrecoMin { get; set; }
    public decimal? PrecoMax { get; set; }
}

public class CreateProdutoRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [StringLength(500)]
    public string Descricao { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }

    [Required]
    public int CategoriaId { get; set; }
}

public class UpdateProdutoRequest
{
    [StringLength(100)]
    public string Nome { get; set; }

    [StringLength(500)]
    public string Descricao { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Preco { get; set; }

    public int? CategoriaId { get; set; }
}