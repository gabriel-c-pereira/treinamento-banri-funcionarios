// Exercício 3: Delegate Funcional
// Objetivo: Implementar sistema de filtros usando Func<T, bool>

using System;
using System.Linq;
using System.Collections.Generic;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    SISTEMA DE FILTROS COM DELEGATES   ");
Console.WriteLine("═══════════════════════════════════════");

// Lista de números para testar
List<int> numeros = new List<int> { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };

Console.WriteLine($"\n📋 Lista original:");
Console.WriteLine($"   [{string.Join(", ", numeros)}]\n");

// ═══════════════════════════════════════════════════════════
// 1. DEFINIÇÃO DE FILTROS (Func<int, bool>)
// ═══════════════════════════════════════════════════════════

Func<int, bool> ehPar = n => n % 2 == 0;
Func<int, bool> ehMaiorQue20 = n => n > 20;
Func<int, bool> ehMenorQue30 = n => n < 30;
Func<int, bool> ehDivisivelPor5 = n => n % 5 == 0;
Func<int, bool> ehDivisivelPor10 = n => n % 10 == 0;

// ═══════════════════════════════════════════════════════════
// 2. APLICANDO FILTROS INDIVIDUAIS
// ═══════════════════════════════════════════════════════════

Console.WriteLine("🔍 FILTROS INDIVIDUAIS:\n");

var pares = AplicarFiltro(numeros, ehPar);
Console.WriteLine($"1️⃣ Números PARES:");
Console.WriteLine($"   [{string.Join(", ", pares)}]");

var maioresQue20 = AplicarFiltro(numeros, ehMaiorQue20);
Console.WriteLine($"\n2️⃣ Números MAIORES que 20:");
Console.WriteLine($"   [{string.Join(", ", maioresQue20)}]");

var divisivelPor5 = AplicarFiltro(numeros, ehDivisivelPor5);
Console.WriteLine($"\n3️⃣ Números DIVISÍVEIS por 5:");
Console.WriteLine($"   [{string.Join(", ", divisivelPor5)}]");

// ═══════════════════════════════════════════════════════════
// 3. COMBINANDO MÚLTIPLOS FILTROS
// ═══════════════════════════════════════════════════════════

Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("🔗 FILTROS COMBINADOS:\n");

// Combinar: par E maior que 20
var paresEMaioresQue20 = CombinarFiltros(numeros, ehPar, ehMaiorQue20);
Console.WriteLine($"4️⃣ Pares E Maiores que 20:");
Console.WriteLine($"   [{string.Join(", ", paresEMaioresQue20)}]");

// Combinar: maior que 20 E menor que 30
var entre20e30 = CombinarFiltros(numeros, ehMaiorQue20, ehMenorQue30);
Console.WriteLine($"\n5️⃣ Maior que 20 E Menor que 30:");
Console.WriteLine($"   [{string.Join(", ", entre20e30)}]");

// Combinar 3 filtros: par E divisível por 10 E maior que 20
var filtroTriplo = CombinarFiltrosMultiplos(numeros, 
    ehPar, ehDivisivelPor10, ehMaiorQue20);
Console.WriteLine($"\n6️⃣ Par E Divisível por 10 E Maior que 20:");
Console.WriteLine($"   [{string.Join(", ", filtroTriplo)}]");

Console.WriteLine("\n═══════════════════════════════════════");

// ═══════════════════════════════════════════════════════════
// FUNÇÕES AUXILIARES
// ═══════════════════════════════════════════════════════════

// Aplica um único filtro
List<int> AplicarFiltro(List<int> lista, Func<int, bool> filtro)
{
    return lista.Where(filtro).ToList();
}

// Combina dois filtros com operador E (AND)
List<int> CombinarFiltros(List<int> lista, Func<int, bool> filtro1, Func<int, bool> filtro2)
{
    return lista.Where(n => filtro1(n) && filtro2(n)).ToList();
}

// Combina múltiplos filtros
List<int> CombinarFiltrosMultiplos(List<int> lista, params Func<int, bool>[] filtros)
{
    var resultado = lista.AsEnumerable();
    
    foreach (var filtro in filtros)
    {
        resultado = resultado.Where(filtro);
    }
    
    return resultado.ToList();
}

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. FUNC<T, TRESULT>:
 *    - Delegate genérico que retorna valor
 *    - Func<int, bool>: recebe int, retorna bool
 *    - Perfeito para criar predicados (filtros)
 * 
 * 2. PREDICADOS:
 *    - Função que retorna true/false
 *    - Usado para testes condicionais
 *    - Exemplo: n => n % 2 == 0 (é par?)
 * 
 * 3. COMPOSIÇÃO DE FUNÇÕES:
 *    - Combinar múltiplos filtros
 *    - Operador && (E lógico)
 *    - Operador || (OU lógico)
 * 
 * 4. PARAMS KEYWORD:
 *    - params Func<int, bool>[] filtros
 *    - Permite passar N argumentos
 *    - Array é criado automaticamente
 * 
 * 5. WHERE ENCADEADO:
 *    - resultado.Where(filtro1).Where(filtro2)
 *    - Aplica filtros sequencialmente
 *    - Mais eficiente que combinação manual
 * 
 * PADRÕES DE DESIGN:
 * ✅ Strategy Pattern: diferentes estratégias de filtro
 * ✅ Composition: combinar múltiplas operações
 * ✅ Higher-Order Functions: funções que recebem funções
 * 
 * APLICAÇÕES PRÁTICAS:
 * • Validação de dados
 * • Busca avançada
 * • Sistemas de permissões
 * • Regras de negócio configuráveis
 */
