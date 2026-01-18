# 🧬 Herança em C#

## 🎯 Conceito Básico

Herança permite criar classes derivadas que **herdam** características de uma classe base.

```csharp
public class Animal
{
    public string Nome { get; set; }
    public void Respirar() => Console.WriteLine("Respirando...");
}

public class Cachorro : Animal  // ← Herança
{
    public void Latir() => Console.WriteLine("Au au!");
}

// Uso
var dog = new Cachorro { Nome = "Rex" };
dog.Respirar();  // Método herdado
dog.Latir();     // Método próprio
```

## 🔑 Constructor Base

```csharp
public class Animal
{
    public string Nome { get; set; }
    public Animal(string nome) => Nome = nome;
}

public class Cachorro : Animal
{
    public Cachorro(string nome, string raca) : base(nome)  // ← Chama base
    {
        Raca = raca;
    }
    public string Raca { get; set; }
}
```

## 🔄 Virtual e Override

```csharp
public class Animal
{
    public virtual void EmitirSom() => Console.WriteLine("Som genérico");
}

public class Cachorro : Animal
{
    public override void EmitirSom() => Console.WriteLine("Au au!");
}

// Polimorfismo
Animal animal = new Cachorro();
animal.EmitirSom();  // "Au au!"
```

## 🔒 Modificadores de Acesso

- `private`: Só na própria classe
- `protected`: Classe + derivadas
- `public`: Todos acessam

## 🤔 Quando Usar?

✅ Relação "É UM" (Dog is an Animal)  
❌ Relação "TEM UM" (use composição)

**Próximo**: Polimorfismo!</content>
<parameter name="filePath">/Users/lucasfranco/development/curso-csharp/Curso-Csharp/dia-03/01-heranca/01-conteudo.md
