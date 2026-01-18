// Exercício 10: Prática Geral - Informações Pessoais
// Objetivo: Praticar diferentes tipos de dados e interpolação de strings

// Informações pessoais
string nome = "João Silva";
int idade = 28;
double altura = 1.75;
double peso = 72.5;
bool estudante = true;
char genero = 'M';

// Cálculo do IMC (Índice de Massa Corporal)
double imc = peso / (altura * altura);

// Determinando categoria do IMC
string categoriaIMC;
if (imc < 18.5)
    categoriaIMC = "Abaixo do peso";
else if (imc < 25)
    categoriaIMC = "Peso normal";
else if (imc < 30)
    categoriaIMC = "Sobrepeso";
else
    categoriaIMC = "Obesidade";

// Exibindo informações
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("       INFORMAÇÕES PESSOAIS            ");
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine($"Nome: {nome}");
Console.WriteLine($"Idade: {idade} anos");
Console.WriteLine($"Altura: {altura:F2}m");
Console.WriteLine($"Peso: {peso:F1}kg");
Console.WriteLine($"Gênero: {genero}");
Console.WriteLine($"Estudante: {(estudante ? "Sim" : "Não")}");
Console.WriteLine("───────────────────────────────────────");
Console.WriteLine($"IMC: {imc:F2} - {categoriaIMC}");
Console.WriteLine("═══════════════════════════════════════");

/*
 * TIPOS DE DADOS UTILIZADOS:
 * 
 * string  - Texto (Nome)
 * int     - Número inteiro (Idade)
 * double  - Número decimal (Altura, Peso, IMC)
 * bool    - Verdadeiro/Falso (Estudante)
 * char    - Caractere único (Gênero)
 * 
 * CONCEITOS IMPORTANTES:
 * 
 * 1. INTERPOLAÇÃO DE STRINGS:
 *    - $"texto {variavel}" 
 *    - Mais legível que concatenação com +
 * 
 * 2. OPERADOR TERNÁRIO:
 *    - condição ? valor_se_true : valor_se_false
 *    - Usado em: {(estudante ? "Sim" : "Não")}
 * 
 * 3. FORMATAÇÃO:
 *    - :F2 = 2 casas decimais (1.75)
 *    - :F1 = 1 casa decimal (72.5)
 * 
 * 4. CÁLCULOS:
 *    - IMC = peso / (altura × altura)
 *    - Ordem de operações: parênteses primeiro
 * 
 * TABELA IMC:
 * - < 18.5  : Abaixo do peso
 * - 18.5-25 : Peso normal
 * - 25-30   : Sobrepeso
 * - > 30    : Obesidade
 */
