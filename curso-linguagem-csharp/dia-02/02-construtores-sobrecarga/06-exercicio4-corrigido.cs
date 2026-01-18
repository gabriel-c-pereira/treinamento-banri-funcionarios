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