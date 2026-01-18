// Exercício 1: Filtragem e Projeção com LINQ
// Objetivo: Filtrar, projetar e ordenar dados usando LINQ

using System;
using System.Linq;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    LINQ - FILTRAGEM E PROJEÇÃO        ");
Console.WriteLine("═══════════════════════════════════════");

// Classe Produto
class Produto
{
    public string Nome { get; set; } = "";
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = "";
    
    public override string ToString() => $"{Nome} - R$ {Preco:F2} ({Categoria})";
}

// Lista de produtos
var produtos = new List<Produto>
{
    new Produto { Nome = "Mouse", Preco = 45.00m, Categoria = "Periféricos" },
    new Produto { Nome = "Teclado", Preco = 120.00m, Categoria = "Periféricos" },
    new Produto { Nome = "Monitor", Preco = 850.00m, Categoria = "Monitores" },
    new Produto { Nome = "Webcam", Preco = 35.00m, Categoria = "Periféricos" },
    new Produto { Nome = "HD Externo", Preco = 280.00m, Categoria = "Armazenamento" },
    new Produto { Nome = "SSD 500GB", Preco = 350.00m, Categoria = "Armazenamento" },
    new Produto { Nome = "Notebook", Preco = 3200.00m, Categoria = "Computadores" },
    new Produto { Nome = "Mouse Pad", Preco = 25.00m, Categoria = "Acessórios" }
};

Console.WriteLine("\n📋 Lista completa de produtos:");
foreach (var p in produtos)
    Console.WriteLine($"   • {p}");

// ═══════════════════════════════════════════════════════════
// 1. FILTRAR produtos com preço > 50
// ═══════════════════════════════════════════════════════════

var produtosCaros = produtos
    .Where(p => p.Preco > 50)
    .ToList();

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("1️⃣ Produtos com PREÇO > R$ 50:");
foreach (var p in produtosCaros)
    Console.WriteLine($"   • {p}");

// ═══════════════════════════════════════════════════════════
// 2. PROJETAR apenas Nome e Preço
// ═══════════════════════════════════════════════════════════

var nomeEPreco = produtos
    .Select(p => new { p.Nome, p.Preco })
    .ToList();

Console.WriteLine("\n2️⃣ Projeção (apenas Nome e Preço):");
foreach (var item in nomeEPreco)
    Console.WriteLine($"   • {item.Nome}: R$ {item.Preco:F2}");

// ═══════════════════════════════════════════════════════════
// 3. ORDENAR por preço crescente
// ═══════════════════════════════════════════════════════════

var produtosOrdenados = produtos
    .OrderBy(p => p.Preco)
    .ToList();

Console.WriteLine("\n3️⃣ Produtos ORDENADOS por preço (crescente):");
foreach (var p in produtosOrdenados)
    Console.WriteLine($"   • {p}");

// ═══════════════════════════════════════════════════════════
// BÔNUS: Combinando operações
// ═══════════════════════════════════════════════════════════

var resultadoCombinado = produtos
    .Where(p => p.Preco > 50)           // Filtrar
    .OrderBy(p => p.Preco)              // Ordenar
    .Select(p => new                     // Projetar
    { 
        p.Nome, 
        PrecoFormatado = $"R$ {p.Preco:F2}" 
    })
    .ToList();

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("🎁 BÔNUS - Operação Combinada:");
Console.WriteLine("   (Preço > 50, Ordenado, Nome + Preço)");
foreach (var item in resultadoCombinado)
    Console.WriteLine($"   • {item.Nome}: {item.PrecoFormatado}");

Console.WriteLine("\n═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. WHERE - FILTRAÇÃO:
 *    - Filtra elementos baseado em condição
 *    - Retorna IEnumerable<T>
 *    - Exemplo: Where(p => p.Preco > 50)
 * 
 * 2. SELECT - PROJEÇÃO:
 *    - Transforma/projeta elementos
 *    - Cria novos objetos anônimos ou tipos
 *    - Exemplo: Select(p => new { p.Nome, p.Preco })
 * 
 * 3. ORDERBY - ORDENAÇÃO:
 *    - OrderBy(): crescente
 *    - OrderByDescending(): decrescente
 *    - ThenBy(): ordenação secundária
 * 
 * 4. OBJETOS ANÔNIMOS:
 *    - new { Nome = "...", Preco = 10 }
 *    - Propriedades inferidas automaticamente
 *    - Útil para projeções temporárias
 * 
 * 5. METHOD CHAINING:
 *    - Encadear múltiplos métodos LINQ
 *    - Cada método retorna IEnumerable
 *    - Permite operações fluentes
 * 
 * SINTAXE DE CONSULTA (alternativa):
 * 
 * var resultado = from p in produtos
 *                 where p.Preco > 50
 *                 orderby p.Preco
 *                 select new { p.Nome, p.Preco };
 */
