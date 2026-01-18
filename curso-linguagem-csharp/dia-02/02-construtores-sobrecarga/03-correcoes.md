# 📝 Correções dos Exercícios

## 🎯 Exercício 1

```csharp
namespace CursoCSharp.Dia02.Construtores;

/// <summary>
/// EXERCÍCIO 1 - Livro com Múltiplos Construtores
/// 
/// Demonstra:
/// - Múltiplos construtores
/// - Constructor chaining com :this()
/// - Valores padrão
/// - Validação em construtores
/// </summary>

// ============================================
// VERSÃO 1: Básica com múltiplos construtores
// ============================================
public class Livro
{
    // Properties
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoPublicacao { get; set; }
    public int NumeroPaginas { get; set; }

    // Construtor 1: Completo
    public Livro(string titulo, string autor, int anoPublicacao, int numeroPaginas)
    {
        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
        NumeroPaginas = numeroPaginas;
    }

    // Construtor 2: Sem número de páginas (chama o completo)
    public Livro(string titulo, string autor, int anoPublicacao)
        : this(titulo, autor, anoPublicacao, 0) // Constructor chaining
    {
        // Lógica adicional se necessário
    }

    // Construtor 3: Apenas título e autor (chama o anterior)
    public Livro(string titulo, string autor)
        : this(titulo, autor, DateTime.Now.Year) // Ano atual como padrão
    {
    }

    // Construtor 4: Padrão (chama outro construtor)
    public Livro()
        : this("Título Desconhecido", "Autor Desconhecido")
    {
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"📖 {Titulo}");
        Console.WriteLine($"   Autor: {Autor}");
        Console.WriteLine($"   Ano: {AnoPublicacao}");
        Console.WriteLine($"   Páginas: {(NumeroPaginas > 0 ? NumeroPaginas : "Não informado")}");
    }
}

// =============================================
// VERSÃO 2: Com validação nos construtores
// =============================================
public class LivroValidado
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int AnoPublicacao { get; set; }
    public int NumeroPaginas { get; set; }

    // Construtor principal com validação
    public LivroValidado(string titulo, string autor, int anoPublicacao, int numeroPaginas)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título não pode ser vazio", nameof(titulo));

        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("Autor não pode ser vazio", nameof(autor));

        if (anoPublicacao < 0 || anoPublicacao > DateTime.Now.Year)
            throw new ArgumentException($"Ano deve estar entre 0 e {DateTime.Now.Year}", nameof(anoPublicacao));

        if (numeroPaginas < 0)
            throw new ArgumentException("Número de páginas não pode ser negativo", nameof(numeroPaginas));

        Titulo = titulo;
        Autor = autor;
        AnoPublicacao = anoPublicacao;
        NumeroPaginas = numeroPaginas;
    }

    // Outros construtores chamam o principal (herdam a validação)
    public LivroValidado(string titulo, string autor, int anoPublicacao)
        : this(titulo, autor, anoPublicacao, 0)
    {
    }

    public LivroValidado(string titulo, string autor)
        : this(titulo, autor, DateTime.Now.Year)
    {
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"📖 {Titulo}");
        Console.WriteLine($"   Autor: {Autor}");
        Console.WriteLine($"   Ano: {AnoPublicacao}");
        Console.WriteLine($"   Páginas: {(NumeroPaginas > 0 ? NumeroPaginas.ToString() : "Não informado")}");
    }
}

// =============================================
// VERSÃO 3: Moderna com init e records
// =============================================
public class LivroModerno
{
    // Properties com init (imutáveis após construção)
    public string Titulo { get; init; }
    public string Autor { get; init; }
    public int AnoPublicacao { get; init; }
    public int NumeroPaginas { get; init; }

    // Property calculada
    public int IdadeAnos => DateTime.Now.Year - AnoPublicacao;
    public bool EhClassico => IdadeAnos > 50;

    // Construtor completo
    public LivroModerno(string titulo, string autor, int anoPublicacao, int numeroPaginas)
    {
        Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
        Autor = autor ?? throw new ArgumentNullException(nameof(autor));
        AnoPublicacao = anoPublicacao;
        NumeroPaginas = numeroPaginas;

        // Validar após atribuição
        ValidarDados();
    }

    // Construtores com chaining
    public LivroModerno(string titulo, string autor, int anoPublicacao)
        : this(titulo, autor, anoPublicacao, 0)
    {
    }

    public LivroModerno(string titulo, string autor)
        : this(titulo, autor, DateTime.Now.Year)
    {
    }

    private void ValidarDados()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            throw new ArgumentException("Título inválido");

        if (string.IsNullOrWhiteSpace(Autor))
            throw new ArgumentException("Autor inválido");

        if (AnoPublicacao < 0 || AnoPublicacao > DateTime.Now.Year)
            throw new ArgumentException("Ano inválido");

        if (NumeroPaginas < 0)
            throw new ArgumentException("Número de páginas inválido");
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"📖 {Titulo}");
        Console.WriteLine($"   Autor: {Autor}");
        Console.WriteLine($"   Ano: {AnoPublicacao} ({IdadeAnos} anos)");
        Console.WriteLine($"   Páginas: {(NumeroPaginas > 0 ? NumeroPaginas : "Não informado")}");
        Console.WriteLine($"   Clássico: {(EhClassico ? "Sim ⭐" : "Não")}");
    }
}

// =============================================
// VERSÃO 4: Record (C# 9+) - Ainda mais concisa
// =============================================
public record LivroRecord(
    string Titulo,
    string Autor,
    int AnoPublicacao,
    int NumeroPaginas = 0) // Valor padrão
{
    // Validação no construtor do record
    public LivroRecord(string titulo, string autor, int anoPublicacao, int numeroPaginas = 0)
        : this(titulo, autor, anoPublicacao, numeroPaginas)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título inválido", nameof(titulo));

        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("Autor inválido", nameof(autor));
    }

    // Properties calculadas
    public int IdadeAnos => DateTime.Now.Year - AnoPublicacao;
    public bool EhClassico => IdadeAnos > 50;

    // Construtores adicionais
    public LivroRecord(string titulo, string autor)
        : this(titulo, autor, DateTime.Now.Year, 0)
    {
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"📖 {Titulo}");
        Console.WriteLine($"   Autor: {Autor}");
        Console.WriteLine($"   Ano: {AnoPublicacao} ({IdadeAnos} anos)");
        Console.WriteLine($"   Páginas: {(NumeroPaginas > 0 ? NumeroPaginas : "Não informado")}");
        Console.WriteLine($"   Clássico: {(EhClassico ? "Sim ⭐" : "Não")}");
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaLivro
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
        // Usando diferentes construtores
        var livro1 = new Livro("1984", "George Orwell", 1949, 328);
        var livro2 = new Livro("O Senhor dos Anéis", "J.R.R. Tolkien", 1954);
        var livro3 = new Livro("Clean Code", "Robert C. Martin");
        var livro4 = new Livro(); // Construtor padrão

        livro1.ExibirInformacoes();
        Console.WriteLine();
        livro2.ExibirInformacoes();
        Console.WriteLine();
        livro3.ExibirInformacoes();
        Console.WriteLine();
        livro4.ExibirInformacoes();
    }

    static void TestarVersaoValidada()
    {
        try
        {
            // Criação válida
            var livro1 = new LivroValidado("Dom Casmurro", "Machado de Assis", 1899, 256);
            livro1.ExibirInformacoes();
            Console.WriteLine();

            // Tentativa inválida (vai lançar exceção)
            var livro2 = new LivroValidado("", "Autor", 2000);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }
    }

    static void TestarVersaoModerna()
    {
        // Usando construtores
        var livro1 = new LivroModerno("O Cortiço", "Aluísio Azevedo", 1890, 280);
        livro1.ExibirInformacoes();
        Console.WriteLine();

        // Usando object initializer (combinado com construtor)
        var livro2 = new LivroModerno("Harry Potter", "J.K. Rowling", 1997)
        {
            // init properties podem ser definidas aqui se necessário
        };
        livro2.ExibirInformacoes();
    }

    static void TestarVersaoRecord()
    {
        // Record com construtor completo
        var livro1 = new LivroRecord("Cem Anos de Solidão", "Gabriel García Márquez", 1967, 417);
        livro1.ExibirInformacoes();
        Console.WriteLine();

        // Record com construtor simplificado
        var livro2 = new LivroRecord("Clean Architecture", "Robert C. Martin");
        livro2.ExibirInformacoes();
        Console.WriteLine();

        // Usando with (copia e modifica)
        var livro3 = livro1 with { AnoPublicacao = 2000 };
        Console.WriteLine("Cópia modificada:");
        livro3.ExibirInformacoes();
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Constructor Chaining
 *    - Usar :this() para chamar outro construtor
 *    - Evita duplicação de código
 *    - Validação centralizada
 * 
 * ✅ Múltiplos Construtores
 *    - Diferentes níveis de inicialização
 *    - Valores padrão progressivos
 * 
 * ✅ Validação em Construtores
 *    - Garantir estado válido desde a criação
 *    - Lançar exceções para dados inválidos
 * 
 * ✅ Evolution of Approaches
 *    - Básica: Funcional mas sem validação
 *    - Validada: Adiciona segurança
 *    - Moderna: Usa init e properties calculadas
 *    - Record: Ainda mais concisa e imutável
 * 
 * ✅ Boas Práticas
 *    - Constructor chaining evita duplicação
 *    - Validar no construtor principal
 *    - Usar init para imutabilidade quando possível
 *    - Records para DTOs e dados imutáveis
 */
```

---

## 🎯 Exercício 5

```csharp
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
```

---

## 🎯 Exercício 7

```csharp
namespace CursoCSharp.Dia02.Construtores;

/// <summary>
/// EXERCÍCIO 7 - Círculo com Constantes e Sobrecarga
/// 
/// Demonstra:
/// - Constantes (const e readonly)
/// - Method overloading
/// - Sobrecarga de operações
/// - Properties calculadas
/// </summary>

// =============================================
// VERSÃO 1: Implementação básica
// =============================================
public class Circulo
{
    // Constante PI (valor não pode mudar)
    public const double PI = 3.14159265359;

    // Property
    public double Raio { get; set; }

    // Construtor
    public Circulo(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        Raio = raio;
    }

    // Métodos sobrecarregados para cálculo de área

    // 1. Área do círculo atual
    public double CalcularArea()
    {
        return PI * Raio * Raio;
    }

    // 2. Área de um círculo com raio específico (método estático)
    public static double CalcularArea(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        return PI * raio * raio;
    }

    // 3. Área de múltiplos círculos
    public static double CalcularArea(params double[] raios)
    {
        double areaTotal = 0;
        foreach (var raio in raios)
        {
            if (raio <= 0)
                throw new ArgumentException("Todos os raios devem ser positivos");

            areaTotal += CalcularArea(raio);
        }
        return areaTotal;
    }

    // Métodos sobrecarregados para perímetro

    public double CalcularPerimetro()
    {
        return 2 * PI * Raio;
    }

    public static double CalcularPerimetro(double raio)
    {
        return 2 * PI * raio;
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"⭕ Círculo");
        Console.WriteLine($"   Raio: {Raio:F2}");
        Console.WriteLine($"   Área: {CalcularArea():F2}");
        Console.WriteLine($"   Perímetro: {CalcularPerimetro():F2}");
    }
}

// =============================================
// VERSÃO 2: Com mais sobrecarga e funcionalidades
// =============================================
public class CirculoAvancado
{
    // Constante estática
    public const double PI = Math.PI;

    // Readonly (definido uma vez, no construtor)
    public readonly string Unidade;

    // Properties
    public double Raio { get; set; }
    public string Cor { get; set; }

    // Properties calculadas
    public double Diametro => Raio * 2;
    public double Area => CalcularArea();
    public double Perimetro => CalcularPerimetro();

    // Construtores sobrecarregados
    public CirculoAvancado(double raio, string unidade = "cm", string cor = "Preto")
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(raio));

        Raio = raio;
        Unidade = unidade; // readonly só pode ser definido aqui
        Cor = cor;
    }

    // Métodos de instância

    public double CalcularArea()
    {
        return PI * Raio * Raio;
    }

    public double CalcularPerimetro()
    {
        return 2 * PI * Raio;
    }

    // Redimensionar - versões sobrecarregadas

    // 1. Por fator multiplicativo
    public void Redimensionar(double fator)
    {
        if (fator <= 0)
            throw new ArgumentException("Fator deve ser positivo", nameof(fator));

        Raio *= fator;
        Console.WriteLine($"✅ Círculo redimensionado por fator {fator:F2}");
    }

    // 2. Para um novo raio absoluto
    public void Redimensionar(double novoRaio, bool absoluto)
    {
        if (!absoluto)
        {
            // Se não é absoluto, trata como fator
            Redimensionar(novoRaio);
            return;
        }

        if (novoRaio <= 0)
            throw new ArgumentException("Raio deve ser positivo", nameof(novoRaio));

        Raio = novoRaio;
        Console.WriteLine($"✅ Círculo redimensionado para raio {novoRaio:F2} {Unidade}");
    }

    // 3. Redimensionar para área específica
    public void RedimensionarParaArea(double areaDesejada)
    {
        if (areaDesejada <= 0)
            throw new ArgumentException("Área deve ser positiva", nameof(areaDesejada));

        // Calcular novo raio: raio = sqrt(area / PI)
        double novoRaio = Math.Sqrt(areaDesejada / PI);
        Raio = novoRaio;
        Console.WriteLine($"✅ Círculo redimensionado para área {areaDesejada:F2} {Unidade}²");
    }

    // Métodos estáticos sobrecarregados

    // Comparar áreas
    public static double CompararAreas(CirculoAvancado c1, CirculoAvancado c2)
    {
        return c1.Area - c2.Area;
    }

    // Criar círculo a partir de área
    public static CirculoAvancado CriarPorArea(double area, string unidade = "cm")
    {
        double raio = Math.Sqrt(area / PI);
        return new CirculoAvancado(raio, unidade);
    }

    // Criar círculo a partir de perímetro
    public static CirculoAvancado CriarPorPerimetro(double perimetro, string unidade = "cm")
    {
        double raio = perimetro / (2 * PI);
        return new CirculoAvancado(raio, unidade);
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"⭕ Círculo {Cor}");
        Console.WriteLine($"   Raio: {Raio:F2} {Unidade}");
        Console.WriteLine($"   Diâmetro: {Diametro:F2} {Unidade}");
        Console.WriteLine($"   Área: {Area:F2} {Unidade}²");
        Console.WriteLine($"   Perímetro: {Perimetro:F2} {Unidade}");
    }
}

// =============================================
// VERSÃO 3: Com operadores sobrecarregados
// =============================================
public class CirculoComOperadores
{
    public const double PI = Math.PI;
    public double Raio { get; init; }

    public CirculoComOperadores(double raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo");
        Raio = raio;
    }

    // Properties calculadas
    public double Area => PI * Raio * Raio;
    public double Perimetro => 2 * PI * Raio;

    // Sobrecarga de operadores

    // Operador + (soma de áreas, retorna novo círculo)
    public static CirculoComOperadores operator +(CirculoComOperadores c1, CirculoComOperadores c2)
    {
        double areaTotal = c1.Area + c2.Area;
        double novoRaio = Math.Sqrt(areaTotal / PI);
        return new CirculoComOperadores(novoRaio);
    }

    // Operador - (diferença de áreas)
    public static CirculoComOperadores operator -(CirculoComOperadores c1, CirculoComOperadores c2)
    {
        double areaDiferenca = Math.Abs(c1.Area - c2.Area);
        double novoRaio = Math.Sqrt(areaDiferenca / PI);
        return new CirculoComOperadores(novoRaio);
    }

    // Operador * (multiplica raio por escalar)
    public static CirculoComOperadores operator *(CirculoComOperadores c, double escalar)
    {
        return new CirculoComOperadores(c.Raio * escalar);
    }

    // Operador / (divide raio por escalar)
    public static CirculoComOperadores operator /(CirculoComOperadores c, double escalar)
    {
        if (escalar == 0)
            throw new DivideByZeroException("Não é possível dividir por zero");
        return new CirculoComOperadores(c.Raio / escalar);
    }

    // Operadores de comparação
    public static bool operator >(CirculoComOperadores c1, CirculoComOperadores c2)
        => c1.Area > c2.Area;

    public static bool operator <(CirculoComOperadores c1, CirculoComOperadores c2)
        => c1.Area < c2.Area;

    public static bool operator ==(CirculoComOperadores c1, CirculoComOperadores c2)
        => Math.Abs(c1.Area - c2.Area) < 0.0001; // Comparação com tolerância

    public static bool operator !=(CirculoComOperadores c1, CirculoComOperadores c2)
        => !(c1 == c2);

    public override bool Equals(object obj)
        => obj is CirculoComOperadores c && this == c;

    public override int GetHashCode()
        => Raio.GetHashCode();

    public override string ToString()
        => $"Círculo (R={Raio:F2}, A={Area:F2})";
}

// =============================================
// VERSÃO 4: Record com métodos sobrecarregados
// =============================================
public record CirculoRecord(double Raio)
{
    public const double PI = Math.PI;

    // Validação no construtor
    public CirculoRecord(double raio) : this(raio)
    {
        if (raio <= 0)
            throw new ArgumentException("Raio deve ser positivo");
    }

    // Properties calculadas
    public double Area => PI * Raio * Raio;
    public double Perimetro => 2 * PI * Raio;
    public double Diametro => Raio * 2;

    // Métodos sobrecarregados

    // Escalar por fator
    public CirculoRecord Escalar(double fator)
    {
        return this with { Raio = Raio * fator };
    }

    // Escalar para área específica
    public CirculoRecord EscalarParaArea(double areaDesejada)
    {
        double novoRaio = Math.Sqrt(areaDesejada / PI);
        return this with { Raio = novoRaio };
    }

    // Escalar para perímetro específico
    public CirculoRecord EscalarParaPerimetro(double perimetroDesejado)
    {
        double novoRaio = perimetroDesejado / (2 * PI);
        return this with { Raio = novoRaio };
    }

    // Factory methods sobrecarregados
    public static CirculoRecord Criar(double raio) => new(raio);
    public static CirculoRecord CriarPorArea(double area) => new(Math.Sqrt(area / PI));
    public static CirculoRecord CriarPorPerimetro(double perimetro) => new(perimetro / (2 * PI));
    public static CirculoRecord CriarPorDiametro(double diametro) => new(diametro / 2);
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaCirculo
{
    public static void Main()
    {
        Console.WriteLine("=== VERSÃO 1: BÁSICA ===\n");
        TestarVersaoBasica();

        Console.WriteLine("\n=== VERSÃO 2: AVANÇADA ===\n");
        TestarVersaoAvancada();

        Console.WriteLine("\n=== VERSÃO 3: COM OPERADORES ===\n");
        TestarVersaoComOperadores();

        Console.WriteLine("\n=== VERSÃO 4: RECORD ===\n");
        TestarVersaoRecord();
    }

    static void TestarVersaoBasica()
    {
        // Criar círculo
        var circulo = new Circulo(5);
        circulo.ExibirInformacoes();
        Console.WriteLine();

        // Método estático - área de um círculo com raio 10
        double area = Circulo.CalcularArea(10);
        Console.WriteLine($"Área de círculo com raio 10: {area:F2}");

        // Método estático - área total de múltiplos círculos
        double areaTotal = Circulo.CalcularArea(5, 10, 15);
        Console.WriteLine($"Área total de 3 círculos: {areaTotal:F2}");
    }

    static void TestarVersaoAvancada()
    {
        // Criar círculos
        var c1 = new CirculoAvancado(5, "cm", "Vermelho");
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar por fator
        c1.Redimensionar(2); // Dobra o raio
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar para raio absoluto
        c1.Redimensionar(10, true);
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Redimensionar para área específica
        c1.RedimensionarParaArea(100);
        c1.ExibirInformacoes();
        Console.WriteLine();

        // Factory methods
        var c2 = CirculoAvancado.CriarPorArea(50, "m");
        Console.WriteLine("Círculo criado por área:");
        c2.ExibirInformacoes();
        Console.WriteLine();

        var c3 = CirculoAvancado.CriarPorPerimetro(31.4159, "km");
        Console.WriteLine("Círculo criado por perímetro:");
        c3.ExibirInformacoes();
    }

    static void TestarVersaoComOperadores()
    {
        var c1 = new CirculoComOperadores(5);
        var c2 = new CirculoComOperadores(10);

        Console.WriteLine($"C1: {c1}");
        Console.WriteLine($"C2: {c2}");
        Console.WriteLine();

        // Operações
        var c3 = c1 + c2; // Soma de áreas
        Console.WriteLine($"C1 + C2 = {c3}");

        var c4 = c2 - c1; // Diferença de áreas
        Console.WriteLine($"C2 - C1 = {c4}");

        var c5 = c1 * 2; // Dobra o raio
        Console.WriteLine($"C1 * 2 = {c5}");

        var c6 = c2 / 2; // Divide o raio
        Console.WriteLine($"C2 / 2 = {c6}");
        Console.WriteLine();

        // Comparações
        Console.WriteLine($"C1 > C2: {c1 > c2}");
        Console.WriteLine($"C1 < C2: {c1 < c2}");
        Console.WriteLine($"C1 == C2: {c1 == c2}");
    }

    static void TestarVersaoRecord()
    {
        // Criar por diferentes métodos
        var c1 = CirculoRecord.Criar(5);
        var c2 = CirculoRecord.CriarPorArea(78.54);
        var c3 = CirculoRecord.CriarPorPerimetro(31.4159);
        var c4 = CirculoRecord.CriarPorDiametro(20);

        Console.WriteLine($"C1: {c1} - Área: {c1.Area:F2}");
        Console.WriteLine($"C2: {c2} - Área: {c2.Area:F2}");
        Console.WriteLine($"C3: {c3} - Perímetro: {c3.Perimetro:F2}");
        Console.WriteLine($"C4: {c4} - Diâmetro: {c4.Diametro:F2}");
        Console.WriteLine();

        // Escalar
        var c5 = c1.Escalar(2);
        Console.WriteLine($"C1 escalado 2x: {c5} - Área: {c5.Area:F2}");

        var c6 = c1.EscalarParaArea(100);
        Console.WriteLine($"C1 para área 100: {c6} - Área: {c6.Area:F2}");

        // Imutabilidade com with
        var c7 = c1 with { Raio = 15 };
        Console.WriteLine($"\nOriginal C1: {c1}");
        Console.WriteLine($"Modificado C7: {c7}");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Constantes
 *    - const PI: Valor fixo em tempo de compilação
 *    - readonly Unidade: Valor fixo após construção
 * 
 * ✅ Method Overloading
 *    - CalcularArea(): 3 versões diferentes
 *    - Redimensionar(): 3 versões diferentes
 *    - Factory methods: 4 formas de criar
 * 
 * ✅ Operator Overloading (Versão 3)
 *    - Aritméticos: +, -, *, /
 *    - Comparação: >, <, ==, !=
 *    - Permite sintaxe natural: c1 + c2
 * 
 * ✅ Properties Calculadas
 *    - Area, Perimetro, Diametro
 *    - Sempre atualizadas com o raio
 * 
 * ✅ Factory Methods (Versão 2 e 4)
 *    - CriarPorArea, CriarPorPerimetro, CriarPorDiametro
 *    - Nomes descritivos da intenção
 * 
 * ✅ Imutabilidade (Versão 4)
 *    - Record com init
 *    - Métodos retornam novos círculos
 *    - with expressions
 * 
 * ✅ Boas Práticas
 *    - Validação em construtores
 *    - Métodos estáticos para operações sem estado
 *    - Sobrecarga para flexibilidade
 *    - Nomes descritivos
 */
```

---

## 🎯 Exercício 10

```csharp
namespace CursoCSharp.Dia02.Construtores;

/// <summary>
/// EXERCÍCIO 10 - Sistema de Reservas (PROJETO FINAL)
/// 
/// Sistema completo de hotel demonstrando:
/// - Múltiplos construtores com chaining
/// - Optional parameters e named arguments
/// - Method overloading
/// - Validação em construtores
/// - Primary constructors (C# 12)
/// - Properties calculadas
/// - Integração entre classes
/// </summary>

// =============================================
// CLASSE 1: Cliente
// =============================================
public class Cliente
{
    // Properties
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public DateTime DataCadastro { get; init; }
    public TipoCliente Tipo { get; set; }

    // Property calculada
    public int AnosCadastrado => (DateTime.Now - DataCadastro).Days / 365;
    public bool EhClienteVIP => Tipo == TipoCliente.VIP || AnosCadastrado >= 5;

    // Construtor completo
    public Cliente(string nome, string cpf, string email, string telefone, TipoCliente tipo = TipoCliente.Regular)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            throw new ArgumentException("CPF inválido", nameof(cpf));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Email inválido", nameof(email));

        Nome = nome;
        CPF = cpf;
        Email = email;
        Telefone = telefone ?? "Não informado";
        DataCadastro = DateTime.Now;
        Tipo = tipo;

        Console.WriteLine($"✅ Cliente {Nome} cadastrado como {Tipo}");
    }

    // Construtor simplificado (sem telefone)
    public Cliente(string nome, string cpf, string email)
        : this(nome, cpf, email, null)
    {
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"👤 {Nome} {(EhClienteVIP ? "⭐ VIP" : "")}");
        Console.WriteLine($"   CPF: {CPF}");
        Console.WriteLine($"   Email: {Email}");
        Console.WriteLine($"   Telefone: {Telefone}");
        Console.WriteLine($"   Tipo: {Tipo}");
        Console.WriteLine($"   Cadastrado há: {AnosCadastrado} anos");
    }

    public decimal ObterDesconto()
    {
        return Tipo switch
        {
            TipoCliente.Regular => 0m,
            TipoCliente.Frequente => 0.10m,  // 10%
            TipoCliente.VIP => 0.20m,        // 20%
            _ => 0m
        };
    }
}

public enum TipoCliente
{
    Regular,
    Frequente,
    VIP
}

// =============================================
// CLASSE 2: QuartoHotel
// =============================================
public class QuartoHotel
{
    // Properties
    public int Numero { get; init; }
    public TipoQuarto Tipo { get; init; }
    public decimal PrecoDiaria { get; set; }
    public int CapacidadeMaxima { get; init; }
    public bool TemVistaMar { get; init; }
    public bool TemVaranda { get; init; }
    public List<string> Comodidades { get; init; }

    // Property calculada
    public bool EstaDisponivel { get; set; } = true;
    public string Descricao => $"Quarto {Numero} - {Tipo} (até {CapacidadeMaxima} pessoas)";

    // Construtor completo
    public QuartoHotel(
        int numero,
        TipoQuarto tipo,
        decimal precoDiaria,
        int capacidadeMaxima,
        bool temVistaMar = false,
        bool temVaranda = false)
    {
        if (numero <= 0)
            throw new ArgumentException("Número do quarto deve ser positivo", nameof(numero));

        if (precoDiaria <= 0)
            throw new ArgumentException("Preço deve ser positivo", nameof(precoDiaria));

        if (capacidadeMaxima <= 0)
            throw new ArgumentException("Capacidade deve ser positiva", nameof(capacidadeMaxima));

        Numero = numero;
        Tipo = tipo;
        PrecoDiaria = precoDiaria;
        CapacidadeMaxima = capacidadeMaxima;
        TemVistaMar = temVistaMar;
        TemVaranda = temVaranda;
        Comodidades = new List<string>();

        // Comodidades básicas por tipo
        AdicionarComodidadesBasicas();
    }

    // Construtor simplificado (valores padrão baseados no tipo)
    public QuartoHotel(int numero, TipoQuarto tipo)
        : this(
            numero,
            tipo,
            ObterPrecoPadrao(tipo),
            ObterCapacidadePadrao(tipo))
    {
    }

    // Métodos privados auxiliares
    private static decimal ObterPrecoPadrao(TipoQuarto tipo)
    {
        return tipo switch
        {
            TipoQuarto.Standard => 150m,
            TipoQuarto.Luxo => 300m,
            TipoQuarto.Suite => 500m,
            TipoQuarto.PenthouseSuite => 1000m,
            _ => 150m
        };
    }

    private static int ObterCapacidadePadrao(TipoQuarto tipo)
    {
        return tipo switch
        {
            TipoQuarto.Standard => 2,
            TipoQuarto.Luxo => 3,
            TipoQuarto.Suite => 4,
            TipoQuarto.PenthouseSuite => 6,
            _ => 2
        };
    }

    private void AdicionarComodidadesBasicas()
    {
        // Comodidades básicas para todos
        Comodidades.Add("Wi-Fi");
        Comodidades.Add("TV");
        Comodidades.Add("Ar Condicionado");

        // Comodidades adicionais por tipo
        switch (Tipo)
        {
            case TipoQuarto.Luxo:
                Comodidades.Add("Frigobar");
                Comodidades.Add("Cofre");
                break;
            case TipoQuarto.Suite:
                Comodidades.Add("Frigobar");
                Comodidades.Add("Cofre");
                Comodidades.Add("Banheira de Hidromassagem");
                break;
            case TipoQuarto.PenthouseSuite:
                Comodidades.Add("Frigobar Premium");
                Comodidades.Add("Cofre");
                Comodidades.Add("Banheira de Hidromassagem");
                Comodidades.Add("Sala de Estar");
                Comodidades.Add("Cozinha");
                break;
        }
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"🏨 {Descricao}");
        Console.WriteLine($"   Preço: {PrecoDiaria:C}/noite");
        Console.WriteLine($"   Capacidade: {CapacidadeMaxima} pessoas");
        Console.WriteLine($"   Vista para o mar: {(TemVistaMar ? "Sim 🌊" : "Não")}");
        Console.WriteLine($"   Varanda: {(TemVaranda ? "Sim" : "Não")}");
        Console.WriteLine($"   Status: {(EstaDisponivel ? "Disponível ✅" : "Ocupado ❌")}");
        Console.WriteLine($"   Comodidades: {string.Join(", ", Comodidades)}");
    }

    public decimal CalcularValorEstadia(int numeroNoites, decimal desconto = 0)
    {
        decimal valorBase = PrecoDiaria * numeroNoites;
        decimal valorComDesconto = valorBase * (1 - desconto);
        return valorComDesconto;
    }
}

public enum TipoQuarto
{
    Standard,
    Luxo,
    Suite,
    PenthouseSuite
}

// =============================================
// CLASSE 3: Reserva
// =============================================
public class Reserva
{
    private static int _proximoId = 1;

    // Properties
    public int Id { get; init; }
    public Cliente Cliente { get; init; }
    public QuartoHotel Quarto { get; init; }
    public DateTime DataCheckIn { get; set; }
    public DateTime DataCheckOut { get; set; }
    public int NumeroHospedes { get; set; }
    public StatusReserva Status { get; set; }
    public DateTime DataReserva { get; init; }
    public string Observacoes { get; set; }

    // Properties calculadas
    public int NumeroNoites => (DataCheckOut - DataCheckIn).Days;
    public decimal ValorTotal => CalcularValorTotal();
    public bool EstaAtiva => Status == StatusReserva.Confirmada || Status == StatusReserva.CheckIn;

    // Construtor completo
    public Reserva(
        Cliente cliente,
        QuartoHotel quarto,
        DateTime dataCheckIn,
        DateTime dataCheckOut,
        int numeroHospedes,
        string observacoes = "")
    {
        // Validações
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        if (quarto == null)
            throw new ArgumentNullException(nameof(quarto));

        if (dataCheckIn < DateTime.Now.Date)
            throw new ArgumentException("Data de check-in não pode ser no passado", nameof(dataCheckIn));

        if (dataCheckOut <= dataCheckIn)
            throw new ArgumentException("Data de check-out deve ser posterior ao check-in", nameof(dataCheckOut));

        if (numeroHospedes <= 0 || numeroHospedes > quarto.CapacidadeMaxima)
            throw new ArgumentException($"Número de hóspedes deve estar entre 1 e {quarto.CapacidadeMaxima}", nameof(numeroHospedes));

        if (!quarto.EstaDisponivel)
            throw new InvalidOperationException("Quarto não está disponível");

        Id = _proximoId++;
        Cliente = cliente;
        Quarto = quarto;
        DataCheckIn = dataCheckIn;
        DataCheckOut = dataCheckOut;
        NumeroHospedes = numeroHospedes;
        DataReserva = DateTime.Now;
        Status = StatusReserva.Pendente;
        Observacoes = observacoes ?? "";

        // Marcar quarto como ocupado
        quarto.EstaDisponivel = false;

        Console.WriteLine($"✅ Reserva #{Id} criada para {cliente.Nome}");
    }

    // Construtor simplificado (sem observações)
    public Reserva(Cliente cliente, QuartoHotel quarto, DateTime dataCheckIn, DateTime dataCheckOut, int numeroHospedes)
        : this(cliente, quarto, dataCheckIn, dataCheckOut, numeroHospedes, "")
    {
    }

    // Construtor com duração em noites (sobrecarga)
    public Reserva(Cliente cliente, QuartoHotel quarto, DateTime dataCheckIn, int numeroNoites, int numeroHospedes, string observacoes = "")
        : this(cliente, quarto, dataCheckIn, dataCheckIn.AddDays(numeroNoites), numeroHospedes, observacoes)
    {
        Console.WriteLine($"📅 Reserva de {numeroNoites} noite(s)");
    }

    private decimal CalcularValorTotal()
    {
        decimal valorBase = Quarto.PrecoDiaria * NumeroNoites;

        // Aplicar desconto do cliente
        decimal desconto = Cliente.ObterDesconto();
        decimal valorComDesconto = valorBase * (1 - desconto);

        // Taxa de serviço (10%)
        decimal taxaServico = valorComDesconto * 0.10m;

        return valorComDesconto + taxaServico;
    }

    // Métodos para gerenciar o ciclo de vida da reserva

    public void Confirmar()
    {
        if (Status != StatusReserva.Pendente)
            throw new InvalidOperationException("Apenas reservas pendentes podem ser confirmadas");

        Status = StatusReserva.Confirmada;
        Console.WriteLine($"✅ Reserva #{Id} confirmada!");
    }

    public void FazerCheckIn()
    {
        if (Status != StatusReserva.Confirmada)
            throw new InvalidOperationException("Reserva deve estar confirmada para check-in");

        if (DateTime.Now.Date < DataCheckIn.Date)
            throw new InvalidOperationException("Check-in só pode ser feito a partir da data reservada");

        Status = StatusReserva.CheckIn;
        Console.WriteLine($"🔑 Check-in realizado para reserva #{Id}");
    }

    public void FazerCheckOut()
    {
        if (Status != StatusReserva.CheckIn)
            throw new InvalidOperationException("Check-out só pode ser feito após check-in");

        Status = StatusReserva.CheckOut;
        Quarto.EstaDisponivel = true; // Liberar quarto
        Console.WriteLine($"👋 Check-out realizado para reserva #{Id}");
    }

    public void Cancelar()
    {
        if (Status == StatusReserva.CheckOut)
            throw new InvalidOperationException("Não é possível cancelar reserva já finalizada");

        Status = StatusReserva.Cancelada;
        Quarto.EstaDisponivel = true; // Liberar quarto
        Console.WriteLine($"❌ Reserva #{Id} cancelada");
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"📋 RESERVA #{Id} - {Status}");
        Console.WriteLine($"   Cliente: {Cliente.Nome} {(Cliente.EhClienteVIP ? "⭐" : "")}");
        Console.WriteLine($"   Quarto: {Quarto.Numero} ({Quarto.Tipo})");
        Console.WriteLine($"   Check-in: {DataCheckIn:dd/MM/yyyy}");
        Console.WriteLine($"   Check-out: {DataCheckOut:dd/MM/yyyy}");
        Console.WriteLine($"   Noites: {NumeroNoites}");
        Console.WriteLine($"   Hóspedes: {NumeroHospedes}");
        Console.WriteLine($"   Valor Total: {ValorTotal:C}");
        if (!string.IsNullOrWhiteSpace(Observacoes))
            Console.WriteLine($"   Obs: {Observacoes}");
    }

    public void ExibirResumo()
    {
        Console.WriteLine($"#{Id} | {Cliente.Nome} | Quarto {Quarto.Numero} | {DataCheckIn:dd/MM} - {DataCheckOut:dd/MM} | {ValorTotal:C} | {Status}");
    }
}

public enum StatusReserva
{
    Pendente,
    Confirmada,
    CheckIn,
    CheckOut,
    Cancelada
}

// =============================================
// CLASSE 4: GerenciadorReservas
// =============================================
public class GerenciadorReservas
{
    private List<Reserva> _reservas = new();
    private List<QuartoHotel> _quartos = new();
    private List<Cliente> _clientes = new();

    public GerenciadorReservas()
    {
        Console.WriteLine("🏨 Sistema de Reservas iniciado\n");
    }

    // Cadastros

    public void CadastrarCliente(Cliente cliente)
    {
        _clientes.Add(cliente);
    }

    public void CadastrarQuarto(QuartoHotel quarto)
    {
        _quartos.Add(quarto);
    }

    // Criar reserva - versões sobrecarregadas

    public Reserva CriarReserva(Cliente cliente, QuartoHotel quarto, DateTime checkIn, DateTime checkOut, int hospedes, string obs = "")
    {
        var reserva = new Reserva(cliente, quarto, checkIn, checkOut, hospedes, obs);
        _reservas.Add(reserva);
        return reserva;
    }

    public Reserva CriarReserva(Cliente cliente, QuartoHotel quarto, DateTime checkIn, int numeroNoites, int hospedes, string obs = "")
    {
        var reserva = new Reserva(cliente, quarto, checkIn, numeroNoites, hospedes, obs);
        _reservas.Add(reserva);
        return reserva;
    }

    // Consultas

    public List<QuartoHotel> ListarQuartosDisponiveis()
    {
        return _quartos.Where(q => q.EstaDisponivel).ToList();
    }

    public List<QuartoHotel> ListarQuartosDisponiveis(TipoQuarto tipo)
    {
        return _quartos.Where(q => q.EstaDisponivel && q.Tipo == tipo).ToList();
    }

    public List<Reserva> ListarReservasAtivas()
    {
        return _reservas.Where(r => r.EstaAtiva).ToList();
    }

    public List<Reserva> ListarReservasCliente(Cliente cliente)
    {
        return _reservas.Where(r => r.Cliente == cliente).ToList();
    }

    // Relatórios

    public void ExibirResumoGeral()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("        RESUMO DO SISTEMA");
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine($"Total de Clientes: {_clientes.Count}");
        Console.WriteLine($"Total de Quartos: {_quartos.Count}");
        Console.WriteLine($"Quartos Disponíveis: {_quartos.Count(q => q.EstaDisponivel)}");
        Console.WriteLine($"Total de Reservas: {_reservas.Count}");
        Console.WriteLine($"Reservas Ativas: {_reservas.Count(r => r.EstaAtiva)}");
        Console.WriteLine($"Receita Total: {_reservas.Where(r => r.Status == StatusReserva.CheckOut).Sum(r => r.ValorTotal):C}");
        Console.WriteLine("═══════════════════════════════════════\n");
    }

    public void ExibirTodasReservas()
    {
        Console.WriteLine("═══ TODAS AS RESERVAS ═══");
        foreach (var reserva in _reservas.OrderBy(r => r.DataCheckIn))
        {
            reserva.ExibirResumo();
        }
        Console.WriteLine();
    }
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaSistemaReservas
{
    public static void Main()
    {
        var gerenciador = new GerenciadorReservas();

        // Cadastrar quartos
        Console.WriteLine("═══ CADASTRANDO QUARTOS ═══\n");
        var q101 = new QuartoHotel(101, TipoQuarto.Standard);
        var q201 = new QuartoHotel(201, TipoQuarto.Luxo, 350, 3, temVistaMar: true);
        var q301 = new QuartoHotel(301, TipoQuarto.Suite, 550, 4, temVistaMar: true, temVaranda: true);
        var q401 = new QuartoHotel(401, TipoQuarto.PenthouseSuite);

        gerenciador.CadastrarQuarto(q101);
        gerenciador.CadastrarQuarto(q201);
        gerenciador.CadastrarQuarto(q301);
        gerenciador.CadastrarQuarto(q401);

        Console.WriteLine("\n═══ INFORMAÇÕES DOS QUARTOS ═══\n");
        q101.ExibirInformacoes();
        Console.WriteLine();
        q301.ExibirInformacoes();
        Console.WriteLine();

        // Cadastrar clientes
        Console.WriteLine("═══ CADASTRANDO CLIENTES ═══\n");
        var cliente1 = new Cliente("João Silva", "12345678901", "joao@email.com", "11999999999");
        var cliente2 = new Cliente("Maria Santos", "98765432109", "maria@email.com", tipo: TipoCliente.VIP);
        var cliente3 = new Cliente("Pedro Oliveira", "11122233344", "pedro@email.com");

        gerenciador.CadastrarCliente(cliente1);
        gerenciador.CadastrarCliente(cliente2);
        gerenciador.CadastrarCliente(cliente3);
        Console.WriteLine();

        // Criar reservas
        Console.WriteLine("═══ CRIANDO RESERVAS ═══\n");

        // Reserva 1: Usando datas completas
        var reserva1 = gerenciador.CriarReserva(
            cliente1,
            q101,
            DateTime.Now.AddDays(7),
            DateTime.Now.AddDays(10),
            2,
            "Chegada tarde"
        );
        reserva1.Confirmar();

        // Reserva 2: Usando número de noites
        var reserva2 = gerenciador.CriarReserva(
            cliente2,
            q301,
            DateTime.Now.AddDays(5),
            3, // 3 noites
            2,
            "Cliente VIP - preparar welcome gift"
        );
        reserva2.Confirmar();

        // Reserva 3: Reserva simples
        var reserva3 = gerenciador.CriarReserva(
            cliente3,
            q201,
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            1
        );

        Console.WriteLine("\n═══ DETALHES DAS RESERVAS ═══\n");
        reserva1.ExibirInformacoes();
        Console.WriteLine();
        reserva2.ExibirInformacoes();
        Console.WriteLine();

        // Simular ciclo de vida
        Console.WriteLine("═══ SIMULANDO CICLO DE VIDA ═══\n");
        try
        {
            reserva3.Confirmar();
            Console.WriteLine();

            // Simular check-in (em produção, seria na data correta)
            // reserva3.FazerCheckIn();
            // reserva3.FazerCheckOut();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"⚠️  Erro: {ex.Message}\n");
        }

        // Relatórios
        gerenciador.ExibirResumoGeral();
        gerenciador.ExibirTodasReservas();

        // Listar quartos disponíveis
        Console.WriteLine("═══ QUARTOS DISPONÍVEIS ═══");
        var disponiveis = gerenciador.ListarQuartosDisponiveis();
        foreach (var quarto in disponiveis)
        {
            Console.WriteLine($"- Quarto {quarto.Numero} ({quarto.Tipo}) - {quarto.PrecoDiaria:C}/noite");
        }
    }
}

/*
 * CONCEITOS DEMONSTRADOS NO PROJETO FINAL:
 * 
 * ✅ Constructor Chaining
 *    - Cliente: 2 construtores encadeados
 *    - QuartoHotel: Construtores com valores padrão
 *    - Reserva: 3 construtores diferentes
 * 
 * ✅ Optional Parameters
 *    - tipo, observacoes, temVistaMar, temVaranda
 *    - Valores padrão inteligentes
 * 
 * ✅ Named Arguments
 *    - Demonstrado nas criações: temVistaMar: true
 *    - Melhora legibilidade
 * 
 * ✅ Method Overloading
 *    - CriarReserva: 2 versões (data completa vs número de noites)
 *    - ListarQuartosDisponiveis: 2 versões (todos vs por tipo)
 * 
 * ✅ Validation
 *    - Todos os construtores validam parâmetros
 *    - Lançam exceções específicas
 *    - Mensagens descritivas
 * 
 * ✅ Properties Calculadas
 *    - NumeroNoites, ValorTotal, EstaAtiva
 *    - AnosCadastrado, EhClienteVIP
 *    - Sempre atualizadas
 * 
 * ✅ Enums
 *    - TipoCliente, TipoQuarto, StatusReserva
 *    - Type-safe e descritivo
 * 
 * ✅ Business Logic
 *    - Cálculo de descontos por tipo de cliente
 *    - Ciclo de vida da reserva
 *    - Gestão de disponibilidade
 * 
 * ✅ SOLID Principles (Preview)
 *    - Single Responsibility: Cada classe tem um propósito
 *    - Dependency Inversion: Gerenciador depende de abstrações
 * 
 * ✅ Real-World Application
 *    - Sistema completo e funcional
 *    - Integração entre múltiplas classes
 *    - Validações de negócio
 *    - Relatórios e consultas
 * 
 * 🎯 Este exercício integra TODOS os conceitos do Dia 02:
 *    - Classes e Objetos (Dia 02.1)
 *    - Construtores e Sobrecarga (Dia 02.2)
 *    - Preview de conceitos futuros (Herança, Interfaces)
 */
```

---

