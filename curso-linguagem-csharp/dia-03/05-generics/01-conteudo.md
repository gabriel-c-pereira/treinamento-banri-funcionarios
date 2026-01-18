# 🧬 Tipos Genéricos (Generics)

> **Tempo estimado**: 1 hora  
> **Nível**: Intermediário/Avançado

## 🎯 O que são Generics?

**Generics** permitem criar classes, métodos e interfaces **reutilizáveis** que funcionam com **qualquer tipo**, mantendo **type safety**.

### Problema Sem Generics

```csharp
// Sem generics: precisa criar uma classe para cada tipo
public class IntList
{
    private int[] items = new int[10];
    public void Add(int item) { }
}

public class StringList
{
    private string[] items = new string[10];
    public void Add(string item) { }
}

// Ou usar object (perde type safety)
public class ObjectList
{
    private object[] items = new object[10];
    public void Add(object item) { }
    
    // ❌ Problema: boxing/unboxing e sem type safety
    ObjectList list = new();
    list.Add(42);
    int number = (int)list.Get(0); // Cast necessário
}
```

### Solução Com Generics

```csharp
// Uma classe para TODOS os tipos!
public class GenericList<T>
{
    private T[] items = new T[10];
    
    public void Add(T item) { }
    public T Get(int index) => items[index];
}

// Uso
GenericList<int> numbers = new();
numbers.Add(42); // Type safe!

GenericList<string> names = new();
names.Add("João"); // Type safe!
```

---

## 📦 Classes Genéricas

```csharp
public class Repository<T>
{
    private List<T> _items = new();
    
    public void Add(T item) => _items.Add(item);
    
    public T GetById(int id) => _items[id];
    
    public List<T> GetAll() => _items;
}

// Uso
var clienteRepo = new Repository<Cliente>();
clienteRepo.Add(new Cliente { Nome = "João" });

var produtoRepo = new Repository<Produto>();
produtoRepo.Add(new Produto { Nome = "Mouse" });
```

---

## 🔧 Métodos Genéricos

```csharp
public class Utils
{
    // Método genérico
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
}

// Uso
int x = 10, y = 20;
Utils.Swap(ref x, ref y); // Inferência de tipo
Console.WriteLine($"x={x}, y={y}"); // x=20, y=10

string max = Utils.Max("abc", "xyz"); // "xyz"
```

---

## 🔒 Constraints (Restrições)

Constraints limitam os tipos que podem ser usados com generics.

### Tipos de Constraints

```csharp
// where T : class - T deve ser reference type
public class RefTypeRepo<T> where T : class { }

// where T : struct - T deve ser value type
public class ValueTypeRepo<T> where T : struct { }

// where T : new() - T deve ter construtor sem parâmetros
public class Factory<T> where T : new()
{
    public T Create() => new T();
}

// where T : BaseClass - T deve herdar de BaseClass
public class AnimalRepo<T> where T : Animal { }

// where T : IInterface - T deve implementar IInterface
public class SerializableRepo<T> where T : ISerializable { }

// Múltiplas constraints
public class ComplexRepo<T> where T : class, IDisposable, new()
{
    public T Create()
    {
        return new T();
    }
    
    public void Cleanup(T item)
    {
        item.Dispose(); // Garantido por IDisposable
    }
}
```

### Exemplos Práticos

```csharp
// Repository genérico com constraint
public class Repository<T> where T : class, IEntity
{
    private List<T> _items = new();
    
    public void Add(T item)
    {
        if (item.Id == 0) // IEntity garante que tem Id
            item.Id = GenerateId();
        _items.Add(item);
    }
    
    public T GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }
}

public interface IEntity
{
    int Id { get; set; }
}

public class Cliente : IEntity
{
    public int Id { get; set; }
    public string Nome { get; set; }
}
```

---

## 🔄 Covariance e Contravariance

### Covariance (`out`) - Apenas Leitura

```csharp
// out: pode retornar T, mas não pode receber T como parâmetro
public interface IProducer<out T>
{
    T Get(); // ✅ Pode retornar
    // void Set(T item); // ❌ Não pode receber
}

// Uso
IProducer<Animal> animalProducer = new Producer<Cachorro>(); // ✅ OK
```

### Contravariance (`in`) - Apenas Escrita

```csharp
// in: pode receber T como parâmetro, mas não pode retornar T
public interface IConsumer<in T>
{
    void Process(T item); // ✅ Pode receber
    // T Get(); // ❌ Não pode retornar
}

// Uso
IConsumer<Cachorro> cachorroConsumer = new Consumer<Animal>(); // ✅ OK
```

---

## 📊 Exemplo Completo: Sistema de Cache Genérico

```csharp
public interface ICacheService<T>
{
    void Add(string key, T value);
    T Get(string key);
    bool Exists(string key);
    void Remove(string key);
}

public class MemoryCache<T> : ICacheService<T>
{
    private Dictionary<string, T> _cache = new();
    
    public void Add(string key, T value)
    {
        _cache[key] = value;
    }
    
    public T Get(string key)
    {
        return _cache.ContainsKey(key) ? _cache[key] : default;
    }
    
    public bool Exists(string key)
    {
        return _cache.ContainsKey(key);
    }
    
    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}

// Uso
var clienteCache = new MemoryCache<Cliente>();
clienteCache.Add("cliente1", new Cliente { Nome = "João" });

var produtoCache = new MemoryCache<Produto>();
produtoCache.Add("prod1", new Produto { Nome = "Mouse" });
```

---

## 🎯 Benefícios dos Generics

✅ **Type Safety**: Erros em tempo de compilação  
✅ **Reutilização**: Um código para múltiplos tipos  
✅ **Performance**: Sem boxing/unboxing  
✅ **Legibilidade**: Código mais claro  

---

## 🎓 Resumo

✅ **Generics** permitem código reutilizável e type-safe  
✅ **Constraints** limitam tipos aceitos  
✅ **Covariance** (`out`) para saída  
✅ **Contravariance** (`in`) para entrada  
✅ Usado extensivamente no .NET (`List<T>`, `Dictionary<TKey, TValue>`)  

---

**Parabéns! Você completou o Dia 03! 🎉**

➡️ **Próximo Dia**: Coleções, Listas e LINQ
