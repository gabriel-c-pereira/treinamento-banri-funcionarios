// Interface para persistência
public interface ISalvar
{
    void Salvar();
    void Carregar();
}

// Interface para validação
public interface IValidar
{
    bool EhValido();
    List<string> ObterErros();
}

// Classe Produto implementando ambas
public class Produto : ISalvar, IValidar
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    // Implementação ISalvar
    public void Salvar()
    {
        Console.WriteLine($"Salvando produto: {Nome}");
        // Lógica de salvar no banco...
    }

    public void Carregar()
    {
        Console.WriteLine($"Carregando produto: {Nome}");
        // Lógica de carregar do banco...
    }

    // Implementação IValidar
    public bool EhValido()
    {
        return !string.IsNullOrEmpty(Nome) && Preco > 0 && Estoque >= 0;
    }

    public List<string> ObterErros()
    {
        var erros = new List<string>();

        if (string.IsNullOrEmpty(Nome))
            erros.Add("Nome é obrigatório");

        if (Preco <= 0)
            erros.Add("Preço deve ser maior que zero");

        if (Estoque < 0)
            erros.Add("Estoque não pode ser negativo");

        return erros;
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        var produto = new Produto
        {
            Nome = "Notebook",
            Preco = 2500,
            Estoque = 10
        };

        // Usando como ISalvar
        ISalvar salvavel = produto;
        salvavel.Salvar();

        // Usando como IValidar
        IValidar validavel = produto;
        if (validavel.EhValido())
        {
            Console.WriteLine("Produto válido!");
        }
        else
        {
            Console.WriteLine("Erros de validação:");
            foreach (var erro in validavel.ObterErros())
                Console.WriteLine($"- {erro}");
        }
    }
}
