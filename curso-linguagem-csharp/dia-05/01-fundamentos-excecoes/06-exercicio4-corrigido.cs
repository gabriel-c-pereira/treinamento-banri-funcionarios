using System;

namespace ExerciciosExcecoes
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Cenário 1: Sem Try-Catch (Exceção Não Tratada) ===\n");
            TestarCenario1();
            
            Console.WriteLine("\n=== Cenário 2: Try-Catch em MetodoA ===\n");
            TestarCenario2();
            
            Console.WriteLine("\n=== Cenário 3: Try-Catch em MetodoB ===\n");
            TestarCenario3();
        }
        
        // Cenário 1: Nenhum try-catch (crashará o programa)
        static void TestarCenario1()
        {
            try
            {
                Log("Main", "Iniciado");
                MetodoA_Cenario1();
                Log("Main", "Finalizando normalmente");
            }
            catch (Exception ex)
            {
                // Captura aqui para não crashar o exemplo
                Log("Main", $"Capturou exceção não tratada: {ex.Message}");
            }
        }
        
        static void MetodoA_Cenario1()
        {
            Log("MetodoA", "Iniciado");
            MetodoB_Cenario1();
            Log("MetodoA", "Finalizando"); // Nunca executa
        }
        
        static void MetodoB_Cenario1()
        {
            Log("MetodoB", "Iniciado");
            MetodoC_Cenario1();
            Log("MetodoB", "Finalizando"); // Nunca executa
        }
        
        static void MetodoC_Cenario1()
        {
            Log("MetodoC", "Iniciado");
            Log("MetodoC", "Lançando exceção...");
            throw new InvalidOperationException("Erro em C");
            // Volta para B → A → Main (stack unwinding)
        }
        
        // Cenário 2: Try-Catch em MetodoA
        static void TestarCenario2()
        {
            Log("Main", "Iniciado");
            MetodoA_Cenario2();
            Log("Main", "Finalizando");
        }
        
        static void MetodoA_Cenario2()
        {
            Log("MetodoA", "Iniciado");
            
            try
            {
                MetodoB_Cenario2();
            }
            catch (InvalidOperationException ex)
            {
                Log("MetodoA", $"Exceção capturada: {ex.Message}");
                // Não re-lança, então Main continua normalmente
            }
            
            Log("MetodoA", "Finalizando");
        }
        
        static void MetodoB_Cenario2()
        {
            Log("MetodoB", "Iniciado");
            MetodoC_Cenario2();
            Log("MetodoB", "Finalizando"); // Nunca executa (exceção propaga)
        }
        
        static void MetodoC_Cenario2()
        {
            Log("MetodoC", "Iniciado");
            Log("MetodoC", "Lançando exceção...");
            throw new InvalidOperationException("Erro em C");
        }
        
        // Cenário 3: Try-Catch em MetodoB
        static void TestarCenario3()
        {
            Log("Main", "Iniciado");
            MetodoA_Cenario3();
            Log("Main", "Finalizando");
        }
        
        static void MetodoA_Cenario3()
        {
            Log("MetodoA", "Iniciado");
            MetodoB_Cenario3();
            Log("MetodoA", "Finalizando");
        }
        
        static void MetodoB_Cenario3()
        {
            Log("MetodoB", "Iniciado");
            
            try
            {
                MetodoC_Cenario3();
            }
            catch (InvalidOperationException ex)
            {
                Log("MetodoB", $"Exceção capturada: {ex.Message}");
                // Trata e continua, A e Main executam normalmente
            }
            
            Log("MetodoB", "Finalizando");
        }
        
        static void MetodoC_Cenario3()
        {
            Log("MetodoC", "Iniciado");
            Log("MetodoC", "Lançando exceção...");
            throw new InvalidOperationException("Erro em C");
        }
        
        static void Log(string metodo, string mensagem)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {metodo}: {mensagem}");
        }
    }
}