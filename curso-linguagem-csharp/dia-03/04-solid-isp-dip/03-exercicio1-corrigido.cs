// ✅ Interfaces segregadas (ISP - Interface Segregation Principle)
public interface ITrabalhador
{
    void Trabalhar();
    void Comer();
    void Dormir();
}

public interface IProgramador
{
    void Programar();
}

public interface IVendedor
{
    void Vender();
}

// Implementações específicas
public class Desenvolvedor : ITrabalhador, IProgramador
{
    public void Trabalhar() => Console.WriteLine("Desenvolvedor trabalhando");
    public void Comer() => Console.WriteLine("Comendo");
    public void Dormir() => Console.WriteLine("Dormindo");
    public void Programar() => Console.WriteLine("Programando código");
}

public class Vendedor : ITrabalhador, IVendedor
{
    public void Trabalhar() => Console.WriteLine("Vendedor trabalhando");
    public void Comer() => Console.WriteLine("Comendo");
    public void Dormir() => Console.WriteLine("Dormindo");
    public void Vender() => Console.WriteLine("Vendendo produtos");
}

public class Gerente : ITrabalhador
{
    public void Trabalhar() => Console.WriteLine("Gerenciando equipe");
    public void Comer() => Console.WriteLine("Comendo");
    public void Dormir() => Console.WriteLine("Dormindo");
}

// Programa principal
class Program
{
    static void Main()
    {
        var dev = new Desenvolvedor();
        dev.Trabalhar();
        dev.Programar();

        var vendedor = new Vendedor();
        vendedor.Trabalhar();
        vendedor.Vender();

        var gerente = new Gerente();
        gerente.Trabalhar();
    }
}
