# Exercícios — Best Practices e Padrões de Resiliência

## Instruções Gerais

- Foco em aplicar padrões profissionais
- Compare "antes" e "depois" em cada refatoração
- Documente as melhorias alcançadas
- Meça performance onde relevante

**Tempo estimado:** 6-8 horas

---

## Exercício 1: Refatorar Anti-Patterns ⭐⭐

### Objetivo
Identificar e corrigir anti-patterns comuns em código legado.

### Código Legado (cheio de problemas!)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class PedidoService
{
    private List<Pedido> _pedidos = new();
    
    public Pedido BuscarPedido(int id)
    {
        try
        {
            // PROBLEMA 1: Exceção para controle de fluxo
            return _pedidos.Single(p => p.Id == id);
        }
        catch
        {
            // PROBLEMA 2: Exceção silenciosa
            return null;
        }
    }
    
    public void ProcessarPedido(Pedido pedido)
    {
        try
        {
            ValidarPedido(pedido);
            CalcularTotal(pedido);
            SalvarPedido(pedido);
        }
        catch (Exception ex)
        {
            // PROBLEMA 3: Pokémon catch
            Console.WriteLine("Erro");
            // PROBLEMA 4: Não re-lança
        }
    }
    
    public decimal CalcularDesconto(decimal valor, decimal percentual)
    {
        // PROBLEMA 5: Sem validação
        return valor * (percentual / 100);
    }
    
    public void EnviarEmail(string email)
    {
        try
        {
            // Simula envio
            if (string.IsNullOrEmpty(email))
                throw new Exception("Email vazio");
        }
        catch (Exception ex)
        {
            // PROBLEMA 6: throw ex (perde stack trace)
            throw ex;
        }
    }
    
    private void ValidarPedido(Pedido pedido)
    {
        // PROBLEMA 7: Validação no lugar errado
        if (pedido == null)
            return;
    }
    
    private decimal CalcularTotal(Pedido pedido)
    {
        try
        {
            return pedido.Itens.Sum(i => i.Preco * i.Quantidade);
        }
        catch
        {
            // PROBLEMA 8: Retorno mágico
            return -1;
        }
    }
    
    private void SalvarPedido(Pedido pedido)
    {
        _pedidos.Add(pedido);
    }
}

record Pedido(int Id, string Cliente, List<ItemPedido> Itens);
record ItemPedido(string Produto, int Quantidade, decimal Preco);
```

### Tarefas

1. **Identifique TODOS os problemas** (pelo menos 8)
2. **Refatore o código** aplicando best practices:
   - Use exceções específicas
   - Adicione logging estruturado
   - Implemente validações fail-fast
   - Preserve stack traces
   - Use métodos corretos do LINQ

3. **Crie testes** que demonstram:
   - Código legado com problemas
   - Código refatorado funcionando
   - Exceções corretas sendo lançadas

4. **Documente as mudanças:**
   | Problema | Antes | Depois | Benefício |
   |----------|-------|--------|-----------|
   | 1 | ... | ... | ... |

### Entrega

- Código refatorado completo
- Comparação lado a lado
- Testes demonstrando melhorias
- Documento explicando cada mudança

### Critérios de Avaliação

- ✅ 8+ problemas identificados
- ✅ Código refatorado segue best practices
- ✅ Testes cobrem cenários principais
- ✅ Documentação clara das mudanças

---

## Exercício 2: Implementar Padrões de Resiliência com Polly ⭐⭐⭐

### Objetivo
Usar Polly para adicionar retry, circuit breaker e timeout.

### Requisitos

```bash
dotnet new console -n ResilienciaPolly
cd ResilienciaPolly
dotnet add package Polly
dotnet add package Microsoft.Extensions.Http.Polly
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
```

### Tarefas

1. **Simule API externa instável:**
   ```csharp
   public class ApiExternaSimulada
   {
       private int _chamadas = 0;
       private readonly Random _random = new();
       
       public async Task<string> BuscarDadosAsync()
       {
           _chamadas++;
           await Task.Delay(100);
           
           // 40% de chance de falha
           if (_random.Next(100) < 40)
               throw new HttpRequestException("API temporariamente indisponível");
           
           return $"Dados da chamada #{_chamadas}";
       }
   }
   ```

2. **Implemente Policy de Retry:**
   - 3 tentativas
   - Backoff exponencial (1s, 2s, 4s)
   - Log de cada retry
   - Apenas para `HttpRequestException`

3. **Implemente Circuit Breaker:**
   - Abre após 5 falhas consecutivas
   - Fecha após 30 segundos
   - Estados: Closed, Open, HalfOpen
   - Logs de mudanças de estado

4. **Implemente Timeout:**
   - 5 segundos máximo
   - Log quando timeout ocorre

5. **Combine políticas (PolicyWrap):**
   ```
   Timeout(5s) → Retry(3x) → CircuitBreaker(5 fails)
   ```

6. **Execute 50 chamadas** e mostre:
   - Quantas tiveram sucesso na 1ª tentativa
   - Quantas precisaram de retry
   - Quantas falharam mesmo após retry
   - Quando circuit breaker abriu/fechou

### Entrega

- Implementação completa das 3 políticas
- PolicyWrap combinando todas
- Estatísticas das 50 chamadas
- Logs mostrando resiliência funcionando

### Critérios de Avaliação

- ✅ Retry funciona corretamente
- ✅ Circuit breaker abre/fecha apropriadamente
- ✅ Timeout aborta chamadas longas
- ✅ PolicyWrap combina corretamente
- ✅ Logs estruturados e informativos

---

## Exercício 3: Global Exception Handler ⭐⭐⭐

### Objetivo
Implementar exception handling centralizado em API ASP.NET Core.

### Requisitos

```bash
dotnet new webapi -n GlobalHandlerApi
cd GlobalHandlerApi
dotnet add package Serilog.AspNetCore
```

### Tarefas

1. **Crie exceções customizadas:**
   ```csharp
   public class ValidationException : Exception { }
   public class NotFoundException : Exception { }
   public class BusinessException : Exception { }
   public class UnauthorizedException : Exception { }
   ```

2. **Implemente middleware de exception handling:**
   - Mapeia exceções para status codes HTTP
   - Retorna JSON padronizado
   - Log apropriado para cada tipo
   - NÃO expõe stack traces em produção

3. **Formato de resposta:**
   ```json
   {
     "type": "ValidationError",
     "title": "Um ou mais erros de validação ocorreram",
     "status": 400,
     "errors": {
       "Email": ["Email é obrigatório"],
       "Idade": ["Idade deve ser maior que 18"]
     },
     "traceId": "00-abc123-def456-01"
   }
   ```

4. **Crie endpoints que lançam cada exceção:**
   - `GET /api/produtos/999` → NotFoundException
   - `POST /api/produtos` (dados inválidos) → ValidationException
   - `POST /api/auth/login` (credenciais erradas) → UnauthorizedException
   - `GET /api/critical` → Exception genérica

5. **Implemente ProblemDetails:**
   - Use RFC 7807 (Problem Details for HTTP APIs)
   - Inclua extensões customizadas quando necessário

6. **Diferencie Development vs Production:**
   - Development: inclui stack trace
   - Production: mensagem genérica

### Entrega

- GlobalExceptionMiddleware completo
- 4+ endpoints testáveis
- Responses em formato padronizado
- Testes com Postman/Insomnia mostrando cada cenário

### Critérios de Avaliação

- ✅ Middleware captura todas as exceções
- ✅ Mapeamento correto para status codes
- ✅ JSON padronizado (ProblemDetails)
- ✅ Logs estruturados
- ✅ Diferenciação Dev/Prod

---

## Exercício 4: Fail Fast vs Defensive Programming ⭐⭐⭐

### Objetivo
Comparar abordagens e aplicar cada uma adequadamente.

### Requisitos

Implemente 2 versões de um sistema de cadastro de usuários:

### Tarefas

1. **Versão Fail Fast:**
   ```csharp
   public class UsuarioServiceFailFast
   {
       public void CadastrarUsuario(Usuario usuario)
       {
           // Fail fast: valida TUDO no início
           if (usuario == null)
               throw new ArgumentNullException(nameof(usuario));
           
           if (string.IsNullOrWhiteSpace(usuario.Nome))
               throw new ArgumentException("Nome é obrigatório", nameof(usuario));
           
           if (string.IsNullOrWhiteSpace(usuario.Email))
               throw new ArgumentException("Email é obrigatório", nameof(usuario));
           
           if (!IsEmailValido(usuario.Email))
               throw new ArgumentException("Email inválido", nameof(usuario));
           
           if (usuario.Idade < 18)
               throw new ArgumentException("Idade mínima é 18 anos", nameof(usuario));
           
           // Processamento com confiança total
           SalvarNoBanco(usuario);
       }
   }
   ```

2. **Versão Defensive:**
   ```csharp
   public class UsuarioServiceDefensive
   {
       public Result CadastrarUsuario(Usuario usuario)
       {
           var erros = new List<string>();
           
           // Defensive: coleta TODOS os erros
           if (usuario == null)
               return Result.Failure("Usuário não pode ser nulo");
           
           if (string.IsNullOrWhiteSpace(usuario.Nome))
               erros.Add("Nome é obrigatório");
           
           if (string.IsNullOrWhiteSpace(usuario.Email))
               erros.Add("Email é obrigatório");
           else if (!IsEmailValido(usuario.Email))
               erros.Add("Email inválido");
           
           if (usuario.Idade < 18)
               erros.Add("Idade mínima é 18 anos");
           
           if (erros.Any())
               return Result.Failure(erros);
           
           // Processa
           SalvarNoBanco(usuario);
           return Result.Success();
       }
   }
   
   public record Result(bool Sucesso, List<string> Erros = null);
   ```

3. **Compare comportamento:**
   - Fail Fast: para no primeiro erro
   - Defensive: retorna todos os erros

4. **Crie API endpoints para cada versão:**
   - `POST /api/usuarios/failfast`
   - `POST /api/usuarios/defensive`

5. **Teste com dados inválidos:**
   ```json
   {
     "nome": "",
     "email": "invalido",
     "idade": 15
   }
   ```
   
   Fail Fast: retorna apenas "Nome é obrigatório"
   Defensive: retorna os 3 erros

6. **Analise:**
   - Quando usar Fail Fast?
   - Quando usar Defensive?
   - Qual oferece melhor UX?

### Entrega

- 2 implementações completas
- API testável
- Comparação detalhada:
  | Aspecto | Fail Fast | Defensive |
  |---------|-----------|-----------|
  | Feedback | ... | ... |
  | Performance | ... | ... |
  | UX | ... | ... |
  | Use cases | ... | ... |

### Critérios de Avaliação

- ✅ 2 versões implementadas corretamente
- ✅ Comportamentos distintos demonstrados
- ✅ Análise comparativa completa
- ✅ Recomendações de quando usar cada uma

---

## Exercício 5: Performance - Exceções vs Retorno ⭐⭐⭐⭐

### Objetivo
Medir impacto de performance ao usar exceções para controle de fluxo.

### Requisitos

Implemente e compare 3 abordagens:

### Tarefas

1. **Versão 1 - Exceções:**
   ```csharp
   public int BuscarIndicePorId_Excecoes(int[] array, int id)
   {
       try
       {
           return Array.IndexOf(array, id);
       }
       catch (Exception)
       {
           throw new NotFoundException($"ID {id} não encontrado");
       }
   }
   ```

2. **Versão 2 - Nullable:**
   ```csharp
   public int? BuscarIndicePorId_Nullable(int[] array, int id)
   {
       int index = Array.IndexOf(array, id);
       return index == -1 ? null : index;
   }
   ```

3. **Versão 3 - TryPattern:**
   ```csharp
   public bool TryBuscarIndicePorId(int[] array, int id, out int index)
   {
       index = Array.IndexOf(array, id);
       return index != -1;
   }
   ```

4. **Execute benchmark:**
   - 10.000 buscas bem-sucedidas
   - 10.000 buscas falhadas
   - Meça tempo total e memória

5. **Use BenchmarkDotNet:**
   ```bash
   dotnet add package BenchmarkDotNet
   ```
   
   ```csharp
   [MemoryDiagnoser]
   public class ExceptionBenchmarks
   {
       [Benchmark]
       public void ComExcecoes() { ... }
       
       [Benchmark]
       public void ComNullable() { ... }
       
       [Benchmark]
       public void ComTryPattern() { ... }
   }
   ```

6. **Compare:**
   - Tempo de execução
   - Alocação de memória
   - CPU usage

### Entrega

- 3 implementações
- Resultados do BenchmarkDotNet
- Gráfico comparativo
- Conclusão: "Use exceções para ____, use retornos para ____"

### Critérios de Avaliação

- ✅ 3 abordagens implementadas
- ✅ Benchmarks corretos (BenchmarkDotNet)
- ✅ Resultados documentados
- ✅ Análise de quando usar cada abordagem

---

## Exercício 6: Async Exception Handling ⭐⭐⭐⭐

### Objetivo
Dominar tratamento de exceções em código assíncrono.

### Requisitos

```csharp
public class AsyncService
{
    public async Task<string> OperacaoAsync(int id)
    {
        await Task.Delay(100);
        
        if (id < 0)
            throw new ArgumentException("ID inválido");
        
        if (id == 0)
            throw new InvalidOperationException("ID zero não permitido");
        
        return $"Resultado para ID {id}";
    }
}
```

### Tarefas

1. **Task.WhenAll com múltiplas exceções:**
   ```csharp
   var tasks = new[]
   {
       OperacaoAsync(-1),  // ArgumentException
       OperacaoAsync(0),   // InvalidOperationException
       OperacaoAsync(1),   // Sucesso
   };
   
   // Capture TODAS as exceções, não apenas a primeira
   ```

2. **Fire-and-forget perigoso:**
   ```csharp
   // Identifique o problema:
   public void IniciarProcessamento()
   {
       Task.Run(async () =>
       {
           await ProcessarDadosAsync();  // E se lançar exceção?
       });
   }
   ```

3. **ConfigureAwait e contexto:**
   ```csharp
   public async Task ProcessarAsync()
   {
       try
       {
           await BuscarDadosAsync().ConfigureAwait(false);
       }
       catch (Exception ex)
       {
           // Captura funciona igual com ConfigureAwait(false)
       }
   }
   ```

4. **ValueTask exceções:**
   ```csharp
   public ValueTask<int> ObterValorAsync()
   {
       // Como lançar exceção sincrônica em ValueTask?
   }
   ```

5. **UnobservedTaskException:**
   ```csharp
   TaskScheduler.UnobservedTaskException += ...
   // Capture exceções de tasks esquecidas
   ```

### Entrega

- 5 cenários implementados
- Demonstração de cada problema
- Solução correta para cada um
- Explicação: "Por que exceções em async são diferentes?"

### Critérios de Avaliação

- ✅ Task.WhenAll com múltiplas exceções
- ✅ Fire-and-forget tratado corretamente
- ✅ ConfigureAwait não afeta exceptions
- ✅ ValueTask exceptions implementadas
- ✅ UnobservedTaskException handler

---

## 🎓 Resumo de Habilidades

Ao completar estes exercícios, você dominará:

- ✅ Identificar e refatorar anti-patterns
- ✅ Implementar Polly (retry, circuit breaker, timeout)
- ✅ Criar global exception handlers
- ✅ Aplicar fail-fast vs defensive programming
- ✅ Otimizar performance (exceções vs retornos)
- ✅ Tratar exceções em código assíncrono

**Tempo total estimado:** 8-10 horas
