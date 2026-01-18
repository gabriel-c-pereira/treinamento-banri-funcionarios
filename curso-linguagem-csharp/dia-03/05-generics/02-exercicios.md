# 🎯 Exercícios - Tipos Genéricos (Generics)

> **Tempo estimado**: 1 hora  
> **Dificuldade**: Intermediário/Avançado

---

## 📝 Instruções Gerais

- Use generics para criar código reutilizável
- Aplique constraints quando necessário
- Teste com diferentes tipos
- Demonstre type safety

---

## 🟢 Exercício 1: Lista Genérica Simples

Crie sua própria implementação de lista genérica.

**Requisitos:**
```csharp
public class MinhaLista<T>
{
    private T[] _items;
    private int _count;
    
    public void Add(T item) { }
    public T Get(int index) { }
    public int Count => _count;
    public bool Contains(T item) { }
    public void Remove(T item) { }
}
```

**Testes:**
```csharp
var numeros = new MinhaLista<int>();
numeros.Add(10);
numeros.Add(20);

var nomes = new MinhaLista<string>();
nomes.Add("João");
nomes.Add("Maria");
```

---

## 🟢 Exercício 2: Par Genérico

Crie uma classe genérica que armazena um par de valores.

**Requisitos:**
```csharp
public class Par<T1, T2>
{
    public T1 Primeiro { get; set; }
    public T2 Segundo { get; set; }
    
    public void Swap() // Troca valores
    {
        // Como fazer se tipos são diferentes?
        // Retorne novo Par<T2, T1>
    }
}
```

**Testes:**
```csharp
var par1 = new Par<int, string> { Primeiro = 1, Segundo = "Um" };
var par2 = new Par<string, double> { Primeiro = "PI", Segundo = 3.14 };
```

---

## 🟡 Exercício 3: Pilha Genérica (Stack)

Implemente uma pilha genérica (LIFO).

**Requisitos:**
```csharp
public class Pilha<T>
{
    public void Push(T item) { }
    public T Pop() { }
    public T Peek() { }
    public bool IsEmpty => /* ... */;
    public int Count { get; }
}
```

**Testes:**
```csharp
var pilha = new Pilha<int>();
pilha.Push(1);
pilha.Push(2);
pilha.Push(3);
Console.WriteLine(pilha.Pop()); // 3
Console.WriteLine(pilha.Pop()); // 2
```

---

## 🟡 Exercício 4: Métodos Genéricos Utilitários

Crie classe com métodos genéricos úteis.

**Requisitos:**
```csharp
public static class Utils
{
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
    
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }
    
    public static T[] CreateArray<T>(int size, T defaultValue)
    {
        // Criar array preenchido com defaultValue
    }
    
    public static List<T> Filter<T>(List<T> list, Func<T, bool> predicate)
    {
        // Filtrar lista baseado em predicate
    }
}
```

---

## 🟡 Exercício 5: Repository Genérico

Implemente Repository Pattern com generics e constraints.

**Requisitos:**
```csharp
public interface IEntity
{
    int Id { get; set; }
}

public class Repository<T> where T : class, IEntity, new()
{
    private List<T> _items = new();
    
    public void Add(T entity)
    {
        if (entity.Id == 0)
            entity.Id = GenerateId();
        _items.Add(entity);
    }
    
    public T GetById(int id) { }
    public IEnumerable<T> GetAll() { }
    public void Update(T entity) { }
    public void Delete(int id) { }
}
```

**Classes de teste:**
```csharp
public class Cliente : IEntity
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

public class Produto : IEntity
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}
```

---

## 🟡 Exercício 6: Cache Genérico

Sistema de cache genérico com tempo de expiração.

**Requisitos:**
```csharp
public class Cache<TKey, TValue>
{
    private class CacheItem
    {
        public TValue Value { get; set; }
        public DateTime Expiration { get; set; }
    }
    
    private Dictionary<TKey, CacheItem> _cache = new();
    
    public void Set(TKey key, TValue value, TimeSpan expiration) { }
    public TValue Get(TKey key) { }
    public bool TryGet(TKey key, out TValue value) { }
    public void Remove(TKey key) { }
    public void Clear() { }
    public int Count => _cache.Count;
}
```

**Testes:**
```csharp
var cache = new Cache<string, Usuario>();
cache.Set("user1", usuario, TimeSpan.FromMinutes(5));
var user = cache.Get("user1");
```

---

## 🔴 Exercício 7: Árvore Binária Genérica

Implemente árvore binária de busca genérica.

**Requisitos:**
```csharp
public class Node<T> where T : IComparable<T>
{
    public T Value { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }
}

public class BinaryTree<T> where T : IComparable<T>
{
    private Node<T> _root;
    
    public void Insert(T value) { }
    public bool Contains(T value) { }
    public void Remove(T value) { }
    public IEnumerable<T> InOrder() { } // Travessia em ordem
    public IEnumerable<T> PreOrder() { }
    public IEnumerable<T> PostOrder() { }
    public int Height() { }
}
```

**Testes:**
```csharp
var tree = new BinaryTree<int>();
tree.Insert(50);
tree.Insert(30);
tree.Insert(70);
tree.Insert(20);
tree.Insert(40);

foreach (var value in tree.InOrder())
    Console.WriteLine(value); // 20, 30, 40, 50, 70
```

---

## 🔴 Exercício 8: Event Bus Genérico

Sistema de eventos genérico type-safe.

**Requisitos:**
```csharp
public interface IEvent { }

public class EventBus
{
    private Dictionary<Type, List<object>> _handlers = new();
    
    public void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        // Registrar handler para evento tipo T
    }
    
    public void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        // Remover handler
    }
    
    public void Publish<T>(T @event) where T : IEvent
    {
        // Chamar todos os handlers registrados para T
    }
}
```

**Eventos de exemplo:**
```csharp
public class UserRegisteredEvent : IEvent
{
    public string Username { get; set; }
    public DateTime RegisteredAt { get; set; }
}

public class OrderPlacedEvent : IEvent
{
    public int OrderId { get; set; }
    public decimal Total { get; set; }
}
```

**Testes:**
```csharp
var bus = new EventBus();

bus.Subscribe<UserRegisteredEvent>(e => 
    Console.WriteLine($"Usuário {e.Username} registrado!"));

bus.Subscribe<OrderPlacedEvent>(e => 
    Console.WriteLine($"Pedido {e.OrderId}: {e.Total:C}"));

bus.Publish(new UserRegisteredEvent { Username = "João" });
bus.Publish(new OrderPlacedEvent { OrderId = 1, Total = 100 });
```

---

## 🔴 Exercício 9: Query Builder Genérico

Construtor de queries genérico type-safe.

**Requisitos:**
```csharp
public class Query<T> where T : class
{
    private List<Func<T, bool>> _filters = new();
    private Func<T, object> _orderBy;
    private bool _ascending = true;
    private int? _take;
    private int? _skip;
    
    public Query<T> Where(Func<T, bool> predicate)
    {
        _filters.Add(predicate);
        return this;
    }
    
    public Query<T> OrderBy<TKey>(Func<T, TKey> keySelector)
    {
        _orderBy = x => keySelector(x);
        _ascending = true;
        return this;
    }
    
    public Query<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
    {
        _orderBy = x => keySelector(x);
        _ascending = false;
        return this;
    }
    
    public Query<T> Take(int count)
    {
        _take = count;
        return this;
    }
    
    public Query<T> Skip(int count)
    {
        _skip = count;
        return this;
    }
    
    public IEnumerable<T> Execute(IEnumerable<T> source)
    {
        // Aplicar filtros, ordenação, skip, take
    }
}
```

**Testes:**
```csharp
var produtos = new List<Produto> { /* ... */ };

var query = new Query<Produto>()
    .Where(p => p.Preco > 100)
    .Where(p => p.Nome.Contains("Mouse"))
    .OrderBy(p => p.Preco)
    .Skip(10)
    .Take(5);

var resultado = query.Execute(produtos);
```

---

## 🔴 Exercício 10: Sistema de Validação Genérico (PROJETO FINAL)

Sistema completo de validação usando generics e fluent API.

### Requisitos:

#### 1. Interfaces Base
```csharp
public interface IValidator<T>
{
    ValidationResult Validate(T instance);
}

public interface IValidationRule<T>
{
    bool IsValid(T value);
    string ErrorMessage { get; }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

#### 2. Validator Base
```csharp
public abstract class Validator<T> : IValidator<T>
{
    protected List<IValidationRule<T>> Rules = new();
    
    public ValidationResult Validate(T instance)
    {
        var result = new ValidationResult { IsValid = true };
        
        foreach (var rule in Rules)
        {
            if (!rule.IsValid(instance))
            {
                result.IsValid = false;
                result.Errors.Add(rule.ErrorMessage);
            }
        }
        
        return result;
    }
    
    protected void AddRule(IValidationRule<T> rule)
    {
        Rules.Add(rule);
    }
}
```

#### 3. Fluent Validator
```csharp
public class FluentValidator<T> : Validator<T>
{
    public FluentValidator<T> RuleFor<TProp>(
        Func<T, TProp> propertySelector,
        Action<PropertyValidator<T, TProp>> configure)
    {
        var propertyValidator = new PropertyValidator<T, TProp>(propertySelector);
        configure(propertyValidator);
        
        foreach (var rule in propertyValidator.GetRules())
            AddRule(rule);
        
        return this;
    }
}

public class PropertyValidator<T, TProp>
{
    private Func<T, TProp> _selector;
    private List<IValidationRule<T>> _rules = new();
    
    public PropertyValidator(Func<T, TProp> selector)
    {
        _selector = selector;
    }
    
    public PropertyValidator<T, TProp> NotNull(string message = "Não pode ser nulo")
    {
        _rules.Add(new NotNullRule<T, TProp>(_selector, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> NotEmpty(string message = "Não pode ser vazio")
    {
        _rules.Add(new NotEmptyRule<T, TProp>(_selector, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> MinLength(int min, string message = null)
    {
        _rules.Add(new MinLengthRule<T, TProp>(_selector, min, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> MaxLength(int max, string message = null)
    {
        _rules.Add(new MaxLengthRule<T, TProp>(_selector, max, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> Range<TValue>(TValue min, TValue max, string message = null)
        where TValue : IComparable<TValue>
    {
        _rules.Add(new RangeRule<T, TProp, TValue>(_selector, min, max, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> Must(Func<TProp, bool> predicate, string message)
    {
        _rules.Add(new CustomRule<T, TProp>(_selector, predicate, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> Email(string message = "Email inválido")
    {
        _rules.Add(new EmailRule<T, TProp>(_selector, message));
        return this;
    }
    
    public PropertyValidator<T, TProp> CreditCard(string message = "Cartão inválido")
    {
        _rules.Add(new CreditCardRule<T, TProp>(_selector, message));
        return this;
    }
    
    public IEnumerable<IValidationRule<T>> GetRules() => _rules;
}
```

#### 4. Regras Específicas

Implemente todas as regras:
- `NotNullRule<T, TProp>`
- `NotEmptyRule<T, TProp>`
- `MinLengthRule<T, TProp>`
- `MaxLengthRule<T, TProp>`
- `RangeRule<T, TProp, TValue>`
- `CustomRule<T, TProp>`
- `EmailRule<T, TProp>`
- `CreditCardRule<T, TProp>`
- `RegexRule<T, TProp>`
- `UniqueRule<T, TProp>`

#### 5. Exemplo de Uso

```csharp
public class Cliente
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Idade { get; set; }
    public string Telefone { get; set; }
    public string CartaoCredito { get; set; }
}

public class ClienteValidator : FluentValidator<Cliente>
{
    public ClienteValidator()
    {
        RuleFor(c => c.Nome, prop => prop
            .NotNull()
            .NotEmpty()
            .MinLength(3, "Nome deve ter no mínimo 3 caracteres")
            .MaxLength(100, "Nome deve ter no máximo 100 caracteres"));
        
        RuleFor(c => c.Email, prop => prop
            .NotNull()
            .NotEmpty()
            .Email());
        
        RuleFor(c => c.Idade, prop => prop
            .Range(18, 120, "Idade deve estar entre 18 e 120 anos"));
        
        RuleFor(c => c.Telefone, prop => prop
            .Must(tel => tel?.Length == 11, "Telefone deve ter 11 dígitos"));
        
        RuleFor(c => c.CartaoCredito, prop => prop
            .CreditCard());
    }
}

// Uso
var cliente = new Cliente
{
    Nome = "Jo",
    Email = "invalido",
    Idade = 15,
    Telefone = "123",
    CartaoCredito = "1234"
};

var validator = new ClienteValidator();
var result = validator.Validate(cliente);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
        Console.WriteLine(error);
}
```

#### 6. Features Avançadas

**a) Validação Assíncrona:**
```csharp
public interface IAsyncValidator<T>
{
    Task<ValidationResult> ValidateAsync(T instance);
}
```

**b) Validação Condicional:**
```csharp
RuleFor(c => c.CPF, prop => prop.NotEmpty())
    .When(c => c.TipoPessoa == TipoPessoa.Fisica);
```

**c) Validação de Coleções:**
```csharp
RuleFor(p => p.Itens, prop => prop
    .NotNull()
    .Must(items => items.Any(), "Pedido deve ter pelo menos 1 item"));
```

**d) Validação Aninhada:**
```csharp
public class PedidoValidator : FluentValidator<Pedido>
{
    public PedidoValidator()
    {
        RuleFor(p => p.Cliente, prop => prop
            .SetValidator(new ClienteValidator()));
    }
}
```

#### 7. Testes Obrigatórios

```csharp
// 1. Validação de Cliente
var clienteValidator = new ClienteValidator();
var cliente = new Cliente { /* ... */ };
var result = clienteValidator.Validate(cliente);

// 2. Validação de Produto
var produtoValidator = new ProdutoValidator();
// ...

// 3. Validação de Pedido (com aninhamento)
var pedidoValidator = new PedidoValidator();
// ...

// 4. Validação customizada
RuleFor(x => x.Senha, prop => prop
    .Must(senha => senha.Length >= 8 && 
                   senha.Any(char.IsUpper) && 
                   senha.Any(char.IsDigit),
          "Senha deve ter 8+ caracteres, 1 maiúscula e 1 número"));

// 5. Validação com múltiplas regras
```

### Estrutura Esperada:
- 10+ regras de validação
- Fluent API completa
- Suporte a tipos genéricos
- 600-800 linhas de código
- Sistema extensível
- Type-safe
- Comentários explicativos

---

## 📚 Dicas

- Use constraints para restringir tipos
- Covariance (`out`) para leitura
- Contravariance (`in`) para escrita
- Generics evitam boxing/unboxing
- Type safety em tempo de compilação

---

## 🎯 Critérios de Avaliação

- ✅ Uso correto de generics
- ✅ Constraints apropriados
- ✅ Type safety
- ✅ Código reutilizável
- ✅ Performance
- ✅ API intuitiva

Boa sorte! 🚀
