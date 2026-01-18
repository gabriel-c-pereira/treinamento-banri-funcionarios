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