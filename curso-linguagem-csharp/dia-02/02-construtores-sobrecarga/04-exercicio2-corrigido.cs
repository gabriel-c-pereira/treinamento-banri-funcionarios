namespace CursoCSharp.Dia02.Construtores;

/// <summary>
/// EXERCÍCIO 5 - Funcionário com Constructor Chaining
/// 
/// Demonstra:
/// - Constructor chaining completo (3 níveis)
/// - Validação progressiva
/// - Valores padrão inteligentes
/// - Properties calculadas
/// </summary>

// =============================================
// VERSÃO 1: Implementação básica do enunciado
// =============================================
public class Funcionario
{
    // Properties
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; }

    // Construtor 1: Completo (recebe todos os parâmetros)
    public Funcionario(string nome, string cargo, decimal salario, DateTime dataAdmissao)
    {
        Nome = nome;
        Cargo = cargo;
        Salario = salario;
        DataAdmissao = dataAdmissao;
    }

    // Construtor 2: Sem data de admissão (usa data atual)
    public Funcionario(string nome, string cargo, decimal salario)
        : this(nome, cargo, salario, DateTime.Now) // Chama o construtor completo
    {
        Console.WriteLine($"✅ Funcionário criado com data de admissão: {DateTime.Now:dd/MM/yyyy}");
    }

    // Construtor 3: Sem salário e sem data (usa valores padrão)
    public Funcionario(string nome, string cargo)
        : this(nome, cargo, 0) // Chama o construtor anterior
    {
        Console.WriteLine("⚠️  Salário não informado, necessário definir posteriormente");
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"👤 {Nome}");
        Console.WriteLine($"   Cargo: {Cargo}");
        Console.WriteLine($"   Salário: {Salario:C}");
        Console.WriteLine($"   Admissão: {DataAdmissao:dd/MM/yyyy}");
    }
}

// =============================================
// VERSÃO 2: Com validação e lógica adicional
// =============================================
public class FuncionarioValidado
{
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; }
    public string Departamento { get; set; }

    // Property calculada
    public int AnosEmpresa => (DateTime.Now - DataAdmissao).Days / 365;
    public bool EhVeterano => AnosEmpresa >= 5;

    // Construtor completo com validação
    public FuncionarioValidado(string nome, string cargo, decimal salario, DateTime dataAdmissao, string departamento = "Geral")
    {
        // Validações
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(cargo))
            throw new ArgumentException("Cargo não pode ser vazio", nameof(cargo));

        if (salario < 0)
            throw new ArgumentException("Salário não pode ser negativo", nameof(salario));

        if (dataAdmissao > DateTime.Now)
            throw new ArgumentException("Data de admissão não pode ser no futuro", nameof(dataAdmissao));

        Nome = nome;
        Cargo = cargo;
        Salario = salario;
        DataAdmissao = dataAdmissao;
        Departamento = departamento;

        // Lógica adicional
        Console.WriteLine($"✅ Funcionário {Nome} cadastrado no departamento {Departamento}");
    }

    // Construtor sem data (usa hoje)
    public FuncionarioValidado(string nome, string cargo, decimal salario, string departamento = "Geral")
        : this(nome, cargo, salario, DateTime.Now, departamento)
    {
        Console.WriteLine($"📅 Data de admissão definida como: {DateTime.Now:dd/MM/yyyy}");
    }

    // Construtor mínimo (salário zerado)
    public FuncionarioValidado(string nome, string cargo)
        : this(nome, cargo, 0)
    {
        Console.WriteLine("⚠️  Salário zerado - necessário atualizar!");
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"👤 {Nome} {(EhVeterano ? "⭐ (Veterano)" : "")}");
        Console.WriteLine($"   Cargo: {Cargo}");
        Console.WriteLine($"   Departamento: {Departamento}");
        Console.WriteLine($"   Salário: {Salario:C}");
        Console.WriteLine($"   Admissão: {DataAdmissao:dd/MM/yyyy} ({AnosEmpresa} anos)");
    }

    public decimal CalcularBonus()
    {
        // Bônus baseado em tempo de empresa
        return AnosEmpresa switch
        {
            < 1 => Salario * 0.05m,      // 5%
            < 3 => Salario * 0.10m,      // 10%
            < 5 => Salario * 0.15m,      // 15%
            _ => Salario * 0.20m         // 20%
        };
    }
}

// =============================================
// VERSÃO 3: Moderna com init e factory methods
// =============================================
public class FuncionarioModerno
{
    public string Nome { get; init; }
    public string Cargo { get; init; }
    public decimal Salario { get; init; }
    public DateTime DataAdmissao { get; init; }
    public string Departamento { get; init; }

    // Properties calculadas
    public int AnosEmpresa => (DateTime.Now - DataAdmissao).Days / 365;
    public int MesesEmpresa => (DateTime.Now - DataAdmissao).Days / 30;
    public bool EhVeterano => AnosEmpresa >= 5;
    public decimal SalarioAnual => Salario * 12;

    // Construtor principal (privado)
    private FuncionarioModerno(string nome, string cargo, decimal salario, DateTime dataAdmissao, string departamento)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Cargo = cargo ?? throw new ArgumentNullException(nameof(cargo));
        Salario = salario >= 0 ? salario : throw new ArgumentException("Salário inválido");
        DataAdmissao = dataAdmissao <= DateTime.Now ? dataAdmissao : throw new ArgumentException("Data inválida");
        Departamento = departamento ?? "Geral";
    }

    // Factory Methods (padrão de criação recomendado)
    public static FuncionarioModerno Criar(string nome, string cargo, decimal salario, DateTime dataAdmissao, string departamento = "Geral")
    {
        return new FuncionarioModerno(nome, cargo, salario, dataAdmissao, departamento);
    }

    public static FuncionarioModerno CriarHoje(string nome, string cargo, decimal salario, string departamento = "Geral")
    {
        return new FuncionarioModerno(nome, cargo, salario, DateTime.Now, departamento);
    }

    public static FuncionarioModerno CriarSemSalario(string nome, string cargo, string departamento = "Geral")
    {
        Console.WriteLine("⚠️  Funcionário criado sem salário definido");
        return new FuncionarioModerno(nome, cargo, 0, DateTime.Now, departamento);
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"👤 {Nome} {(EhVeterano ? "⭐ (Veterano)" : "")}");
        Console.WriteLine($"   Cargo: {Cargo}");
        Console.WriteLine($"   Departamento: {Departamento}");
        Console.WriteLine($"   Salário: {Salario:C} (Anual: {SalarioAnual:C})");
        Console.WriteLine($"   Admissão: {DataAdmissao:dd/MM/yyyy}");
        Console.WriteLine($"   Tempo: {AnosEmpresa} anos e {MesesEmpresa % 12} meses");
    }

    public decimal CalcularBonus() => AnosEmpresa switch
    {
        < 1 => Salario * 0.05m,
        < 3 => Salario * 0.10m,
        < 5 => Salario * 0.15m,
        _ => Salario * 0.20m
    };

    public decimal CalcularFeriasProporcionais()
    {
        // 1/12 por mês trabalhado
        var mesesTrabalhados = Math.Min(MesesEmpresa, 12);
        return (Salario / 12) * mesesTrabalhados;
    }
}

// =============================================
// VERSÃO 4: Record com validação
// =============================================
public record FuncionarioRecord
{
    public string Nome { get; init; }
    public string Cargo { get; init; }
    public decimal Salario { get; init; }
    public DateTime DataAdmissao { get; init; }
    public string Departamento { get; init; }

    // Construtor principal
    public FuncionarioRecord(string nome, string cargo, decimal salario, DateTime dataAdmissao, string departamento = "Geral")
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        Cargo = cargo ?? throw new ArgumentNullException(nameof(cargo));
        Salario = salario >= 0 ? salario : throw new ArgumentException("Salário inválido");
        DataAdmissao = dataAdmissao <= DateTime.Now ? dataAdmissao : throw new ArgumentException("Data inválida");
        Departamento = departamento;
    }

    // Construtor sem data
    public FuncionarioRecord(string nome, string cargo, decimal salario, string departamento = "Geral")
        : this(nome, cargo, salario, DateTime.Now, departamento)
    {
    }

    // Construtor mínimo
    public FuncionarioRecord(string nome, string cargo)
        : this(nome, cargo, 0, DateTime.Now, "Geral")
    {
    }

    // Properties calculadas
    public int AnosEmpresa => (DateTime.Now - DataAdmissao).Days / 365;
    public bool EhVeterano => AnosEmpresa >= 5;

    public void ExibirInformacoes()
    {
        Console.WriteLine($"👤 {Nome} {(EhVeterano ? "⭐" : "")}");
        Console.WriteLine($"   {Cargo} - {Departamento}");
        Console.WriteLine($"   {Salario:C} | {AnosEmpresa} anos");
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaFuncionario
{
    public static void Main()
    {
        Console.WriteLine("=== VERSÃO 1: BÁSICA ===\n");
        TestarVersaoBasica();

        Console.WriteLine("\n=== VERSÃO 2: VALIDADA ===\n");
        TestarVersaoValidada();

        Console.WriteLine("\n=== VERSÃO 3: MODERNA ===\n");
        TestarVersaoModerna();

        Console.WriteLine("\n=== VERSÃO 4: RECORD ===\n");
        TestarVersaoRecord();
    }

    static void TestarVersaoBasica()
    {
        // Usando os 3 construtores
        var func1 = new Funcionario("João Silva", "Desenvolvedor", 5000, new DateTime(2020, 3, 15));
        var func2 = new Funcionario("Maria Santos", "Analista", 4500);
        var func3 = new Funcionario("Pedro Oliveira", "Estagiário");

        func1.ExibirInformacoes();
        Console.WriteLine();
        func2.ExibirInformacoes();
        Console.WriteLine();
        func3.ExibirInformacoes();
    }

    static void TestarVersaoValidada()
    {
        var func1 = new FuncionarioValidado(
            "Ana Costa",
            "Gerente",
            8000,
            new DateTime(2018, 6, 1),
            "TI"
        );
        func1.ExibirInformacoes();
        Console.WriteLine($"   Bônus: {func1.CalcularBonus():C}");
        Console.WriteLine();

        var func2 = new FuncionarioValidado("Carlos Lima", "Desenvolvedor", 6000, "TI");
        func2.ExibirInformacoes();
        Console.WriteLine($"   Bônus: {func2.CalcularBonus():C}");
    }

    static void TestarVersaoModerna()
    {
        // Factory methods
        var func1 = FuncionarioModerno.Criar(
            "Beatriz Alves",
            "Arquiteta",
            10000,
            new DateTime(2019, 1, 10),
            "Arquitetura"
        );
        func1.ExibirInformacoes();
        Console.WriteLine($"   Bônus: {func1.CalcularBonus():C}");
        Console.WriteLine($"   Férias Proporcionais: {func1.CalcularFeriasProporcionais():C}");
        Console.WriteLine();

        var func2 = FuncionarioModerno.CriarHoje("Ricardo Souza", "DevOps", 7000, "Infraestrutura");
        func2.ExibirInformacoes();
    }

    static void TestarVersaoRecord()
    {
        var func1 = new FuncionarioRecord("Fernanda Lima", "Tech Lead", 12000, new DateTime(2017, 5, 20), "Engenharia");
        func1.ExibirInformacoes();
        Console.WriteLine();

        // Usando with para criar variação
        var func2 = func1 with { Nome = "Fernanda Lima Jr.", Cargo = "Senior Developer", Salario = 9000 };
        func2.ExibirInformacoes();
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Constructor Chaining (3 níveis)
 *    - Construtor completo ← Construtor médio ← Construtor mínimo
 *    - Cada um adiciona defaults progressivos
 *    - Validação centralizada no construtor principal
 * 
 * ✅ Validação Progressiva
 *    - Versão 1: Sem validação
 *    - Versão 2: Validação completa
 *    - Versão 3: Validação + factory methods
 *    - Versão 4: Record com validação
 * 
 * ✅ Properties Calculadas
 *    - AnosEmpresa, MesesEmpresa
 *    - EhVeterano (bool)
 *    - SalarioAnual
 * 
 * ✅ Factory Methods (Versão 3)
 *    - Alternativa mais expressiva aos construtores
 *    - Criar(), CriarHoje(), CriarSemSalario()
 *    - Nomes descritivos da intenção
 * 
 * ✅ Evolution of Approaches
 *    - Básica: Constructor chaining simples
 *    - Validada: Adiciona segurança e lógica
 *    - Moderna: Factory methods + init
 *    - Record: Imutabilidade + with expressions
 * 
 * ✅ Boas Práticas
 *    - Validar no construtor principal
 *    - Usar chaining para evitar duplicação
 *    - Properties calculadas quando possível
 *    - Factory methods para clareza
 */