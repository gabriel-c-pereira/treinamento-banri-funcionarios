// Classe abstrata
public abstract class Veiculo
{
    public string Modelo { get; set; }
    public int Velocidade { get; set; }

    // Método abstrato - deve ser implementado pelas subclasses
    public abstract void Mover();

    // Método concreto - compartilhado por todas as subclasses
    public void Acelerar(int incremento)
    {
        Velocidade += incremento;
        Console.WriteLine($"{Modelo} acelerou para {Velocidade} km/h");
    }
}

// Classe concreta Carro
public class Carro : Veiculo
{
    public override void Mover()
    {
        Console.WriteLine($"{Modelo} está dirigindo na estrada");
    }
}

// Classe concreta Moto
public class Moto : Veiculo
{
    public override void Mover()
    {
        Console.WriteLine($"{Modelo} está pilotando na pista");
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        var carro = new Carro { Modelo = "Civic" };
        carro.Acelerar(50);  // Método herdado
        carro.Mover();       // Método implementado

        var moto = new Moto { Modelo = "CB 300" };
        moto.Acelerar(30);
        moto.Mover();
    }
}
