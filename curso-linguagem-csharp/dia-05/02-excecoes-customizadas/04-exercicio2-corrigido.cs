using System;

namespace ExcecoesCustomizadas.Ex02
{
    /// <summary>
    /// Exceção base para operações bancárias.
    /// </summary>
    public abstract class OperacaoBancariaException : Exception
    {
        public string NumeroConta { get; }
        public DateTime DataHora { get; }
        
        protected OperacaoBancariaException(
            string numeroConta,
            string mensagem)
            : base(mensagem)
        {
            NumeroConta = numeroConta;
            DataHora = DateTime.UtcNow;
        }
        
        protected OperacaoBancariaException(
            string numeroConta,
            string mensagem,
            Exception innerException)
            : base(mensagem, innerException)
        {
            NumeroConta = numeroConta;
            DataHora = DateTime.UtcNow;
        }
        
        public override string ToString()
        {
            return $"{GetType().Name}\n" +
                   $"Conta: {NumeroConta}\n" +
                   $"Data/Hora: {DataHora:yyyy-MM-dd HH:mm:ss}\n" +
                   $"Mensagem: {Message}";
        }
    }
    
    /// <summary>
    /// Exceção lançada quando há saldo insuficiente para uma operação.
    /// </summary>
    public class SaldoInsuficienteException : OperacaoBancariaException
    {
        public decimal ValorSolicitado { get; }
        public decimal SaldoAtual { get; }
        public decimal Diferenca => ValorSolicitado - SaldoAtual;
        
        public SaldoInsuficienteException(
            string numeroConta,
            decimal valorSolicitado,
            decimal saldoAtual)
            : base(
                numeroConta,
                $"Saldo insuficiente. Solicitado: {valorSolicitado:C}, " +
                $"Disponível: {saldoAtual:C}, Faltam: {(valorSolicitado - saldoAtual):C}")
        {
            ValorSolicitado = valorSolicitado;
            SaldoAtual = saldoAtual;
        }
    }
    
    /// <summary>
    /// Exceção lançada quando se tenta operar em conta bloqueada.
    /// </summary>
    public class ContaBloqueadaException : OperacaoBancariaException
    {
        public string Motivo { get; }
        public DateTime DataBloqueio { get; }
        
        public ContaBloqueadaException(
            string numeroConta,
            string motivo,
            DateTime dataBloqueio)
            : base(
                numeroConta,
                $"Conta bloqueada desde {dataBloqueio:dd/MM/yyyy}: {motivo}")
        {
            Motivo = motivo;
            DataBloqueio = dataBloqueio;
        }
    }
    
    /// <summary>
    /// Exceção lançada quando valor de transação excede limite permitido.
    /// </summary>
    public class LimiteTransacaoExcedidoException : OperacaoBancariaException
    {
        public decimal ValorLimite { get; }
        public decimal ValorTentado { get; }
        public string TipoLimite { get; }
        
        public LimiteTransacaoExcedidoException(
            string numeroConta,
            decimal valorLimite,
            decimal valorTentado,
            string tipoLimite = "diário")
            : base(
                numeroConta,
                $"Limite {tipoLimite} excedido. Limite: {valorLimite:C}, " +
                $"Tentado: {valorTentado:C}")
        {
            ValorLimite = valorLimite;
            ValorTentado = valorTentado;
            TipoLimite = tipoLimite;
        }
    }
    
    public class ContaBancaria
    {
        public string Numero { get; }
        public decimal Saldo { get; private set; }
        public bool Bloqueada { get; private set; }
        public string? MotivoBloqueio { get; private set; }
        public DateTime? DataBloqueio { get; private set; }
        public decimal LimiteDiario { get; set; } = 5000m;
        public decimal TotalSacadoHoje { get; private set; }
        private DateTime _ultimaAtualizacaoLimite = DateTime.Today;
        
        public ContaBancaria(string numero, decimal saldoInicial)
        {
            Numero = numero;
            Saldo = saldoInicial;
        }
        
        public void Bloquear(string motivo)
        {
            Bloqueada = true;
            MotivoBloqueio = motivo;
            DataBloqueio = DateTime.UtcNow;
        }
        
        public void Desbloquear()
        {
            Bloqueada = false;
            MotivoBloqueio = null;
            DataBloqueio = null;
        }
        
        public void Sacar(decimal valor)
        {
            // Verifica bloqueio
            if (Bloqueada)
                throw new ContaBloqueadaException(
                    Numero,
                    MotivoBloqueio!,
                    DataBloqueio!.Value);
            
            // Reseta contador diário se necessário
            if (DateTime.Today > _ultimaAtualizacaoLimite)
            {
                TotalSacadoHoje = 0;
                _ultimaAtualizacaoLimite = DateTime.Today;
            }
            
            // Verifica limite diário
            if (TotalSacadoHoje + valor > LimiteDiario)
                throw new LimiteTransacaoExcedidoException(
                    Numero,
                    LimiteDiario,
                    TotalSacadoHoje + valor);
            
            // Verifica saldo
            if (valor > Saldo)
                throw new SaldoInsuficienteException(
                    Numero,
                    valor,
                    Saldo);
            
            Saldo -= valor;
            TotalSacadoHoje += valor;
        }
        
        public void Transferir(string contaDestino, decimal valor)
        {
            // Verifica bloqueio
            if (Bloqueada)
                throw new ContaBloqueadaException(
                    Numero,
                    MotivoBloqueio!,
                    DataBloqueio!.Value);
            
            // Para transferências, usa limite maior
            decimal limiteTransferencia = LimiteDiario * 2;
            
            if (valor > limiteTransferencia)
                throw new LimiteTransacaoExcedidoException(
                    Numero,
                    limiteTransferencia,
                    valor,
                    "de transferência");
            
            // Verifica saldo
            if (valor > Saldo)
                throw new SaldoInsuficienteException(
                    Numero,
                    valor,
                    Saldo);
            
            Saldo -= valor;
            Console.WriteLine($"Transferência de {valor:C} para conta {contaDestino} realizada");
        }
        
        public void Depositar(decimal valor)
        {
            if (Bloqueada)
                throw new ContaBloqueadaException(
                    Numero,
                    MotivoBloqueio!,
                    DataBloqueio!.Value);
            
            Saldo += valor;
        }
    }
    
    class Program
    {
        static void Main()
        {
            var conta = new ContaBancaria("12345-6", 1000m);
            conta.LimiteDiario = 500m;
            
            // Teste 1: Saque normal
            Console.WriteLine("=== Teste 1: Saque Normal ===");
            TestarOperacao(() =>
            {
                conta.Sacar(200m);
                Console.WriteLine($"✓ Saque realizado. Saldo: {conta.Saldo:C}");
            });
            
            // Teste 2: Saldo insuficiente
            Console.WriteLine("\n=== Teste 2: Saldo Insuficiente ===");
            TestarOperacao(() =>
            {
                conta.Sacar(1500m);
            });
            
            // Teste 3: Limite excedido
            Console.WriteLine("\n=== Teste 3: Limite Diário Excedido ===");
            TestarOperacao(() =>
            {
                conta.Sacar(400m); // 200 + 400 = 600 > 500 (limite)
            });
            
            // Teste 4: Bloquear conta
            Console.WriteLine("\n=== Teste 4: Conta Bloqueada ===");
            conta.Bloquear("Atividade suspeita detectada");
            TestarOperacao(() =>
            {
                conta.Sacar(50m);
            });
            
            // Teste 5: Desbloquear e transferir
            Console.WriteLine("\n=== Teste 5: Transferência ===");
            conta.Desbloquear();
            TestarOperacao(() =>
            {
                conta.Transferir("98765-4", 100m);
                Console.WriteLine($"✓ Transferência realizada. Saldo: {conta.Saldo:C}");
            });
            
            Console.WriteLine("\nPressione qualquer tecla...");
            Console.ReadKey();
        }
        
        static void TestarOperacao(Action operacao)
        {
            try
            {
                operacao();
            }
            catch (SaldoInsuficienteException ex)
            {
                Console.WriteLine("✗ SALDO INSUFICIENTE:");
                Console.WriteLine($"  Conta: {ex.NumeroConta}");
                Console.WriteLine($"  Valor solicitado: {ex.ValorSolicitado:C}");
                Console.WriteLine($"  Saldo atual: {ex.SaldoAtual:C}");
                Console.WriteLine($"  Diferença: {ex.Diferenca:C}");
            }
            catch (ContaBloqueadaException ex)
            {
                Console.WriteLine("✗ CONTA BLOQUEADA:");
                Console.WriteLine($"  Conta: {ex.NumeroConta}");
                Console.WriteLine($"  Motivo: {ex.Motivo}");
                Console.WriteLine($"  Bloqueada em: {ex.DataBloqueio:dd/MM/yyyy HH:mm}");
            }
            catch (LimiteTransacaoExcedidoException ex)
            {
                Console.WriteLine("✗ LIMITE EXCEDIDO:");
                Console.WriteLine($"  Conta: {ex.NumeroConta}");
                Console.WriteLine($"  Tipo: {ex.TipoLimite}");
                Console.WriteLine($"  Limite: {ex.ValorLimite:C}");
                Console.WriteLine($"  Tentado: {ex.ValorTentado:C}");
            }
            catch (OperacaoBancariaException ex)
            {
                // Catch genérico para outras exceções bancárias
                Console.WriteLine($"✗ ERRO BANCÁRIO: {ex.Message}");
            }
        }
    }
}