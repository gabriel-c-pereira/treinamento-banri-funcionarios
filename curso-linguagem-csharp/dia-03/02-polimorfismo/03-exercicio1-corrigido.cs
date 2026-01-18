/*
 * Exercício 5 - Sistema de Pagamentos com Polimorfismo
 * 
 * Demonstra:
 * - Polimorfismo em ação
 * - Pattern matching para taxas
 * - Upcasting e uso polimórfico
 * - Classe base com métodos virtuais
 */

using System;
using System.Collections.Generic;

namespace Exercicio05_SistemaPagamentos
{
    // Classe base com comportamento polimórfico
    public abstract class Pagamento
    {
        public decimal ValorBase { get; set; }
        public DateTime DataProcessamento { get; set; }
        public string NumeroTransacao { get; protected set; }
        
        // Método abstrato - cada classe implementa sua lógica
        public abstract bool Processar(decimal valor);
        
        // Método virtual - pode ser sobrescrito
        public virtual decimal CalcularValorFinal(decimal valor)
        {
            return valor + ObterTaxa(valor);
        }
        
        // Método abstrato - cada pagamento tem sua taxa
        protected abstract decimal ObterTaxa(decimal valor);
        
        // Método concreto - comum a todos
        public string GerarRecibo()
        {
            return $"Recibo: {NumeroTransacao} - Valor: {ValorBase:C} - Data: {DataProcessamento}";
        }
    }

    // ===== IMPLEMENTAÇÕES ESPECÍFICAS =====
    
    public class CartaoCredito : Pagamento
    {
        public string NumeroCartao { get; set; }
        public int Parcelas { get; set; }
        
        public CartaoCredito(string numeroCartao, int parcelas = 1)
        {
            NumeroCartao = numeroCartao;
            Parcelas = parcelas;
        }
        
        public override bool Processar(decimal valor)
        {
            ValorBase = valor;
            DataProcessamento = DateTime.Now;
            NumeroTransacao = $"CC-{Guid.NewGuid().ToString().Substring(0, 8)}";
            
            decimal valorFinal = CalcularValorFinal(valor);
            Console.WriteLine($"💳 Cartão: Processando {valorFinal:C} em {Parcelas}x de {valorFinal/Parcelas:C}");
            
            return true;
        }
        
        protected override decimal ObterTaxa(decimal valor)
        {
            // Taxa de 2.5%
            return valor * 0.025m;
        }
    }

    public class Pix : Pagamento
    {
        public string ChavePix { get; set; }
        
        public Pix(string chavePix)
        {
            ChavePix = chavePix;
        }
        
        public override bool Processar(decimal valor)
        {
            ValorBase = valor;
            DataProcessamento = DateTime.Now;
            NumeroTransacao = $"PIX-{Guid.NewGuid().ToString().Substring(0, 8)}";
            
            decimal valorFinal = CalcularValorFinal(valor);
            Console.WriteLine($"⚡ PIX: Processamento instantâneo de {valorFinal:C}");
            
            return true;
        }
        
        protected override decimal ObterTaxa(decimal valor)
        {
            // PIX sem taxa!
            return 0;
        }
    }

    public class Boleto : Pagamento
    {
        public string CodigoBarras { get; private set; }
        public DateTime Vencimento { get; set; }
        
        public Boleto(DateTime vencimento)
        {
            Vencimento = vencimento;
        }
        
        public override bool Processar(decimal valor)
        {
            ValorBase = valor;
            DataProcessamento = DateTime.Now;
            NumeroTransacao = $"BOL-{Guid.NewGuid().ToString().Substring(0, 8)}";
            CodigoBarras = GerarCodigoBarras();
            
            decimal valorFinal = CalcularValorFinal(valor);
            Console.WriteLine($"📄 Boleto: {valorFinal:C} - Vencimento: {Vencimento:dd/MM/yyyy}");
            Console.WriteLine($"   Código de barras: {CodigoBarras}");
            
            return true;
        }
        
        protected override decimal ObterTaxa(decimal valor)
        {
            // Taxa fixa de R$ 2,00
            return 2.00m;
        }
        
        private string GerarCodigoBarras()
        {
            return $"34191.79001 01043.510047 91020.150008 {new Random().Next(1, 10)} 88260000{(int)(ValorBase * 100):D10}";
        }
    }

    public class PayPal : Pagamento
    {
        public string EmailPayPal { get; set; }
        
        public PayPal(string emailPayPal)
        {
            EmailPayPal = emailPayPal;
        }
        
        public override bool Processar(decimal valor)
        {
            ValorBase = valor;
            DataProcessamento = DateTime.Now;
            NumeroTransacao = $"PP-{Guid.NewGuid().ToString().Substring(0, 8)}";
            
            decimal valorFinal = CalcularValorFinal(valor);
            Console.WriteLine($"🅿️  PayPal: {valorFinal:C} para {EmailPayPal}");
            
            return true;
        }
        
        protected override decimal ObterTaxa(decimal valor)
        {
            // Taxa de 3.5%
            return valor * 0.035m;
        }
    }

    // ===== PROCESSADOR DE PAGAMENTOS =====
    
    public class ProcessadorPagamentos
    {
        // Método que recebe tipo base (polimorfismo!)
        public void ProcessarPagamento(Pagamento pagamento, decimal valor)
        {
            Console.WriteLine($"\n--- Processando Pagamento ---");
            
            // Pattern matching para obter informações específicas
            string tipoPagamento = pagamento switch
            {
                CartaoCredito cc => $"Cartão **** **** **** {cc.NumeroCartao.Substring(cc.NumeroCartao.Length - 4)}",
                Pix pix => $"PIX - Chave: {pix.ChavePix}",
                Boleto bol => $"Boleto - Vencimento: {bol.Vencimento:dd/MM/yyyy}",
                PayPal pp => $"PayPal - {pp.EmailPayPal}",
                _ => "Pagamento desconhecido"
            };
            
            Console.WriteLine($"Tipo: {tipoPagamento}");
            Console.WriteLine($"Valor: {valor:C}");
            
            // Chama método polimórfico - cada classe executa sua lógica!
            bool sucesso = pagamento.Processar(valor);
            
            if (sucesso)
            {
                Console.WriteLine($"✅ Pagamento aprovado!");
                Console.WriteLine($"Taxa aplicada: {pagamento.CalcularValorFinal(valor) - valor:C}");
                Console.WriteLine($"Valor final: {pagamento.CalcularValorFinal(valor):C}");
                Console.WriteLine(pagamento.GerarRecibo());
            }
            else
            {
                Console.WriteLine($"❌ Pagamento recusado!");
            }
        }
        
        // Processar múltiplos pagamentos (polimorfismo em lista!)
        public void ProcessarLote(List<(Pagamento pagamento, decimal valor)> lote)
        {
            Console.WriteLine($"\n========== PROCESSAMENTO EM LOTE ==========");
            Console.WriteLine($"Total de pagamentos: {lote.Count}\n");
            
            decimal totalProcessado = 0;
            decimal totalTaxas = 0;
            
            foreach (var (pagamento, valor) in lote)
            {
                ProcessarPagamento(pagamento, valor);
                totalProcessado += valor;
                totalTaxas += pagamento.CalcularValorFinal(valor) - valor;
            }
            
            Console.WriteLine($"\n========== RESUMO ==========");
            Console.WriteLine($"Total processado: {totalProcessado:C}");
            Console.WriteLine($"Total em taxas: {totalTaxas:C}");
            Console.WriteLine($"Valor líquido: {totalProcessado - totalTaxas:C}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE PAGAMENTOS COM POLIMORFISMO ===\n");
            
            var processador = new ProcessadorPagamentos();
            
            // ===== TESTE 1: Pagamento individual =====
            Console.WriteLine("📌 TESTE 1: Pagamento Individual");
            
            Pagamento pagamento1 = new CartaoCredito("1234-5678-9012-3456", parcelas: 3);
            processador.ProcessarPagamento(pagamento1, 300.00m);
            
            // ===== TESTE 2: Lista polimórfica =====
            Console.WriteLine("\n\n📌 TESTE 2: Lista Polimórfica");
            
            List<Pagamento> pagamentos = new List<Pagamento>
            {
                new CartaoCredito("1111-2222-3333-4444", 1),
                new Pix("joao@email.com"),
                new Boleto(DateTime.Now.AddDays(7)),
                new PayPal("maria@paypal.com"),
                new CartaoCredito("5555-6666-7777-8888", 6)
            };
            
            // Polimorfismo: mesma interface, comportamentos diferentes!
            foreach (var pg in pagamentos)
            {
                pg.Processar(100.00m);
                Console.WriteLine();
            }
            
            // ===== TESTE 3: Processamento em lote =====
            Console.WriteLine("\n📌 TESTE 3: Processamento em Lote");
            
            var lote = new List<(Pagamento, decimal)>
            {
                (new CartaoCredito("1234-5678", 1), 150.00m),
                (new Pix("cliente@email.com"), 200.00m),
                (new Boleto(DateTime.Now.AddDays(3)), 350.00m),
                (new PayPal("user@paypal.com"), 500.00m)
            };
            
            processador.ProcessarLote(lote);
            
            // ===== TESTE 4: Pattern matching para relatório =====
            Console.WriteLine("\n\n📌 TESTE 4: Relatório com Pattern Matching");
            
            foreach (var pg in pagamentos)
            {
                string descricao = pg switch
                {
                    CartaoCredito { Parcelas: > 1 } cc => 
                        $"Cartão parcelado: {cc.Parcelas}x",
                    CartaoCredito => 
                        "Cartão à vista",
                    Pix => 
                        "PIX - Desconto especial aplicável",
                    Boleto { Vencimento: var v } when v < DateTime.Now.AddDays(3) => 
                        "Boleto com vencimento próximo",
                    Boleto => 
                        "Boleto normal",
                    PayPal => 
                        "PayPal - Proteção ao comprador",
                    _ => 
                        "Pagamento não identificado"
                };
                
                Console.WriteLine($"• {descricao} - Taxa: {pg.CalcularValorFinal(100) - 100:C}");
            }
            
            Console.WriteLine("\n\n✅ Demonstração completa de polimorfismo!");
            Console.ReadKey();
        }
    }
}

/*
 * CONCEITOS DEMONSTRADOS:
 * 
 * 1. POLIMORFISMO:
 *    - Método Processar() funciona diferente em cada classe
 *    - Lista de Pagamento aceita qualquer tipo derivado
 *    - Processador trabalha com tipo base
 * 
 * 2. MÉTODOS VIRTUAIS:
 *    - CalcularValorFinal() pode ser sobrescrito
 *    - ObterTaxa() abstrato - obriga implementação
 * 
 * 3. PATTERN MATCHING:
 *    - Switch expressions para diferentes tipos
 *    - Condições complexas com when
 *    - Desconstrução de propriedades
 * 
 * 4. BOAS PRÁTICAS:
 *    - Classe base abstrata
 *    - Métodos protegidos para herança
 *    - Encapsulamento de lógica específica
 *    - Código limpo e documentado
 */