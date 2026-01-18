# 🏗️ SOLID: ISP e DIP

> **Tempo estimado**: 1 hora  
> **Nível**: Avançado

## 🎯 O que é SOLID?

**SOLID** são 5 princípios de design orientado a objetos criados por Robert C. Martin (Uncle Bob):

- **S** - Single Responsibility Principle
- **O** - Open/Closed Principle
- **L** - Liskov Substitution Principle
- **I** - Interface Segregation Principle
- **D** - Dependency Inversion Principle

---

## 🔹 ISP - Interface Segregation Principle

> **"Clientes não devem ser forçados a depender de interfaces que não usam"**

### ❌ Problema: "Fat Interface"

```csharp
// Interface com coisas demais - nem todos precisam de tudo
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

// Robô não come nem dorme!
public class Robot : IWorker
{
    public void Work() => Console.WriteLine("Trabalhando...");
    public void Eat() => throw new NotImplementedException(); // ❌
    public void Sleep() => throw new NotImplementedException(); // ❌
}
```

### ✅ Solução: Interfaces Pequenas e Específicas

```csharp
public interface IWorkable
{
    void Work();
}

public interface IFeedable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

// Humano implementa tudo
public class Human : IWorkable, IFeedable, ISleepable
{
    public void Work() => Console.WriteLine("Trabalhando...");
    public void Eat() => Console.WriteLine("Comendo...");
    public void Sleep() => Console.WriteLine("Dormindo...");
}

// Robô implementa apenas o que faz sentido
public class Robot : IWorkable
{
    public void Work() => Console.WriteLine("Trabalhando...");
}
```

### Benefícios do ISP

✅ **Flexibilidade**: Classes implementam apenas o necessário  
✅ **Manutenibilidade**: Mudanças em uma interface não afetam outras  
✅ **Clareza**: Interfaces pequenas são mais fáceis de entender  

---

## 🔸 DIP - Dependency Inversion Principle

> **"Dependa de abstrações, não de implementações concretas"**

### Regras do DIP

1. Módulos de alto nível não devem depender de módulos de baixo nível
2. Ambos devem depender de abstrações (interfaces)
3. Abstrações não devem depender de detalhes
4. Detalhes devem depender de abstrações

### ❌ Problema: Acoplamento Forte

```csharp
// Classe de baixo nível
public class EmailService
{
    public void SendEmail(string to, string message)
    {
        Console.WriteLine($"Email enviado para {to}: {message}");
    }
}

// Classe de alto nível DEPENDE da implementação concreta
public class UserService
{
    private EmailService _emailService = new EmailService(); // ❌ Acoplamento
    
    public void RegisterUser(string email)
    {
        // Registrar usuário...
        _emailService.SendEmail(email, "Bem-vindo!");
    }
}
```

**Problemas:**
- ❌ Difícil de testar (não pode mockar EmailService)
- ❌ Difícil trocar implementação (ex: SMS em vez de Email)
- ❌ Difícil manter (mudança no EmailService afeta UserService)

### ✅ Solução: Depender de Abstração

```csharp
// Abstração (interface)
public interface INotificationService
{
    void Send(string to, string message);
}

// Implementações concretas
public class EmailService : INotificationService
{
    public void Send(string to, string message)
    {
        Console.WriteLine($"Email para {to}: {message}");
    }
}

public class SmsService : INotificationService
{
    public void Send(string to, string message)
    {
        Console.WriteLine($"SMS para {to}: {message}");
    }
}

// Classe de alto nível depende da ABSTRAÇÃO
public class UserService
{
    private readonly INotificationService _notificationService;
    
    // Injeção de dependência via construtor
    public UserService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    public void RegisterUser(string contact)
    {
        // Registrar usuário...
        _notificationService.Send(contact, "Bem-vindo!");
    }
}

// Uso
INotificationService emailService = new EmailService();
UserService userService = new UserService(emailService);

// Fácil trocar implementação!
INotificationService smsService = new SmsService();
UserService userService2 = new UserService(smsService);
```

### Benefícios do DIP

✅ **Testabilidade**: Fácil criar mocks/stubs  
✅ **Flexibilidade**: Fácil trocar implementações  
✅ **Manutenibilidade**: Mudanças não propagam  
✅ **Reusabilidade**: Código mais genérico  

---

## 🔧 Injeção de Dependência (DI)

### 3 Formas de Injetar Dependências

#### 1. **Constructor Injection** (Recomendado)

```csharp
public class OrderService
{
    private readonly IPaymentService _paymentService;
    
    public OrderService(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
}
```

#### 2. **Property Injection**

```csharp
public class OrderService
{
    public IPaymentService PaymentService { get; set; }
}
```

#### 3. **Method Injection**

```csharp
public class OrderService
{
    public void ProcessOrder(Order order, IPaymentService paymentService)
    {
        paymentService.Process(order.Total);
    }
}
```

---

## 📊 Exemplo Completo: Sistema de Pagamentos

```csharp
// ===== INTERFACES (Abstrações) =====
public interface IPaymentGateway
{
    bool Process(decimal amount);
}

public interface ILogger
{
    void Log(string message);
}

// ===== IMPLEMENTAÇÕES =====
public class StripeGateway : IPaymentGateway
{
    public bool Process(decimal amount)
    {
        Console.WriteLine($"Processando {amount:C} via Stripe");
        return true;
    }
}

public class PayPalGateway : IPaymentGateway
{
    public bool Process(decimal amount)
    {
        Console.WriteLine($"Processando {amount:C} via PayPal");
        return true;
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

// ===== SERVIÇO DE ALTO NÍVEL =====
public class PaymentService
{
    private readonly IPaymentGateway _gateway;
    private readonly ILogger _logger;
    
    // Injeção de dependência
    public PaymentService(IPaymentGateway gateway, ILogger logger)
    {
        _gateway = gateway;
        _logger = logger;
    }
    
    public bool ProcessPayment(decimal amount)
    {
        _logger.Log($"Iniciando pagamento de {amount:C}");
        
        bool success = _gateway.Process(amount);
        
        if (success)
            _logger.Log("Pagamento aprovado");
        else
            _logger.Log("Pagamento recusado");
        
        return success;
    }
}

// ===== USO =====
// Fácil trocar implementações!
IPaymentGateway gateway = new StripeGateway();
// IPaymentGateway gateway = new PayPalGateway(); // ← Trocar facilmente
ILogger logger = new ConsoleLogger();

PaymentService service = new PaymentService(gateway, logger);
service.ProcessPayment(100.00m);
```

---

## 🎯 Resumo

✅ **ISP**: Interfaces pequenas e específicas  
✅ **DIP**: Depender de abstrações, não de implementações  
✅ **Injeção de Dependência**: Passar dependências de fora  
✅ **Benefícios**: Testável, flexível, manutenível  
✅ **Padrão**: Constructor injection é o mais usado  

➡️ **Próximo**: Tipos Genéricos (Generics)
