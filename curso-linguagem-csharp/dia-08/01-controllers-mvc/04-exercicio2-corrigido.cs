[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    [HttpGet("search")]
    public IActionResult SearchPedidos(
        [FromQuery] string status,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int page = 1)
    {
        // Lógica de busca
        return Ok(new { status, dataInicio, dataFim, page });
    }

    [HttpGet("{id}")]
    public IActionResult GetPedido(int id)
    {
        // Buscar pedido por ID
        return Ok(new { id, status = "Pendente" });
    }

    [HttpPost]
    public IActionResult CreatePedido([FromBody] CreatePedidoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Criar pedido
        var pedido = new { id = 1, clienteId = request.ClienteId, total = request.Total };
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.id }, pedido);
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        // Atualizar status
        return NoContent();
    }
}

// DTOs
public class CreatePedidoRequest
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    [MinLength(1)]
    public List<ItemPedidoRequest> Itens { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }
}

public class ItemPedidoRequest
{
    [Required]
    public int ProdutoId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; }
}

public class UpdateStatusRequest
{
    [Required]
    public string Status { get; set; }
}