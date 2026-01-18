namespace CursoCSharp.Dia02.Referencias;

/// <summary>
/// EXERCÍCIO 4 - Records para Dados Imutáveis
/// 
/// Demonstra:
/// - Records (C# 9+)
/// - Imutabilidade
/// - Comparação por valor
/// - with expressions
/// - Deconstrução
/// </summary>

// =============================================
// VERSÃO 1: Record Básico
// =============================================
public record Pessoa(string Nome, string CPF, DateTime DataNascimento);

// =============================================
// VERSÃO 2: Record com Properties Calculadas
// =============================================
public record PessoaCompleta(string Nome, string CPF, DateTime DataNascimento)
{
    // Property calculada
    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;
            return idade;
        }
    }

    // Método para criar cópia com nome alterado
    public PessoaCompleta ComNome(string novoNome)
    {
        return this with { Nome = novoNome };
    }

    // Método para verificar maioridade
    public bool EhMaiorDeIdade() => Idade >= 18;

    // Categoria por idade
    public string Categoria => Idade switch
    {
        < 13 => "Criança",
        < 18 => "Adolescente",
        < 60 => "Adulto",
        _ => "Idoso"
    };
}

// =============================================
// VERSÃO 3: Record com Validação
// =============================================
public record PessoaValidada
{
    public string Nome { get; init; }
    public string CPF { get; init; }
    public DateTime DataNascimento { get; init; }

    public PessoaValidada(string nome, string cpf, DateTime dataNascimento)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            throw new ArgumentException("CPF inválido", nameof(cpf));

        if (dataNascimento > DateTime.Today)
            throw new ArgumentException("Data de nascimento não pode ser futura", nameof(dataNascimento));

        Nome = nome;
        CPF = cpf;
        DataNascimento = dataNascimento;
    }

    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;
            return idade;
        }
    }

    public PessoaValidada ComNome(string novoNome)
    {
        return new PessoaValidada(novoNome, CPF, DataNascimento);
    }

    public PessoaValidada ComIdade(int novaIdade)
    {
        var novaData = DateTime.Today.AddYears(-novaIdade);
        return new PessoaValidada(Nome, CPF, novaData);
    }
}

// =============================================
// VERSÃO 4: Record Class vs Record Struct
// =============================================

// Record Class (padrão) - Reference Type
public record class PessoaRecordClass(string Nome, int Idade);

// Record Struct (C# 10+) - Value Type
public record struct PessoaRecordStruct(string Nome, int Idade);

// =============================================
// EXEMPLOS AVANÇADOS DE RECORDS
// =============================================

// Record com herança
public record PessoaBase(string Nome, DateTime DataNascimento);
public record Funcionario(string Nome, DateTime DataNascimento, string Cargo, decimal Salario)
    : PessoaBase(Nome, DataNascimento);

// Record com propriedades adicionais
public record Endereco
{
    public string Rua { get; init; }
    public int Numero { get; init; }
    public string Cidade { get; init; }
    public string Estado { get; init; }
    public string CEP { get; init; }

    public Endereco(string rua, int numero, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Cidade = cidade;
        Estado = estado;
        CEP = cep;
    }

    // Override ToString para formatação customizada
    public override string ToString()
    {
        return $"{Rua}, {Numero} - {Cidade}/{Estado} - CEP: {CEP}";
    }
}

// Record complexo com outro record
public record Cliente
{
    public string Nome { get; init; }
    public string Email { get; init; }
    public Endereco Endereco { get; init; }
    public DateTime DataCadastro { get; init; }

    public Cliente(string nome, string email, Endereco endereco)
    {
        Nome = nome;
        Email = email;
        Endereco = endereco;
        DataCadastro = DateTime.Now;
    }

    public int AnosCadastrado => (DateTime.Now - DataCadastro).Days / 365;
}

// =============================================
// PROGRAMA DE TESTE
// =============================================
public class ProgramaRecords
{
    public static void Main()
    {
        Console.WriteLine("═══════════════════════════════════════");
        Console.WriteLine("           RECORDS EM C#");
        Console.WriteLine("═══════════════════════════════════════\n");

        TestarRecordBasico();
        Console.WriteLine();

        TestarComparacaoPorValor();
        Console.WriteLine();

        TestarWithExpressions();
        Console.WriteLine();

        TestarDesconstrucao();
        Console.WriteLine();

        TestarRecordCompleto();
        Console.WriteLine();

        TestarRecordValidado();
        Console.WriteLine();

        CompararRecordClassVsRecordStruct();
        Console.WriteLine();

        TestarHeranca();
        Console.WriteLine();

        TestarRecordComplexo();
    }

    static void TestarRecordBasico()
    {
        Console.WriteLine("=== RECORD BÁSICO ===\n");

        // Criação simples
        var pessoa1 = new Pessoa("João Silva", "12345678901", new DateTime(1990, 5, 15));

        // ToString automático (todos os valores)
        Console.WriteLine($"pessoa1: {pessoa1}");
        Console.WriteLine($"Nome: {pessoa1.Nome}");
        Console.WriteLine($"CPF: {pessoa1.CPF}");
        Console.WriteLine($"Data Nascimento: {pessoa1.DataNascimento:dd/MM/yyyy}");
    }

    static void TestarComparacaoPorValor()
    {
        Console.WriteLine("=== COMPARAÇÃO POR VALOR ===\n");

        var pessoa1 = new Pessoa("Maria Santos", "98765432109", new DateTime(1985, 10, 20));
        var pessoa2 = new Pessoa("Maria Santos", "98765432109", new DateTime(1985, 10, 20));
        var pessoa3 = new Pessoa("Pedro Oliveira", "11122233344", new DateTime(1995, 3, 8));

        Console.WriteLine($"pessoa1: {pessoa1}");
        Console.WriteLine($"pessoa2: {pessoa2}");
        Console.WriteLine($"pessoa3: {pessoa3}\n");

        // Comparação por valor (não por referência!)
        Console.WriteLine($"pessoa1 == pessoa2: {pessoa1 == pessoa2} ← Mesmos valores!");
        Console.WriteLine($"pessoa1 == pessoa3: {pessoa1 == pessoa3} ← Valores diferentes");
        Console.WriteLine($"ReferenceEquals(pessoa1, pessoa2): {ReferenceEquals(pessoa1, pessoa2)} ← Objetos diferentes\n");

        Console.WriteLine("💡 Records comparam por VALOR, não por referência!");
        Console.WriteLine("   Classes normais comparam por referência.");
    }

    static void TestarWithExpressions()
    {
        Console.WriteLine("=== with EXPRESSIONS ===\n");

        var pessoa1 = new Pessoa("Ana Costa", "55566677788", new DateTime(1992, 7, 12));
        Console.WriteLine($"Original: {pessoa1}\n");

        // Criar cópia modificando apenas o nome
        var pessoa2 = pessoa1 with { Nome = "Ana Costa Silva" };
        Console.WriteLine($"Com nome alterado: {pessoa2}");
        Console.WriteLine($"Original ainda: {pessoa1}\n");

        // Modificar múltiplas propriedades
        var pessoa3 = pessoa1 with
        {
            Nome = "Ana Beatriz Costa",
            DataNascimento = new DateTime(1993, 8, 20)
        };
        Console.WriteLine($"Múltiplas alterações: {pessoa3}\n");

        Console.WriteLine("💡 'with' cria uma NOVA instância (imutabilidade)");
        Console.WriteLine("   Original permanece inalterado.");
    }

    static void TestarDesconstrucao()
    {
        Console.WriteLine("=== DECONSTRUÇÃO ===\n");

        var pessoa = new Pessoa("Carlos Lima", "99988877766", new DateTime(1988, 12, 25));

        // Deconstruir em variáveis separadas
        var (nome, cpf, data) = pessoa;

        Console.WriteLine($"Pessoa completa: {pessoa}\n");
        Console.WriteLine("Deconstruída:");
        Console.WriteLine($"  Nome: {nome}");
        Console.WriteLine($"  CPF: {cpf}");
        Console.WriteLine($"  Data: {data:dd/MM/yyyy}\n");

        // Descartar valores com _
        var (nomeApenas, _, _) = pessoa;
        Console.WriteLine($"Só o nome: {nomeApenas}");
    }

    static void TestarRecordCompleto()
    {
        Console.WriteLine("=== RECORD COM PROPERTIES CALCULADAS ===\n");

        var pessoa = new PessoaCompleta(
            "Beatriz Alves",
            "44455566677",
            new DateTime(2000, 3, 15)
        );

        Console.WriteLine($"Nome: {pessoa.Nome}");
        Console.WriteLine($"Idade: {pessoa.Idade} anos");
        Console.WriteLine($"Categoria: {pessoa.Categoria}");
        Console.WriteLine($"Maior de idade: {(pessoa.EhMaiorDeIdade() ? "Sim" : "Não")}\n");

        // Usar método ComNome
        var pessoaCasada = pessoa.ComNome("Beatriz Alves Silva");
        Console.WriteLine($"Após casamento: {pessoaCasada.Nome}");
        Console.WriteLine($"Original: {pessoa.Nome} ← Não mudou!");
    }

    static void TestarRecordValidado()
    {
        Console.WriteLine("=== RECORD COM VALIDAÇÃO ===\n");

        try
        {
            var pessoa1 = new PessoaValidada(
                "Ricardo Souza",
                "33344455566",
                new DateTime(1995, 8, 10)
            );
            Console.WriteLine($"✅ Pessoa válida: {pessoa1.Nome}, Idade: {pessoa1.Idade}");
            Console.WriteLine();

            // Criar variação
            var pessoa2 = pessoa1.ComNome("Ricardo Souza Jr.");
            Console.WriteLine($"✅ Com novo nome: {pessoa2.Nome}");
            Console.WriteLine();

            var pessoa3 = pessoa1.ComIdade(30);
            Console.WriteLine($"✅ Com nova idade: {pessoa3.Idade} anos");
            Console.WriteLine();

            // Tentar criar pessoa inválida
            var pessoaInvalida = new PessoaValidada("", "123", DateTime.Today.AddDays(1));
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
        }
    }

    static void CompararRecordClassVsRecordStruct()
    {
        Console.WriteLine("=== RECORD CLASS vs RECORD STRUCT ===\n");

        // Record Class (reference type)
        var p1Class = new PessoaRecordClass("João", 30);
        var p2Class = p1Class; // Copia a referência
        Console.WriteLine($"Record Class - p1: {p1Class}");
        Console.WriteLine($"Record Class - p2: {p2Class}");
        Console.WriteLine($"São o mesmo objeto? {ReferenceEquals(p1Class, p2Class)}\n");

        // Record Struct (value type)
        var p1Struct = new PessoaRecordStruct("Maria", 25);
        var p2Struct = p1Struct; // Copia o valor
        Console.WriteLine($"Record Struct - p1: {p1Struct}");
        Console.WriteLine($"Record Struct - p2: {p2Struct}");
        Console.WriteLine($"São o mesmo objeto? {ReferenceEquals(p1Struct, p2Struct)}\n");

        Console.WriteLine("💡 Record Class:");
        Console.WriteLine("   • Reference type (padrão)");
        Console.WriteLine("   • Alocado no Heap");
        Console.WriteLine("   • Comparação por valor");
        Console.WriteLine("   • Ideal para DTOs\n");

        Console.WriteLine("💡 Record Struct:");
        Console.WriteLine("   • Value type");
        Console.WriteLine("   • Alocado no Stack");
        Console.WriteLine("   • Comparação por valor");
        Console.WriteLine("   • Ideal para dados pequenos e imutáveis");
    }

    static void TestarHeranca()
    {
        Console.WriteLine("=== HERANÇA COM RECORDS ===\n");

        var pessoa = new PessoaBase("Fernanda Lima", new DateTime(1992, 6, 18));
        var funcionario = new Funcionario(
            "Carlos Mendes",
            new DateTime(1988, 4, 22),
            "Desenvolvedor",
            8000
        );

        Console.WriteLine($"Pessoa: {pessoa}");
        Console.WriteLine($"Funcionário: {funcionario}\n");

        // with expressions funcionam com herança
        var funcionarioPromovido = funcionario with { Cargo = "Tech Lead", Salario = 12000 };
        Console.WriteLine($"Promovido: {funcionarioPromovido}");
    }

    static void TestarRecordComplexo()
    {
        Console.WriteLine("=== RECORD COMPLEXO ===\n");

        // Criar endereço
        var endereco = new Endereco(
            "Av. Paulista",
            1000,
            "São Paulo",
            "SP",
            "01310-100"
        );

        // Criar cliente com endereço
        var cliente = new Cliente(
            "Paula Rodrigues",
            "paula@email.com",
            endereco
        );

        Console.WriteLine($"Cliente: {cliente.Nome}");
        Console.WriteLine($"Email: {cliente.Email}");
        Console.WriteLine($"Endereço: {cliente.Endereco}");
        Console.WriteLine($"Anos cadastrado: {cliente.AnosCadastrado}\n");

        // Alterar endereço (with aninhado)
        var clienteMudou = cliente with
        {
            Endereco = endereco with { Numero = 2000 }
        };

        Console.WriteLine("Após mudança:");
        Console.WriteLine($"Cliente: {clienteMudou.Nome}");
        Console.WriteLine($"Novo endereço: {clienteMudou.Endereco}\n");
        Console.WriteLine($"Original: {cliente.Endereco} ← Não mudou!");
    }
}

// =============================================
// COMPARAÇÃO: Class vs Record
// =============================================
public class ComparacaoClassVsRecord
{
    // Class tradicional
    public class PessoaClass
    {
        public string Nome { get; set; }
        public int Idade { get; set; }

        // Precisa implementar manualmente
        public override bool Equals(object obj)
        {
            if (obj is not PessoaClass other) return false;
            return Nome == other.Nome && Idade == other.Idade;
        }

        public override int GetHashCode() => HashCode.Combine(Nome, Idade);
        public override string ToString() => $"PessoaClass {{ Nome = {Nome}, Idade = {Idade} }}";
    }

    // Record - tudo automático!
    public record PessoaRecord(string Nome, int Idade);

    public static void Comparar()
    {
        Console.WriteLine("═══ CLASS vs RECORD ═══\n");

        Console.WriteLine("Class:");
        Console.WriteLine("  • Precisa implementar Equals, GetHashCode, ToString");
        Console.WriteLine("  • Mutável por padrão");
        Console.WriteLine("  • Comparação por referência\n");

        Console.WriteLine("Record:");
        Console.WriteLine("  • Equals, GetHashCode, ToString automáticos ✅");
        Console.WriteLine("  • Imutável por padrão (init) ✅");
        Console.WriteLine("  • Comparação por valor ✅");
        Console.WriteLine("  • with expressions ✅");
        Console.WriteLine("  • Deconstrução automática ✅");
        Console.WriteLine("  • Sintaxe concisa ✅");
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * ✅ Records (C# 9+)
 *    - Sintaxe concisa para DTOs
 *    - Imutabilidade por padrão (init)
 *    - Comparação por valor automática
 *    - ToString, Equals, GetHashCode automáticos
 * 
 * ✅ with Expressions
 *    - Criar cópias modificadas
 *    - Preserva imutabilidade
 *    - Sintaxe elegante
 * 
 * ✅ Deconstrução
 *    - Extrair valores facilmente
 *    - var (a, b, c) = record
 *    - Descartar com _
 * 
 * ✅ Properties Calculadas
 *    - Idade baseada em data de nascimento
 *    - Categorização dinâmica
 * 
 * ✅ Validação
 *    - Possível em construtores
 *    - Métodos para criar variações
 * 
 * ✅ Record Class vs Record Struct
 *    - Reference type vs Value type
 *    - Quando usar cada um
 * 
 * ✅ Herança
 *    - Records podem herdar de outros records
 *    - with funciona com herança
 * 
 * ✅ Records Complexos
 *    - Records dentro de records
 *    - with aninhado
 * 
 * 💡 QUANDO USAR RECORDS:
 *    • DTOs (Data Transfer Objects)
 *    • Value Objects
 *    • Dados imutáveis
 *    • Comparação por valor necessária
 *    • APIs e serialização
 */