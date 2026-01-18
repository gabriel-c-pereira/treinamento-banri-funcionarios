# Exercícios — Logging e Rastreamento

## Instruções Gerais

- Use Serilog para todos os exercícios
- Logs devem ser estruturados (não apenas texto)
- Teste cenários de sucesso E falha
- Documente configurações e resultados
- Compare logs em diferentes níveis

**Tempo estimado:** 6-8 horas

---

## Exercício 1: Configuração Básica de Logging ⭐

### Objetivo
Configurar Serilog com múltiplos sinks e níveis de log.

### Requisitos

Crie um projeto console `LoggingBasico`:

```bash
dotnet new console -n LoggingBasico
cd LoggingBasico
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
```

### Tarefas

1. **Configure Serilog com 3 sinks:**
   - Console (todos os níveis)
   - Arquivo debug (Debug+)
   - Arquivo errors (Error+)

2. **Implemente sistema de pedidos:**
   ```csharp
   record Pedido(int Id, string Cliente, decimal Total);
   ```

3. **Crie logs em diferentes níveis:**
   - Trace: Entrada/saída de métodos
   - Debug: Valores de variáveis
   - Information: Pedidos criados/processados
   - Warning: Total baixo (<10), estoque baixo
   - Error: Falhas em validação
   - Critical: Sistema sem conexão com banco

4. **Teste todos os níveis:**
   - Crie 10 pedidos com valores variados
   - Simule 2 falhas de validação
   - Simule 1 falha crítica

### Entrega

- Código completo com configuração
- 3 arquivos de log gerados
- Tabela mostrando onde cada nível aparece

### Critérios de Avaliação

- ✅ 3 sinks configurados corretamente
- ✅ 6 níveis de log usados
- ✅ Logs estruturados ({PedidoId}, {Cliente}, etc.)
- ✅ Filtros funcionando (arquivos com conteúdo correto)

---

## Exercício 2: Logging Estruturado e Enrichers ⭐⭐

### Objetivo
Usar enrichers para adicionar contexto automático aos logs.

### Requisitos

```bash
dotnet add package Serilog.Enrichers.Thread
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Formatting.Compact
```

### Tarefas

1. **Configure enrichers:**
   - MachineName
   - ThreadId
   - EnvironmentUserName
   - Propriedade customizada: ApplicationName, Version

2. **Implemente sistema de e-commerce:**
   ```csharp
   record Produto(int Id, string Nome, decimal Preco, int Estoque);
   record ItemCarrinho(Produto Produto, int Quantidade);
   record Carrinho(int CarrinhoId, string UsuarioId, List<ItemCarrinho> Itens);
   ```

3. **Use @ para serialização completa:**
   ```csharp
   Log.Information("Carrinho criado {@Carrinho}", carrinho);
   ```

4. **Crie logs estruturados:**
   - Login do usuário: `{UsuarioId}`, `{Timestamp}`
   - Produto adicionado: `{ProdutoId}`, `{Quantidade}`, `{PrecoUnitario}`
   - Carrinho finalizado: `{CarrinhoId}`, `{TotalItens}`, `{ValorTotal}`

5. **Exporte para JSON:**
   - Use `CompactJsonFormatter`
   - Analise estrutura do JSON gerado

### Entrega

- Configuração Serilog completa
- Código do sistema e-commerce
- Arquivo JSON com logs
- Análise: "Quais enrichers foram úteis e por quê?"

### Critérios de Avaliação

- ✅ 5+ enrichers configurados
- ✅ Logs em formato JSON
- ✅ Objetos serializados com @
- ✅ Contexto completo em cada log

---

## Exercício 3: Correlation ID e Contexto Distribuído ⭐⭐⭐

### Objetivo
Rastrear operações através de múltiplas camadas com Correlation ID.

### Requisitos

Implemente arquitetura em 3 camadas:
- **API Layer:** Recebe requisições
- **Service Layer:** Lógica de negócio
- **Repository Layer:** Acesso a dados

### Tarefas

1. **Implemente OperationContext:**
   ```csharp
   public class OperationContext : IDisposable
   {
       public string CorrelationId { get; }
       public string OperationId { get; }
       // ...
   }
   ```

2. **Use LogContext.PushProperty:**
   ```csharp
   using (LogContext.PushProperty("CorrelationId", correlationId))
   {
       // Todos os logs terão CorrelationId
   }
   ```

3. **Crie operação completa:**
   ```
   API Layer: Recebe requisição → gera CorrelationId
   ↓
   Service Layer: Processa pedido (mesmo CorrelationId)
   ↓
   Repository Layer: Salva no banco (mesmo CorrelationId)
   ```

4. **Simule 3 requisições simultâneas:**
   - Use `Task.Run` para executar em paralelo
   - Cada uma deve ter CorrelationId único
   - Logs devem permitir rastrear cada requisição

5. **Implemente propagação:**
   - CorrelationId deve ser passado entre camadas
   - Todos os logs de uma operação têm mesmo CorrelationId

### Entrega

- Implementação das 3 camadas
- OperationContext funcional
- Logs mostrando rastreamento completo
- Demonstração: "Como identificar todos os logs de uma requisição específica?"

### Critérios de Avaliação

- ✅ OperationContext implementado
- ✅ CorrelationId em todos os logs
- ✅ Possível rastrear operações individuais
- ✅ Funciona com múltiplas threads

---

## Exercício 4: Exception Logging e Context ⭐⭐⭐

### Objetivo
Logar exceções com contexto completo e InnerException.

### Requisitos

Crie sistema de processamento de pagamentos com múltiplas exceções:

```csharp
public class PaymentException : Exception { }
public class InvalidCardException : PaymentException { }
public class InsufficientFundsException : PaymentException { }
public class PaymentGatewayException : PaymentException { }
```

### Tarefas

1. **Implemente ProcessadorPagamento:**
   - ValidarCartao() → pode lançar InvalidCardException
   - VerificarSaldo() → pode lançar InsufficientFundsException
   - ProcessarNoGateway() → pode lançar PaymentGatewayException

2. **Log estruturado de exceções:**
   ```csharp
   try
   {
       ProcessarPagamento(pagamento);
   }
   catch (PaymentException ex)
   {
       Log.Error(ex, "Falha no pagamento {@Pagamento}", pagamento);
       // Adicione contexto extra
       Log.ForContext("PedidoId", pagamento.PedidoId)
          .ForContext("ValorTentado", pagamento.Valor)
          .Error(ex, "Detalhes do erro de pagamento");
   }
   ```

3. **Capturar Exception Wrapping:**
   - Camada Repository lança `DbException`
   - Camada Service wrappea em `ServiceException`
   - Camada API wrappea em `ApiException`
   - Logs devem mostrar toda a cadeia (InnerException)

4. **Adicione propriedades customizadas:**
   ```csharp
   ex.Data["PedidoId"] = pedidoId;
   ex.Data["Timestamp"] = DateTime.UtcNow;
   ex.Data["Usuario"] = usuarioId;
   ```

5. **Teste cenários:**
   - Cartão inválido
   - Saldo insuficiente
   - Gateway offline
   - Exceção inesperada

### Entrega

- Sistema de pagamentos completo
- Logs de todas as exceções
- Análise: "Como logs ajudam a debugar exceções em produção?"
- Exemplo de InnerException chain nos logs

### Critérios de Avaliação

- ✅ Exceções logadas com contexto completo
- ✅ InnerException preservada e visível
- ✅ ex.Data capturada nos logs
- ✅ Diferentes tipos tratados diferentemente

---

## Exercício 5: Performance e Logging Condicional ⭐⭐⭐

### Objetivo
Otimizar logging para não impactar performance.

### Requisitos

Crie aplicação que processa 10.000 registros:

```csharp
record Registro(int Id, string Dados, DateTime Timestamp);
```

### Tarefas

1. **Implemente 3 versões:**

   **Versão 1 - Ineficiente:**
   ```csharp
   foreach (var registro in registros)
   {
       // Log de TUDO
       Log.Debug("Processando {@Registro}", registro);
       Processar(registro);
   }
   ```

   **Versão 2 - Com verificação:**
   ```csharp
   foreach (var registro in registros)
   {
       if (Log.IsEnabled(LogEventLevel.Debug))
       {
           Log.Debug("Processando {@Registro}", registro);
       }
       Processar(registro);
   }
   ```

   **Versão 3 - Sampling:**
   ```csharp
   foreach (var registro in registros)
   {
       // Log apenas 1% dos registros
       if (Random.Shared.Next(100) < 1)
       {
           Log.Debug("Processando {@Registro}", registro);
       }
       Processar(registro);
   }
   ```

2. **Meça performance:**
   ```csharp
   var sw = Stopwatch.StartNew();
   ProcessarLote(registros);
   sw.Stop();
   Log.Information("Lote processado em {Tempo}ms", sw.ElapsedMilliseconds);
   ```

3. **Compare tempos:**
   - Versão 1 com Log.Debug habilitado
   - Versão 1 com Log.Debug desabilitado
   - Versão 2 com verificação
   - Versão 3 com sampling

4. **Implemente batching:**
   ```csharp
   // Log resumo a cada 1000 registros
   if (contador % 1000 == 0)
   {
       Log.Information("Processados {Contador}/{Total}", contador, total);
   }
   ```

### Entrega

- Código das 3 versões
- Tabela de performance:
  | Versão | Debug OFF | Debug ON | Impacto |
  |--------|-----------|----------|---------|
  | 1      | Xms       | Yms      | Z%      |
  | 2      | Xms       | Yms      | Z%      |
  | 3      | Xms       | Yms      | Z%      |
- Recomendação: qual abordagem usar quando?

### Critérios de Avaliação

- ✅ 3 versões implementadas
- ✅ Performance medida corretamente
- ✅ Análise de impacto documentada
- ✅ Recomendações práticas

---

## Exercício 6: OpenTelemetry e Distributed Tracing ⭐⭐⭐⭐

### Objetivo
Integrar OpenTelemetry para rastreamento distribuído.

### Requisitos

```bash
dotnet add package OpenTelemetry
dotnet add package OpenTelemetry.Exporter.Console
dotnet add package OpenTelemetry.Instrumentation.Http
```

### Tarefas

1. **Configure OpenTelemetry:**
   ```csharp
   var tracerProvider = Sdk.CreateTracerProviderBuilder()
       .AddSource("MeuServico")
       .AddConsoleExporter()
       .Build();
   ```

2. **Crie ActivitySource:**
   ```csharp
   private static readonly ActivitySource ActivitySource = new("MeuServico");
   ```

3. **Implemente spans:**
   ```csharp
   using var activity = ActivitySource.StartActivity("ProcessarPedido");
   activity?.SetTag("pedido.id", pedidoId);
   activity?.SetTag("cliente.id", clienteId);
   ```

4. **Crie hierarquia de spans:**
   ```
   ProcessarPedido (parent)
   ├── ValidarPedido (child)
   ├── CalcularTotal (child)
   │   ├── AplicarDesconto (grandchild)
   │   └── CalcularTaxas (grandchild)
   └── SalvarPedido (child)
   ```

5. **Adicione eventos e atributos:**
   ```csharp
   activity?.AddEvent(new ActivityEvent("Validação concluída"));
   activity?.SetTag("total", total);
   activity?.SetTag("itens.count", itens.Count);
   ```

6. **Simule chamadas HTTP:**
   - Use HttpClient para fazer requisições
   - Verifique que spans são criados automaticamente
   - Observe parent-child relationships

### Entrega

- Código completo com OpenTelemetry
- Saída do console mostrando traces
- Diagrama mostrando hierarquia de spans
- Explicação: "Como traces ajudam a identificar gargalos?"

### Critérios de Avaliação

- ✅ ActivitySource configurado
- ✅ Spans hierárquicos criados
- ✅ Tags e eventos adicionados
- ✅ Parent-child relationships corretos

---

## Exercício 7: Logging em Produção (Avançado) ⭐⭐⭐⭐⭐

### Objetivo
Configurar logging pronto para produção com todos os recursos.

### Requisitos

```bash
dotnet add package Serilog.Sinks.Seq
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Expressions
```

### Tarefas

1. **Configuração completa:**
   - Múltiplos sinks com níveis diferentes
   - Enrichers (machine, thread, environment)
   - Filtros (excluir health checks)
   - Sampling (10% de debug logs)
   - JSON formatado
   - Rotação de arquivos

2. **Implemente mascaramento:**
   ```csharp
   public class SensitiveDataMask : IDestructuringPolicy
   {
       public bool TryDestructure(object value, ...)
       {
           // Mascarar CPF, cartão, senha
       }
   }
   ```

3. **Crie health checks:**
   ```csharp
   Log.Information("HealthCheck executado");
   // Este log NÃO deve aparecer nos arquivos (filtrado)
   ```

4. **Implemente rate limiting:**
   ```csharp
   // Máximo 10 logs por segundo de um tipo específico
   ```

5. **Configure Seq:**
   - Instale Seq localmente (Docker)
   - Envie logs para Seq
   - Use query language para análises

6. **Adicione métricas:**
   ```csharp
   Log.Information("Pedido processado em {Tempo}ms", tempo);
   // No Seq: query média de tempo
   ```

### Entrega

- Configuração Serilog completa (50+ linhas)
- Implementação de todos os recursos
- Screenshots do Seq com logs
- Queries úteis no Seq:
  - Todos os erros da última hora
  - Tempo médio de processamento
  - Top 10 usuários mais ativos

### Critérios de Avaliação

- ✅ Configuração pronta para produção
- ✅ Dados sensíveis mascarados
- ✅ Filtros e sampling funcionando
- ✅ Seq integrado e usado para análises

---

## 🎓 Resumo de Habilidades

Ao completar estes exercícios, você dominará:

- ✅ Configuração completa de Serilog
- ✅ Logs estruturados com message templates
- ✅ Enrichers e contexto automático
- ✅ Correlation IDs para rastreamento
- ✅ Exception logging com InnerException
- ✅ Otimização de performance
- ✅ OpenTelemetry e distributed tracing
- ✅ Configuração pronta para produção

**Tempo total estimado:** 8-10 horas
