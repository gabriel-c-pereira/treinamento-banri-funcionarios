// Program.cs
builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseResponseCaching();

// Controller com Response Caching
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
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "categoria", "pagina" })]
    public async Task<IActionResult> GetProdutos(
        [FromQuery] string categoria = null,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10)
    {
        var produtos = await _service.GetProdutosFiltradosAsync(categoria, pagina, tamanhoPagina);
        return Ok(new
        {
            produtos,
            pagina,
            tamanhoPagina,
            categoria,
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetProduto(int id)
    {
        var produto = await _service.GetByIdAsync(id);
        if (produto == null)
            return NotFound();

        Response.Headers.Add("Last-Modified", produto.DataModificacao.ToString("R"));
        return Ok(produto);
    }

    [HttpGet("estatisticas")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetEstatisticas()
    {
        var estatisticas = await _service.GetEstatisticasAsync();
        return Ok(estatisticas);
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

// Middleware para Cache Condicional
public class ConditionalCacheMiddleware
{
    private readonly RequestDelegate _next;

    public ConditionalCacheMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Verificar If-Modified-Since header
        if (context.Request.Headers.TryGetValue("If-Modified-Since", out var ifModifiedSince))
        {
            if (DateTime.TryParse(ifModifiedSince, out var modifiedSince))
            {
                // Lógica para verificar se o recurso foi modificado
                // Se não foi modificado, retornar 304 Not Modified
                var lastModified = DateTime.UtcNow.AddMinutes(-30); // Exemplo

                if (modifiedSince >= lastModified)
                {
                    context.Response.StatusCode = 304;
                    return;
                }
            }
        }

        await _next(context);

        // Adicionar headers de cache
        if (context.Response.StatusCode == 200)
        {
            context.Response.Headers.Add("Cache-Control", "public, max-age=300");
            context.Response.Headers.Add("Last-Modified", DateTime.UtcNow.ToString("R"));
        }
    }
}

// Program.cs (registro do middleware customizado)
app.UseMiddleware<ConditionalCacheMiddleware>();