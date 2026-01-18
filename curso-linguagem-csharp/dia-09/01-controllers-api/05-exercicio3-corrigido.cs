[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    [HttpGet("produtos")]
    public IActionResult GetRelatorioProdutos([FromQuery] string format = "json")
    {
        var produtos = new[]
        {
            new { Id = 1, Nome = "Produto 1", Preco = 10.99m },
            new { Id = 2, Nome = "Produto 2", Preco = 25.50m }
        };

        switch (format.ToLower())
        {
            case "xml":
                return new ContentResult
                {
                    Content = SerializeToXml(produtos),
                    ContentType = "application/xml",
                    StatusCode = 200
                };

            case "csv":
                return new ContentResult
                {
                    Content = SerializeToCsv(produtos),
                    ContentType = "text/csv",
                    StatusCode = 200
                };

            default:
                return Ok(produtos);
        }
    }

    private string SerializeToXml(object data)
    {
        // Implementação simplificada
        return "<produtos><produto><id>1</id><nome>Produto 1</nome></produto></produtos>";
    }

    private string SerializeToCsv(object data)
    {
        // Implementação simplificada
        return "Id,Nome,Preco\n1,Produto 1,10.99\n2,Produto 2,25.50";
    }
}