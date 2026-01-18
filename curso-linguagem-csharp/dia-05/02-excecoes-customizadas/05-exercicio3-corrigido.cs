using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcecoesCustomizadas.Ex03
{
    // ========== CAMADA DE DADOS ==========
    
    /// <summary>
    /// Exceção da camada de dados (repositórios).
    /// </summary>
    public class RepositorioException : Exception
    {
        public string NomeRepositorio { get; }
        public string Operacao { get; }
        public DateTime Timestamp { get; }
        
        public RepositorioException(
            string nomeRepositorio,
            string operacao,
            Exception innerException)
            : base(
                $"Erro no repositório '{nomeRepositorio}' durante operação '{operacao}'",
                innerException)
        {
            NomeRepositorio = nomeRepositorio;
            Operacao = operacao;
            Timestamp = DateTime.UtcNow;
        }
    }
    
    public record Produto(int Id, string Nome, decimal Preco, int Estoque);
    
    public class ProdutoRepository
    {
        private readonly List<Produto> _produtos = new()
        {
            new(1, "Notebook", 3500m, 10),
            new(2, "Mouse", 50m, 100),
            new(3, "Teclado", 150m, 50)
        };
        
        private readonly Random _random = new();
        
        public Produto Buscar(int id)
        {
            try
            {
                // Simula erro aleatório de banco de dados
                if (_random.Next(0, 100) < 30) // 30% de chance de erro
                {
                    throw new InvalidOperationException(
                        "Connection timeout: Unable to connect to database server");
                }
                
                var produto = _produtos.FirstOrDefault(p => p.Id == id);
                
                if (produto == null)
                {
                    // Simula exceção de dados não encontrados
                    throw new KeyNotFoundException($"Produto ID {id} não existe no banco");
                }
                
                return produto;
            }
            catch (Exception ex)
            {
                // Wrappea QUALQUER exceção da camada de dados
                throw new RepositorioException(
                    nameof(ProdutoRepository),
                    nameof(Buscar),
                    ex);
            }
        }
    }
    
    // ========== CAMADA DE NEGÓCIO ==========
    
    /// <summary>
    /// Exceção da camada de serviços (regras de negócio).
    /// </summary>
    public class ServicoException : Exception
    {
        public string NomeServico { get; }
        public Dictionary<string, object> Contexto { get; }
        public DateTime Timestamp { get; }
        
        public ServicoException(
            string nomeServico,
            Dictionary<string, object> contexto,
            Exception innerException)
            : base(
                $"Erro no serviço '{nomeServico}'",
                innerException)
        {
            NomeServico = nomeServico;
            Contexto = contexto ?? new Dictionary<string, object>();
            Timestamp = DateTime.UtcNow;
        }
        
        public ServicoException(
            string nomeServico,
            string mensagem,
            Dictionary<string, object>? contexto = null)
            : base($"Erro no serviço '{nomeServico}': {mensagem}")
        {
            NomeServico = nomeServico;
            Contexto = contexto ?? new Dictionary<string, object>();
            Timestamp = DateTime.UtcNow;
        }
    }
    
    public class ProdutoService
    {
        private readonly ProdutoRepository _repository;
        
        public ProdutoService(ProdutoRepository repository)
        {
            _repository = repository;
        }
        
        public Produto ObterProduto(int produtoId, string usuarioId)
        {
            try
            {
                var produto = _repository.Buscar(produtoId);
                
                // Regra de negócio: produto sem estoque não pode ser vendido
                if (produto.Estoque == 0)
                {
                    throw new ServicoException(
                        nameof(ProdutoService),
                        "Produto sem estoque disponível",
                        new Dictionary<string, object>
                        {
                            ["ProdutoId"] = produtoId,
                            ["ProdutoNome"] = produto.Nome,
                            ["UsuarioId"] = usuarioId
                        });
                }
                
                return produto;
            }
            catch (RepositorioException ex)
            {
                // Wrappea exceção da camada inferior
                throw new ServicoException(
                    nameof(ProdutoService),
                    new Dictionary<string, object>
                    {
                        ["ProdutoId"] = produtoId,
                        ["UsuarioId"] = usuarioId,
                        ["Operacao"] = "ObterProduto"
                    },
                    ex);
            }
            catch (ServicoException)
            {
                // Re-lança exceções já da camada de serviço
                throw;
            }
            catch (Exception ex)
            {
                // Wrappea exceções inesperadas
                throw new ServicoException(
                    nameof(ProdutoService),
                    new Dictionary<string, object>
                    {
                        ["ProdutoId"] = produtoId,
                        ["UsuarioId"] = usuarioId,
                        ["ErroInesperado"] = true
                    },
                    ex);
            }
        }
    }
    
    // ========== UTILITÁRIOS ==========
    
    public static class ExceptionHelper
    {
        /// <summary>
        /// Percorre toda a cadeia de InnerException e retorna árvore formatada.
        /// </summary>
        public static void ExibirCadeiaCompleta(Exception ex, int nivel = 0)
        {
            string indentacao = new string(' ', nivel * 4);
            string prefixo = nivel == 0 ? "" : "└─ ";
            
            Console.WriteLine($"{indentacao}{prefixo}{ex.GetType().Name}: {ex.Message}");
            
            // Exibe propriedades customizadas
            switch (ex)
            {
                case RepositorioException repoEx:
                    Console.WriteLine($"{indentacao}    Repositório: {repoEx.NomeRepositorio}");
                    Console.WriteLine($"{indentacao}    Operação: {repoEx.Operacao}");
                    Console.WriteLine($"{indentacao}    Timestamp: {repoEx.Timestamp:HH:mm:ss.fff}");
                    break;
                    
                case ServicoException svcEx:
                    Console.WriteLine($"{indentacao}    Serviço: {svcEx.NomeServico}");
                    if (svcEx.Contexto.Any())
                    {
                        Console.WriteLine($"{indentacao}    Contexto:");
                        foreach (var kvp in svcEx.Contexto)
                        {
                            Console.WriteLine($"{indentacao}      - {kvp.Key}: {kvp.Value}");
                        }
                    }
                    break;
            }
            
            // Recursivamente exibe InnerException
            if (ex.InnerException != null)
            {
                ExibirCadeiaCompleta(ex.InnerException, nivel + 1);
            }
        }
        
        /// <summary>
        /// Extrai todas as exceções da cadeia em lista flat.
        /// </summary>
        public static List<Exception> ObterTodasExcecoes(Exception ex)
        {
            var lista = new List<Exception>();
            var atual = ex;
            
            while (atual != null)
            {
                lista.Add(atual);
                atual = atual.InnerException;
            }
            
            return lista;
        }
    }
    
    class Program
    {
        static void Main()
        {
            var repository = new ProdutoRepository();
            var service = new ProdutoService(repository);
            
            Console.WriteLine("Simulando chamadas ao serviço...\n");
            
            for (int tentativa = 1; tentativa <= 5; tentativa++)
            {
                Console.WriteLine($"=== Tentativa #{tentativa} ===");
                
                try
                {
                    var produto = service.ObterProduto(1, "user-123");
                    Console.WriteLine($"✓ Sucesso: {produto.Nome} - {produto.Preco:C}");
                }
                catch (ServicoException ex)
                {
                    Console.WriteLine("✗ ERRO CAPTURADO:\n");
                    ExceptionHelper.ExibirCadeiaCompleta(ex);
                    
                    Console.WriteLine("\n--- Análise da Cadeia ---");
                    var todasExcecoes = ExceptionHelper.ObterTodasExcecoes(ex);
                    Console.WriteLine($"Total de exceções na cadeia: {todasExcecoes.Count}");
                    
                    for (int i = 0; i < todasExcecoes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {todasExcecoes[i].GetType().Name}");
                    }
                }
                
                Console.WriteLine();
            }
            
            Console.WriteLine("Pressione qualquer tecla...");
            Console.ReadKey();
        }
    }
}