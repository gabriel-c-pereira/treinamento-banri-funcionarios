# 🎭 Classes Abstratas e Interfaces

> **Tempo estimado**: 2 horas  
> **Nível**: Intermediário/Avançado

## 🎯 Classes Abstratas

**Classe abstrata** é uma classe que **não pode ser instanciada** e serve como **base** para outras classes.

### Sintaxe

```csharp
public abstract class Animal
{
    // Método concreto (tem implementação)
    public void Dormir()
    {
        Console.WriteLine("Zzz...");
    }
    
    // Método abstrato (sem implementação)
    public abstract void EmitirSom();
}

public class Cachorro : Animal
{
    // OBRIGATÓRIO implementar métodos abstratos
    public override void EmitirSom()
    {
        Console.WriteLine("Au au!");
    }
}

// Uso
// Animal a = new Animal(); // ❌ ERRO: não pode instanciar
Animal a = new Cachorro();  // ✅ OK
a.EmitirSom(); // "Au au!"
a.Dormir();    // "Zzz..."
```

### Características

✅ Pode ter métodos **abstratos** (sem corpo) e **concretos** (com corpo)  
✅ Pode ter **campos** e **properties**  
✅ Pode ter **construtores**  
✅ Suporta **herança simples** (só uma classe base)  
❌ **Não pode ser instanciada** diretamente  

---

## 🔌 Interfaces

**Interface** é um **contrato** que define métodos e properties que uma classe deve implementar.

### Sintaxe

```csharp
public interface IPagamento
{
    // Métodos (sem implementação)
    bool ProcessarPagamento(decimal valor);
    bool Cancelar();
    
    // Properties (sem implementação)
    string NumeroTransacao { get; }
}

public class CartaoCredito : IPagamento
{
    public string NumeroTransacao { get; private set; }
    
    public bool ProcessarPagamento(decimal valor)
    {
        NumeroTransacao = Guid.NewGuid().ToString();
        Console.WriteLine($"Pagamento de {valor:C} processado!");
        return true;
    }
    
    public bool Cancelar()
    {
        Console.WriteLine("Pagamento cancelado");
        return true;
    }
}
```

### Características

✅ **Apenas contratos** (sem implementação) - exceto C# 8+  
✅ Suporta **múltiplas interfaces**  
✅ Todos os membros são públicos implicitamente  
✅ Pode ter **default methods** (C# 8+)  
❌ Não pode ter **campos**  
❌ Não pode ter **construtores**  

---

## 🆚 Classe Abstrata vs Interface

| Aspecto | Classe Abstrata | Interface |
|---------|----------------|-----------|
| **Instanciação** | ❌ Não | ❌ Não |
| **Métodos concretos** | ✅ Sim | ⚠️ Sim (C# 8+ default methods) |
| **Campos** | ✅ Sim | ❌ Não |
| **Construtores** | ✅ Sim | ❌ Não |
| **Herança múltipla** | ❌ Não (só uma base) | ✅ Sim (várias interfaces) |
| **Modificadores de acesso** | ✅ Sim | ❌ Não (sempre public) |
| **Properties** | ✅ Sim | ✅ Sim |

### Quando Usar Cada Um?

**USE Classe Abstrata quando:**
- Quer compartilhar **código** entre classes relacionadas
- Precisa de **campos** ou **construtores**
- Relação "é um" clara
- Classes têm comportamento comum

**USE Interface quando:**
- Quer definir um **contrato** sem implementação
- Precisa de **múltipla herança**
- Classes não relacionadas implementam o mesmo comportamento
- Quer mais flexibilidade

---

## 🔗 Múltiplas Interfaces

```csharp
public interface IVoavel
{
    void Voar();
}

public interface INadavel
{
    void Nadar();
}

public interface ICorrer
{
    void Correr();
}

// Classe pode implementar múltiplas interfaces!
public class Pato : IVoavel, INadavel, ICorrer
{
    public void Voar() => Console.WriteLine("Pato voando");
    public void Nadar() => Console.WriteLine("Pato nadando");
    public void Correr() => Console.WriteLine("Pato correndo");
}
```

---

## 🎯 Exemplo Completo: Sistema de Persistência

```csharp
// Interface genérica
public interface IRepository<T>
{
    void Add(T item);
    T GetById(int id);
    List<T> GetAll();
    void Update(T item);
    void Delete(int id);
}

// Implementação em memória
public class MemoryRepository<T> : IRepository<T>
{
    private List<T> _items = new();
    
    public void Add(T item) => _items.Add(item);
    public T GetById(int id) => _items[id];
    public List<T> GetAll() => _items;
    public void Update(T item) { /* implementação */ }
    public void Delete(int id) => _items.RemoveAt(id);
}

// Uso polimórfico
IRepository<Cliente> repo = new MemoryRepository<Cliente>();
repo.Add(new Cliente { Nome = "João" });
```

---

## 🎓 Resumo

✅ **Classes abstratas** combinam contrato + implementação  
✅ **Interfaces** definem apenas contratos  
✅ Use **abstract** para famílias de classes relacionadas  
✅ Use **interface** para contratos flexíveis  
✅ **Múltiplas interfaces** permitem maior flexibilidade  

➡️ **Próximo**: SOLID Principles (ISP e DIP)
