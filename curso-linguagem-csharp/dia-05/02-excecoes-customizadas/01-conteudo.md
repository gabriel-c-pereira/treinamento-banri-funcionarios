# 02 - Exceções Customizadas

## 📚 Introdução

Exceções customizadas permitem criar tipos de erro específicos do seu domínio, tornando o tratamento de erros mais expressivo e mantendo a semântica do negócio no código de exceção. Uma exceção bem projetada comunica claramente o que deu errado e fornece contexto suficiente para debugging e recuperação.

## 🎯 Objetivos

Ao final deste tópico, você será capaz de:
- Criar exceções específicas do domínio seguindo convenções .NET
- Decidir quando criar exceções customizadas vs usar exceções built-in
- Implementar exceções com propriedades contextuais relevantes
- Usar InnerException para preservar contexto de erros
- Aplicar padrões como Exception Builder e Exception Wrapping
- Entender considerações de serialização

## 🏗️ Quando Criar Exceções Customizadas

### ✅ Crie Exceções Customizadas Quando:

1. **Erro específico do domínio**: Representa um conceito de negócio
2. **Contexto adicional**: Precisa carregar dados específicos além da mensagem
3. **Tratamento diferenciado**: Consumidor precisa tratar de forma específica
4. **Clareza semântica**: Torna o código mais expressivo

### ❌ NÃO Crie Exceções Customizadas Quando:

1. Exceção built-in já expressa o erro adequadamente
2. Não há contexto adicional a fornecer
3. Ninguém vai tratar especificamente essa exceção

## 📐 Anatomia de uma Exceção Customizada

### Estrutura Básica

```csharp
/// <summary>
/// Exceção lançada quando um pedido é inválido segundo regras de negócio.
/// </summary>
public class PedidoInvalidoException : Exception
{
    /// <summary>
    /// ID do pedido que causou o erro.
    /// </summary>
    public int PedidoId { get; }
    
    /// <summary>
    /// Motivo específico da invalidação.
    /// </summary>
    public string MotivoInvalidacao { get; }
    
    public PedidoInvalidoException(int pedidoId, string motivoInvalidacao)
        : base($"Pedido #{pedidoId} é inválido: {motivoInvalidacao}")
    {
        PedidoId = pedidoId;
        MotivoInvalidacao = motivoInvalidacao;
    }
    
    public PedidoInvalidoException(int pedidoId, string motivoInvalidacao, Exception innerException)
        : base($"Pedido #{pedidoId} é inválido: {motivoInvalidacao}", innerException)
    {
        PedidoId = pedidoId;
        MotivoInvalidacao = motivoInvalidacao;
    }
}
```

### Convenções de Nomenclatura

- Sempre termine com sufixo **Exception**
- Use substantivos que descrevem o erro
- Seja específico: `PedidoCanceladoException` > `PedidoException`

```csharp
// ✅ BOM
public class ClienteNaoEncontradoException : Exception { }
public class SaldoInsuficienteException : Exception { }
public class PagamentoDuplicadoException : Exception { }

// ❌ RUIM
public class ClienteException : Exception { }      // Muito genérico
public class Problem : Exception { }                // Não termina com Exception
public class ErroX : Exception { }                  // Nome não descritivo
```

## 🎨 Padrões de Design

### Padrão 1: Exceção com Propriedades Contextuais

```csharp
public class TransacaoBancariaException : Exception
{
    public string NumeroConta { get; }
    public decimal ValorTentado { get; }
    public decimal SaldoDisponivel { get; }
    public DateTime DataHora { get; }
    
    public TransacaoBancariaException(
        string numeroConta,
        decimal valorTentado,
        decimal saldoDisponivel,
        string mensagem)
        : base(mensagem)
    {
        NumeroConta = numeroConta;
        ValorTentado = valorTentado;
        SaldoDisponivel = saldoDisponivel;
        DataHora = DateTime.UtcNow;
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}\n" +
               $"Conta: {NumeroConta}\n" +
               $"Valor tentado: {ValorTentado:C}\n" +
               $"Saldo disponível: {SaldoDisponivel:C}\n" +
               $"Data/Hora: {DataHora:yyyy-MM-dd HH:mm:ss}";
    }
}

// Uso
throw new TransacaoBancariaException(
    "12345-6",
    1500.00m,
    800.00m,
    "Saldo insuficiente para realizar saque"
);
```

### Padrão 2: Exception Wrapping (Preservando Contexto)

```csharp
public class RepositorioException : Exception
{
    public string NomeRepositorio { get; }
    public string Operacao { get; }
    
    public RepositorioException(
        string nomeRepositorio,
        string operacao,
        Exception innerException)
        : base($"Erro no repositório '{nomeRepositorio}' durante operação '{operacao}'",
               innerException)
    {
        NomeRepositorio = nomeRepositorio;
        Operacao = operacao;
    }
}

// Uso - Camada de Dados
public class ClienteRepository
{
    public Cliente BuscarPorId(int id)
    {
        try
        {
            // Código que pode lançar SqlException
            return _dbContext.Clientes.Find(id);
        }
        catch (SqlException ex)
        {
            // Wrappea exceção técnica em exceção de domínio
            throw new RepositorioException(
                nameof(ClienteRepository),
                nameof(BuscarPorId),
                ex);  // Preserva exceção original como InnerException
        }
    }
}

// Uso - Camada de Serviço
public class ClienteService
{
    public void ProcessarCliente(int clienteId)
    {
        try
        {
            var cliente = _repository.BuscarPorId(clienteId);
            // Processa cliente
        }
        catch (RepositorioException ex)
        {
            // Pode acessar exceção original
            var sqlEx = ex.InnerException as SqlException;
            
            if (sqlEx?.Number == 1205) // Deadlock
            {
                // Retry logic
            }
            
            throw; // Re-lança com contexto preservado
        }
    }
}
```

### Padrão 3: Hierarquia de Exceções de Domínio

```csharp
// Exceção base do domínio
public abstract class PagamentoException : Exception
{
    public Guid TransacaoId { get; }
    public DateTime Timestamp { get; }
    
    protected PagamentoException(Guid transacaoId, string mensagem)
        : base(mensagem)
    {
        TransacaoId = transacaoId;
        Timestamp = DateTime.UtcNow;
    }
    
    protected PagamentoException(
        Guid transacaoId,
        string mensagem,
        Exception innerException)
        : base(mensagem, innerException)
    {
        TransacaoId = transacaoId;
        Timestamp = DateTime.UtcNow;
    }
}

// Exceções específicas
public class PagamentoRecusadoException : PagamentoException
{
    public string MotivoRecusa { get; }
    
    public PagamentoRecusadoException(
        Guid transacaoId,
        string motivoRecusa)
        : base(transacaoId, $"Pagamento recusado: {motivoRecusa}")
    {
        MotivoRecusa = motivoRecusa;
    }
}

public class PagamentoTimeoutException : PagamentoException
{
    public TimeSpan TempoEsperado { get; }
    
    public PagamentoTimeoutException(
        Guid transacaoId,
        TimeSpan tempoEsperado)
        : base(transacaoId, $"Timeout após {tempoEsperado.TotalSeconds}s")
    {
        TempoEsperado = tempoEsperado;
    }
}

public class PagamentoDuplicadoException : PagamentoException
{
    public Guid TransacaoOriginalId { get; }
    
    public PagamentoDuplicadoException(
        Guid transacaoId,
        Guid transacaoOriginalId)
        : base(transacaoId, "Pagamento duplicado detectado")
    {
        TransacaoOriginalId = transacaoOriginalId;
    }
}

// Uso - Permite tratamento específico
try
{
    await _pagamentoService.ProcessarPagamento(transacao);
}
catch (PagamentoRecusadoException ex)
{
    // Notifica cliente sobre recusa
    await NotificarClienteRecusa(ex.MotivoRecusa);
}
catch (PagamentoTimeoutException ex)
{
    // Agenda retry
    await AgendarRetry(ex.TransacaoId, ex.TempoEsperado);
}
catch (PagamentoDuplicadoException ex)
{
    // Retorna transação original
    return await BuscarTransacao(ex.TransacaoOriginalId);
}
catch (PagamentoException ex)
{
    // Tratamento genérico para outros erros de pagamento
    await LogarErroPagamento(ex);
    throw;
}
```

### Padrão 4: Exception Builder (Fluent API)

```csharp
public class ValidacaoException : Exception
{
    public List<ErroValidacao> Erros { get; }
    
    private ValidacaoException(List<ErroValidacao> erros, string mensagem)
        : base(mensagem)
    {
        Erros = erros;
    }
    
    public class Builder
    {
        private readonly List<ErroValidacao> _erros = new();
        
        public Builder AdicionarErro(string campo, string mensagem)
        {
            _erros.Add(new ErroValidacao(campo, mensagem));
            return this;
        }
        
        public Builder AdicionarErroSe(bool condicao, string campo, string mensagem)
        {
            if (condicao)
                _erros.Add(new ErroValidacao(campo, mensagem));
            return this;
        }
        
        public void LancarSeHouverErros()
        {
            if (_erros.Any())
            {
                var mensagem = $"Validação falhou com {_erros.Count} erro(s):\n" +
                              string.Join("\n", _erros.Select(e => $"- {e.Campo}: {e.Mensagem}"));
                
                throw new ValidacaoException(_erros, mensagem);
            }
        }
        
        public ValidacaoException? Construir()
        {
            return _erros.Any()
                ? new ValidacaoException(_erros, "Validação falhou")
                : null;
        }
    }
    
    public static Builder Criar() => new Builder();
}

public record ErroValidacao(string Campo, string Mensagem);

// Uso
public void ValidarUsuario(Usuario usuario)
{
    ValidacaoException.Criar()
        .AdicionarErroSe(
            string.IsNullOrWhiteSpace(usuario.Nome),
            nameof(usuario.Nome),
            "Nome é obrigatório")
        .AdicionarErroSe(
            usuario.Nome?.Length < 3,
            nameof(usuario.Nome),
            "Nome deve ter no mínimo 3 caracteres")
        .AdicionarErroSe(
            !usuario.Email.Contains("@"),
            nameof(usuario.Email),
            "Email inválido")
        .AdicionarErroSe(
            usuario.Idade < 18,
            nameof(usuario.Idade),
            "Usuário deve ser maior de idade")
        .LancarSeHouverErros();
}
```

## 🔐 Boas Práticas

### 1. Torne Exceções Imutáveis

```csharp
// ✅ BOM - Propriedades read-only
public class PedidoException : Exception
{
    public int PedidoId { get; }  // Sem setter
    
    public PedidoException(int pedidoId, string mensagem)
        : base(mensagem)
    {
        PedidoId = pedidoId;
    }
}

// ❌ RUIM - Propriedades mutáveis
public class PedidoException : Exception
{
    public int PedidoId { get; set; }  // Mutável!
}
```

### 2. Forneça Múltiplos Construtores

```csharp
public class ProcessamentoException : Exception
{
    public string RecursoId { get; }
    
    // Construtor com mensagem
    public ProcessamentoException(string recursoId, string mensagem)
        : base(mensagem)
    {
        RecursoId = recursoId;
    }
    
    // Construtor com InnerException
    public ProcessamentoException(
        string recursoId,
        string mensagem,
        Exception innerException)
        : base(mensagem, innerException)
    {
        RecursoId = recursoId;
    }
}
```

### 3. Use InnerException Para Preservar Contexto

```csharp
public void ProcessarPedido(int pedidoId)
{
    try
    {
        var pedido = _repository.Buscar(pedidoId);
        // Processa
    }
    catch (Exception ex)
    {
        // ✅ Preserva exceção original
        throw new ProcessamentoPedidoException(
            pedidoId,
            "Falha ao processar pedido",
            ex);  // InnerException preservado
        
        // ❌ Perde contexto
        // throw new ProcessamentoPedidoException(
        //     pedidoId,
        //     $"Falha: {ex.Message}");
    }
}
```

### 4. Adicione Dados Contextuais com Data Dictionary

```csharp
public void ProcessarTransacao(Transacao tx)
{
    try
    {
        // Processa transação
    }
    catch (Exception ex)
    {
        // Adiciona dados contextuais sem criar nova exceção
        ex.Data["TransacaoId"] = tx.Id;
        ex.Data["ValorTransacao"] = tx.Valor;
        ex.Data["ContaOrigem"] = tx.ContaOrigem;
        ex.Data["ContaDestino"] = tx.ContaDestino;
        ex.Data["Timestamp"] = DateTime.UtcNow;
        
        throw;  // Re-lança com dados adicionais
    }
}

// Recuperando dados
catch (Exception ex)
{
    if (ex.Data.Contains("TransacaoId"))
    {
        var txId = ex.Data["TransacaoId"];
        Console.WriteLine($"Erro na transação: {txId}");
    }
}
```

### 5. Override ToString() Para Debugging

```csharp
public class PedidoException : Exception
{
    public int PedidoId { get; }
    public string ClienteId { get; }
    public decimal ValorTotal { get; }
    
    public PedidoException(int pedidoId, string clienteId, decimal valorTotal, string mensagem)
        : base(mensagem)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
    }
    
    public override string ToString()
    {
        return $"{GetType().Name}: {Message}\n" +
               $"PedidoId: {PedidoId}\n" +
               $"ClienteId: {ClienteId}\n" +
               $"ValorTotal: {ValorTotal:C}\n" +
               $"Stack Trace:\n{StackTrace}";
    }
}
```

## 📦 Serialização (Avançado)

**Nota**: `BinaryFormatter` está obsoleto no .NET 5+. Use serialização JSON para persistência.

```csharp
using System.Text.Json.Serialization;

[Serializable]
public class ApplicationException : Exception
{
    public string ApplicationId { get; }
    public string UserId { get; }
    
    [JsonConstructor]
    public ApplicationException(string applicationId, string userId, string mensagem)
        : base(mensagem)
    {
        ApplicationId = applicationId;
        UserId = userId;
    }
    
    // Para serialização JSON
    public ApplicationException() : base() { }
}

// Uso com System.Text.Json
var exception = new ApplicationException("app-123", "user-456", "Erro grave");
string json = JsonSerializer.Serialize(exception);
var deserialized = JsonSerializer.Deserialize<ApplicationException>(json);
```

## 📊 Comparação: Custom vs Built-in

| Cenário | Use Custom | Use Built-in |
|---------|------------|--------------|
| Erro específico do domínio | ✅ `PedidoCanceladoException` | ❌ |
| Precisa contexto adicional | ✅ `SaldoInsuficienteException` | ❌ |
| Argumento nulo/inválido | ❌ | ✅ `ArgumentException` |
| Estado inválido do objeto | ❌ | ✅ `InvalidOperationException` |
| Operação não suportada | ❌ | ✅ `NotSupportedException` |
| Arquivo não encontrado | ❌ | ✅ `FileNotFoundException` |

## 🎯 Exemplo Completo: Sistema de Pagamentos

```csharp
// Exceções do domínio
public abstract class PagamentoBaseException : Exception
{
    public Guid PagamentoId { get; }
    public DateTime OcorreuEm { get; }
    
    protected PagamentoBaseException(Guid pagamentoId, string mensagem)
        : base(mensagem)
    {
        PagamentoId = pagamentoId;
        OcorreuEm = DateTime.UtcNow;
    }
    
    protected PagamentoBaseException(Guid pagamentoId, string mensagem, Exception inner)
        : base(mensagem, inner)
    {
        PagamentoId = pagamentoId;
        OcorreuEm = DateTime.UtcNow;
    }
}

public class CartaoRecusadoException : PagamentoBaseException
{
    public string NumeroCartao { get; }
    public string MotivoRecusa { get; }
    
    public CartaoRecusadoException(
        Guid pagamentoId,
        string numeroCartao,
        string motivoRecusa)
        : base(pagamentoId, $"Cartão {numeroCartao} recusado: {motivoRecusa}")
    {
        NumeroCartao = numeroCartao;
        MotivoRecusa = motivoRecusa;
    }
}

public class LimiteExcedidoException : PagamentoBaseException
{
    public decimal ValorTentado { get; }
    public decimal LimiteDisponivel { get; }
    
    public LimiteExcedidoException(
        Guid pagamentoId,
        decimal valorTentado,
        decimal limiteDisponivel)
        : base(pagamentoId, "Limite de crédito excedido")
    {
        ValorTentado = valorTentado;
        LimiteDisponivel = limiteDisponivel;
    }
}

// Serviço de pagamento
public class PagamentoService
{
    public async Task<ResultadoPagamento> ProcessarPagamento(Pagamento pagamento)
    {
        try
        {
            // Valida cartão
            if (!ValidarCartao(pagamento.NumeroCartao))
            {
                throw new CartaoRecusadoException(
                    pagamento.Id,
                    pagamento.NumeroCartao,
                    "Cartão inválido ou expirado");
            }
            
            // Verifica limite
            var limiteDisponivel = await ObterLimiteDisponivel(pagamento.NumeroCartao);
            if (pagamento.Valor > limiteDisponivel)
            {
                throw new LimiteExcedidoException(
                    pagamento.Id,
                    pagamento.Valor,
                    limiteDisponivel);
            }
            
            // Processa
            return await ExecutarPagamento(pagamento);
        }
        catch (PagamentoBaseException)
        {
            // Re-lança exceções de domínio
            throw;
        }
        catch (Exception ex)
        {
            // Wrappea exceções técnicas
            throw new PagamentoBaseException(
                pagamento.Id,
                "Erro técnico ao processar pagamento",
                ex);
        }
    }
}

// Uso
try
{
    var resultado = await _pagamentoService.ProcessarPagamento(pagamento);
    return Ok(resultado);
}
catch (CartaoRecusadoException ex)
{
    return BadRequest(new
    {
        Erro = "Cartão recusado",
        Motivo = ex.MotivoRecusa,
        PagamentoId = ex.PagamentoId
    });
}
catch (LimiteExcedidoException ex)
{
    return BadRequest(new
    {
        Erro = "Limite excedido",
        ValorTentado = ex.ValorTentado,
        LimiteDisponivel = ex.LimiteDisponivel,
        PagamentoId = ex.PagamentoId
    });
}
catch (PagamentoBaseException ex)
{
    _logger.LogError(ex, "Erro ao processar pagamento");
    return StatusCode(500, "Erro ao processar pagamento");
}
```

## ✅ Checklist de Design

Ao criar exceções customizadas, pergunte-se:

- [ ] O nome é claro e termina com "Exception"?
- [ ] Herda de Exception ou exceção apropriada?
- [ ] Propriedades são read-only (imutáveis)?
- [ ] Fornece múltiplos construtores (com/sem InnerException)?
- [ ] Mensagens são claras e acionáveis?
- [ ] Contexto adicional é relevante e útil?
- [ ] Documentação XML está completa?
- [ ] Seguiu convenções .NET?

## 📚 Recursos Adicionais

- [Designing Custom Exceptions](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions)
- [Exception Class Design](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions)

---

**Próximo:** [Depuração no VS Code](../03-depuracao-vscode/01-conteudo.md)
