using System;

namespace CalculadoraDebug
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Calculadora ===\n");
            
            try
            {
                Console.Write("Digite o primeiro número: ");
                int num1 = int.Parse(Console.ReadLine() ?? "0");  // ⬤ Breakpoint 1
                
                Console.Write("Digite o segundo número: ");
                int num2 = int.Parse(Console.ReadLine() ?? "0");  // ⬤ Breakpoint 2
                
                Console.Write("Digite a operação (+, -, *, /): ");
                string operacao = Console.ReadLine() ?? "+";
                
                double resultado = Calcular(num1, num2, operacao);
                Console.WriteLine($"\nResultado: {resultado}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"❌ Erro: Não é possível dividir por zero!");
                Console.WriteLine($"Detalhes: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"❌ Erro: Digite apenas números!");
                Console.WriteLine($"Detalhes: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado: {ex.Message}");
            }
        }
        
        static double Calcular(int a, int b, string op)
        {
            // ⬤ Breakpoint 3 - Início do método
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    // ⬤ Breakpoint 4 - Antes da divisão
                    if (b == 0)
                        throw new DivideByZeroException("Tentativa de divisão por zero");
                    return (double)a / b;
                default:
                    Console.WriteLine("Operação inválida, usando soma");
                    return a + b;
            }
        }
    }
}