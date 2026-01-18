// Desafio dojo: Sistema bancário orientado a objetos

using System;
using System.Collections.Generic;

class App
{
    static void Main()
    {
        Console.WriteLine("Iniciando o sistema bancário...");

        Banco banco = new Banco();

        // Menu de operações
        while (true)
        {
            Console.WriteLine("\n1 - Cadastrar novo cliente");
            Console.WriteLine("2 - Cadastrar conta para cliente");
            Console.WriteLine("3 - Listar clientes");
            Console.WriteLine("4 - Consultar saldo de conta");
            Console.WriteLine("5 - Efetuar depósito");
            Console.WriteLine("6 - Efetuar saque");
            Console.WriteLine("7 - Efetuar transferência");
            Console.WriteLine("S - Sair");
            Console.Write("Selecione a ação: ");
            string opcao = Console.ReadLine().ToUpper();

            if (opcao == "S")
                break;

            switch (opcao)
            {
                case "1":
                    banco.CadastrarNovoCliente();

                    break;
                case "2":
                    banco.CadastrarNovaContaBancaria();

                    break;
                case "3":
                    banco.ListarClientesEContasBancarias();

                    break;
                case "4":
                    banco.ConsultarSaldoContaBancaria();

                    break;
                case "5":
                    banco.RealizarDepositoContaBancaria();

                    break;
                case "6":
                    banco.RealizarSaqueContaBancaria();

                    break;
                case "7":
                    banco.RealizarTransferenciaContasBancarias();

                    break;
                default:
                    Console.WriteLine("\nOpção inválida.");

                    break;
            }
        }

        Console.WriteLine("\nEncerrando o sistema bancário...");
    }
}

public class Cliente
{
    public string Cpf { get; }
    public string Nome { get; }

    private List<ContaBancaria> _contasBancarias;

    public Cliente(string cpf, string nome)
    {
        Cpf = cpf;
        Nome = nome;

        _contasBancarias = new List<ContaBancaria>();
    }

    public List<ContaBancaria> ObterContasBancarias()
    {
        return _contasBancarias;
    }

    public ContaBancaria ObterContaBancaria(int numero)
    {
        for (int i = 0; i < _contasBancarias.Count; i++)
        {
            if (_contasBancarias[i].Numero == numero)
            {
                return _contasBancarias[i];
            }
        }

        return null;
    }

    public void VincularNovaContaBancaria(ContaBancaria conta)
    {
        _contasBancarias.Add(conta);
    }
}

public abstract class ContaBancaria
{
    public abstract int Tipo { get; }
    public abstract string DescricaoAmigavel { get; }

    public int Numero { get; }

    private decimal _saldo;
    public string Saldo
    {
        get
        {
            return $"R$ {_saldo:F2}";
        }
    }

    protected ContaBancaria(int numero, decimal saldoInicial)
    {
        Numero = numero;

        _saldo = saldoInicial;
    }

    public virtual void Depositar(decimal valor)
    {
        _saldo += valor;
    }

    public virtual bool Sacar(decimal valor)
    {
        if (_saldo < valor)
            return false;

        _saldo -= valor;

        return true;
    }
}

public class ContaPoupanca : ContaBancaria
{
    public override int Tipo { get; } = 1;
    public override string DescricaoAmigavel { get; } = "Poupança";

    public ContaPoupanca(int numero, decimal saldoInicial) : base(numero, saldoInicial)
    { }
}

public class ContaCorrente : ContaBancaria
{
    public override int Tipo { get; } = 2;
    public override string DescricaoAmigavel { get; } = "Corrente";

    public ContaCorrente(int numero, decimal saldoInicial) : base(numero, saldoInicial)
    { }
}

public class Banco
{
    private static int _numeroConta = 1;

    private Dictionary<int, string> _mapaContasBancariasCpfs; // Chave: Números de conta, Valor: CPF

    private List<Cliente> _clientes;

    public Banco()
    {
        _clientes = new List<Cliente>();

        _mapaContasBancariasCpfs = new Dictionary<int, string>();
    }

    #region Operações públicas de clientes

    public List<Cliente> ObterClientes()
    {
        return _clientes;
    }

    public Cliente ObterCliente(string cpf)
    {
        for (int i = 0; i < _clientes.Count; i++)
        {
            if (_clientes[i].Cpf == cpf)
            {
                return _clientes[i];
            }
        }

        return null;
    }

    public bool ClienteCadastrado(string cpf)
    {
        return _mapaContasBancariasCpfs.ContainsValue(cpf);
    }

    public void CadastrarNovoCliente()
    {
        Console.WriteLine("\nDigite o CPF do cliente (ou 'S' para sair):");
        string inputCPF = Console.ReadLine();

        if (inputCPF.ToUpper() == "S")
            return;

        if (this.ClienteCadastrado(inputCPF))
        {
            Console.WriteLine("Cliente com esse CPF já está cadastrado.");

            return;
        }

        Console.WriteLine($"Digite o nome do cliente (ou 'S' para sair):");
        string inputNome = Console.ReadLine();

        if (inputNome.ToUpper() == "S")
            return;

        Cliente novoCliente = new Cliente(inputCPF, inputNome);

        _clientes.Add(novoCliente);

        Console.WriteLine($"Cliente '{novoCliente.Nome}' cadastrado com sucesso!");
    }

    #endregion

    #region Operações públicas de contas bancárias

    public ContaBancaria ObterContaBancaria(int numeroConta)
    {
        Cliente cliente = this.ObterCliente(_mapaContasBancariasCpfs[numeroConta]);

        return cliente.ObterContaBancaria(numeroConta);
    }

    public bool ContaBancariaCadastrada(int numeroConta)
    {
        return _mapaContasBancariasCpfs.ContainsKey(numeroConta);
    }

    public void CadastrarNovaContaBancaria()
    {
        Console.WriteLine("\nDigite o CPF do cliente (ou 'S' para sair):");
        string inputCPF = Console.ReadLine();

        if (inputCPF.ToUpper() == "S")
            return;

        Cliente cliente = this.ObterCliente(inputCPF);

        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");

            return;
        }

        Console.WriteLine("Digite o número respectivo ao tipo de conta, sendo 1 para 'Poupança' e 2 para 'Corrente' (ou 'S' para sair):");
        string inputTipo = Console.ReadLine();

        if (inputTipo.ToUpper() == "S")
            return;

        if (!int.TryParse(inputTipo, out int tipo))
        {
            Console.WriteLine("Tipo de conta inválido.");

            return;
        }

        if (!this.TipoValido(tipo))
        {
            Console.WriteLine("Tipo de conta inválido.");

            return;
        }

        Console.WriteLine("Digite o saldo inicial (R$):");
        string inputSaldo = Console.ReadLine();

        if (!decimal.TryParse(inputSaldo, out decimal saldo))
        {
            Console.WriteLine("Saldo inválido. Conta iniciará com saldo R$ 0,00.");

            saldo = 0;
        }

        ContaBancaria novaContaBancaria = this.TipoContaPoupanca(tipo)
            ? new ContaPoupanca(_numeroConta, saldo)
            : new ContaCorrente(_numeroConta, saldo);

        cliente.VincularNovaContaBancaria(novaContaBancaria);

        _mapaContasBancariasCpfs.Add(novaContaBancaria.Numero, cliente.Cpf);

        Console.WriteLine($"Conta {novaContaBancaria.Numero} criada para o cliente {cliente.Nome} com sucesso! Saldo de {novaContaBancaria.Saldo}.");

        _numeroConta++;
    }

    public void ConsultarSaldoContaBancaria()
    {
        Console.WriteLine("\nDigite o número da conta (ou 'S' para sair):");
        string inputNumeroConta = Console.ReadLine();

        if (inputNumeroConta.ToUpper() == "S")
            return;

        if (!int.TryParse(inputNumeroConta, out int numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        if (!this.ContaBancariaCadastrada(numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        ContaBancaria contaBancaria = this.ObterContaBancaria(numeroConta);

        Console.WriteLine($"Saldo da conta número {contaBancaria.Numero}: {contaBancaria.Saldo}.");

        // Aqui o desafio adicional do dojo anterior se torna trivial: E se precisar descobrir o cliente dono da conta pra imprimir junto na mensagem?

        /*
         *   Cliente cliente = this.ObterCliente(_mapaContasBancariasCpfs[numeroConta]);
        */
    }

    public void RealizarDepositoContaBancaria()
    {
        Console.WriteLine("\nDigite o número da conta (ou 'S' para sair):");
        string inputNumeroConta = Console.ReadLine();

        if (inputNumeroConta.ToUpper() == "S")
            return;

        if (!int.TryParse(inputNumeroConta, out int numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        if (!this.ContaBancariaCadastrada(numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        Console.WriteLine("Digite o valor do depósito (R$):");
        string inputDeposito = Console.ReadLine();

        if (!decimal.TryParse(inputDeposito, out decimal deposito))
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        if (deposito <= 0)
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        ContaBancaria contaBancaria = this.ObterContaBancaria(numeroConta);

        contaBancaria.Depositar(deposito);

        Console.WriteLine($"Depósito de R$ {deposito:F2} realizado com sucesso na conta {contaBancaria.Numero}! Saldo de {contaBancaria.Saldo}.");
    }

    public void RealizarSaqueContaBancaria()
    {
        Console.WriteLine("\nDigite o número da conta (ou 'S' para sair):");
        string inputNumeroConta = Console.ReadLine();

        if (inputNumeroConta.ToUpper() == "S")
            return;

        if (!int.TryParse(inputNumeroConta, out int numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        if (!this.ContaBancariaCadastrada(numeroConta))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        Console.WriteLine("Digite o valor do saque (R$):");
        string inputSaque = Console.ReadLine();

        if (!decimal.TryParse(inputSaque, out decimal saque))
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        if (saque <= 0)
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        ContaBancaria contaBancaria = this.ObterContaBancaria(numeroConta);

        bool saqueBemSucedido = contaBancaria.Sacar(saque);

        if (!saqueBemSucedido)
        {
            Console.WriteLine($"Saque de R$ {saque:F2} NÃO foi realizado devido a saldo insuficiente na conta {contaBancaria.Numero}! Saldo de {contaBancaria.Saldo}.");

            return;
        }

        Console.WriteLine($"Saque de R$ {saque:F2} realizado com sucesso na conta {contaBancaria.Numero}! Saldo de {contaBancaria.Saldo}.");
    }

    public void RealizarTransferenciaContasBancarias()
    {
        Console.WriteLine("\nDigite o número da conta originária (ou 'S' para sair):");
        string inputNumeroContaOriginaria = Console.ReadLine();

        if (inputNumeroContaOriginaria.ToUpper() == "S")
            return;

        if (!int.TryParse(inputNumeroContaOriginaria, out int numeroContaOriginaria))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        if (!this.ContaBancariaCadastrada(numeroContaOriginaria))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        Console.WriteLine("\nDigite o número da conta destinatária (ou 'S' para sair):");
        string inputNumeroContaDestinataria = Console.ReadLine();

        if (inputNumeroContaDestinataria.ToUpper() == "S")
            return;

        if (!int.TryParse(inputNumeroContaDestinataria, out int numeroContaDestinataria))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        if (!this.ContaBancariaCadastrada(numeroContaDestinataria))
        {
            Console.WriteLine("Conta não encontrada.");

            return;
        }

        Console.WriteLine("Digite o valor da transferência (R$):");
        string inputTransferencia = Console.ReadLine();

        if (!decimal.TryParse(inputTransferencia, out decimal transferencia))
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        if (transferencia <= 0)
        {
            Console.WriteLine("Valor inválido.");

            return;
        }

        ContaBancaria contaBancariaOriginaria = this.ObterContaBancaria(numeroContaOriginaria);

        bool saqueBemSucedido = contaBancariaOriginaria.Sacar(transferencia);

        if (!saqueBemSucedido)
        {
            Console.WriteLine($"Transferência de R$ {transferencia:F2} NÃO foi realizada devido a saldo insuficiente na conta originária {contaBancariaOriginaria.Numero}! Saldo de {contaBancariaOriginaria.Saldo}.");

            return;
        }

        ContaBancaria contaBancariaDestinataria = this.ObterContaBancaria(numeroContaDestinataria);

        contaBancariaDestinataria.Depositar(transferencia);

        Console.WriteLine($"Transferência de R$ {transferencia:F2} realizada com sucesso da conta originária {contaBancariaOriginaria.Numero} para a conta destinatária {contaBancariaDestinataria.Numero}!");
    }

    #endregion

    #region Operações públicas híbridas (clientes e contas bancárias)

    public void ListarClientesEContasBancarias()
    {
        List<Cliente> clientes = this.ObterClientes();

        if (clientes.Count == 0)
        {
            Console.WriteLine("Não há clientes cadastrados.");

            return;
        }

        Console.WriteLine("\nLista de clientes:");

        for (int i = 0; i < clientes.Count; i++)
        {
            Cliente cliente = clientes[i];

            Console.WriteLine($"\n{cliente.Nome} ({cliente.Cpf}):");

            List<ContaBancaria> contas = cliente.ObterContasBancarias();

            if (contas.Count == 0)
            {
                Console.WriteLine($">>> Nenhuma conta cadastrada.");

                continue;
            }

            for (int j = 0; j < contas.Count; j++)
            {
                ContaBancaria conta = contas[j];

                Console.WriteLine($">>> Conta {conta.DescricaoAmigavel.ToLower()} número {conta.Numero}: {conta.Saldo}.");
            }
        }
    }

    #endregion

    private bool TipoContaPoupanca(int tipo)
    {
        /*
         * Existem formas muito mais elegantes e otimizadas de fazer isso, como por exemplo:
         *   a) Utilizar uma interface com uma propriedade "static abstract int Tipo { get; }", gerando um contrato para forçar a existência de um 
         *      "Tipo" estático nas classes consumidoras, e por consequência conseguir acessar os tipos sem precisar instanciar objetos temporários;
         *   b) Transportar as responsabilidades de criação para uma Factory que saberia como aplicar as devidas validações e instanciar novas contas 
         *      conforme o tipo solicitado.
         * Para manter o código simples e didático, vamos manter essa abordagem mais "rudimentar".
        */

        ContaPoupanca contaPoupancaTemp = new ContaPoupanca(0, 0); // Conta meramente temporária para obter o tipo.

        return tipo == contaPoupancaTemp.Tipo;
    }

    private bool TipoContaCorrente(int tipo)
    {
        /*
         * Existem formas muito mais elegantes e otimizadas de fazer isso, como por exemplo:
         *   a) Utilizar uma interface com uma propriedade "static abstract int Tipo { get; }", gerando um contrato para forçar a existência de um 
         *      "Tipo" estático nas classes consumidoras, e por consequência conseguir acessar os tipos sem precisar instanciar objetos temporários;
         *   b) Transportar as responsabilidades de criação para uma Factory que saberia como aplicar as devidas validações e instanciar novas contas 
         *      conforme o tipo solicitado.
         * Para manter o código simples e didático, vamos manter essa abordagem mais "rudimentar".
        */

        ContaCorrente contaCorrenteTemp = new ContaCorrente(0, 0); // Conta meramente temporária para obter o tipo.

        return tipo == contaCorrenteTemp.Tipo;
    }

    private bool TipoValido(int tipo)
    {
        return this.TipoContaCorrente(tipo) || this.TipoContaPoupanca(tipo);
    }
}
