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