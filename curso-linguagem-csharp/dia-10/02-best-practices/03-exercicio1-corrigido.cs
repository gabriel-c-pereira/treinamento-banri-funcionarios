// Interfaces seguindo ISP
public interface IOrderValidator
{
    Task ValidateAsync(Order order);
}

public interface IOrderCalculator
{
    decimal CalculateTotal(Order order);
}

public interface IOrderRepository
{
    Task SaveAsync(Order order);
}

public interface IEmailService
{
    Task SendOrderConfirmationAsync(string customerEmail, decimal total);
}

// Implementações seguindo SRP
public class OrderValidator : IOrderValidator
{
    public async Task ValidateAsync(Order order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        if (order.Items == null || order.Items.Count == 0)
            throw new ValidationException("Pedido deve conter pelo menos um item");

        if (string.IsNullOrEmpty(order.CustomerEmail))
            throw new ValidationException("Email do cliente é obrigatório");

        // Outras validações...
        await Task.CompletedTask;
    }
}

public class OrderCalculator : IOrderCalculator
{
    public decimal CalculateTotal(Order order)
    {
        if (order?.Items == null)
            return 0;

        return order.Items.Sum(item => item.Price * item.Quantity);
    }
}

public class SqlOrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public SqlOrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task SaveAsync(Order order)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Implementação do salvamento
        const string sql = @"
            INSERT INTO Orders (Id, CustomerEmail, Total, CreatedAt)
            VALUES (@Id, @CustomerEmail, @Total, @CreatedAt)";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", order.Id);
        command.Parameters.AddWithValue("@CustomerEmail", order.CustomerEmail);
        command.Parameters.AddWithValue("@Total", order.Total);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        await command.ExecuteNonQueryAsync();

        // Salvar itens do pedido
        foreach (var item in order.Items)
        {
            // Implementação para salvar itens
        }
    }
}

public class SmtpEmailService : IEmailService
{
    private readonly string _smtpServer;

    public SmtpEmailService(string smtpServer)
    {
        _smtpServer = smtpServer;
    }

    public async Task SendOrderConfirmationAsync(string customerEmail, decimal total)
    {
        using var smtp = new SmtpClient(_smtpServer);
        var mail = new MailMessage(
            "admin@store.com",
            customerEmail,
            "Pedido Confirmado",
            $"Seu pedido no valor de R$ {total:N2} foi processado com sucesso!");

        await smtp.SendMailAsync(mail);
    }
}

// Classe principal seguindo DIP e SRP
public class OrderProcessor
{
    private readonly IOrderValidator _validator;
    private readonly IOrderCalculator _calculator;
    private readonly IOrderRepository _repository;
    private readonly IEmailService _emailService;

    public OrderProcessor(
        IOrderValidator validator,
        IOrderCalculator calculator,
        IOrderRepository repository,
        IEmailService emailService)
    {
        _validator = validator;
        _calculator = calculator;
        _repository = repository;
        _emailService = emailService;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        // SRP: Cada responsabilidade é delegada para sua interface
        await _validator.ValidateAsync(order);

        order.Total = _calculator.CalculateTotal(order);

        await _repository.SaveAsync(order);

        await _emailService.SendOrderConfirmationAsync(order.CustomerEmail, order.Total);
    }
}

// Configuração de DI (seguindo DIP)
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrderProcessing(this IServiceCollection services)
    {
        services.AddScoped<IOrderValidator, OrderValidator>();
        services.AddScoped<IOrderCalculator, OrderCalculator>();
        services.AddScoped<IOrderRepository, SqlOrderRepository>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<OrderProcessor>();

        return services;
    }
}

// Modelos
public class Order
{
    public Guid Id { get; set; }
    public string CustomerEmail { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}