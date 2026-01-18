# 📝 Correções dos Exercícios

## 🎯 Exercício 1

```csharp
// Exercício 1 Corrigido: Calculadora com Menu
// Arquivo: Program.cs

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("         CALCULADORA                ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("1. Somar (+)");
Console.WriteLine("2. Subtrair (-)");
Console.WriteLine("3. Multiplicar (×)");
Console.WriteLine("4. Dividir (÷)");
Console.WriteLine("═══════════════════════════════════");
Console.Write("Escolha a operação: ");

int opcao = int.Parse(Console.ReadLine());

Console.Write("Digite o primeiro número: ");
double num1 = double.Parse(Console.ReadLine());

Console.Write("Digite o segundo número: ");
double num2 = double.Parse(Console.ReadLine());

Console.WriteLine();

// SOLUÇÃO 1: Switch Clássico
string operador = "";
double resultado = 0;
bool operacaoValida = true;

switch (opcao)
{
    case 1:
        resultado = num1 + num2;
        operador = "+";
        break;
    case 2:
        resultado = num1 - num2;
        operador = "-";
        break;
    case 3:
        resultado = num1 * num2;
        operador = "×";
        break;
    case 4:
        if (num2 == 0)
        {
            Console.WriteLine("❌ ERRO: Divisão por zero!");
            operacaoValida = false;
        }
        else
        {
            resultado = num1 / num2;
            operador = "÷";
        }
        break;
    default:
        Console.WriteLine("❌ ERRO: Opção inválida!");
        operacaoValida = false;
        break;
}

if (operacaoValida)
{
    Console.WriteLine($"✓ Resultado: {num1} {operador} {num2} = {resultado}");
}

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════");

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 2: Switch Expression (C# 8+) - MODERNA
// ═══════════════════════════════════════════════════════════

Console.WriteLine("\n--- Versão com Switch Expression ---\n");

string resultado2 = opcao switch
{
    1 => $"{num1} + {num2} = {num1 + num2}",
    2 => $"{num1} - {num2} = {num1 - num2}",
    3 => $"{num1} × {num2} = {num1 * num2}",
    4 when num2 != 0 => $"{num1} ÷ {num2} = {num1 / num2}",
    4 => "❌ ERRO: Divisão por zero!",
    _ => "❌ ERRO: Opção inválida!"
};

Console.WriteLine(resultado2);

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. SWITCH CLÁSSICO:
 *    - Usa case com break obrigatório
 *    - Variável modificada dentro dos cases
 *    - Mais verboso, mas muito claro
 *    - Melhor para lógica complexa
 * 
 * 2. SWITCH EXPRESSION (C# 8+):
 *    - Mais conciso e funcional
 *    - Retorna diretamente um valor
 *    - "=>" em vez de ":"
 *    - "_" é o default
 *    - "when" para condições adicionais
 * 
 * 3. VALIDAÇÃO DE DIVISÃO:
 *    - SEMPRE verificar divisão por zero
 *    - Em C#, divisão por zero lança exceção (int)
 *    - Com double/float, retorna Infinity (mas ainda ruim)
 * 
 * 4. TIPOS DE DADOS:
 *    - Usamos double (não int) para aceitar decimais
 *    - Permite resultados como 7 / 2 = 3.5
 * 
 * ═══════════════════════════════════════════════════════════
 * DESAFIO EXTRA - CALCULADORA COMPLETA
 * ═══════════════════════════════════════════════════════════
 */

Console.WriteLine("\n═══════════════════════════════════");
Console.WriteLine("    DESAFIO: OPERAÇÕES EXTRAS       ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("5. Potência (^)");
Console.WriteLine("6. Raiz Quadrada (√)");
Console.WriteLine("7. Módulo (%)");
Console.WriteLine("═══════════════════════════════════");
Console.Write("Escolha: ");

int opcaoExtra = int.Parse(Console.ReadLine());

double resultadoExtra = opcaoExtra switch
{
    5 => Math.Pow(num1, num2),
    6 => Math.Sqrt(num1),
    7 when num2 != 0 => num1 % num2,
    7 => double.NaN,  // Not a Number
    _ => double.NaN
};

string descricaoExtra = opcaoExtra switch
{
    5 => $"{num1} elevado a {num2}",
    6 => $"Raiz quadrada de {num1}",
    7 when num2 != 0 => $"{num1} módulo {num2}",
    7 => "❌ Erro: Divisão por zero no módulo",
    _ => "❌ Operação inválida"
};

if (!double.IsNaN(resultadoExtra))
{
    Console.WriteLine($"\n✓ {descricaoExtra} = {resultadoExtra:F2}");
}
else
{
    Console.WriteLine($"\n{descricaoExtra}");
}

/*
 * ═══════════════════════════════════════════════════════════
 * FUNÇÕES MATEMÁTICAS EM C#
 * ═══════════════════════════════════════════════════════════
 * 
 * Math.Pow(base, expoente)     → Potência
 * Math.Sqrt(numero)            → Raiz quadrada
 * Math.Abs(numero)             → Valor absoluto
 * Math.Round(numero)           → Arredonda
 * Math.Ceiling(numero)         → Arredonda para cima
 * Math.Floor(numero)           → Arredonda para baixo
 * Math.Max(a, b)               → Maior valor
 * Math.Min(a, b)               → Menor valor
 * Math.PI                      → Constante π
 * Math.E                       → Constante e
 * 
 * ═══════════════════════════════════════════════════════════
 * COMPARAÇÃO: SWITCH vs IF-ELSE
 * ═══════════════════════════════════════════════════════════
 * 
 * USE SWITCH quando:
 * ✅ Comparar UMA variável com múltiplos valores
 * ✅ Valores são constantes (1, 2, "A", "B")
 * ✅ Código mais limpo e legível
 * 
 * USE IF-ELSE quando:
 * ✅ Condições complexas (múltiplas variáveis)
 * ✅ Ranges (if x > 10 && x < 20)
 * ✅ Expressões booleanas compostas
 * 
 * Exemplo:
 * 
 * // ✅ SWITCH é melhor aqui
 * switch (opcao)
 * {
 *     case 1: ... break;
 *     case 2: ... break;
 * }
 * 
 * // ✅ IF-ELSE é melhor aqui
 * if (idade >= 18 && temCNH && !estaSuspenso)
 * {
 *     // lógica complexa
 * }
 * 
 * ═══════════════════════════════════════════════════════════
 * BOAS PRÁTICAS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. SEMPRE valide divisão por zero
 * 2. Use default/_ para casos não previstos
 * 3. Prefira switch expression quando possível (C# 8+)
 * 4. Nomeie variáveis descritivamente (operador, resultado)
 * 5. Agrupe cases relacionados (weekend: case 6: case 7:)
 * 
 * ═══════════════════════════════════════════════════════════
 * VARIAÇÕES DO EXERCÍCIO
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. CALCULADORA COM LOOP:
 *    do {
 *        // operação
 *        Console.Write("Continuar? (S/N): ");
 *    } while (Console.ReadLine().ToUpper() == "S");
 * 
 * 2. CALCULADORA COM HISTÓRICO:
 *    List<string> historico = new();
 *    historico.Add($"{num1} + {num2} = {resultado}");
 * 
 * 3. CALCULADORA CIENTÍFICA:
 *    Adicionar sin, cos, tan, log, etc.
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

## 🎯 Exercício 3

```csharp
// Exercício 3 Corrigido: Classificador de Idade
// Arquivo: Program.cs

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("    CLASSIFICADOR DE IDADE         ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

Console.Write("Digite a idade: ");
string entrada = Console.ReadLine();

// Validação de entrada
if (!int.TryParse(entrada, out int idade))
{
    Console.WriteLine("❌ ERRO: Digite um número válido!");
    return;
}

// Validação de range
if (idade < 0 || idade > 150)
{
    Console.WriteLine("❌ ERRO: Idade deve estar entre 0 e 150!");
    return;
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 1: if-else if-else (Clássico)
// ═══════════════════════════════════════════════════════════

string classificacao;
string emoji;

if (idade <= 12)
{
    classificacao = "Criança";
    emoji = "👶";
}
else if (idade <= 17)
{
    classificacao = "Adolescente";
    emoji = "🧒";
}
else if (idade <= 59)
{
    classificacao = "Adulto";
    emoji = "👨";
}
else
{
    classificacao = "Idoso";
    emoji = "👴";
}

Console.WriteLine($"{emoji} Classificação: {classificacao}");

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 2: Operador Ternário Aninhado
// ═══════════════════════════════════════════════════════════

string classificacao2 = idade <= 12 ? "Criança" :
                        idade <= 17 ? "Adolescente" :
                        idade <= 59 ? "Adulto" : "Idoso";

Console.WriteLine($"Classificação (ternário): {classificacao2}");

// ═══════════════════════════════════════════════════════════
// SOLUÇÃO 3: Switch Expression com Pattern Matching (C# 8+)
// ═══════════════════════════════════════════════════════════

string classificacao3 = idade switch
{
    <= 12 => "Criança",
    <= 17 => "Adolescente",
    <= 59 => "Adulto",
    _ => "Idoso"
};

Console.WriteLine($"Classificação (switch): {classificacao3}");

// ═══════════════════════════════════════════════════════════
// DESAFIO EXTRA: Subcategorias Detalhadas
// ═══════════════════════════════════════════════════════════

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("  CLASSIFICAÇÃO DETALHADA          ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

string classificacaoDetalhada = idade switch
{
    0 => "Recém-nascido",
    >= 1 and <= 2 => "Bebê",
    >= 3 and <= 5 => "Criança (primeira infância)",
    >= 6 and <= 12 => "Criança (segunda infância)",
    >= 13 and <= 15 => "Adolescente inicial",
    >= 16 and <= 17 => "Adolescente final",
    >= 18 and <= 25 => "Adulto jovem",
    >= 26 and <= 35 => "Adulto",
    >= 36 and <= 45 => "Meia-idade inicial",
    >= 46 and <= 59 => "Meia-idade",
    >= 60 and <= 75 => "Idoso ativo",
    >= 76 and <= 90 => "Idoso",
    > 90 => "Idoso longevo",
    _ => "Idade inválida"
};

Console.WriteLine($"📊 Classificação detalhada: {classificacaoDetalhada}");

// Informações adicionais baseadas na idade
Console.WriteLine();
Console.WriteLine("ℹ️  Informações relevantes:");

switch (idade)
{
    case <= 15:
        Console.WriteLine("   • Ensino fundamental");
        Console.WriteLine("   • Não pode votar");
        Console.WriteLine("   • Não pode dirigir");
        break;
    
    case 16 or 17:
        Console.WriteLine("   • Ensino médio");
        Console.WriteLine("   • Voto facultativo");
        Console.WriteLine("   • Não pode dirigir");
        break;
    
    case >= 18 and < 70:
        Console.WriteLine("   • Pode votar (obrigatório)");
        Console.WriteLine("   • Pode dirigir");
        Console.WriteLine("   • Responsabilidade penal completa");
        break;
    
    case >= 70:
        Console.WriteLine("   • Voto facultativo");
        Console.WriteLine("   • Direitos de transporte gratuito");
        Console.WriteLine("   • Prioridade em atendimentos");
        break;
}

// Expectativa de vida
Console.WriteLine();
if (idade < 80)
{
    int anosRestantes = 80 - idade;
    Console.WriteLine($"⏰ Expectativa: aproximadamente {anosRestantes} anos pela frente");
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. if-else if-else:
 *    - Avalia condições em ordem
 *    - Para na primeira condição verdadeira
 *    - Use quando as condições são mutuamente exclusivas
 * 
 *    IMPORTANTE: Ordem importa!
 *    if (idade <= 12)      ← Testa primeiro
 *    else if (idade <= 17) ← Só testa se idade > 12
 *    else if (idade <= 59) ← Só testa se idade > 17
 * 
 * 2. Operador Ternário:
 *    condicao ? valorSeTrue : valorSeFalse
 *    
 *    Pode ser aninhado, mas cuidado com legibilidade:
 *    x ? a : y ? b : z ? c : d  ← Difícil de ler!
 * 
 * 3. Switch Expression com Relational Patterns:
 *    idade switch
 *    {
 *        <= 12 => ...,     ← Menor ou igual
 *        > 12 and <= 17 => ← Range com 'and'
 *        _ => ...          ← Default
 *    }
 * 
 * 4. Pattern Matching Avançado:
 *    - Relational: <, <=, >, >=
 *    - Logical: and, or, not
 *    - Type: is string, is int
 *    - Property: { Idade: > 18 }
 * 
 * ═══════════════════════════════════════════════════════════
 * COMPARAÇÃO DAS ABORDAGENS
 * ═══════════════════════════════════════════════════════════
 * 
 * | Abordagem      | Legibilidade | Modernidade | Flexibilidade |
 * |----------------|--------------|-------------|---------------|
 * | if-else if     | ⭐⭐⭐⭐⭐  | ⭐⭐        | ⭐⭐⭐⭐⭐    |
 * | Ternário       | ⭐⭐⭐      | ⭐⭐⭐      | ⭐⭐⭐        |
 * | Switch Expr    | ⭐⭐⭐⭐⭐  | ⭐⭐⭐⭐⭐  | ⭐⭐⭐⭐      |
 * 
 * QUANDO USAR CADA UM:
 * 
 * if-else if:
 * ✅ Condições complexas
 * ✅ Múltiplas variáveis
 * ✅ Lógica dentro dos blocos
 * 
 * Ternário:
 * ✅ Atribuições simples
 * ✅ Código inline
 * ❌ Evite aninhamentos profundos
 * 
 * Switch Expression:
 * ✅ C# 8+ (moderno)
 * ✅ Múltiplas comparações
 * ✅ Pattern matching
 * ✅ Retorno direto de valor
 * 
 * ═══════════════════════════════════════════════════════════
 * VALIDAÇÃO DE ENTRADA
 * ═══════════════════════════════════════════════════════════
 * 
 * int.TryParse() é MELHOR que int.Parse() porque:
 * 
 * // ❌ int.Parse() lança exceção se falhar
 * int idade = int.Parse(entrada);  // Crash se não for número
 * 
 * // ✅ TryParse retorna bool + out parameter
 * if (int.TryParse(entrada, out int idade))
 * {
 *     // Sucesso: idade tem o valor
 * }
 * else
 * {
 *     // Falhou: entrada não é número válido
 * }
 * 
 * ═══════════════════════════════════════════════════════════
 * EARLY RETURN PATTERN
 * ═══════════════════════════════════════════════════════════
 * 
 * Em vez de:
 * if (valido)
 * {
 *     // 50 linhas de código
 * }
 * else
 * {
 *     erro();
 * }
 * 
 * Prefira:
 * if (!valido)
 * {
 *     erro();
 *     return;  ← Sai cedo
 * }
 * // 50 linhas sem indentação
 * 
 * Vantagens:
 * ✅ Menos indentação
 * ✅ Mais legível
 * ✅ Lógica de erro no topo
 * 
 * ═══════════════════════════════════════════════════════════
 * EXTENSÕES POSSÍVEIS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. Adicionar mais categorias:
 *    - Escola (fundamental, médio, superior)
 *    - Direitos legais
 *    - Benefícios sociais
 * 
 * 2. Calcular idade em anos/meses/dias:
 *    DateTime nascimento = new DateTime(2000, 5, 15);
 *    TimeSpan diferenca = DateTime.Now - nascimento;
 * 
 * 3. Verificar maioridade em outros países:
 *    USA: 21 para bebidas alcoólicas
 *    Japão: 20 para maioridade
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

## 🎯 Exercício 5

```csharp
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
```

---

## 🎯 Exercício 8

```csharp
// Exercício 8 Corrigido: Jogo de Adivinhação
// Arquivo: Program.cs

using System;

Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("     JOGO DE ADIVINHAÇÃO          ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

// Configuração do jogo
Random random = new Random();
int numeroSecreto = random.Next(1, 101);  // 1 a 100
int tentativas = 0;
int maxTentativas = 7;
bool acertou = false;

Console.WriteLine("🎲 Pensei em um número entre 1 e 100!");
Console.WriteLine($"Você tem {maxTentativas} tentativas para acertar.");
Console.WriteLine();

// Loop principal do jogo
while (tentativas < maxTentativas && !acertou)
{
    tentativas++;
    Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
    
    // Validação de entrada
    if (!int.TryParse(Console.ReadLine(), out int palpite))
    {
        Console.WriteLine("❌ Digite um número válido!");
        tentativas--;  // Não conta como tentativa
        continue;
    }
    
    // Validação de range
    if (palpite < 1 || palpite > 100)
    {
        Console.WriteLine("❌ O número deve estar entre 1 e 100!");
        tentativas--;  // Não conta como tentativa
        continue;
    }
    
    // Verificação do palpite
    if (palpite == numeroSecreto)
    {
        acertou = true;
        Console.WriteLine($"🎉 PARABÉNS! Você acertou em {tentativas} tentativa(s)!");
    }
    else if (palpite < numeroSecreto)
    {
        int diferenca = numeroSecreto - palpite;
        
        if (diferenca <= 5)
            Console.WriteLine("🔥 MUITO QUENTE! O número é MAIOR!");
        else if (diferenca <= 15)
            Console.WriteLine("🌡️  Quente! O número é maior.");
        else
            Console.WriteLine("❄️  Frio! O número é muito maior.");
    }
    else  // palpite > numeroSecreto
    {
        int diferenca = palpite - numeroSecreto;
        
        if (diferenca <= 5)
            Console.WriteLine("🔥 MUITO QUENTE! O número é MENOR!");
        else if (diferenca <= 15)
            Console.WriteLine("🌡️  Quente! O número é menor.");
        else
            Console.WriteLine("❄️  Frio! O número é muito menor.");
    }
    
    Console.WriteLine();
}

// Resultado final
if (!acertou)
{
    Console.WriteLine($"😞 Você perdeu! O número era {numeroSecreto}.");
}

Console.WriteLine();

// ═══════════════════════════════════════════════════════════
// VERSÃO 2: Com do-while (jogar novamente)
// ═══════════════════════════════════════════════════════════

bool jogarNovamente;

do
{
    Console.WriteLine("═══════════════════════════════════");
    Console.WriteLine("     NOVA PARTIDA                  ");
    Console.WriteLine("═══════════════════════════════════");
    Console.WriteLine();
    
    numeroSecreto = random.Next(1, 101);
    tentativas = 0;
    acertou = false;
    
    while (tentativas < maxTentativas && !acertou)
    {
        tentativas++;
        Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
        
        if (int.TryParse(Console.ReadLine(), out int palpite) && 
            palpite >= 1 && palpite <= 100)
        {
            if (palpite == numeroSecreto)
            {
                acertou = true;
                Console.WriteLine($"🎉 Você acertou em {tentativas} tentativa(s)!");
            }
            else if (palpite < numeroSecreto)
            {
                Console.WriteLine("📈 O número é MAIOR!");
            }
            else
            {
                Console.WriteLine("📉 O número é MENOR!");
            }
        }
        else
        {
            Console.WriteLine("❌ Entrada inválida!");
            tentativas--;
        }
        
        Console.WriteLine();
    }
    
    if (!acertou)
    {
        Console.WriteLine($"😞 Game Over! O número era {numeroSecreto}.");
        Console.WriteLine();
    }
    
    // Perguntar se quer jogar novamente
    Console.Write("Jogar novamente? (S/N): ");
    string resposta = Console.ReadLine()?.Trim().ToUpper() ?? "N";
    jogarNovamente = resposta == "S" || resposta == "SIM";
    Console.WriteLine();
    
} while (jogarNovamente);

Console.WriteLine("Obrigado por jogar! 👋");

// ═══════════════════════════════════════════════════════════
// VERSÃO 3: Com histórico de tentativas
// ═══════════════════════════════════════════════════════════

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine("  VERSÃO COM HISTÓRICO             ");
Console.WriteLine("═══════════════════════════════════");
Console.WriteLine();

numeroSecreto = random.Next(1, 101);
tentativas = 0;
acertou = false;
List<int> historico = new List<int>();  // Armazena todos os palpites

while (tentativas < maxTentativas && !acertou)
{
    tentativas++;
    
    // Mostrar histórico
    if (historico.Count > 0)
    {
        Console.Write("Palpites anteriores: ");
        Console.WriteLine(string.Join(", ", historico));
    }
    
    Console.Write($"Tentativa {tentativas}/{maxTentativas}: ");
    
    if (int.TryParse(Console.ReadLine(), out int palpite) && 
        palpite >= 1 && palpite <= 100)
    {
        // Verificar se já tentou esse número
        if (historico.Contains(palpite))
        {
            Console.WriteLine("⚠️  Você já tentou esse número!");
            tentativas--;
            continue;
        }
        
        historico.Add(palpite);
        
        if (palpite == numeroSecreto)
        {
            acertou = true;
            Console.WriteLine($"🎉 ACERTOU em {tentativas} tentativa(s)!");
            
            // Estatísticas
            int menorPalpite = historico.Min();
            int maiorPalpite = historico.Max();
            Console.WriteLine($"Menor palpite: {menorPalpite}");
            Console.WriteLine($"Maior palpite: {maiorPalpite}");
        }
        else
        {
            string dica = palpite < numeroSecreto ? "MAIOR" : "MENOR";
            Console.WriteLine($"O número é {dica}!");
        }
    }
    else
    {
        Console.WriteLine("❌ Entrada inválida!");
        tentativas--;
    }
    
    Console.WriteLine();
}

if (!acertou)
{
    Console.WriteLine($"😞 Acabaram as tentativas! Era {numeroSecreto}.");
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. LOOP WHILE:
 * 
 *    while (condição)
 *    {
 *        // Executa enquanto condição for true
 *    }
 *    
 *    Características:
 *    - Testa condição ANTES de executar
 *    - Pode executar 0 vezes se condição inicialmente falsa
 *    - Ideal quando não se sabe quantas iterações
 * 
 *    Exemplo:
 *    int tentativas = 0;
 *    while (tentativas < 5)
 *    {
 *        Console.WriteLine(tentativas);
 *        tentativas++;
 *    }
 *    // Output: 0, 1, 2, 3, 4
 * 
 * 2. LOOP DO-WHILE:
 * 
 *    do
 *    {
 *        // Executa pelo menos uma vez
 *    } while (condição);
 *    
 *    Características:
 *    - Testa condição DEPOIS de executar
 *    - SEMPRE executa pelo menos 1 vez
 *    - Ideal para menus e validações
 * 
 *    Exemplo:
 *    string resposta;
 *    do
 *    {
 *        Console.Write("Continuar? (S/N): ");
 *        resposta = Console.ReadLine();
 *    } while (resposta != "S" && resposta != "N");
 * 
 * 3. CONDIÇÕES COMPOSTAS:
 * 
 *    while (tentativas < maxTentativas && !acertou)
 *                   ↑                        ↑
 *            Condição 1           Condição 2
 *    
 *    Operadores lógicos:
 *    && (AND) - Ambas devem ser true
 *    || (OR)  - Pelo menos uma deve ser true
 *    !  (NOT) - Inverte o valor booleano
 *    
 *    Tabela verdade AND:
 *    true  && true  → true
 *    true  && false → false
 *    false && true  → false
 *    false && false → false
 *    
 *    Tabela verdade OR:
 *    true  || true  → true
 *    true  || false → true
 *    false || true  → true
 *    false || false → false
 * 
 * 4. CLASSE RANDOM:
 * 
 *    Random random = new Random();
 *    
 *    Métodos:
 *    random.Next()           → 0 a int.MaxValue
 *    random.Next(100)        → 0 a 99
 *    random.Next(1, 101)     → 1 a 100
 *    random.NextDouble()     → 0.0 a 1.0
 *    
 *    Exemplo:
 *    int dado = random.Next(1, 7);     // 1 a 6
 *    double porcentagem = random.NextDouble() * 100;
 * 
 * 5. CONTINUE vs BREAK:
 * 
 *    continue - Pula para próxima iteração
 *    break    - Sai do loop imediatamente
 *    
 *    Exemplo:
 *    for (int i = 0; i < 10; i++)
 *    {
 *        if (i % 2 == 0)
 *            continue;  // Pula números pares
 *        
 *        if (i == 7)
 *            break;     // Para quando chegar em 7
 *        
 *        Console.WriteLine(i);
 *    }
 *    // Output: 1, 3, 5
 * 
 * ═══════════════════════════════════════════════════════════
 * DIFERENÇAS: WHILE vs DO-WHILE vs FOR
 * ═══════════════════════════════════════════════════════════
 * 
 * WHILE:
 * ------
 * while (contador < 10)
 * {
 *     Console.WriteLine(contador);
 *     contador++;
 * }
 * 
 * Uso: Quando não sabe quantas iterações
 * Executa: 0 ou mais vezes
 * Testa: ANTES de executar
 * 
 * 
 * DO-WHILE:
 * ---------
 * do
 * {
 *     Console.WriteLine(contador);
 *     contador++;
 * } while (contador < 10);
 * 
 * Uso: Quando precisa executar pelo menos 1 vez
 * Executa: 1 ou mais vezes
 * Testa: DEPOIS de executar
 * 
 * 
 * FOR:
 * ----
 * for (int i = 0; i < 10; i++)
 * {
 *     Console.WriteLine(i);
 * }
 * 
 * Uso: Quando sabe exatamente quantas iterações
 * Executa: 0 ou mais vezes
 * Testa: ANTES de executar
 * 
 * ═══════════════════════════════════════════════════════════
 * ARMADILHAS COMUNS (COMMON PITFALLS)
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. LOOP INFINITO:
 * 
 *    ❌ ERRADO:
 *    int i = 0;
 *    while (i < 10)
 *    {
 *        Console.WriteLine(i);
 *        // ESQUECEU de incrementar i!
 *    }
 *    
 *    ✅ CORRETO:
 *    int i = 0;
 *    while (i < 10)
 *    {
 *        Console.WriteLine(i);
 *        i++;  // ← Incrementa!
 *    }
 * 
 * 2. CONDIÇÃO SEMPRE VERDADEIRA:
 * 
 *    ❌ ERRADO:
 *    while (true)  // Loop infinito!
 *    {
 *        // Sem break, roda para sempre
 *    }
 *    
 *    ✅ CORRETO:
 *    while (true)
 *    {
 *        if (condicaoSaida)
 *            break;  // ← Saída
 *    }
 * 
 * 3. OFF-BY-ONE ERROR:
 * 
 *    ❌ ERRADO:
 *    int tentativas = 0;
 *    while (tentativas <= 5)  // Executa 6 vezes!
 *    {
 *        tentativas++;
 *    }
 *    
 *    ✅ CORRETO:
 *    int tentativas = 0;
 *    while (tentativas < 5)   // Executa 5 vezes
 *    {
 *        tentativas++;
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * PADRÕES DE USO COMUNS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. VALIDAÇÃO DE ENTRADA:
 * 
 *    int numero;
 *    while (true)
 *    {
 *        Console.Write("Digite um número: ");
 *        if (int.TryParse(Console.ReadLine(), out numero))
 *            break;  // Entrada válida, sai do loop
 *        
 *        Console.WriteLine("Entrada inválida!");
 *    }
 * 
 * 2. MENU INTERATIVO:
 * 
 *    string opcao;
 *    do
 *    {
 *        Console.WriteLine("1 - Jogar");
 *        Console.WriteLine("2 - Configurações");
 *        Console.WriteLine("0 - Sair");
 *        opcao = Console.ReadLine();
 *        
 *        // Processar opção...
 *        
 *    } while (opcao != "0");
 * 
 * 3. PROCESSAMENTO ATÉ SENTINELA:
 * 
 *    string linha;
 *    while ((linha = Console.ReadLine()) != "fim")
 *    {
 *        // Processar linha
 *        Console.WriteLine($"Você digitou: {linha}");
 *    }
 * 
 * 4. JOGO COM GAME LOOP:
 * 
 *    bool jogoAtivo = true;
 *    while (jogoAtivo)
 *    {
 *        // 1. Processar entrada
 *        // 2. Atualizar estado
 *        // 3. Renderizar
 *        // 4. Verificar condições de saída
 *        
 *        if (jogadorPerdeu || jogadorDesistiu)
 *            jogoAtivo = false;
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * OTIMIZAÇÕES E PERFORMANCE
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. EVITE OPERAÇÕES PESADAS DENTRO DO LOOP:
 * 
 *    ❌ LENTO:
 *    while (condicao)
 *    {
 *        var lista = ObterLista();  // Chama função toda vez!
 *        // ...
 *    }
 *    
 *    ✅ RÁPIDO:
 *    var lista = ObterLista();      // Chama uma vez só
 *    while (condicao)
 *    {
 *        // Usa lista
 *    }
 * 
 * 2. USE BREAK PARA SAÍDA ANTECIPADA:
 * 
 *    while (condicao)
 *    {
 *        if (encontrouResultado)
 *            break;  // Sai imediatamente
 *        
 *        // Não precisa continuar procurando
 *    }
 * 
 * ═══════════════════════════════════════════════════════════
 * DICAS E BOAS PRÁTICAS
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Use while quando não sabe quantas iterações
 * ✅ Use do-while para menus e validações
 * ✅ Use for quando sabe quantas iterações
 * ✅ Sempre garanta que o loop pode terminar
 * ✅ Use break para sair antecipadamente
 * ✅ Use continue para pular iteração
 * ✅ Comente loops complexos
 * 
 * ❌ Evite loops infinitos sem break
 * ❌ Evite modificar contador de forma imprevisível
 * ❌ Evite muitos níveis de loops aninhados
 * ❌ Evite operações pesadas dentro do loop
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

## 🎯 Exercício 10

```csharp
// Exercício 10 Corrigido: Sistema Completo de Gerenciamento de Notas
// Arquivo: Program.cs

using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("═══════════════════════════════════════════════════════════");
Console.WriteLine("          SISTEMA DE GERENCIAMENTO DE NOTAS               ");
Console.WriteLine("═══════════════════════════════════════════════════════════");
Console.WriteLine();

// Estrutura de dados
List<string> nomes = new List<string>();
List<double> notas = new List<double>();

string opcao;

do
{
    // Menu principal
    Console.WriteLine("╔═══════════════════════════════════╗");
    Console.WriteLine("║          MENU PRINCIPAL           ║");
    Console.WriteLine("╠═══════════════════════════════════╣");
    Console.WriteLine("║ 1 - Adicionar aluno               ║");
    Console.WriteLine("║ 2 - Listar todos os alunos        ║");
    Console.WriteLine("║ 3 - Buscar aluno por nome         ║");
    Console.WriteLine("║ 4 - Calcular média da turma       ║");
    Console.WriteLine("║ 5 - Mostrar estatísticas          ║");
    Console.WriteLine("║ 6 - Alunos aprovados/reprovados   ║");
    Console.WriteLine("║ 7 - Remover aluno                 ║");
    Console.WriteLine("║ 8 - Editar nota                   ║");
    Console.WriteLine("║ 0 - Sair                          ║");
    Console.WriteLine("╚═══════════════════════════════════╝");
    Console.Write("\nEscolha uma opção: ");
    opcao = Console.ReadLine()?.Trim() ?? "";
    Console.WriteLine();

    switch (opcao)
    {
        case "1":  // Adicionar aluno
            AdicionarAluno();
            break;

        case "2":  // Listar todos
            ListarAlunos();
            break;

        case "3":  // Buscar por nome
            BuscarAluno();
            break;

        case "4":  // Calcular média
            CalcularMedia();
            break;

        case "5":  // Estatísticas
            MostrarEstatisticas();
            break;

        case "6":  // Aprovados/Reprovados
            MostrarAprovados();
            break;

        case "7":  // Remover aluno
            RemoverAluno();
            break;

        case "8":  // Editar nota
            EditarNota();
            break;

        case "0":  // Sair
            Console.WriteLine("👋 Encerrando sistema...");
            break;

        default:
            Console.WriteLine("❌ Opção inválida!");
            break;
    }

    Console.WriteLine();

} while (opcao != "0");

// ═══════════════════════════════════════════════════════════
// FUNÇÕES DO SISTEMA
// ═══════════════════════════════════════════════════════════

void AdicionarAluno()
{
    Console.WriteLine("─── ADICIONAR ALUNO ───");
    
    // Nome
    Console.Write("Nome do aluno: ");
    string nome = Console.ReadLine()?.Trim() ?? "";
    
    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("❌ Nome não pode ser vazio!");
        return;
    }
    
    // Verificar se já existe
    if (nomes.Any(n => n.Equals(nome, StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("⚠️  Aluno já cadastrado!");
        return;
    }
    
    // Nota
    Console.Write("Nota (0-10): ");
    if (!double.TryParse(Console.ReadLine(), out double nota) || 
        nota < 0 || nota > 10)
    {
        Console.WriteLine("❌ Nota inválida! Deve estar entre 0 e 10.");
        return;
    }
    
    // Adicionar
    nomes.Add(nome);
    notas.Add(nota);
    
    Console.WriteLine($"✅ Aluno '{nome}' adicionado com sucesso!");
    Console.WriteLine($"   Nota: {nota:F2} - Situação: {ObterSituacao(nota)}");
}

void ListarAlunos()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    Console.WriteLine("─── LISTA DE ALUNOS ───");
    Console.WriteLine();
    Console.WriteLine($"{"#",-4} {"Nome",-20} {"Nota",-8} {"Situação",-15}");
    Console.WriteLine(new string('─', 50));
    
    for (int i = 0; i < nomes.Count; i++)
    {
        string situacao = ObterSituacao(notas[i]);
        string emoji = ObterEmoji(notas[i]);
        
        Console.WriteLine($"{i + 1,-4} {nomes[i],-20} {notas[i],-8:F2} {emoji} {situacao}");
    }
    
    Console.WriteLine(new string('─', 50));
    Console.WriteLine($"Total de alunos: {nomes.Count}");
}

void BuscarAluno()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    Console.Write("Digite o nome do aluno: ");
    string busca = Console.ReadLine()?.Trim() ?? "";
    
    // Busca case-insensitive e parcial
    var resultados = new List<int>();
    for (int i = 0; i < nomes.Count; i++)
    {
        if (nomes[i].Contains(busca, StringComparison.OrdinalIgnoreCase))
        {
            resultados.Add(i);
        }
    }
    
    if (resultados.Count == 0)
    {
        Console.WriteLine($"❌ Nenhum aluno encontrado com '{busca}'.");
        return;
    }
    
    Console.WriteLine($"\n🔍 Encontrado(s) {resultados.Count} aluno(s):");
    Console.WriteLine();
    
    foreach (int i in resultados)
    {
        Console.WriteLine($"Nome: {nomes[i]}");
        Console.WriteLine($"Nota: {notas[i]:F2}");
        Console.WriteLine($"Situação: {ObterSituacao(notas[i])}");
        Console.WriteLine();
    }
}

void CalcularMedia()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Calcular média
    double soma = 0;
    for (int i = 0; i < notas.Count; i++)
    {
        soma += notas[i];
    }
    double media = soma / notas.Count;
    
    Console.WriteLine("─── MÉDIA DA TURMA ───");
    Console.WriteLine($"Total de alunos: {nomes.Count}");
    Console.WriteLine($"Média geral: {media:F2}");
    Console.WriteLine($"Situação da turma: {ObterSituacao(media)}");
}

void MostrarEstatisticas()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Calcular estatísticas
    double menorNota = notas[0];
    double maiorNota = notas[0];
    double soma = notas[0];
    string alunoMenorNota = nomes[0];
    string alunoMaiorNota = nomes[0];
    
    for (int i = 1; i < notas.Count; i++)
    {
        if (notas[i] < menorNota)
        {
            menorNota = notas[i];
            alunoMenorNota = nomes[i];
        }
        
        if (notas[i] > maiorNota)
        {
            maiorNota = notas[i];
            alunoMaiorNota = nomes[i];
        }
        
        soma += notas[i];
    }
    
    double media = soma / notas.Count;
    
    // Contar aprovados/reprovados
    int aprovados = 0;
    int reprovados = 0;
    int recuperacao = 0;
    
    foreach (double nota in notas)
    {
        if (nota >= 7.0)
            aprovados++;
        else if (nota >= 5.0)
            recuperacao++;
        else
            reprovados++;
    }
    
    // Mostrar estatísticas
    Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
    Console.WriteLine("║               ESTATÍSTICAS DA TURMA                       ║");
    Console.WriteLine("╠═══════════════════════════════════════════════════════════╣");
    Console.WriteLine($"║ Total de alunos:           {nomes.Count,3}                         ║");
    Console.WriteLine($"║                                                           ║");
    Console.WriteLine($"║ Média geral:               {media,6:F2}                       ║");
    Console.WriteLine($"║ Menor nota:                {menorNota,6:F2} ({alunoMenorNota,-15})   ║");
    Console.WriteLine($"║ Maior nota:                {maiorNota,6:F2} ({alunoMaiorNota,-15})   ║");
    Console.WriteLine($"║                                                           ║");
    Console.WriteLine($"║ ✅ Aprovados (≥ 7.0):      {aprovados,3} ({aprovados * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine($"║ ⚠️  Recuperação (5.0-6.9): {recuperacao,3} ({recuperacao * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine($"║ ❌ Reprovados (< 5.0):     {reprovados,3} ({reprovados * 100.0 / nomes.Count,5:F1}%)              ║");
    Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
}

void MostrarAprovados()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    // Separar por categoria
    var aprovados = new List<(string nome, double nota)>();
    var recuperacao = new List<(string nome, double nota)>();
    var reprovados = new List<(string nome, double nota)>();
    
    for (int i = 0; i < nomes.Count; i++)
    {
        if (notas[i] >= 7.0)
            aprovados.Add((nomes[i], notas[i]));
        else if (notas[i] >= 5.0)
            recuperacao.Add((nomes[i], notas[i]));
        else
            reprovados.Add((nomes[i], notas[i]));
    }
    
    // Mostrar aprovados
    Console.WriteLine("✅ APROVADOS (≥ 7.0):");
    if (aprovados.Count > 0)
    {
        foreach (var aluno in aprovados)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
    Console.WriteLine();
    
    // Mostrar recuperação
    Console.WriteLine("⚠️  RECUPERAÇÃO (5.0 - 6.9):");
    if (recuperacao.Count > 0)
    {
        foreach (var aluno in recuperacao)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
    Console.WriteLine();
    
    // Mostrar reprovados
    Console.WriteLine("❌ REPROVADOS (< 5.0):");
    if (reprovados.Count > 0)
    {
        foreach (var aluno in reprovados)
        {
            Console.WriteLine($"   • {aluno.nome,-20} Nota: {aluno.nota:F2}");
        }
    }
    else
    {
        Console.WriteLine("   (nenhum)");
    }
}

void RemoverAluno()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    ListarAlunos();
    Console.WriteLine();
    Console.Write("Digite o número do aluno para remover (0 para cancelar): ");
    
    if (!int.TryParse(Console.ReadLine(), out int indice) || 
        indice < 0 || indice > nomes.Count)
    {
        Console.WriteLine("❌ Número inválido!");
        return;
    }
    
    if (indice == 0)
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }
    
    indice--;  // Ajustar para índice 0-based
    
    // Confirmar remoção
    Console.Write($"Tem certeza que deseja remover '{nomes[indice]}'? (S/N): ");
    string confirmacao = Console.ReadLine()?.Trim().ToUpper() ?? "";
    
    if (confirmacao == "S" || confirmacao == "SIM")
    {
        string nomeRemovido = nomes[indice];
        nomes.RemoveAt(indice);
        notas.RemoveAt(indice);
        Console.WriteLine($"✅ Aluno '{nomeRemovido}' removido com sucesso!");
    }
    else
    {
        Console.WriteLine("Operação cancelada.");
    }
}

void EditarNota()
{
    if (nomes.Count == 0)
    {
        Console.WriteLine("📋 Nenhum aluno cadastrado ainda.");
        return;
    }
    
    ListarAlunos();
    Console.WriteLine();
    Console.Write("Digite o número do aluno para editar a nota (0 para cancelar): ");
    
    if (!int.TryParse(Console.ReadLine(), out int indice) || 
        indice < 0 || indice > nomes.Count)
    {
        Console.WriteLine("❌ Número inválido!");
        return;
    }
    
    if (indice == 0)
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }
    
    indice--;  // Ajustar para índice 0-based
    
    Console.WriteLine($"\nAluno: {nomes[indice]}");
    Console.WriteLine($"Nota atual: {notas[indice]:F2}");
    Console.Write("Nova nota (0-10): ");
    
    if (!double.TryParse(Console.ReadLine(), out double novaNota) || 
        novaNota < 0 || novaNota > 10)
    {
        Console.WriteLine("❌ Nota inválida! Deve estar entre 0 e 10.");
        return;
    }
    
    double notaAntiga = notas[indice];
    notas[indice] = novaNota;
    
    Console.WriteLine($"✅ Nota atualizada!");
    Console.WriteLine($"   Anterior: {notaAntiga:F2} ({ObterSituacao(notaAntiga)})");
    Console.WriteLine($"   Nova: {novaNota:F2} ({ObterSituacao(novaNota)})");
}

// ═══════════════════════════════════════════════════════════
// FUNÇÕES AUXILIARES
// ═══════════════════════════════════════════════════════════

string ObterSituacao(double nota)
{
    return nota switch
    {
        >= 9.0 => "Excelente",
        >= 7.0 => "Aprovado",
        >= 5.0 => "Recuperação",
        _ => "Reprovado"
    };
}

string ObterEmoji(double nota)
{
    return nota switch
    {
        >= 9.0 => "🏆",
        >= 7.0 => "✅",
        >= 5.0 => "⚠️",
        _ => "❌"
    };
}

/*
 * ═══════════════════════════════════════════════════════════
 * EXPLICAÇÃO TÉCNICA - PROJETO COMPLETO
 * ═══════════════════════════════════════════════════════════
 * 
 * Este projeto integra TODOS os conceitos do Dia 1:
 * 
 * 1. VARIÁVEIS E TIPOS DE DADOS:
 *    - string: nomes, opções
 *    - double: notas
 *    - int: índices, contadores
 *    - bool: confirmações
 * 
 * 2. ESTRUTURAS DE CONTROLE:
 *    - if/else: validações
 *    - switch: menu principal
 *    - switch expression: classificações
 *    - for: iteração sobre arrays
 *    - foreach: iteração simplificada
 *    - do-while: loop do menu
 *    - while: validações repetidas
 * 
 * 3. COLEÇÕES:
 *    - List<T>: listas dinâmicas
 *    - Arrays paralelos (nomes + notas)
 *    - Tuplas: (string nome, double nota)
 * 
 * 4. FUNÇÕES (LOCAL FUNCTIONS):
 *    - void: ações sem retorno
 *    - string: funções que retornam texto
 *    - Parâmetros e retornos
 * 
 * ═══════════════════════════════════════════════════════════
 * PADRÕES E TÉCNICAS UTILIZADAS
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. ESTRUTURA DO MENU:
 * 
 *    do
 *    {
 *        // Mostrar opções
 *        // Ler escolha
 *        // Processar com switch
 *    } while (opcao != "0");
 *    
 *    Vantagens:
 *    - Sempre mostra menu pelo menos uma vez
 *    - Loop contínuo até usuário sair
 *    - Código organizado
 * 
 * 2. LISTAS PARALELAS:
 * 
 *    List<string> nomes = new();
 *    List<double> notas = new();
 *    
 *    nomes[0] corresponde a notas[0]
 *    nomes[1] corresponde a notas[1]
 *    ...
 *    
 *    Alternativa (melhor):
 *    - Criar uma classe Aluno
 *    - List<Aluno> alunos
 *    (Veremos no Dia 2!)
 * 
 * 3. VALIDAÇÃO DE ENTRADA:
 * 
 *    if (!double.TryParse(input, out double valor) || 
 *        valor < 0 || valor > 10)
 *    {
 *        // Entrada inválida
 *        return;
 *    }
 *    
 *    Componentes:
 *    - TryParse: converte e valida tipo
 *    - Validação de range: valor < 0 || valor > 10
 *    - Early return: sai da função se inválido
 * 
 * 4. BUSCA EM LISTA:
 * 
 *    Método 1 (manual):
 *    for (int i = 0; i < lista.Count; i++)
 *    {
 *        if (lista[i] == valor)
 *            return i;
 *    }
 *    
 *    Método 2 (LINQ - Dia 4):
 *    lista.Any(x => x == valor)
 *    lista.FirstOrDefault(x => x == valor)
 * 
 * 5. FORMATAÇÃO DE STRINGS:
 * 
 *    {valor,-20}  → Alinha à esquerda, 20 caracteres
 *    {valor,20}   → Alinha à direita, 20 caracteres
 *    {valor:F2}   → 2 casas decimais: 7.50
 *    {valor:P1}   → Percentual: 75.0%
 *    
 *    Exemplo:
 *    double nota = 7.5;
 *    Console.WriteLine($"Nota: {nota,-8:F2}");
 *    // Output: "Nota: 7.50    "
 * 
 * 6. NULL-COALESCING OPERATOR (??):
 * 
 *    string input = Console.ReadLine() ?? "";
 *                                      ↑
 *                   Se for null, usa ""
 *    
 *    Útil para evitar NullReferenceException:
 *    string nome = obterNome() ?? "Desconhecido";
 * 
 * 7. NULL-CONDITIONAL OPERATOR (?.):
 * 
 *    string input = Console.ReadLine()?.Trim() ?? "";
 *                                     ↑
 *                   Só chama Trim() se não for null
 *    
 *    Equivalente a:
 *    string temp = Console.ReadLine();
 *    string input = temp != null ? temp.Trim() : "";
 * 
 * ═══════════════════════════════════════════════════════════
 * MELHORIAS POSSÍVEIS (Para o Futuro)
 * ═══════════════════════════════════════════════════════════
 * 
 * 1. USAR CLASSE (DIA 2):
 * 
 *    class Aluno
 *    {
 *        public string Nome { get; set; }
 *        public double Nota { get; set; }
 *        public string Situacao => ObterSituacao(Nota);
 *    }
 *    
 *    List<Aluno> alunos = new();
 * 
 * 2. PERSISTÊNCIA DE DADOS (DIA 6):
 * 
 *    - Salvar em arquivo JSON
 *    - Carregar ao iniciar
 *    - Manter dados entre execuções
 * 
 * 3. LINQ (DIA 4):
 * 
 *    var aprovados = alunos.Where(a => a.Nota >= 7.0);
 *    var media = alunos.Average(a => a.Nota);
 *    var melhorAluno = alunos.OrderByDescending(a => a.Nota).First();
 * 
 * 4. TRATAMENTO DE EXCEÇÕES (DIA 5):
 * 
 *    try
 *    {
 *        // Código que pode falhar
 *    }
 *    catch (Exception ex)
 *    {
 *        Console.WriteLine($"Erro: {ex.Message}");
 *    }
 * 
 * 5. INTERFACE GRÁFICA:
 * 
 *    - Windows Forms
 *    - WPF
 *    - Blazor (web)
 * 
 * ═══════════════════════════════════════════════════════════
 * ESTRUTURA DE CÓDIGO LIMPO
 * ═══════════════════════════════════════════════════════════
 * 
 * ✅ Organização:
 *    1. Variáveis globais no topo
 *    2. Loop principal do menu
 *    3. Funções específicas
 *    4. Funções auxiliares
 * 
 * ✅ Nomenclatura:
 *    - Funções: PascalCase (AdicionarAluno)
 *    - Variáveis: camelCase (nomeAluno)
 *    - Constantes: UPPER_CASE (MAX_NOTA)
 * 
 * ✅ Responsabilidade única:
 *    - Cada função faz UMA coisa
 *    - Funções pequenas e focadas
 *    - Reutilização de código
 * 
 * ✅ Validações:
 *    - Sempre validar entrada do usuário
 *    - Mensagens claras de erro
 *    - Early returns para casos especiais
 * 
 * ✅ Feedback ao usuário:
 *    - Emojis para visual feedback
 *    - Mensagens descritivas
 *    - Confirmações para ações destrutivas
 * 
 * ═══════════════════════════════════════════════════════════
 * EXERCÍCIOS DE EXTENSÃO
 * ═══════════════════════════════════════════════════════════
 * 
 * Tente implementar:
 * 
 * 1. Ordenação:
 *    - Ordenar alunos por nome (A-Z)
 *    - Ordenar por nota (maior primeiro)
 * 
 * 2. Relatórios:
 *    - Gerar relatório em texto
 *    - Mostrar gráfico ASCII das notas
 * 
 * 3. Múltiplas notas:
 *    - Cada aluno tem várias notas
 *    - Calcular média por aluno
 * 
 * 4. Disciplinas:
 *    - Gerenciar múltiplas disciplinas
 *    - Cada disciplina tem seus alunos e notas
 * 
 * 5. Importar/Exportar:
 *    - Ler de arquivo CSV
 *    - Exportar para CSV/JSON
 * 
 * 6. Histórico:
 *    - Registrar todas as alterações
 *    - Permitir desfazer ações
 * 
 * ═══════════════════════════════════════════════════════════
 */
```

---

