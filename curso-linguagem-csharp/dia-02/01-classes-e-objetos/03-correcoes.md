# 📝 Correções dos Exercícios

## 🎯 Exercício 1

```csharp
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
```

---

## 🎯 Exercício 2

```csharp
// Exercício 2 Corrigido: Conta Bancária com Encapsulamento
// Arquivo: Program.cs

using System;

public class ContaBancaria
{
    // Field privado para saldo
    private decimal _saldo;
    
    // Property somente leitura (definida no construtor)
    public string NumeroConta { get; }
    
    // Property pública
    public string Titular { get; set; }
    
    // Property com getter público, setter privado
    public decimal Saldo => _saldo;
    
    // Construtor
    public ContaBancaria(string numeroConta, string titular)
    {
        if (string.IsNullOrWhiteSpace(numeroConta))
            throw new ArgumentException("Número da conta é obrigatório");
        
        if (string.IsNullOrWhiteSpace(titular))
            throw new ArgumentException("Titular é obrigatório");
        
        NumeroConta = numeroConta;
        Titular = titular;
        _saldo = 0;
    }
    
    // Construtor com saldo inicial
    public ContaBancaria(string numeroConta, string titular, decimal saldoInicial)
        : this(numeroConta, titular)
    {
        if (saldoInicial < 0)
            throw new ArgumentException("Saldo inicial não pode ser negativo");
        
        _saldo = saldoInicial;
    }
    
    public void Depositar(decimal valor)
    {
        if (valor <= 0)
        {
            Console.WriteLine("❌ Valor de depósito deve ser positivo!");
            return;
        }
        
        _saldo += valor;
        Console.WriteLine($"✅ Depósito de R$ {valor:F2} realizado com sucesso!");
        Console.WriteLine($"   Saldo atual: R$ {_saldo:F2}");
    }
    
    public bool Sacar(decimal valor)
    {
        if (valor <= 0)
        {
            Console.WriteLine("❌ Valor de saque deve ser positivo!");
            return false;
        }
        
        if (valor > _saldo)
        {
            Console.WriteLine($"❌ Saldo insuficiente! Saldo: R$ {_saldo:F2}, Tentativa: R$ {valor:F2}");
            return false;
        }
        
        _saldo -= valor;
        Console.WriteLine($"✅ Saque de R$ {valor:F2} realizado com sucesso!");
        Console.WriteLine($"   Saldo atual: R$ {_saldo:F2}");
        return true;
    }
    
    public bool Transferir(ContaBancaria destino, decimal valor)
    {
        if (destino == null)
        {
            Console.WriteLine("❌ Conta de destino inválida!");
            return false;
        }
        
        if (valor <= 0)
        {
            Console.WriteLine("❌ Valor de transferência deve ser positivo!");
            return false;
        }
        
        if (valor > _saldo)
        {
            Console.WriteLine($"❌ Saldo insuficiente para transferência!");
            return false;
        }
        
        _saldo -= valor;
        destino._saldo += valor;
        
        Console.WriteLine($"✅ Transferência de R$ {valor:F2} realizada com sucesso!");
        Console.WriteLine($"   De: {Titular} (Conta: {NumeroConta})");
        Console.WriteLine($"   Para: {destino.Titular} (Conta: {destino.NumeroConta})");
        Console.WriteLine($"   Seu saldo atual: R$ {_saldo:F2}");
        
        return true;
    }
    
    public void ExibirExtrato()
    {
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine("         EXTRATO BANCÁRIO          ");
        Console.WriteLine("═══════════════════════════════════");
        Console.WriteLine($"Conta:   {NumeroConta}");
        Console.WriteLine($"Titular: {Titular}");
        Console.WriteLine($"Saldo:   R$ {_saldo:F2}");
        Console.WriteLine("═══════════════════════════════════");
    }
}

// ═══════════════════════════════════════════════════════════
// VERSÃO AVANÇADA: Com Histórico de Transações
// ═══════════════════════════════════════════════════════════

public class ContaBancariaAvancada
{
    private decimal _saldo;
    private List<string> _historico = new List<string>();
    
    public string NumeroConta { get; }
    public string Titular { get; set; }
    public decimal Saldo => _saldo;
    public DateTime DataAbertura { get; }
    
    public ContaBancariaAvancada(string numeroConta, string titular)
    {
        NumeroConta = numeroConta;
        Titular = titular;
        DataAbertura = DateTime.Now;
        _historico.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} - Conta criada");
    }
    
    public void Depositar(decimal valor)
    {
        if (valor <= 0) return;
        
        _saldo += valor;
        _historico.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} - Depósito: +R$ {valor:F2} | Saldo: R$ {_saldo:F2}");
        Console.WriteLine($"✅ Depósito realizado!");
    }
    
    public bool Sacar(decimal valor)
    {
        if (valor <= 0 || valor > _saldo) return false;
        
        _saldo -= valor;
        _historico.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} - Saque: -R$ {valor:F2} | Saldo: R$ {_saldo:F2}");
        Console.WriteLine($"✅ Saque realizado!");
        return true;
    }
    
    public void ExibirHistorico()
    {
        Console.WriteLine("\n═══════════════════════════════════");
        Console.WriteLine("      HISTÓRICO DE TRANSAÇÕES      ");
        Console.WriteLine("═══════════════════════════════════");
        
        foreach (var transacao in _historico)
        {
            Console.WriteLine(transacao);
        }
        
        Console.WriteLine("═══════════════════════════════════");
    }
}

// ═══════════════════════════════════════════════════════════
// TESTE
// ═══════════════════════════════════════════════════════════

class Program
{
    static void Main()
    {
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║   SISTEMA DE CONTAS BANCÁRIAS     ║");
        Console.WriteLine("╚═══════════════════════════════════╝\n");
        
        // Criar contas
        var conta1 = new ContaBancaria("001-X", "João Silva");
        var conta2 = new ContaBancaria("002-Y", "Maria Santos", 500);
        
        // Testar depósitos
        Console.WriteLine("─── DEPÓSITOS ───\n");
        conta1.Depositar(1000);
        conta1.Depositar(-50);  // Inválido
        Console.WriteLine();
        
        // Testar saques
        Console.WriteLine("─── SAQUES ───\n");
        conta1.Sacar(300);
        conta1.Sacar(2000);  // Insuficiente
        Console.WriteLine();
        
        // Exibir extratos
        Console.WriteLine("─── EXTRATOS ───\n");
        conta1.ExibirExtrato();
        Console.WriteLine();
        conta2.ExibirExtrato();
        Console.WriteLine();
        
        // Testar transferência
        Console.WriteLine("─── TRANSFERÊNCIAS ───\n");
        conta1.Transferir(conta2, 200);
        Console.WriteLine();
        
        // Extratos após transferência
        conta1.ExibirExtrato();
        Console.WriteLine();
        conta2.ExibirExtrato();
        
        // Teste com histórico
        Console.WriteLine("\n\n╔═══════════════════════════════════╗");
        Console.WriteLine("║   CONTA COM HISTÓRICO             ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        
        var contaAvancada = new ContaBancariaAvancada("003-Z", "Pedro Costa");
        contaAvancada.Depositar(1000);
        contaAvancada.Sacar(200);
        contaAvancada.Depositar(500);
        contaAvancada.Sacar(100);
        contaAvancada.ExibirHistorico();
    }
}

/*
 * ═══════════════════════════════════════════════════════════
 * CONCEITOS DEMONSTRADOS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. ENCAPSULAMENTO:
 *    - Field privado: _saldo
 *    - Acesso controlado através de métodos
 *    - Validações em todos os métodos
 * 
 * 2. PROPERTIES SOMENTE LEITURA:
 *    public string NumeroConta { get; }
 *    - Só pode ser definido no construtor
 *    - Imutável após criação
 * 
 * 3. PROPERTY CALCULADA:
 *    public decimal Saldo => _saldo;
 *    - Retorna valor do field privado
 *    - Não permite alteração externa
 * 
 * 4. CONSTRUCTOR CHAINING:
 *    public ContaBancaria(string num, string tit, decimal saldo)
 *        : this(num, tit)
 *    - Chama outro construtor
 *    - Evita duplicação de código
 * 
 * 5. VALIDAÇÕES:
 *    - Valores positivos
 *    - Saldo suficiente
 *    - Null checks
 * 
 * ═══════════════════════════════════════════════════════════
 * POR QUE ENCAPSULAMENTO É IMPORTANTE?
 * ═══════════════════════════════════════════════════════════
 * 
 * SEM ENCAPSULAMENTO (❌ RUIM):
 * 
 * public class ContaRuim
 * {
 *     public decimal saldo;  // EXPOSTO!
 * }
 * 
 * var conta = new ContaRuim();
 * conta.saldo = -1000;  // ❌ Permitido mas incorreto!
 * conta.saldo = 999999; // ❌ Sem controle!
 * 
 * 
 * COM ENCAPSULAMENTO (✅ BOM):
 * 
 * public class ContaBoa
 * {
 *     private decimal _saldo;  // PROTEGIDO!
 *     
 *     public void Depositar(decimal valor)
 *     {
 *         if (valor > 0)  // ✅ VALIDADO!
 *             _saldo += valor;
 *     }
 * }
 * 
 * BENEFÍCIOS:
 * 1. Proteção de dados
 * 2. Validação garantida
 * 3. Manutenção facilitada
 * 4. Lógica centralizada
 * 5. Menos bugs
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

## 🎯 Exercício 10

```csharp
// Exercício 10 Corrigido: Sistema de Pedidos (PROJETO FINAL)
// Arquivo: Program.cs

using System;
using System.Collections.Generic;
using System.Linq;

// ═══════════════════════════════════════════════════════════
// CLASSE: ItemPedido
// ═══════════════════════════════════════════════════════════

public class ItemPedido
{
    private decimal _precoUnitario;
    private int _quantidade;
    
    public string NomeProduto { get; set; }
    
    public decimal PrecoUnitario
    {
        get => _precoUnitario;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Preço deve ser positivo");
            _precoUnitario = value;
        }
    }
    
    public int Quantidade
    {
        get => _quantidade;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Quantidade deve ser positiva");
            _quantidade = value;
        }
    }
    
    // Property calculada
    public decimal Subtotal => _precoUnitario * _quantidade;
    
    public void ExibirItem()
    {
        Console.WriteLine($"  • {NomeProduto,-30} {Quantidade,3} x R$ {PrecoUnitario,8:F2} = R$ {Subtotal,10:F2}");
    }
}

// ═══════════════════════════════════════════════════════════
// CLASSE: Cliente
// ═══════════════════════════════════════════════════════════

public class Cliente
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string CPF { get; }
    public DateTime DataCadastro { get; }
    
    public Cliente(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório");
        
        CPF = cpf;
        DataCadastro = DateTime.Now;
    }
    
    public void ExibirDados()
    {
        Console.WriteLine($"Nome:     {Nome}");
        Console.WriteLine($"CPF:      {CPF}");
        Console.WriteLine($"Email:    {Email}");
        Console.WriteLine($"Telefone: {Telefone}");
        Console.WriteLine($"Cliente desde: {DataCadastro:dd/MM/yyyy}");
    }
}

// ═══════════════════════════════════════════════════════════
// CLASSE: Pedido
// ═══════════════════════════════════════════════════════════

public class Pedido
{
    // Static field para gerar números únicos
    private static int _proximoNumero = 1000;
    
    // Fields privados
    private List<ItemPedido> _itens;
    private string _status;
    private decimal _percentualDesconto;
    
    // Properties
    public int NumeroPedido { get; }
    public Cliente Cliente { get; }
    public DateTime DataPedido { get; }
    
    public string Status
    {
        get => _status;
        private set
        {
            var statusValidos = new[] { "Pendente", "Pago", "Enviado", "Entregue", "Cancelado" };
            if (!statusValidos.Contains(value))
                throw new ArgumentException($"Status inválido. Use: {string.Join(", ", statusValidos)}");
            _status = value;
        }
    }
    
    public bool TemDesconto => _percentualDesconto > 0;
    
    public decimal PercentualDesconto
    {
        get => _percentualDesconto;
        private set
        {
            if (value < 0 || value > 50)
                throw new ArgumentException("Desconto deve estar entre 0% e 50%");
            _percentualDesconto = value;
        }
    }
    
    // Properties calculadas
    public decimal ValorTotal => _itens.Sum(item => item.Subtotal);
    
    public decimal ValorDesconto => ValorTotal * (PercentualDesconto / 100);
    
    public decimal ValorFinal => ValorTotal - ValorDesconto;
    
    public int TotalItens => _itens.Count;
    
    public int TotalProdutos => _itens.Sum(item => item.Quantidade);
    
    // Construtor privado
    private Pedido(Cliente cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));
        
        NumeroPedido = _proximoNumero++;
        Cliente = cliente;
        DataPedido = DateTime.Now;
        _itens = new List<ItemPedido>();
        _status = "Pendente";
        _percentualDesconto = 0;
    }
    
    // Factory Method (static)
    public static Pedido CriarPedido(Cliente cliente)
    {
        return new Pedido(cliente);
    }
    
    public void AdicionarItem(ItemPedido item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        if (_status != "Pendente")
        {
            Console.WriteLine($"❌ Não é possível adicionar itens. Pedido está '{_status}'.");
            return;
        }
        
        // Verificar se já existe
        var itemExistente = _itens.FirstOrDefault(i => 
            i.NomeProduto.Equals(item.NomeProduto, StringComparison.OrdinalIgnoreCase));
        
        if (itemExistente != null)
        {
            itemExistente.Quantidade += item.Quantidade;
            Console.WriteLine($"✅ Quantidade atualizada: {item.NomeProduto}");
        }
        else
        {
            _itens.Add(item);
            Console.WriteLine($"✅ Item adicionado: {item.NomeProduto}");
        }
    }
    
    public bool RemoverItem(string nomeProduto)
    {
        if (_status != "Pendente")
        {
            Console.WriteLine($"❌ Não é possível remover itens. Pedido está '{_status}'.");
            return false;
        }
        
        var item = _itens.FirstOrDefault(i => 
            i.NomeProduto.Equals(nomeProduto, StringComparison.OrdinalIgnoreCase));
        
        if (item != null)
        {
            _itens.Remove(item);
            Console.WriteLine($"✅ Item removido: {nomeProduto}");
            return true;
        }
        
        Console.WriteLine($"❌ Item não encontrado: {nomeProduto}");
        return false;
    }
    
    public void AplicarDesconto(decimal percentual)
    {
        if (_status != "Pendente")
        {
            Console.WriteLine($"❌ Não é possível aplicar desconto. Pedido está '{_status}'.");
            return;
        }
        
        try
        {
            PercentualDesconto = percentual;
            Console.WriteLine($"✅ Desconto de {percentual}% aplicado!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ {ex.Message}");
        }
    }
    
    public void AlterarStatus(string novoStatus)
    {
        try
        {
            var statusAnterior = _status;
            Status = novoStatus;
            Console.WriteLine($"✅ Status alterado: {statusAnterior} → {novoStatus}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ {ex.Message}");
        }
    }
    
    public void ExibirResumo()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    RESUMO DO PEDIDO                       ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.WriteLine($"Pedido #:      {NumeroPedido}");
        Console.WriteLine($"Data:          {DataPedido:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Cliente:       {Cliente.Nome}");
        Console.WriteLine($"Status:        {Status}");
        Console.WriteLine($"Total de itens: {TotalItens}");
        Console.WriteLine($"Total produtos: {TotalProdutos}");
        Console.WriteLine($"Valor total:    R$ {ValorTotal:F2}");
        
        if (TemDesconto)
        {
            Console.WriteLine($"Desconto ({PercentualDesconto}%): -R$ {ValorDesconto:F2}");
            Console.WriteLine($"VALOR FINAL:    R$ {ValorFinal:F2}");
        }
        
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }
    
    public void ExibirDetalhado()
    {
        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  PEDIDO DETALHADO                         ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.WriteLine($"Pedido #: {NumeroPedido}");
        Console.WriteLine($"Data: {DataPedido:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine();
        
        Console.WriteLine("─── DADOS DO CLIENTE ───");
        Cliente.ExibirDados();
        Console.WriteLine();
        
        Console.WriteLine("─── ITENS DO PEDIDO ───");
        Console.WriteLine($"{"Produto",-30} {"Qtd",5} {"Preço Unit.",12} {"Subtotal",15}");
        Console.WriteLine(new string('─', 65));
        
        foreach (var item in _itens)
        {
            item.ExibirItem();
        }
        
        Console.WriteLine(new string('─', 65));
        Console.WriteLine($"{"SUBTOTAL:",45} R$ {ValorTotal,10:F2}");
        
        if (TemDesconto)
        {
            Console.WriteLine($"{"Desconto (" + PercentualDesconto + "%):",45} -R$ {ValorDesconto,10:F2}");
            Console.WriteLine(new string('─', 65));
            Console.WriteLine($"{"VALOR FINAL:",45} R$ {ValorFinal,10:F2}");
        }
        
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }
}

// ═══════════════════════════════════════════════════════════
// PROGRAMA DE TESTE
// ═══════════════════════════════════════════════════════════

class Program
{
    static void Main()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              SISTEMA DE GERENCIAMENTO DE PEDIDOS          ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝\n");
        
        // Criar cliente
        var cliente = new Cliente("123.456.789-00")
        {
            Nome = "João Silva",
            Email = "joao@email.com",
            Telefone = "(11) 98765-4321"
        };
        
        Console.WriteLine("─── CRIANDO PEDIDO ───\n");
        
        // Criar pedido usando factory method
        var pedido = Pedido.CriarPedido(cliente);
        
        Console.WriteLine($"Pedido #{pedido.NumeroPedido} criado com sucesso!\n");
        
        // Adicionar itens
        Console.WriteLine("─── ADICIONANDO ITENS ───\n");
        
        pedido.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Notebook Dell Inspiron 15",
            PrecoUnitario = 3500,
            Quantidade = 1
        });
        
        pedido.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Mouse Logitech MX Master",
            PrecoUnitario = 350,
            Quantidade = 2
        });
        
        pedido.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Teclado Mecânico Keychron",
            PrecoUnitario = 650,
            Quantidade = 1
        });
        
        pedido.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Webcam Logitech C920",
            PrecoUnitario = 450,
            Quantidade = 1
        });
        
        Console.WriteLine();
        
        // Exibir pedido
        pedido.ExibirDetalhado();
        
        // Aplicar desconto
        Console.WriteLine("\n─── APLICANDO DESCONTO ───\n");
        pedido.AplicarDesconto(10);
        
        Console.WriteLine();
        pedido.ExibirResumo();
        
        // Alterar status
        Console.WriteLine("\n─── PROCESSANDO PEDIDO ───\n");
        pedido.AlterarStatus("Pago");
        pedido.AlterarStatus("Enviado");
        
        Console.WriteLine();
        pedido.ExibirResumo();
        
        // Tentar adicionar item após pago
        Console.WriteLine("\n─── TENTANDO ADICIONAR ITEM ───\n");
        pedido.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Mousepad",
            PrecoUnitario = 80,
            Quantidade = 1
        });
        
        // Criar segundo pedido
        Console.WriteLine("\n\n─── CRIANDO SEGUNDO PEDIDO ───\n");
        
        var cliente2 = new Cliente("987.654.321-00")
        {
            Nome = "Maria Santos",
            Email = "maria@email.com",
            Telefone = "(11) 91234-5678"
        };
        
        var pedido2 = Pedido.CriarPedido(cliente2);
        
        pedido2.AdicionarItem(new ItemPedido
        {
            NomeProduto = "Monitor LG 27''",
            PrecoUnitario = 1200,
            Quantidade = 2
        });
        
        pedido2.AplicarDesconto(15);
        pedido2.ExibirDetalhado();
        
        Console.WriteLine("\n\n✅ Sistema testado com sucesso!");
    }
}

/*
 * ═══════════════════════════════════════════════════════════
 * CONCEITOS AVANÇADOS DEMONSTRADOS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. COMPOSIÇÃO:
 *    - Pedido TEM Cliente
 *    - Pedido TEM Lista de ItemPedido
 *    - Relacionamento "tem-um" (has-a)
 * 
 * 2. ENCAPSULAMENTO COMPLETO:
 *    - Todos os fields privados
 *    - Acesso controlado por properties e métodos
 *    - Validações em todos os setters
 * 
 * 3. FACTORY METHOD PATTERN:
 *    public static Pedido CriarPedido(Cliente cliente)
 *    - Construtor privado
 *    - Criação controlada
 *    - Inicialização garantida
 * 
 * 4. PROPERTIES CALCULADAS:
 *    public decimal ValorTotal => _itens.Sum(item => item.Subtotal);
 *    - Sempre atualizado
 *    - Não armazena valor
 *    - Calcula em tempo real
 * 
 * 5. AUTO-INCREMENT STATIC:
 *    private static int _proximoNumero = 1000;
 *    - Compartilhado por todas as instâncias
 *    - Gera IDs únicos
 *    - Thread-safe em cenários simples
 * 
 * 6. LINQ INTEGRATION:
 *    _itens.Sum(item => item.Subtotal)
 *    _itens.FirstOrDefault(i => ...)
 *    - Operações em coleções
 *    - Sintaxe funcional
 * 
 * 7. IMUTABILIDADE PARCIAL:
 *    public int NumeroPedido { get; }
 *    - Propriedades readonly
 *    - Definidas apenas no construtor
 * 
 * 8. VALIDAÇÃO DE REGRAS DE NEGÓCIO:
 *    - Desconto máximo 50%
 *    - Não modificar pedido pago
 *    - Status válidos predefinidos
 * 
 * ═══════════════════════════════════════════════════════════
 * PADRÕES DE DESIGN APLICADOS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. FACTORY METHOD:
 *    - Criação controlada de objetos
 *    - Construtor privado
 *    - Método estático público
 * 
 * 2. VALUE OBJECT:
 *    - ItemPedido como value object
 *    - Representa valor, não entidade
 * 
 * 3. AGGREGATE ROOT:
 *    - Pedido como raiz do agregado
 *    - Controla acesso aos itens
 *    - Mantém consistência
 * 
 * 4. FLUENT INTERFACE (parcial):
 *    - Métodos retornam void mas poderiam retornar this
 *    - Para encadeamento: pedido.Add().Apply().Save()
 * 
 * ═══════════════════════════════════════════════════════════
 * MELHORIAS POSSÍVEIS (PRÓXIMOS DIAS)
 * ═══════════════════════════════════════════════════════════
 * 
 * DIA 03 - HERANÇA:
 * - TipoPedido: PedidoNormal, PedidoExpress, PedidoInternacional
 * - Diferentes regras de frete e prazo
 * 
 * DIA 04 - COLEÇÕES:
 * - IEnumerable<ItemPedido> para itens
 * - LINQ avançado para consultas
 * 
 * DIA 05 - EXCEÇÕES:
 * - PedidoInvalidoException
 * - ItemNaoEncontradoException
 * - StatusInvalidoException
 * 
 * DIA 06 - ARQUIVOS:
 * - Salvar pedidos em JSON
 * - Carregar histórico
 * - Exportar para PDF
 * 
 * DIA 07 - BANCO DE DADOS:
 * - Persistir em SQL Server
 * - Entity Framework Core
 * - Migrations
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

