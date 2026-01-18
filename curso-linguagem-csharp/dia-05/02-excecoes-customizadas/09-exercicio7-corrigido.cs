// Base
public abstract class ECommerceException : Exception
{
    public string CodigoErro { get; }
    public DateTime Timestamp { get; }

    protected ECommerceException(string codigo, string message, Exception? inner = null)
        : base(message, inner)
    {
        CodigoErro = codigo;
        Timestamp = DateTime.UtcNow;
    }
}

// Carrinho
public class CarrinhoVazioException : ECommerceException
{
    public string UsuarioId { get; }

    public CarrinhoVazioException(string usuarioId)
        : base("CART_001", $"Carrinho do usuário {usuarioId} está vazio")
    {
        UsuarioId = usuarioId;
    }
}

// Estoque
public class EstoqueInsuficienteException : ECommerceException
{
    public int ProdutoId { get; }
    public int QuantidadeSolicitada { get; }
    public int QuantidadeDisponivel { get; }

    public EstoqueInsuficienteException(int produtoId, int solicitada, int disponivel)
        : base("INV_001", 
            $"Produto {produtoId}: solicitado {solicitada}, disponível {disponivel}")
    {
        ProdutoId = produtoId;
        QuantidadeSolicitada = solicitada;
        QuantidadeDisponivel = disponivel;
    }
}

// Pagamento
public class PagamentoRecusadoException : ECommerceException
{
    public string MotivoBanco { get; }
    public decimal Valor { get; }

    public PagamentoRecusadoException(decimal valor, string motivo)
        : base("PAY_001", $"Pagamento de {valor:C} recusado: {motivo}")
    {
        Valor = valor;
        MotivoBanco = motivo;
    }
}

// Checkout
public class CheckoutException : ECommerceException
{
    public int PedidoId { get; }
    public List<string> Erros { get; }

    public CheckoutException(int pedidoId, List<string> erros, Exception? inner = null)
        : base("CHK_001", 
            $"Checkout do pedido {pedidoId} falhou com {erros.Count} erro(s)", 
            inner)
    {
        PedidoId = pedidoId;
        Erros = erros;
    }
}

// ═══════════════════════════════════════════════════

public class CheckoutService
{
    public async Task<Pedido> ProcessarCheckout(Carrinho carrinho)
    {
        var erros = new List<string>();

        // 1. Validar carrinho
        if (!carrinho.Itens.Any())
            throw new CarrinhoVazioException(carrinho.UsuarioId);

        // 2. Verificar estoque
        foreach (var item in carrinho.Itens)
        {
            var estoque = await ObterEstoque(item.ProdutoId);
            if (estoque < item.Quantidade)
            {
                erros.Add($"Produto {item.ProdutoId}: estoque insuficiente");
            }
        }

        if (erros.Any())
            throw new CheckoutException(carrinho.Id, erros);

        // 3. Processar pagamento
        try
        {
            await ProcessarPagamento(carrinho.Total);
        }
        catch (PagamentoRecusadoException ex)
        {
            throw new CheckoutException(
                carrinho.Id, 
                new List<string> { "Pagamento recusado" }, 
                ex);
        }

        // 4. Criar pedido
        return CriarPedido(carrinho);
    }
}