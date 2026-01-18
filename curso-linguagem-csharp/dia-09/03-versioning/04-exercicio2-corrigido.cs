// Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Controller
[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ClientesController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetClientesV1()
    {
        var clientes = new[]
        {
            new { Id = 1, Nome = "João Silva" },
            new { Id = 2, Nome = "Maria Santos" }
        };

        return Ok(clientes);
    }

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetClientesV2([FromQuery] string filtro = null)
    {
        var clientes = new[]
        {
            new {
                Id = 1,
                Nome = "João Silva",
                Email = "joao@email.com",
                DataCadastro = DateTime.Now.AddDays(-30)
            },
            new {
                Id = 2,
                Nome = "Maria Santos",
                Email = "maria@email.com",
                DataCadastro = DateTime.Now.AddDays(-15)
            }
        };

        if (!string.IsNullOrEmpty(filtro))
        {
            clientes = clientes.Where(c => c.Nome.Contains(filtro)).ToArray();
        }

        return Ok(new
        {
            FiltroAplicado = filtro,
            Resultados = clientes,
            Total = clientes.Length
        });
    }

    [HttpGet("versoes-suportadas")]
    public IActionResult GetVersoesSuportadas()
    {
        return Ok(new[]
        {
            new { Versao = "1.0", Status = "Suportada", Descontinuacao = (DateTime?)null },
            new { Versao = "2.0", Status = "Atual", Descontinuacao = (DateTime?)null }
        });
    }
}