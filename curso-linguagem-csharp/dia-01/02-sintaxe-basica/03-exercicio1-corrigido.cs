// Exercício 1: Calculadora Básica
// Objetivo: Praticar declaração de variáveis e operadores aritméticos

int numero1 = 20;
int numero2 = 3;

// Operações básicas
int soma = numero1 + numero2;
int subtracao = numero1 - numero2;
int multiplicacao = numero1 * numero2;
int divisao = numero1 / numero2;
int resto = numero1 % numero2;

// Exibindo os resultados
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("       CALCULADORA BÁSICA              ");
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine($"Soma: {numero1} + {numero2} = {soma}");
Console.WriteLine($"Subtração: {numero1} - {numero2} = {subtracao}");
Console.WriteLine($"Multiplicação: {numero1} * {numero2} = {multiplicacao}");
Console.WriteLine($"Divisão: {numero1} / {numero2} = {divisao}");
Console.WriteLine($"Resto: {numero1} % {numero2} = {resto}");
Console.WriteLine("═══════════════════════════════════════");

// DESAFIO EXTRA: Divisão com valor decimal
double divisaoDecimal = (double)numero1 / numero2;
Console.WriteLine($"\nDivisão decimal: {numero1} / {numero2} = {divisaoDecimal:F2}");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. OPERADORES ARITMÉTICOS:
 *    + (soma), - (subtração), * (multiplicação), / (divisão), % (módulo/resto)
 * 
 * 2. DIVISÃO INTEIRA vs DECIMAL:
 *    - int / int = int (resultado truncado)
 *    - double / int = double (resultado decimal)
 *    - Use cast (double) para forçar conversão
 * 
 * 3. FORMATAÇÃO:
 *    - :F2 = Fixed-point com 2 casas decimais
 *    - Interpolação: $"texto {variavel}"
 */
