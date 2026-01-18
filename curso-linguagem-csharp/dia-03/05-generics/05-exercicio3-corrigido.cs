// Interface para entidades
public interface IEntidade
{
    int Id { get; set; }
    string Nome { get; set; }
}

// Classe genérica para repositório
public class Repositorio<T> where T : class, IEntidade, new()
{
    private readonly List<T> _itens = new List<T>();
    private int _proximoId = 1;

    public void Adicionar(T item)
    {
        item.Id = _proximoId++;
        _itens.Add(item);
    }

    public T ObterPorId(int id)
    {
        return _itens.FirstOrDefault(i => i.Id == id);
    }

    public List<T> ObterTodos()
    {
        return new List<T>(_itens);
    }

    public void Atualizar(T item)
    {
        var existente = _itens.FirstOrDefault(i => i.Id == item.Id);
        if (existente != null)
        {
            _itens.Remove(existente);
            _itens.Add(item);
        }
    }

    public void Remover(int id)
    {
        var item = _itens.FirstOrDefault(i => i.Id == id);
        if (item != null)
            _itens.Remove(item);
    }
}

// Classes que implementam IEntidade
public class Produto : IEntidade
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}

public class Cliente : IEntidade
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

// Programa principal
class Program
{
    static void Main()
    {
        var repoProdutos = new Repositorio<Produto>();
        repoProdutos.Adicionar(new Produto { Nome = "Notebook", Preco = 2500 });
        repoProdutos.Adicionar(new Produto { Nome = "Mouse", Preco = 50 });

        var produto = repoProdutos.ObterPorId(1);
        Console.WriteLine($"Produto: {produto.Nome} - R$ {produto.Preco}");

        var repoClientes = new Repositorio<Cliente>();
        repoClientes.Adicionar(new Cliente { Nome = "João", Email = "joao@email.com" });

        var cliente = repoClientes.ObterPorId(1);
        Console.WriteLine($"Cliente: {cliente.Nome} - {cliente.Email}");
    }
}
