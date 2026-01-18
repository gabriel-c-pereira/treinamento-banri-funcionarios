# Dia 05 - Tratamento de Exceções e Depuração

## 📋 Visão Geral

Neste quinto dia do curso, você aprenderá a construir aplicações robustas e resilientes através do tratamento adequado de exceções e técnicas eficazes de depuração. Dominar esses conceitos é fundamental para criar software profissional que lida graciosamente com situações inesperadas.

**Duração:** 8 horas  
**Nível:** Intermediário  
**Pré-requisitos:** Conclusão dos dias 01-04 (fundamentos, OOP, coleções)

## 🎯 Objetivos de Aprendizagem

Ao final deste dia, você será capaz de:

- ✅ Compreender a hierarquia de exceções do .NET e quando usar cada tipo
- ✅ Implementar tratamento robusto de exceções com try-catch-finally
- ✅ Criar e lançar exceções customizadas para domínios específicos
- ✅ Utilizar técnicas avançadas de debugging no VS Code
- ✅ Implementar logging estruturado e rastreamento de erros
- ✅ Aplicar best practices de error handling em aplicações profissionais
- ✅ Usar filters de exceção e when clauses
- ✅ Compreender o impacto de performance do tratamento de exceções

## 📚 Conteúdo Programático

### 1. Fundamentos de Exceções (1h30)
**Arquivo:** `01-fundamentos-excecoes/`

- Hierarquia de exceções no .NET
- Try-catch-finally: sintaxe e semântica
- Tipos comuns: ArgumentException, InvalidOperationException, NullReferenceException
- Quando lançar vs quando capturar
- Exception filters e when clauses
- Exceções vs códigos de retorno

**Conceitos-chave:**
- Stack unwinding
- Exception propagation
- Finally block guarantees
- Multiple catch blocks ordering

### 2. Exceções Customizadas (1h30)
**Arquivo:** `02-excecoes-customizadas/`

- Criando exceções específicas do domínio
- Herança de Exception vs ApplicationException
- Propriedades essenciais: Message, InnerException, StackTrace
- Serialização de exceções
- Exception builder pattern
- Agregação de exceções (AggregateException)

**Conceitos-chave:**
- Domain-specific exceptions
- Exception wrapping
- Exception context preservation
- Custom exception data

### 3. Depuração no VS Code (2h)
**Arquivo:** `03-depuracao-vscode/`

- Configurando o debugger para C#
- Breakpoints: condicionais, logpoints, hit count
- Step over, step into, step out
- Watch expressions e variáveis
- Call stack e exception helpers
- Debug Console e Immediate Window
- Remote debugging e attach to process

**Conceitos-chave:**
- Breakpoint strategies
- Debug configuration (launch.json)
- Source mapping
- Performance profiling basics

### 4. Logging e Rastreamento (2h)
**Arquivo:** `04-logging-rastreamento/`

- Microsoft.Extensions.Logging
- Log levels: Trace, Debug, Information, Warning, Error, Critical
- Structured logging com Serilog
- Configuração de providers (Console, File, Application Insights)
- Correlation IDs e distributed tracing
- Performance monitoring
- Best practices de logging

**Conceitos-chave:**
- Structured logging
- Log correlation
- Telemetry
- Observability patterns

### 5. Best Practices e Padrões (1h)
**Arquivo:** `05-best-practices/`

- Exception handling anti-patterns
- Fail-fast vs defensive programming
- Exception shielding em APIs
- Retry policies e circuit breaker
- Global exception handlers
- Exception handling em async/await
- Performance considerations

**Conceitos-chave:**
- Resilience patterns
- Exception translation
- Error boundaries
- Graceful degradation

## 🛠️ Ferramentas e Recursos

### Extensões VS Code Recomendadas
- **C# Dev Kit** - Debugging avançado
- **C# Extensions** - Snippets de try-catch
- **Error Lens** - Visualização inline de erros
- **GitLens** - Rastreamento de bugs no histórico

### NuGet Packages
```xml
<PackageReference Include="Serilog" Version="3.1.1" />
<PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
<PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
<PackageReference Include="Polly" Version="8.2.0" />
<PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />
```

### Documentação Oficial
- [Exception Handling (C#)](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/)
- [Debugging in VS Code](https://code.visualstudio.com/docs/editor/debugging)
- [Logging in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging)
- [Best practices for exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)

## 💡 Dicas de Estudo

1. **Pratique com Cenários Reais**: Use os exercícios para simular situações comuns (validação de entrada, falhas de rede, recursos indisponíveis)

2. **Debug Ativamente**: Não apenas leia os exemplos, execute-os no debugger e observe o fluxo de execução

3. **Leia Stack Traces**: Aprenda a interpretar stack traces complexos — essa habilidade economiza horas de debugging

4. **Logging Estruturado**: Desde o início, pratique logging com contexto rico, não apenas mensagens simples

5. **Performance**: Use exceções para casos excepcionais, não para controle de fluxo regular

## 🎯 Critérios de Avaliação

Para considerar este dia completo, você deve:

- [ ] Implementar tratamento de exceções em pelo menos 3 cenários diferentes
- [ ] Criar 2 exceções customizadas com propriedades específicas do domínio
- [ ] Depurar uma aplicação usando breakpoints condicionais e watch expressions
- [ ] Configurar logging estruturado em uma aplicação console
- [ ] Resolver todos os exercícios propostos (mínimo 7 de 10 por tópico)
- [ ] Implementar um retry policy usando Polly
- [ ] Criar um global exception handler

## 📊 Distribuição do Tempo

| Tópico | Teoria | Prática | Exercícios |
|--------|--------|---------|------------|
| Fundamentos de Exceções | 30min | 40min | 20min |
| Exceções Customizadas | 30min | 40min | 20min |
| Depuração VS Code | 40min | 60min | 20min |
| Logging e Rastreamento | 40min | 60min | 20min |
| Best Practices | 20min | 20min | 20min |
| **Total** | **2h40** | **4h** | **1h20** |

## 🔗 Conexões com Outros Dias

**Pré-requisitos:**
- Dia 02: Classes e objetos (para criar exceções customizadas)
- Dia 03: Herança e polimorfismo (hierarquia de exceções)
- Dia 04: Coleções (AggregateException, logging de coleções)

**Prepara para:**
- Dia 06: Arquivos (exceções de I/O)
- Dia 07: Banco de dados (DbException, transaction rollback)
- Dia 08-09: Web/APIs (middleware de exceções, HTTP status codes)

## 📝 Projeto Integrador do Dia

**Sistema de Processamento de Pedidos com Resiliência**

Construa um sistema que:
1. Processa pedidos de um arquivo CSV
2. Valida dados com exceções customizadas (PedidoInvalidoException, ClienteNaoEncontradoException)
3. Implementa retry com Polly para operações que podem falhar
4. Loga todas as operações estruturadamente com Serilog
5. Gera relatório de erros ao final do processamento
6. Permite debugging interativo para investigar falhas

**Entregáveis:**
- Código fonte completo com tratamento robusto de exceções
- Arquivo de log estruturado
- README com instruções de debugging
- Testes unitários para cenários de erro

## 🚀 Desafios Extras

Para quem quer ir além:

1. **Exception Aggregator**: Implemente um coletor que agrupa múltiplas exceções e as reporta de forma consolidada

2. **Custom Debug Visualizer**: Crie um visualizador customizado para suas exceções de domínio no debugger

3. **Distributed Tracing**: Configure OpenTelemetry para rastrear exceções em um sistema distribuído simulado

4. **Performance Profiler**: Implemente um profiler que mede o impacto de performance do exception handling

5. **Smart Logger**: Crie um logger que automaticamente detecta padrões de erro e sugere soluções

## 📖 Leitura Complementar

- **Livro**: "The Art of Unit Testing" - Roy Osherove (capítulos sobre exception testing)
- **Artigo**: "Vexing exceptions" - Eric Lippert
- **Vídeo**: "Exception Handling Best Practices in .NET" - NDC Conference
- **Blog**: Engineering blog da Microsoft sobre resilience patterns

---

**Preparado por:** Instrutor C# | **Versão:** 2.0 | **Data:** 2025-10  
**Próximo:** [Dia 06 - Arquivos e Serialização](../dia-06/README.md)
