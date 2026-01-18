// Interface
public interface IImprimivel
{
    void Imprimir();
    string ObterDescricao();
}

// Classe Documento
public class Documento : IImprimivel
{
    public string Titulo { get; set; }
    public string Conteudo { get; set; }

    public void Imprimir()
    {
        Console.WriteLine($"Imprimindo documento: {Titulo}");
        Console.WriteLine(Conteudo);
    }

    public string ObterDescricao()
    {
        return $"Documento: {Titulo}";
    }
}

// Classe Relatorio
public class Relatorio : IImprimivel
{
    public string Nome { get; set; }
    public DateTime Data { get; set; }
    public List<string> Itens { get; set; } = new List<string>();

    public void Imprimir()
    {
        Console.WriteLine($"=== RELATÓRIO: {Nome} ===");
        Console.WriteLine($"Data: {Data:dd/MM/yyyy}");
        Console.WriteLine("Itens:");
        foreach (var item in Itens)
            Console.WriteLine($"- {item}");
    }

    public string ObterDescricao()
    {
        return $"Relatório: {Nome} ({Itens.Count} itens)";
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        List<IImprimivel> documentos = new List<IImprimivel>
        {
            new Documento { Titulo = "Contrato", Conteudo = "Texto do contrato..." },
            new Relatorio { Nome = "Vendas", Data = DateTime.Now, Itens = new List<string> {"Item 1", "Item 2"} }
        };

        foreach (var doc in documentos)
        {
            Console.WriteLine(doc.ObterDescricao());
            doc.Imprimir();
            Console.WriteLine();
        }
    }
}
