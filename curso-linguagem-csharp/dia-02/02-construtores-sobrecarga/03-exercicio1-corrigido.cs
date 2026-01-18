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