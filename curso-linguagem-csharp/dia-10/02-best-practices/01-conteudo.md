# Melhores Práticas

## Clean Architecture

### Camadas da Arquitetura
```
┌─────────────────────────────────────┐
│        Presentation Layer           │  Controllers, Views, DTOs
├─────────────────────────────────────┤
│        Application Layer            │  Services, Commands, Queries
├─────────────────────────────────────┤
│        Domain Layer                 │  Entities, Value Objects, Domain Services
├─────────────────────────────────────┤
│        Infrastructure Layer         │  Repositories, External Services
└─────────────────────────────────────┘
```

### Princípios SOLID

#### Single Responsibility Principle (SRP)
```csharp
// ❌ Ruim - Classe faz muitas coisas
public class UserService
{
    public void CreateUser(User user) { /* validação + persistência + email */ }
    public void SendEmail(User user) { /* envio de email */ }
    public void ValidateUser(User user) { /* validação */ }
    public void SaveToDatabase(User user) { /* persistência */ }
}

// ✅ Bom - Cada classe tem uma responsabilidade
public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IUserValidator _validator;

    public async Task CreateUserAsync(CreateUserCommand command)
    {
        var user = new User(command.Name, command.Email);
        await _validator.ValidateAsync(user);
        await _repository.AddAsync(user);
        await _emailService.SendWelcomeEmailAsync(user);
    }
}
```

#### Open/Closed Principle (OCP)
```csharp
// ❌ Ruim - Modificação direta da classe
public class PaymentProcessor
{
    public void Process(Payment payment)
    {
        if (payment.Type == "CreditCard")
            ProcessCreditCard(payment);
        else if (payment.Type == "PayPal")
            ProcessPayPal(payment);
        // Adicionar novo tipo requer modificar esta classe
    }
}

// ✅ Bom - Extensão através de interfaces
public interface IPaymentProcessor
{
    bool CanProcess(string paymentType);
    Task ProcessAsync(Payment payment);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public bool CanProcess(string paymentType) => paymentType == "CreditCard";
    public async Task ProcessAsync(Payment payment) { /* implementação */ }
}

public class PaymentService
{
    private readonly IEnumerable<IPaymentProcessor> _processors;

    public PaymentService(IEnumerable<IPaymentProcessor> processors)
    {
        _processors = processors;
    }

    public async Task ProcessAsync(Payment payment)
    {
        var processor = _processors.FirstOrDefault(p => p.CanProcess(payment.Type));
        if (processor == null) throw new NotSupportedException();
        await processor.ProcessAsync(payment);
    }
}
```

#### Liskov Substitution Principle (LSP)
```csharp
// ❌ Ruim - Quebra LSP
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : Rectangle
{
    public override int Width
    {
        set { base.Width = base.Height = value; }
    }

    public override int Height
    {
        set { base.Width = base.Height = value; }
    }
}

// ✅ Bom - Hierarquia correta
public abstract class Shape
{
    public abstract int Area { get; }
}

public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public override int Area => Width * Height;
}

public class Square : Shape
{
    public int Side { get; set; }
    public override int Area => Side * Side;
}
```

#### Interface Segregation Principle (ISP)
```csharp
// ❌ Ruim - Interface gorda
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void Code();
    void Test();
    void Deploy();
}

// ✅ Bom - Interfaces segregadas
public interface IWorkable { void Work(); }
public interface IFeedable { void Eat(); }
public interface IRestable { void Sleep(); }
public interface IDevelopable { void Code(); void Test(); void Deploy(); }

public class Developer : IWorkable, IFeedable, IRestable, IDevelopable
{
    public void Work() => Code();
    public void Eat() { /* implementação */ }
    public void Sleep() { /* implementação */ }
    public void Code() { /* implementação */ }
    public void Test() { /* implementação */ }
    public void Deploy() { /* implementação */ }
}
```

#### Dependency Inversion Principle (DIP)
```csharp
// ❌ Ruim - Dependência de classe concreta
public class OrderService
{
    private readonly SqlOrderRepository _repository = new SqlOrderRepository();

    public void ProcessOrder(Order order)
    {
        _repository.Save(order);
    }
}

// ✅ Bom - Dependência de abstração
public interface IOrderRepository
{
    Task SaveAsync(Order order);
}

public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        await _repository.SaveAsync(order);
    }
}
```

## Domain-Driven Design (DDD)

### Entities e Value Objects
```csharp
// Entity
public class Customer : Entity<CustomerId>
{
    public CustomerId Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }

    public Customer(CustomerId id, string name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void ChangeAddress(Address newAddress)
    {
        // Regras de negócio
        Address = newAddress;
    }
}

// Value Object
public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format");

        Value = value;
    }

    private bool IsValidEmail(string email)
    {
        // Validação simplificada
        return email.Contains("@");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }
}
```

### Domain Services
```csharp
public interface ICustomerService
{
    Task<Customer> RegisterCustomerAsync(string name, string email);
    Task<bool> CanCustomerOrderAsync(CustomerId customerId, Order order);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;

    public CustomerService(
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Customer> RegisterCustomerAsync(string name, string email)
    {
        var existingCustomer = await _customerRepository.GetByEmailAsync(email);
        if (existingCustomer != null)
            throw new CustomerAlreadyExistsException(email);

        var customer = new Customer(
            CustomerId.New(),
            name,
            new Email(email));

        await _customerRepository.AddAsync(customer);
        return customer;
    }

    public async Task<bool> CanCustomerOrderAsync(CustomerId customerId, Order order)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null) return false;

        // Regras de negócio complexas
        var totalOrders = await _orderRepository.GetTotalByCustomerAsync(customerId);
        return totalOrders < 100; // Limite de pedidos por cliente
    }
}
```

### Repository Pattern
```csharp
public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(CustomerId id);
    Task<Customer> GetByEmailAsync(string email);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(CustomerId id);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public async Task<Customer> GetByIdAsync(CustomerId id)
    {
        return await _context.Customers
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email.Value == email);
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CustomerId id)
    {
        var customer = await GetByIdAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
```

## CQRS (Command Query Responsibility Segregation)

### Commands e Queries
```csharp
// Commands
public interface ICommand { }

public class CreateCustomerCommand : ICommand
{
    public string Name { get; }
    public string Email { get; }

    public CreateCustomerCommand(string name, string email)
    {
        Name = name;
        Email = email;
    }
}

public class UpdateCustomerCommand : ICommand
{
    public CustomerId CustomerId { get; }
    public string Name { get; }
    public string Email { get; }

    public UpdateCustomerCommand(CustomerId customerId, string name, string email)
    {
        CustomerId = customerId;
        Name = name;
        Email = email;
    }
}

// Queries
public interface IQuery<TResult> { }

public class GetCustomerByIdQuery : IQuery<CustomerDto>
{
    public CustomerId CustomerId { get; }

    public GetCustomerByIdQuery(CustomerId customerId)
    {
        CustomerId = customerId;
    }
}

public class GetAllCustomersQuery : IQuery<IEnumerable<CustomerDto>> { }
```

### Command Handlers
```csharp
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    private readonly ICustomerService _customerService;

    public CreateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task HandleAsync(CreateCustomerCommand command)
    {
        await _customerService.RegisterCustomerAsync(command.Name, command.Email);
    }
}

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(UpdateCustomerCommand command)
    {
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
        if (customer == null)
            throw new CustomerNotFoundException(command.CustomerId);

        customer.UpdateName(command.Name);
        customer.UpdateEmail(new Email(command.Email));

        await _customerRepository.UpdateAsync(customer);
    }
}
```

### Query Handlers
```csharp
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> HandleAsync(GetCustomerByIdQuery query)
    {
        var customer = await _customerRepository.GetByIdAsync(query.CustomerId);
        if (customer == null)
            throw new CustomerNotFoundException(query.CustomerId);

        return new CustomerDto
        {
            Id = customer.Id.Value,
            Name = customer.Name,
            Email = customer.Email.Value
        };
    }
}

public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetAllCustomersQuery query)
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id.Value,
            Name = c.Name,
            Email = c.Email.Value
        });
    }
}
```

## Testes

### Unit Tests
```csharp
[TestFixture]
public class CustomerServiceTests
{
    private Mock<ICustomerRepository> _repositoryMock;
    private Mock<IEmailService> _emailServiceMock;
    private CustomerService _service;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ICustomerRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _service = new CustomerService(_repositoryMock.Object, _emailServiceMock.Object);
    }

    [Test]
    public async Task RegisterCustomerAsync_ValidData_CreatesCustomer()
    {
        // Arrange
        var name = "João Silva";
        var email = "joao@email.com";
        var command = new CreateCustomerCommand(name, email);

        _repositoryMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync((Customer)null);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.RegisterCustomerAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Email.Value.Should().Be(email);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        _emailServiceMock.Verify(e => e.SendWelcomeEmailAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Test]
    public void RegisterCustomerAsync_ExistingEmail_ThrowsException()
    {
        // Arrange
        var email = "existing@email.com";
        var command = new CreateCustomerCommand("Nome", email);

        _repositoryMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(new Customer(CustomerId.New(), "Existing", new Email(email)));

        // Act & Assert
        Assert.ThrowsAsync<CustomerAlreadyExistsException>(
            () => _service.RegisterCustomerAsync(command));
    }
}
```

### Integration Tests
```csharp
[TestFixture]
public class CustomerRepositoryIntegrationTests : IDisposable
{
    private ApplicationDbContext _context;
    private CustomerRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Test]
    public async Task AddAsync_ValidCustomer_SavesToDatabase()
    {
        // Arrange
        var customer = new Customer(
            CustomerId.New(),
            "João Silva",
            new Email("joao@email.com"));

        // Act
        await _repository.AddAsync(customer);

        // Assert
        var savedCustomer = await _context.Customers.FindAsync(customer.Id);
        savedCustomer.Should().NotBeNull();
        savedCustomer.Name.Should().Be(customer.Name);
    }

    [Test]
    public async Task GetByIdAsync_ExistingCustomer_ReturnsCustomer()
    {
        // Arrange
        var customerId = CustomerId.New();
        var customer = new Customer(customerId, "João", new Email("joao@email.com"));
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
        result.Name.Should().Be("João");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```
