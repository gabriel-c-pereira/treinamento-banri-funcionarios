using System;

namespace ExcecoesCustomizadas.Ex01
{
    /// <summary>
    /// Exceção lançada quando um cliente não é encontrado no sistema.
    /// </summary>
    public class ClienteNaoEncontradoException : Exception
    {
        /// <summary>
        /// ID do cliente que não foi encontrado.
        /// </summary>
        public int ClienteId { get; }
        
        /// <summary>
        /// Momento em que o erro ocorreu.
        /// </summary>
        public DateTime Timestamp { get; }
        
        public ClienteNaoEncontradoException(int clienteId)
            : base($"Cliente {clienteId} não encontrado")
        {
            ClienteId = clienteId;
            Timestamp = DateTime.UtcNow;
        }
        
        public ClienteNaoEncontradoException(int clienteId, Exception innerException)
            : base($"Cliente {clienteId} não encontrado", innerException)
        {
            ClienteId = clienteId;
            Timestamp = DateTime.UtcNow;
        }
    }
    
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
    
    public class ClienteRepository
    {
        private readonly List<Cliente> _clientes = new()
        {
            new Cliente { Id = 1, Nome = "João Silva", Email = "joao@email.com" },
            new Cliente { Id = 2, Nome = "Maria Santos", Email = "maria@email.com" },
            new Cliente { Id = 3, Nome = "Pedro Costa", Email = "pedro@email.com" }
        };
        
        public Cliente ObterCliente(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            
            if (cliente == null)
                throw new ClienteNaoEncontradoException(id);
            
            return cliente;
        }
    }
    
    class Program
    {
        static void Main()
        {
            var repository = new ClienteRepository();
            
            // Teste 1: Cliente existente
            Console.WriteLine("=== Teste 1: Cliente Existente ===");
            try
            {
                var cliente = repository.ObterCliente(1);
                Console.WriteLine($"✓ Cliente encontrado: {cliente.Nome}");
            }
            catch (ClienteNaoEncontradoException ex)
            {
                Console.WriteLine($"✗ Erro: {ex.Message}");
            }
            
            // Teste 2: Cliente inexistente
            Console.WriteLine("\n=== Teste 2: Cliente Inexistente ===");
            try
            {
                var cliente = repository.ObterCliente(999);
                Console.WriteLine($"✓ Cliente encontrado: {cliente.Nome}");
            }
            catch (ClienteNaoEncontradoException ex)
            {
                Console.WriteLine($"✗ Erro: {ex.Message}");
                Console.WriteLine($"  ClienteId: {ex.ClienteId}");
                Console.WriteLine($"  Timestamp: {ex.Timestamp:yyyy-MM-dd HH:mm:ss}");
            }
            
            Console.ReadKey();
        }
    }
}

// ═══════════════════════════════════════════════════

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExcecoesCustomizadas.Ex01.Avancado
{
    /// <summary>
    /// Exceção lançada quando um cliente não é encontrado no sistema.
    /// </summary>
    public class ClienteNaoEncontradoException : Exception
    {
        public int ClienteId { get; }
        public DateTime Timestamp { get; }
        public string? ContextoAdicional { get; }
        
        public ClienteNaoEncontradoException(int clienteId, string? contexto = null)
            : base(ConstruirMensagem(clienteId, contexto))
        {
            ClienteId = clienteId;
            Timestamp = DateTime.UtcNow;
            ContextoAdicional = contexto;
        }
        
        public ClienteNaoEncontradoException(
            int clienteId,
            Exception innerException,
            string? contexto = null)
            : base(ConstruirMensagem(clienteId, contexto), innerException)
        {
            ClienteId = clienteId;
            Timestamp = DateTime.UtcNow;
            ContextoAdicional = contexto;
        }
        
        private static string ConstruirMensagem(int clienteId, string? contexto)
        {
            var mensagem = $"Cliente {clienteId} não encontrado";
            return contexto != null ? $"{mensagem}. Contexto: {contexto}" : mensagem;
        }
        
        public override string ToString()
        {
            return $"{GetType().Name}: {Message}\n" +
                   $"ClienteId: {ClienteId}\n" +
                   $"Timestamp: {Timestamp:yyyy-MM-dd HH:mm:ss.fff}\n" +
                   $"Contexto: {ContextoAdicional ?? "N/A"}\n" +
                   $"Stack Trace:\n{StackTrace}";
        }
    }
    
    public record Cliente(int Id, string Nome, string Email, bool Ativo = true);
    
    public interface IClienteRepository
    {
        Cliente ObterCliente(int id);
        IEnumerable<Cliente> ListarTodos();
    }
    
    public class ClienteRepository : IClienteRepository
    {
        private readonly List<Cliente> _clientes;
        private readonly Action<string>? _logger;
        
        public ClienteRepository(Action<string>? logger = null)
        {
            _logger = logger;
            _clientes = new List<Cliente>
            {
                new(1, "João Silva", "joao@email.com"),
                new(2, "Maria Santos", "maria@email.com", false), // Inativo
                new(3, "Pedro Costa", "pedro@email.com")
            };
        }
        
        public Cliente ObterCliente(int id)
        {
            _logger?.Invoke($"Buscando cliente {id}...");
            
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            
            if (cliente == null)
            {
                _logger?.Invoke($"Cliente {id} não encontrado");
                throw new ClienteNaoEncontradoException(
                    id,
                    $"Busca realizada em {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            }
            
            if (!cliente.Ativo)
            {
                _logger?.Invoke($"Cliente {id} está inativo");
                throw new ClienteNaoEncontradoException(
                    id,
                    "Cliente existe mas está inativo");
            }
            
            _logger?.Invoke($"Cliente {id} encontrado: {cliente.Nome}");
            return cliente;
        }
        
        public IEnumerable<Cliente> ListarTodos() => _clientes;
    }
    
    // Testes Unitários
    public class ClienteRepositoryTests
    {
        [Fact]
        public void ObterCliente_ClienteExistente_RetornaCliente()
        {
            // Arrange
            var repository = new ClienteRepository();
            
            // Act
            var cliente = repository.ObterCliente(1);
            
            // Assert
            Assert.NotNull(cliente);
            Assert.Equal(1, cliente.Id);
            Assert.Equal("João Silva", cliente.Nome);
        }
        
        [Fact]
        public void ObterCliente_ClienteInexistente_LancaExcecao()
        {
            // Arrange
            var repository = new ClienteRepository();
            
            // Act & Assert
            var exception = Assert.Throws<ClienteNaoEncontradoException>(
                () => repository.ObterCliente(999));
            
            Assert.Equal(999, exception.ClienteId);
            Assert.NotEqual(default(DateTime), exception.Timestamp);
            Assert.Contains("999", exception.Message);
        }
        
        [Fact]
        public void ObterCliente_ClienteInativo_LancaExcecaoComContexto()
        {
            // Arrange
            var repository = new ClienteRepository();
            
            // Act & Assert
            var exception = Assert.Throws<ClienteNaoEncontradoException>(
                () => repository.ObterCliente(2));
            
            Assert.Equal(2, exception.ClienteId);
            Assert.Contains("inativo", exception.ContextoAdicional, StringComparison.OrdinalIgnoreCase);
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Logger simples
            void Log(string mensagem) =>
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {mensagem}");
            
            var repository = new ClienteRepository(Log);
            
            // Teste 1: Cliente existente
            Console.WriteLine("=== Teste 1: Cliente Existente ===\n");
            try
            {
                var cliente = repository.ObterCliente(1);
                Console.WriteLine($"✓ Cliente: {cliente.Nome} ({cliente.Email})");
            }
            catch (ClienteNaoEncontradoException ex)
            {
                ExibirErro(ex);
            }
            
            // Teste 2: Cliente inexistente
            Console.WriteLine("\n=== Teste 2: Cliente Inexistente ===\n");
            try
            {
                var cliente = repository.ObterCliente(999);
            }
            catch (ClienteNaoEncontradoException ex)
            {
                ExibirErro(ex);
            }
            
            // Teste 3: Cliente inativo
            Console.WriteLine("\n=== Teste 3: Cliente Inativo ===\n");
            try
            {
                var cliente = repository.ObterCliente(2);
            }
            catch (ClienteNaoEncontradoException ex)
            {
                ExibirErro(ex);
            }
            
            Console.WriteLine("\nPressione qualquer tecla...");
            Console.ReadKey();
        }
        
        static void ExibirErro(ClienteNaoEncontradoException ex)
        {
            Console.WriteLine("✗ ERRO CAPTURADO:");
            Console.WriteLine($"  Tipo: {ex.GetType().Name}");
            Console.WriteLine($"  Mensagem: {ex.Message}");
            Console.WriteLine($"  ClienteId: {ex.ClienteId}");
            Console.WriteLine($"  Timestamp: {ex.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            if (ex.ContextoAdicional != null)
                Console.WriteLine($"  Contexto: {ex.ContextoAdicional}");
        }
    }
}