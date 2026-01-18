# Controllers API

## Web API Controllers

```csharp
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
    public async Task<IActionResult> GetProdutos([FromQuery] ProdutoFilter filter)
    {
        var produtos = await _service.GetFilteredAsync(filter);
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
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoRequest request)
    {
        var produto = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
```

## Atributos de Rota

```csharp
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    // GET /api/v1/produtos
    [HttpGet]
    public IActionResult GetV1() { }

    // GET /api/v2/produtos
    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2() { }

    // POST /api/produtos
    [HttpPost]
    public IActionResult Create() { }

    // GET /api/produtos/ativos
    [HttpGet("ativos")]
    public IActionResult GetAtivos() { }

    // GET /api/produtos/categoria/{categoriaId}
    [HttpGet("categoria/{categoriaId}")]
    public IActionResult GetByCategoria(int categoriaId) { }
}
```

## Model Binding

```csharp
[ApiController]
public class PedidosController : ControllerBase
{
    // FromQuery - parâmetros de query string
    [HttpGet("search")]
    public IActionResult Search(
        [FromQuery] string status,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] int page = 1) { }

    // FromRoute - parâmetros da rota
    [HttpGet("{id}")]
    public IActionResult Get(int id) { }

    // FromBody - corpo da requisição
    [HttpPost]
    public IActionResult Create([FromBody] CreatePedidoRequest request) { }

    // FromForm - dados de formulário
    [HttpPost("upload")]
    public IActionResult Upload([FromForm] IFormFile file) { }

    // FromHeader - headers HTTP
    [HttpGet("info")]
    public IActionResult GetInfo([FromHeader] string authorization) { }
}
```

## Validação de Modelos

```csharp
public class CreateProdutoRequest
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Descricao { get; set; }

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Preço deve estar entre 0.01 e 999999.99")]
    public decimal Preco { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Categoria deve ser maior que zero")]
    public int CategoriaId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Estoque não pode ser negativo")]
    public int Estoque { get; set; } = 0;
}

[ApiController]
public class ProdutosController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateProdutoRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        // Criar produto...
        return Ok();
    }
}
```

## Response Types

```csharp
[ApiController]
public class ProdutosController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProdutos() { }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProduto(int id) { }

    [HttpPost]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoRequest request) { }
}
```

## Content Negotiation

```csharp
[ApiController]
public class ProdutosController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProdutos()
    {
        var produtos = GetProdutosFromDatabase();

        // Retorna JSON por padrão
        return Ok(produtos);
    }

    [HttpGet("export")]
    public IActionResult ExportProdutos([FromQuery] string format = "json")
    {
        var produtos = GetProdutosFromDatabase();

        switch (format.ToLower())
        {
            case "xml":
                return new ContentResult
                {
                    Content = SerializeToXml(produtos),
                    ContentType = "application/xml",
                    StatusCode = 200
                };

            case "csv":
                return new ContentResult
                {
                    Content = SerializeToCsv(produtos),
                    ContentType = "text/csv",
                    StatusCode = 200
                };

            default:
                return Ok(produtos);
        }
    }
}
```
