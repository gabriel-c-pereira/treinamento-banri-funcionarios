// Exercício 1 Corrigido: Classe Pessoa Simples
// Arquivo: Program.cs

using System;

// ═══════════════════════════════════════════════════════════
// VERSÃO 1: Básica
// ═══════════════════════════════════════════════════════════

public class Pessoa
{
    // Auto-properties
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    
    // Método para apresentação
    public void ApresentarSe()
    {
        Console.WriteLine($"Olá, meu nome é {Nome} e tenho {Idade} anos.");
    }
    
    // Método para verificar maioridade
    public bool EhMaiorDeIdade()
    {
        return Idade >= 18;
    }
}

// ═══════════════════════════════════════════════════════════
// VERSÃO 2: Com Validação
// ═══════════════════════════════════════════════════════════

public class PessoaValidada
{
    private string _nome;
    private int _idade;
    private string _email;
    
    public string Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nome não pode ser vazio");
            _nome = value.Trim();
        }
    }
    
    public int Idade
    {
        get => _idade;
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Idade inválida");
            _idade = value;
        }
    }
    
    public string Email
    {
        get => _email;
        set
        {
            if (!value.Contains("@"))
                throw new ArgumentException("Email inválido");
            _email = value.Trim().ToLower();
        }
    }
    
    public void ApresentarSe()
    {
        string maioridade = EhMaiorDeIdade() ? "sou maior de idade" : "sou menor de idade";
        Console.WriteLine($"Olá, meu nome é {Nome} e tenho {Idade} anos. Eu {maioridade}.");
    }
    
    public bool EhMaiorDeIdade() => Idade >= 18;
}

// ═══════════════════════════════════════════════════════════
// VERSÃO 3: Com Expression-Bodied Members (C# 7+)
// ═══════════════════════════════════════════════════════════

public class PessoaModerna
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    
    // Properties calculadas
    public bool EhMaiorDeIdade() => Idade >= 18;
    public string Categoria => Idade switch
    {
        < 12 => "Criança",
        < 18 => "Adolescente",
        < 60 => "Adulto",
        _ => "Idoso"
    };
    
    public void ApresentarSe() =>
        Console.WriteLine($"Olá, meu nome é {Nome}, tenho {Idade} anos e sou {Categoria}.");
}

// ═══════════════════════════════════════════════════════════
// TESTE
// ═══════════════════════════════════════════════════════════

class Program
{
    static void Main()
    {
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("  TESTE: CLASSE PESSOA BÁSICA     ");
        Console.WriteLine("═══════════════════════════════════\n");
        
        // Criar 3 pessoas
        var pessoa1 = new Pessoa
        {
            Nome = "João Silva",
            Idade = 25,
            Email = "joao@email.com"
        };
        
        var pessoa2 = new Pessoa
        {
            Nome = "Maria Santos",
            Idade = 17,
            Email = "maria@email.com"
        };
        
        var pessoa3 = new Pessoa
        {
            Nome = "Pedro Costa",
            Idade = 65,
            Email = "pedro@email.com"
        };
        
        // Testar métodos
        pessoa1.ApresentarSe();
        Console.WriteLine($"Maior de idade? {pessoa1.EhMaiorDeIdade()}\n");
        
        pessoa2.ApresentarSe();
        Console.WriteLine($"Maior de idade? {pessoa2.EhMaiorDeIdade()}\n");
        
        pessoa3.ApresentarSe();
        Console.WriteLine($"Maior de idade? {pessoa3.EhMaiorDeIdade()}\n");
        
        // Teste com validação
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("  TESTE: COM VALIDAÇÃO            ");
        Console.WriteLine("═══════════════════════════════════\n");
        
        var pessoaV = new PessoaValidada
        {
            Nome = "  Ana Paula  ",  // Será limpo
            Idade = 30,
            Email = "ANA@EMAIL.COM"  // Será convertido para minúsculo
        };
        
        pessoaV.ApresentarSe();
        Console.WriteLine($"Email normalizado: {pessoaV.Email}\n");
        
        // Teste com versão moderna
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("  TESTE: VERSÃO MODERNA           ");
        Console.WriteLine("═══════════════════════════════════\n");
        
        var pessoaM = new PessoaModerna
        {
            Nome = "Carlos Eduardo",
            Idade = 45,
            Email = "carlos@email.com"
        };
        
        pessoaM.ApresentarSe();
        Console.WriteLine($"Categoria: {pessoaM.Categoria}");
    }
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. AUTO-PROPERTIES:
 * 
 *    public string Nome { get; set; }
 *    
 *    Equivalente a:
 *    private string _nome;
 *    public string Nome
 *    {
 *        get { return _nome; }
 *        set { _nome = value; }
 *    }
 * 
 * 2. EXPRESSION-BODIED MEMBERS:
 * 
 *    public bool EhMaiorDeIdade() => Idade >= 18;
 *    
 *    Equivalente a:
 *    public bool EhMaiorDeIdade()
 *    {
 *        return Idade >= 18;
 *    }
 * 
 * 3. OBJECT INITIALIZER:
 * 
 *    var pessoa = new Pessoa
 *    {
 *        Nome = "João",
 *        Idade = 25
 *    };
 *    
 *    Equivalente a:
 *    var pessoa = new Pessoa();
 *    pessoa.Nome = "João";
 *    pessoa.Idade = 25;
 * 
 * ═══════════════════════════════════════════════════════════
 * BOAS PRÁTICAS
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Use auto-properties quando não há validação
 * ✅ Valide dados importantes (idade, email)
 * ✅ Normalize dados (Trim, ToLower)
 * ✅ Use expression-bodied para métodos simples
 * ✅ Crie properties calculadas quando fizer sentido
 * 
 * ═══════════════════════════════════════════════════════════
 */