// Modelos
[ApiVersion("1.0")]
public class ProdutoV1
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
}

[ApiVersion("2.0")]
public class ProdutoV2 : ProdutoV1
{
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; }
    public DateTime DataCriacao { get; set; }
}

[ApiVersion("3.0")]
public class ProdutoV3 : ProdutoV2
{
    public List<string> Tags { get; set; } = new();
    public decimal? PrecoPromocional { get; set; }
    public int Estoque { get; set; }
}

// Controller
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
public class ProdutosController : ControllerBase
{
    private readonly List<ProdutoV3> _produtos = new()
    {
        new ProdutoV3
        {
            Id = 1,
            Nome = "Notebook Dell",
            Ativo = true,
            Descricao = "Notebook para trabalho",
            Preco = 3500.00m,
            Categoria = "Eletrônicos",
            DataCriacao = DateTime.Now.AddDays(-10),
            Tags = new() { "notebook", "trabalho", "dell" },
            Estoque = 15
        }
    };

    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetV1()
    {
        var produtosV1 = _produtos.Select(p => new ProdutoV1
        {
            Id = p.Id,
            Nome = p.Nome,
            Ativo = p.Ativo
        });

        return Ok(produtosV1);
    }

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2()
    {
        var produtosV2 = _produtos.Select(p => new ProdutoV2
        {
            Id = p.Id,
            Nome = p.Nome,
            Ativo = p.Ativo,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Categoria = p.Categoria,
            DataCriacao = p.DataCriacao
        });

        return Ok(produtosV2);
    }

    [HttpGet]
    [MapToApiVersion("3.0")]
    public IActionResult GetV3()
    {
        return Ok(_produtos);
    }

    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    public IActionResult GetByIdV1(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return NotFound();

        return Ok(new ProdutoV1
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Ativo = produto.Ativo
        });
    }

    [HttpGet("{id}")]
    [MapToApiVersion("2.0")]
    public IActionResult GetByIdV2(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return NotFound();

        return Ok(new ProdutoV2
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Ativo = produto.Ativo,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Categoria = produto.Categoria,
            DataCriacao = produto.DataCriacao
        });
    }

    [HttpGet("{id}")]
    [MapToApiVersion("3.0")]
    public IActionResult GetByIdV3(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return NotFound();

        return Ok(produto);
    }
}