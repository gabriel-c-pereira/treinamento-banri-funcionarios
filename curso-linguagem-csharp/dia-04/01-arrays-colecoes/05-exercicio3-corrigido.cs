// Exercício 3: Array Multidimensional (Matriz 3x3)
// Objetivo: Trabalhar com matrizes bidimensionais

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("      MATRIZ 3x3 - ANÁLISE             ");
Console.WriteLine("═══════════════════════════════════════");

// Declaração e preenchimento da matriz
int[,] matriz = new int[3, 3];

Console.WriteLine("\nPreencha a matriz 3x3:");
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.Write($"Elemento [{i},{j}]: ");
        matriz[i, j] = int.Parse(Console.ReadLine() ?? "0");
    }
}

// Exibição da matriz
Console.WriteLine("\n╔═════════════════════════════════════╗");
Console.WriteLine("║          MATRIZ 3x3                 ║");
Console.WriteLine("╠═════════════════════════════════════╣");
for (int i = 0; i < 3; i++)
{
    Console.Write("║  ");
    for (int j = 0; j < 3; j++)
    {
        Console.Write($"{matriz[i, j],5}  ");
    }
    Console.WriteLine("║");
}
Console.WriteLine("╚═════════════════════════════════════╝");

// Cálculo da soma por linha
Console.WriteLine("\n📊 SOMA POR LINHA:");
for (int i = 0; i < 3; i++)
{
    int somaLinha = 0;
    for (int j = 0; j < 3; j++)
    {
        somaLinha += matriz[i, j];
    }
    Console.WriteLine($"   Linha {i + 1}: {somaLinha}");
}

// Cálculo da soma por coluna
Console.WriteLine("\n📊 SOMA POR COLUNA:");
for (int j = 0; j < 3; j++)
{
    int somaColuna = 0;
    for (int i = 0; i < 3; i++)
    {
        somaColuna += matriz[i, j];
    }
    Console.WriteLine($"   Coluna {j + 1}: {somaColuna}");
}

// Soma total
int somaTotal = 0;
for (int i = 0; i < 3; i++)
    for (int j = 0; j < 3; j++)
        somaTotal += matriz[i, j];

Console.WriteLine($"\n📈 SOMA TOTAL: {somaTotal}");
Console.WriteLine("═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. ARRAY MULTIDIMENSIONAL:
 *    - int[,] matriz = new int[3, 3];  // Matriz 3x3
 *    - Acesso: matriz[linha, coluna]
 *    - Diferente de array jagged (int[][])
 * 
 * 2. LOOPS ANINHADOS:
 *    - Loop externo: percorre linhas (i)
 *    - Loop interno: percorre colunas (j)
 *    - Necessário para processar todos os elementos
 * 
 * 3. FORMATAÇÃO:
 *    - {valor,5} alinha valor em 5 caracteres
 *    - Útil para exibir matrizes alinhadas
 * 
 * 4. DIMENSÕES:
 *    - GetLength(0) retorna número de linhas
 *    - GetLength(1) retorna número de colunas
 * 
 * EXEMPLO DE MATRIZ:
 * ┌─────────────┐
 * │  1   2   3  │
 * │  4   5   6  │
 * │  7   8   9  │
 * └─────────────┘
 */
