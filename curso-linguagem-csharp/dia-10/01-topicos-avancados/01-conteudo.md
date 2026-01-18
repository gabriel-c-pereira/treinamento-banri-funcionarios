# Tópicos Avançados

## Generics Avançados

### Constraints
```csharp
// Constraints básicas
public class Repository<T> where T : class, new()
{
    // T deve ser uma classe e ter construtor padrão
}

public class Calculator<T> where T : struct, IComparable<T>
{
    // T deve ser um struct e implementar IComparable<T>
}

// Constraints múltiplas
public class Service<T, TKey> where T : Entity<TKey>
                                where TKey : IEquatable<TKey>
{
    // T herda de Entity<TKey> e TKey implementa IEquatable<TKey>
}
```

### Generic Methods
```csharp
public static class CollectionExtensions
{
    public static T FindMax<T>(this IEnumerable<T> source) where T : IComparable<T>
    {
        if (!source.Any()) throw new InvalidOperationException();

        T max = source.First();
        foreach (T item in source.Skip(1))
        {
            if (item.CompareTo(max) > 0)
                max = item;
        }
        return max;
    }

    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (T item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }
}
```

### Covariância e Contravariância
```csharp
// Covariância (out) - permite retorno mais específico
public interface IRepository<out T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
}

// Contravariância (in) - permite parâmetro mais genérico
public interface IComparer<in T>
{
    int Compare(T x, T y);
}

// Invariância - nem covariante nem contravariante
public interface IList<T>
{
    T this[int index] { get; set; }
    void Add(T item);
}
```

## Reflection

### Inspeção de Tipos
```csharp
public class TypeInspector
{
    public void InspectType(Type type)
    {
        Console.WriteLine($"Type: {type.Name}");
        Console.WriteLine($"Namespace: {type.Namespace}");
        Console.WriteLine($"Assembly: {type.Assembly.FullName}");

        // Propriedades
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            Console.WriteLine($"Property: {prop.Name} ({prop.PropertyType.Name})");
        }

        // Métodos
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var method in methods)
        {
            Console.WriteLine($"Method: {method.Name}");
        }
    }
}
```

### Criação Dinâmica de Objetos
```csharp
public class ObjectFactory
{
    public T CreateInstance<T>() where T : new()
    {
        return new T();
    }

    public object CreateInstance(Type type)
    {
        return Activator.CreateInstance(type);
    }

    public T CreateInstance<T>(params object[] args)
    {
        return (T)Activator.CreateInstance(typeof(T), args);
    }
}
```

### Invocação Dinâmica de Métodos
```csharp
public class MethodInvoker
{
    public object InvokeMethod(object target, string methodName, params object[] parameters)
    {
        var type = target.GetType();
        var method = type.GetMethod(methodName);

        if (method == null)
            throw new MissingMethodException($"Method {methodName} not found on type {type.Name}");

        return method.Invoke(target, parameters);
    }

    public async Task<object> InvokeAsyncMethod(object target, string methodName, params object[] parameters)
    {
        var type = target.GetType();
        var method = type.GetMethod(methodName);

        if (method == null)
            throw new MissingMethodException($"Method {methodName} not found on type {type.Name}");

        var result = method.Invoke(target, parameters);

        if (result is Task task)
        {
            await task;
            return task.GetType().GetProperty("Result")?.GetValue(task);
        }

        return result;
    }
}
```

## Dynamic

### ExpandoObject
```csharp
public class DynamicObjectExample
{
    public dynamic CreateDynamicPerson()
    {
        dynamic person = new ExpandoObject();
        person.Name = "João";
        person.Age = 30;
        person.Email = "joao@email.com";

        // Adicionar método dinamicamente
        person.Greet = (Func<string>)(() => $"Olá, meu nome é {person.Name}");

        return person;
    }

    public void ProcessDynamicObject(dynamic obj)
    {
        // Acesso dinâmico a propriedades
        Console.WriteLine($"Nome: {obj.Name}");
        Console.WriteLine($"Idade: {obj.Age}");

        // Chamada dinâmica de método
        if (obj.Greet != null)
        {
            Console.WriteLine(obj.Greet());
        }
    }
}
```

### Dynamic com DLR
```csharp
public class DynamicCalculator
{
    public dynamic Add(dynamic a, dynamic b)
    {
        return a + b;
    }

    public dynamic Multiply(dynamic a, dynamic b)
    {
        return a * b;
    }

    public void ProcessNumbers(dynamic numbers)
    {
        foreach (dynamic number in numbers)
        {
            Console.WriteLine($"Número: {number}, Tipo: {number.GetType().Name}");
        }
    }
}
```

## Expression Trees

### Construção Básica
```csharp
public class ExpressionBuilder
{
    public Expression<Func<int, int, int>> BuildAddExpression()
    {
        // Parâmetros
        var paramA = Expression.Parameter(typeof(int), "a");
        var paramB = Expression.Parameter(typeof(int), "b");

        // Corpo: a + b
        var body = Expression.Add(paramA, paramB);

        // Criar lambda
        return Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
    }

    public Expression<Func<T, bool>> BuildEqualExpression<T, TValue>(string propertyName, TValue value)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(param, propertyName);
        var constant = Expression.Constant(value);
        var body = Expression.Equal(property, constant);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
```

### Compilação e Execução
```csharp
public class ExpressionExecutor
{
    public Func<int, int, int> CompileAddFunction()
    {
        var expression = BuildAddExpression();
        return expression.Compile();
    }

    public IEnumerable<T> Filter<T>(IEnumerable<T> source, Expression<Func<T, bool>> predicate)
    {
        var compiledPredicate = predicate.Compile();
        return source.Where(compiledPredicate);
    }

    private Expression<Func<int, int, int>> BuildAddExpression()
    {
        var paramA = Expression.Parameter(typeof(int), "a");
        var paramB = Expression.Parameter(typeof(int), "b");
        var body = Expression.Add(paramA, paramB);
        return Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
    }
}
```

### LINQ Provider Customizado
```csharp
public class CustomQueryable<T> : IQueryable<T>
{
    public CustomQueryable(Expression expression)
    {
        Expression = expression;
        Provider = new CustomQueryProvider();
    }

    public Type ElementType => typeof(T);
    public Expression Expression { get; }
    public IQueryProvider Provider { get; }
    public IEnumerator<T> GetEnumerator() => Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class CustomQueryProvider : IQueryProvider
{
    public IQueryable CreateQuery(Expression expression)
    {
        var elementType = expression.Type.GetGenericArguments()[0];
        var queryableType = typeof(CustomQueryable<>).MakeGenericType(elementType);
        return (IQueryable)Activator.CreateInstance(queryableType, expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new CustomQueryable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        // Implementar execução da expressão
        return ExecuteCore(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        var result = ExecuteCore(expression);
        return (TResult)result;
    }

    private object ExecuteCore(Expression expression)
    {
        // Lógica de execução customizada
        // Por exemplo, converter para SQL ou outra linguagem
        Console.WriteLine($"Executando expressão: {expression}");
        return new List<object>(); // Placeholder
    }
}
```

## Attributes Customizados

### Validation Attribute
```csharp
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        var cpf = value.ToString();
        if (!IsValidCpf(cpf))
        {
            return new ValidationResult("CPF inválido");
        }

        return ValidationResult.Success;
    }

    private bool IsValidCpf(string cpf)
    {
        // Implementação da validação de CPF
        // (lógica simplificada)
        return cpf.Length == 11 && cpf.All(char.IsDigit);
    }
}

public class Cliente
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Cpf]
    public string CPF { get; set; }
}
```

### Authorization Attribute
```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresPermissionAttribute : Attribute
{
    public string Permission { get; }

    public RequiresPermissionAttribute(string permission)
    {
        Permission = permission;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresRoleAttribute : Attribute
{
    public string[] Roles { get; }

    public RequiresRoleAttribute(params string[] roles)
    {
        Roles = roles;
    }
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = context.User;
        var endpoint = context.Resource as HttpContext;

        if (endpoint != null)
        {
            var actionDescriptor = endpoint.GetEndpoint()?.Metadata
                .GetMetadata<ControllerActionDescriptor>();

            if (actionDescriptor != null)
            {
                var permissionAttribute = actionDescriptor.MethodInfo
                    .GetCustomAttribute<RequiresPermissionAttribute>();

                if (permissionAttribute != null)
                {
                    // Verificar se usuário tem a permissão
                    if (user.HasClaim("permission", permissionAttribute.Permission))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }

        context.Fail();
    }
}
```
