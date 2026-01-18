// Exercício 1: Array Básico
// Objetivo: Praticar arrays, entrada de dados e cálculos básicos

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    ARRAY BÁSICO - SOMA E MÉDIA        ");
Console.WriteLine("═══════════════════════════════════════");

// Declaração do array
int[] numeros = new int[5];
int soma = 0;

// Preenchimento do array
Console.WriteLine("\nDigite 5 números inteiros:");
for (int i = 0; i < numeros.Length; i++)
{
    Console.Write($"Número {i + 1}: ");
    numeros[i] = int.Parse(Console.ReadLine() ?? "0");
    soma += numeros[i];
}

// Cálculo da média
double media = (double)soma / numeros.Length;

// Exibição dos resultados
Console.WriteLine("\n───────────────────────────────────────");
Console.WriteLine("RESULTADOS:");
Console.WriteLine($"Valores digitados: [{string.Join(", ", numeros)}]");
Console.WriteLine($"Soma: {soma}");
Console.WriteLine($"Média: {media:F2}");
Console.WriteLine("═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. ARRAYS:
 *    - int[] numeros = new int[5];  // Declara array com 5 posições
 *    - Índices começam em 0
 *    - numeros.Length retorna o tamanho do array
 * 
 * 2. LOOP FOR:
 *    - for (int i = 0; i < numeros.Length; i++)
 *    - Percorre todos os elementos do array
 * 
 * 3. CONVERSÃO:
 *    - (double)soma converte int para double
 *    - Necessário para obter média com decimais
 * 
 * 4. STRING.JOIN:
 *    - string.Join(", ", numeros) junta elementos com vírgula
 *    - Útil para exibir arrays formatados
 */
