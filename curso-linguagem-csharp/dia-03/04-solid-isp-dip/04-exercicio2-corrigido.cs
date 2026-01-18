// ✅ Inversão de Dependência (DIP - Dependency Inversion Principle)

// Abstração (interface)
public interface IPagamentoService
{
    void Pagar(decimal valor);
    bool ValidarPagamento();
}

// Implementações concretas
public class PagSeguroService : IPagamentoService
{
    public void Pagar(decimal valor) => Console.WriteLine($"PagSeguro: R$ {valor:F2}");
    public bool ValidarPagamento() => true;
}

public class MercadoPagoService : IPagamentoService
{
    public void Pagar(decimal valor) => Console.WriteLine($"MercadoPago: R$ {valor:F2}");
    public bool ValidarPagamento() => true;
}

// Classe depende da abstração, não da implementação
public class ProcessadorPagamento
{
    private readonly IPagamentoService _pagamentoService;

    // Injeção de dependência via construtor
    public ProcessadorPagamento(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    public void Processar(decimal valor)
    {
        if (_pagamentoService.ValidarPagamento())
        {
            _pagamentoService.Pagar(valor);
            Console.WriteLine("Pagamento processado com sucesso!");
        }
        else
        {
            Console.WriteLine("Falha na validação do pagamento");
        }
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        // Usando PagSeguro
        var pagSeguro = new PagSeguroService();
        var processador1 = new ProcessadorPagamento(pagSeguro);
        processador1.Processar(100.00m);

        Console.WriteLine();

        // Usando MercadoPago (facilmente substituível)
        var mercadoPago = new MercadoPagoService();
        var processador2 = new ProcessadorPagamento(mercadoPago);
        processador2.Processar(250.50m);
    }
}
