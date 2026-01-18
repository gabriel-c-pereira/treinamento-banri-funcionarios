try
{
    ProcessarPagamento(pagamento);
}
catch (PaymentException ex)
{
    ex.Data["PedidoId"] = pagamento.PedidoId;
    ex.Data["Timestamp"] = DateTime.UtcNow;
    
    Log.ForContext("ValorTentado", pagamento.Valor)
       .Error(ex, "Falha no pagamento {@Pagamento}", pagamento);
    throw;
}