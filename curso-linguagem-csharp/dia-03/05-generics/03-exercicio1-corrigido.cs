public class MinhaLista<T>
{
    private T[] _itens;
    private int _tamanho;

    public MinhaLista(int capacidade = 4)
    {
        _itens = new T[capacidade];
        _tamanho = 0;
    }

    public void Adicionar(T item)
    {
        if (_tamanho >= _itens.Length)
        {
            // Expandir array
            Array.Resize(ref _itens, _itens.Length * 2);
        }
        _itens[_tamanho] = item;
        _tamanho++;
    }

    public T Obter(int indice)
    {
        if (indice < 0 || indice >= _tamanho)
            throw new IndexOutOfRangeException();

        return _itens[indice];
    }

    public int Tamanho => _tamanho;

    public void Remover(int indice)
    {
        if (indice < 0 || indice >= _tamanho)
            return;

        // Deslocar elementos
        for (int i = indice; i < _tamanho - 1; i++)
        {
            _itens[i] = _itens[i + 1];
        }
        _tamanho--;
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        var listaInt = new MinhaLista<int>();
        listaInt.Adicionar(10);
        listaInt.Adicionar(20);
        Console.WriteLine($"Item 0: {listaInt.Obter(0)}"); // 10

        var listaString = new MinhaLista<string>();
        listaString.Adicionar("Olá");
        listaString.Adicionar("Mundo");
        Console.WriteLine($"Item 1: {listaString.Obter(1)}"); // Mundo
    }
}
