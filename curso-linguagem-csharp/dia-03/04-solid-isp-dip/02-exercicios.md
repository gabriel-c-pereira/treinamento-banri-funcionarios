# 🎯 Exercícios - SOLID: ISP e DIP

> **Tempo estimado**: 1 hora  
> **Dificuldade**: Avançado

---

## 📝 Instruções Gerais

- Aplique ISP: interfaces pequenas e específicas
- Aplique DIP: dependa de abstrações
- Use injeção de dependência
- Refatore código legado

---

## 🟢 Exercício 1: Refatorar "Fat Interface"

Refatore a interface abaixo aplicando ISP.

**❌ Código Problemático:**
```csharp
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void GetPaid();
    void TakeVacation();
}

public class Robot : IWorker // Robô não come, dorme ou tira férias!
{
    public void Work() { /* OK */ }
    public void Eat() => throw new NotImplementedException();
    public void Sleep() => throw new NotImplementedException();
    public void GetPaid() => throw new NotImplementedException();
    public void TakeVacation() => throw new NotImplementedException();
}
```

**✅ Requisitos:**
- Segregue em interfaces menores: `IWorkable`, `IFeedable`, `ISleepable`, `IPayable`
- Crie classes `Human`, `Robot`, `Animal`
- Cada um implementa apenas o necessário

---

## 🟢 Exercício 2: Injeção de Dependência Básica

Refatore o código para usar DIP.

**❌ Código Problemático:**
```csharp
public class UserService
{
    private EmailService _emailService = new EmailService(); // Acoplamento forte!
    
    public void RegisterUser(string email)
    {
        // Registrar usuário...
        _emailService.SendEmail(email, "Bem-vindo!");
    }
}
```

**✅ Requisitos:**
- Crie interface `INotificationService`
- Implemente `EmailService` e `SmsService`
- Use constructor injection
- Teste com ambos os serviços

---

## 🟡 Exercício 3: Sistema de Logging com DIP

Crie sistema de logging aplicando DIP.

**Requisitos:**
```csharp
public interface ILogger
{
    void Log(string message);
}

public class OrderService
{
    private readonly ILogger _logger;
    
    public OrderService(ILogger logger) // DIP: depende da abstração
    {
        _logger = logger;
    }
    
    public void CreateOrder(Order order)
    {
        _logger.Log($"Criando pedido {order.Id}");
        // ...
    }
}
```

**Implementações:**
- `ConsoleLogger`
- `FileLogger`
- `DatabaseLogger`
- `CompositeLogger` (combina múltiplos loggers)
- Teste `OrderService` com cada implementação

---

## 🟡 Exercício 4: Repository Pattern com DIP

Implemente Repository Pattern aplicando DIP.

**Requisitos:**
```csharp
public interface IRepository<T>
{
    void Add(T entity);
    T GetById(int id);
    IEnumerable<T> GetAll();
}

public class ProductService
{
    private readonly IRepository<Product> _repository;
    
    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }
}
```

**Implementações:**
- `MemoryRepository<T>`
- `FileRepository<T>`
- Troque implementação facilmente
- Service não muda!

---

## 🟡 Exercício 5: Refatorar Sistema de Pagamento

Refatore aplicando ISP e DIP.

**❌ Código Problemático:**
```csharp
public interface IPayment
{
    void ProcessCreditCard();
    void ProcessPix();
    void ProcessBoleto();
    void ProcessPayPal();
}

public class PaymentService
{
    private CreditCardProcessor _creditCard = new CreditCardProcessor();
    private PixProcessor _pix = new PixProcessor();
    // ...
}
```

**✅ Requisitos:**
- Segregue interfaces por método de pagamento
- Crie interface `IPaymentProcessor`
- Use DIP no `PaymentService`
- Adicione novo método de pagamento sem modificar código existente

---

## 🔴 Exercício 6: Sistema de Notificações Multi-canal

Sistema complexo com ISP e DIP.

**Requisitos:**
- Interfaces segregadas:
  - `INotifiable`: enviar notificação
  - `ISchedulable`: agendar notificação
  - `IPrioritizable`: definir prioridade
  - `ITrackable`: rastrear entrega
- Classes:
  - `Email`: todas as interfaces
  - `SMS`: `INotifiable`, `ITrackable`
  - `Push`: `INotifiable`, `IPrioritizable`
  - `WhatsApp`: todas menos `ISchedulable`
- `NotificationService` com DIP
- Teste múltiplos canais

---

## 🔴 Exercício 7: Sistema de Cache com Strategy e DIP

Implemente cache com diferentes estratégias.

**Requisitos:**
```csharp
public interface ICacheStrategy
{
    void Set(string key, object value);
    object Get(string key);
    void Remove(string key);
}

public interface IExpirationPolicy
{
    bool IsExpired(DateTime createdAt);
}

public class CacheService
{
    private readonly ICacheStrategy _strategy;
    private readonly IExpirationPolicy _policy;
    
    public CacheService(ICacheStrategy strategy, IExpirationPolicy policy)
    {
        _strategy = strategy;
        _policy = policy;
    }
}
```

**Implementações:**
- `MemoryCacheStrategy`
- `RedisCacheStrategy` (simule)
- `FileCache Strategy`
- Políticas: `TimeBasedExpiration`, `SlidingExpiration`, `NeverExpire`

---

## 🔴 Exercício 8: Sistema de Relatórios com Open/Closed

Sistema que segue OCP, ISP e DIP.

**Requisitos:**
```csharp
public interface IReportGenerator
{
    string Generate(IEnumerable<object> data);
}

public interface IReportExporter
{
    void Export(string content, string filename);
}

public class ReportService
{
    private readonly IReportGenerator _generator;
    private readonly IReportExporter _exporter;
    
    public ReportService(IReportGenerator generator, IReportExporter exporter)
    {
        _generator = generator;
        _exporter = exporter;
    }
    
    public void CreateReport(IEnumerable<object> data, string filename)
    {
        var content = _generator.Generate(data);
        _exporter.Export(content, filename);
    }
}
```

**Implementações:**
- Generators: `PdfGenerator`, `ExcelGenerator`, `CsvGenerator`, `HtmlGenerator`
- Exporters: `FileExporter`, `EmailExporter`, `CloudExporter`
- Adicione novos sem modificar `ReportService`

---

## 🔴 Exercício 9: Sistema de Autenticação com DIP

Sistema completo de auth com múltiplos provedores.

**Requisitos:**
```csharp
public interface IAuthenticationProvider
{
    Task<AuthResult> Authenticate(string username, string password);
}

public interface ITokenGenerator
{
    string GenerateToken(User user);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface IUserRepository
{
    Task<User> GetByUsername(string username);
}

public class AuthService
{
    private readonly IAuthenticationProvider _authProvider;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _hasher;
    private readonly IUserRepository _repository;
    
    // Constructor injection de todas as dependências
}
```

**Implementações:**
- `DatabaseAuthProvider`, `LdapAuthProvider`, `OAuth2Provider`
- `JwtTokenGenerator`, `SimpleTokenGenerator`
- `Bcrypt Hasher`, `SHA256Hasher`
- Teste diferentes combinações

---

## 🔴 Exercício 10: E-commerce com SOLID Completo (PROJETO FINAL)

Refatore um e-commerce legado aplicando TODOS os princípios SOLID.

### Código Legado (Problemático):

```csharp
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Validação
        if (string.IsNullOrEmpty(order.CustomerName))
            throw new Exception("Nome inválido");
        
        // Calcular total
        decimal total = 0;
        foreach (var item in order.Items)
        {
            total += item.Price * item.Quantity;
            if (item.HasDiscount)
                total -= total * 0.1m;
        }
        
        // Processar pagamento
        if (order.PaymentMethod == "CreditCard")
        {
            // Lógica cartão de crédito
            Console.WriteLine("Processando cartão...");
        }
        else if (order.PaymentMethod == "Pix")
        {
            // Lógica PIX
            Console.WriteLine("Processando PIX...");
        }
        
        // Calcular frete
        if (order.CEP.StartsWith("01"))
            order.Freight = 10;
        else
            order.Freight = 20;
        
        // Enviar email
        Console.WriteLine($"Enviando email para {order.CustomerEmail}");
        
        // Salvar no banco
        using (var connection = new SqlConnection("..."))
        {
            // SQL direto
        }
        
        // Gerar nota fiscal
        Console.WriteLine("Gerando NF...");
        
        // Atualizar estoque
        Console.WriteLine("Atualizando estoque...");
    }
}
```

### Refatoração com SOLID:

#### 1. Single Responsibility Principle
Separe responsabilidades em serviços:
- `IOrderValidator`
- `IPriceCalculator`
- `IPaymentProcessor`
- `IFreightCalculator`
- `INotificationService`
- `IOrderRepository`
- `IInvoiceGenerator`
- `IStockManager`

#### 2. Open/Closed Principle
Sistema extensível sem modificação:
```csharp
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal price);
}

// Adicione novos descontos sem modificar código existente
public class PercentageDiscount : IDiscountStrategy { }
public class FixedDiscount : IDiscountStrategy { }
public class ProgressiveDiscount : IDiscountStrategy { }
```

#### 3. Liskov Substitution Principle
```csharp
public abstract class PaymentProcessor
{
    public abstract bool Process(decimal amount);
    
    protected bool ValidateAmount(decimal amount)
    {
        return amount > 0;
    }
}

// Todas as implementações podem substituir a base
public class CreditCardProcessor : PaymentProcessor { }
public class PixProcessor : PaymentProcessor { }
```

#### 4. Interface Segregation Principle
Interfaces específicas:
```csharp
public interface IValidatable
{
    bool Validate();
}

public interface ICalculable
{
    decimal Calculate();
}

public interface IPersistable
{
    void Save();
}

// Implemente apenas o necessário
public class Order : IValidatable, ICalculable, IPersistable
{
    // ...
}
```

#### 5. Dependency Inversion Principle
```csharp
public class OrderService
{
    private readonly IOrderValidator _validator;
    private readonly IPriceCalculator _calculator;
    private readonly IPaymentProcessor _payment;
    private readonly IFreightCalculator _freight;
    private readonly INotificationService _notification;
    private readonly IOrderRepository _repository;
    private readonly IInvoiceGenerator _invoice;
    private readonly IStockManager _stock;
    
    public OrderService(
        IOrderValidator validator,
        IPriceCalculator calculator,
        IPaymentProcessor payment,
        IFreightCalculator freight,
        INotificationService notification,
        IOrderRepository repository,
        IInvoiceGenerator invoice,
        IStockManager stock)
    {
        _validator = validator;
        _calculator = calculator;
        _payment = payment;
        _freight = freight;
        _notification = notification;
        _repository = repository;
        _invoice = invoice;
        _stock = stock;
    }
    
    public async Task ProcessOrder(Order order)
    {
        if (!_validator.Validate(order))
            throw new InvalidOrderException();
        
        order.Total = _calculator.Calculate(order);
        order.Freight = _freight.Calculate(order.CEP, order.Weight);
        
        await _payment.Process(order.Total);
        await _repository.Save(order);
        await _invoice.Generate(order);
        await _stock.Update(order.Items);
        await _notification.SendConfirmation(order.CustomerEmail);
    }
}
```

### Testes Obrigatórios:

```csharp
// 1. Configurar dependências
IOrderValidator validator = new OrderValidator();
IPriceCalculator calculator = new PriceCalculator(new PercentageDiscount());
IPaymentProcessor payment = new CreditCardProcessor();
IFreightCalculator freight = new CorreiosFreightCalculator();
INotificationService notification = new EmailNotificationService();
IOrderRepository repository = new MemoryOrderRepository();
IInvoiceGenerator invoice = new PDFInvoiceGenerator();
IStockManager stock = new StockManager(new MemoryStockRepository());

// 2. Criar serviço com injeção de dependência
var orderService = new OrderService(
    validator,
    calculator,
    payment,
    freight,
    notification,
    repository,
    invoice,
    stock
);

// 3. Processar pedido
await orderService.ProcessOrder(order);

// 4. Trocar implementações facilmente
IPaymentProcessor pixPayment = new PixProcessor();
var orderService2 = new OrderService(
    validator,
    calculator,
    pixPayment, // ← Mudou apenas o processador de pagamento
    freight,
    notification,
    repository,
    invoice,
    stock
);
```

### Estrutura Esperada:
- 15+ interfaces
- 8+ serviços
- 20+ classes
- 600-800 linhas
- SOLID completo aplicado
- Testável e extensível
- Baixo acoplamento
- Alta coesão

---

## 📚 Dicas

- ISP: Interfaces pequenas e focadas
- DIP: Sempre dependa de abstrações
- Use constructor injection (mais testável)
- Composição sobre herança
- Single Responsibility sempre

---

## 🎯 Critérios de Avaliação

- ✅ ISP aplicado corretamente
- ✅ DIP em todas as dependências
- ✅ Código testável
- ✅ Baixo acoplamento
- ✅ Fácil manutenção
- ✅ Extensível sem modificações

Boa sorte! 🚀
