// Exercício 3: Join e GroupBy com LINQ
// Objetivo: Relacionar dados de múltiplas coleções usando Join e GroupBy

using System;
using System.Linq;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("      LINQ - JOIN E GROUPBY            ");
Console.WriteLine("═══════════════════════════════════════");

// Classes
class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
}

class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public int CategoriaId { get; set; }
    public decimal Preco { get; set; }
}

// Dados de exemplo
var categorias = new List<Categoria>
{
    new Categoria { Id = 1, Nome = "Eletrônicos" },
    new Categoria { Id = 2, Nome = "Periféricos" },
    new Categoria { Id = 3, Nome = "Acessórios" },
    new Categoria { Id = 4, Nome = "Computadores" }
};

var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Notebook Dell", CategoriaId = 4, Preco = 3500.00m },
    new Produto { Id = 2, Nome = "Mouse Logitech", CategoriaId = 2, Preco = 85.00m },
    new Produto { Id = 3, Nome = "Teclado Mecânico", CategoriaId = 2, Preco = 450.00m },
    new Produto { Id = 4, Nome = "Monitor LG 24\"", CategoriaId = 1, Preco = 950.00m },
    new Produto { Id = 5, Nome = "Webcam HD", CategoriaId = 1, Preco = 280.00m },
    new Produto { Id = 6, Nome = "Mouse Pad", CategoriaId = 3, Preco = 35.00m },
    new Produto { Id = 7, Nome = "Hub USB", CategoriaId = 3, Preco = 65.00m },
    new Produto { Id = 8, Nome = "Headset Gamer", CategoriaId = 2, Preco = 320.00m },
    new Produto { Id = 9, Nome = "SSD 500GB", CategoriaId = 1, Preco = 380.00m },
    new Produto { Id = 10, Nome = "PC Gamer", CategoriaId = 4, Preco = 5200.00m }
};

// ═══════════════════════════════════════════════════════════
// 1. JOIN - Relacionar Produtos com Categorias
// ═══════════════════════════════════════════════════════════

var produtosComCategoria = produtos
    .Join(
        categorias,                          // Segunda coleção
        produto => produto.CategoriaId,      // Chave do produto
        categoria => categoria.Id,           // Chave da categoria
        (produto, categoria) => new          // Resultado
        {
            ProdutoNome = produto.Nome,
            CategoriaNome = categoria.Nome,
            Preco = produto.Preco
        }
    )
    .OrderBy(x => x.CategoriaNome)
    .ThenBy(x => x.ProdutoNome)
    .ToList();

Console.WriteLine("\n1️⃣ JOIN - Produtos com suas Categorias:\n");
Console.WriteLine($"   {"Produto",-25} {"Categoria",-15} {"Preço",12}");
Console.WriteLine($"   {new string('-', 55)}");
foreach (var item in produtosComCategoria)
{
    Console.WriteLine($"   {item.ProdutoNome,-25} {item.CategoriaNome,-15} R$ {item.Preco,8:N2}");
}

// ═══════════════════════════════════════════════════════════
// 2. GROUPBY - Agrupar por Categoria
// ═══════════════════════════════════════════════════════════

var produtosPorCategoria = produtos
    .Join(categorias,
          p => p.CategoriaId,
          c => c.Id,
          (p, c) => new { Produto = p, Categoria = c })
    .GroupBy(x => x.Categoria.Nome)
    .Select(g => new
    {
        Categoria = g.Key,
        Produtos = g.Select(x => x.Produto.Nome).ToList(),
        Quantidade = g.Count(),
        ValorTotal = g.Sum(x => x.Produto.Preco),
        ValorMedio = g.Average(x => x.Produto.Preco)
    })
    .OrderByDescending(x => x.Quantidade)
    .ToList();

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("2️⃣ GROUPBY - Produtos agrupados por Categoria:\n");

foreach (var grupo in produtosPorCategoria)
{
    Console.WriteLine($"   📂 {grupo.Categoria}");
    Console.WriteLine($"      Quantidade: {grupo.Quantidade} produtos");
    Console.WriteLine($"      Valor total: R$ {grupo.ValorTotal:N2}");
    Console.WriteLine($"      Valor médio: R$ {grupo.ValorMedio:N2}");
    Console.WriteLine($"      Produtos:");
    foreach (var produto in grupo.Produtos)
    {
        Console.WriteLine($"         • {produto}");
    }
    Console.WriteLine();
}

// ═══════════════════════════════════════════════════════════
// 3. CONTAGEM DE PRODUTOS POR CATEGORIA
// ═══════════════════════════════════════════════════════════

var contagemPorCategoria = produtos
    .GroupBy(p => p.CategoriaId)
    .Join(categorias,
          g => g.Key,
          c => c.Id,
          (g, c) => new
          {
              Categoria = c.Nome,
              Quantidade = g.Count(),
              PrecoMaior = g.Max(p => p.Preco),
              PrecoMenor = g.Min(p => p.Preco)
          })
    .OrderByDescending(x => x.Quantidade)
    .ToList();

Console.WriteLine("───────────────────────────────────────");
Console.WriteLine("3️⃣ CONTAGEM e ESTATÍSTICAS por Categoria:\n");
Console.WriteLine($"   {"Categoria",-15} {"Qtd",5}  {"Menor",12}  {"Maior",12}");
Console.WriteLine($"   {new string('-', 50)}");

foreach (var item in contagemPorCategoria)
{
    Console.WriteLine($"   {item.Categoria,-15} {item.Quantidade,5}  " +
                     $"R$ {item.PrecoMenor,8:N2}  R$ {item.PrecoMaior,8:N2}");
}

// ═══════════════════════════════════════════════════════════
// BÔNUS: Categoria com mais produtos e maior valor
// ═══════════════════════════════════════════════════════════

var categoriaMaisProdutos = produtosPorCategoria
    .OrderByDescending(x => x.Quantidade)
    .First();

var categoriaMaiorValor = produtosPorCategoria
    .OrderByDescending(x => x.ValorTotal)
    .First();

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("🏆 DESTAQUES:");
Console.WriteLine($"\n   Mais produtos: {categoriaMaisProdutos.Categoria}");
Console.WriteLine($"   ({categoriaMaisProdutos.Quantidade} produtos)");
Console.WriteLine($"\n   Maior valor total: {categoriaMaiorValor.Categoria}");
Console.WriteLine($"   (R$ {categoriaMaiorValor.ValorTotal:N2})");

Console.WriteLine("\n═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. JOIN:
 *    - Relaciona duas coleções
 *    - Sintaxe: collection1.Join(collection2, key1, key2, resultado)
 *    - Similar ao SQL JOIN
 *    - Retorna apenas registros com correspondência
 * 
 * 2. GROUPBY:
 *    - Agrupa elementos por chave
 *    - g.Key: valor da chave de agrupamento
 *    - Permite agregações: Count, Sum, Average, etc
 *    - Útil para relatórios e estatísticas
 * 
 * 3. THENBY:
 *    - Ordenação secundária
 *    - Usado após OrderBy
 *    - ThenByDescending para decrescente
 * 
 * 4. OPERAÇÕES ANINHADAS:
 *    - Join dentro de GroupBy
 *    - GroupBy dentro de Select
 *    - Permite consultas complexas
 * 
 * 5. MATERIALIZANDO GRUPOS:
 *    - ToList() dentro do Select
 *    - Converte IGrouping em lista
 *    - Facilita navegação posterior
 * 
 * SINTAXE DE CONSULTA (alternativa):
 * 
 * var query = from p in produtos
 *             join c in categorias on p.CategoriaId equals c.Id
 *             group p by c.Nome into g
 *             select new { Categoria = g.Key, Qtd = g.Count() };
 * 
 * DICAS:
 * ✅ Use Join para relacionar dados
 * ✅ Use GroupBy para agregações
 * ✅ Combine Join + GroupBy para relatórios
 * ✅ OrderBy + ThenBy para ordenação múltipla
 */
