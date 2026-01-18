public class PedidoService
{
    public void ProcessarPedido(Pedido pedido)
    {
        // Validação imediata
        ArgumentNullException.ThrowIfNull(pedido);
        
        if (pedido.Itens.Count == 0)
            throw new InvalidOperationException("Pedido sem itens");
        
        if (pedido.Total <= 0)
            throw new InvalidOperationException("Total inválido");

        // Processar apenas se válido
        SalvarPedido(pedido);
    }
}

// ═══════════════════════════════════════════════════

public class PedidoService
{
    public Result ProcessarPedido(Pedido pedido)
    {
        // Validações com returns
        if (pedido == null)
            return Result.Fail("Pedido nulo");
        
        if (pedido.Itens == null || pedido.Itens.Count == 0)
            return Result.Fail("Pedido sem itens");
        
        if (pedido.Total <= 0)
            return Result.Fail("Total inválido");

        try
        {
            SalvarPedido(pedido);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao salvar");
            return Result.Fail("Erro ao processar");
        }
    }
}