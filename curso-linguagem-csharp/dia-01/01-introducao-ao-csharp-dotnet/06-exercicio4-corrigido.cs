// Exercício 9 Corrigido: Formatação de Saída
// Arquivo: Program.cs

// Declarando variáveis
string nome = "João";
int idade = 28;
double altura = 1.75;
double salario = 4500.50;
DateTime hoje = DateTime.Now;

Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    EXEMPLOS DE FORMATAÇÃO EM C#       ");
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine();

// 1. CONCATENAÇÃO (forma antiga, menos recomendada)
Console.WriteLine("1. Concatenação:");
Console.WriteLine("Olá, " + nome + "! Você tem " + idade + " anos.");
Console.WriteLine();

// 2. STRING.FORMAT (forma intermediária)
Console.WriteLine("2. String.Format:");
Console.WriteLine(String.Format("Nome: {0}, Idade: {1}, Altura: {2}", nome, idade, altura));
Console.WriteLine();

// 3. INTERPOLAÇÃO DE STRINGS (RECOMENDADO - mais legível)
Console.WriteLine("3. Interpolação (Recomendado):");
Console.WriteLine($"Nome: {nome}, Idade: {idade}, Altura: {altura}");
Console.WriteLine();

// 4. FORMATAÇÃO DE NÚMEROS
Console.WriteLine("4. Formatação de Números:");
Console.WriteLine($"Altura: {altura:F2} metros");           // 2 casas decimais
Console.WriteLine($"Altura: {altura:F4} metros");           // 4 casas decimais
Console.WriteLine($"Salário: {salario:C}");                 // Formato de moeda
Console.WriteLine($"Salário: R$ {salario:N2}");             // Número com separador de milhares
Console.WriteLine($"Idade: {idade:D5}");                    // Inteiro com 5 dígitos (padding)
Console.WriteLine($"Porcentagem: {0.85:P}");                // Formato de porcentagem
Console.WriteLine();

// 5. FORMATAÇÃO DE DATA E HORA
Console.WriteLine("5. Formatação de Data e Hora:");
Console.WriteLine($"Data padrão: {hoje}");
Console.WriteLine($"Data: {hoje:dd/MM/yyyy}");              // 14/10/2025
Console.WriteLine($"Data e hora: {hoje:dd/MM/yyyy HH:mm:ss}"); // 14/10/2025 15:30:45
Console.WriteLine($"Hora: {hoje:HH:mm}");                   // 15:30
Console.WriteLine($"Data longa: {hoje:dddd, dd MMMM yyyy}"); // terça-feira, 14 outubro 2025
Console.WriteLine($"Data curta: {hoje:d}");                 // 14/10/2025
Console.WriteLine($"Hora curta: {hoje:t}");                 // 15:30
Console.WriteLine();

// 6. ALINHAMENTO E PADDING
Console.WriteLine("6. Alinhamento e Padding:");
Console.WriteLine($"|{nome,-20}|"); // Alinhado à esquerda, 20 caracteres
Console.WriteLine($"|{nome,20}|");  // Alinhado à direita, 20 caracteres
Console.WriteLine($"|{idade,5}|");  // Número alinhado à direita, 5 caracteres
Console.WriteLine();

// 7. COMBINANDO FORMATAÇÕES
Console.WriteLine("7. Formatações Combinadas:");
Console.WriteLine($"|{"Nome",-15}|{"Idade",10}|{"Altura",10}|");
Console.WriteLine($"|{"-",-15}|{"-",10}|{"-",10}|");
Console.WriteLine($"|{nome,-15}|{idade,10}|{altura,10:F2}|");
Console.WriteLine();

// 8. ESCAPE DE CARACTERES ESPECIAIS
Console.WriteLine("8. Caracteres Especiais:");
Console.WriteLine("Aspas: \"Olá\"");
Console.WriteLine("Barra invertida: C:\\Users\\João");
Console.WriteLine("Quebra de linha: Linha 1\nLinha 2");
Console.WriteLine("Tab: Coluna1\tColuna2");
Console.WriteLine();

// 9. STRINGS VERBATIM (@ antes da string)
Console.WriteLine("9. Strings Verbatim:");
string caminho = @"C:\Users\João\Documents\arquivo.txt";
Console.WriteLine($"Caminho: {caminho}");
Console.WriteLine();

// 10. RAW STRING LITERALS (C# 11+) - Multiline sem escapes
Console.WriteLine("10. Raw String Literals:");
string json = """
    {
        "nome": "João",
        "idade": 28,
        "altura": 1.75
    }
    """;
Console.WriteLine(json);

Console.WriteLine();
Console.WriteLine("═══════════════════════════════════════");

/*
 * ═══════════════════════════════════════════════════════════════
 * GUIA RÁPIDO DE FORMATAÇÃO
 * ═══════════════════════════════════════════════════════════════
 * 
 * NÚMEROS:
 * ────────
 * {valor:C}      → Moeda (Currency): R$ 1.234,56
 * {valor:N2}     → Número com 2 decimais: 1.234,56
 * {valor:F2}     → Fixo 2 decimais: 1234.56
 * {valor:P}      → Porcentagem: 85,00%
 * {valor:E}      → Científico: 1.234560E+003
 * {valor:D5}     → Inteiro 5 dígitos: 00123
 * 
 * DATAS:
 * ──────
 * {data:d}       → Data curta: 14/10/2025
 * {data:D}       → Data longa: terça-feira, 14 de outubro de 2025
 * {data:t}       → Hora curta: 15:30
 * {data:T}       → Hora longa: 15:30:45
 * {data:g}       → Data/hora curta: 14/10/2025 15:30
 * {data:G}       → Data/hora longa: 14/10/2025 15:30:45
 * 
 * PERSONALIZADO:
 * ──────────────
 * {data:dd/MM/yyyy}           → 14/10/2025
 * {data:dd/MM/yyyy HH:mm:ss}  → 14/10/2025 15:30:45
 * {data:yyyy-MM-dd}           → 2025-10-14 (ISO 8601)
 * {data:dddd}                 → terça-feira
 * {data:MMMM}                 → outubro
 * 
 * ALINHAMENTO:
 * ────────────
 * {valor,10}     → Alinhado à direita, 10 chars
 * {valor,-10}    → Alinhado à esquerda, 10 chars
 * {valor,10:F2}  → Combinado: alinhamento + formato
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * BOAS PRÁTICAS:
 * 
 * ✅ USE interpolação ($"") para melhor legibilidade
 * ✅ USE :F2 para valores monetários e medidas
 * ✅ USE :C para exibir moeda com símbolo
 * ✅ USE dd/MM/yyyy para datas no Brasil
 * ✅ USE @ para caminhos de arquivo
 * ✅ USE """ (raw strings) para JSON/XML multilinha
 * 
 * ❌ EVITE concatenação (+) com muitas variáveis
 * ❌ EVITE String.Format quando interpolação é mais clara
 * ❌ EVITE hardcoded separadores de decimal
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * LINKS ÚTEIS:
 * 
 * • Standard format strings:
 *   https://learn.microsoft.com/dotnet/standard/base-types/standard-numeric-format-strings
 * 
 * • Custom format strings:
 *   https://learn.microsoft.com/dotnet/standard/base-types/custom-numeric-format-strings
 * 
 * • Date and time formats:
 *   https://learn.microsoft.com/dotnet/standard/base-types/standard-date-and-time-format-strings
 * 
 * ═══════════════════════════════════════════════════════════════
 */