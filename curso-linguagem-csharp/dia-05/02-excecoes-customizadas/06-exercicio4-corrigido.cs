public class PagamentoException : Exception
{
    public string NumeroCartao { get; }
    public decimal Valor { get; }
    public string Gateway { get; }
    public DateTime Timestamp { get; }
    public Dictionary<string, object> Metadata { get; }

    private PagamentoException(string message, string numeroCartao, decimal valor, 
        string gateway, Dictionary<string, object> metadata, Exception? innerException = null)
        : base(message, innerException)
    {
        NumeroCartao = numeroCartao;
        Valor = valor;
        Gateway = gateway;
        Timestamp = DateTime.UtcNow;
        Metadata = metadata ?? new();
    }

    public class Builder
    {
        private string _numeroCartao = string.Empty;
        private decimal _valor;
        private string _gateway = "Unknown";
        private readonly Dictionary<string, object> _metadata = new();
        private Exception? _innerException;

        public Builder ComCartao(string numero)
        {
            _numeroCartao = numero;
            return this;
        }

        public Builder ComValor(decimal valor)
        {
            _valor = valor;
            return this;
        }

        public Builder NoGateway(string gateway)
        {
            _gateway = gateway;
            return this;
        }

        public Builder ComMetadata(string chave, object valor)
        {
            _metadata[chave] = valor;
            return this;
        }

        public Builder CausadaPor(Exception inner)
        {
            _innerException = inner;
            return this;
        }

        public PagamentoException Build(string mensagem)
        {
            return new PagamentoException(
                mensagem, _numeroCartao, _valor, _gateway, _metadata, _innerException);
        }
    }
}

// Uso
var exception = new PagamentoException.Builder()
    .ComCartao("**** 1234")
    .ComValor(150.00m)
    .NoGateway("PaymentPro")
    .ComMetadata("PedidoId", 1001)
    .ComMetadata("TentativaNumero", 3)
    .Build("Timeout ao processar pagamento");

throw exception;