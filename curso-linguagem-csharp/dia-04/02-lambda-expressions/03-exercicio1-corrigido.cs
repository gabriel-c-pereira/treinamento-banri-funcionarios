// Exercício 1: Lambda Básico
// Objetivo: Criar expressões lambda simples para operações comuns

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("      EXPRESSÕES LAMBDA BÁSICAS        ");
Console.WriteLine("═══════════════════════════════════════");

// 1. Lambda para verificar se número é par
Func<int, bool> ehPar = numero => numero % 2 == 0;

// 2. Lambda para calcular quadrado
Func<int, int> calcularQuadrado = numero => numero * numero;

// 3. Lambda para converter string para maiúsculo
Func<string, string> paraMaiusculo = texto => texto.ToUpper();

// Testando as lambdas
Console.WriteLine("\n📝 TESTANDO EXPRESSÕES LAMBDA:\n");

// Teste 1: Verificar se é par
Console.WriteLine("1️⃣ Verificador de número par:");
int[] numeros = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
foreach (int num in numeros)
{
    string resultado = ehPar(num) ? "PAR" : "ÍMPAR";
    Console.WriteLine($"   {num} é {resultado}");
}

// Teste 2: Calcular quadrados
Console.WriteLine("\n2️⃣ Calculadora de quadrados:");
int[] valores = { 1, 2, 3, 4, 5 };
foreach (int val in valores)
{
    int quadrado = calcularQuadrado(val);
    Console.WriteLine($"   {val}² = {quadrado}");
}

// Teste 3: Converter para maiúsculo
Console.WriteLine("\n3️⃣ Conversor para MAIÚSCULO:");
string[] palavras = { "hello", "world", "csharp", "lambda" };
foreach (string palavra in palavras)
{
    string maiusculo = paraMaiusculo(palavra);
    Console.WriteLine($"   {palavra} → {maiusculo}");
}

Console.WriteLine("\n═══════════════════════════════════════");

/*
 * CONCEITOS IMPORTANTES:
 * 
 * 1. EXPRESSÃO LAMBDA:
 *    - Sintaxe: parametro => expressao
 *    - Forma concisa de escrever funções anônimas
 *    - Exemplo: x => x * 2
 * 
 * 2. FUNC<T, TResult>:
 *    - Delegate que retorna um valor
 *    - Func<int, bool>: recebe int, retorna bool
 *    - Func<string, string>: recebe string, retorna string
 * 
 * 3. SINTAXE LAMBDA:
 *    - Um parâmetro: x => x * 2
 *    - Múltiplos parâmetros: (x, y) => x + y
 *    - Sem parâmetros: () => DateTime.Now
 *    - Com bloco: x => { return x * 2; }
 * 
 * 4. OPERADOR TERNÁRIO:
 *    - condição ? valor_true : valor_false
 *    - Usado em: ehPar(num) ? "PAR" : "ÍMPAR"
 * 
 * VANTAGENS DAS LAMBDAS:
 * ✅ Código mais conciso
 * ✅ Fácil de ler e manter
 * ✅ Integração perfeita com LINQ
 * ✅ Permite programação funcional
 */
