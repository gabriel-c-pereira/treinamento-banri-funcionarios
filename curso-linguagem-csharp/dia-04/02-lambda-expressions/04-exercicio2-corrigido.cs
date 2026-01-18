// Exercício 2: LINQ Simples
// Objetivo: Usar LINQ com expressões lambda para manipular coleções

using System;
using System.Linq;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("        LINQ COM LAMBDAS               ");
Console.WriteLine("═══════════════════════════════════════");

// Lista de números para trabalhar
List<int> numeros = new List<int> { 1, 15, 3, 22, 5, 18, 7, 30, 9, 12, 25, 8 };

Console.WriteLine($"\n📋 Lista original: [{string.Join(", ", numeros)}]");
Console.WriteLine("───────────────────────────────────────");

// 1. Filtrar números pares
var numerosPares = numeros.Where(n => n % 2 == 0).ToList();
Console.WriteLine($"\n1️⃣ Números PARES:");
Console.WriteLine($"   [{string.Join(", ", numerosPares)}]");

// 2. Ordenar em ordem crescente
var numerosOrdenados = numeros.OrderBy(n => n).ToList();
Console.WriteLine($"\n2️⃣ Números ORDENADOS (crescente):");
Console.WriteLine($"   [{string.Join(", ", numerosOrdenados)}]");

// 3. Filtrar números maiores que 10
var maioresQueDez = numeros.Where(n => n > 10).ToList();
Console.WriteLine($"\n3️⃣ Números MAIORES que 10:");
Console.WriteLine($"   [{string.Join(", ", maioresQueDez)}]");

// 4. Calcular soma dos números maiores que 10
int soma = numeros.Where(n => n > 10).Sum();
Console.WriteLine($"\n4️⃣ SOMA dos números maiores que 10:");
Console.WriteLine($"   {soma}");

// BÔNUS: Combinando operações
Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("🎁 BÔNUS - Operações Combinadas:");

var paresOrdenadosMaioresQue10 = numeros
    .Where(n => n > 10)      // Filtra maiores que 10
    .Where(n => n % 2 == 0)  // Filtra pares
    .OrderBy(n => n)         // Ordena
    .ToList();

Console.WriteLine($"   Pares > 10 ordenados: [{string.Join(", ", paresOrdenadosMaioresQue10)}]");

// Estatísticas
int maior = numeros.Max();
int menor = numeros.Min();
double media = numeros.Average();
int quantidade = numeros.Count();

Console.WriteLine($"\n📊 ESTATÍSTICAS:");
Console.WriteLine($"   Maior: {maior}");
Console.WriteLine($"   Menor: {menor}");
Console.WriteLine($"   Média: {media:F2}");
Console.WriteLine($"   Quantidade: {quantidade}");

Console.WriteLine("\n═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. LINQ (Language Integrated Query):
 *    - Consultas integradas à linguagem
 *    - Trabalha com IEnumerable<T>
 *    - Usa expressões lambda
 * 
 * 2. MÉTODOS LINQ PRINCIPAIS:
 *    - Where():    Filtra elementos (condição)
 *    - OrderBy():  Ordena crescente
 *    - Select():   Projeta/transforma elementos
 *    - Sum():      Soma valores
 *    - Count():    Conta elementos
 *    - Max/Min():  Maior/menor valor
 *    - Average():  Média dos valores
 * 
 * 3. ENCADEAMENTO (METHOD CHAINING):
 *    - numeros.Where().OrderBy().ToList()
 *    - Cada método retorna IEnumerable
 *    - Permite combinar múltiplas operações
 * 
 * 4. EXECUÇÃO DIFERIDA (LAZY EVALUATION):
 *    - LINQ não executa imediatamente
 *    - Executa quando necessário (ToList, foreach, etc)
 *    - Otimiza performance
 * 
 * DICAS:
 * ✅ Use ToList() para materializar resultado
 * ✅ Combine Where para múltiplos filtros
 * ✅ OrderBy para crescente, OrderByDescending para decrescente
 */
