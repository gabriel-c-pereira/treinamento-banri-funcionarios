// Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Controller
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProdutosController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetV1()
    {
        var produtos = new[]
        {
            new { Id = 1, Nome = "Produto 1" },
            new { Id = 2, Nome = "Produto 2" }
        };

        return Ok(new
        {
            Versao = "1.0",
            Dados = produtos
        });
    }

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2()
    {
        var produtos = new[]
        {
            new { Id = 1, Nome = "Produto 1", Preco = 10.99m, Categoria = "Eletrônicos" },
            new { Id = 2, Nome = "Produto 2", Preco = 25.50m, Categoria = "Livros" }
        };

        return Ok(new
        {
            Versao = "2.0",
            Dados = produtos,
            Metadata = new { Total = produtos.Length, Pagina = 1 }
        });
    }
}