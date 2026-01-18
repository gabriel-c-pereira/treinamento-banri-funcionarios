# Exercícios — Exceções Customizadas

## 📝 Instruções Gerais

- Crie um projeto console para cada exercício
- Implemente as exceções seguindo todas as convenções .NET
- Teste todos os cenários (lançar e capturar)
- Documente as classes com XML comments
- Valide imutabilidade das propriedades

## Exercício 1: Exceção Básica de Domínio ⭐

**Objetivo:** Criar sua primeira exceção customizada simples.

**Requisitos:**

Crie uma exceção `ClienteNaoEncontradoException` que:
- Herda de `Exception`
- Contém propriedade `ClienteId` (int)
- Contém propriedade `Timestamp` (DateTime)
- Fornece 2 construtores:
  - `ClienteNaoEncontradoException(int clienteId)`
  - `ClienteNaoEncontradoException(int clienteId, Exception innerException)`
- Mensagem automática: "Cliente {clienteId} não encontrado"

**Teste:**

Crie uma classe `ClienteRepository` com método `ObterCliente(int id)` que:
- Simula lista de clientes em memória
- Lança a exceção se cliente não existir
- No Main, capture e exiba a exceção formatada

**Dica:** Use `base()` para chamar construtor da classe pai.

---

## Exercício 2: Hierarquia de Exceções ⭐⭐

**Objetivo:** Criar uma hierarquia de exceções relacionadas.

**Requisitos:**

Crie uma hierarquia para um sistema bancário:

1. **`OperacaoBancariaException`** (base abstrata)
   - Propriedades: `NumeroConta`, `DataHora`
   - Construtor protegido

2. **`SaldoInsuficienteException`** (derivada)
   - Propriedades adicionais: `ValorSolicitado`, `SaldoAtual`
   - Mensagem: "Saldo insuficiente. Solicitado: {valor}, Disponível: {saldo}"

3. **`ContaBloqueadaException`** (derivada)
   - Propriedades adicionais: `Motivo`, `DataBloqueio`
   - Mensagem: "Conta bloqueada desde {data}: {motivo}"

4. **`LimiteTransacaoExcedidoException`** (derivada)
   - Propriedades adicionais: `ValorLimite`, `ValorTentado`

**Teste:**

Crie classe `ContaBancaria` com métodos:
- `Sacar(decimal valor)`
- `Transferir(string contaDestino, decimal valor)`

No Main, teste cada tipo de exceção e mostre tratamento específico para cada.

**Critérios de Avaliação:**
- [ ] Hierarquia bem estruturada
- [ ] Propriedades imutáveis
- [ ] Mensagens claras e contextualizadas
- [ ] Múltiplos construtores

---

## Exercício 3: Exception Wrapping ⭐⭐

**Objetivo:** Praticar wrapping de exceções entre camadas.

**Requisitos:**

Implemente um sistema de 3 camadas:

**Camada de Dados:**
```csharp
public class ProdutoRepository
{
    public Produto Buscar(int id)
    {
        // Simula erro de banco de dados
        // Lança InvalidOperationException simulando erro SQL
    }
}
```

**Camada de Negócio:**
- Crie `RepositorioException` que wrappea exceções da camada de dados
- Propriedades: `NomeRepositorio`, `Operacao`, `Timestamp`
- Preserve InnerException original

**Camada de Aplicação:**
- Crie `ServicoException` que wrappea exceções de negócio
- Propriedades: `NomeServico`, `Contexto` (Dictionary<string, object>)
- Adicione contexto útil para debugging

**Teste:**

No Main:
1. Chame o serviço que falha
2. Capture a exceção mais externa
3. Percorra toda cadeia de InnerException
4. Exiba árvore completa de erros com indentação

**Exemplo de Saída:**
```
ServicoException: Erro ao processar produto
    Serviço: ProdutoService
    └─ RepositorioException: Falha ao buscar do banco
        Repositório: ProdutoRepository
        Operação: Buscar
        └─ InvalidOperationException: Connection timeout
```

---

## Exercício 4: Exception Builder Pattern ⭐⭐⭐

**Objetivo:** Implementar padrão builder para construir exceções complexas.

**Requisitos:**

Crie uma `ValidacaoException` com pattern builder:

```csharp
public class ValidacaoException : Exception
{
    public List<ErroValidacao> Erros { get; }
    
    // Classe builder interna
    public class Builder
    {
        // Métodos fluentes para adicionar erros
        public Builder AdicionarErro(string campo, string mensagem)
        public Builder AdicionarErroSe(bool condicao, string campo, string mensagem)
        public void LancarSeHouverErros()
    }
    
    public static Builder Criar() => new Builder();
}

public record ErroValidacao(string Campo, string Mensagem);
```

**Teste:**

Crie classe `Usuario` com propriedades: Nome, Email, Idade, CPF

Crie método `ValidarUsuario(Usuario usuario)` que usa o builder:
```csharp
ValidacaoException.Criar()
    .AdicionarErroSe(string.IsNullOrWhiteSpace(usuario.Nome), "Nome", "obrigatório")
    .AdicionarErroSe(usuario.Idade < 18, "Idade", "deve ser >= 18")
    // ... mais validações
    .LancarSeHouverErros();
```

No Main, capture e exiba todos os erros formatados.

**Desafio Extra:** Adicione método `ComoWarning()` que coleta erros mas não lança, retornando lista.

---

## Exercício 5: Exceções com Dados Contextuais ⭐⭐⭐

**Objetivo:** Usar Data dictionary para adicionar contexto dinâmico.

**Requisitos:**

Crie `ProcessamentoException` que:
- Herda de `Exception`
- Fornece métodos extension para adicionar contexto:

```csharp
public static class ExceptionExtensions
{
    public static T ComContexto<T>(this T exception, string chave, object valor)
        where T : Exception
    {
        exception.Data[chave] = valor;
        return exception;
    }
    
    public static T ComContextoDeUsuario<T>(this T exception, string userId)
        where T : Exception
    {
        // Adiciona userId, IP, timestamp, etc
    }
}
```

**Teste:**

Simule processamento de pedido que falha:
```csharp
try
{
    ProcessarPedido(pedido);
}
catch (Exception ex)
{
    throw ex
        .ComContexto("PedidoId", pedido.Id)
        .ComContexto("ClienteId", pedido.ClienteId)
        .ComContexto("Valor", pedido.ValorTotal)
        .ComContextoDeUsuario(currentUser.Id);
}
```

No catch final, extraia e exiba todos os dados contextuais.

---

## Exercício 6: Agregação de Exceções ⭐⭐⭐

**Objetivo:** Trabalhar com `AggregateException` para operações batch.

**Requisitos:**

Crie processador que valida múltiplos itens:

```csharp
public class ProcessadorLote
{
    public void ProcessarLote(List<Item> itens)
    {
        var excecoes = new List<Exception>();
        
        foreach (var item in itens)
        {
            try
            {
                Validar(item);
                Processar(item);
            }
            catch (Exception ex)
            {
                // Adiciona contexto e coleta
                ex.Data["ItemId"] = item.Id;
                excecoes.Add(ex);
            }
        }
        
        if (excecoes.Any())
            throw new AggregateException(
                "Falhas no processamento do lote",
                excecoes);
    }
}
```

**Teste:**

- Crie lote de 10 itens onde 3 falham
- Capture `AggregateException`
- Exiba relatório de erros:
  - Total de itens processados
  - Total de sucessos
  - Total de falhas
  - Detalhes de cada falha

**Desafio Extra:** Implemente retry apenas para falhas temporárias (timeout, rede).

---

## Exercício 7: Exceção com Serialização JSON ⭐⭐⭐

**Objetivo:** Tornar exceções serializáveis para transporte entre processos.

**Requisitos:**

Crie `ApiException` que pode ser serializada para JSON:

```csharp
[Serializable]
public class ApiException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }
    public Dictionary<string, object> Details { get; }
    
    [JsonConstructor]
    public ApiException(
        int statusCode,
        string errorCode,
        string mensagem,
        Dictionary<string, object> details)
        : base(mensagem)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Details = details ?? new();
    }
}
```

**Teste:**

1. Crie uma `ApiException` com dados complexos
2. Serialize para JSON usando `System.Text.Json`
3. Deserialize de volta
4. Valide que todos os dados foram preservados

**Desafio Extra:** Crie middleware ASP.NET que converte exceções para JSON responses.

---

## Exercício 8: Sistema de Pagamentos Completo ⭐⭐⭐⭐

**Objetivo:** Projeto integrado aplicando todos os conceitos.

**Requisitos:**

Implemente sistema de pagamentos com:

**Exceções de Domínio:**
1. `PagamentoException` (base abstrata)
   - Propriedades: `PagamentoId`, `Timestamp`

2. Exceções específicas (herdam de PagamentoException):
   - `CartaoRecusadoException` (motivo, bandeira)
   - `SaldoInsuficienteException` (valor solicitado, saldo)
   - `PagamentoDuplicadoException` (id original)
   - `GatewayIndisponivelException` (nome gateway, retry-after)
   - `FraudeDetectadaException` (score de risco, regras violadas)

**Serviços:**
- `PagamentoService` com método `ProcessarPagamento(PagamentoRequest)`
- `ValidadorPagamento` que acumula erros
- `DetectorFraude` que analisa transação

**Funcionalidades:**
- Validação completa (cartão, valor, limites)
- Detecção de fraude simulada
- Exception wrapping entre camadas
- Logging estruturado de erros
- Retry para falhas temporárias

**Teste:**

Crie casos de teste para cada tipo de exceção:
1. Pagamento bem-sucedido
2. Cartão recusado
3. Saldo insuficiente
4. Gateway indisponível (com retry)
5. Fraude detectada
6. Validações múltiplas falhando

Cada teste deve:
- Capturar exceção específica
- Logar informações relevantes
- Retornar response apropriado

**Exemplo de Saída:**
```
[OK] Pagamento #1234: Aprovado
[ERRO] Pagamento #1235: Cartão recusado - Limite excedido
[ERRO] Pagamento #1236: Fraude detectada - Score: 0.95
  Regras violadas:
    - IP suspeito (Rússia)
    - Valor acima do padrão (10x média)
    - Múltiplas tentativas em curto período
[RETRY] Pagamento #1237: Gateway timeout, tentando novamente...
```

---

## Exercício 9: Performance de Exceções ⭐⭐⭐⭐

**Objetivo:** Medir impacto de performance de exceções customizadas.

**Requisitos:**

Compare performance de diferentes abordagens:

1. **Exceção Simples:**
```csharp
public class SimpleException : Exception
{
    public SimpleException(string message) : base(message) { }
}
```

2. **Exceção Com Propriedades:**
```csharp
public class RichException : Exception
{
    public int Code { get; }
    public string Category { get; }
    public Dictionary<string, object> Data { get; }
    // ... muitas propriedades
}
```

3. **Sem Exceção (Result Pattern):**
```csharp
public record Result<T>
{
    public bool Success { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
}
```

**Benchmark:**
- Execute cada abordagem 100.000 vezes
- Meça tempo total e memória alocada
- Calcule overhead por operação

**Análise:**
- Compare performance das 3 abordagens
- Identifique quando usar cada uma
- Documente trade-offs (performance vs expressividade)

---

## Exercício 10: Exception Translation Layer ⭐⭐⭐⭐⭐

**Objetivo:** Criar camada que traduz exceções entre diferentes contextos.

**Requisitos:**

Implemente `ExceptionTranslator` que:

1. **Traduz exceções técnicas para domínio:**
```csharp
SqlException → RepositorioException
HttpRequestException → ServicoExternoException
JsonException → DadosInvalidosException
```

2. **Adiciona contexto automaticamente:**
```csharp
// Registra tradutores
translator.Registrar<SqlException>(ex => 
    new RepositorioException(
        GetRepositorioName(),
        GetOperacaoName(),
        ex));
```

3. **Suporta fallback chain:**
```csharp
try { }
catch (Exception ex)
{
    var translated = translator
        .TentarTraduzir(ex)
        .ComContextoDe(currentContext)
        .ComCorrelationId(correlationId)
        .Build();
    
    throw translated;
}
```

4. **Mantém telemetria:**
- Conta tipos de exceção
- Rastreia cadeias de tradução
- Gera métricas

**Teste:**

Crie sistema com múltiplas camadas (API → Service → Repository → DB) onde:
- Cada camada lança exceções técnicas diferentes
- Translator converte apropriadamente
- Camada API retorna erros user-friendly
- Telemetria registra toda a cadeia

**Desafio Extra:** Integre com Application Insights ou Serilog para logging estruturado.

---

## 🎯 Critérios de Avaliação

Para cada exercício, verifique se você:

- [ ] Seguiu convenções de nomenclatura (.NET)
- [ ] Propriedades são imutáveis (read-only)
- [ ] Forneceu múltiplos construtores apropriados
- [ ] Preservou InnerException quando relevante
- [ ] Mensagens são claras e acionáveis
- [ ] Adicionou documentação XML completa
- [ ] Testou todos os cenários (lançar/capturar)
- [ ] Seguiu hierarquia apropriada (herança)
- [ ] Implementou ToString() quando necessário
- [ ] Código está bem comentado

## 📚 Recursos Complementares

- [How to: Create User-Defined Exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions)
- [Exception Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions)
- [Best Practices for exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)

---

**Tempo estimado:** 8-10 horas para todos os exercícios  
**Nível:** Básico (ex 1-2), Intermediário (ex 3-5), Avançado (ex 6-8), Expert (ex 9-10)

