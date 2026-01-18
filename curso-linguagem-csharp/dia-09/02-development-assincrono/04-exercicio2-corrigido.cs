[ApiController]
[Route("api/processamento")]
public class ProcessamentoController : ControllerBase
{
    private readonly IProcessamentoService _service;

    public ProcessamentoController(IProcessamentoService service)
    {
        _service = service;
    }

    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatusProcessamento(
        int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var status = await _service.GetStatusAsync(id, cancellationToken);
            return Ok(status);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499, new { message = "Operação cancelada pelo cliente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno", details = ex.Message });
        }
    }

    [HttpPost("{id}/cancelar")]
    public async Task<IActionResult> CancelarProcessamento(int id)
    {
        var result = await _service.CancelarAsync(id);
        if (!result)
            return NotFound();

        return Ok(new { message = "Processamento cancelado" });
    }
}

public interface IProcessamentoService
{
    Task<ProcessamentoStatus> GetStatusAsync(int id, CancellationToken cancellationToken);
    Task<bool> CancelarAsync(int id);
}

public class ProcessamentoStatus
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int Progresso { get; set; }
    public DateTime Inicio { get; set; }
}