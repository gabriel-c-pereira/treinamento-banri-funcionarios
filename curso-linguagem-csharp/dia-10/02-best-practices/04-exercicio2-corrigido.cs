// Base classes
public abstract class Entity<TId> where TId : IEquatable<TId>
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode();
}

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}

// Value Objects
public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value.ToLower().Trim();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        Currency = currency ?? "BRL";
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Multiply(int factor)
    {
        return new Money(Amount * factor, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Amount:N2}";
}

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    public Address(string street, string city, string state, string zipCode, string country = "Brazil")
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Country = country ?? "Brazil";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
    }

    public override string ToString() => $"{Street}, {City} - {State}, {ZipCode}, {Country}";
}

// Entity
public class CustomerId : ValueObject
{
    public Guid Value { get; }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty", nameof(value));

        Value = value;
    }

    public static CustomerId New() => new CustomerId(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}

public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Money CreditLimit { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Customer(CustomerId id, string name, Email email) : base(id)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        CreditLimit = new Money(0);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Customer Create(string name, Email email)
    {
        var id = CustomerId.New();
        return new Customer(id, name, email);
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));

        Name = newName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCreditLimit(Money creditLimit)
    {
        if (creditLimit.Amount < 0)
            throw new ArgumentException("Credit limit cannot be negative");

        CreditLimit = creditLimit;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

// Domain Service
public interface ICustomerService
{
    Task<Customer> RegisterCustomerAsync(string name, string email);
    Task<bool> CanCustomerHaveCreditLimitAsync(CustomerId customerId, Money requestedLimit);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> RegisterCustomerAsync(string name, string email)
    {
        // Regra de negócio: Email deve ser único
        var existingCustomer = await _customerRepository.GetByEmailAsync(email);
        if (existingCustomer != null)
            throw new CustomerAlreadyExistsException(email);

        var customer = Customer.Create(name, new Email(email));
        await _customerRepository.AddAsync(customer);

        return customer;
    }

    public async Task<bool> CanCustomerHaveCreditLimitAsync(CustomerId customerId, Money requestedLimit)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null || !customer.IsActive)
            return false;

        // Regra de negócio: Limite de crédito baseado no tempo de cadastro
        var monthsSinceRegistration = (DateTime.UtcNow - customer.CreatedAt).Days / 30;
        var maxAllowedLimit = new Money(Math.Min(monthsSinceRegistration * 1000, 50000));

        return requestedLimit.Amount <= maxAllowedLimit.Amount;
    }
}

// Repository Interface
public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(CustomerId id);
    Task<Customer> GetByEmailAsync(string email);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(CustomerId id);
}

// Exceptions
public class CustomerAlreadyExistsException : Exception
{
    public CustomerAlreadyExistsException(string email)
        : base($"Customer with email {email} already exists") { }
}

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(CustomerId customerId)
        : base($"Customer with ID {customerId} not found") { }
}