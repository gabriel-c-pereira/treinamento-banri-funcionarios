[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    [HttpGet("search")]
    public IActionResult SearchPedidos(
        [FromQuery] string status,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Implementar busca
        return Ok(new { status, dataInicio, dataFim, page, pageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPedido(int id)
    {
        // Buscar pedido
        return Ok(new { id, status = "Pendente" });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePedido([FromBody] CreatePedidoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Criar pedido
        var pedido = new { id = 1, clienteId = request.ClienteId };
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.id }, pedido);
    }

    [HttpPost("{id}/upload")]
    public async Task<IActionResult> UploadArquivo(int id, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Arquivo não fornecido");

        // Processar arquivo
        return Ok(new { fileName = file.FileName, size = file.Length });
    }

    [HttpGet("info")]
    public IActionResult GetInfo([FromHeader] string authorization)
    {
        return Ok(new { authorized = !string.IsNullOrEmpty(authorization) });
    }
}

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