# Controllers e MVC

## Estrutura MVC

O padrão MVC (Model-View-Controller) separa a aplicação em três componentes:

- **Model**: Representa os dados e regras de negócio
- **View**: Interface do usuário (HTML, Razor)
- **Controller**: Controla o fluxo da aplicação

## Controllers

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
    public async Task<IActionResult> CreateProduto([FromBody] CreateProdutoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var produto = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }
}
```

## Model Binding

O ASP.NET Core automaticamente mapeia dados da requisição para parâmetros:

```csharp
// From query string
[HttpGet("search")]
public IActionResult Search([FromQuery] string termo, [FromQuery] int? categoriaId)

// From route
[HttpGet("{id}")]
public IActionResult Get(int id)

// From body
[HttpPost]
public IActionResult Create([FromBody] ProdutoDto produto)

// From form
[HttpPost("upload")]
public IActionResult Upload([FromForm] IFormFile file)

// From header
[HttpGet("info")]
public IActionResult GetInfo([FromHeader] string authorization)
```

## Action Results

```csharp
// Status codes comuns
return Ok(data);           // 200
return Created(uri, data); // 201
return NoContent();        // 204
return BadRequest();       // 400
return NotFound();         // 404
return Unauthorized();     // 401
return Forbid();           // 403
return StatusCode(500);    // Custom status
```

## Routing

```csharp
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    [HttpGet]                    // GET /api/produtos
    public IActionResult GetAll() { }

    [HttpGet("{id}")]           // GET /api/produtos/1
    public IActionResult Get(int id) { }

    [HttpPost]                  // POST /api/produtos
    public IActionResult Create() { }

    [HttpPut("{id}")]           // PUT /api/produtos/1
    public IActionResult Update(int id) { }

    [HttpDelete("{id}")]        // DELETE /api/produtos/1
    public IActionResult Delete(int id) { }
}
```

## Filtros

```csharp
// Authorization filter
[Authorize]
public class ProdutosController : ControllerBase { }

// Custom filter
public class LogActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Antes da action executar
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Depois da action executar
    }
}
```
