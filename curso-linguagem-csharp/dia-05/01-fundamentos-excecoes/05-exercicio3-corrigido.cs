using System;
using System.IO;

namespace ExerciciosExcecoes
{
    public class GerenciadorArquivos
    {
        public void CopiarArquivo(string origem, string destino)
        {
            FileStream? fsOrigem = null;
            FileStream? fsDestino = null;
            
            try
            {
                // Abre arquivo de origem
                fsOrigem = new FileStream(origem, FileMode.Open, FileAccess.Read);
                Console.WriteLine($"✓ Arquivo origem aberto: {origem}");
                
                // Cria arquivo de destino
                fsDestino = new FileStream(destino, FileMode.Create, FileAccess.Write);
                Console.WriteLine($"✓ Arquivo destino criado: {destino}");
                
                // Copia byte a byte
                byte[] buffer = new byte[4096]; // 4KB buffer
                int bytesLidos;
                long totalBytes = 0;
                long tamanhoArquivo = fsOrigem.Length;
                
                while ((bytesLidos = fsOrigem.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsDestino.Write(buffer, 0, bytesLidos);
                    totalBytes += bytesLidos;
                    
                    // Mostra progresso
                    double percentual = (double)totalBytes / tamanhoArquivo * 100;
                    Console.Write($"\rProgresso: {percentual:F1}%");
                }
                
                Console.WriteLine("\n✓ Cópia concluída com sucesso");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\n✗ Arquivo não encontrado: {ex.FileName}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"\n✗ Sem permissão para acessar arquivo: {ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"\n✗ Erro de I/O: {ex.Message}");
                throw;
            }
            finally
            {
                // GARANTE que recursos são liberados
                if (fsOrigem != null)
                {
                    fsOrigem.Close();
                    fsOrigem.Dispose();
                    Console.WriteLine("✓ Stream de origem fechado");
                }
                
                if (fsDestino != null)
                {
                    fsDestino.Close();
                    fsDestino.Dispose();
                    Console.WriteLine("✓ Stream de destino fechado");
                }
                
                Console.WriteLine("✓ Limpeza finalizada");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var gerenciador = new GerenciadorArquivos();
            
            // Teste 1: Arquivo que existe
            Console.WriteLine("=== Teste 1: Arquivo Existente ===");
            try
            {
                gerenciador.CopiarArquivo("arquivo-origem.txt", "arquivo-copia.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.GetType().Name}");
            }
            
            Console.WriteLine("\n=== Teste 2: Arquivo Inexistente ===");
            try
            {
                gerenciador.CopiarArquivo("nao-existe.txt", "copia.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.GetType().Name}");
            }
            
            Console.WriteLine("\nPressione qualquer tecla...");
            Console.ReadKey();
        }
    }
}

// ═══════════════════════════════════════════════════

public class GerenciadorArquivos
{
    public void CopiarArquivoComUsing(string origem, string destino)
    {
        try
        {
            using var fsOrigem = new FileStream(origem, FileMode.Open, FileAccess.Read);
            using var fsDestino = new FileStream(destino, FileMode.Create, FileAccess.Write);
            
            Console.WriteLine($"✓ Arquivos abertos");
            
            byte[] buffer = new byte[4096];
            int bytesLidos;
            long totalBytes = 0;
            long tamanhoArquivo = fsOrigem.Length;
            
            while ((bytesLidos = fsOrigem.Read(buffer, 0, buffer.Length)) > 0)
            {
                fsDestino.Write(buffer, 0, bytesLidos);
                totalBytes += bytesLidos;
                
                double percentual = (double)totalBytes / tamanhoArquivo * 100;
                Console.Write($"\rProgresso: {percentual:F1}%");
            }
            
            Console.WriteLine("\n✓ Cópia concluída");
            
            // Dispose() automático ao sair do escopo
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"\n✗ Arquivo não encontrado: {ex.FileName}");
            throw;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"\n✗ Sem permissão: {ex.Message}");
            throw;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"\n✗ Erro de I/O: {ex.Message}");
            throw;
        }
    }
    
    // Versão assíncrona moderna
    public async Task CopiarArquivoAsync(string origem, string destino, 
        IProgress<double>? progresso = null)
    {
        using var fsOrigem = new FileStream(origem, FileMode.Open, FileAccess.Read);
        using var fsDestino = new FileStream(destino, FileMode.Create, FileAccess.Write);
        
        byte[] buffer = new byte[81920]; // 80KB buffer
        int bytesLidos;
        long totalBytes = 0;
        long tamanhoArquivo = fsOrigem.Length;
        
        while ((bytesLidos = await fsOrigem.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await fsDestino.WriteAsync(buffer, 0, bytesLidos);
            totalBytes += bytesLidos;
            
            progresso?.Report((double)totalBytes / tamanhoArquivo * 100);
        }
    }
}