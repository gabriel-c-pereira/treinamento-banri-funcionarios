# 📝 Exercícios - Versioning

## 🎯 Objetivo

Praticar versionamento de APIs no ASP.NET Core usando diferentes estratégias (URI, query string, header) e implementar compatibilidade entre versões.

---

## ✏️ Exercício 1: URI Versioning Básico

**Dificuldade**: ⭐ Iniciante

Configure API versioning usando URI versioning com diferentes versões do mesmo recurso:

### Requisitos:
1. **Configuração no Program.cs**:
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
```

2. **Modelos para diferentes versões**:
```csharp
// Versão 1.0 - Básica
public class ProdutoV1
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}

// Versão 2.0 - Expandida
public class ProdutoV2
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}
```

3. **Controller versão 1.0**:
```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProdutosV1Controller : ControllerBase
{
    private static readonly List<ProdutoV1> _produtos = new()
    {
        new ProdutoV1 { Id = 1, Nome = "Produto A", Preco = 10.99m },
        new ProdutoV1 { Id = 2, Nome = "Produto B", Preco = 25.50m }
    };

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoV1>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ProdutoV1>> GetProdutos()
    {
        return Ok(_produtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProdutoV1), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProdutoV1> GetProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        return produto == null ? NotFound() : Ok(produto);
    }
}
```

4. **Controller versão 2.0**:
```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class ProdutosV2Controller : ControllerBase
{
    private static readonly List<ProdutoV2> _produtos = new()
    {
        new ProdutoV2
        {
            Id = 1,
            Nome = "Produto A",
            Descricao = "Descrição detalhada do Produto A",
            Preco = 10.99m,
            Estoque = 100,
            DataCadastro = DateTime.Now.AddDays(-30),
            Ativo = true
        },
        new ProdutoV2
        {
            Id = 2,
            Nome = "Produto B",
            Descricao = "Descrição detalhada do Produto B",
            Preco = 25.50m,
            Estoque = 50,
            DataCadastro = DateTime.Now.AddDays(-15),
            Ativo = true
        }
    };

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoV2>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ProdutoV2>> GetProdutos()
    {
        return Ok(_produtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProdutoV2), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProdutoV2> GetProduto(int id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        return produto == null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProdutoV2), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<ProdutoV2> PostProduto([FromBody] ProdutoV2 produto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        produto.Id = _produtos.Max(p => p.Id) + 1;
        produto.DataCadastro = DateTime.Now;
        _produtos.Add(produto);

        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
    }
}
```

5. **Testes das versões**:
```bash
# Versão 1.0
curl -H "Accept: application/json" "https://localhost:5001/api/v1.0/produtos"
curl -H "Accept: application/json" "https://localhost:5001/api/v1.0/produtos/1"

# Versão 2.0
curl -H "Accept: application/json" "https://localhost:5001/api/v2.0/produtos"
curl -H "Accept: application/json" "https://localhost:5001/api/v2.0/produtos/1"

# POST na versão 2.0
curl -X POST "https://localhost:5001/api/v2.0/produtos" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Novo Produto",
    "descricao": "Descrição do novo produto",
    "preco": 99.99,
    "estoque": 10,
    "ativo": true
  }'
```

---

## ✏️ Exercício 2: Query String Versioning

**Dificuldade**: ⭐⭐ Intermediário

Implemente versioning via query string com validação de versão:

### Requisitos:
1. **Configuração para query string**:
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;

    // Configura leitura da versão via query string
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});
```

2. **Controller com múltiplas versões no mesmo endpoint**:
```csharp
[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ClientesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public IActionResult GetClientes()
    {
        var apiVersion = HttpContext.GetRequestedApiVersion();

        if (apiVersion == new ApiVersion(1, 0))
        {
            // Versão 1.0 - Dados básicos
            var clientesV1 = new[]
            {
                new { Id = 1, Nome = "João Silva", Email = "joao@email.com" },
                new { Id = 2, Nome = "Maria Santos", Email = "maria@email.com" }
            };
            return Ok(clientesV1);
        }
        else if (apiVersion == new ApiVersion(2, 0))
        {
            // Versão 2.0 - Dados expandidos
            var clientesV2 = new[]
            {
                new { Id = 1, Nome = "João Silva", Email = "joao@email.com", Telefone = "11999999999", DataCadastro = DateTime.Now.AddDays(-30) },
                new { Id = 2, Nome = "Maria Santos", Email = "maria@email.com", Telefone = "11888888888", DataCadastro = DateTime.Now.AddDays(-15) }
            };
            return Ok(clientesV2);
        }

        return BadRequest("Versão da API não suportada");
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCliente(int id)
    {
        var apiVersion = HttpContext.GetRequestedApiVersion();

        if (apiVersion == new ApiVersion(1, 0))
        {
            var cliente = new { Id = id, Nome = "João Silva", Email = "joao@email.com" };
            return Ok(cliente);
        }
        else if (apiVersion == new ApiVersion(2, 0))
        {
            var cliente = new
            {
                Id = id,
                Nome = "João Silva",
                Email = "joao@email.com",
                Telefone = "11999999999",
                DataCadastro = DateTime.Now.AddDays(-30),
                Endereco = new { Rua = "Rua A", Numero = "123", Cidade = "São Paulo" }
            };
            return Ok(cliente);
        }

        return BadRequest("Versão da API não suportada");
    }
}
```

3. **Middleware para validação de versão**:
```csharp
public class ApiVersionValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiVersion[] _supportedVersions = { new(1, 0), new(2, 0) };

    public ApiVersionValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestedVersion = context.GetRequestedApiVersion();

        if (requestedVersion != null && !_supportedVersions.Contains(requestedVersion))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Versão da API não suportada",
                supportedVersions = _supportedVersions.Select(v => v.ToString()),
                requestedVersion = requestedVersion.ToString()
            });
            return;
        }

        await _next(context);
    }
}
```

4. **Testes com query string**:
```bash
# Versão 1.0 via query string
curl -H "Accept: application/json" "https://localhost:5001/api/clientes?api-version=1.0"
curl -H "Accept: application/json" "https://localhost:5001/api/clientes/1?api-version=1.0"

# Versão 2.0 via query string
curl -H "Accept: application/json" "https://localhost:5001/api/clientes?api-version=2.0"
curl -H "Accept: application/json" "https://localhost:5001/api/clientes/1?api-version=2.0"

# Versão não suportada
curl -H "Accept: application/json" "https://localhost:5001/api/clientes?api-version=3.0"
```

---

## ✏️ Exercício 3: Versionamento de Modelos

**Dificuldade**: ⭐⭐⭐ Avançado

Crie diferentes modelos para diferentes versões da API com mapeamento automático:

### Requisitos:
1. **Modelos fortemente tipados**:
```csharp
// Versão 1.0
public class ClienteV1
{
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
}

// Versão 2.0
public class ClienteV2
{
    public int Id { get; set; }
    [Required] public string Nome { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
    [Phone] public string Telefone { get; set; }
    public DateTime DataCadastro { get; set; }
    public EnderecoV2 Endereco { get; set; }
    public bool Ativo { get; set; } = true;
}

public class EnderecoV2
{
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string CEP { get; set; }
}

// Versão 3.0 (se necessário no futuro)
public class ClienteV3 : ClienteV2
{
    public string CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public List<string> Tags { get; set; } = new();
}
```

2. **Serviço de mapeamento entre versões**:
```csharp
public interface IClienteMapper
{
    ClienteV2 MapToV2(ClienteV1 clienteV1);
    ClienteV1 MapToV1(ClienteV2 clienteV2);
    ClienteV3 MapToV3(ClienteV2 clienteV2);
}

public class ClienteMapper : IClienteMapper
{
    public ClienteV2 MapToV2(ClienteV1 clienteV1)
    {
        return new ClienteV2
        {
            Id = clienteV1.Id,
            Nome = clienteV1.Nome,
            Email = clienteV1.Email,
            DataCadastro = DateTime.Now,
            Ativo = true
        };
    }

    public ClienteV1 MapToV1(ClienteV2 clienteV2)
    {
        return new ClienteV1
        {
            Id = clienteV2.Id,
            Nome = clienteV2.Nome,
            Email = clienteV2.Email
        };
    }

    public ClienteV3 MapToV3(ClienteV2 clienteV2)
    {
        return new ClienteV3
        {
            Id = clienteV2.Id,
            Nome = clienteV2.Nome,
            Email = clienteV2.Email,
            Telefone = clienteV2.Telefone,
            DataCadastro = clienteV2.DataCadastro,
            Endereco = clienteV2.Endereco,
            Ativo = clienteV2.Ativo,
            DataNascimento = DateTime.MinValue, // Valor padrão
            Tags = new List<string>()
        };
    }
}
```

3. **Controllers para cada versão**:
```csharp
[ApiController]
[Route("api/v{version:apiVersion}/clientes")]
[ApiVersion("1.0")]
public class ClientesV1Controller : ControllerBase
{
    private readonly IClienteMapper _mapper;

    public ClientesV1Controller(IClienteMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult<ClienteV1> PostCliente([FromBody] ClienteV1 cliente)
    {
        // Lógica para salvar versão 1.0
        // Pode mapear para versão interna se necessário
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    [HttpGet("{id}")]
    public ActionResult<ClienteV1> GetCliente(int id)
    {
        // Retorna sempre versão 1.0
        var cliente = new ClienteV1 { Id = id, Nome = "Cliente", Email = "cliente@email.com" };
        return Ok(cliente);
    }
}

[ApiController]
[Route("api/v{version:apiVersion}/clientes")]
[ApiVersion("2.0")]
public class ClientesV2Controller : ControllerBase
{
    private readonly IClienteMapper _mapper;

    public ClientesV2Controller(IClienteMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult<ClienteV2> PostCliente([FromBody] ClienteV2 cliente)
    {
        // Lógica para salvar versão 2.0
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    [HttpGet("{id}")]
    public ActionResult<ClienteV2> GetCliente(int id)
    {
        // Retorna versão 2.0
        var cliente = new ClienteV2
        {
            Id = id,
            Nome = "Cliente",
            Email = "cliente@email.com",
            Telefone = "11999999999",
            DataCadastro = DateTime.Now,
            Ativo = true
        };
        return Ok(cliente);
    }
}
```

4. **Configuração de mapeamento automático**:
```csharp
builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<ClienteV1, ClienteV2>()
        .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.Now))
        .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true));

    config.CreateMap<ClienteV2, ClienteV1>();

    config.CreateMap<ClienteV2, ClienteV3>()
        .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => DateTime.MinValue))
        .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => new List<string>()));
});

builder.Services.AddScoped<IClienteMapper, ClienteMapper>();
```

5. **Uso do AutoMapper nos controllers**:
```csharp
public class ClientesV2Controller : ControllerBase
{
    private readonly IMapper _mapper;

    public ClientesV2Controller(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("from-v1")]
    public ActionResult<ClienteV2> PostClienteFromV1([FromBody] ClienteV1 clienteV1)
    {
        var clienteV2 = _mapper.Map<ClienteV2>(clienteV1);
        // Salvar clienteV2...
        return CreatedAtAction(nameof(GetCliente), new { id = clienteV2.Id }, clienteV2);
    }
}
```
