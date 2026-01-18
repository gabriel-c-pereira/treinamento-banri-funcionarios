# Exercícios - Fundamentos de Exceções

## 📝 Instruções Gerais

- Crie um projeto console para cada exercício
- Use try-catch-finally apropriadamente
- Teste todos os cenários (sucesso e falha)
- Inclua mensagens de log/console para demonstrar o fluxo
- Comente o código explicando as decisões

## Exercício 1: Calculadora com Tratamento de Exceções ⭐

**Objetivo:** Criar uma calculadora que trata exceções de divisão e entrada inválida.

**Requisitos:**
1. Crie uma classe `Calculadora` com métodos: `Somar`, `Subtrair`, `Multiplicar`, `Dividir`
2. O método `Dividir` deve lançar `DivideByZeroException` quando divisor for zero
3. Crie uma interface de console que:
   - Solicita dois números ao usuário
   - Solicita a operação (+ - * /)
   - Trata `FormatException` para entrada inválida
   - Trata `DivideByZeroException` para divisão por zero
   - Usa `finally` para exibir "Operação finalizada"

**Exemplo de Saída:**
```
Digite o primeiro número: 10
Digite o segundo número: abc
Erro: Entrada inválida. Digite apenas números.
Operação finalizada.
```

**Dica:** Use `double.TryParse` para conversão segura OU `double.Parse` dentro de try-catch.

---

## Exercício 2: Validador de CPF ⭐⭐

**Objetivo:** Criar um validador que usa exceções personalizadas para diferentes tipos de erro.

**Requisitos:**
1. Crie um método `ValidarCPF(string cpf)` que valida:
   - CPF não pode ser null → `ArgumentNullException`
   - CPF deve ter 11 dígitos → `ArgumentException`
   - CPF não pode ter todos dígitos iguais (ex: 111.111.111-11) → `ArgumentException`
   - Dígitos verificadores devem ser válidos → `ArgumentException`

2. Crie um programa que:
   - Lê CPF do usuário
   - Chama `ValidarCPF`
   - Captura e exibe mensagens específicas para cada tipo de exceção
   - Usa exception filters (`when`) para diferenciar mensagens

**Exemplos de CPF:**
- Válido: 123.456.789-09
- Inválido (todos iguais): 111.111.111-11
- Inválido (tamanho): 123.456
- Inválido (dígito verificador): 123.456.789-00

**Dica:** Algoritmo do CPF disponível na correção.

---

## Exercício 3: Gerenciador de Arquivos com Finally ⭐⭐

**Objetivo:** Demonstrar uso de `finally` para liberar recursos.

**Requisitos:**
1. Crie um método `CopiarArquivo(string origem, string destino)` que:
   - Abre o arquivo de origem para leitura
   - Cria o arquivo de destino para escrita
   - Copia o conteúdo byte a byte
   - **Usa `finally` para garantir que ambos os streams sejam fechados**
   - Trata `FileNotFoundException`, `UnauthorizedAccessException`, `IOException`

2. Crie uma versão alternativa usando `using` statements

3. Compare as duas abordagens em comentários

**Teste:**
- Arquivo que existe
- Arquivo que não existe
- Caminho sem permissão de escrita (ex: C:\Windows\arquivo.txt no Windows)

**Desafio Extra:** Adicione uma barra de progresso que mostra % copiado mesmo se houver erro.

---

## Exercício 4: Stack Unwinding em Ação ⭐⭐

**Objetivo:** Entender como exceções propagam pela pilha de chamadas.

**Requisitos:**
1. Crie uma hierarquia de métodos:
   ```
   Main() → MetodoA() → MetodoB() → MetodoC()
   ```

2. `MetodoC()` lança uma `InvalidOperationException`

3. Implemente 3 cenários diferentes:
   - **Cenário 1:** Nenhum método tem try-catch (exceção não tratada)
   - **Cenário 2:** `MetodoA` tem try-catch
   - **Cenário 3:** `MetodoB` tem try-catch

4. Em cada cenário, adicione logging em cada método para mostrar:
   - Quando o método inicia
   - Quando o método termina normalmente
   - Quando o método termina com exceção

**Exemplo de Saída (Cenário 2):**
```
Main: Iniciado
MetodoA: Iniciado
MetodoB: Iniciado
MetodoC: Iniciado
MetodoC: Lançando exceção
MetodoB: Finalizando (sem catch)
MetodoA: Exceção capturada: Operação inválida
MetodoA: Finalizando
Main: Finalizando
```

---

## Exercício 5: Exception Filters (when clauses) ⭐⭐⭐

**Objetivo:** Usar filtros de exceção do C# 6+.

**Requisitos:**
1. Crie uma classe `HttpException` com propriedade `StatusCode`:
   ```csharp
   public class HttpException : Exception
   {
       public int StatusCode { get; }
       public HttpException(int statusCode, string message) 
           : base(message) 
       {
           StatusCode = statusCode;
       }
   }
   ```

2. Crie um método `ProcessarRequisicao()` que simula chamadas HTTP:
   - Aleatoriamente lança `HttpException` com diferentes status codes
   - Códigos possíveis: 400, 404, 500, 503

3. No método chamador, use exception filters para:
   - Capturar e logar 404 (não encontrado) → não re-lança
   - Capturar 500-599 (erro servidor) → re-lança após log
   - Ignorar 400 (bad request) → propaga sem tratar
   - Capturar qualquer exceção e logar SEM capturar (filtro que retorna false)

**Dica:** Crie um método auxiliar `LogarExcecao(Exception ex)` que sempre retorna `false`.

**Exemplo:**
```csharp
catch (HttpException ex) when (ex.StatusCode == 404)
{
    Console.WriteLine("Recurso não encontrado");
}
catch (HttpException ex) when (ex.StatusCode >= 500 && ex.StatusCode < 600)
{
    Console.WriteLine($"Erro no servidor: {ex.StatusCode}");
    throw;
}
catch (Exception ex) when (LogarExcecao(ex))
{
    // Nunca executa (LogarExcecao retorna false)
    // Mas loga antes de propagar
}
```

---

## Exercício 6: Inner Exception e Exception Wrapping ⭐⭐⭐

**Objetivo:** Preservar contexto ao transformar exceções entre camadas.

**Requisitos:**
1. Crie 3 camadas simuladas:
   - **Camada de Dados:** Lança `SqlException` (simule com uma exception customizada)
   - **Camada de Negócio:** Captura e wrappea em `InvalidOperationException`
   - **Camada de Apresentação:** Captura e exibe mensagem amigável

2. Cada camada deve:
   - Adicionar contexto relevante na mensagem
   - Preservar a exceção original como `InnerException`
   - Logar o StackTrace completo

3. No final, percorra toda a cadeia de `InnerException` e exiba:
   ```
   Exceção Principal: InvalidOperationException - Falha ao processar pedido #123
       Causada por: DataException - Erro ao acessar banco de dados
           Causada por: SqlException - Connection timeout
   ```

**Desafio:** Crie um método `ExibirCadeiaCompleta(Exception ex)` que formata recursivamente.

---

## Exercício 7: Performance - Try/Parse vs Try/Catch ⭐⭐⭐

**Objetivo:** Medir impacto de performance de exceções.

**Requisitos:**
1. Implemente 2 métodos de conversão de string para int:
   - `ConverterComException(string)` - usa `int.Parse` + try/catch
   - `ConverterComTryParse(string)` - usa `int.TryParse`

2. Execute cada método 100.000 vezes com:
   - 50% de entradas válidas ("123")
   - 50% de entradas inválidas ("abc")

3. Use `Stopwatch` para medir tempo de cada abordagem

4. Calcule e exiba:
   - Tempo total de cada abordagem
   - Tempo médio por operação
   - Diferença percentual

**Exemplo de Saída:**
```
Try/Parse: 15ms (média 0.00015ms por operação)
Try/Catch: 2.350ms (média 0.0235ms por operação)
Try/Catch é 156x mais lento
```

**Conclusão:** Documente quando usar cada abordagem.

---

## Exercício 8: Sistema de Validação Robusto ⭐⭐⭐⭐

**Objetivo:** Criar um sistema completo de validação com múltiplas exceções.

**Requisitos:**
1. Crie uma classe `Usuario` com propriedades: Nome, Email, Idade, CPF

2. Crie uma classe `ValidadorUsuario` com método `Validar(Usuario user)` que:
   - Valida cada campo com exceções específicas
   - Acumula múltiplos erros usando `AggregateException`
   - Retorna todos os erros de uma vez (não para no primeiro)

3. Implemente validações:
   - Nome: não pode ser null/vazio, mínimo 3 caracteres
   - Email: não pode ser null, deve conter @, deve ter domínio
   - Idade: entre 0 e 150
   - CPF: validação completa (exercício 2)

4. No Main:
   - Crie usuários válidos e inválidos
   - Capture `AggregateException`
   - Exiba todos os erros de forma formatada

**Exemplo de Saída:**
```
Validação falhou com 3 erros:
1. ArgumentException: Nome deve ter no mínimo 3 caracteres
2. ArgumentException: Email inválido: falta domínio
3. ArgumentOutOfRangeException: Idade 200 está fora do intervalo [0-150]
```

**Desafio:** Permita validação "fail-fast" (para no primeiro erro) ou "collect-all" (acumula todos).

---

## Exercício 9: Global Exception Handler (Simulado) ⭐⭐⭐⭐

**Objetivo:** Criar um handler global para exceções não tratadas.

**Requisitos:**
1. Configure `AppDomain.CurrentDomain.UnhandledException` para capturar exceções não tratadas

2. Crie um método `TratadorGlobalExcecoes` que:
   - Loga a exceção com timestamp, tipo e stack trace
   - Salva em arquivo "crash_report.txt"
   - Exibe mensagem amigável ao usuário
   - Tenta fazer cleanup de recursos críticos

3. Simule diferentes tipos de exceções não tratadas:
   - NullReferenceException (bug)
   - OutOfMemoryException (recurso)
   - DivideByZeroException (lógica)

4. Para cada tipo, o handler deve logar de forma diferente

**Desafio:** Adicione handler para `TaskScheduler.UnobservedTaskException` (exceções em Tasks).

---

## Exercício 10: Retry Pattern com Exceções ⭐⭐⭐⭐⭐

**Objetivo:** Implementar retry logic para operações que podem falhar temporariamente.

**Requisitos:**
1. Crie um método `ExecutarComRetry<T>(Func<T> operacao, int tentativas, TimeSpan delay)` que:
   - Executa a operação
   - Se lançar exceção "retriable" (ex: `TimeoutException`, `IOException`), tenta novamente
   - Se lançar exceção "fatal" (ex: `ArgumentException`), falha imediatamente
   - Espera `delay` entre tentativas com backoff exponencial
   - Após `tentativas` falhas, lança `AggregateException` com todas as tentativas

2. Implemente operação simulada `ChamarApiExterna()` que:
   - Falha aleatoriamente 70% do tempo com `TimeoutException`
   - Sucede 30% do tempo

3. Use o retry para chamar a API com:
   - Máximo 5 tentativas
   - Delay inicial de 100ms
   - Backoff exponencial (100ms, 200ms, 400ms, 800ms, 1600ms)

**Exemplo de Saída:**
```
Tentativa 1: Timeout após 100ms
Aguardando 100ms...
Tentativa 2: Timeout após 150ms
Aguardando 200ms...
Tentativa 3: Sucesso!
```

**Desafio:** Adicione circuit breaker (após N falhas consecutivas, para de tentar por X tempo).

---

## 🎯 Critérios de Avaliação

Para cada exercício, considere se você:

- [ ] Usou o tipo de exceção mais apropriado
- [ ] Forneceu mensagens de erro claras e acionáveis
- [ ] Preservou o stack trace quando necessário
- [ ] Usou `finally` ou `using` para limpar recursos
- [ ] Testou todos os caminhos de exceção
- [ ] Documentou decisões de design em comentários
- [ ] Evitou catch-all desnecessários
- [ ] Seguiu o princípio fail-fast quando apropriado

## 📚 Recursos Adicionais

- [Exception Handling Best Practices](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)
- [Exception Class](https://learn.microsoft.com/en-us/dotnet/api/system.exception)
- [Try-Catch](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch)

---

**Tempo estimado:** 6-8 horas para todos os exercícios  
**Nível:** Básico (ex 1-3), Intermediário (ex 4-6), Avançado (ex 7-10)
