# 📝 Correções dos Exercícios

## 🎯 Exercício 2

```csharp
namespace CursoCSharp.Dia03.Heranca;

/// <summary>
/// EXERCÍCIO 2 - Sistema de Funcionários com Herança
/// 
/// Demonstra:
/// - Herança básica
/// - Uso de base para construtor
/// - Override de métodos
/// - Polimorfismo básico
/// </summary>

// =============================================
// CLASSE BASE: Funcionario
// =============================================
public class Funcionario
{
    public string Nome { get; set; }
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; }

    public Funcionario(string nome, decimal salario)
    {
        Nome = nome;
        Salario = salario;
        DataAdmissao = DateTime.Now;
    }

    // Virtual: permite override nas derivadas
    public virtual decimal CalcularBonus()
    {
        return Salario * 0.10m; // 10%
    }

    public virtual void ExibirInformacoes()
    {
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Salário: {Salario:C}");
        Console.WriteLine($"Admissão: {DataAdmissao:dd/MM/yyyy}");
        Console.WriteLine($"Bônus: {CalcularBonus():C}");
    }
}

// =============================================
// CLASSE DERIVADA: Gerente
// =============================================
public class Gerente : Funcionario
{
    public int NumeroSubordinados { get; set; }

    // Chamar construtor da base com :base()
    public Gerente(string nome, decimal salario, int subordinados)
        : base(nome, salario)
    {
        NumeroSubordinados = subordinados;
    }

    // Override: 20% + R$ 1000 fixo
    public override decimal CalcularBonus()
    {
        return (Salario * 0.20m) + 1000m;
    }

    public override void ExibirInformacoes()
    {
        base.ExibirInformacoes(); // Chama método da base
        Console.WriteLine($"Cargo: Gerente");
        Console.WriteLine($"Subordinados: {NumeroSubordinados}");
    }
}

// =============================================
// CLASSE DERIVADA: Desenvolvedor
// =============================================
public class Desenvolvedor : Funcionario
{
    public string Linguagem { get; set; }
    public int HorasExtras { get; set; }

    public Desenvolvedor(string nome, decimal salario, string linguagem)
        : base(nome, salario)
    {
        Linguagem = linguagem;
    }

    // Override: 15% + (HorasExtras * 50)
    public override decimal CalcularBonus()
    {
        decimal bonusBase = Salario * 0.15m;
        decimal bonusHoras = HorasExtras * 50m;
        return bonusBase + bonusHoras;
    }

    public override void ExibirInformacoes()
    {
        base.ExibirInformacoes();
        Console.WriteLine($"Cargo: Desenvolvedor");
        Console.WriteLine($"Linguagem: {Linguagem}");
        Console.WriteLine($"Horas Extras: {HorasExtras}");
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaFuncionarios
{
    public static void Main()
    {
        Console.WriteLine("═══ SISTEMA DE FUNCIONÁRIOS ═══\n");

        // Criar funcionários de diferentes tipos
        var funcionarios = new List<Funcionario>
        {
            new Funcionario("João Silva", 3000),
            new Gerente("Maria Santos", 8000, 10),
            new Desenvolvedor("Pedro Oliveira", 6000, "C#") { HorasExtras = 20 },
            new Gerente("Ana Costa", 10000, 15),
            new Desenvolvedor("Carlos Lima", 7000, "Python") { HorasExtras = 10 }
        };

        // Polimorfismo: cada um chama sua própria versão
        foreach (var func in funcionarios)
        {
            func.ExibirInformacoes();
            Console.WriteLine();
        }

        // Calcular folha de pagamento
        decimal totalSalarios = funcionarios.Sum(f => f.Salario);
        decimal totalBonus = funcionarios.Sum(f => f.CalcularBonus());
        decimal totalFolha = totalSalarios + totalBonus;

        Console.WriteLine("═══ FOLHA DE PAGAMENTO ═══");
        Console.WriteLine($"Total Salários: {totalSalarios:C}");
        Console.WriteLine($"Total Bônus: {totalBonus:C}");
        Console.WriteLine($"Total Geral: {totalFolha:C}");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Herança básica
 *    - Gerente : Funcionario
 *    - Desenvolvedor : Funcionario
 * 
 * ✅ Uso de base
 *    - :base(nome, salario) no construtor
 *    - base.ExibirInformacoes() no método
 * 
 * ✅ Override de métodos
 *    - CalcularBonus() customizado por tipo
 *    - ExibirInformacoes() estendido
 * 
 * ✅ Polimorfismo
 *    - List<Funcionario> contém diferentes tipos
 *    - Cada um executa sua própria versão do método
 * 
 * ✅ Virtual/Override
 *    - virtual na base permite override
 *    - override nas derivadas substitui comportamento
 */
```

---

