// Exercício 5 Corrigido: Tabuada Completa
// Arquivo: Program.cs

using System;

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("        TABUADA COMPLETA           ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

Console.Write("Digite um número (1-10): ");
int numero = int.Parse(Console.ReadLine());

// Validação
if (numero < 1 || numero > 10)
{
    Console.WriteLine("❌ Número deve estar entre 1 e 10!");
    return;
}

Console.WriteLine();
Console.WriteLine($"═══════════════════════════════════");
Console.WriteLine($"       TABUADA DO {numero}             ");
Console.WriteLine($"═══════════════════════════════════");
Console.WriteLine();

// Loop básico: tabuada de multiplicação
for (int i = 1; i <= 10; i++)
{
    int resultado = numero * i;
    Console.WriteLine($"{numero} × {i,2} = {resultado,3}");
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// DESAFIO: Tabuadas de 1 a 10 (Loops Aninhados)
// ═══════════════════════════════════════════════════════════

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("   TODAS AS TABUADAS (1 a 10)     ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

for (int tabuada = 1; tabuada <= 10; tabuada++)
{
    Console.WriteLine($"--- Tabuada do {tabuada} ---");
    
    for (int multiplicador = 1; multiplicador <= 10; multiplicador++)
    {
        int resultado = tabuada * multiplicador;
        Console.WriteLine($"{tabuada} × {multiplicador,2} = {resultado,3}");
    }
    
    Console.WriteLine();
}

// ═══════════════════════════════════════════════════════════
// DESAFIO: Formato de Tabela
// ═══════════════════════════════════════════════════════════

Console.WriteLine("═══════════════════════════════════════════════════════════════════════");
Console.WriteLine("                           TABELA DE MULTIPLICAÇÃO                      ");
Console.WriteLine("═══════════════════════════════════════════════════════════════════════");
Console.WriteLine();

// Cabeçalho
Console.Write("    |");
for (int col = 1; col <= 10; col++)
{
    Console.Write($"{col,4} ");
}
Console.WriteLine();
Console.WriteLine("─────" + new string('─', 50));

// Corpo da tabela
for (int linha = 1; linha <= 10; linha++)
{
    Console.Write($" {linha,2} |");
    
    for (int coluna = 1; coluna <= 10; coluna++)
    {
        int produto = linha * coluna;
        Console.Write($"{produto,4} ");
    }
    
    Console.WriteLine();
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// DESAFIO EXTRA: Tabuada de Divisão
// ═══════════════════════════════════════════════════════════

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine($"   TABUADA DE DIVISÃO DO {numero}      ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

for (int i = 1; i <= 10; i++)
{
    int dividendo = numero * i;
    double quociente = (double)dividendo / numero;
    Console.WriteLine($"{dividendo,3} ÷ {numero} = {quociente,2}");
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// VERSÃO INTERATIVA: Testando conhecimento
// ═══════════════════════════════════════════════════════════

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("   TESTE SEU CONHECIMENTO!        ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

Random random = new Random();
int acertos = 0;
int totalPerguntas = 5;

for (int i = 1; i <= totalPerguntas; i++)
{
    int num1 = random.Next(1, 11);
    int num2 = random.Next(1, 11);
    int respostaCorreta = num1 * num2;
    
    Console.Write($"Pergunta {i}: {num1} × {num2} = ");
    int resposta = int.Parse(Console.ReadLine());
    
    if (resposta == respostaCorreta)
    {
        Console.WriteLine("✓ Correto!");
        acertos++;
    }
    else
    {
        Console.WriteLine($"✗ Errado! A resposta correta é {respostaCorreta}");
    }
    
    Console.WriteLine();
}

double percentualAcerto = (double)acertos / totalPerguntas * 100;
Console.WriteLine($"Você acertou {acertos} de {totalPerguntas} ({percentualAcerto:F1}%)");

if (percentualAcerto == 100)
    Console.WriteLine("🏆 Perfeito! Você é um mestre da tabuada!");
else if (percentualAcerto >= 80)
    Console.WriteLine("👍 Muito bem! Continue praticando!");
else if (percentualAcerto >= 60)
    Console.WriteLine("😊 Bom trabalho! Pratique mais um pouco!");
else
    Console.WriteLine("📚 Continue estudando! A prática leva à perfeição!");

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. LOOP FOR BÁSICO:
 * 
 *    for (int i = 1; i <= 10; i++)
 *    {
 *        // Executa 10 vezes: i = 1, 2, 3, ..., 10
 *    }
 *    
 *    Componentes:
 *    - Inicialização: int i = 1
 *    - Condição: i <= 10
 *    - Incremento: i++
 * 
 * 2. LOOPS ANINHADOS (Nested Loops):
 * 
 *    for (int i = 1; i <= 3; i++)        ← Loop externo
 *    {
 *        for (int j = 1; j <= 3; j++)    ← Loop interno
 *        {
 *            // Executa 3 × 3 = 9 vezes
 *        }
 *    }
 *    
 *    Iterações:
 *    i=1, j=1 → i=1, j=2 → i=1, j=3
 *    i=2, j=1 → i=2, j=2 → i=2, j=3
 *    i=3, j=1 → i=3, j=2 → i=3, j=3
 * 
 * 3. FORMATAÇÃO DE SAÍDA:
 * 
 *    {numero,3}  → Alinha à direita, 3 caracteres
 *    {numero,-3} → Alinha à esquerda, 3 caracteres
 *    
 *    Exemplo:
 *    Console.WriteLine($"{5,3}");   // "  5"
 *    Console.WriteLine($"{5,-3}");  // "5  "
 *    Console.WriteLine($"{100,3}"); // "100"
 * 
 * 4. CRIANDO LINHAS REPETIDAS:
 * 
 *    new string('-', 50)  → "---...---" (50 vezes)
 *    
 *    Útil para criar separadores:
 *    Console.WriteLine(new string('=', 40));
 * 
 * 5. GERAÇÃO DE NÚMEROS ALEATÓRIOS:
 * 
 *    Random random = new Random();
 *    int numero = random.Next(1, 11);  // 1 a 10 (11 é exclusivo)
 *    
 *    Ranges:
 *    random.Next()        → 0 a int.MaxValue
 *    random.Next(10)      → 0 a 9
 *    random.Next(1, 11)   → 1 a 10
 * 
 * ═══════════════════════════════════════════════════════════
 * ESTRUTURA DE LOOPS ANINHADOS
 * ═══════════════════════════════════════════════════════════
 * 
 * Tabela de multiplicação 3x3:
 * 
 *      1   2   3
 *   ┌─────────────
 * 1 │  1   2   3
 * 2 │  2   4   6
 * 3 │  3   6   9
 * 
 * Código:
 * for (int linha = 1; linha <= 3; linha++)
 * {
 *     for (int coluna = 1; coluna <= 3; coluna++)
 *     {
 *         int produto = linha * coluna;
 *         Console.Write($"{produto,4}");
 *     }
 *     Console.WriteLine();  // Nova linha após cada row
 * }
 * 
 * ═══════════════════════════════════════════════════════════
 * OTIMIZAÇÕES E VARIAÇÕES
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. TABUADA COLORIDA (com ANSI escape codes):
 * 
 *    for (int i = 1; i <= 10; i++)
 *    {
 *        if (i % 2 == 0)
 *            Console.ForegroundColor = ConsoleColor.Green;
 *        else
 *            Console.ForegroundColor = ConsoleColor.Cyan;
 *        
 *        Console.WriteLine($"{numero} × {i} = {numero * i}");
 *    }
 *    Console.ResetColor();
 * 
 * 2. TABUADA OTIMIZADA (pula números):
 * 
 *    for (int i = 2; i <= 10; i += 2)  // Só números pares
 *    {
 *        Console.WriteLine($"{numero} × {i} = {numero * i}");
 *    }
 * 
 * 3. TABUADA REVERSA:
 * 
 *    for (int i = 10; i >= 1; i--)  // Decremento
 *    {
 *        Console.WriteLine($"{numero} × {i} = {numero * i}");
 *    }
 * 
 * 4. TABUADA COM BREAK:
 * 
 *    for (int i = 1; i <= 100; i++)
 *    {
 *        int resultado = numero * i;
 *        Console.WriteLine($"{numero} × {i} = {resultado}");
 *        
 *        if (resultado > 100)  // Para quando passar de 100
 *            break;
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * PERFORMANCE E COMPLEXIDADE
 * ═══════════════════════════════════════════════════════════
 * 
 * Loop simples:
 * - Complexidade: O(n)
 * - Executa n vezes
 * - Rápido
 * 
 * Loops aninhados:
 * - Complexidade: O(n²)
 * - Executa n × m vezes
 * - Cuidado com grandes valores!
 * 
 * Exemplo:
 * for (int i = 0; i < 1000; i++)        ← 1.000 iterações
 * {
 *     for (int j = 0; j < 1000; j++)    ← 1.000 iterações
 *     {
 *         // Total: 1.000.000 execuções!
 *     }
 * }
 * 
 * ═══════════════════════════════════════════════════════════
 * APLICAÇÕES PRÁTICAS DE LOOPS ANINHADOS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. Matrizes (arrays 2D):
 *    for (int i = 0; i < linhas; i++)
 *        for (int j = 0; j < colunas; j++)
 *            matriz[i][j] = ...
 * 
 * 2. Imagens (pixels):
 *    for (int y = 0; y < altura; y++)
 *        for (int x = 0; x < largura; x++)
 *            pixel[x,y] = cor;
 * 
 * 3. Jogos (grade):
 *    for (int row = 0; row < 8; row++)
 *        for (int col = 0; col < 8; col++)
 *            tabuleiro[row][col] = ...
 * 
 * 4. Comparações (todos com todos):
 *    for (int i = 0; i < alunos.Length; i++)
 *        for (int j = i + 1; j < alunos.Length; j++)
 *            Comparar(alunos[i], alunos[j]);
 * 
 * ═══════════════════════════════════════════════════════════
 * DICAS E BOAS PRÁTICAS
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Use nomes descritivos (linha, coluna, not i, j)
 * ✅ Comente loops complexos
 * ✅ Evite loops aninhados > 3 níveis
 * ✅ Considere performance em loops grandes
 * ✅ Use break/continue quando apropriado
 * ✅ Valide ranges antes do loop
 * 
 * ❌ Evite modificar contador dentro do loop
 * ❌ Evite loops infinitos sem break
 * ❌ Evite lógica complexa dentro de loops
 * 
 * ═══════════════════════════════════════════════════════════
 */