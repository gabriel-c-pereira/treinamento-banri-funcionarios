// Exercício 2: Agregação com LINQ
// Objetivo: Usar operações de agregação (Sum, Average, Count, etc)

using System;
using System.Linq;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("       LINQ - AGREGAÇÃO DE DADOS       ");
Console.WriteLine("═══════════════════════════════════════");

// Classe Venda
class Venda
{
    public string Produto { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
    
    public override string ToString() => 
        $"{Produto}: {Quantidade}x R$ {PrecoUnitario:F2} = R$ {Total:F2}";
}

// Lista de vendas
var vendas = new List<Venda>
{
    new Venda { Produto = "Notebook", Quantidade = 5, PrecoUnitario = 3200.00m },
    new Venda { Produto = "Mouse", Quantidade = 25, PrecoUnitario = 45.00m },
    new Venda { Produto = "Teclado", Quantidade = 15, PrecoUnitario = 120.00m },
    new Venda { Produto = "Monitor", Quantidade = 8, PrecoUnitario = 850.00m },
    new Venda { Produto = "Mouse", Quantidade = 10, PrecoUnitario = 45.00m },
    new Venda { Produto = "Webcam", Quantidade = 12, PrecoUnitario = 180.00m },
    new Venda { Produto = "Notebook", Quantidade = 3, PrecoUnitario = 3200.00m },
    new Venda { Produto = "HD Externo", Quantidade = 20, PrecoUnitario = 280.00m }
};

Console.WriteLine("\n📋 Lista de vendas:");
foreach (var v in vendas)
    Console.WriteLine($"   • {v}");

Console.WriteLine("\n═══════════════════════════════════════");

// ═══════════════════════════════════════════════════════════
// 1. TOTAL DE VENDAS
// ═══════════════════════════════════════════════════════════

decimal totalVendas = vendas.Sum(v => v.Total);
Console.WriteLine($"\n1️⃣ TOTAL DE VENDAS:");
Console.WriteLine($"   R$ {totalVendas:N2}");

// ═══════════════════════════════════════════════════════════
// 2. MÉDIA DE VENDAS POR PRODUTO
// ═══════════════════════════════════════════════════════════

var mediaPorProduto = vendas
    .GroupBy(v => v.Produto)
    .Select(g => new
    {
        Produto = g.Key,
        MediaValor = g.Average(v => v.Total),
        TotalVendas = g.Sum(v => v.Total),
        Quantidade = g.Sum(v => v.Quantidade)
    })
    .OrderByDescending(x => x.TotalVendas)
    .ToList();

Console.WriteLine($"\n2️⃣ MÉDIA DE VENDAS POR PRODUTO:");
Console.WriteLine($"   {"Produto",-15} {"Qtd",5}  {"Média",12}  {"Total",12}");
Console.WriteLine($"   {new string('-', 50)}");
foreach (var item in mediaPorProduto)
{
    Console.WriteLine($"   {item.Produto,-15} {item.Quantidade,5}  " +
                     $"R$ {item.MediaValor,8:N2}  R$ {item.TotalVendas,8:N2}");
}

// ═══════════════════════════════════════════════════════════
// 3. PRODUTO MAIS VENDIDO
// ═══════════════════════════════════════════════════════════

var produtoMaisVendido = vendas
    .GroupBy(v => v.Produto)
    .Select(g => new
    {
        Produto = g.Key,
        QuantidadeTotal = g.Sum(v => v.Quantidade),
        ValorTotal = g.Sum(v => v.Total)
    })
    .OrderByDescending(x => x.QuantidadeTotal)
    .First();

Console.WriteLine($"\n3️⃣ PRODUTO MAIS VENDIDO:");
Console.WriteLine($"   🏆 {produtoMaisVendido.Produto}");
Console.WriteLine($"   Quantidade: {produtoMaisVendido.QuantidadeTotal} unidades");
Console.WriteLine($"   Valor total: R$ {produtoMaisVendido.ValorTotal:N2}");

// ═══════════════════════════════════════════════════════════
// BÔNUS: Estatísticas gerais
// ═══════════════════════════════════════════════════════════

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("📊 ESTATÍSTICAS GERAIS:");

int totalTransacoes = vendas.Count();
int totalItensVendidos = vendas.Sum(v => v.Quantidade);
decimal vendaMedia = vendas.Average(v => v.Total);
decimal maiorVenda = vendas.Max(v => v.Total);
decimal menorVenda = vendas.Min(v => v.Total);
int produtosDistintos = vendas.Select(v => v.Produto).Distinct().Count();

Console.WriteLine($"   Transações: {totalTransacoes}");
Console.WriteLine($"   Itens vendidos: {totalItensVendidos}");
Console.WriteLine($"   Produtos distintos: {produtosDistintos}");
Console.WriteLine($"   Venda média: R$ {vendaMedia:N2}");
Console.WriteLine($"   Maior venda: R$ {maiorVenda:N2}");
Console.WriteLine($"   Menor venda: R$ {menorVenda:N2}");

Console.WriteLine("\n═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. OPERAÇÕES DE AGREGAÇÃO:
 *    - Sum():     Soma de valores
 *    - Average(): Média aritmética
 *    - Count():   Contagem de elementos
 *    - Max():     Valor máximo
 *    - Min():     Valor mínimo
 * 
 * 2. GROUPBY:
 *    - Agrupa elementos por chave
 *    - Retorna IGrouping<TKey, TElement>
 *    - Key: chave do agrupamento
 *    - Permite agregações por grupo
 * 
 * 3. SELECT APÓS GROUPBY:
 *    - g.Key: chave do grupo
 *    - g.Sum(), g.Average(), etc: agregações do grupo
 *    - Cria objetos anônimos com resumo
 * 
 * 4. DISTINCT:
 *    - Remove duplicatas
 *    - Útil para contar itens únicos
 *    - Exemplo: produtos.Distinct().Count()
 * 
 * 5. FIRST vs SINGLE:
 *    - First(): retorna primeiro elemento (erro se vazio)
 *    - FirstOrDefault(): retorna default se vazio
 *    - Single(): retorna único elemento (erro se 0 ou >1)
 * 
 * FORMATAÇÃO:
 * - :N2 = Número com separador de milhares e 2 decimais
 * - {valor,8} = Alinhamento em 8 caracteres
 * - {texto,-15} = Alinhamento à esquerda em 15 chars
 */
