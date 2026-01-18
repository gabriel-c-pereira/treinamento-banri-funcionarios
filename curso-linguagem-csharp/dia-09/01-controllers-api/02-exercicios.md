# 📝 Exercícios - Controllers API

## 🎯 Objetivo

Praticar a criação de APIs RESTful no ASP.NET Core, incluindo operações CRUD, model binding, validação, content negotiation e diferentes formatos de resposta.

---

## ✏️ Exercício 1: Controller CRUD Básico

**Dificuldade**: ⭐ Iniciante

Crie um controller `ClientesController` com operações CRUD completas:

### Requisitos:
1. **Modelo Cliente**:
```csharp
public class Cliente
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string Telefone { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public bool Ativo { get; set; } = true;
}
```

2. **ClientesController**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    // Simulação de banco de dados em memória
    private static List<Cliente> _clientes = new();
    private static int _proximoId = 1;

    // GET api/clientes
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Cliente>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Cliente>> GetClientes(
        [FromQuery] string? nome = null,
        [FromQuery] bool? ativo = null)
    {
        var query = _clientes.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

        if (ativo.HasValue)
            query = query.Where(c => c.Ativo == ativo.Value);

        return Ok(query.OrderBy(c => c.Nome));
    }

    // GET api/clientes/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Cliente> GetCliente(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente == null)
            return NotFound($"Cliente com ID {id} não encontrado");

        return Ok(cliente);
    }

    // POST api/clientes
    [HttpPost]
    [ProducesResponseType(typeof(Cliente), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Cliente> PostCliente([FromBody] Cliente cliente)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        cliente.Id = _proximoId++;
        cliente.DataCadastro = DateTime.Now;
        _clientes.Add(cliente);

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    // PUT api/clientes/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutCliente(int id, [FromBody] Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest("ID da URL não corresponde ao ID do cliente");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var clienteExistente = _clientes.FirstOrDefault(c => c.Id == id);
        if (clienteExistente == null)
            return NotFound($"Cliente com ID {id} não encontrado");

        // Atualizar propriedades
        clienteExistente.Nome = cliente.Nome;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Telefone = cliente.Telefone;
        clienteExistente.DataNascimento = cliente.DataNascimento;
        clienteExistente.Ativo = cliente.Ativo;

        return NoContent();
    }

    // DELETE api/clientes/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteCliente(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente == null)
            return NotFound($"Cliente com ID {id} não encontrado");

        _clientes.Remove(cliente);
        return NoContent();
    }
}
```

3. **Testes com Postman/curl**:
```bash
# GET todos os clientes
curl -X GET "https://localhost:5001/api/clientes" -H "accept: application/json"

# GET cliente específico
curl -X GET "https://localhost:5001/api/clientes/1" -H "accept: application/json"

# POST novo cliente
curl -X POST "https://localhost:5001/api/clientes" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao@email.com",
    "telefone": "11999999999",
    "dataNascimento": "1990-01-01"
  }'

# PUT atualizar cliente
curl -X PUT "https://localhost:5001/api/clientes/1" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "nome": "João Silva Atualizado",
    "email": "joao.atualizado@email.com",
    "telefone": "11999999999",
    "dataNascimento": "1990-01-01",
    "ativo": true
  }'

# DELETE cliente
curl -X DELETE "https://localhost:5001/api/clientes/1"
```

---

## ✏️ Exercício 2: Model Binding e Validação Avançada

**Dificuldade**: ⭐⭐ Intermediário

Implemente um controller `PedidosController` com validação avançada:

### Requisitos:
1. **Modelos**:
```csharp
public class Pedido
{
    public int Id { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }

    [Required]
    public DateTime DataPedido { get; set; } = DateTime.Now;

    public StatusPedido Status { get; set; } = StatusPedido.Pendente;

    [MinLength(1, ErrorMessage = "Pedido deve ter pelo menos um item")]
    public List<ItemPedido> Itens { get; set; } = new();

    [NotMapped] // Calculado
    public decimal ValorTotal => Itens.Sum(i => i.ValorTotal);
}

public class ItemPedido
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string NomeProduto { get; set; }

    [Required]
    [Range(1, 999, ErrorMessage = "Quantidade deve ser entre 1 e 999")]
    public int Quantidade { get; set; }

    [Required]
    [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser entre 0.01 e 999999.99")]
    public decimal PrecoUnitario { get; set; }

    [NotMapped]
    public decimal ValorTotal => Quantidade * PrecoUnitario;
}

public enum StatusPedido
{
    Pendente,
    Processando,
    Enviado,
    Entregue,
    Cancelado
}
```

2. **Validação Customizada**:
```csharp
public class PedidoValidator : AbstractValidator<Pedido>
{
    public PedidoValidator()
    {
        RuleFor(p => p.ClienteId)
            .GreaterThan(0)
            .WithMessage("Cliente deve ser informado");

        RuleFor(p => p.DataPedido)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Data do pedido não pode ser futura");

        RuleFor(p => p.Itens)
            .NotEmpty()
            .WithMessage("Pedido deve ter pelo menos um item");

        RuleForEach(p => p.Itens)
            .SetValidator(new ItemPedidoValidator());
    }
}

public class ItemPedidoValidator : AbstractValidator<ItemPedido>
{
    public ItemPedidoValidator()
    {
        RuleFor(i => i.ProdutoId)
            .GreaterThan(0);

        RuleFor(i => i.NomeProduto)
            .NotEmpty()
            .Length(2, 100);

        RuleFor(i => i.Quantidade)
            .InclusiveBetween(1, 999);

        RuleFor(i => i.PrecoUnitario)
            .InclusiveBetween(0.01m, 999999.99m);
    }
}
```

3. **PedidosController com diferentes tipos de binding**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private static List<Pedido> _pedidos = new();
    private static int _proximoId = 1;

    // GET api/pedidos?clienteId=1&status=Pendente
    [HttpGet]
    public ActionResult<IEnumerable<Pedido>> GetPedidos(
        [FromQuery] int? clienteId = null,
        [FromQuery] StatusPedido? status = null,
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null)
    {
        var query = _pedidos.AsQueryable();

        if (clienteId.HasValue)
            query = query.Where(p => p.ClienteId == clienteId);

        if (status.HasValue)
            query = query.Where(p => p.Status == status);

        if (dataInicio.HasValue)
            query = query.Where(p => p.DataPedido >= dataInicio);

        if (dataFim.HasValue)
            query = query.Where(p => p.DataPedido <= dataFim);

        return Ok(query.OrderByDescending(p => p.DataPedido));
    }

    // GET api/pedidos/5
    [HttpGet("{id}")]
    public ActionResult<Pedido> GetPedido(int id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        return pedido == null ? NotFound() : Ok(pedido);
    }

    // POST api/pedidos
    [HttpPost]
    public ActionResult<Pedido> PostPedido([FromBody] Pedido pedido)
    {
        var validator = new PedidoValidator();
        var result = validator.Validate(pedido);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        pedido.Id = _proximoId++;
        _pedidos.Add(pedido);

        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
    }

    // PUT api/pedidos/5/status
    [HttpPut("{id}/status")]
    public IActionResult PutStatusPedido(int id, [FromBody] StatusPedido novoStatus)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido == null)
            return NotFound();

        // Validação de transição de status
        if (!IsTransicaoValida(pedido.Status, novoStatus))
            return BadRequest("Transição de status inválida");

        pedido.Status = novoStatus;
        return NoContent();
    }

    private bool IsTransicaoValida(StatusPedido atual, StatusPedido novo)
    {
        return (atual, novo) switch
        {
            (StatusPedido.Pendente, StatusPedido.Processando) => true,
            (StatusPedido.Processando, StatusPedido.Enviado) => true,
            (StatusPedido.Enviado, StatusPedido.Entregue) => true,
            (StatusPedido.Pendente, StatusPedido.Cancelado) => true,
            (StatusPedido.Processando, StatusPedido.Cancelado) => true,
            _ => false
        };
    }
}
```

---

## ✏️ Exercício 3: Content Negotiation

**Dificuldade**: ⭐⭐⭐ Avançado

Crie um controller que suporte diferentes formatos de resposta:

### Requisitos:
1. **Modelo de resposta**:
```csharp
public class RelatorioVendas
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public List<Venda> Vendas { get; set; } = new();
    public decimal TotalVendas => Vendas.Sum(v => v.Valor);
    public int TotalItens => Vendas.Sum(v => v.Quantidade);
}

public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Cliente { get; set; }
    public string Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
}
```

2. **Controller com múltiplos formatos**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    // GET api/relatorios/vendas?formato=json
    [HttpGet("vendas")]
    [Produces("application/json", "application/xml", "text/csv", "application/pdf")]
    public IActionResult GetRelatorioVendas(
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null,
        [FromQuery] string formato = "json")
    {
        // Dados de exemplo
        var relatorio = new RelatorioVendas
        {
            DataInicio = dataInicio ?? DateTime.Now.AddDays(-30),
            DataFim = dataFim ?? DateTime.Now,
            Vendas = new List<Venda>
            {
                new Venda { Id = 1, Data = DateTime.Now, Cliente = "João", Produto = "Notebook", Quantidade = 1, Valor = 2500 },
                new Venda { Id = 2, Data = DateTime.Now, Cliente = "Maria", Produto = "Mouse", Quantidade = 2, Valor = 100 }
            }
        };

        return formato.ToLower() switch
        {
            "json" => Ok(relatorio),
            "xml" => Ok(relatorio), // ASP.NET Core converte automaticamente
            "csv" => GerarCsv(relatorio),
            "pdf" => GerarPdf(relatorio),
            _ => BadRequest("Formato não suportado")
        };
    }

    private IActionResult GerarCsv(RelatorioVendas relatorio)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Id,Data,Cliente,Produto,Quantidade,Valor");

        foreach (var venda in relatorio.Vendas)
        {
            csv.AppendLine($"{venda.Id},{venda.Data:yyyy-MM-dd},{venda.Cliente},{venda.Produto},{venda.Quantidade},{venda.Valor:F2}");
        }

        csv.AppendLine($",,,TOTAL,,{relatorio.TotalVendas:F2}");

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "relatorio-vendas.csv");
    }

    private IActionResult GerarPdf(RelatorioVendas relatorio)
    {
        // Simulação - em produção usaria biblioteca como iTextSharp ou PdfSharp
        var html = $@"
        <html>
        <body>
            <h1>Relatório de Vendas</h1>
            <p>Período: {relatorio.DataInicio:dd/MM/yyyy} - {relatorio.DataFim:dd/MM/yyyy}</p>
            <table border='1'>
                <tr><th>ID</th><th>Data</th><th>Cliente</th><th>Produto</th><th>Qtde</th><th>Valor</th></tr>
                {@string.Join("", relatorio.Vendas.Select(v => $"<tr><td>{v.Id}</td><td>{v.Data:dd/MM}</td><td>{v.Cliente}</td><td>{v.Produto}</td><td>{v.Quantidade}</td><td>R$ {v.Valor:F2}</td></tr>"))}
                <tr><td colspan='5'><strong>TOTAL</strong></td><td><strong>R$ {relatorio.TotalVendas:F2}</strong></td></tr>
            </table>
        </body>
        </html>";

        // Em produção, converter HTML para PDF
        return Content(html, "text/html");
    }
}
```

3. **Configuração para XML**:
```csharp
// No Program.cs
builder.Services.AddControllers()
    .AddXmlSerializerFormatters(); // Habilita suporte a XML
```

4. **Testes dos diferentes formatos**:
```bash
# JSON (padrão)
curl -H "Accept: application/json" "https://localhost:5001/api/relatorios/vendas"

# XML
curl -H "Accept: application/xml" "https://localhost:5001/api/relatorios/vendas"

# CSV
curl -H "Accept: text/csv" "https://localhost:5001/api/relatorios/vendas" -o relatorio.csv

# PDF
curl -H "Accept: application/pdf" "https://localhost:5001/api/relatorios/vendas" -o relatorio.pdf
```
