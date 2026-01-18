// 1. Swallowing exceptions
try { ProcessarPedido(); } catch { }

// 2. catch (Exception)
try { ProcessarPedido(); } catch (Exception ex) { Log(ex); }

// 3. throw ex (perde stack trace)
catch (Exception ex) { throw ex; }

// 4. Exceções para controle de fluxo
if (usuario == null) throw new Exception("Não encontrado");

// 5. Mensagens genéricas
throw new Exception("Erro");

// ═══════════════════════════════════════════════════

// 1. Não suprimir exceções
try 
{ 
    ProcessarPedido(); 
} 
catch (Exception ex) 
{ 
    Log.Error(ex, "Falha ao processar pedido");
    throw;
}

// 2. Catch específico
try 
{ 
    ProcessarPedido(); 
} 
catch (InvalidOperationException ex) 
{ 
    Log.Warning(ex, "Operação inválida");
    return Result.Fail("Pedido em estado inválido");
}

// 3. Preservar stack trace
catch (Exception ex) 
{ 
    throw; // SEM ex
}

// 4. Usar return para controle de fluxo
if (usuario == null) 
    return Result.NotFound("Usuário não encontrado");

// 5. Mensagens descritivas
throw new InvalidOperationException(
    $"Pedido {pedidoId} não pode ser processado no estado {estado}");