# Desenvolvimento Assíncrono

## Conceitos Fundamentais

### Async/Await Pattern
- `async` marca método assíncrono
- `await` aguarda operação assíncrona
- `Task` representa operação assíncrona
- `Task<T>` representa operação com resultado

### Benefícios
- Não bloqueia thread principal
- Melhor responsividade da aplicação
- Melhor utilização de recursos do servidor
- Suporte a operações I/O intensivas

### Boas Práticas
- Use async até a camada de apresentação
- Evite async void (exceto event handlers)
- Use ConfigureAwait(false) em bibliotecas
- Cancele operações quando apropriado

## Padrões Comuns

### 1. Controller Assíncrono
```csharp
[HttpGet]
public async Task<IActionResult> GetDataAsync()
{
    var data = await _service.GetDataAsync();
    return Ok(data);
}
```

### 2. Service Layer Assíncrono
```csharp
public async Task<List<Item>> GetItemsAsync()
{
    return await _context.Items.ToListAsync();
}
```

### 3. Repository Pattern Assíncrono
```csharp
public async Task<T> GetByIdAsync(int id)
{
    return await _dbSet.FindAsync(id);
}
```

## Cancellation Tokens

```csharp
public async Task<IActionResult> LongRunningOperationAsync(
    CancellationToken cancellationToken)
{
    var result = await _service.ProcessAsync(cancellationToken);
    return Ok(result);
}
```

## Tratamento de Exceções

```csharp
try
{
    await _service.OperationAsync();
}
catch (OperationCanceledException)
{
    // Operação cancelada
    return StatusCode(499, "Client Closed Request");
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro na operação");
    return StatusCode(500, "Erro interno");
}
```

## Performance Considerations

- Evite async quando não há I/O
- Use ValueTask para operações rápidas
- Considere paralelismo quando apropriado
- Monitore deadlocks e starvation
