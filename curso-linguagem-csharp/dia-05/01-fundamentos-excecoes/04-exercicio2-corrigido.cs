using System;
using System.Linq;

namespace ExerciciosExcecoes
{
    public class ValidadorCPF
    {
        public static void ValidarCPF(string cpf)
        {
            // Validação 1: Null
            if (cpf == null)
                throw new ArgumentNullException(nameof(cpf), "CPF não pode ser null");
            
            // Remove formatação
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            
            // Validação 2: Tamanho
            if (cpf.Length != 11)
                throw new ArgumentException(
                    $"CPF deve ter 11 dígitos. Recebido: {cpf.Length} dígitos",
                    nameof(cpf));
            
            // Validação 3: Todos dígitos iguais
            if (cpf.Distinct().Count() == 1)
                throw new ArgumentException(
                    $"CPF inválido: todos os dígitos são iguais ({cpf})",
                    nameof(cpf));
            
            // Validação 4: Dígitos verificadores
            if (!ValidarDigitosVerificadores(cpf))
                throw new ArgumentException(
                    $"CPF inválido: dígitos verificadores incorretos ({cpf})",
                    nameof(cpf));
        }
        
        private static bool ValidarDigitosVerificadores(string cpf)
        {
            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            
            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;
            
            if (int.Parse(cpf[9].ToString()) != digito1)
                return false;
            
            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            
            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;
            
            return int.Parse(cpf[10].ToString()) == digito2;
        }
    }

    class Program
    {
        static void Main()
        {
            string[] cpfsParaTestar = new[]
            {
                "123.456.789-09",  // Válido
                "111.111.111-11",  // Inválido: todos iguais
                "123.456",         // Inválido: tamanho
                null,              // Inválido: null
                "123.456.789-00"   // Inválido: dígito verificador
            };
            
            foreach (var cpf in cpfsParaTestar)
            {
                Console.WriteLine($"\nTestando CPF: {cpf ?? "null"}");
                
                try
                {
                    ValidadorCPF.ValidarCPF(cpf);
                    Console.WriteLine("✓ CPF válido");
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"✗ Erro: {ex.Message}");
                }
                catch (ArgumentException ex) when (ex.Message.Contains("tamanho"))
                {
                    Console.WriteLine($"✗ Erro de tamanho: {ex.Message}");
                }
                catch (ArgumentException ex) when (ex.Message.Contains("iguais"))
                {
                    Console.WriteLine($"✗ Erro de formato: {ex.Message}");
                }
                catch (ArgumentException ex) when (ex.Message.Contains("verificadores"))
                {
                    Console.WriteLine($"✗ Erro de validação: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    // Catch genérico para outros erros de argumento
                    Console.WriteLine($"✗ Erro de argumento: {ex.Message}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}

// ═══════════════════════════════════════════════════

public class ValidadorCPF
{
    // Versão que retorna bool ao invés de lançar exceção
    public static bool TryValidarCPF(string cpf, out string mensagemErro)
    {
        mensagemErro = null;
        
        if (cpf == null)
        {
            mensagemErro = "CPF não pode ser null";
            return false;
        }
        
        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        
        if (cpf.Length != 11)
        {
            mensagemErro = $"CPF deve ter 11 dígitos. Recebido: {cpf.Length}";
            return false;
        }
        
        if (cpf.Distinct().Count() == 1)
        {
            mensagemErro = "CPF inválido: todos os dígitos são iguais";
            return false;
        }
        
        if (!ValidarDigitosVerificadores(cpf))
        {
            mensagemErro = "Dígitos verificadores incorretos";
            return false;
        }
        
        return true;
    }
    
    // ... ValidarDigitosVerificadores igual ao anterior
}

// Uso
if (ValidadorCPF.TryValidarCPF(cpf, out string erro))
{
    Console.WriteLine("CPF válido");
}
else
{
    Console.WriteLine($"CPF inválido: {erro}");
}