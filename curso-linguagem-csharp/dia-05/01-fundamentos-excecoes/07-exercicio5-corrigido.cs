using System;

namespace ExerciciosExcecoes
{
    public class HttpException : Exception
    {
        public int StatusCode { get; }
        
        public HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    class Program
    {
        private static Random _random = new Random();
        private static int _tentativa = 0;
        
        static void Main()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"\n=== Requisição #{i + 1} ===");
                
                try
                {
                    ProcessarRequisicao();
                    Console.WriteLine("✓ Requisição processada com sucesso");
                }
                catch (HttpException ex) when (ex.StatusCode == 404)
                {
                    // Captura 404: não re-lança (recurso opcional)
                    Console.WriteLine($"⚠ Recurso não encontrado (404)");
                }
                catch (HttpException ex) when (ex.StatusCode >= 500 && ex.StatusCode < 600)
                {
                    // Captura 5xx: loga e re-lança (erro crítico)
                    Console.WriteLine($"✗ Erro no servidor ({ex.StatusCode}): {ex.Message}");
                    Console.WriteLine("⚠ Aplicação deve tentar novamente ou alertar ops");
                    // throw; // Descomente para re-lançar
                }
                catch (HttpException ex) when (ex.StatusCode == 400)
                {
                    // Não captura 400: deixa propagar
                    // Este catch nunca será atingido por causa do when
                    Console.WriteLine("Nunca executa");
                }
                catch (Exception ex) when (LogarExcecao(ex))
                {
                    // Filtro com side-effect: loga mas NÃO captura
                    // LogarExcecao retorna false, então exceção continua propagando
                    Console.WriteLine("Também nunca executa");
                }
                catch (Exception ex)
                {
                    // Catch-all para qualquer outra exceção
                    Console.WriteLine($"✗ Erro inesperado: {ex.GetType().Name} - {ex.Message}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla...");
            Console.ReadKey();
        }
        
        static void ProcessarRequisicao()
        {
            _tentativa++;
            
            // Simula diferentes cenários
            int cenario = _random.Next(0, 100);
            
            if (cenario < 10) // 10% sucesso
            {
                Console.WriteLine("→ Chamada HTTP bem-sucedida");
                return;
            }
            else if (cenario < 30) // 20% 404
            {
                throw new HttpException(404, "Endpoint não encontrado");
            }
            else if (cenario < 50) // 20% 500
            {
                throw new HttpException(500, "Erro interno do servidor");
            }
            else if (cenario < 65) // 15% 503
            {
                throw new HttpException(503, "Serviço temporariamente indisponível");
            }
            else if (cenario < 80) // 15% 400
            {
                throw new HttpException(400, "Requisição inválida");
            }
            else // 20% outro erro
            {
                throw new InvalidOperationException("Falha de rede");
            }
        }
        
        static bool LogarExcecao(Exception ex)
        {
            // Loga a exceção mas retorna FALSE para NÃO capturá-la
            Console.WriteLine($"[LOG] Exceção detectada: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"[LOG] Stack trace: {ex.StackTrace?.Split('\n')[0]}");
            
            // Retorna false: filtro não corresponde, exceção continua propagando
            return false;
        }
    }
}

// ═══════════════════════════════════════════════════

// ❌ Catch + Re-throw: Modifica stack trace
try
{
    ProcessarRequisicao();
}
catch (HttpException ex)
{
    if (ex.StatusCode != 404)
        throw; // Stack trace mostra AQUI como ponto de throw
    
    // Trata 404
}

// ✅ Filter: Preserva stack trace original
try
{
    ProcessarRequisicao();
}
catch (HttpException ex) when (ex.StatusCode == 404)
{
    // Stack trace preservado, mostra ponto original
}