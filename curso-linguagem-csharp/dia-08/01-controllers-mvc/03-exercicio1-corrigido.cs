[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientes()
    {
        var clientes = await _service.GetAllAsync();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCliente(int id)
    {
        var cliente = await _service.GetByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente([FromBody] CreateClienteRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }
}

// DTOs
public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataCadastro { get; set; }
}

public class CreateClienteRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}