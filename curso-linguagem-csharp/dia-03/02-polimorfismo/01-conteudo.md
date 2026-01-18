# 🔄 Polimorfismo em C#

> **Tempo estimado**: 2 horas  
> **Nível**: Intermediário

## 🎯 O que é Polimorfismo?

**Polimorfismo** significa "muitas formas". É a capacidade de um objeto assumir múltiplas formas e responder de maneiras diferentes dependendo do seu tipo real.

### Tipos de Polimorfismo em C#

1. **Polimorfismo de Compilação** (Compile-time)
   - Method Overloading
   - Operator Overloading

2. **Polimorfismo de Execução** (Runtime) ← **FOCO DESTE TÓPICO**
   - Method Overriding (virtual/override)
   - Interfaces

---

## 🚀 Polimorfismo em Ação

### Exemplo Clássico

```csharp
// Classe base
public class Animal
{
    public virtual void FazerSom()
    {
        Console.WriteLine("Animal fazendo som...");
    }
}

// Classes derivadas
public class Cachorro : Animal
{
    public override void FazerSom()
    {
        Console.WriteLine("Au au!");
    }
}

public class Gato : Animal
{
    public override void FazerSom()
    {
        Console.WriteLine("Miau!");
    }
}

// ===== POLIMORFISMO =====
Animal animal1 = new Cachorro(); // ← Polimorfismo!
Animal animal2 = new Gato();     // ← Polimorfismo!

animal1.FazerSom(); // "Au au!" ← Chama versão do Cachorro
animal2.FazerSom(); // "Miau!"  ← Chama versão do Gato
```

**O que aconteceu?**
- Variável do tipo `Animal` (base)
- Objeto é `Cachorro` ou `Gato` (derivados)
- Método correto é chamado em **tempo de execução**

---

## 🔼 Upcasting (Conversão Implícita)

```csharp
// Upcasting: Derivada → Base (SEMPRE seguro)
Cachorro cachorro = new Cachorro();
Animal animal = cachorro; // ← Upcasting automático ✅

// Funciona porque "Cachorro É UM Animal"
```

---

## 🔽 Downcasting (Conversão Explícita)

```csharp
// Downcasting: Base → Derivada (Pode falhar!)
Animal animal = new Cachorro();

// ❌ PERIGOSO: pode lançar InvalidCastException
Cachorro cachorro = (Cachorro)animal; // Cast explícito

// ✅ SEGURO: Verificar antes
if (animal is Cachorro)
{
    Cachorro c = (Cachorro)animal;
    // Usar c...
}

// ✅ AINDA MELHOR: Operator 'as'
Cachorro c = animal as Cachorro;
if (c != null)
{
    // Usar c...
}

// ✅ MODERNO: Pattern matching (C# 7+)
if (animal is Cachorro cachorro2)
{
    // cachorro2 já está convertido!
}
```

---

## 🔍 Operators: `is` e `as`

### Operator `is`

```csharp
Animal animal = new Cachorro();

// Verificar tipo
if (animal is Cachorro)
{
    Console.WriteLine("É um cachorro!");
}

// Pattern matching com declaração (C# 7+)
if (animal is Cachorro c)
{
    c.Latir(); // c já está convertido
}

// Switch com pattern matching (C# 8+)
string descricao = animal switch
{
    Cachorro c => $"Cachorro: {c.Nome}",
    Gato g => $"Gato: {g.Nome}",
    _ => "Animal desconhecido"
};
```

### Operator `as`

```csharp
Animal animal = new Cachorro();

// Tentar converter, retorna null se falhar
Cachorro cachorro = animal as Cachorro;
if (cachorro != null)
{
    cachorro.Latir();
}

// Ou mais conciso
Cachorro c = animal as Cachorro;
c?.Latir(); // Null-conditional operator
```

---

## 📊 Exemplo Completo: Sistema de Formas

```csharp
public abstract class Forma
{
    public string Cor { get; set; }
    
    public abstract double CalcularArea();
    public abstract double CalcularPerimetro();
    
    public virtual void Desenhar()
    {
        Console.WriteLine($"Desenhando forma {Cor}...");
    }
}

public class Circulo : Forma
{
    public double Raio { get; set; }
    
    public override double CalcularArea()
    {
        return Math.PI * Raio * Raio;
    }
    
    public override double CalcularPerimetro()
    {
        return 2 * Math.PI * Raio;
    }
    
    public override void Desenhar()
    {
        base.Desenhar();
        Console.WriteLine($"Círculo com raio {Raio}");
    }
}

public class Retangulo : Forma
{
    public double Largura { get; set; }
    public double Altura { get; set; }
    
    public override double CalcularArea()
    {
        return Largura * Altura;
    }
    
    public override double CalcularPerimetro()
    {
        return 2 * (Largura + Altura);
    }
    
    public override void Desenhar()
    {
        base.Desenhar();
        Console.WriteLine($"Retângulo {Largura}x{Altura}");
    }
}

// ===== USO POLIMÓRFICO =====
List<Forma> formas = new()
{
    new Circulo { Cor = "Vermelho", Raio = 5 },
    new Retangulo { Cor = "Azul", Largura = 10, Altura = 20 },
    new Circulo { Cor = "Verde", Raio = 3 }
};

double areaTotal = 0;
foreach (Forma forma in formas)
{
    forma.Desenhar(); // Polimorfismo: cada uma chama sua versão
    areaTotal += forma.CalcularArea();
}

Console.WriteLine($"Área total: {areaTotal:F2}");
```

---

## 🎯 Benefícios do Polimorfismo

✅ **Flexibilidade**: Adicione novos tipos sem mudar código existente  
✅ **Extensibilidade**: Open/Closed Principle (SOLID)  
✅ **Manutenibilidade**: Menos duplicação de código  
✅ **Testabilidade**: Mock de interfaces facilmente  

---

## 🎓 Resumo

✅ Polimorfismo permite tratar objetos diferentes através de interface comum  
✅ Upcasting é automático e sempre seguro  
✅ Downcasting requer verificação (`is`, `as`)  
✅ Pattern matching moderno é mais elegante  
✅ Base para design patterns e SOLID  

➡️ **Próximo**: Classes Abstratas e Interfaces
