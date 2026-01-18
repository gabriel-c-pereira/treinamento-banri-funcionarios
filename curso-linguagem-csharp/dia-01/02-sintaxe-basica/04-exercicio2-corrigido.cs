// Exercício 2: Conversor de Temperaturas
// Objetivo: Trabalhar com tipos numéricos e conversões

double celsius = 25.0;

// Conversões
double fahrenheit = celsius * 9 / 5 + 32;
double kelvin = celsius + 273.15;

// Exibindo os resultados
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine("    CONVERSOR DE TEMPERATURAS          ");
Console.WriteLine("═══════════════════════════════════════");
Console.WriteLine($"Temperatura em Celsius: {celsius:F2}°C");
Console.WriteLine($"Temperatura em Fahrenheit: {fahrenheit:F2}°F");
Console.WriteLine($"Temperatura em Kelvin: {kelvin:F2}K");
Console.WriteLine("═══════════════════════════════════════");

/*
 * FÓRMULAS DE CONVERSÃO:
 * 
 * Celsius → Fahrenheit:  F = C × 9/5 + 32
 * Celsius → Kelvin:      K = C + 273.15
 * 
 * CONCEITOS IMPORTANTES:
 * 
 * 1. PRECISÃO DECIMAL:
 *    - Use double para cálculos com decimais
 *    - 9/5 é calculado como double automaticamente
 * 
 * 2. ORDEM DAS OPERAÇÕES:
 *    - Multiplicação e divisão antes de soma/subtração
 *    - Use parênteses quando necessário: (C * 9) / 5 + 32
 * 
 * 3. FORMATAÇÃO:
 *    - :F2 exibe 2 casas decimais
 *    - Símbolos especiais (°) podem ser usados em strings
 * 
 * VALORES DE REFERÊNCIA:
 * - Água congela: 0°C = 32°F = 273.15K
 * - Água ferve: 100°C = 212°F = 373.15K
 * - Temperatura ambiente: ~25°C = 77°F = 298.15K
 */
