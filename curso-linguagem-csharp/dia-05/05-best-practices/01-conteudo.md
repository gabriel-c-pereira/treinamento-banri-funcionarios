# 05 - Best Practices e Padrões de Resiliência

## 🎯 Objetivos

Ao final deste módulo, você será capaz de:
- Identificar e evitar anti-patterns comuns de exception handling
- Implementar padrões de resiliência (Retry, Circuit Breaker, Timeout)
- Usar Polly para políticas de retry e circuit breaker
- Criar global exception handlers
- Aplicar fail-fast vs defensive programming
- Tratar erros em código assíncrono corretamente

---

## 📑 Índice

1. [Anti-Patterns Comuns](#1-anti-patterns-comuns)
2. [Padrões de Resiliência](#2-padrões-de-resiliência)
3. [Polly - Resilience Framework](#3-polly---resilience-framework)
4. [Global Exception Handlers](#4-global-exception-handlers)
5. [Fail Fast vs Defensive Programming](#5-fail-fast-vs-defensive-programming)
6. [Exceções em Async/Await](#6-exceções-em-asyncawait)
7. [Checklist de Code Review](#7-checklist-de-code-review)

---

## 1. Anti-Patterns Comuns

### 1.1 Pokémon Exception Handling

```csharp
// ❌ ANTI-PATTERN: Catch 'em all!
try
{
    ProcessarPedido(pedido);
}
catch (Exception ex)
{
    // Esconde TODOS os erros
    Console.WriteLine("Erro");
}

// ✅ CORRETO: Catch específico
try
{
    ProcessarPedido(pedido);
}
catch (ValidationException ex)
{
    _logger.LogWarning(ex, "Validação falhou para pedido {PedidoId}", pedido.Id);
    throw;  // Re-lança para camadas superiores
}
catch (PaymentException ex)
{
    _logger.LogError(ex, "Falha no pagamento");
    // Tratamento específico
}
// Deixa outras exceções propagarem
```

### 1.2 Exception para Controle de Fluxo

```csharp
// ❌ ANTI-PATTERN: Usar exceções como if/else
public Usuario? BuscarUsuario(int id)
{
    try
    {
        return _usuarios.Single(u => u.Id == id);
    }
    catch (InvalidOperationException)
    {
        return null;  // Exceção esperada!
    }
}

// ✅ CORRETO: Usar métodos apropriados
public Usuario? BuscarUsuario(int id)
{
    return _usuarios.FirstOrDefault(u => u.Id == id);
}
```

**Por que evitar:**
- Exceções são CARAS (performance)
- Dificulta debugging (poluição do call stack)
- Código menos legível

### 1.3 Exceções Silenciosas

```csharp
// ❌ ANTI-PATTERN: Engolir exceção
try
{
    SalvarNoBanco(dados);
}
catch
{
    // Não faz nada!
}

// ✅ CORRETO: Log mínimo
try
{
    SalvarNoBanco(dados);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Falha ao salvar dados");
    throw;  // Ou trate apropriadamente
}
```

### 1.4 throw ex (perde stack trace)

```csharp
// ❌ ANTI-PATTERN: Perde stack trace original
try
{
    ProcessarDados();
}
catch (Exception ex)
{
    _logger.LogError(ex.Message);
    throw ex;  // ❌ Recria exceção, perde stack trace
}

// ✅ CORRETO: Preserva stack trace
try
{
    ProcessarDados();
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro ao processar");
    throw;  // ✅ Re-lança original
}
```

### 1.5 Strings de Erro ao invés de Exception Types

```csharp
// ❌ ANTI-PATTERN: Verificar mensagem
try
{
    ProcessarPagamento();
}
catch (Exception ex)
{
    if (ex.Message.Contains("saldo insuficiente"))
    {
        // Frágil! E se a mensagem mudar?
    }
}

// ✅ CORRETO: Usar tipos específicos
try
{
    ProcessarPagamento();
}
catch (InsufficientFundsException ex)
{
    // Tratamento específico
}
```

### 1.6 Exception como Retorno

```csharp
// ❌ ANTI-PATTERN: Retornar exceção
public Exception ValidarUsuario(Usuario usuario)
{
    if (string.IsNullOrEmpty(usuario.Email))
        return new ValidationException("Email obrigatório");
    
    return null;  // Sucesso?
}

// ✅ CORRETO: Lançar exceção
public void ValidarUsuario(Usuario usuario)
{
    if (string.IsNullOrEmpty(usuario.Email))
        throw new ValidationException("Email obrigatório");
}

// ✅ ALTERNATIVA: Result pattern
public Result ValidarUsuario(Usuario usuario)
{
    if (string.IsNullOrEmpty(usuario.Email))
        return Result.Failure("Email obrigatório");
    
    return Result.Success();
}
```

---

## 2. Padrões de Resiliência

### 2.1 Retry Pattern

Tentar novamente após falha temporária:

```csharp
public async Task<T> ExecutarComRetryAsync<T>(
    Func<Task<T>> operacao,
    int maxTentativas = 3,
    int delayMs = 1000)
{
    int tentativa = 0;
    
    while (true)
    {
        try
        {
            tentativa++;
            return await operacao();
        }
        catch (Exception ex) when (tentativa < maxTentativas)
        {
            _logger.LogWarning(ex, 
                "Tentativa {Tentativa}/{Max} falhou. Aguardando {Delay}ms", 
                tentativa, maxTentativas, delayMs);
            
            await Task.Delay(delayMs);
        }
    }
}

// Uso
var resultado = await ExecutarComRetryAsync(
    async () => await _apiClient.BuscarDadosAsync(),
    maxTentativas: 3,
    delayMs: 2000
);
```

**Exponential Backoff:**

```csharp
public async Task<T> ExecutarComRetryExponencialAsync<T>(
    Func<Task<T>> operacao,
    int maxTentativas = 5)
{
    for (int i = 0; i < maxTentativas; i++)
    {
        try
        {
            return await operacao();
        }
        catch (Exception ex) when (i < maxTentativas - 1)
        {
            // Delay exponencial: 1s, 2s, 4s, 8s, 16s...
            int delayMs = (int)Math.Pow(2, i) * 1000;
            
            _logger.LogWarning(ex, 
                "Retry {Tentativa}/{Max} após {Delay}ms", 
                i + 1, maxTentativas, delayMs);
            
            await Task.Delay(delayMs);
        }
    }
    
    throw new InvalidOperationException($"Falha após {maxTentativas} tentativas");
}
```

### 2.2 Circuit Breaker Pattern

Evita chamadas repetidas a serviços que estão falhando:

```csharp
public class CircuitBreaker
{
    private readonly int _threshold;
    private readonly TimeSpan _timeout;
    private int _failureCount;
    private DateTime _lastFailureTime;
    private CircuitState _state = CircuitState.Closed;
    
    public enum CircuitState { Closed, Open, HalfOpen }
    
    public CircuitBreaker(int threshold = 5, TimeSpan? timeout = null)
    {
        _threshold = threshold;
        _timeout = timeout ?? TimeSpan.FromMinutes(1);
    }
    
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operacao)
    {
        if (_state == CircuitState.Open)
        {
            if (DateTime.UtcNow - _lastFailureTime > _timeout)
            {
                // Tentar fechar o circuito
                _state = CircuitState.HalfOpen;
            }
            else
            {
                throw new CircuitBreakerOpenException(
                    $"Circuito aberto. Aguarde {_timeout.TotalSeconds}s");
            }
        }
        
        try
        {
            var resultado = await operacao();
            
            // Sucesso: resetar contador
            if (_state == CircuitState.HalfOpen)
            {
                _state = CircuitState.Closed;
                _failureCount = 0;
            }
            
            return resultado;
        }
        catch (Exception ex)
        {
            _failureCount++;
            _lastFailureTime = DateTime.UtcNow;
            
            if (_failureCount >= _threshold)
            {
                _state = CircuitState.Open;
                _logger.LogWarning("Circuit breaker ABERTO após {Count} falhas", _failureCount);
            }
            
            throw;
        }
    }
}

// Uso
var circuitBreaker = new CircuitBreaker(threshold: 5, timeout: TimeSpan.FromMinutes(1));

try
{
    var dados = await circuitBreaker.ExecuteAsync(
        async () => await _apiClient.BuscarDadosAsync());
}
catch (CircuitBreakerOpenException ex)
{
    _logger.LogWarning("Serviço temporariamente indisponível");
    // Usar cache ou fallback
}
```

### 2.3 Timeout Pattern

```csharp
public async Task<T> ExecutarComTimeoutAsync<T>(
    Func<Task<T>> operacao,
    TimeSpan timeout)
{
    using var cts = new CancellationTokenSource(timeout);
    
    try
    {
        return await operacao().WaitAsync(cts.Token);
    }
    catch (OperationCanceledException)
    {
        throw new TimeoutException(
            $"Operação excedeu timeout de {timeout.TotalSeconds}s");
    }
}

// Uso
try
{
    var resultado = await ExecutarComTimeoutAsync(
        async () => await _apiClient.BuscarDadosAsync(),
        timeout: TimeSpan.FromSeconds(30)
    );
}
catch (TimeoutException ex)
{
    _logger.LogWarning(ex, "Timeout na API externa");
}
```

### 2.4 Fallback Pattern

```csharp
public async Task<Produto> ObterProdutoAsync(int id)
{
    try
    {
        // Tenta API principal
        return await _apiClient.BuscarProdutoAsync(id);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "API principal falhou. Usando cache");
        
        // Fallback: cache
        var produtoCache = await _cache.GetAsync<Produto>($"produto:{id}");
        
        if (produtoCache != null)
            return produtoCache;
        
        // Fallback: banco de dados
        return await _repository.BuscarProdutoAsync(id);
    }
}
```

---

## 3. Polly - Resilience Framework

### 3.1 Instalação

```bash
dotnet add package Polly
dotnet add package Microsoft.Extensions.Http.Polly
```

### 3.2 Retry Policy

```csharp
using Polly;
using Polly.Retry;

// Retry simples
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .RetryAsync(3, (exception, retryCount) =>
    {
        _logger.LogWarning(exception, 
            "Retry {RetryCount}/3", retryCount);
    });

await retryPolicy.ExecuteAsync(async () =>
{
    return await _httpClient.GetStringAsync(url);
});

// Retry com backoff exponencial
var retryPolicyExponencial = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(
        retryCount: 5,
        sleepDurationProvider: retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (exception, timeSpan, retryCount, context) =>
        {
            _logger.LogWarning(exception,
                "Retry {RetryCount}: aguardando {Delay}s",
                retryCount, timeSpan.TotalSeconds);
        });
```

### 3.3 Circuit Breaker Policy

```csharp
var circuitBreakerPolicy = Policy
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromMinutes(1),
        onBreak: (exception, duration) =>
        {
            _logger.LogWarning("Circuit breaker ABERTO por {Duration}s", 
                duration.TotalSeconds);
        },
        onReset: () =>
        {
            _logger.LogInformation("Circuit breaker FECHADO");
        },
        onHalfOpen: () =>
        {
            _logger.LogInformation("Circuit breaker MEIO ABERTO (testando)");
        });

try
{
    await circuitBreakerPolicy.ExecuteAsync(async () =>
    {
        return await _httpClient.GetStringAsync(url);
    });
}
catch (BrokenCircuitException ex)
{
    _logger.LogWarning("Serviço indisponível (circuit breaker aberto)");
}
```

### 3.4 Timeout Policy

```csharp
var timeoutPolicy = Policy
    .TimeoutAsync(TimeSpan.FromSeconds(30), (context, timeSpan, task) =>
    {
        _logger.LogWarning("Timeout após {Timeout}s", timeSpan.TotalSeconds);
        return Task.CompletedTask;
    });

await timeoutPolicy.ExecuteAsync(async () =>
{
    return await _httpClient.GetStringAsync(url);
});
```

### 3.5 Policy Wrap (Combinar Políticas)

```csharp
// Combina: Timeout → Retry → Circuit Breaker
var policyWrap = Policy.WrapAsync(
    timeoutPolicy,
    retryPolicy,
    circuitBreakerPolicy
);

await policyWrap.ExecuteAsync(async () =>
{
    return await _httpClient.GetStringAsync(url);
});
```

### 3.6 HttpClient com Polly

```csharp
services.AddHttpClient<IApiClient, ApiClient>()
    .AddTransientHttpErrorPolicy(builder =>
        builder.WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddTransientHttpErrorPolicy(builder =>
        builder.CircuitBreakerAsync(5, TimeSpan.FromMinutes(1)));
```

---

## 4. Global Exception Handlers

### 4.1 ASP.NET Core Middleware

```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    
    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validação falhou");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado");
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");
            await HandleUnhandledExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleValidationExceptionAsync(
        HttpContext context,
        ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            error = "Validation Error",
            message = ex.Message,
            details = ex.Errors
        };
        
        await context.Response.WriteAsJsonAsync(response);
    }
    
    private static async Task HandleNotFoundExceptionAsync(
        HttpContext context,
        NotFoundException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Not Found",
            message = ex.Message
        });
    }
    
    private static async Task HandleUnhandledExceptionAsync(
        HttpContext context,
        Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal Server Error",
            message = "Ocorreu um erro inesperado. Por favor, tente novamente."
            // NÃO exponha detalhes em produção!
        });
    }
}

// Registro
app.UseMiddleware<GlobalExceptionMiddleware>();
```

### 4.2 Console App - UnhandledException

```csharp
class Program
{
    static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += 
            (sender, args) =>
            {
                var ex = (Exception)args.ExceptionObject;
                Log.Fatal(ex, "Exceção não tratada fatal");
                Log.CloseAndFlush();
            };
        
        try
        {
            IniciarAplicacao();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Erro fatal na aplicação");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
```

### 4.3 Task Unobserved Exceptions

```csharp
TaskScheduler.UnobservedTaskException += (sender, args) =>
{
    Log.Error(args.Exception, "Task exception não observada");
    args.SetObserved();  // Previne crash do app
};
```

---

## 5. Fail Fast vs Defensive Programming

### 5.1 Fail Fast

```csharp
// ✅ Fail Fast: Falhe imediatamente se algo está errado
public class PedidoService
{
    private readonly IPedidoRepository _repository;
    
    public PedidoService(IPedidoRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    public void ProcessarPedido(Pedido pedido)
    {
        if (pedido == null)
            throw new ArgumentNullException(nameof(pedido));
        
        if (pedido.Itens.Count == 0)
            throw new InvalidOperationException("Pedido sem itens");
        
        // Processa com confiança
        _repository.Salvar(pedido);
    }
}
```

**Vantagens:**
- Problemas detectados imediatamente
- Erros não se propagam silenciosamente
- Debug mais fácil

### 5.2 Defensive Programming

```csharp
// ✅ Defensive: Proteja contra entradas inválidas
public decimal CalcularDesconto(decimal valor, decimal percentual)
{
    // Validações defensivas
    if (valor < 0)
        throw new ArgumentException("Valor não pode ser negativo", nameof(valor));
    
    if (percentual < 0 || percentual > 100)
        throw new ArgumentException("Percentual deve estar entre 0 e 100", nameof(percentual));
    
    return valor * (percentual / 100);
}

// ❌ Sem defesa: aceita qualquer valor
public decimal CalcularDesconto(decimal valor, decimal percentual)
{
    return valor * (percentual / 100);  // E se percentual = 150? ou -50?
}
```

### 5.3 Quando Usar Cada Um

| Cenário | Abordagem | Exemplo |
|---------|-----------|---------|
| Parâmetros públicos | Defensive | Validar sempre |
| Métodos internos | Fail Fast | Assert/Debug.Assert |
| APIs externas | Defensive | Try-catch, validação |
| Código crítico | Fail Fast | Throw cedo |
| Entrada de usuário | Defensive | Validação + mensagens amigáveis |

---

## 6. Exceções em Async/Await

### 6.1 Exceções em Tasks

```csharp
// ❌ Exceção perdida!
Task.Run(() =>
{
    throw new Exception("Nunca será vista");
});
// Task é "fire and forget" - exceção não é observada

// ✅ Aguarde o Task
await Task.Run(() =>
{
    throw new Exception("Será capturada");
});
```

### 6.2 Task.WhenAll com Exceções

```csharp
var tasks = new[]
{
    Task.Run(() => throw new InvalidOperationException("Task 1")),
    Task.Run(() => throw new ArgumentException("Task 2")),
    Task.Run(() => 42)
};

try
{
    await Task.WhenAll(tasks);
}
catch (Exception ex)
{
    // Apenas PRIMEIRA exceção é capturada!
    Console.WriteLine(ex.Message);  // "Task 1"
}

// ✅ Capturar TODAS as exceções
try
{
    await Task.WhenAll(tasks);
}
catch
{
    foreach (var task in tasks)
    {
        if (task.IsFaulted && task.Exception != null)
        {
            foreach (var ex in task.Exception.InnerExceptions)
            {
                _logger.LogError(ex, "Task falhou");
            }
        }
    }
}

// ✅ OU: Usar AggregateException
try
{
    Task.WaitAll(tasks);  // Versão síncrona
}
catch (AggregateException ex)
{
    foreach (var inner in ex.InnerExceptions)
    {
        _logger.LogError(inner, "Task falhou");
    }
}
```

### 6.3 ConfigureAwait e Exceções

```csharp
// Em biblioteca: use ConfigureAwait(false)
public async Task<Dados> BuscarDadosAsync()
{
    try
    {
        return await _httpClient.GetFromJsonAsync<Dados>(url)
            .ConfigureAwait(false);
    }
    catch (HttpRequestException ex)
    {
        // Exceção capturada normalmente
        _logger.LogError(ex, "Falha HTTP");
        throw;
    }
}
```

### 6.4 ValueTask e Exceções

```csharp
public ValueTask<int> ObterValorAsync()
{
    if (_cache.TryGet(out var valor))
    {
        // Exceção sincrônica em ValueTask
        if (valor < 0)
            throw new InvalidOperationException("Valor inválido");
        
        return new ValueTask<int>(valor);
    }
    
    return new ValueTask<int>(BuscarDoServidorAsync());
}

// Uso
try
{
    var resultado = await ObterValorAsync();
}
catch (InvalidOperationException ex)
{
    // Captura exceção normalmente
}
```

---

## 7. Checklist de Code Review

### ✅ Exception Handling

- [ ] Exceções são capturadas em níveis apropriados?
- [ ] Tipos específicos de exceção são usados (não apenas `Exception`)?
- [ ] Exceções são logadas com contexto suficiente?
- [ ] Stack trace é preservado (`throw;` ao invés de `throw ex;`)?
- [ ] Recursos são liberados (using, try-finally)?
- [ ] Exceptions não são usadas para controle de fluxo?
- [ ] Exceções customizadas seguem convenções (sufixo `Exception`)?
- [ ] InnerException é preservada em wrapping?

### ✅ Logging

- [ ] Logs estruturados são usados (message templates)?
- [ ] Níveis de log estão corretos (Info, Warning, Error)?
- [ ] Dados sensíveis são mascarados (senhas, cartões)?
- [ ] Correlation IDs são propagados?
- [ ] Performance não é impactada por logs excessivos?
- [ ] Logs contêm contexto suficiente para debug?

### ✅ Async/Await

- [ ] Tasks são sempre aguardados?
- [ ] ConfigureAwait(false) usado em bibliotecas?
- [ ] Exceções em Task.WhenAll são tratadas corretamente?
- [ ] CancellationToken é passado e respeitado?
- [ ] Não há deadlocks (`.Result` ou `.Wait()`)?

### ✅ Resiliência

- [ ] Retry está implementado para operações transientes?
- [ ] Circuit breaker protege serviços externos?
- [ ] Timeouts estão configurados?
- [ ] Fallbacks existem para cenários críticos?
- [ ] Polly é usado onde apropriado?

### ✅ Validação

- [ ] Parâmetros públicos são validados?
- [ ] Null checks são feitos onde necessário?
- [ ] Guard clauses no início dos métodos?
- [ ] Mensagens de erro são claras e acionáveis?

---

## 🎓 Resumo

Você aprendeu:

1. **Anti-Patterns:** Pokémon catch, exceção para fluxo, silenciosas
2. **Padrões:** Retry, Circuit Breaker, Timeout, Fallback
3. **Polly:** Framework completo de resiliência
4. **Global Handlers:** Middleware, UnhandledException
5. **Filosofias:** Fail Fast vs Defensive Programming
6. **Async:** Exceções em Tasks, WhenAll, ValueTask
7. **Code Review:** Checklist completo

**Próximo:** Aplicar tudo isso em projetos reais do Dia 06!
