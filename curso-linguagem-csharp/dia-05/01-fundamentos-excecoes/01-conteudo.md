# Fundamentos de Exceções em C#

## 📚 Introdução

Exceções são o mecanismo do .NET para lidar com situações anormais que ocorrem durante a execução de um programa. Diferente de códigos de retorno, exceções interrompem o fluxo normal de execução e permitem que erros sejam tratados em locais apropriados da aplicação.

## 🎯 Objetivos

Ao final deste tópico, você será capaz de:
- Entender a hierarquia de exceções do .NET
- Usar try-catch-finally corretamente
- Escolher os tipos apropriados de exceções
- Aplicar filtros de exceção
- Decidir quando lançar vs capturar exceções

## 🏗️ Hierarquia de Exceções no .NET

Todas as exceções em C# derivam da classe `System.Exception`:

```
System.Object
    └── System.Exception
            ├── System.SystemException
            │       ├── ArgumentException
            │       │       ├── ArgumentNullException
            │       │       ├── ArgumentOutOfRangeException
            │       │       └── ArgumentEmptyException (custom)
            │       ├── InvalidOperationException
            │       ├── NullReferenceException
            │       ├── IndexOutOfRangeException
            │       ├── NotSupportedException
            │       └── FormatException
            ├── System.ApplicationException (deprecated base)
            └── Custom Exceptions (derive from Exception)
```

### Propriedades Importantes de Exception

```csharp
public class Exception
{
    public string Message { get; }              // Mensagem descritiva do erro
    public Exception? InnerException { get; }   // Exceção que causou esta
    public string StackTrace { get; }           // Stack trace completo
    public IDictionary Data { get; }            // Dados contextuais adicionais
    public string? Source { get; set; }         // Assembly que lançou
    public string? HelpLink { get; set; }       // Link para documentação
    public int HResult { get; set; }            // Código de erro (HRESULT)
}
```

## 🔧 Try-Catch-Finally: Sintaxe e Semântica

### Estrutura Básica

```csharp
try
{
    // Código que pode lançar exceções
    int resultado = DividirNumeros(10, 0);
}
catch (DivideByZeroException ex)
{
    // Tratamento específico para divisão por zero
    Console.WriteLine($"Erro de divisão: {ex.Message}");
}
catch (Exception ex)
{
    // Tratamento genérico (sempre no final)
    Console.WriteLine($"Erro inesperado: {ex.Message}");
    throw; // Re-lança a exceção preservando o stack trace
}
finally
{
    // Sempre executado, mesmo se houver return ou exception
    // Usado para cleanup de recursos
    Console.WriteLine("Operação finalizada");
}
```

### Ordem dos Catch Blocks

**IMPORTANTE**: Catch blocks devem ser ordenados do mais específico para o mais genérico:

```csharp
try
{
    ProcessarArquivo("dados.txt");
}
catch (FileNotFoundException ex)          // ✅ Mais específico primeiro
{
    Console.WriteLine("Arquivo não encontrado");
}
catch (UnauthorizedAccessException ex)    // ✅ Específico
{
    Console.WriteLine("Sem permissão para acessar");
}
catch (IOException ex)                     // ✅ Genérico
{
    Console.WriteLine("Erro de I/O");
}
catch (Exception ex)                       // ✅ Mais genérico por último
{
    Console.WriteLine("Erro desconhecido");
}
```

### Finally Block: Garantias

O bloco `finally` **sempre** executa, exceto em casos extremos:
- `Environment.FailFast()` é chamado
- Processo é morto externamente
- Stack overflow fatal

```csharp
FileStream? stream = null;
try
{
    stream = File.OpenRead("dados.txt");
    // Processa arquivo
}
catch (IOException ex)
{
    Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
}
finally
{
    // Garante que o recurso seja liberado
    stream?.Dispose();
}
```

**Padrão moderno com using**: Preferred para IDisposable:

```csharp
try
{
    using FileStream stream = File.OpenRead("dados.txt");
    // stream.Dispose() é chamado automaticamente ao final do escopo
}
catch (IOException ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}
```

## 🎨 Exception Filters (When Clauses)

Introduzido no C# 6, permite filtrar exceções com condições:

```csharp
try
{
    var resultado = await ChamarApiExterna();
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    // Trata apenas 404
    Console.WriteLine("Recurso não encontrado");
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
{
    // Trata apenas 401
    Console.WriteLine("Não autorizado - renovando token...");
    await RenovarToken();
}
catch (HttpRequestException ex)
{
    // Outros erros HTTP
    Console.WriteLine($"Erro HTTP: {ex.Message}");
}
```

### Vantagens dos Filters

1. **Preserva Stack Trace**: Filter que retorna `false` não "captura" a exceção
2. **Performance**: Evita capturar e re-lançar exceções desnecessariamente
3. **Expressividade**: Código mais limpo que múltiplos catch/if

```csharp
// ❌ Sem filter - captura e re-lança (perde performance)
catch (SqlException ex)
{
    if (ex.Number == 1205) // Deadlock
    {
        // Trata deadlock
    }
    else
    {
        throw; // Re-lança
    }
}

// ✅ Com filter - não captura se condição for false
catch (SqlException ex) when (ex.Number == 1205)
{
    // Trata apenas deadlock
}
```

### Filters com Side Effects (Use com Cuidado)

```csharp
catch (Exception ex) when (LogException(ex))
{
    // Este bloco NUNCA executa se LogException retorna false
}

bool LogException(Exception ex)
{
    // Side effect: loga mesmo se não capturar
    logger.LogError(ex, "Exceção ocorreu");
    return false; // Não captura, apenas loga
}
```

## 🚀 Tipos Comuns de Exceções

### 1. ArgumentException (e derivadas)

Para validação de parâmetros de métodos:

```csharp
public class ContaBancaria
{
    public decimal Saldo { get; private set; }
    
    public void Sacar(decimal valor)
    {
        // ArgumentOutOfRangeException - valor fora do range válido
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), 
                valor, "Valor deve ser positivo");
        
        // InvalidOperationException - estado inválido para operação
        if (valor > Saldo)
            throw new InvalidOperationException(
                $"Saldo insuficiente. Saldo: {Saldo:C}, Tentativa: {valor:C}");
        
        Saldo -= valor;
    }
    
    public void DefinirTitular(string nome)
    {
        // ArgumentNullException - parâmetro null não permitido
        if (nome is null)
            throw new ArgumentNullException(nameof(nome));
        
        // ArgumentException - valor inválido genérico
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException(
                "Nome não pode ser vazio ou apenas espaços", nameof(nome));
        
        // Implementação...
    }
}
```

### 2. InvalidOperationException

Para operações que são inválidas no estado atual:

```csharp
public class Pedido
{
    public StatusPedido Status { get; private set; }
    public List<ItemPedido> Itens { get; } = new();
    
    public void Finalizar()
    {
        if (Status != StatusPedido.Aberto)
            throw new InvalidOperationException(
                $"Pedido não pode ser finalizado no status {Status}");
        
        if (!Itens.Any())
            throw new InvalidOperationException(
                "Pedido não possui itens");
        
        Status = StatusPedido.Finalizado;
    }
}
```

### 3. NotSupportedException

Para operações não suportadas pela implementação:

```csharp
public abstract class Forma
{
    public abstract double CalcularArea();
    
    // Método que pode ser não suportado em algumas formas
    public virtual double CalcularVolume()
    {
        throw new NotSupportedException(
            $"{GetType().Name} é uma forma 2D e não possui volume");
    }
}

public class Circulo : Forma
{
    public double Raio { get; set; }
    
    public override double CalcularArea() => Math.PI * Raio * Raio;
    
    // Não sobrescreve CalcularVolume - lançará NotSupportedException
}

public class Esfera : Forma
{
    public double Raio { get; set; }
    
    public override double CalcularArea() => 4 * Math.PI * Raio * Raio;
    
    public override double CalcularVolume() => (4.0/3) * Math.PI * Math.Pow(Raio, 3);
}
```

### 4. NullReferenceException

**EVITE LANÇAR**: É quase sempre um bug, não uma condição esperada:

```csharp
// ❌ MAU: Lançar NullReferenceException manualmente
if (cliente is null)
    throw new NullReferenceException("Cliente é null");

// ✅ BOM: Use ArgumentNullException para parâmetros
if (cliente is null)
    throw new ArgumentNullException(nameof(cliente));

// ✅ MELHOR: Use null-conditional e null-coalescing
string nome = cliente?.Nome ?? "Desconhecido";

// ✅ MODERNO: Use nullable reference types
public void ProcessarCliente(Cliente cliente) // cliente não pode ser null
{
    // Se chegar null aqui, é um bug no chamador
    Console.WriteLine(cliente.Nome); // Compiler warning se cliente for nullable
}
```

## 🎯 Quando Lançar vs Quando Capturar

### Quando Lançar Exceções

1. **Pré-condições violadas**:
```csharp
public void DefinirIdade(int idade)
{
    if (idade < 0 || idade > 150)
        throw new ArgumentOutOfRangeException(nameof(idade));
    
    Idade = idade;
}
```

2. **Operação impossível no estado atual**:
```csharp
public void IniciarJogo()
{
    if (Jogadores.Count < 2)
        throw new InvalidOperationException("Mínimo 2 jogadores necessários");
}
```

3. **Recursos não disponíveis**:
```csharp
public void AbrirConexao()
{
    if (!RedeDisponivel())
        throw new InvalidOperationException("Sem conexão de rede");
}
```

### Quando Capturar Exceções

1. **Você pode lidar com o erro de forma significativa**:
```csharp
try
{
    var dados = CarregarDados();
}
catch (FileNotFoundException)
{
    // Cria arquivo com dados padrão
    dados = CriarDadosPadrao();
}
```

2. **Para adicionar contexto antes de re-lançar**:
```csharp
try
{
    ProcessarPedido(pedido);
}
catch (Exception ex)
{
    throw new InvalidOperationException(
        $"Erro ao processar pedido {pedido.Id}", ex); // InnerException preservado
}
```

3. **Para logging e telemetria**:
```csharp
try
{
    await ExecutarOperacao();
}
catch (Exception ex)
{
    logger.LogError(ex, "Falha na operação para usuário {UserId}", userId);
    throw; // Re-lança preservando stack trace
}
```

### Quando NÃO Capturar

1. **❌ Captura genérica sem tratamento**:
```csharp
// MAU - engole todos os erros
try
{
    FazerAlgo();
}
catch { } // Silencia TUDO, incluindo OutOfMemoryException
```

2. **❌ Capturar apenas para logar e re-lançar**:
```csharp
// MAU - adiciona overhead sem valor
try
{
    FazerAlgo();
}
catch (Exception ex)
{
    Console.WriteLine(ex); // Apenas loga
    throw; // Re-lança
}
// BOM: Use filter ou deixe propagar e logue em um ponto central
```

3. **❌ Capturar exceções que você não pode tratar**:
```csharp
// MAU - captura mas não sabe o que fazer
try
{
    ConectarBancoDados();
}
catch (SqlException ex)
{
    // E agora? Não tem fallback
    MessageBox.Show("Erro de banco"); // Pior que deixar propagar
}
```

## ⚡ Stack Unwinding e Exception Propagation

Quando uma exceção é lançada, o runtime percorre a call stack procurando um handler apropriado:

```csharp
void MetodoA()
{
    try
    {
        MetodoB(); // Exceção propagada de B → lançada em C
    }
    catch (InvalidOperationException ex)
    {
        // Captura aqui se B ou C lançar InvalidOperationException
        Console.WriteLine("Tratado em A");
    }
}

void MetodoB()
{
    // Não tem try-catch, exceção propaga para A
    MetodoC();
}

void MetodoC()
{
    // Exceção lançada aqui
    throw new InvalidOperationException("Erro em C");
}

// Call stack: A → B → C → THROW
// Unwinding:  A ← B ← C ← Exception
```

### Preserve Stack Trace

```csharp
// ❌ MAU - perde stack trace original
catch (Exception ex)
{
    throw ex; // NUNCA FAÇA ISSO
}

// ✅ BOM - preserva stack trace
catch (Exception ex)
{
    throw; // Re-lança a MESMA instância
}

// ✅ BOM - wrapping com InnerException
catch (Exception ex)
{
    throw new InvalidOperationException("Contexto adicional", ex);
}
```

## 🆚 Exceções vs Códigos de Retorno

### Exceções (Preferred em C#)

**Vantagens:**
- Impossível ignorar (compilador força tratamento)
- Separa happy path do error handling
- Propagação automática pela call stack
- Contexto rico (stack trace, inner exceptions, data)

```csharp
// Código limpo - happy path claro
public decimal CalcularDesconto(Pedido pedido)
{
    if (pedido is null)
        throw new ArgumentNullException(nameof(pedido));
    
    if (pedido.Total <= 0)
        throw new ArgumentException("Total deve ser positivo");
    
    // Happy path sem if/else de erro
    return pedido.Total * ObterPercentualDesconto(pedido.Cliente);
}
```

**Desvantagens:**
- Performance: ~1000x mais lento que `return`
- Fluxo de controle "invisível" (pode ser confuso)

### Códigos de Retorno

**Vantagens:**
- Performance: extremamente rápido
- Fluxo explícito

**Desvantagens:**
- Fácil esquecer de checar
- Difícil propagar contexto

```csharp
// Código difícil de identificar que os primeiros ifs são erros
public (bool success, decimal valor, string erro) CalcularDesconto(Pedido pedido)
{
    if (pedido is null)
        return (false, 0, "Pedido é null");
    
    if (pedido.Total <= 0)
        return (false, 0, "Total inválido");
    
    var percentual = ObterPercentualDesconto(pedido.Cliente);
    return (true, pedido.Total * percentual, null);
}

// Chamador precisa verificar manualmente
var (sucesso, desconto, erro) = CalcularDesconto(pedido);
if (!sucesso)
{
    Console.WriteLine($"Erro: {erro}");
    return;
}
// Usa desconto
```

### Quando Usar Cada Um

| Situação | Use |
|----------|-----|
| Falha é excepcional (< 1% casos) | **Exceções** |
| Falha é esperada (validação input) | **Códigos ou Result<T>** |
| Performance crítica (hot path) | **Códigos ou Result<T>** |
| API pública/library | **Exceções** (documentadas) |
| Falha requer contexto rico | **Exceções** |

### Padrão Moderno: Result<T>

Combina vantagens de ambos:

```csharp
public record Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    
    public static Result<T> Success(T value) => 
        new() { IsSuccess = true, Value = value };
    
    public static Result<T> Failure(string error) => 
        new() { IsSuccess = false, Error = error };
}

// Uso
public Result<decimal> CalcularDesconto(Pedido pedido)
{
    if (pedido is null)
        return Result<decimal>.Failure("Pedido é null");
    
    if (pedido.Total <= 0)
        return Result<decimal>.Failure("Total inválido");
    
    var desconto = pedido.Total * ObterPercentualDesconto(pedido.Cliente);
    return Result<decimal>.Success(desconto);
}

// Chamador
var resultado = CalcularDesconto(pedido);
if (!resultado.IsSuccess)
{
    logger.LogWarning("Falha ao calcular desconto: {Erro}", resultado.Error);
    return;
}
decimal desconto = resultado.Value;
```


## 🎓 Resumo e Melhores Práticas

### ✅ Faça

1. Use exceções para situações excepcionais, não para controle de fluxo
2. Lance exceções específicas (ArgumentNullException, não Exception)
3. Preserve stack trace ao re-lançar (use `throw;`)
4. Use finally ou using para cleanup de recursos
5. Ordene catch blocks do mais específico ao mais genérico
6. Adicione contexto ao lançar exceções customizadas
7. Use exception filters para condições complexas
8. Documente exceções que sua API pode lançar

### ❌ Não Faça

1. Não capture exceções que você não pode tratar
2. Não engula exceções silenciosamente (catch vazio)
3. Não use exceções para validação de input comum
4. Não lance ou capture System.Exception diretamente
5. Não ignore o InnerException
6. Não use throw ex (perde stack trace)
7. Não capture para apenas logar e re-lançar sem valor adicional
8. Não lance NullReferenceException manualmente

## 🔗 Próximos Passos

Agora que você domina os fundamentos de exceções:
1. Complete os exercícios propostos
2. Leia sobre [Exceções Customizadas](../02-excecoes-customizadas/01-conteudo.md)
3. Explore [Depuração no VS Code](../03-depuracao-vscode/01-conteudo.md)
4. Pratique [Logging estruturado](../04-logging-rastreamento/01-conteudo.md)

---

**Próximo:** [02-exercicios.md](./02-exercicios.md) | **Tempo estimado:** 90 minutos
