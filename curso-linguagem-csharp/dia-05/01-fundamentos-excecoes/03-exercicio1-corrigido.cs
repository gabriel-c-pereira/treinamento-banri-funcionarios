using System;

namespace ExerciciosExcecoes
{
    public class Calculadora
    {
        public double Somar(double a, double b) => a + b;
        public double Subtrair(double a, double b) => a - b;
        public double Multiplicar(double a, double b) => a * b;
        
        public double Dividir(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Não é possível dividir por zero");
            
            return a / b;
        }
    }

    class Program
    {
        static void Main()
        {
            var calc = new Calculadora();
            
            try
            {
                Console.Write("Digite o primeiro número: ");
                double num1 = double.Parse(Console.ReadLine());
                
                Console.Write("Digite o segundo número: ");
                double num2 = double.Parse(Console.ReadLine());
                
                Console.Write("Digite a operação (+ - * /): ");
                string op = Console.ReadLine();
                
                double resultado = op switch
                {
                    "+" => calc.Somar(num1, num2),
                    "-" => calc.Subtrair(num1, num2),
                    "*" => calc.Multiplicar(num1, num2),
                    "/" => calc.Dividir(num1, num2),
                    _ => throw new InvalidOperationException($"Operação '{op}' não suportada")
                };
                
                Console.WriteLine($"Resultado: {resultado}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Erro: Entrada inválida. Digite apenas números.");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Operação finalizada.");
            }
        }
    }
}

// ═══════════════════════════════════════════════════

using System;

namespace ExerciciosExcecoes.Avancado
{
    public class Calculadora
    {
        public double Somar(double a, double b) => a + b;
        public double Subtrair(double a, double b) => a - b;
        public double Multiplicar(double a, double b) => a * b;
        
        public double Dividir(double a, double b)
        {
            if (double.IsNaN(b) || double.IsInfinity(b))
                throw new ArgumentException("Divisor não pode ser NaN ou Infinity", nameof(b));
            
            if (b == 0)
                throw new DivideByZeroException("Não é possível dividir por zero");
            
            return a / b;
        }
    }

    class Program
    {
        static void Main()
        {
            var calc = new Calculadora();
            bool continuar = true;
            
            while (continuar)
            {
                try
                {
                    double num1 = LerNumero("Digite o primeiro número");
                    double num2 = LerNumero("Digite o segundo número");
                    char operacao = LerOperacao();
                    
                    double resultado = ExecutarOperacao(calc, num1, num2, operacao);
                    
                    Console.WriteLine($"\n✓ Resultado: {num1} {operacao} {num2} = {resultado}");
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"\n✗ Erro de divisão: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"\n✗ Operação inválida: {ex.Message}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nOperação cancelada pelo usuário.");
                    break;
                }
                catch (Exception ex)
                {
                    // Catch-all para erros inesperados (não esconde bugs)
                    Console.WriteLine($"\n✗ Erro inesperado: {ex.Message}");
                    Console.WriteLine($"Tipo: {ex.GetType().Name}");
                }
                finally
                {
                    Console.WriteLine("Operação finalizada.");
                }
                
                Console.Write("\nDeseja fazer outra operação? (s/n): ");
                continuar = Console.ReadLine()?.ToLower() == "s";
                Console.WriteLine();
            }
            
            Console.WriteLine("Calculadora encerrada. Pressione qualquer tecla...");
            Console.ReadKey();
        }
        
        static double LerNumero(string mensagem)
        {
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    throw new OperationCanceledException("Entrada vazia");
                
                if (double.TryParse(input, out double numero))
                    return numero;
                
                Console.WriteLine("✗ Entrada inválida. Digite um número válido.");
            }
        }
        
        static char LerOperacao()
        {
            Console.Write("Digite a operação (+ - * /): ");
            string input = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(input))
                throw new OperationCanceledException("Operação não informada");
            
            if (input.Length != 1 || !"+-*/".Contains(input))
                throw new InvalidOperationException(
                    $"Operação '{input}' inválida. Use +, -, * ou /");
            
            return input[0];
        }
        
        static double ExecutarOperacao(Calculadora calc, double a, double b, char op)
        {
            return op switch
            {
                '+' => calc.Somar(a, b),
                '-' => calc.Subtrair(a, b),
                '*' => calc.Multiplicar(a, b),
                '/' => calc.Dividir(a, b),
                _ => throw new InvalidOperationException($"Operação '{op}' não suportada")
            };
        }
    }
}